namespace UFIDA.U9.LH.LHPubBP.AutoProgramBP
{
	using System;
	using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Text;
    using System.Web.Script.Serialization;
    using UFIDA.U9.LH.LHPubBP.Model;
    using UFSoft.UBF.AopFrame;	
	using UFSoft.UBF.Util.Context;
    using UFSoft.UBF.Util.DataAccess;

    /// <summary>
    /// AutoCreateU9ARDoc partial 
    /// </summary>	
	public partial class AutoCreateU9ARDoc 
	{	
		internal BaseStrategy Select()
		{
			return new AutoCreateU9ARDocImpementStrategy();	
		}		
	}
	
	#region  implement strategy	
	/// <summary>
	/// Impement Implement
	/// 
	/// </summary>	
	internal partial class AutoCreateU9ARDocImpementStrategy : BaseStrategy
	{
		public AutoCreateU9ARDocImpementStrategy() { }

		public override object Do(object obj)
		{						
			AutoCreateU9ARDoc bpObj = (AutoCreateU9ARDoc)obj;

            string dataJson = string.Empty;
            string posturl = string.Empty;
            try
            {
                string sql = string.Empty;
                DataSet ds = new DataSet();
                sql = string.Format("Select top 10  ID,DocType,Json  from Cust_FailInfo where issuccess=0 and  DocType='应收单创建' order by CreatedOn");
                DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null, out ds);
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables.Count >= 1 && ds.Tables[0].Rows.Count > 0))
                {
                    string apirEEEeuslt = string.Empty;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        // posturl = CHelper.GetDefineValueUrl("CustParam", "01");
                        posturl = File.ReadAllText("C:\\\\Windows\\U9Interface.txt", Encoding.UTF8);
                        dataJson = row["Json"].ToString();
                        apirEEEeuslt = UFIDA.U9.LH.LHPubBP.Tools.HttpPost(posturl, dataJson);
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        RtnDataJson rtns = serializer.Deserialize<RtnDataJson>(apirEEEeuslt);
                        if (rtns.IsSuccess)
                        {
                            sql = string.Format(@"update Cust_FailInfo set  issuccess=1  where ID={0}", Convert.ToInt64(row["ID"]));
                            DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                        }
                        else
                        {
                            rtns.Msg = rtns.Msg.Length > 100 ? rtns.Msg.Substring(0, 100) : rtns.Msg;
                            sql = string.Format(@"update Cust_FailInfo set  issuccess=2,Reason='{1}'  where ID={0}", Convert.ToInt64(row["ID"]), rtns.Msg);
                            DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                        }
                    }

                }


            }
            catch (Exception ex)
            {
                CHelper.InsertU9Log(false, "应收单创建", ex.Message, dataJson, posturl);
                throw new Exception(ex.Message);
            }
            return "OK";
        }		
	}

	#endregion
	
	
}