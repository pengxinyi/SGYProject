namespace UFIDA.U9.LH.LHPubBP.AutoProgramBP
{
	using System;
	using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Text;
    using System.Web.Script.Serialization;
    using UFIDA.U9.CBO.SCM.Warehouse;
    using UFIDA.U9.LH.LHPubBP.Model;
    using UFIDA.U9.PM.Rcv;
    using UFIDA.U9.SM.DealerSO;
    using UFSoft.UBF.AopFrame;	
	using UFSoft.UBF.Util.Context;
    using UFSoft.UBF.Util.DataAccess;

    /// <summary>
    /// AutoCreateU9PurRcvDoc partial 
    /// </summary>	
    public partial class AutoCreateU9PurRcvDoc 
	{	
		internal BaseStrategy Select()
		{
			return new AutoCreateU9PurRcvDocImpementStrategy();	
		}		
	}
	
	#region  implement strategy	
	/// <summary>
	/// Impement Implement
	/// 
	/// </summary>	
	internal partial class AutoCreateU9PurRcvDocImpementStrategy : BaseStrategy
	{
		public AutoCreateU9PurRcvDocImpementStrategy() { }

		public override object Do(object obj)
		{						
			AutoCreateU9PurRcvDoc bpObj = (AutoCreateU9PurRcvDoc)obj;

			string dataJson = string.Empty;
			string posturl = string.Empty;
			try
			{
				string sql = string.Empty;
				DataSet ds = new DataSet();
				sql = string.Format("Select top 100  ID,DocType,Json  from Cust_FailInfo where issuccess=0 and  DocType='委外收货创建' order by CreatedOn");
				DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null, out ds);
				if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables.Count >= 1 && ds.Tables[0].Rows.Count > 0))
				{
					string apirEEEeuslt = string.Empty;
					foreach (DataRow row in ds.Tables[0].Rows)
					{
                        //posturl = CHelper.GetDefineValueUrl("CustParam", "01");
                        posturl = File.ReadAllText("C:\\\\Windows\\U9Interface.txt", Encoding.UTF8);
                        dataJson = row["Json"].ToString();
						apirEEEeuslt = UFIDA.U9.LH.LHPubBP.Tools.HttpPost(posturl, dataJson);
						JavaScriptSerializer serializer = new JavaScriptSerializer();
						RtnDataJson rtns = serializer.Deserialize<RtnDataJson>(apirEEEeuslt);
						if (rtns.IsSuccess)
						{
                            sql = string.Format(@"update Cust_FailInfo set  issuccess=1,Reason=''  where ID={0}", Convert.ToInt64(row["ID"]));
                            DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                            Receivement rc = Receivement.Finder.FindByID(rtns.ID);
							if (rc != null)
							{
                                Warehouse WH = Warehouse.FindByCode(rc.Org, rc.RcvLines[0].Wh?.Code);
                                if (WH != null && WH.DescFlexField.PrivateDescSeg4 == "True")
                                {
                                    string dataJson1 = JsonHelper.ApprovePMRcv(rtns.DocNo, rc);
                                    // posturl = CHelper.GetDefineValueUrl("CustParam", "01"); //CustParam
                                    // posturl = File.ReadAllText("C:\\\\Windows\\U9Interface.txt", Encoding.UTF8);
                                    apirEEEeuslt = UFIDA.U9.LH.LHPubBP.Tools.HttpPost(posturl, dataJson1);
                                    RtnDataJson rtnss = serializer.Deserialize<RtnDataJson>(apirEEEeuslt);
                                }
                            }
                           
							
		 
						}
						else
						{
                            rtns.Msg = rtns.Msg.Length > 100 ? rtns.Msg.Substring(0, 100) : rtns.Msg;
                            sql = string.Format(@"update Cust_FailInfo set  Reason='{1}'  where ID={0}", Convert.ToInt64(row["ID"]), rtns.Msg);
							DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
						}
					}

				}


			}
			catch (Exception ex)
			{
				CHelper.InsertU9Log(false, "委外收货创建", ex.Message, dataJson, posturl);
				throw new Exception(ex.Message);
			}
			return "OK";
		}		
	}

	#endregion
	
	
}