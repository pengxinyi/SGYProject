namespace UFIDA.U9.LH.LHPubBP.AutoProgramBP
{
	using System;
	using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using System.Web.Script.Serialization;
    using UFIDA.U9.Base.Organization;
    using UFIDA.U9.Base.PropertyTypes;
    using UFIDA.U9.CBO.DTOs;
    using UFIDA.U9.CBO.SCM.PropertyTypes;
    using UFIDA.U9.CBO.SCM.Warehouse;
    using UFIDA.U9.LH.LHPubBP.Model;
    using UFIDA.U9.PM.Rcv;
    using UFIDA.U9.PM.Rcv.Proxy;
    using UFIDA.U9.PM.Util;
    using UFSoft.UBF.AopFrame;
    using UFSoft.UBF.Business;
    using UFSoft.UBF.PL;
    using UFSoft.UBF.Transactions;
    using UFSoft.UBF.Util.Context;
    using UFSoft.UBF.Util.DataAccess;
    using UFSoft.UBF.Util.Exceptions;

    /// <summary>
    /// AutoUpdateU9SaleReturnDoc partial 
    /// </summary>	
	public partial class AutoUpdateU9SaleReturnDoc
    {
        internal BaseStrategy Select()
        {
            return new AutoUpdateU9SaleReturnDocImpementStrategy();
        }
    }

    #region  implement strategy	
    /// <summary>
    /// Impement Implement
    /// 
    /// </summary>	
	internal partial class AutoUpdateU9SaleReturnDocImpementStrategy : BaseStrategy
	{
		public AutoUpdateU9SaleReturnDocImpementStrategy() { }

		public override object Do(object obj)
		{						
			AutoUpdateU9SaleReturnDoc bpObj = (AutoUpdateU9SaleReturnDoc)obj;

            string dataJson = string.Empty;
            try
            {
                string sql = string.Empty;
                DataSet ds = new DataSet();
                sql = string.Format("Select top 100  ID,DocType,Json,DocNo  from Cust_FailInfo where issuccess=0 and  DocType='采购退货更新' order by CreatedOn");
                DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null, out ds);
                if ((ds != null) && (ds.Tables.Count > 0) && (ds.Tables.Count >= 1 && ds.Tables[0].Rows.Count > 0))
                {
                    string apirEEEeuslt = string.Empty;
                    Receivement Rcv = null;
                    string ErrMsg = string.Empty;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        dataJson = row["Json"].ToString();
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        UpdateDocDto docDto = serializer.Deserialize<UpdateDocDto>(dataJson);
                        if (docDto != null)
                        {
                            Rcv = Receivement.Finder.FindByID(docDto.ID);
                            Organization org = Organization.FindByCode(docDto.OrgCode);
                            if (Rcv != null && Rcv.Status == RcvStatusEnum.Approving)
                            {
                                // List<SplitRcvLinesDTO> SplitRcvLines = new List<SplitRcvLinesDTO>();
                                //判断是否有残次品，进行拆行
                                //bool IsDefect = false;
                                //foreach (UpdateDocLineDto Deatail in docDto.DocLines)
                                //{
                                //    if (Deatail.defect)
                                //    {
                                //        IsDefect = true;
                                //    }
                                //}
                                //if (IsDefect)
                                //{
                                //    foreach (UpdateDocLineDto Deatail in docDto.DocLines)
                                //    {
                                //        RcvLine rcvline = RcvLine.Finder.Find("Receivement=@Receivement and ItemInfo.ItemCode=@ItemCode", new OqlParam("Receivement", Rcv.ID), new OqlParam("ItemCode", Deatail.ItemCode));
                                //        SplitRcvLinesDTO splitRcvLine = new SplitRcvLinesDTO();
                                //        splitRcvLine.RcvLine = rcvline;
                                //        splitRcvLine.NewQtyTU = new CBO.DTOs.DoubleQuantityData() { Amount1 = Deatail.Amount };
                                //        splitRcvLine.Defect = Deatail.defect;
                                //        SplitRcvLines.Add(splitRcvLine);
                                //    }


                                //    if (SplitRcvLines != null && SplitRcvLines.Count > 0)
                                //    {
                                //        SplitRcvLinesList(SplitRcvLines, org);
                                //    }
                                //}
                                //else
                                //{
                                Warehouse wh = null;
                                using (ISession session = Session.Open())
                                {
                                    Rcv.DescFlexField.PrivateDescSeg4 = "True";
                                    Rcv.Memo = docDto.Remark;
                                    foreach (RcvLine rcvline in Rcv.RcvLines)
                                    {
                                        foreach (UpdateDocLineDto Deatail in docDto.DocLines)
                                        {
                                            if (rcvline.ItemInfo.ItemCode == Deatail.ItemCode)
                                            {
                                                rcvline.Memo = Deatail.remark;
                                                if (rcvline.RtnFillQtyTU > 0)
                                                {
                                                    rcvline.RejectQtyCU = Deatail.Amount;
                                                    rcvline.RejectQtyPU = Deatail.Amount;
                                                    rcvline.RejectQtyTU = Deatail.Amount;
                                                    rcvline.RejectQtySU = Deatail.Amount;
                                                    rcvline.RtnFillQtyTU = Deatail.Amount;
                                                    rcvline.RtnFillQtyPU = Deatail.Amount;
                                                    rcvline.RtnFillQtySU = Deatail.Amount;
                                                    rcvline.RtnFillQtyCU = Deatail.Amount;
                                                   // rcvline.RejectMnyTC = Math.Round(Deatail.Amount * rcvline.FinallyPriceTC,2);
                                                }
                                                else if (rcvline.RtnDeductQtyTU > 0)
                                                {
                                                    rcvline.RejectQtyCU = Deatail.Amount;
                                                    rcvline.RejectQtyPU = Deatail.Amount;
                                                    rcvline.RejectQtyTU = Deatail.Amount;
                                                    rcvline.RejectQtySU = Deatail.Amount;
                                                    rcvline.RtnDeductQtyCU = Deatail.Amount;
                                                    rcvline.RtnDeductQtyPU = Deatail.Amount;
                                                    rcvline.RtnDeductQtySU = Deatail.Amount;
                                                    rcvline.RtnDeductQtyTU = Deatail.Amount;
                                                  //  rcvline.RejectMnyTC =Math.Round(Deatail.Amount * rcvline.FinallyPriceTC,2);
                                                }

                                                  wh = Warehouse.Finder.Find("Code=@Code and org=@org", new UFSoft.UBF.PL.OqlParam("Code", Deatail.WHCode), new UFSoft.UBF.PL.OqlParam("org", org?.ID));
                                                if (wh != null)
                                                {
                                                    rcvline.Wh = wh;
                                                    rcvline.Wh.ID = wh.ID;
                                                    rcvline.Wh.Code = wh.Code;
                                                }
                                                // rcvline.ConfirmDate = DateTime.Now.AddDays(0);
                                            }
                                            else if (rcvline.DescFlexSegments.PrivateDescSeg2 == Deatail.specno)
                                            {
                                                rcvline.Memo = Deatail.remark;
                                                if (rcvline.RtnFillQtyTU > 0)
                                                {
                                                    rcvline.RejectQtyCU = Deatail.Amount;
                                                    rcvline.RejectQtyPU = Deatail.Amount;
                                                    rcvline.RejectQtyTU = Deatail.Amount;
                                                    rcvline.RejectQtySU = Deatail.Amount;
                                                    rcvline.RtnFillQtyTU = Deatail.Amount;
                                                    rcvline.RtnFillQtyPU = Deatail.Amount;
                                                    rcvline.RtnFillQtySU = Deatail.Amount;
                                                    rcvline.RtnFillQtyCU = Deatail.Amount;
                                                   // rcvline.RejectMnyTC = Math.Round(Deatail.Amount * rcvline.FinallyPriceTC, 2);
                                                }
                                                else if (rcvline.RtnDeductQtyTU > 0)
                                                {
                                                    rcvline.RejectQtyCU = Deatail.Amount;
                                                    rcvline.RejectQtyPU = Deatail.Amount;
                                                    rcvline.RejectQtyTU = Deatail.Amount;
                                                    rcvline.RejectQtySU = Deatail.Amount;
                                                    rcvline.RtnDeductQtyCU = Deatail.Amount;
                                                    rcvline.RtnDeductQtyPU = Deatail.Amount;
                                                    rcvline.RtnDeductQtySU = Deatail.Amount;
                                                    rcvline.RtnDeductQtyTU = Deatail.Amount;
                                                  //  rcvline.RejectMnyTC = Math.Round(Deatail.Amount * rcvline.FinallyPriceTC, 2);
                                                }
                                                  wh = Warehouse.Finder.Find("Code=@Code and org=@org", new UFSoft.UBF.PL.OqlParam("Code", Deatail.WHCode), new UFSoft.UBF.PL.OqlParam("org", org?.ID));
                                                if (wh != null)
                                                {
                                                    rcvline.Wh = wh;
                                                    rcvline.Wh.ID = wh.ID;
                                                    rcvline.Wh.Code = wh.Code;
                                                }
                                                //rcvline.ConfirmDate = DateTime.Now.AddDays(1);
                                            }
                                        }
                                    }
                                    session.Commit();
                                    //if (wh!=null && wh.DescFlexField.PrivateDescSeg4!="True")
                                    //{
                                    //    sql = string.Format(@"update Cust_FailInfo set  issuccess=1  where ID={0}", Convert.ToInt64(row["ID"]));
                                    //    DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                                    //}
                                    sql = string.Format(@"update Cust_FailInfo set  issuccess=1  where ID={0}", Convert.ToInt64(row["ID"]));
                                    DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                                }
                                // }

                                //if (wh != null && wh.DescFlexField.PrivateDescSeg4 == "True")
                                //{
                                //    try
                                //    {
                                //        List<long> lstID = new List<long>();
                                //        lstID.Add(Rcv.ID);
                                //        RcvBatchApprovedBPProxy appProxy = new RcvBatchApprovedBPProxy();
                                //        appProxy.ActType = 8;
                                //        appProxy.DocHeadIDs = lstID;

                                //        List<UFIDA.U9.PM.Rcv.PMDocErrListDTOData> appErrors = appProxy.Do();
                                //        if (appErrors.Count > 0 && !string.IsNullOrEmpty(appErrors[0].ErrMsg))
                                //        {
                                //            ErrMsg = appErrors[0].ErrMsg.Length > 50 ? appErrors[0].ErrMsg.Substring(0, 50) : appErrors[0].ErrMsg;
                                //            sql = string.Format(@"update Cust_FailInfo set  issuccess=2,Reason='{1}'  where ID={0}", Convert.ToInt64(row["ID"]), ErrMsg);
                                //            DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                                //            CHelper.InsertU9Log(false, "采购退货审核", appErrors[0].ErrMsg, Rcv.DocNo, "");
                                //        }
                                //        else
                                //        {
                                //            sql = string.Format(@"update Cust_FailInfo set  issuccess=1  where ID={0}", Convert.ToInt64(row["ID"]));
                                //            DataAccessor.RunSQL(DataAccessor.GetConn(), sql, null);
                                //        }
                                //        // scope.Commit();

                                //    }
                                //    catch (Exception ex)
                                //    {
                                //        // scope.Rollback();
                                //        CHelper.InsertU9Log(false, "采购退货审核", ex.Message, Rcv.DocNo, "");
                                //        throw new Exception(ex.Message);
                                //    }
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
                CHelper.InsertU9Log(false, "采购退货更新", ex.Message, dataJson, "");
                throw new Exception(ex.Message);
            }
            
            return "OK";
        }

        public void SplitRcvLinesList(List<SplitRcvLinesDTO> SplitRcvLines, Organization org)
        {
            List<SplitRcvLinesDTO> list = new List<SplitRcvLinesDTO>(SplitRcvLines.Count);
            Dictionary<long, List<SplitRcvLinesDTO>> dictionary = new Dictionary<long, List<SplitRcvLinesDTO>>();
            List<long> list2 = new List<long>();
            using (UBFTransactionScope scope = new UBFTransactionScope(TransactionOption.Required))
            {
                using (Session session = Session.Open())
                {
                    foreach (SplitRcvLinesDTO splitRcvLineDTO in SplitRcvLines)
                    {
                        if (!(splitRcvLineDTO.RcvLine == null))
                        {
                            long id = splitRcvLineDTO.RcvLine.ID;
                            if (!list2.Contains(id))
                            {
                                list2.Add(id);
                            }
                            if (!dictionary.ContainsKey(splitRcvLineDTO.RcvLine.ID))
                            {
                                dictionary.Add(id, new List<SplitRcvLinesDTO>());
                            }
                            dictionary[id].Add(splitRcvLineDTO);
                        }
                    }
                    //string text = "RcvLineID__IDList";
                    //string opath = string.Format(" ID in (@{0}) ", text);
                    //OqlParam[] oqlParameters = new OqlParam[]
                    //{
                    //    new OqlParam(text, UBF.UMC.SysUtils.ToXml(UBF.UMC.SysUtils.CreateIDTable(list2)))
                    //};
                    //RcvLine.EntityList entityList = RcvLine.Finder.FindAll(opath, oqlParameters);
                    foreach (long num in dictionary.Keys)
                    {
                        list.AddRange(this.SplitRcvLine(num, dictionary[num], org));
                    }
                    session.Commit();
                }
                scope.Commit();
            }

        }


        private List<SplitRcvLinesDTO> SplitRcvLine(long rcvline, List<SplitRcvLinesDTO> list, Organization org)
        {
            List<SplitRcvLinesDTO> list2 = new List<SplitRcvLinesDTO>();
            RcvLine rcvLine = RcvLine.Finder.FindByID(rcvline);
            // rcvLine.InvLotCode = list[0].InvLotCode;
            // rcvLine.DescFlexSegments.PrivateDescSeg5 = "条码已推送";
            Receivement receivement = rcvLine.Receivement;
            rcvLine.ActivateType = UFIDA.U9.PM.Enums.ActivateTypeEnum.OBAUpdate;
            receivement.ActivateType = UFIDA.U9.PM.Enums.ActivateTypeEnum.OBAUpdate;
            if (rcvLine.SplitFlag != SplitFlag.NoSplit)
            {
                throw new ApplicationExceptionBase(string.Format(RcvSVRes.RcvSVRes_100003, receivement.DocNo, rcvLine.DocLineNo))
                {
                    EntityID = rcvLine.ID,
                    EntityFullName = rcvLine.Key.EntityType
                };
            }
            for (int i = 0; i < list.Count; i++)
            {
                RcvLine rcvLine2;
                if (i == 0)
                {
                    rcvLine2 = rcvLine;
                }
                else
                {
                    RcvLine rcv = (RcvLine)rcvLine.Clone();
                    rcvLine2 = RcvLine.Create(receivement);
                    rcvLine2.ActivateType = UFIDA.U9.PM.Enums.ActivateTypeEnum.OBAUpdate;
                    rcv.CopyTo(rcvLine2);
                    // rcvLine2.SysState = UFSoft.UBF.PL.Engine.ObjectState.Inserted;
                    rcvLine2.DocLineNo = rcvLine2.GetNextLineNo();


                    //rcvLine2.DescFlexSegments.PrivateDescSeg5 = "条码已推送";
                    this.DealSubLineInfo(rcvLine2, rcvLine);
                }
                //rcvLine2.ActivateType = ActivateTypeEnum.SplitRcvLine;
                this.DealRcvLineQty(rcvLine2, rcvLine, list[i]);
                rcvLine2.StorageType = list[i].Defect ? CBO.Enums.StorageTypeEnum.Reject : CBO.Enums.StorageTypeEnum.Useable;
                rcvLine2.SetPriceRelativeAttrFromPriceList();
                SrcDocInfo srcDocInfo = new SrcDocInfo();
                srcDocInfo.SrcDoc = new BizEntityKey(receivement.ID, receivement.Key.EntityType);
                srcDocInfo.SrcDocDate = receivement.BusinessDate;
                srcDocInfo.SrcDocLine = new BizEntityKey(rcvLine2.ID, rcvLine2.Key.EntityType);
                srcDocInfo.SrcDocLineNo = rcvLine2.DocLineNo;
                srcDocInfo.SrcDocNo = receivement.DocNo;
                srcDocInfo.SrcDocOrg = rcvLine2.CurrentOrg;
                srcDocInfo.SrcDocVer = (int)rcvLine2.SysVersion;
                list[i].RcvInfo = srcDocInfo;
                list2.Add(list[i]);
            }
            return list2;
        }

        private void DealRcvLineQty(RcvLine tmpLine, RcvLine line, SplitRcvLinesDTO splitRcvLineDTO)
        {
            bool isDualQuantity = line.ItemInfo.ItemID.IsDualQuantity;
            DoubleQuantity doubleQuantity = new DoubleQuantity(0m, 0m, tmpLine.Qty1UOMInfoDTO, tmpLine.Qty2UOMInfoDTO);
            decimal num = 0m;
            bool flag = false;
            if (tmpLine.ProcessItem != null && tmpLine.ProcessItem.ItemIDKey != null)
            {
                flag = true;
            }
            if (flag)
            {
                num = tmpLine.ProcessUOM.Round.GetRoundValue(splitRcvLineDTO.NewQtyTU.Amount1);
                doubleQuantity = PMCommonHelper.FromBaseAmount(num * tmpLine.DosagePerPiece * tmpLine.TUToTBURate, tmpLine.Qty1UOMInfoDTO, tmpLine.Qty2UOMInfoDTO);
                if (isDualQuantity)
                {
                    doubleQuantity = PMCommonHelper.FromBaseAmount(num * tmpLine.DosagePerPiece * tmpLine.TUToTBURate, tmpLine.Qty1UOMInfoDTO, tmpLine.Qty2UOMInfoDTO);
                }
                else
                {
                    doubleQuantity.Amount1 = num * tmpLine.DosagePerPiece;
                }
            }
            else if (isDualQuantity)
            {
                doubleQuantity = PMCommonHelper.FromBaseAmount(splitRcvLineDTO.NewQtyTU.Amount1, tmpLine.Qty1UOMInfoDTO, tmpLine.Qty2UOMInfoDTO);
            }
            else
            {
                doubleQuantity.Amount1 = splitRcvLineDTO.NewQtyTU.Amount1;
            }
            decimal roundValue;
            if (isDualQuantity)
            {
                roundValue = tmpLine.TBSubUOM.Round.GetRoundValue(doubleQuantity.Amount1 * tmpLine.TUToTBURate + doubleQuantity.Amount2);
            }
            else
            {
                roundValue = tmpLine.TradeUOM.Round.GetRoundValue(doubleQuantity.Amount1);
            }
            decimal roundValue2 = tmpLine.PriceUOM.Round.GetRoundValue(roundValue * tmpLine.TBUToPBURate);
            decimal roundValue3;
            if (flag)
            {
                roundValue3 = tmpLine.StoreUOM.Round.GetRoundValue(num * tmpLine.TBUToPBURate);
            }
            else
            {
                roundValue3 = tmpLine.StoreUOM.Round.GetRoundValue(roundValue * tmpLine.TBUToPBURate);
            }
            decimal roundValue4 = tmpLine.CostUOM.Round.GetRoundValue(roundValue3 * tmpLine.TBUToPBURate);
            //tmpLine.QCBill = splitRcvLineDTO.QCBill;
            //tmpLine.QCBillNo = splitRcvLineDTO.QCBillNo;
            tmpLine.EyeballingQtyTU = doubleQuantity.Amount1;
            tmpLine.EyeballingQtyTBU = doubleQuantity.Amount2;
            tmpLine.ProcessEyeballingQty = num;
            tmpLine.EyeballingQtyPU = roundValue2;
            tmpLine.EyeballingQtyCU = roundValue4;
            tmpLine.EyeballingQtySU = roundValue3;
            ClearRcvLineInfo(tmpLine);
            //SplitRcvLineSVImpementStrategy.ClearRcvLineInfo(tmpLine);
            //tmpLine.ArriveQtyTU = tmpLine.EyeballingQtyTU + tmpLine.RejectQtyTU;
            //tmpLine.ArriveQtyTBU = tmpLine.EyeballingQtyTBU + tmpLine.RejectQtyTBU;
            //tmpLine.ArriveQtyPU = tmpLine.EyeballingQtyPU + tmpLine.RejectQtyPU;
            //tmpLine.ArriveQtySU = tmpLine.EyeballingQtySU + tmpLine.RejectQtySU;
            //tmpLine.ArriveQtyCU = tmpLine.EyeballingQtyCU + tmpLine.RejectQtyCU;
            //tmpLine.ProcessArriveQty = tmpLine.ProcessEyeballingQty + tmpLine.ProcessRejectQty;
            //tmpLine.PlanQtyTU = tmpLine.ArriveQtyTU;
            //tmpLine.PlanQtyTBU = tmpLine.ArriveQtyTBU;
            //tmpLine.PlanQtyPU = tmpLine.ArriveQtyPU;
            //tmpLine.PlanQtySU = tmpLine.ArriveQtySU;
            //tmpLine.PlanQtyCU = tmpLine.ArriveQtyCU;
            if (line.RtnFillQtyTU > 0)
            {
                tmpLine.RejectQtyCU = doubleQuantity.Amount1;
                tmpLine.RejectQtyPU = doubleQuantity.Amount1;
                tmpLine.RejectQtyTU = doubleQuantity.Amount1;
                tmpLine.RejectQtySU = doubleQuantity.Amount1;
                tmpLine.RtnFillQtyTU = doubleQuantity.Amount1;
                tmpLine.RtnFillQtyPU = doubleQuantity.Amount1;
                tmpLine.RtnFillQtySU = doubleQuantity.Amount1;
                tmpLine.RtnFillQtyCU = doubleQuantity.Amount1;
                tmpLine.RejectMnyTC = doubleQuantity.Amount1 * line.FinallyPriceTC;
            }
            else if (line.RtnDeductQtyTU > 0)
            {
                tmpLine.RejectQtyCU = doubleQuantity.Amount1;
                tmpLine.RejectQtyPU = doubleQuantity.Amount1;
                tmpLine.RejectQtyTU = doubleQuantity.Amount1;
                tmpLine.RejectQtySU = doubleQuantity.Amount1;
                tmpLine.RtnDeductQtyCU = doubleQuantity.Amount1;
                tmpLine.RtnDeductQtyPU = doubleQuantity.Amount1;
                tmpLine.RtnDeductQtySU = doubleQuantity.Amount1;
                tmpLine.RtnDeductQtyTU = doubleQuantity.Amount1;
                tmpLine.RejectMnyTC = doubleQuantity.Amount1 * line.FinallyPriceTC;
            }
            // tmpLine.ProcessPlanQty = tmpLine.ProcessArriveQty;
            tmpLine.SetPriceRelativeAttrFromPriceList();
        }

        private void DealSubLineInfo(RcvLine tmpLine, RcvLine line)
        {
            if (line.ID != tmpLine.ID)
            {
                if (line.RcvAddress != null && line.RcvAddress.Count > 0)
                {
                    foreach (RcvAddress rcvAddress in line.RcvAddress)
                    {
                        RcvAddress rcvAddress2 = RcvAddress.Create(tmpLine);
                        rcvAddress.CopyTo(rcvAddress2);
                        rcvAddress.SysState = UFSoft.UBF.PL.Engine.ObjectState.Inserted;
                    }
                }
                if (line.RcvContacts != null && line.RcvContacts.Count > 0)
                {
                    foreach (RcvContact rcvContact in line.RcvContacts)
                    {
                        RcvContact rcvContact2 = RcvContact.Create(tmpLine);
                        rcvContact.CopyTo(rcvContact2);
                        rcvContact.SysState = UFSoft.UBF.PL.Engine.ObjectState.Inserted;
                    }
                }
            }
        }

        public void ClearRcvLineInfo(RcvLine tmpLine)
        {
            if (tmpLine.SysState == UFSoft.UBF.PL.Engine.ObjectState.Inserted)
            {
                tmpLine.RejectQtyCU = 0m;
                tmpLine.RejectQtyPU = 0m;
                tmpLine.RejectQtySU = 0m;
                tmpLine.RejectQtyTU = 0m;
                tmpLine.RejectQtyTBU = 0m;
                tmpLine.RtnFillQtyCU = 0m;
                tmpLine.RtnFillQtyPU = 0m;
                tmpLine.RtnFillQtySU = 0m;
                tmpLine.RtnFillQtyTU = 0m;
                tmpLine.RtnFillQtyTBU = 0m;
                tmpLine.RtnDeductQtyCU = 0m;
                tmpLine.RtnDeductQtyPU = 0m;
                tmpLine.RtnDeductQtySU = 0m;
                tmpLine.RtnDeductQtyTU = 0m;
                tmpLine.RtnDeductQtyTBU = 0m;
                tmpLine.ProcessReFillQty = 0m;
                tmpLine.ProcessRejectQty = 0m;
                tmpLine.ProcessReDeductQty = 0m;
                tmpLine.DeliverCheckQtyCU = 0m;
                tmpLine.DeliverCheckQtyPU = 0m;
                tmpLine.DeliverCheckQtySU = 0m;
                tmpLine.DeliverCheckQtyTU = 0m;
                tmpLine.DeliverCheckQtyTBU = 0m;
            }
            tmpLine.RcvQtyCU = 0m;
            tmpLine.RcvQtyPU = 0m;
            tmpLine.RcvQtySU = 0m;
            tmpLine.RcvQtyTU = 0m;
            tmpLine.RcvQtyTBU = 0m;
            tmpLine.ProcessRcvQty = 0m;
            tmpLine.QualifiedQtyCU = 0m;
            tmpLine.QualifiedQtyPU = 0m;
            tmpLine.QualifiedQtySU = 0m;
            tmpLine.QualifiedQtyTU = 0m;
            tmpLine.QualifiedQtyTBU = 0m;
            tmpLine.UnqualifiedQtyCU = 0m;
            tmpLine.UnqualifiedQtyPU = 0m;
            tmpLine.UnqualifiedQtySU = 0m;
            tmpLine.UnqualifiedQtyTU = 0m;
            tmpLine.UnqualifiedQtyTBU = 0m;
            tmpLine.ProcessQualifiedQty = 0m;
            tmpLine.ProcessUnqualifiedQty = 0m;
            tmpLine.CanAPConfirmQtyCU = 0m;
            tmpLine.CanAPConfirmQtyPU = 0m;
            tmpLine.CanAPConfirmQtySU = 0m;
            tmpLine.CanAPConfirmQtyTU = 0m;
            tmpLine.CanAPConfirmQtyTBU = 0m;
            tmpLine.DestroyQtyCU = 0m;
            tmpLine.DestroyQtyPU = 0m;
            tmpLine.DestroyQtySU = 0m;
            tmpLine.DestroyQtyTU = 0m;
            tmpLine.DestroyQtyTBU = 0m;
            tmpLine.ProcessDestroyQty = 0m;
            tmpLine.ArriveTotalMnyAC = 0m;
            tmpLine.ArriveTotalMnyTC = 0m;
            tmpLine.ArriveTotalMnyFC = 0m;
            tmpLine.ArriveTotalNetMnyAC = 0m;
            tmpLine.ArriveTotalNetMnyTC = 0m;
            tmpLine.ArriveTotalNetMnyFC = 0m;
            tmpLine.ArriveTotalTaxAC = 0m;
            tmpLine.ArriveTotalTaxTC = 0m;
            tmpLine.ArriveTotalTaxFC = 0m;
            tmpLine.TotalMnyAC = 0m;
            tmpLine.TotalMnyTC = 0m;
            tmpLine.TotalMnyFC = 0m;
            tmpLine.TotalNetMnyAC = 0m;
            tmpLine.TotalNetMnyTC = 0m;
            tmpLine.TotalNetMnyFC = 0m;
            tmpLine.TotalTaxAC = 0m;
            tmpLine.TotalTaxTC = 0m;
            tmpLine.TotalTaxFC = 0m;
            tmpLine.RtnDeductMnyAC = 0m;
            tmpLine.RtnDeductMnyTC = 0m;
            tmpLine.RtnDeductMnyFC = 0m;
            tmpLine.RtnDeductNetMnyAC = 0m;
            tmpLine.RtnDeductNetMnyTC = 0m;
            tmpLine.RtnDeductNetMnyFC = 0m;
            tmpLine.RtnDeductTaxAC = 0m;
            tmpLine.RtnDeductTaxTC = 0m;
            tmpLine.RtnDeductTaxFC = 0m;
            tmpLine.RejectMnyAC = 0m;
            tmpLine.RejectMnyTC = 0m;
            tmpLine.RejectMnyFC = 0m;
            tmpLine.RejectNetMnyAC = 0m;
            tmpLine.RejectNetMnyTC = 0m;
            tmpLine.RejectNetMnyFC = 0m;
            tmpLine.RejectTaxMnyAC = 0m;
            tmpLine.RejectTaxMnyTC = 0m;
            tmpLine.RejectTaxMnyFC = 0m;
            tmpLine.TotalNetFeeAC = 0m;
            tmpLine.TotalNetFeeTC = 0m;
            tmpLine.TotalNetFeeFC = 0m;
            tmpLine.PrePayBill = 0L;
            tmpLine.PrePayBillLine = 0L;
            tmpLine.PrePayBillNo = string.Empty;
            tmpLine.PrePayBillLineNo = 0;
            tmpLine.PaymentBill = 0L;
            tmpLine.PaymentBillLine = 0L;
            tmpLine.PaymentBillNo = string.Empty;
            tmpLine.PaymentBillLineNo = 0;
            tmpLine.PrePayQtyPU = 0m;
            tmpLine.PrePayQtyTU = 0m;
            tmpLine.PrePayQtyTBU = 0m;
            tmpLine.ExecPrePayMny = 0m;
            tmpLine.ExecPrePayQtyPU = 0m;
            tmpLine.ExecPrePayQtyTBU = 0m;
            tmpLine.ExecPrePayQtyTU = 0m;
            tmpLine.PrePayMnyAC = 0m;
            tmpLine.Weight = 0m;
            tmpLine.Volume = 0m;
            tmpLine.Piece = 0m;
        }
    }

	#endregion
	
	
}