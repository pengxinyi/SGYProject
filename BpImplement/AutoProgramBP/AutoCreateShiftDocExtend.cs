namespace UFIDA.U9.LH.LHPubBP.AutoProgramBP
{
    using Newtonsoft.Json;
    using System;
	using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Text;
    using System.Web.Script.Serialization;
    using UFIDA.U9.Base.Organization;
    using UFIDA.U9.LH.LHPubBP.Model;
    using UFIDA.U9.PM.Rtn;
    using UFIDA.U9.SM.Ship;
    using UFSoft.UBF.AopFrame;
    using UFSoft.UBF.Util.Context;
    using UFSoft.UBF.Util.DataAccess;

    /// <summary>
    /// 合并销售出库明细生成无来源出货
    /// </summary>	
    public partial class AutoCreateShiftDoc 
	{	
		internal BaseStrategy Select()
		{
			return new AutoCreateShiftDocImpementStrategy();	
		}		
	}
	
	#region  implement strategy	
	/// <summary>
	/// Impement Implement
	/// 
	/// </summary>	
	internal partial class AutoCreateShiftDocImpementStrategy : BaseStrategy
	{
		public AutoCreateShiftDocImpementStrategy() { }

		public override object Do(object obj)
		{						
			AutoCreateShiftDoc bpObj = (AutoCreateShiftDoc)obj;
            RtnDataJson rtn = new RtnDataJson();
            string json = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                DataSet dsq = new DataSet();
                List<string> SODocList = new List<string>();
                string dataJson = string.Empty;
                string  dataJson1 = string.Empty;
                string sql = string.Empty;
                string errormsg = string.Empty;
                string sodoc = string.Empty;
                //select distinct CusCode from sgy_saleship Where convert(varchar(10),ShipDate,120) = Cast(getdate() as Date)
                sql = string.Format("select distinct  CusCode,Org,convert(varchar(10), ShipDate,20) as ShipTime from sgy_saleship Where Status=0 and ShipDate<=DATEADD(MILLISECOND, -2, DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0))  and CusCode is not null");
                DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null, out ds);
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables.Count >= 1 && ds.Tables[0].Rows.Count > 0))
                {
                    string posturl = string.Empty;
                    string apirEEEeuslt = string.Empty;
                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    RtnDataJson rtns = null;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        sql = string.Format(" select CusCode,WHCode,ItemCode,SO,sum(Amount)数量,Org,sum(TotalMny) as  总支付金额 from sgy_saleship where Status=0  and ShipDate<=DATEADD(MILLISECOND, -2, DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0)) and CusCode ='{0}' and Org= {1} and convert(varchar(10), ShipDate,20)='{2}'  group by Org, CusCode,WHCode,ItemCode,SO", row["CusCode"].ToString(), Convert.ToInt64(row["Org"]), row["ShipTime"].ToString());

                        DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null, out dsq);
                        if ((dsq != null) && (dsq.Tables.Count > 0) && (dsq.Tables.Count >= 1 && dsq.Tables[0].Rows.Count > 0))
                        {
                            Organization org = Organization.Finder.FindByID(Convert.ToInt64(row["Org"]));
                            dataJson = JsonHelper.MergeShipJson(dsq, org, row["CusCode"].ToString(), row["ShipTime"].ToString());
                            json = dataJson;
                            //posturl = CHelper.GetDefineValueUrl("CustParam", "01");
                            posturl = File.ReadAllText("C:\\\\Windows\\U9Interface.txt", Encoding.UTF8);
                            apirEEEeuslt = UFIDA.U9.LH.LHPubBP.Tools.HttpPost(posturl, dataJson);
                            rtns = serializer.Deserialize<RtnDataJson>(apirEEEeuslt);
                            if (rtns.IsSuccess && !string.IsNullOrEmpty(rtns.DocNo))
                            {
                                if (dsq.Tables[0].Rows.Count <= 30)
                                {
                                    dataJson1 = JsonHelper.ApproveShipJson(org, rtns.DocNo);
                                    apirEEEeuslt = UFIDA.U9.LH.LHPubBP.Tools.HttpPost(posturl, dataJson1);
                                }


                                //CHelper.InsertU9Log(true, "合并销售明细生成出货单", rtns.Msg, dataJson, "", rtns.DocNo);
                                sql = string.Format(@"update sgy_saleship set Ship='{2}',Status=1,Memo=''  where status =0  and ShipDate<=DATEADD(MILLISECOND, -2, DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0))  and  CusCode='{0}' and Org={1} and convert(varchar(10), ShipDate,20)='{3}'", row["CusCode"].ToString(), Convert.ToInt64(row["Org"]), rtns.DocNo, row["ShipTime"].ToString());
                                DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                            }
                            else
                            {
                                CHelper.InsertU9Log(false, "合并销售明细生成出货单", rtns.Msg, dataJson, posturl, "");
                                errormsg += "生成出货单失败：" + rtns.Msg;
                                rtn.Msg = rtns.Msg.Length > 50 ? rtns.Msg.Substring(0, 50) : rtns.Msg;
                                sql = string.Format(@"update sgy_saleship set Memo='{2}'  where status=0 and ShipDate<=DATEADD(MILLISECOND, -2, DATEADD(DAY, DATEDIFF(DAY, 0, GETDATE()), 0)) and   CusCode='{0}' and Org={1} and convert(varchar(10), ShipDate,20)='{3}'", row["CusCode"].ToString(), Convert.ToInt64(row["Org"]), rtn.Msg, row["ShipTime"].ToString());
                                DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CHelper.InsertU9Log(false, "合并销售明细生成出货单", ex.Message, json, "", "");
                throw new Exception(ex.Message);
            }
            return "";
        }
    }

	#endregion
	
	
}