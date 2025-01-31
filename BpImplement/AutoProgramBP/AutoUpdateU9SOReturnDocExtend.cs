namespace UFIDA.U9.LH.LHPubBP.AutoProgramBP
{
	using System;
	using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Web.Script.Serialization;
    using UFIDA.U9.Base.Organization;
    using UFIDA.U9.CBO.SCM.Warehouse;
    using UFIDA.U9.LH.LHPubBP.Model;
    using UFIDA.U9.SM.RMA;
    using UFIDA.U9.SM.RMA.Proxy;
    using UFSoft.UBF.AopFrame;
    using UFSoft.UBF.Business;
    using UFSoft.UBF.Util.Context;
    using UFSoft.UBF.Util.DataAccess;

    /// <summary>
    /// AutoUpdateU9SOReturnDoc partial 
    /// </summary>	
	public partial class AutoUpdateU9SOReturnDoc
    {
        internal BaseStrategy Select()
        {
            return new AutoUpdateU9SOReturnDocImpementStrategy();
        }
    }

    #region  implement strategy	
    /// <summary>
    /// Impement Implement
    /// 
    /// </summary>	
	internal partial class AutoUpdateU9SOReturnDocImpementStrategy : BaseStrategy
	{
		public AutoUpdateU9SOReturnDocImpementStrategy() { }

		public override object Do(object obj)
		{						
			AutoUpdateU9SOReturnDoc bpObj = (AutoUpdateU9SOReturnDoc)obj;
            string dataJson = string.Empty;
            try
            {
                string sql = string.Empty;
                DataSet ds = new DataSet();
                sql = string.Format("Select top 100  ID,DocType,Json,DocNo  from Cust_FailInfo where issuccess=0 and  DocType='退回处理更新' order by CreatedOn");
                DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null, out ds);
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables.Count >= 1 && ds.Tables[0].Rows.Count > 0))
                {
                    string apirEEEeuslt = string.Empty;
                    RMA Rcv = null;
                    string ErrMsg = string.Empty;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        dataJson = row["Json"].ToString();
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        UpdateDocDto docDto = serializer.Deserialize<UpdateDocDto>(dataJson);
                        if (docDto != null)
                        {
                            Rcv = RMA.Finder.FindByID(docDto.ID);
                            Organization org = Organization.FindByCode(docDto.OrgCode);

                            if (Rcv != null && Rcv.Status == RMAStatusEnum.Posting)
                            {
                                using (ISession session = Session.Open())
                                {
                                    Rcv.DescFlexField.PrivateDescSeg7 = "True";
                                    Rcv.DescFlexField.PrivateDescSeg2 = docDto.RefundNo ;
                                    Rcv.DescFlexField.PrivateDescSeg8 = docDto.RKDocNo;
                                    Rcv.DescFlexField.PrivateDescSeg6 = docDto.Remark;
                                    foreach (RMALine rcvline in Rcv.RMALines)
                                    {
                                        foreach (UpdateDocLineDto Deatail in docDto.DocLines)
                                        {
                                            if (rcvline.ItemInfo.ItemCode == Deatail.ItemCode)
                                            {
                                                rcvline.ApplyQtyTU1 = Deatail.Amount;
                                                rcvline.ApplyQtyPU = Deatail.Amount;
                                                rcvline.RtnQtyTU1 = Deatail.Amount;
                                                rcvline.RtnQtyPU = Deatail.Amount;
                                                rcvline.ApplyQtyIU = Deatail.Amount;
                                                rcvline.RtnQtyIU = Deatail.Amount;
                                                rcvline.DescFlexField.PrivateDescSeg4 = Deatail.defect ? "2" : "1";
                                                rcvline.ApplyMoneyTC = Deatail.Amount * rcvline.ApplyPrice;
                                                Warehouse wh = Warehouse.Finder.Find("Code=@Code and org=@org", new UFSoft.UBF.PL.OqlParam("Code", Deatail.WHCode), new UFSoft.UBF.PL.OqlParam("org", org?.ID));
                                                if (wh != null)
                                                {
                                                    rcvline.Warehouse = wh;
                                                    rcvline.Warehouse.ID = wh.ID;
                                                    rcvline.Warehouse.Code = wh.Code;
                                                }
                                            }
                                            //else if (rcvline.DescFlexSegments.PrivateDescSeg1 == Deatail.specno)
                                            //{
                                            //    rcvline.RejectQtyTU = Deatail.Amount;
                                            //    rcvline.RtnFillQtyTU = Deatail.Amount;
                                            //    Warehouse wh = Warehouse.Finder.Find("Code=@Code and org=@org", new UFSoft.UBF.PL.OqlParam("Code", Deatail.WHCode), new UFSoft.UBF.PL.OqlParam("org", org?.ID));
                                            //    if (wh != null)
                                            //    {
                                            //        rcvline.Wh = wh;
                                            //        rcvline.Wh.ID = wh.ID;
                                            //        rcvline.Wh.Code = wh.Code;
                                            //    }
                                            //    rcvline.ConfirmDate = Rcv.BusinessDate;
                                            //}
                                        }
                                    }
                                    session.Commit();
                                    sql = string.Format(@"update Cust_FailInfo set  issuccess=1  where ID={0}", Convert.ToInt64(row["ID"]));
                                    DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                                }
                                    //try
                                    //{
                                    //new RmaStatusOperateBPProxy
                                    //{
                                    //    DocHead = new RMAUIVerDTOData()
                                    //    {
                                    //        RMAKey = Rcv.ID,
                                    //        UISysVersion = Rcv.SysVersion
                                    //    },
                                    //    OperateType = 3
                                    //}.Do();

                                    //sql = string.Format(@"update Cust_FailInfo set  issuccess=1  where ID={0}", Convert.ToInt64(row["ID"]));
                                    //DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);

                                    //}
                                    //catch (Exception ex)
                                    //{
                                    //sql = string.Format(@"update Cust_FailInfo set  issuccess=2,Reason='{1}'  where ID={0}", Convert.ToInt64(row["ID"]),ex.Message);
                                    //DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                                    //CHelper.InsertU9Log(false, "退回处理审核", ex.Message, Rcv.DocNo, "");
                                    
                                    //}
                            }
                            //Rcv = Receivement.Finder.FindByID(docDto.ID);
                            //if (Rcv!=null && Rcv.Status == RcvStatusEnum.Closed)
                            //{
                            //	sql = string.Format(@"update Cust_FailInfo set  issuccess=1  where ID={0}", Convert.ToInt64(row["ID"]));
                            //	DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                            //}
                            //else
                            //{
                            //	sql = string.Format(@"update Cust_FailInfo set  issuccess=2,Reason='{1}'  where ID={0}", Convert.ToInt64(row["ID"]), ErrMsg);
                            //	DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CHelper.InsertU9Log(false, "退回处理更新", ex.Message, dataJson, "");
                throw new Exception(ex.Message);
            }
            return "OK";
        }		
	}

	#endregion
	
	
}