namespace UFIDA.U9.LH.LHPubBP.AutoProgramBP
{
    using Newtonsoft.Json;
    using System;
	using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Text;
    using System.Web.Script.Serialization;
    using Top.Api.Domain;
    using UFIDA.U9.Base.Organization;
    using UFIDA.U9.LH.LHPubBP.Model;
    using UFIDA.U9.SM.Ship;
    using UFIDA.U9.SM.SO;
    using UFSoft.UBF.AopFrame;	
	using UFSoft.UBF.Util.Context;
    using UFSoft.UBF.Util.DataAccess;

    /// <summary>
    /// AutoCreateU9SrcShipDoc partial 
    /// </summary>	
    public partial class AutoCreateU9SrcShipDoc 
	{	
		internal BaseStrategy Select()
		{
			return new AutoCreateU9SrcShipDocImpementStrategy();	
		}		
	}
	
	#region  implement strategy	
	/// <summary>
	/// Impement Implement
	/// 
	/// </summary>	
	internal partial class AutoCreateU9SrcShipDocImpementStrategy : BaseStrategy
	{
		public AutoCreateU9SrcShipDocImpementStrategy() { }

		public override object Do(object obj)
		{						
			AutoCreateU9SrcShipDoc bpObj = (AutoCreateU9SrcShipDoc)obj;

			string dataJson = string.Empty;
			string posturl = string.Empty;
			try
			{
				string sql = string.Empty;
				DataSet ds = new DataSet();
				sql = string.Format("Select top 100  ID,DocType,Json,SrcDocNo,DocNo,Org  from Cust_FailInfo where issuccess=0 and  DocType='标准出货创建' order by CreatedOn desc");
				DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null, out ds);
				if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables.Count >= 1 && ds.Tables[0].Rows.Count > 0))
				{
					string apirEEEeuslt = string.Empty;
					string srcdocno = string.Empty;
                    string dataJson1 = string.Empty;
                    foreach (DataRow row in ds.Tables[0].Rows)
					{
						srcdocno = Convert.ToString(row["SrcDocNo"]);

                        if (!string.IsNullOrEmpty(srcdocno) && !srcdocno.StartsWith("SO9"))
						{
                            SO so = SO.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", row["SrcDocNo"].ToString()), new UFSoft.UBF.PL.OqlParam("Org", Convert.ToInt64(row["Org"])));
							if (so!=null)
							{
								Ship ship = Ship.Finder.Find("DocNo=@DocNo and Org=@Org", new UFSoft.UBF.PL.OqlParam("DocNo", row["DocNo"].ToString()), new UFSoft.UBF.PL.OqlParam("Org", Convert.ToInt64(row["Org"])));
								if (ship!=null)
								{
                                    sql = string.Format(@"update Cust_FailInfo set  issuccess=1  where ID={0}", Convert.ToInt64(row["ID"]));
                                    DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                                    continue;
								}
								Organization org = Organization.Finder.FindByID(Convert.ToInt64(row["Org"]));
                                MiscSalesOrderDto orderdto = JsonConvert.DeserializeObject<MiscSalesOrderDto>(row["Json"].ToString());
                                dataJson = JsonHelper.GetWDTShipFromSO(so, row["DocNo"].ToString(), org?.Code, orderdto);
                                // posturl = CHelper.GetDefineValueUrl("CustParam", "01"); //CustParam
                                posturl = File.ReadAllText("C:\\\\Windows\\U9Interface.txt", Encoding.UTF8);
                                apirEEEeuslt = UFIDA.U9.LH.LHPubBP.Tools.HttpPost(posturl, dataJson);
                                JavaScriptSerializer serializer = new JavaScriptSerializer();
                                RtnDataJson rtns = serializer.Deserialize<RtnDataJson>(apirEEEeuslt);
                                if (rtns.IsSuccess)
                                {
                                    sql = string.Format(@"update Cust_FailInfo set  issuccess=1  where ID={0}", Convert.ToInt64(row["ID"]));
                                    DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                                    if (!string.IsNullOrEmpty(rtns.DocNo) && !srcdocno.StartsWith("SO9"))
                                    {
                                        dataJson1 = JsonHelper.ApproveShipJson(org, rtns.DocNo);
                                        apirEEEeuslt = UFIDA.U9.LH.LHPubBP.Tools.HttpPost(posturl, dataJson1);
                                    }
                                }
                                else
                                {
                                    rtns.Msg = rtns.Msg.Length > 100 ? rtns.Msg.Substring(0, 100) : rtns.Msg;
                                    sql = string.Format(@"update Cust_FailInfo set  issuccess=2,Reason='{1}'  where ID={0}", Convert.ToInt64(row["ID"]), rtns.Msg);
                                    DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                                }
                            }
                        }
						else
						{                          
                            Organization org = Organization.Finder.FindByID(Convert.ToInt64(row["Org"]));
                            string DocNo = Convert.ToString(row["DocNo"]);
                            // posturl = CHelper.GetDefineValueUrl("CustParam", "01");
                            posturl = File.ReadAllText("C:\\\\Windows\\U9Interface.txt", Encoding.UTF8);
                            dataJson = row["Json"].ToString();
                            apirEEEeuslt = UFIDA.U9.LH.LHPubBP.Tools.HttpPost(posturl, dataJson);
                            JavaScriptSerializer serializer = new JavaScriptSerializer();
                            RtnDataJson rtns = serializer.Deserialize<RtnDataJson>(apirEEEeuslt);
                            if (rtns.IsSuccess)
                            {
                                if ((!string.IsNullOrEmpty(rtns.DocNo) && srcdocno.StartsWith("SO9")) || DocNo.Contains("-1"))
                                {
                                    dataJson1 = JsonHelper.ApproveShipJson(org, rtns.DocNo);
                                    apirEEEeuslt = UFIDA.U9.LH.LHPubBP.Tools.HttpPost(posturl, dataJson1);
                                }
                               
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


			}
			catch (Exception ex)
			{
				CHelper.InsertU9Log(false, "标准出货创建", ex.Message, dataJson, posturl);
				throw new Exception(ex.Message);
			}
			return "OK";
		}		
	}

	#endregion
	
	
}