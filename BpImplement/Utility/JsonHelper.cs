using Newtonsoft.Json;
using QimenCloud.Api.scene3ldsmu02o9.Response;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Top.Api.Domain;
using UFIDA.U9.Base.Contact;
using UFIDA.U9.Base.FlexField.ValueSet;
using UFIDA.U9.Base.Organization;
using UFIDA.U9.CBO.HR.Department;
using UFIDA.U9.CBO.HR.Operator;
using UFIDA.U9.CBO.SCM.Customer;
using UFIDA.U9.CBO.SCM.Customs;
using UFIDA.U9.CBO.SCM.Enums;
using UFIDA.U9.CBO.SCM.Item;
using UFIDA.U9.CBO.SCM.Supplier;
using UFIDA.U9.CBO.SCM.Warehouse;
using UFIDA.U9.InvDoc.MiscRcv;
using UFIDA.U9.InvDoc.MiscShip;
using UFIDA.U9.InvDoc.PrdEndCheck;
using UFIDA.U9.InvDoc.TransferForm;
using UFIDA.U9.InvDoc.TransferIn;
using UFIDA.U9.LH.LHPubBP.Model;
using UFIDA.U9.PM.CreatePMPPubSV;
using UFIDA.U9.PM.PO;
using UFIDA.U9.PM.Rcv;
using UFIDA.U9.PM.Rcv.Proxy;
using UFIDA.U9.PM.Rtn;
using UFIDA.U9.SM.DealerSO;
using UFIDA.U9.SM.RMA;
using UFIDA.U9.SM.Ship;
using UFIDA.U9.SM.ShipPlan;
using UFIDA.U9.SM.SO;
using UFIDA.U9.SM.SOForYDSV;
using UFIDA.U9.SPR.SalePriceList;
using UFSoft.UBF.PL;
using WebSocketSharp.Frame;
using static QimenCloud.Api.scene3ldsmu02o9.Response.WdtAftersalesRefundRefundSearchResponse;
using static QimenCloud.Api.scene3ldsmu02o9.Response.WdtWmsStockinRefundQuerywithdetailResponse;

namespace UFIDA.U9.LH.LHPubBP
{
    /// <summary>
    /// 组装调用WDT接口Json数据
    /// </summary>
    public class JsonHelper
    {
        /// <summary>
        /// 货品推送
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetSendDataJson(ItemMaster item)
        {
            List<object> list = new List<object>();

            GoodsReq goodsReq = new GoodsReq();
            goodsReq.goods_no = item.Code;
            goodsReq.goods_name = item.Name;


            if (item != null)
            {
                DefineValue dvs = DefineValue.Finder.Find("ValueSetDef.Code = @ZJCode and Code = @Code", new OqlParam("ZJCode", "1010"), new OqlParam("Code", item.DescFlexField.PrivateDescSeg1));
                if (dvs != null)
                {
                    goodsReq.class_name = dvs.Name;
                }
                // 8.24 从料品主分类取值集改为私有字段15
                DefineValue dv = DefineValue.Finder.Find("ValueSetDef.Code = @ZJCode and Code = @Code", new OqlParam("ZJCode", "1011"), new OqlParam("Code", item.DescFlexField.PrivateDescSeg15));
                if (dv != null)
                {
                    goodsReq.brand_name = dv.Name;
                }
                DefineValue dvq = DefineValue.Finder.Find("ValueSetDef.Code = @ZJCode and Code = @Code", new OqlParam("ZJCode", "1012"), new OqlParam("Code", item.DescFlexField.PrivateDescSeg4));
                if (dvq != null)
                {
                    goodsReq.prop3 = dvq.Name;
                }
                DefineValue dvi = DefineValue.Finder.Find("ValueSetDef.Code = @ZJCode and Code = @Code", new OqlParam("ZJCode", "1025"), new OqlParam("Code", item.DescFlexField.PrivateDescSeg5));
                if (dvi != null)
                {
                    goodsReq.prop5 = dvi.Name;
                }
                if (!string.IsNullOrEmpty(item.DescFlexField.PrivateDescSeg26))
                {
                    goodsReq.goods_type = Convert.ToInt32(item.DescFlexField.PrivateDescSeg26);
                }
            }
          
            goodsReq.unit_name = item.InventoryUOM?.Name;
            if (!string.IsNullOrEmpty(item.DescFlexField.PrivateDescSeg12))
            {
                int i = Convert.ToInt32(item.DescFlexField.PrivateDescSeg12);
                if (i > 0)
                {
                    goodsReq.aux_unit_name = i.ToString() + item.InventoryUOM?.Name + "/箱";
                }

            }
            goodsReq.short_name = item.DescFlexField.PrivateDescSeg16;
            goodsReq.remark = item.Description;
            goodsReq.prop1 = item.DescFlexField.PrivateDescSeg2;
            // goodsReq.prop2 = item.DescFlexField.PrivateDescSeg3 + "*" + item.DescFlexField.PrivateDescSeg18 + "*" + item.DescFlexField.PrivateDescSeg19;
            goodsReq.prop2 = item.Code2;
            goodsReq.prop6 = !string.IsNullOrEmpty(item.DescFlexField.PrivateDescSeg6)?Convert.ToDecimal(item.DescFlexField.PrivateDescSeg6).ToString("0.000"):"";
            if (item.ItemForm.Value == 2001)
            {
                goodsReq.prop4 = "正常";
            }
            else if (item.ItemForm.Value == 2002)
            {
                goodsReq.prop4 = "淘汰";
            }
            goodsReq.auto_create_bc = false;
            List<SpecInfoReq> listSpec = new List<SpecInfoReq>();
            SpecInfoReq specInfoReq = new SpecInfoReq();
            specInfoReq.spec_no = item.Code1;
            if (!string.IsNullOrEmpty(item.DescFlexField.PrivateDescSeg23))
            {
                specInfoReq.validity_days = Convert.ToInt32(item.DescFlexField.PrivateDescSeg23);
            }
           
            specInfoReq.barcode = item.DescFlexField.PrivateDescSeg7;
            specInfoReq.spec_name = item.SPECS;
           
            if (!string.IsNullOrEmpty(item.DescFlexField.PrivateDescSeg8))
            {
                specInfoReq.weight = Convert.ToDecimal(item.DescFlexField.PrivateDescSeg8);
            }
            if (!string.IsNullOrEmpty(item.DescFlexField.PrivateDescSeg11))
            {
                specInfoReq.height = Convert.ToDecimal(item.DescFlexField.PrivateDescSeg11);
            }
            if (!string.IsNullOrEmpty(item.DescFlexField.PrivateDescSeg9))
            {
                specInfoReq.length = Convert.ToDecimal(item.DescFlexField.PrivateDescSeg9);
            }
            if (!string.IsNullOrEmpty(item.DescFlexField.PrivateDescSeg10))
            {
                specInfoReq.width = Convert.ToDecimal(item.DescFlexField.PrivateDescSeg10);
            }
            if (!string.IsNullOrEmpty(item.DescFlexField.PrivateDescSeg12))
            {
                specInfoReq.spec_code = item.DescFlexField.PrivateDescSeg12;
            }
            if (!string.IsNullOrEmpty(item.DescFlexField.PrivateDescSeg13))
            {
                specInfoReq.retail_price = Convert.ToDecimal(item.DescFlexField.PrivateDescSeg13);
            }
            specInfoReq.prop1 = goodsReq.prop1;
            specInfoReq.prop2 = item.Code2;
            specInfoReq.prop3 = goodsReq.prop3;
            specInfoReq.prop4 = goodsReq.prop4;
            specInfoReq.prop5 = goodsReq.prop5;
            specInfoReq.prop6 = goodsReq.prop6;

           // specInfoReq.img_url = item.PictureRef;
            //specInfoReq.aux_unit_name = item.InventoryUOM.Name;
            if (!string.IsNullOrEmpty(item.DescFlexField.PrivateDescSeg12))
            {
                int i = Convert.ToInt32(item.DescFlexField.PrivateDescSeg12);
                if (i>0)
                {
                    specInfoReq.aux_unit_name = i.ToString() + item.InventoryUOM?.Name + "/箱";
                }
               
            }
           
            specInfoReq.unit_name = item.InventoryUOM?.Name;
            if (!string.IsNullOrEmpty(item.DescFlexField.PrivateDescSeg22))
            {
                specInfoReq.wholesale_price = Convert.ToDecimal(item.DescFlexField.PrivateDescSeg22);
            }
            listSpec.Add(specInfoReq);


            list.Add(goodsReq);
            list.Add(listSpec);
            return JsonConvert.SerializeObject(list);
        }
        /// <summary>
        /// 推送供应商-WDT
        /// </summary>
        /// <param name="supplier"></param>
        /// <returns></returns>
        public static string GetSupplierJson(Supplier supplier)
        {
            List<SupplierDto> supplierDtos = new List<SupplierDto>();
            SupplierDto m = new SupplierDto();
            m.provider_no = supplier.Code;
            m.provider_name = supplier.Name;
            //m.contact =  "";
            //m.telno = "";
            //m.mobile = "";
            //m.fax = "";
            //m.qq = "";
            //m.zip = "";
            //m.wangwang = "";
            //m.email = "";
            //m.website = "";
            //m.address = "";
            m.remark = "";
            m.is_disabled = supplier.Effective.IsEffective ? 0 : 1;
            //m.account_bank_no = "";
            //m.account_bank = "";
            //m.collect_name = "";
            //m.province = 440000;
            //m.city = 440100;
            //m.district = 440111;
            supplierDtos.Add(m);
            return JsonConvert.SerializeObject(supplierDtos);
        }
        /// <summary>
        /// 调拨入库
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetTransferJson(TransferIn transfer)
        {
            bool isCheck = false;
            List<object> list = new List<object>();
            TransferlnDto Transfer = new TransferlnDto();
            Transfer.src_order_no = transfer.DocNo;
            Transfer.warehouse_no = transfer.TransInLines != null ? transfer.TransInLines[0].TransInWh.Code : "";
            Transfer.remark = transfer.Memo;
            // Transfer.is_create_batch = transfer.TransInLines[0].LotInfo != null ? true : false;

            List<TransferlnLineDto> TransferLines = new List<TransferlnLineDto>();
            foreach (TransInLine item in transfer.TransInLines)
            {
                TransferlnLineDto TransferLine = new TransferlnLineDto();
                ItemMaster itemMaster = ItemMaster.Finder.FindByID(item.ItemInfo.ItemID.ID);
                if (itemMaster != null)
                {
                    TransferLine.spec_no = itemMaster.Code1;
                }

                TransferLine.num = item.StoreUOMQty;
                TransferLine.unit_name = item.StoreUOM != null ? item.StoreUOM.Name : "";
                TransferLine.batch_no = item.LotInfo != null ? item.LotInfo.LotCode : "";
                TransferLine.remark = item.DocLineNo.ToString();
                TransferLine.position_no = item.TransInBins != null && item.TransInBins[0].BinInfo != null && item.TransInBins[0].BinInfo.Bin != null ? item.TransInBins[0].BinInfo.Bin.Code : "";
                TransferLines.Add(TransferLine);
            }



            list.Add(Transfer);
            list.Add(TransferLines);
            list.Add(isCheck);
            return JsonConvert.SerializeObject(list);
        }
        /// <summary>
        /// 调入单 调入仓为线上仓推送其他入库业务单，调拨入库
        /// </summary>
        /// <param name="transfer"></param>
        /// <returns></returns>
        public static string GetStockOtherInTransferJson(TransferIn transfer)
        {
            List<object> list = new List<object>();

            StockInPushDto miscShipment = new StockInPushDto();
            miscShipment.outer_no = transfer.DocNo;
            miscShipment.warehouse_no = transfer.TransInLines != null ? transfer.TransInLines[0].TransInWh.Code : "";
            miscShipment.is_check = true;
            miscShipment.reason = "调拨入库";
            miscShipment.prop2 = transfer.CreatedBy;
            miscShipment.remark = transfer.Memo;
            List<StockInPushDetailDto> MiscShipLines = new List<StockInPushDetailDto>();
            foreach (TransInLine item in transfer.TransInLines)
            {
                StockInPushDetailDto MiscShipLine = new StockInPushDetailDto();
                ItemMaster itemMaster = ItemMaster.Finder.FindByID(item.ItemInfo.ItemID.ID);
                if (itemMaster != null && string.IsNullOrEmpty(item.DescFlexSegments.PrivateDescSeg4))
                {
                    MiscShipLine.spec_no = itemMaster.Code1;
                }
                else if (!string.IsNullOrEmpty(item.DescFlexSegments.PrivateDescSeg4))
                {
                    MiscShipLine.spec_no = item.DescFlexSegments.PrivateDescSeg4;
                }
                if (!string.IsNullOrEmpty(itemMaster.DescFlexField.PrivateDescSeg12))
                {
                    int i = Convert.ToInt32(itemMaster.DescFlexField.PrivateDescSeg12);
                    if (i > 0)
                    {
                        MiscShipLine.aux_unit_name = i.ToString() + itemMaster.InventoryUOM?.Name + "/箱";
                    }

                }
                MiscShipLine.num = Convert.ToInt32(item.StoreUOMQty);
                MiscShipLine.batch_no = item.LotInfo != null ? item.LotInfo.LotCode : "";
                MiscShipLine.remark = item.Memo;
                MiscShipLine.price = item.CostPrice;
                MiscShipLines.Add(MiscShipLine);
            }

            list.Add(miscShipment);
            list.Add(MiscShipLines);
            return JsonConvert.SerializeObject(list);
        }
       
     
        /// <summary>
        /// 调入单提交调入仓为pop推送外仓调整入库
        /// </summary>
        /// <param name="transfer"></param>
        /// <returns></returns>
        public static string GetStockOutInTransferJson(TransferIn transfer)
        {
            List<object> list = new List<object>();
            bool is_check = true;
            OutInCreateOrderDto miscShipment = new OutInCreateOrderDto();
            miscShipment.order_no = transfer.DocNo;
            miscShipment.warehouse_no = transfer.TransInLines != null ? transfer.TransInLines[0].TransInWh.Code : "";
            miscShipment.reason = "POP入库";
            miscShipment.src_order_type = 0;
            miscShipment.remark = transfer.CreatedBy;
            List<OutInCreateOrderDetailDto> MiscShipLines = new List<OutInCreateOrderDetailDto>();
            foreach (TransInLine item in transfer.TransInLines)
            {
                OutInCreateOrderDetailDto MiscShipLine = new OutInCreateOrderDetailDto();
                ItemMaster itemMaster = ItemMaster.Finder.FindByID(item.ItemInfo.ItemID.ID);
                if (itemMaster != null && string.IsNullOrEmpty(item.DescFlexSegments.PrivateDescSeg4))
                {
                    MiscShipLine.spec_no = itemMaster.Code1;
                }
                else if (!string.IsNullOrEmpty(item.DescFlexSegments.PrivateDescSeg4))
                {
                    MiscShipLine.spec_no = item.DescFlexSegments.PrivateDescSeg4;
                }
                if (!string.IsNullOrEmpty(itemMaster.DescFlexField.PrivateDescSeg12))
                {
                    int i = Convert.ToInt32(itemMaster.DescFlexField.PrivateDescSeg12);
                    if (i > 0)
                    {
                        MiscShipLine.aux_unit_name = i.ToString() + itemMaster.InventoryUOM?.Name + "/箱";
                    }

                }
                MiscShipLine.num = Convert.ToInt32(item.StoreUOMQty);
                MiscShipLine.remark = item.Memo;
                MiscShipLines.Add(MiscShipLine);
            }

            list.Add(miscShipment);
            list.Add(MiscShipLines);
            list.Add(is_check);
            return JsonConvert.SerializeObject(list);
        }
        /// <summary>
        /// 调入单提交调出仓为pop推送外仓调整出库
        /// </summary>
        /// <param name="transfer"></param>
        /// <returns></returns>
        public static string GetStockOutoutTransferJson(TransferIn transfer)
        {
            List<object> list = new List<object>();
            bool is_check = true;
            OutInCreateOrderDto miscShipment = new OutInCreateOrderDto();
            miscShipment.order_no = transfer.DocNo;
            miscShipment.warehouse_no = transfer.TransInLines != null && transfer.TransInLines[0].TransInSubLines[0] != null ? transfer.TransInLines[0].TransInSubLines[0].TransOutWh.Code : "";
            miscShipment.reason = "POP出库";
            miscShipment.src_order_type = 0;
            miscShipment.remark = transfer.CreatedBy;
            List<OutInCreateOrderDetailDto> MiscShipLines = new List<OutInCreateOrderDetailDto>();
            foreach (TransInLine item in transfer.TransInLines)
            {
                OutInCreateOrderDetailDto MiscShipLine = new OutInCreateOrderDetailDto();
                ItemMaster itemMaster = ItemMaster.Finder.FindByID(item.ItemInfo.ItemID.ID);
                if (itemMaster != null && string.IsNullOrEmpty(item.DescFlexSegments.PrivateDescSeg4))
                {
                    MiscShipLine.spec_no = itemMaster.Code1;
                }
                else if (!string.IsNullOrEmpty(item.DescFlexSegments.PrivateDescSeg4))
                {
                    MiscShipLine.spec_no = item.DescFlexSegments.PrivateDescSeg4;
                }
                if (!string.IsNullOrEmpty(itemMaster.DescFlexField.PrivateDescSeg12))
                {
                    int i = Convert.ToInt32(itemMaster.DescFlexField.PrivateDescSeg12);
                    if (i > 0)
                    {
                        MiscShipLine.aux_unit_name = i.ToString() + itemMaster.InventoryUOM?.Name + "/箱";
                    }

                }
                MiscShipLine.num = Convert.ToInt32(item.StoreUOMQty);
                MiscShipLine.remark = item.Memo;
                MiscShipLines.Add(MiscShipLine);
            }

            list.Add(miscShipment);
            list.Add(MiscShipLines);
            list.Add(is_check);
            return JsonConvert.SerializeObject(list);
        }
        /// <summary>
        /// 调拨单新建
        /// </summary>
        /// <param name="transfer"></param>
        /// <returns></returns>
        public static string GetStocktInTransferJson(TransferIn transfer)
        {
            bool isCheck = false;
            List<object> list = new List<object>();
            StockTransferlnDto Transfer = new StockTransferlnDto();
            Transfer.outer_no = transfer.DocNo;
            Transfer.from_warehouse_no = transfer.TransInLines != null ? transfer.TransInLines[0].TransInWh.Code : "";
            Transfer.to_warehouse_no = transfer.TransInLines != null && transfer.TransInLines[0].TransInSubLines[0] != null ? transfer.TransInLines[0].TransInSubLines[0].TransOutWh.Code : ""; ;
            // Transfer.is_create_batch = transfer.TransInLines[0].LotInfo != null ? true : false;
            Transfer.mode = 0;
            List<StockTransferlnLineDto> TransferLines = new List<StockTransferlnLineDto>();
            foreach (TransInLine item in transfer.TransInLines)
            {
                StockTransferlnLineDto TransferLine = new StockTransferlnLineDto();
                ItemMaster itemMaster = ItemMaster.Finder.FindByID(item.ItemInfo.ItemID.ID);
                if (itemMaster != null)
                {
                    TransferLine.spec_no = itemMaster.Code1;
                }

                TransferLine.num = item.StoreUOMQty;
                TransferLines.Add(TransferLine);
            }



            list.Add(Transfer);
            list.Add(TransferLines);
            list.Add(isCheck);
            return JsonConvert.SerializeObject(list);
        }
        /// <summary>
        /// 调拨出库
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetTransferOutJson(TransferIn transfer)
        {
            bool isCheck = false;
            List<object> list = new List<object>();


            TransferlnDto Transfer = new TransferlnDto();
            Transfer.src_order_no = transfer.DocNo;
            Transfer.warehouse_no = transfer.TransInLines != null && transfer.TransInLines[0].TransInSubLines[0] != null ? transfer.TransInLines[0].TransInSubLines[0].TransOutWh.Code : "";
            Transfer.remark = transfer.Memo;
            Transfer.logistics_code = transfer.DescFlexField.PrivateDescSeg1;
            // Transfer.is_create_batch = transfer.TransInLines[0].LotInfo != null ? true : false ;

            List<TransferlnLineDto> TransferLines = new List<TransferlnLineDto>();
            foreach (TransInLine item in transfer.TransInLines)
            {
                foreach (TransInSubLine SubLine in item.TransInSubLines)
                {
                    TransferlnLineDto TransferLine = new TransferlnLineDto();
                    ItemMaster itemMaster = ItemMaster.Finder.FindByID(SubLine.ItemInfo.ItemID.ID);
                    if (itemMaster != null)
                    {
                        TransferLine.spec_no = itemMaster.Code1;
                    }
                    TransferLine.num = SubLine.StoreUOMQty;
                    TransferLine.unit_name = SubLine.StoreUOM != null ? item.StoreUOM.Name : "";
                    TransferLine.batch_no = SubLine.LotInfo != null ? item.LotInfo.LotCode : "";
                    TransferLine.remark = SubLine.DocLineNo.ToString();
                    TransferLines.Add(TransferLine);
                }

            }



            list.Add(Transfer);
            list.Add(TransferLines);
            list.Add(isCheck);
            return JsonConvert.SerializeObject(list);
        }
        /// <summary>
        /// 调拨出库新方法
        /// </summary>
        /// <param name="transfer"></param>
        /// <returns></returns>
        public static string GetStockOutTransferOutJson(TransferIn transfer)
        {
            List<object> list = new List<object>();
            StockOutPushDto miscShipment = new StockOutPushDto();
            miscShipment.outer_no = transfer.DocNo;
            miscShipment.warehouse_no = transfer.TransInLines != null && transfer.TransInLines[0].TransInSubLines[0] != null ? transfer.TransInLines[0].TransInSubLines[0].TransOutWh.Code : "";
            Warehouse WH = Warehouse.FindByCode(transfer.Org, miscShipment.warehouse_no);
            if (WH != null && WH.DescFlexField != null)
            {
                miscShipment.receiver_name = WH.DescFlexField.PrivateDescSeg6;
                miscShipment.receiver_mobile = WH.DescFlexField.PrivateDescSeg7;
                miscShipment.receiver_province = WH.DescFlexField.PrivateDescSeg8;
                miscShipment.receiver_city = WH.DescFlexField.PrivateDescSeg9;
                miscShipment.receiver_district = WH.DescFlexField.PrivateDescSeg10;
                miscShipment.receiver_address = WH.DescFlexField.PrivateDescSeg11;
            }
         
            miscShipment.is_check = true;
            miscShipment.remark = transfer.TransInLines != null && transfer.TransInLines[0].TransInWh != null ? transfer.TransInLines[0].TransInWh.Name : null;
           // miscShipment.prop3 = transfer.TransInLines != null && transfer.TransInLines[0].TransInWh != null ? transfer.TransInLines[0].TransInWh.Name : null;
            miscShipment.reason = "调拨出库";
            miscShipment.prop2 = transfer.CreatedBy;
            miscShipment.prop1 = transfer.TransInLines != null && transfer.TransInLines[0].TransInWh != null ? transfer.TransInLines[0].TransInWh.Name : null;
            Customer cus = Customer.Finder.Find("Code=@Code and Org=@Org",new OqlParam(transfer.DescFlexField.PrivateDescSeg4), new OqlParam(transfer.Org.ID));
            miscShipment.prop3 = cus != null ? cus.Name : null;
            if (cus!=null)
            {
                if (cus.CustomerSites != null)
                {
                    foreach (CustomerSite site in cus.CustomerSites)
                    {
                        if (site.Code == transfer.DescFlexField.PrivateDescSeg3)
                        {
                            miscShipment.prop4 = site.DescFlexField.PrivateDescSeg6 + " " + site.DescFlexField.PrivateDescSeg7 + " " + site.DescFlexField.PrivateDescSeg8;
                            miscShipment.prop5 = site.DescFlexField.PrivateDescSeg3;
                        }
                    }

                }
            }
           
    
            List<StockOutPushDetailDto> MiscShipLines = new List<StockOutPushDetailDto>();


            foreach (TransInLine item in transfer.TransInLines)
            {
                foreach (TransInSubLine SubLine in item.TransInSubLines)
                {
                    StockOutPushDetailDto MiscShipLine = new StockOutPushDetailDto();
                    ItemMaster itemMaster = ItemMaster.Finder.FindByID(item.ItemInfo.ItemID.ID);
                    if (itemMaster != null && string.IsNullOrEmpty(item.DescFlexSegments.PrivateDescSeg4))
                    {
                        MiscShipLine.spec_no = itemMaster.Code1;
                    }
                    else if (!string.IsNullOrEmpty(item.DescFlexSegments.PrivateDescSeg4))
                    {
                        MiscShipLine.spec_no = item.DescFlexSegments.PrivateDescSeg4;
                    }
                    if (!string.IsNullOrEmpty(itemMaster.DescFlexField.PrivateDescSeg12))
                    {
                        int i = Convert.ToInt32(itemMaster.DescFlexField.PrivateDescSeg12);
                        if (i > 0)
                        {
                            MiscShipLine.aux_unit_name = i.ToString() + itemMaster.InventoryUOM?.Name + "/箱";
                        }

                    }
                    MiscShipLine.num = Convert.ToInt32(item.StoreUOMQty);
                    MiscShipLine.batch_no = item.LotInfo != null ? item.LotInfo.LotCode : "";
                    MiscShipLine.remark = item.Memo;
                    MiscShipLines.Add(MiscShipLine);
                }

            }
            list.Add(miscShipment);
            list.Add(MiscShipLines);
            return JsonConvert.SerializeObject(list);
        }
        /// <summary>
        /// 杂发单
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetMiscShipJson(MiscShipment misc)
        {
            List<object> list = new List<object>();

            List<MiscShipmentDto> miscShipments = new List<MiscShipmentDto>();
            MiscShipmentDto miscShipment = new MiscShipmentDto();
            miscShipment.outer_no = misc.DocNo;
            miscShipment.warehouse_no = misc.MiscShipLs[0].Wh != null ? misc.MiscShipLs[0].Wh.Code : "";
            miscShipment.is_check = false;
            miscShipment.remark = misc.Memo;
            miscShipment.reason = "普通其他出库";
            List<MiscShipLine> MiscShipLines = new List<MiscShipLine>();
            foreach (MiscShipmentL item in misc.MiscShipLs)
            {
                MiscShipLine MiscShipLine = new MiscShipLine();
                ItemMaster itemMaster = ItemMaster.Finder.FindByID(item.ItemInfo.ItemID.ID);
                if (itemMaster != null)
                {
                    MiscShipLine.spec_no = itemMaster.Code1;
                }

                MiscShipLine.num = item.StoreUOMQty;
                MiscShipLine.batch_no = item.LotInfo != null ? item.LotInfo.LotCode : "";
                MiscShipLine.remark = item.DocLineNo.ToString();
                MiscShipLines.Add(MiscShipLine);
            }
            miscShipment.goods_list = MiscShipLines;
            miscShipments.Add(miscShipment);
            return JsonConvert.SerializeObject(miscShipments);
        }
        /// <summary>
        /// 杂发单创建其他出库业务单据委外仓库 先到旺店通ERP 再到WMS
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetStockoutMiscShipJson(MiscShipment misc)
        {
            List<object> list = new List<object>();
            StockOutPushDto miscShipment = new StockOutPushDto();
            miscShipment.outer_no = misc.DocNo;
            miscShipment.warehouse_no = misc.MiscShipLs[0].Wh != null ? misc.MiscShipLs[0].Wh.Code : "";
            Warehouse WH = Warehouse.FindByCode(misc.Org, miscShipment.warehouse_no);
            if (WH != null && WH.DescFlexField != null)
            {
                miscShipment.receiver_name = WH.DescFlexField.PrivateDescSeg6;
                miscShipment.receiver_mobile = WH.DescFlexField.PrivateDescSeg7;
                miscShipment.receiver_province = WH.DescFlexField.PrivateDescSeg8;
                miscShipment.receiver_city = WH.DescFlexField.PrivateDescSeg9;
                miscShipment.receiver_district = WH.DescFlexField.PrivateDescSeg10;
                miscShipment.receiver_address = WH.DescFlexField.PrivateDescSeg11;
            }
            miscShipment.is_check = true;
            // miscShipment.remark = misc.CreatedBy;
            miscShipment.remark = misc.Memo;
            miscShipment.prop2 = misc.CreatedBy;
            miscShipment.reason = "普通其他出库";
            List<StockOutPushDetailDto> MiscShipLines = new List<StockOutPushDetailDto>();
            foreach (MiscShipmentL item in misc.MiscShipLs)
            {
                StockOutPushDetailDto MiscShipLine = new StockOutPushDetailDto();
                ItemMaster itemMaster = ItemMaster.Finder.FindByID(item.ItemInfo.ItemID.ID);
                if (itemMaster != null && string.IsNullOrEmpty(item.DescFlexSegments.PrivateDescSeg3))
                {
                    MiscShipLine.spec_no = itemMaster.Code1;
                }
                else if (!string.IsNullOrEmpty(item.DescFlexSegments.PrivateDescSeg3))
                {
                    MiscShipLine.spec_no = item.DescFlexSegments.PrivateDescSeg3;
                }
                if (!string.IsNullOrEmpty(itemMaster.DescFlexField.PrivateDescSeg12))
                {
                    int i = Convert.ToInt32(itemMaster.DescFlexField.PrivateDescSeg12);
                    if (i > 0)
                    {
                        MiscShipLine.aux_unit_name = i.ToString() + itemMaster.InventoryUOM?.Name + "/箱";
                    }

                }
                MiscShipLine.defect = item.StoreType.Value != 4 ? true : false;
                MiscShipLine.num = Convert.ToInt32(item.StoreUOMQty);
                MiscShipLine.batch_no = item.LotInfo != null ? item.LotInfo.LotCode : "";
                //MiscShipLine.remark = item.DocLineNo.ToString();
                MiscShipLine.remark = item.Memo;
                MiscShipLines.Add(MiscShipLine);
            }
            list.Add(miscShipment);
            list.Add(MiscShipLines);
            return JsonConvert.SerializeObject(list);
        }
        /// <summary>
        /// 杂收单创建其他入库单据内部仓库
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetMiscRcvTransJson(MiscRcvTrans misc)
        {
            List<object> list = new List<object>();

            List<MiscRcvTransDto> miscShipments = new List<MiscRcvTransDto>();
            MiscRcvTransDto miscShipment = new MiscRcvTransDto();
            miscShipment.outer_no = misc.DocNo;
            miscShipment.warehouse_no = misc.MiscRcvTransLs[0].Wh?.Code;
            miscShipment.is_check = false;
            miscShipment.reason = "普通其他入库";
            List<MiscRcvTranLine> MiscShipLines = new List<MiscRcvTranLine>();
            foreach (MiscRcvTransL item in misc.MiscRcvTransLs)
            {
                MiscRcvTranLine MiscShipLine = new MiscRcvTranLine();
                ItemMaster itemMaster = ItemMaster.Finder.FindByID(item.ItemInfo.ItemID.ID);
                if (itemMaster != null && string.IsNullOrEmpty(item.DescFlexSegments.PrivateDescSeg3))
                {
                    MiscShipLine.spec_no = itemMaster.Code1;
                }
                else if (!string.IsNullOrEmpty(item.DescFlexSegments.PrivateDescSeg3))
                {
                    MiscShipLine.spec_no = item.DescFlexSegments.PrivateDescSeg3;
                }

                MiscShipLine.num = item.StoreUOMQty;
                MiscShipLine.batch_no = item.LotInfo != null ? item.LotInfo.LotCode : "";
                MiscShipLine.remark = item.DocLineNo.ToString();
                MiscShipLines.Add(MiscShipLine);
            }
            miscShipment.goods_list = MiscShipLines;
            miscShipments.Add(miscShipment);
            list.Add(miscShipment);
            return JsonConvert.SerializeObject(miscShipments);
        }
        /// <summary>
        /// 杂收单创建其他入库业务单据委外仓库 先到旺店通ERP 再到WMS
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetStockinMiscRcvTransJson(MiscRcvTrans misc)
        {
            List<object> list = new List<object>();

            StockInPushDto miscShipment = new StockInPushDto();
            miscShipment.outer_no = misc.DocNo;
            miscShipment.warehouse_no = misc.MiscRcvTransLs[0].Wh?.Code;
            miscShipment.is_check = true;
            miscShipment.reason = "普通其他入库";
            miscShipment.prop2 = misc.CreatedBy;
            miscShipment.remark = misc.Memo;
            List<StockInPushDetailDto> MiscShipLines = new List<StockInPushDetailDto>();
            foreach (MiscRcvTransL item in misc.MiscRcvTransLs)
            {
                StockInPushDetailDto MiscShipLine = new StockInPushDetailDto();
                ItemMaster itemMaster = ItemMaster.Finder.FindByID(item.ItemInfo.ItemID.ID);
                if (itemMaster != null && string.IsNullOrEmpty(item.DescFlexSegments.PrivateDescSeg3))
                {
                    MiscShipLine.spec_no = itemMaster.Code1;
                }
                else if (!string.IsNullOrEmpty(item.DescFlexSegments.PrivateDescSeg3))
                {
                    MiscShipLine.spec_no = item.DescFlexSegments.PrivateDescSeg3;
                }
                if (!string.IsNullOrEmpty(itemMaster.DescFlexField.PrivateDescSeg12))
                {
                    int i = Convert.ToInt32(itemMaster.DescFlexField.PrivateDescSeg12);
                    if (i > 0)
                    {
                        MiscShipLine.aux_unit_name = i.ToString() + itemMaster.InventoryUOM?.Name + "/箱";
                    }

                }
                MiscShipLine.num = Convert.ToInt32(item.StoreUOMQty);
                MiscShipLine.batch_no = item.LotInfo != null ? item.LotInfo.LotCode : "";
                // MiscShipLine.remark = item.DocLineNo.ToString();
                MiscShipLine.remark = item.Memo;
                MiscShipLine.price = item.CostPrice;
                MiscShipLine.defect = item.StoreType.Value != 4 ? true : false;
                MiscShipLines.Add(MiscShipLine);
            }

            list.Add(miscShipment);
            list.Add(MiscShipLines);
            return JsonConvert.SerializeObject(list);
        }
        /// <summary>
        /// 退回处理推送其他业务入库
        /// </summary>
        /// <param name="misc"></param>
        ///  /// <param name="WHCode"></param>
        /// <returns></returns>
        public static string GetStockinFromRMA(RMA misc)
        {
            List<object> list = new List<object>();

            StockInPushDto miscShipment = new StockInPushDto();
            miscShipment.outer_no = misc.DocNo;
            miscShipment.warehouse_no = misc.RMALines[0].Warehouse?.Code;
            miscShipment.is_check = true;
            miscShipment.reason = "U9退货入库";
            List<StockInPushDetailDto> MiscShipLines = new List<StockInPushDetailDto>();
            foreach (RMALine item in misc.RMALines)
            {
                StockInPushDetailDto MiscShipLine = new StockInPushDetailDto();
                ItemMaster itemMaster = ItemMaster.Finder.FindByID(item.ItemInfo.ItemID.ID);
                if (itemMaster != null)
                {
                    MiscShipLine.spec_no = itemMaster.Code1;
                }
                MiscShipLine.num = Convert.ToInt32(item.RtnQtyTU1);
                MiscShipLine.batch_no = item.LotInfo != null ? item.LotInfo.LotCode : "";
                MiscShipLine.remark = item.Remark;
                MiscShipLine.price = item.ApplyPrice;
                MiscShipLine.defect = item.DescFlexField.PrivateDescSeg4=="2"?true:false;
                MiscShipLines.Add(MiscShipLine);
            }

            list.Add(miscShipment);
            list.Add(MiscShipLines);
            return JsonConvert.SerializeObject(list);
        }
        /// <summary>
        /// 退回处理推送原始退款单
        /// </summary>
        /// <param name="misc"></param>
        /// /// <param name="SO"></param>
        /// /// /// <param name="ship"></param>
        ///  /// /// /// <param name="item"></param>
        /// <returns></returns>
        public static string GetRawRefundFromRMA(RMA misc, string SO, Ship ship, RMALine item)
        {
            // decimal refundMny = 0m;
            List<object> list = new List<object>();
            List<string> itemlist = new List<string>();
            string ShopNo = misc.DescFlexField.PrivateDescSeg5; //不带点的 ，需要确认
            List<RawRefundDto> rawRefunds = new List<RawRefundDto>();
            string docno = string.Empty;
          
                RawRefundDto rawRefund = new RawRefundDto();
                docno = SO + item.Warehouse?.Code;
                rawRefund.refund_version = "1";
                rawRefund.refund_time = DateTime.Now.ToString();
                rawRefund.type = 2;
                rawRefund.status = 3;
                rawRefund.title = item.ItemInfo?.ItemCode;
                rawRefund.num = item.ApplyQtyTU1;
                rawRefund.tid = docno;
                ShipLine shipline = ShipLine.Finder.Find("ship=@ship and DocLineNo=@DocLineNo", new OqlParam("ship", ship.ID), new OqlParam("DocLineNo", item.SrcDocLineNo));
                if (shipline != null)
                {
                    rawRefund.oid = docno + shipline.SrcDocLineNo.ToString();
                }
                rawRefund.refund_no = ship.DocNo+ item.Warehouse?.Code+shipline.SrcDocLineNo.ToString();
                rawRefund.remark = misc.Remark;
                rawRefund.is_aftersale = false;
                rawRefund.refund_amount = item.ApplyMoneyTC;
                rawRefund.actual_refund_amount = item.ApplyMoneyTC;
                rawRefund.logistics_name = misc.DescFlexField.PrivateDescSeg3;
                rawRefund.logistics_no = misc.DescFlexField.PrivateDescSeg4;
                rawRefund.reason = misc.DescFlexField.PrivateDescSeg6;
                rawRefund.buyer_nick = misc.Customer?.Name;
                ItemMaster itemMaster = ItemMaster.Finder.FindByID(item.ItemInfo.ItemID.ID);
                rawRefund.spec_no = itemMaster?.Code1;
                rawRefund.spec_id = itemMaster?.Code1;
                rawRefund.goods_id = itemMaster?.Code1;
                rawRefund.goods_no = itemMaster?.Code1;
                rawRefund.price = item.RtnPice;
                rawRefunds.Add(rawRefund);
       

            list.Add(ShopNo);
            list.Add(rawRefunds);
            return JsonConvert.SerializeObject(list);
        }
        /// <summary>
        ///委外退货
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetMiscRcvPurReturn(Receivement misc)
        {
            bool is_check = true;
            List<object> list = new List<object>();
            // List<ReceivementDto> rcvDtos = new List<ReceivementDto>();
            ReceivementDto rcvDto = new ReceivementDto();
            rcvDto.outer_no = misc.DocNo;
            rcvDto.warehouse_no = misc.RcvLines[0].Wh != null ? misc.RcvLines[0].Wh.Code : "";
            rcvDto.remark = misc.Memo;
            //if (misc.RcvLines[0].Wh != null && misc.RcvLines[0].Wh.DescFlexField != null)
            //{
            //    rcvDto.contact = misc.RcvLines[0].Wh.DescFlexField.PrivateDescSeg6; ;
            //    rcvDto.telno = misc.RcvLines[0].Wh.DescFlexField.PrivateDescSeg7;
            //    rcvDto.receive_province = misc.RcvLines[0].Wh.DescFlexField.PrivateDescSeg8;
            //    rcvDto.receive_city = misc.RcvLines[0].Wh.DescFlexField.PrivateDescSeg9;
            //    rcvDto.receive_district = misc.RcvLines[0].Wh.DescFlexField.PrivateDescSeg10;
            //    rcvDto.receive_address = misc.RcvLines[0].Wh.DescFlexField.PrivateDescSeg11;
            //}
       

            rcvDto.provider_no = misc.Supplier.Code;
            //rcvDtos.Add(rcvDto);
            List<RcvReturnLine> RcvReturnsLines = new List<RcvReturnLine>();
            foreach (RcvLine item in misc.RcvLines)
            {
                RcvReturnLine rcvline = new RcvReturnLine();
                ItemMaster itemMaster = ItemMaster.Finder.FindByID(item.ItemInfo.ItemID.ID);
                if (!string.IsNullOrEmpty(item.DescFlexSegments?.PrivateDescSeg2))
                {
                    rcvline.spec_no = item.DescFlexSegments?.PrivateDescSeg2;
                }
                else if (itemMaster != null)
                {
                    rcvline.spec_no = itemMaster.Code1;
                }

                rcvline.num = item.RejectQtyTU;
                rcvline.batch_no = item.InvLotCode;
                // rcvline.remark = item.DocLineNo.ToString();
                rcvline.remark = item.DescFlexSegments.PrivateDescSeg1;
                rcvline.tax_rate = item.TaxRate;
                rcvline.defect = item.StorageType?.Value == 2 ? true : false;
                if (!string.IsNullOrEmpty(itemMaster.DescFlexField.PrivateDescSeg12))
                {
                    int i = Convert.ToInt32(itemMaster.DescFlexField.PrivateDescSeg12);
                    if (i > 0)
                    {
                        rcvline.unit_name = i.ToString() + itemMaster.InventoryUOM?.Name + "/箱";
                    }

                }
                // rcvline.unit_name = item.StoreUOM.BaseUOM?.Name;
                rcvline.price = item.FinallyPriceTC;
                rcvline.discount = 1m;
                RcvReturnsLines.Add(rcvline);
            }
            list.Add(rcvDto);
            list.Add(RcvReturnsLines);
            list.Add(is_check);
            return JsonConvert.SerializeObject(list);
        }
        /// <summary>
        ///委外收货创建
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetPurReturn(PurchaseOrder misc, StockinPurchaseOrderDto dto, string OrgCode)
        {
            PMRcvToRtn pMRcvToRtn = new PMRcvToRtn();
            RcvToRtnRcvDTO rcvToRtnRcvDTO = new RcvToRtnRcvDTO();
            rcvToRtnRcvDTO.CreatedBy = dto.operator_name;
            rcvToRtnRcvDTO.BusinessDate = dto.modified != null ? CHelper.GetDateTimeMilliseconds(Convert.ToInt64(dto.modified)) : DateTime.Now.ToString();
            rcvToRtnRcvDTO.Status = 0;
            rcvToRtnRcvDTO.DocType = "RCV92";
            rcvToRtnRcvDTO.SupplyCode = dto.provider_no;
            rcvToRtnRcvDTO.ConfirmDate = rcvToRtnRcvDTO.BusinessDate; //dto.check_time;
            rcvToRtnRcvDTO.DocNo = dto.order_no;
            rcvToRtnRcvDTO.OrgCode = OrgCode;
            rcvToRtnRcvDTO.Memo = dto.remark;
            PubPriSVData pubPriSVData = new PubPriSVData();
            pubPriSVData.PrivateDescSeg1 = dto.logistics_no;
            pubPriSVData.PrivateDescSeg2 = dto.logistics_name;
            pubPriSVData.PrivateDescSeg3 = dto.order_no;
            rcvToRtnRcvDTO.PubPriDt = pubPriSVData;
            Organization org = Organization.FindByCode(OrgCode);
            Warehouse WH = Warehouse.FindByCode(org,dto.warehouse_no);
            //if (WH != null && WH.DescFlexField.PrivateDescSeg4 == "True")
            //{
            //    rcvToRtnRcvDTO.Status = 2;
            //}
            //else
            //{
            //    rcvToRtnRcvDTO.Status = 0;
            //}
            List<RcvToRtnLineDTO> lines = new List<RcvToRtnLineDTO>();
            foreach (POLine item in misc.POLines)
            {
                foreach (StockinPurchaseOrderDeatail deatail in dto.details_list)
                {
                    if (item.ItemInfo.ItemCode == deatail.goods_no)
                    {
                        RcvToRtnLineDTO rtnLine = new RcvToRtnLineDTO();
                        rtnLine.FromDocLineNo = item.DocLineNo.ToString();
                        rtnLine.FromDocNo = misc.DocNo;
                        rtnLine.ConfirmDate = rcvToRtnRcvDTO.BusinessDate;
                        rtnLine.Qty = deatail.num;
                        rtnLine.WHCode = dto.warehouse_no;
                        rtnLine.WHMan = misc.PurOper != null ? misc.PurOper.Code : "";
                        rtnLine.LotCode = deatail.batch_no;
                        rtnLine.Defect = deatail.defect;
                        PubPriSVData publinePriSVData = new PubPriSVData();
                        publinePriSVData.PrivateDescSeg1 = deatail.remark; ;
                        rtnLine.PubPriDt = publinePriSVData;
                        lines.Add(rtnLine);
                    }
                    else if (item.DescFlexSegments.PrivateDescSeg1 == deatail.spec_no)
                    {
                        RcvToRtnLineDTO rtnLine = new RcvToRtnLineDTO();
                        rtnLine.FromDocLineNo = item.DocLineNo.ToString();
                        rtnLine.FromDocNo = misc.DocNo;
                        rtnLine.ConfirmDate = dto.check_time;
                        rtnLine.Qty = deatail.num;
                        rtnLine.WHCode = dto.warehouse_no;
                        rtnLine.WHMan = misc.PurOper != null ? misc.PurOper.Code : "";
                        rtnLine.LotCode = deatail.batch_no;
                        rtnLine.Defect = deatail.defect;
                        PubPriSVData publinePriSVData = new PubPriSVData();
                        publinePriSVData.PrivateDescSeg1 = deatail.remark; ;
                        rtnLine.PubPriDt = publinePriSVData;
                        lines.Add(rtnLine);
                    }
                }

            }
            pMRcvToRtn.OrgCode = OrgCode;
            pMRcvToRtn.EntCode = CHelper.EntCode;
            pMRcvToRtn.UserCode = "A001";
            pMRcvToRtn.OptType = CHelper.PMRcvOptype;
            rcvToRtnRcvDTO.RcvToRtnLine = lines;
            pMRcvToRtn.RtnRCVDTO = rcvToRtnRcvDTO;
            return JsonConvert.SerializeObject(pMRcvToRtn);
        }
        /// <summary>
        /// 调入单创建 调拨出库
        /// </summary>
        /// <param name="order"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static string GetTransferInDoc(StockoutTransferOrderDto order,Organization org)
        {
            TransferToIn pMRcvToRtn = new TransferToIn();
            TransferOneInDTO rcvToRtnRcvDTO = new TransferOneInDTO();
            rcvToRtnRcvDTO.BussinessDate = DateTime.Now.ToString();
            rcvToRtnRcvDTO.Status = 0;
           // rcvToRtnRcvDTO.DocNo = order.src_order_no;
            rcvToRtnRcvDTO.DocType = "TransIn011";
            rcvToRtnRcvDTO.TransferDirection = 0;
            rcvToRtnRcvDTO.CreatedBy = order.operator_name;
            rcvToRtnRcvDTO.TransferInType = 0;
            PubPriSVData pubPriSVData = new PubPriSVData();
            pubPriSVData.PrivateDescSeg1 = order.src_order_no;
            pubPriSVData.PrivateDescSeg2 = order.order_no;
            rcvToRtnRcvDTO.PubPriDt = pubPriSVData;
            int num = 10;
            List<TransInOneLineDTO> lines = new List<TransInOneLineDTO>();
           
                foreach (StockoutTransferOrderDeatail deatail in order.detail_list)
                {
                List<TransInChildLineSVData> sublines = new List<TransInChildLineSVData>();
                TransInOneLineDTO rtnLine = new TransInOneLineDTO();
                rtnLine.LineNum = num;
                rtnLine.RcvOrg = org.Code;
                rtnLine.ItemCode = deatail.goods_no;
                rtnLine.RcvQty = deatail.goods_count;
                rtnLine.RcvWH = order.to_warehouse_no;
                Warehouse WH = Warehouse.FindByCode(org, order.to_warehouse_no);
                if (WH != null && WH.DescFlexField.PrivateDescSeg14 == "True")
                {
                    rcvToRtnRcvDTO.Status = 2;
                }
                else
                {
                    rcvToRtnRcvDTO.Status = 0;
                }
                TransInChildLineSVData subline = new TransInChildLineSVData();
                subline.LineNum = num;
                subline.ShipWH = order.from_warehouse_no;
                subline.ShipQty = deatail.goods_count;
                subline.OutOrg = org?.Code;
                subline.ItemCode = deatail.goods_no;
                sublines.Add(subline);
                rtnLine.TransInSubLines = sublines;
                   lines.Add(rtnLine);
                num += 10;
            }
 
            pMRcvToRtn.OrgCode = org.Code;
            pMRcvToRtn.EntCode = CHelper.EntCode;
            pMRcvToRtn.UserCode = "A001";
            pMRcvToRtn.OptType = CHelper.TransferOptype;
            rcvToRtnRcvDTO.TransInLineData = lines;
            pMRcvToRtn.TransInDTO = rcvToRtnRcvDTO;
            return JsonConvert.SerializeObject(pMRcvToRtn);
        }
        /// <summary>
        /// 调入单创建 调拨入库
        /// </summary>
        /// <param name="order"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static string  GetTransferInDocFromIn(StockinTransferOrderDto order, Organization org)
        {
            TransferToIn pMRcvToRtn = new TransferToIn();
            TransferOneInDTO rcvToRtnRcvDTO = new TransferOneInDTO();
            rcvToRtnRcvDTO.BussinessDate = DateTime.Now.ToString();
            rcvToRtnRcvDTO.Status = 0;
            // rcvToRtnRcvDTO.DocNo = order.src_order_no;
            rcvToRtnRcvDTO.DocType = "TransIn011";
            rcvToRtnRcvDTO.TransferDirection = 0;
            rcvToRtnRcvDTO.CreatedBy = order.operator_name;
            rcvToRtnRcvDTO.TransferInType = 0;
            PubPriSVData pubPriSVData = new PubPriSVData();
            pubPriSVData.PrivateDescSeg1 = order.src_order_no;
            pubPriSVData.PrivateDescSeg2 = order.order_no;
            pubPriSVData.PrivateDescSeg12 = "旺店通接口创建";
            rcvToRtnRcvDTO.Memo = order.remark;
            rcvToRtnRcvDTO.PubPriDt = pubPriSVData;
            int num = 10;
            List<TransInOneLineDTO> lines = new List<TransInOneLineDTO>();

            foreach (StockinTransferOrderDeatail deatail in order.detail_list)
            {
                List<TransInChildLineSVData> sublines = new List<TransInChildLineSVData>();
                TransInOneLineDTO rtnLine = new TransInOneLineDTO();
                rtnLine.LineNum = num;
                rtnLine.RcvOrg = org.Code;
                rtnLine.ItemCode = deatail.goods_no;
                rtnLine.RcvQty = deatail.num;
                rtnLine.RcvWH = order.to_warehouse_no;
                Warehouse WH = Warehouse.FindByCode(org, order.to_warehouse_no);
                if (WH != null && WH.DescFlexField.PrivateDescSeg14 == "True")
                {
                    rcvToRtnRcvDTO.Status = 2;
                }
                else
                {
                    rcvToRtnRcvDTO.Status = 0;
                }
                rtnLine.Memo = deatail.remark;
                TransInChildLineSVData subline = new TransInChildLineSVData();
                subline.LineNum = num;
                subline.ShipWH = order.from_warehouse_no;
                subline.ShipQty = deatail.num;
                subline.OutOrg = org?.Code;
                subline.ItemCode = deatail.goods_no;

                sublines.Add(subline);
                rtnLine.TransInSubLines = sublines;
                lines.Add(rtnLine);
                num += 10;
            }

            pMRcvToRtn.OrgCode = org.Code;
            pMRcvToRtn.EntCode = CHelper.EntCode;
            pMRcvToRtn.UserCode = "A001";
            pMRcvToRtn.OptType = CHelper.TransferOptype;
            rcvToRtnRcvDTO.TransInLineData = lines;
            pMRcvToRtn.TransInDTO = rcvToRtnRcvDTO;
            return JsonConvert.SerializeObject(pMRcvToRtn);
        }
        /// <summary>
        /// 调入单创建拆单逻辑 调拨出库
        /// </summary>
        /// <param name="order"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static string  GetTransferInDocs(StockoutTransferOrderDto order, Organization org)
        {
            Organization orga;
            TransferToIn pMRcvToRtn = new TransferToIn();
            TransferOneInDTO rcvToRtnRcvDTO = new TransferOneInDTO();
            rcvToRtnRcvDTO.BussinessDate = DateTime.Now.ToString();
            rcvToRtnRcvDTO.Status = 0;
            // rcvToRtnRcvDTO.DocNo = order.src_order_no;
            rcvToRtnRcvDTO.DocType = "TransIn011";
            rcvToRtnRcvDTO.TransferDirection = 0;
            rcvToRtnRcvDTO.CreatedBy = order.operator_name;
            rcvToRtnRcvDTO.TransferInType = 0;
            PubPriSVData pubPriSVData = new PubPriSVData();
            pubPriSVData.PrivateDescSeg1 = order.src_order_no;
            pubPriSVData.PrivateDescSeg2 = order.order_no;
            rcvToRtnRcvDTO.PubPriDt = pubPriSVData;
            int num = 10;
            List<TransInOneLineDTO> lines = new List<TransInOneLineDTO>();

            foreach (StockoutTransferOrderDeatail deatail in order.detail_list)
            {
                orga = CHelper.GetOrg(deatail.goods_no, "");
                if (orga != null && orga.ID == org.ID)
                {
                    List<TransInChildLineSVData> sublines = new List<TransInChildLineSVData>();
                    TransInOneLineDTO rtnLine = new TransInOneLineDTO();
                    rtnLine.LineNum = num;
                    rtnLine.RcvOrg = org.Code;
                    rtnLine.ItemCode = deatail.goods_no;
                    rtnLine.RcvQty = deatail.goods_count;
                    rtnLine.RcvWH = order.to_warehouse_no;
                    Warehouse WH = Warehouse.FindByCode(org, order.to_warehouse_no);
                    if (WH != null && WH.DescFlexField.PrivateDescSeg14 == "True")
                    {
                        rcvToRtnRcvDTO.Status = 2;
                    }
                    else
                    {
                        rcvToRtnRcvDTO.Status = 0;
                    }
                    TransInChildLineSVData subline = new TransInChildLineSVData();
                    subline.LineNum = num;
                    subline.ShipWH = order.from_warehouse_no;
                    subline.ShipQty = deatail.goods_count;
                    subline.OutOrg = org?.Code;
                    subline.ItemCode = deatail.goods_no;
                    sublines.Add(subline);
                    rtnLine.TransInSubLines = sublines;
                    lines.Add(rtnLine);
                    num += 10;
                }
              
            }

            pMRcvToRtn.OrgCode = org.Code;
            pMRcvToRtn.EntCode = CHelper.EntCode;
            pMRcvToRtn.UserCode = "A001";
            pMRcvToRtn.OptType = CHelper.TransferOptype;
            rcvToRtnRcvDTO.TransInLineData = lines;
            pMRcvToRtn.TransInDTO = rcvToRtnRcvDTO;
            return JsonConvert.SerializeObject(pMRcvToRtn);
        }
        /// <summary>
        /// 调入单创建拆单逻辑 调拨入库
        /// </summary>
        /// <param name="order"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static string  GetTransferInDocsFromIn(StockinTransferOrderDto order, Organization org)
        {
            Organization orga;
            TransferToIn pMRcvToRtn = new TransferToIn();
            TransferOneInDTO rcvToRtnRcvDTO = new TransferOneInDTO();
            rcvToRtnRcvDTO.BussinessDate = DateTime.Now.ToString();
            rcvToRtnRcvDTO.Status = 0;
            // rcvToRtnRcvDTO.DocNo = order.src_order_no;
            rcvToRtnRcvDTO.DocType = "TransIn011";
            rcvToRtnRcvDTO.TransferDirection = 0;
            rcvToRtnRcvDTO.CreatedBy = order.operator_name;
            rcvToRtnRcvDTO.TransferInType = 0;
            PubPriSVData pubPriSVData = new PubPriSVData();
            pubPriSVData.PrivateDescSeg1 = order.src_order_no;
            pubPriSVData.PrivateDescSeg2 = order.order_no;
            pubPriSVData.PrivateDescSeg12 = "旺店通接口创建";
            rcvToRtnRcvDTO.PubPriDt = pubPriSVData;
            int num = 10;
            List<TransInOneLineDTO> lines = new List<TransInOneLineDTO>();

            foreach (StockinTransferOrderDeatail deatail in order.detail_list)
            {
                orga = CHelper.GetOrg(deatail.goods_no, "");
                if (orga != null && orga.ID == org.ID)
                {
                    List<TransInChildLineSVData> sublines = new List<TransInChildLineSVData>();
                    TransInOneLineDTO rtnLine = new TransInOneLineDTO();
                    rtnLine.LineNum = num;
                    rtnLine.RcvOrg = org.Code;
                    rtnLine.ItemCode = deatail.goods_no;
                    rtnLine.RcvQty = deatail.num;
                    rtnLine.RcvWH = order.to_warehouse_no;
                    Warehouse WH = Warehouse.FindByCode(org, order.to_warehouse_no);
                    if (WH != null && WH.DescFlexField.PrivateDescSeg14=="True")
                    {
                        rcvToRtnRcvDTO.Status = 2;
                    }
                    else
                    {
                        rcvToRtnRcvDTO.Status = 0;
                    }
                    TransInChildLineSVData subline = new TransInChildLineSVData();
                    subline.LineNum = num;
                    subline.ShipWH = order.from_warehouse_no;
                    subline.ShipQty = deatail.num;
                    subline.OutOrg = org?.Code;
                    subline.ItemCode = deatail.goods_no;
                    sublines.Add(subline);
                    rtnLine.TransInSubLines = sublines;
                    lines.Add(rtnLine);
                    num += 10;
                }

            }

            pMRcvToRtn.OrgCode = org.Code;
            pMRcvToRtn.EntCode = CHelper.EntCode;
            pMRcvToRtn.UserCode = "A001";
            pMRcvToRtn.OptType = CHelper.TransferOptype;
            rcvToRtnRcvDTO.TransInLineData = lines;
            pMRcvToRtn.TransInDTO = rcvToRtnRcvDTO;
            return JsonConvert.SerializeObject(pMRcvToRtn);
        }
        /// <summary>
        ///委外采购
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetPurchaseOrderExternal(PurchaseOrder misc)
        {
            List<object> list = new List<object>();
            PurchaseOrderExternalDto rcvDto = new PurchaseOrderExternalDto();
            rcvDto.purchase_no = misc.DocNo;
            rcvDto.provider_no = misc.Supplier != null ? misc.Supplier.Code : "";
            POShipLine ss = POShipLine.Finder.Find("POLine=@POLine", new UFSoft.UBF.PL.OqlParam("POLine", misc.POLines[0].ID));
            
            if (ss != null)
            {
                rcvDto.receive_warehouse_nos = ss.Wh != null ? ss.Wh.Code : "";
                rcvDto.expect_warehouse_no = ss.Wh != null ? ss.Wh.Code : "";
                if (ss.PlanArriveDate != null)
                {
                    rcvDto.expect_time = ss.PlanArriveDate.ToString();
                }
            }
           
            rcvDto.purchaser_name = misc.PurOper != null ? misc.PurOper.Name : "";
            rcvDto.is_check = true;
            //  rcvDto.expect_time=misc.POLines[0].shipmen
            rcvDto.prop3 = "委外采购";
            rcvDto.prop1 = misc.DocNo;
            POMemo memo = POMemo.Finder.Find("PurchaseOrder=@POID", new OqlParam("POID", misc.ID));
            if (memo != null)
            {
                rcvDto.remark = memo.Description;
            }

            List<PurchaseOrderExternalDetail> RcvReturnsLines = new List<PurchaseOrderExternalDetail>();
            foreach (POLine item in misc.POLines)
            {
                PurchaseOrderExternalDetail rcvline = new PurchaseOrderExternalDetail();
                ItemMaster itemMaster = ItemMaster.Finder.FindByID(item.ItemInfo.ItemID.ID);
                if (itemMaster != null)
                {
                    rcvline.spec_no = itemMaster.Code1;
                }

                rcvline.num = Convert.ToInt32(item.PurQtyPU);
                rcvline.tax_rate = item.TaxRate;
                // rcvline.remark = item.DocLineNo.ToString();
                rcvline.price = Math.Round(item.NetFinallyPriceTC, 2);
                rcvline.remark = item.DescFlexSegments.PrivateDescSeg2;
                rcvline.discount = 1;
                if (!string.IsNullOrEmpty(itemMaster.DescFlexField.PrivateDescSeg12))
                {
                    int i = Convert.ToInt32(itemMaster.DescFlexField.PrivateDescSeg12);
                    if (i>0)
                    {
                        rcvline.purchase_unit_name = i.ToString() + itemMaster.InventoryUOM?.Name + "/箱";
                    }
                   
                }
                // rcvline.purchase_unit_name = item.TradeUOM != null ? item.TradeUOM.Name : "";
                rcvline.tax_price = Math.Round(item.FinallyPriceTC, 2);
                RcvReturnsLines.Add(rcvline);
            }
            rcvDto.purchase_details = RcvReturnsLines;
            list.Add(rcvDto);
            return JsonConvert.SerializeObject(list);
        }
        /// <summary>
        ///标准收货
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetReceivement(Receivement misc)
        {
            List<object> list = new List<object>();
            StandardRCVDto rcvDto = new StandardRCVDto();
            rcvDto.purchase_no = misc.DocNo;
            rcvDto.warehouse_no = misc.RcvLines[0].Wh != null ? misc.RcvLines[0].Wh.Code : "";


            List<StandardRcvDetailsDto> RcvReturnsLines = new List<StandardRcvDetailsDto>();
            foreach (RcvLine item in misc.RcvLines)
            {
                StandardRcvDetailsDto rcvline = new StandardRcvDetailsDto();
                ItemMaster itemMaster = ItemMaster.Finder.FindByID(item.ItemInfo.ItemID.ID);
                if (itemMaster != null)
                {
                    rcvline.spec_no = itemMaster.Code1;
                }
                rcvline.num = item.RcvQtyTU;
                rcvline.batch_no = item.InvLotCode;
                rcvline.remark = item.DocLineNo.ToString();
                rcvline.unit_name = item.StoreUOM != null ? item.StoreUOM.Name : "";
                RcvReturnsLines.Add(rcvline);
            }
            list.Add(rcvDto);
            list.Add(RcvReturnsLines);
            return JsonConvert.SerializeObject(list);
        }
        /// <summary>
        ///标准采购
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetPurchaseOrder(PurchaseOrder misc)
        {

            List<object> list = new List<object>();
            PurchaseOrderExternalDto rcvDto = new PurchaseOrderExternalDto();
            rcvDto.purchase_no = misc.DocNo;
            rcvDto.provider_no = misc.Supplier != null ? misc.Supplier.Code : "";
            rcvDto.is_check = true;
            POShipLine ss = POShipLine.Finder.Find("POLine=@POLine", new UFSoft.UBF.PL.OqlParam("POLine", misc.POLines[0].ID));
            if (ss != null)
            {
                rcvDto.receive_warehouse_nos = ss.Wh != null ? ss.Wh.Code : "";
                rcvDto.expect_warehouse_no = ss.Wh != null ? ss.Wh.Code : "";
                if (ss.PlanArriveDate != null)
                {
                    rcvDto.expect_time = ss.PlanArriveDate.ToString();
                }
            }
        
            rcvDto.purchaser_name = misc.PurOper != null ? misc.PurOper.Name : "";
            rcvDto.prop1 = misc.DocNo;
            rcvDto.prop3 = "标准采购";
            POMemo memo = POMemo.Finder.Find("PurchaseOrder=@POID", new OqlParam("POID", misc.ID));
            if (memo!=null)
            {
                rcvDto.remark = memo.Description;
            }
          
          
            List<PurchaseOrderExternalDetail> RcvReturnsLines = new List<PurchaseOrderExternalDetail>();
            foreach (POLine item in misc.POLines)
            {
                PurchaseOrderExternalDetail rcvline = new PurchaseOrderExternalDetail();
                ItemMaster itemMaster = ItemMaster.Finder.FindByID(item.ItemInfo.ItemID.ID);
                if (itemMaster != null)
                {
                    //rcvline.spec_no = itemMaster.Code1;//关联旺店通商家编码改为私有段1

                    if (!string.IsNullOrEmpty(item.DescFlexSegments.PrivateDescSeg1))
                    {
                        rcvline.spec_no = item.DescFlexSegments.PrivateDescSeg1;
                    }
                    else
                    {
                        rcvline.spec_no = itemMaster.Code1;
                    }
                    if (!string.IsNullOrEmpty(itemMaster.DescFlexField.PrivateDescSeg12))
                    {
                        int i = Convert.ToInt32(itemMaster.DescFlexField.PrivateDescSeg12);
                        if (i>0)
                        {
                            rcvline.purchase_unit_name = i.ToString() + itemMaster.InventoryUOM?.Name + "/箱";
                        }
                       
                    }
                   
                }

                rcvline.num = Convert.ToInt32(item.PurQtyPU);
                rcvline.tax_rate = item.TaxRate;
                rcvline.price = Math.Round(item.NetFinallyPriceTC, 2);
                // rcvline.purchase_unit_name = item.TradeUOM != null ? item.TradeUOM.Name : "";
                rcvline.tax_price = Math.Round(item.FinallyPriceTC, 2);
                rcvline.discount = 1;
                // rcvline.remark = item.DocLineNo.ToString();
                rcvline.remark = item.DescFlexSegments.PrivateDescSeg2;
                RcvReturnsLines.Add(rcvline);
            }
            rcvDto.purchase_details = RcvReturnsLines;
            list.Add(rcvDto);
            return JsonConvert.SerializeObject(list);
        }
        /// <summary>
        /// 采购单新建从退货
        /// </summary>
        /// <param name="misc"></param>
        /// <returns></returns>
        public static string GetPurchaseOrderFromOCRtn(Receivement misc)
        {

            List<object> list = new List<object>();
            PurchaseOrderExternalDto rcvDto = new PurchaseOrderExternalDto();
            rcvDto.purchase_no = misc.DocNo;
            rcvDto.provider_no = misc.Supplier != null ? misc.Supplier.Code : "";
            rcvDto.is_check = true;
            rcvDto.remark = misc.Memo;
            RcvLine ss = RcvLine.Finder.Find("ID=@ID and DocLineNo=@DocLineNo", new UFSoft.UBF.PL.OqlParam("ID", misc.RcvLines[0]?.ID), new UFSoft.UBF.PL.OqlParam("DocLineNo", 10));
            if (ss != null)
            {
                rcvDto.receive_warehouse_nos = ss.Wh != null ? ss.Wh.Code : "";
                rcvDto.expect_warehouse_no = ss.Wh != null ? ss.Wh.Code : "";
            }


            rcvDto.prop1 = misc.RcvLines[0]?.SrcPO?.SrcDocNo;
            rcvDto.prop3 = "标准采购";
            if (ss.ArrivedTime!=null)
            {
                rcvDto.expect_time = ss.ArrivedTime.ToString();
            }
           
            List<PurchaseOrderExternalDetail> RcvReturnsLines = new List<PurchaseOrderExternalDetail>();
            foreach (RcvLine item in misc.RcvLines)
            {
                PurchaseOrderExternalDetail rcvline = new PurchaseOrderExternalDetail();
                ItemMaster itemMaster = ItemMaster.Finder.FindByID(item.ItemInfo.ItemID.ID);
                if (itemMaster != null)
                {
                    //rcvline.spec_no = itemMaster.Code1;//关联旺店通商家编码改为私有段1

                    if (!string.IsNullOrEmpty(item.DescFlexSegments?.PrivateDescSeg2))
                    {
                        rcvline.spec_no = item.DescFlexSegments?.PrivateDescSeg2;
                    }
                    else
                    {
                        rcvline.spec_no = itemMaster.Code1;
                    }
                }
                rcvDto.purchaser_name = item.WhMan != null ? item.WhMan.Name : "";
                if (item.RtnFillQtyTU>0)
                {
                    rcvline.num = Convert.ToInt32(item.RtnFillQtyTU);
                }
              
                rcvline.tax_rate = item.TaxRate>0?item.TaxRate:0m;
                rcvline.price = Math.Round(item.FinallyPriceTC, 2);
                if (!string.IsNullOrEmpty(itemMaster.DescFlexField?.PrivateDescSeg12))
                {
                    int i = Convert.ToInt32(itemMaster.DescFlexField?.PrivateDescSeg12);
                    if (i > 0)
                    {
                        rcvline.purchase_unit_name = i.ToString() + itemMaster.InventoryUOM?.Name + "/箱";
                    }

                }
                // rcvline.purchase_unit_name = item.TradeUOM != null ? item.TradeUOM.Name : "";
                rcvline.tax_price = Math.Round(item.FinallyPriceTC, 2);
                rcvline.discount = 1;
                //rcvline.remark = item.DocLineNo.ToString();
                rcvline.remark = item.DescFlexSegments?.PrivateDescSeg1;
                rcvline.defect = item.StorageType?.Value == 2 ? true : false;
                RcvReturnsLines.Add(rcvline);
            }
            rcvDto.purchase_details = RcvReturnsLines;
            list.Add(rcvDto);
            return JsonConvert.SerializeObject(list);
        }
        /// <summary>
        /// 关闭旺店通采购订单
        /// </summary>
        /// <param name="docs"></param>
        /// <returns></returns>
        public static string GetPurchaseOrderByClose(List<string> docs)
        {
            List<object> list = new List<object>();
            ClosePurchaseOrderDto CDto = new ClosePurchaseOrderDto();
            CDto.operate_type = 1;
            CDto.purchase_no_list = docs;
            CDto.allow_cancel_checked_order = 1;
            list.Add(CDto);
            return JsonConvert.SerializeObject(list);
        }
        /// <summary>
        /// 奇门来源出货计划标准出货创建
        /// </summary>
        /// <param name="misc"></param>
        /// <param name="SrcOrderNo"></param>
        /// <returns></returns>
        public static string GetShipPlanQM(ShipPlan misc, WdtWmsStockoutSalesQuerywithdetailResponse.OrderDomain order, string OrgCode, long TradeFrom)
        {
            ShipPlanToShip ShipPlanToShip = new ShipPlanToShip();
            ShipPlanToShipDTO shipPlanToShipDTO = new ShipPlanToShipDTO();
            shipPlanToShipDTO.BusinessDate = DateTime.Now.ToString(); // order.ConsignTime;
            shipPlanToShipDTO.Status = 2;
            if (TradeFrom == 1)
            {
                shipPlanToShipDTO.DocType = "SM2";
            }
            else if (TradeFrom == 2 || TradeFrom == 3 || TradeFrom == 4 || TradeFrom == 6)
            {
                shipPlanToShipDTO.DocType = "SM4";
            }
            shipPlanToShipDTO.CreatedBy = misc.CreatedBy;
            shipPlanToShipDTO.OrgCode = OrgCode;
            shipPlanToShipDTO.DocNo = order.OrderNo;
            Customer customer = Customer.Finder.Find("Name=@Name", new UFSoft.UBF.PL.OqlParam("Name", order.ShopNo));
            shipPlanToShipDTO.CustomerCode = customer?.Code;
            PubPriSVData pubPriSVData = new PubPriSVData();
            pubPriSVData.PrivateDescSeg1 = order.LogisticsName;
            pubPriSVData.PrivateDescSeg2 = order.LogisticsCode;
            shipPlanToShipDTO.PubPriDt = pubPriSVData;

            List<ShipPlanToShipLineDTO> lines = new List<ShipPlanToShipLineDTO>();
            foreach (ShipPlanLine item in misc.ShipPlanLines)
            {
                foreach (var detail in order.DetailsList)
                {
                    if (detail.GoodsNo == item.ItemInfo.ItemCode)
                    {
                        ShipPlanToShipLineDTO rtnLine = new ShipPlanToShipLineDTO();
                        rtnLine.ShipPlanLineNum = item.DocLineNo;
                        rtnLine.ShipPlanDocNo = misc.DocNo;
                        rtnLine.LineNo = item.DocLineNo;
                        rtnLine.ConfirmDate = order.ConsignTime;
                        rtnLine.PubPriDt = new PubPriSVData();
                        rtnLine.PubPriDt.PrivateDescSeg1 = detail.SuiteNo;
                        rtnLine.PubPriDt.PrivateDescSeg2 = detail.SellPrice;
                        rtnLine.PubPriDt.PrivateDescSeg3 = detail.Remark;
                        rtnLine.PubPriDt.PrivateDescSeg4 = detail.RefundStatus;
                        rtnLine.PubPriDt.PrivateDescSeg5 = detail.SrcOid;
                        rtnLine.ShipQty = Convert.ToDecimal(detail.GoodsCount);
                        rtnLine.ShipWH = item.WH != null ? item.WH.Code : "";
                        lines.Add(rtnLine);
                    }
                }

            }
            shipPlanToShipDTO.ShipLineDt = lines;
            ShipPlanToShip.ShipDTO = shipPlanToShipDTO;
            ShipPlanToShip.EntCode = CHelper.EntCode;
            ShipPlanToShip.UserCode = "A001";
            ShipPlanToShip.OrgCode = OrgCode;
            ShipPlanToShip.OptType = CHelper.ShipSrcShipOptype;
            return JsonConvert.SerializeObject(ShipPlanToShip);
        }
        /// <summary>
        /// 奇门来源SO标准出货创建
        /// </summary>
        /// <param name="misc"></param>
        /// <param name="SrcOrderNo"></param>
        /// <returns></returns>
        public static string GetShipPlanWDT(ShipPlan misc, MiscSalesOrderDto order, string OrgCode, string DocType)
        {
            ShipPlanToShip ShipPlanToShip = new ShipPlanToShip();
            ShipPlanToShipDTO shipPlanToShipDTO = new ShipPlanToShipDTO();
            shipPlanToShipDTO.BusinessDate = DateTime.Now.ToString();
            shipPlanToShipDTO.Status = 2;
            //Customer customer = Customer.Finder.Find("Name=@Name", new UFSoft.UBF.PL.OqlParam("Name", order.shop_no));         
            shipPlanToShipDTO.DocType = DocType;
            shipPlanToShipDTO.CreatedBy = misc.CreatedBy;
            shipPlanToShipDTO.OrgCode = OrgCode;
            shipPlanToShipDTO.DocNo = order.order_no;
            PubPriSVData pubPriSVData = new PubPriSVData();
            pubPriSVData.PrivateDescSeg1 = order.logistics_name;
            pubPriSVData.PrivateDescSeg2 = order.logistics_code;
            shipPlanToShipDTO.PubPriDt = pubPriSVData;

            List<ShipPlanToShipLineDTO> lines = new List<ShipPlanToShipLineDTO>();
            foreach (ShipPlanLine item in misc.ShipPlanLines)
            {
                foreach (var detail in order.details_list)
                {
                    if (detail.goods_no == item.ItemInfo.ItemCode)
                    {
                        ShipPlanToShipLineDTO rtnLine = new ShipPlanToShipLineDTO();
                        rtnLine.ShipPlanLineNum = item.DocLineNo;
                        rtnLine.ShipPlanDocNo = misc.DocNo;
                        rtnLine.LineNo = item.DocLineNo;
                        shipPlanToShipDTO.CustomerCode = item.OrderBy?.Code;
                        rtnLine.ConfirmDate = DateTime.Now.ToString();
                        rtnLine.ShipQty = Convert.ToDecimal(detail.goods_count);
                        rtnLine.ShipWH = item.WH != null ? item.WH.Code : "";
                        lines.Add(rtnLine);
                    }
                }

            }
            shipPlanToShipDTO.ShipLineDt = lines;
            ShipPlanToShip.ShipDTO = shipPlanToShipDTO;
            ShipPlanToShip.EntCode = CHelper.EntCode;
            ShipPlanToShip.UserCode = "A001";
            ShipPlanToShip.OrgCode = OrgCode;
            ShipPlanToShip.OptType = CHelper.ShipSrcShipOptype;
            return JsonConvert.SerializeObject(ShipPlanToShip);
        }
        /// <summary>
        /// 奇门预入库创建U9杂收
        /// </summary>
        /// <param name="misc"></param>
        /// <param name="SrcOrderNo"></param>
        /// <returns></returns>
        public static string GetQMMiscRcvTrans(WdtWmsStockinPrestockinSearchResponse.OrderDomain item, Organization org)
        {

            PreStockinMiscRcvTransDto ShipPlanToShip = new PreStockinMiscRcvTransDto();
            MiscRcvSVData MiscRcvDTO = new MiscRcvSVData();
            Warehouse WH = Warehouse.FindByCode(org, item.WarehouseNo);
            MiscRcvDTO.BussinessDate = item.CheckTime != null ? CHelper.GetDateTimeMilliseconds(Convert.ToInt64(item.CheckTime)) : DateTime.Now.ToString();
            MiscRcvDTO.Status = 2;
            MiscRcvDTO.DocType = "MiscRcv004";
            MiscRcvDTO.DocNo = item.StockinNo;
            MiscRcvDTO.Memo = item.Remark;
            MiscRcvDTO.OrgCode = org.Code;
            MiscRcvDTO.WHMan = WH?.Manager?.Code;
            MiscRcvDTO.CreatedBy = item.OperatorName;
            PubPriSVData pubPriSVData = new PubPriSVData();
            pubPriSVData.PrivateDescSeg1 = item.SrcOrderNo;
            pubPriSVData.PrivateDescSeg3 = item.LogisticsNo;
            MiscRcvDTO.PubPriDt = pubPriSVData;
            List<MiscRcvLineData> lines = new List<MiscRcvLineData>();
            int num = 10;
            foreach (var Detail in item.DetailList)
            {
                MiscRcvLineData rtnLine = new MiscRcvLineData();
                rtnLine.LineNum = num;
                rtnLine.ItemCode = Detail.GoodsNo;
                rtnLine.RcvQty = Convert.ToDecimal(Detail.Num);
                rtnLine.DeptCode = WH?.Department?.Code;
                rtnLine.RcvWH = item.WarehouseNo;
                rtnLine.StoreType = Detail.Defect ? 2 : 4;
                lines.Add(rtnLine);
                num += 10;
            }
            ShipPlanToShip.OrgCode = org.Code;
            ShipPlanToShip.EntCode = CHelper.EntCode;
            ShipPlanToShip.UserCode = "A001";
            ShipPlanToShip.OptType = CHelper.MiscRcvTransOptype;
            MiscRcvDTO.MiscRcvLineDt = lines;
            ShipPlanToShip.MiscRcvDTO = MiscRcvDTO;
            return JsonConvert.SerializeObject(ShipPlanToShip);
        }

        /// <summary>
        /// 奇门预入库创建U9杂收 cd
        /// </summary>
        /// <param name="misc"></param>
        /// <param name="SrcOrderNo"></param>
        /// <returns></returns>
        public static string  GetQMMiscRcvTranss(WdtWmsStockinPrestockinSearchResponse.OrderDomain item, Organization org)
        {
            Organization orga;
            PreStockinMiscRcvTransDto ShipPlanToShip = new PreStockinMiscRcvTransDto();
            MiscRcvSVData MiscRcvDTO = new MiscRcvSVData();
            Warehouse WH = Warehouse.FindByCode(org, item.WarehouseNo);
            MiscRcvDTO.BussinessDate = item.CheckTime != null ? CHelper.GetDateTimeMilliseconds(Convert.ToInt64(item.CheckTime)) : DateTime.Now.ToString();
            MiscRcvDTO.Status = 2;
            MiscRcvDTO.DocType = "MiscRcv004";
            MiscRcvDTO.DocNo = item.StockinNo;
            MiscRcvDTO.Memo = item.Remark;
            MiscRcvDTO.OrgCode = org.Code;
            MiscRcvDTO.WHMan = WH?.Manager?.Code;
            MiscRcvDTO.CreatedBy = item.OperatorName;
            PubPriSVData pubPriSVData = new PubPriSVData();
            pubPriSVData.PrivateDescSeg1 = item.SrcOrderNo;
            pubPriSVData.PrivateDescSeg3 = item.LogisticsNo;
            MiscRcvDTO.PubPriDt = pubPriSVData;
            List<MiscRcvLineData> lines = new List<MiscRcvLineData>();
            int num = 10;
            foreach (var Detail in item.DetailList)
            {
                orga = CHelper.GetOrg(Detail.GoodsNo,"");
                if (orga!=null && orga.ID==org.ID)
                {
                    MiscRcvLineData rtnLine = new MiscRcvLineData();
                    rtnLine.LineNum = num;
                    rtnLine.ItemCode = Detail.GoodsNo;
                    rtnLine.RcvQty = Convert.ToDecimal(Detail.Num);
                    rtnLine.DeptCode = WH?.Department?.Code;
                    rtnLine.RcvWH = item.WarehouseNo;
                    rtnLine.StoreType = Detail.Defect ? 2 : 4;
                    lines.Add(rtnLine);
                    num += 10;
                }
               
            }
            ShipPlanToShip.OrgCode = org.Code;
            ShipPlanToShip.EntCode = CHelper.EntCode;
            ShipPlanToShip.UserCode = "A001";
            ShipPlanToShip.OptType = CHelper.MiscRcvTransOptype;
            MiscRcvDTO.MiscRcvLineDt = lines;
            ShipPlanToShip.MiscRcvDTO = MiscRcvDTO;
            return JsonConvert.SerializeObject(ShipPlanToShip);
        }
        /// <summary>
        /// 应收单
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static string GetWdtAR(AfterSalesOrderDto item, Organization org)
        {

            ARBillDto ARBill = new ARBillDto();
            ARBillHeadSVDTO MiscRcvDTO = new ARBillHeadSVDTO();
            MiscRcvDTO.OALineNo = "10";
            MiscRcvDTO.Status = 2;
            MiscRcvDTO.AC = "C001";
            Customer customer = Customer.Finder.Find("Code=@Code and Org=@Org", new UFSoft.UBF.PL.OqlParam("Code", item.shop_no), new OqlParam("Org", org?.ID));
            if (customer == null)
            {
                customer = Customer.Finder.Find("DescFlexField.PrivateDescSeg11=@S1 and Org=@Org", new OqlParam("S1", item.shop_no), new OqlParam("Org", org?.ID));
            }
            MiscRcvDTO.AccrueCust = customer?.Code;
            MiscRcvDTO.AccrueDate = item.check_time;// CHelper.GetDateTimeMilliseconds(Convert.ToInt64(item.check_time)); ;
            MiscRcvDTO.BizOrg = org?.Code;
            MiscRcvDTO.DocNo = item.refund_no;
            MiscRcvDTO.DocumentType = "12";
            MiscRcvDTO.PayCust = customer?.Code;
            MiscRcvDTO.PayCustSite = customer?.Code;
            MiscRcvDTO.SettleOrg = org?.Code;
            MiscRcvDTO.SrcBillNum = "";
            MiscRcvDTO.SrcOrg = org?.Code;
            MiscRcvDTO.Memo = item.remark;
            PubPriSVData pubPriSVData = new PubPriSVData();
            pubPriSVData.PrivateDescSeg1 = item.refund_no;
            pubPriSVData.PrivateDescSeg2 = item.src_tids;
            pubPriSVData.PrivateDescSeg3 = item.refund_reason;
            MiscRcvDTO.PubPriDt = pubPriSVData;
            List<ARBillLineDTO> lines = new List<ARBillLineDTO>();
            int num = 10;
            foreach (var Detail in item.detail_list)
            {
                ARBillLineDTO rtnLine = new ARBillLineDTO();
                rtnLine.LineNum = num;
                rtnLine.Item = Detail.goods_no;
                rtnLine.Maturity = DateTime.Now.ToString();
                rtnLine.Money = Detail.total_amount;
                rtnLine.PUAmount = Detail.refund_num;
                rtnLine.ReceiveInvoiceDate = DateTime.Now.ToString();
                rtnLine.Memo = Detail.remark;
                lines.Add(rtnLine);
                num += 10;
            }
            ARBill.OrgCode = org.Code;
            ARBill.EntCode = CHelper.EntCode;
            ARBill.UserCode = "A001";
            ARBill.OptType = CHelper.AROptype;
            MiscRcvDTO.ARBillLines = lines;
            ARBill.ARBillDTO = MiscRcvDTO;
            return JsonConvert.SerializeObject(ARBill);
        }
        /// <summary>
        /// 退换单生成退回处理
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static string GetWdtRefundRMA(WdtAftersalesRefundRefundSearchResponse.OrderDomain item, Organization org)
        {
            NoSrcRMADto noSrcRMADto = new NoSrcRMADto();
            GenRMADto rmadto = new GenRMADto();
            SalePriceList priceList = null;
            rmadto.BusinessDate = item.Modified != null ? CHelper.GetDateTimeMilliseconds(Convert.ToInt64(item.Modified)) : DateTime.Now.ToString();
            rmadto.Status = "2";
            //Customer customer = Customer.Finder.Find("Name=@Name", new UFSoft.UBF.PL.OqlParam("Name", order.shop_no));         
            rmadto.DocumentType = "H0005";
            Customer customer = Customer.Finder.Find("Code=@Code and Org=@Org", new OqlParam(item.ShopNo), new OqlParam(org?.ID));  

            if (customer == null)
            {
                rmadto.Customer = item.ShopNo;
            }
            else
            {
                if (customer.CustomerCategory?.Code == "105")
                {
                    priceList = SalePriceList.Finder.Find("Code=@Code and Org = @org", new UFSoft.UBF.PL.OqlParam("Code", customer.PriceListCode), new OqlParam("org", org.ID));

                }
                rmadto.Customer = customer?.Code;
            }

            rmadto.Memo = item.Remark;
            // rmadto.Currency = item.
            rmadto.CreateBy = item.OperatorName;
            PubPriSVData pubPriSVData = new PubPriSVData();    
            pubPriSVData.PrivateDescSeg2 = item.RefundNo;
            pubPriSVData.PrivateDescSeg3 = item.ReturnLogisticsName;
            pubPriSVData.PrivateDescSeg4 = item.ReturnLogisticsNo;
            rmadto.PubPriDt = pubPriSVData;

            List<RMALineSVDTO> lines = new List<RMALineSVDTO>();
            int num = 10;
            decimal finalprice = 0;
            foreach (var detail in item.DetailList)
            {
                RMALineSVDTO rmaLine = new RMALineSVDTO();
                rmaLine.Qty = detail.RefundNum.ToString();
                rmaLine.WareHouse = item.ReturnWarehouseNo;
                ItemMaster   itemMaster = ItemMaster.Finder.Find("Code=@Code and Org=@Org",new OqlParam("Code",detail.GoodsNo),new OqlParam("Org",org.ID));
                if (itemMaster !=null)
                {
                    rmaLine.UOM = itemMaster.InventoryUOM ?.Name;
                }
               
                rmaLine.ItemInfo = detail.GoodsNo;
                rmaLine.DocLineNo = num.ToString();
                if (priceList != null)
                {
                    SalePriceLine salePriceLine = SalePriceLine.Finder.Find("SalePriceList= @SalePriceList and ItemInfo.ItemCode=@ItemCode", new OqlParam("SalePriceList", priceList.ID), new OqlParam("ItemCode", detail.GoodsNo));
                    rmaLine.Price = salePriceLine?.Price.ToString();
                }
                else
                {
                    finalprice = Convert.ToDecimal(detail.RefundAmount) / Convert.ToDecimal(rmaLine.Qty);
                    rmaLine.Price = finalprice.ToString();
                }

                lines.Add(rmaLine);
                num += 10;
            }
            rmadto.DocLine = lines;
            noSrcRMADto.RMAData = rmadto;
            noSrcRMADto.EntCode = CHelper.EntCode;
            noSrcRMADto.UserCode = "A001";
            noSrcRMADto.OrgCode = org?.Code;
            noSrcRMADto.OptType = CHelper.NoSrcRMAOptype;
            return JsonConvert.SerializeObject(noSrcRMADto);
        }
        /// <summary>
        /// 退换单生成退回处理
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static string GetWdtRefundRMAs(WdtAftersalesRefundRefundSearchResponse.OrderDomain item, Organization org)
        {
            NoSrcRMADto noSrcRMADto = new NoSrcRMADto();
            GenRMADto rmadto = new GenRMADto();
            SalePriceList priceList = null;
            rmadto.BusinessDate = item.Modified != null ? CHelper.GetDateTimeMilliseconds(Convert.ToInt64(item.Modified)) : DateTime.Now.ToString();
            rmadto.Status = "2";
            //Customer customer = Customer.Finder.Find("Name=@Name", new UFSoft.UBF.PL.OqlParam("Name", order.shop_no));         
            rmadto.DocumentType = "H0007";
            Customer customer = Customer.Finder.Find("Code=@Code and Org=@Org", new OqlParam(item.ShopNo), new OqlParam(org?.ID));

            if (customer == null)
            {
                rmadto.Customer = item.ShopNo;
            }
            else
            {
                if (customer.CustomerCategory?.Code == "105")
                {
                    priceList = SalePriceList.Finder.Find("Code=@Code and Org = @org", new UFSoft.UBF.PL.OqlParam("Code", customer.PriceListCode), new OqlParam("org", org.ID));

                }
                rmadto.Customer = customer?.Code;
            }

            rmadto.Memo = item.Remark;
            // rmadto.Currency = item.
            rmadto.CreateBy = item.OperatorName;
            PubPriSVData pubPriSVData = new PubPriSVData();
            pubPriSVData.PrivateDescSeg2 = item.RefundNo;
            pubPriSVData.PrivateDescSeg3 = item.ReturnLogisticsName;
            pubPriSVData.PrivateDescSeg4 = item.ReturnLogisticsNo;
            rmadto.PubPriDt = pubPriSVData;

            List<RMALineSVDTO> lines = new List<RMALineSVDTO>();
            int num = 10;
            decimal finalprice = 0;
            foreach (var detail in item.DetailList)
            {
                if (Convert.ToDecimal(detail.RefundAmount)>0)
                {
                    RMALineSVDTO rmaLine = new RMALineSVDTO();
                    rmaLine.Qty = detail.RefundNum.ToString();
                    rmaLine.WareHouse = item.ReturnWarehouseNo;
                    ItemMaster itemMaster = ItemMaster.Finder.Find("Code=@Code and Org=@Org", new OqlParam("Code", detail.GoodsNo), new OqlParam("Org", org.ID));
                    if (itemMaster != null)
                    {
                        rmaLine.UOM = itemMaster.InventoryUOM?.Name;
                    }

                    rmaLine.ItemInfo = detail.GoodsNo;
                    rmaLine.DocLineNo = num.ToString();
                    if (priceList != null)
                    {
                        SalePriceLine salePriceLine = SalePriceLine.Finder.Find("SalePriceList= @SalePriceList and ItemInfo.ItemCode=@ItemCode", new OqlParam("SalePriceList", priceList.ID), new OqlParam("ItemCode", detail.GoodsNo));
                        rmaLine.Price = salePriceLine?.Price.ToString();
                    }
                    else
                    {
                        finalprice = Convert.ToDecimal(detail.RefundAmount) / Convert.ToDecimal(rmaLine.Qty);
                        rmaLine.Price = finalprice.ToString();
                        rmaLine.OrderPrice = detail.Price;


                    }

                    lines.Add(rmaLine);
                    num += 10;
                }
               
            }
            if (lines.Count<=0)
            {
                return "";
            }
            rmadto.DocLine = lines;
            noSrcRMADto.RMAData = rmadto;
            noSrcRMADto.EntCode = CHelper.EntCode;
            noSrcRMADto.UserCode = "A001";
            noSrcRMADto.OrgCode = org?.Code;
            noSrcRMADto.OptType = CHelper.NoSrcRMAOptype;
            return JsonConvert.SerializeObject(noSrcRMADto);
        }
        /// <summary>
        /// 旺店通预入库创建U9杂收
        /// </summary>
        /// <param name="misc"></param>
        /// <param name="SrcOrderNo"></param>
        /// <returns></returns>
        public static string GetWdtMiscRcvTrans(StockinPreStockOrderData item, Organization org)
        {
            PreStockinMiscRcvTransDto ShipPlanToShip = new PreStockinMiscRcvTransDto();
            MiscRcvSVData MiscRcvDTO = new MiscRcvSVData();
            Warehouse WH = Warehouse.FindByCode(org, item.warehouse_no);
            MiscRcvDTO.BussinessDate = DateTime.Now.ToString();
            MiscRcvDTO.Status = 2;
            MiscRcvDTO.DocType = "MiscRcv004";//
            MiscRcvDTO.DocNo = item.src_order_no;
            MiscRcvDTO.Memo = item.remark;
            MiscRcvDTO.OrgCode = org.Code;
            MiscRcvDTO.WHMan = WH?.Manager?.Code;
            PubPriSVData pubPriSVData = new PubPriSVData();
            pubPriSVData.PrivateDescSeg1 = item.stockin_no;
            MiscRcvDTO.PubPriDt = pubPriSVData;
            List<MiscRcvLineData> lines = new List<MiscRcvLineData>();
            int num = 10;
            foreach (var Detail in item.detail_list)
            {
                MiscRcvLineData rtnLine = new MiscRcvLineData();
                rtnLine.LineNum = num;
                rtnLine.ItemCode = Detail.goods_no;
                rtnLine.RcvQty = Convert.ToDecimal(Detail.num);
                rtnLine.DeptCode = WH?.Department?.Code;
                rtnLine.RcvWH = item.warehouse_no;
                rtnLine.StoreType = 4;
                lines.Add(rtnLine);
                num += 10;
            }
            ShipPlanToShip.OrgCode = org.Code;
            MiscRcvDTO.MiscRcvLineDt = lines;
            ShipPlanToShip.EntCode = CHelper.EntCode;
            ShipPlanToShip.UserCode = "A001";
            ShipPlanToShip.OptType = CHelper.MiscRcvTransOptype;
            ShipPlanToShip.MiscRcvDTO = MiscRcvDTO;
            return JsonConvert.SerializeObject(ShipPlanToShip);
        }
        /// <summary>
        /// 奇门有来源出货退回处理创建
        /// </summary>
        /// <param name="misc"></param>
        /// <param name="SrcOrderNo"></param>
        /// <returns></returns>
        public static string GetSrcShipRMAQM(WdtWmsStockinRefundQuerywithdetailResponse.OrderDomain item, Ship ship, string OrgCode, List<DetailListDomain> Order)
        {
            SrcShipRMA SrcShipToRtn = new SrcShipRMA();

            List<SrcShipRMALineDTO> lines = new List<SrcShipRMALineDTO>();
            foreach (ShipLine line in ship.ShipLines)
            {
                foreach (var Detail in item.DetailsList)
                {
                    if (!string.IsNullOrEmpty(line.DescFlexField.PrivateDescSeg8))
                    {
                        if (line.DescFlexField.PrivateDescSeg8 == Detail.GoodsNo)
                        {
                            foreach (DetailListDomain  pee in Order)
                            {
                                if (Detail.GoodsNo == pee.GoodsNo && Convert.ToDecimal(Detail.StockInNum)== Convert.ToDecimal(pee.RefundNum))
                                {
                                    if (Convert.ToDecimal(pee.RefundAmount) == 0m)
                                    {
                                        if (line.DonationType != null && (line.DonationType.Value == 0 || line.DonationType.Value == 1))
                                        {
                                            SrcShipRMALineDTO rtnLine = new SrcShipRMALineDTO();
                                            rtnLine.DocLine = line.DocLineNo.ToString();
                                            rtnLine.SrcDocNo = ship.DocNo;
                                            rtnLine.Num = Convert.ToDecimal(Detail.Num);
                                            rtnLine.DocNo = item.OrderNo;
                                            rtnLine.DocType = "H0004"; 
                                            rtnLine.CreateBy = ship.CreatedBy;
                                            rtnLine.WHCode = item.WarehouseNo;
                                            rtnLine.LogisticsName = item.LogisticsName;
                                            rtnLine.LogisticsNo = item.LogisticsNo;
                                            rtnLine.BusinessDate = item.CheckTime != null ? CHelper.GetDateTimeMilliseconds(Convert.ToInt64(item.CheckTime)) : DateTime.Now.ToString();
                                            rtnLine.Defect =Convert.ToBoolean(Detail.Defect)?true:false;
                                            rtnLine.OrderNo = item.TradeNoList;
                                            lines.Add(rtnLine);
                                        }
                                    }
                                    else if (Convert.ToDecimal(pee.RefundAmount) > 0)
                                    {
                                        if (line.DonationType == null || (line.DonationType != null && line.DonationType.Value != 0 && line.DonationType.Value != 1))
                                        {
                                            SrcShipRMALineDTO rtnLine = new SrcShipRMALineDTO();
                                            rtnLine.DocLine = line.DocLineNo.ToString();
                                            rtnLine.SrcDocNo = ship.DocNo;
                                            rtnLine.Num = Convert.ToDecimal(Detail.Num);
                                            rtnLine.DocNo = item.OrderNo;
                                            rtnLine.DocType = "H0004";
                                            rtnLine.WHCode = item.WarehouseNo;
                                            rtnLine.CreateBy = ship.CreatedBy;
                                            rtnLine.LogisticsName = item.LogisticsName;
                                            rtnLine.LogisticsNo = item.LogisticsNo;
                                            rtnLine.BusinessDate = item.CheckTime != null ? CHelper.GetDateTimeMilliseconds(Convert.ToInt64(item.CheckTime)) : DateTime.Now.ToString();
                                            rtnLine.Defect = Convert.ToBoolean(Detail.Defect) ? true : false;
                                            rtnLine.OrderNo = item.TradeNoList;
                                            lines.Add(rtnLine);
                                        }
                                    }
                                }
                            }
                         
                           
                        }
                    }
                    else
                    {
                        if (line.ItemInfo.ItemCode == Detail.GoodsNo)
                        {
                            foreach (DetailListDomain pee in Order)
                            {
                                if (Detail.GoodsNo == pee.GoodsNo && Convert.ToDecimal(Detail.StockInNum) == Convert.ToDecimal(pee.RefundNum))
                                {
                                    if (Convert.ToDecimal(pee.RefundAmount) == 0m)
                                    {
                                        if (line.DonationType != null && (line.DonationType.Value == 0 || line.DonationType.Value == 1))
                                        {
                                            SrcShipRMALineDTO rtnLine = new SrcShipRMALineDTO();
                                            rtnLine.DocLine = line.DocLineNo.ToString();
                                            rtnLine.SrcDocNo = ship.DocNo;
                                            rtnLine.Num = Convert.ToDecimal(Detail.Num);
                                            rtnLine.DocNo = item.OrderNo;
                                            rtnLine.DocType = "H0004";
                                            rtnLine.WHCode = item.WarehouseNo;
                                            rtnLine.CreateBy = ship.CreatedBy;
                                            rtnLine.LogisticsName = item.LogisticsName;
                                            rtnLine.LogisticsNo = item.LogisticsNo;
                                            rtnLine.Defect = Convert.ToBoolean(Detail.Defect) ? true : false;
                                            rtnLine.BusinessDate = item.CheckTime != null ? CHelper.GetDateTimeMilliseconds(Convert.ToInt64(item.CheckTime)) : DateTime.Now.ToString();
                                            rtnLine.OrderNo = item.TradeNoList;
                                            lines.Add(rtnLine);
                                        }
                                    }
                                    else if (Convert.ToDecimal(pee.RefundAmount) > 0)
                                    {
                                        if (line.DonationType == null || (line.DonationType != null && line.DonationType.Value != 0 && line.DonationType.Value != 1))
                                        {
                                            SrcShipRMALineDTO rtnLine = new SrcShipRMALineDTO();
                                            rtnLine.DocLine = line.DocLineNo.ToString();
                                            rtnLine.SrcDocNo = ship.DocNo;
                                            rtnLine.Num = Convert.ToDecimal(Detail.Num);
                                            rtnLine.DocNo = item.OrderNo;
                                            rtnLine.DocType = "H0004";
                                            rtnLine.WHCode = item.WarehouseNo;
                                            rtnLine.CreateBy = ship.CreatedBy;
                                            rtnLine.LogisticsName = item.LogisticsName;
                                            rtnLine.LogisticsNo = item.LogisticsNo;
                                            rtnLine.Defect = Convert.ToBoolean(Detail.Defect) ? true : false;
                                            rtnLine.BusinessDate = item.CheckTime != null ? CHelper.GetDateTimeMilliseconds(Convert.ToInt64(item.CheckTime)) : DateTime.Now.ToString();
                                            rtnLine.OrderNo = item.TradeNoList;
                                            lines.Add(rtnLine);
                                        }
                                    }

                                }


                            }
                            
                        }
                    }
                  


                }
            }
            if (lines.Count <= 0)
            {
                return "";
            }
            SrcShipToRtn.ShipData = lines;
            SrcShipToRtn.OrgCode = OrgCode;
            SrcShipToRtn.EntCode = CHelper.EntCode;
            SrcShipToRtn.UserCode = "A001";
            SrcShipToRtn.OptType = CHelper.RMAOptype;
            return JsonConvert.SerializeObject(SrcShipToRtn);
        }
        public static string GetSrcShipRMAQMs(WdtWmsStockinRefundQuerywithdetailResponse.OrderDomain item, Ship ship, string OrgCode, List<DetailListDomain> Order)
        {
            SrcShipRMA SrcShipToRtn = new SrcShipRMA();

            List<SrcShipRMALineDTO> lines = new List<SrcShipRMALineDTO>();
            foreach (ShipLine line in ship.ShipLines)
            {
                foreach (var Detail in item.DetailsList)
                {
                    if (!string.IsNullOrEmpty(line.DescFlexField.PrivateDescSeg8))
                    {
                        if (line.DescFlexField.PrivateDescSeg8 == Detail.GoodsNo)
                        {
                            foreach (DetailListDomain pee in Order)
                            {
                                if (Detail.GoodsNo == pee.GoodsNo && Convert.ToDecimal(Detail.StockInNum) == Convert.ToDecimal(pee.RefundNum))
                                {
                                    if (pee.Oid?.Substring(0,20)==line.SrcDocNo && line.SrcDocLineNo.ToString() == pee.Oid?.Substring(26))
                                    {
                                        SrcShipRMALineDTO rtnLine = new SrcShipRMALineDTO();
                                        rtnLine.DocLine = line.DocLineNo.ToString();
                                        rtnLine.SrcDocNo = ship.DocNo;
                                        rtnLine.Num = Convert.ToDecimal(Detail.Num);
                                        rtnLine.DocNo = item.OrderNo;
                                        rtnLine.DocType = "H0004";
                                        rtnLine.CreateBy = ship.CreatedBy;
                                        rtnLine.WHCode = item.WarehouseNo;
                                        rtnLine.LogisticsName = item.LogisticsName;
                                        rtnLine.LogisticsNo = item.LogisticsNo;
                                        rtnLine.BusinessDate = item.CheckTime != null ? CHelper.GetDateTimeMilliseconds(Convert.ToInt64(item.CheckTime)) : DateTime.Now.ToString();
                                        rtnLine.Defect = Convert.ToBoolean(Detail.Defect) ? true : false;
                                        rtnLine.OrderNo = item.TradeNoList;
                                        lines.Add(rtnLine);
                                    }
                                     
                                }
                            }


                        }
                    }
                    else
                    {
                        if (line.ItemInfo.ItemCode == Detail.GoodsNo)
                        {
                            foreach (DetailListDomain pee in Order)
                            {
                                if (Detail.GoodsNo == pee.GoodsNo && Convert.ToDecimal(Detail.StockInNum) == Convert.ToDecimal(pee.RefundNum))
                                {
                                    if (pee.Oid?.Substring(0, 20) == line.SrcDocNo && line.SrcDocLineNo.ToString() == pee.Oid?.Substring(26))
                                    {
                                        SrcShipRMALineDTO rtnLine = new SrcShipRMALineDTO();
                                        rtnLine.DocLine = line.DocLineNo.ToString();
                                        rtnLine.SrcDocNo = ship.DocNo;
                                        rtnLine.Num = Convert.ToDecimal(Detail.Num);
                                        rtnLine.DocNo = item.OrderNo;
                                        rtnLine.DocType = "H0004";
                                        rtnLine.WHCode = item.WarehouseNo;
                                        rtnLine.CreateBy = ship.CreatedBy;
                                        rtnLine.LogisticsName = item.LogisticsName;
                                        rtnLine.LogisticsNo = item.LogisticsNo;
                                        rtnLine.Defect = Convert.ToBoolean(Detail.Defect) ? true : false;
                                        rtnLine.BusinessDate = item.CheckTime != null ? CHelper.GetDateTimeMilliseconds(Convert.ToInt64(item.CheckTime)) : DateTime.Now.ToString();
                                        rtnLine.OrderNo = item.TradeNoList;
                                        lines.Add(rtnLine);
                                    }

                                }


                            }

                        }
                    }



                }
            }

            SrcShipToRtn.ShipData = lines;
            SrcShipToRtn.OrgCode = OrgCode;
            SrcShipToRtn.EntCode = CHelper.EntCode;
            SrcShipToRtn.UserCode = "A001";
            SrcShipToRtn.OptType = CHelper.RMAOptype;
            return JsonConvert.SerializeObject(SrcShipToRtn);
        }
        /// <summary>
        /// 奇门有来源出货退回处理创建-历史入库单查询
        /// </summary>
        /// <param name="misc"></param>
        /// <param name="SrcOrderNo"></param>
        /// <returns></returns>
        public static string  GetSrcShipRMAQMFromHis(StockinRefundOrderData item, Ship ship, string OrgCode)
        {
            SrcShipRMA SrcShipToRtn = new SrcShipRMA();

            List<SrcShipRMALineDTO> lines = new List<SrcShipRMALineDTO>();
            foreach (ShipLine line in ship.ShipLines)
            {
                foreach (var Detail in item.details_list)
                {
                    if (!string.IsNullOrEmpty(line.DescFlexField.PrivateDescSeg8))
                    {
                        if (line.DescFlexField.PrivateDescSeg8 == Detail.goods_no)
                        {
                            if (Convert.ToBoolean(Detail.defect))
                            {
                                if (line.DonationType != null && (line.DonationType.Value == 0 || line.DonationType.Value == 1))
                                {
                                    SrcShipRMALineDTO rtnLine = new SrcShipRMALineDTO();
                                    rtnLine.DocLine = line.DocLineNo.ToString();
                                    rtnLine.SrcDocNo = ship.DocNo;
                                    rtnLine.Num = Convert.ToDecimal(Detail.num);
                                    rtnLine.DocNo = item.order_no;
                                    rtnLine.DocType = "H0004";
                                    rtnLine.CreateBy = ship.CreatedBy;
                                    rtnLine.WHCode = item.warehouse_no;
                                    rtnLine.LogisticsName = item.logistics_name;
                                    rtnLine.LogisticsNo = item.logistics_no;
                                    rtnLine.Defect = true;
                                    lines.Add(rtnLine);
                                }
                            }
                            else if (!Convert.ToBoolean(Detail.defect))
                            {
                                if (line.DonationType == null || (line.DonationType != null && line.DonationType.Value != 0 && line.DonationType.Value != 1))
                                {
                                    SrcShipRMALineDTO rtnLine = new SrcShipRMALineDTO();
                                    rtnLine.DocLine = line.DocLineNo.ToString();
                                    rtnLine.SrcDocNo = ship.DocNo;
                                    rtnLine.Num = Convert.ToDecimal(Detail.num);
                                    rtnLine.DocNo = item.order_no;
                                    rtnLine.DocType = "H0004";
                                    rtnLine.WHCode = item.warehouse_no;
                                    rtnLine.CreateBy = ship.CreatedBy;
                                    rtnLine.LogisticsName = item.logistics_name;
                                    rtnLine.LogisticsNo = item.logistics_no;
                                    rtnLine.Defect = false;
                                    lines.Add(rtnLine);
                                }
                            }

                        }
                    }
                    else
                    {
                        if (line.ItemInfo.ItemCode == Detail.goods_no)
                        {
                            if (Convert.ToBoolean(Detail.defect))
                            {
                                if (line.DonationType != null && (line.DonationType.Value == 0 || line.DonationType.Value == 1))
                                {
                                    SrcShipRMALineDTO rtnLine = new SrcShipRMALineDTO();
                                    rtnLine.DocLine = line.DocLineNo.ToString();
                                    rtnLine.SrcDocNo = ship.DocNo;
                                    rtnLine.Num = Convert.ToDecimal(Detail.num);
                                    rtnLine.DocNo = item.order_no;
                                    rtnLine.DocType = "H0004";
                                    rtnLine.WHCode = item.warehouse_no;
                                    rtnLine.CreateBy = ship.CreatedBy;
                                    rtnLine.LogisticsName = item.logistics_name;
                                    rtnLine.LogisticsNo = item.logistics_no;
                                    lines.Add(rtnLine);
                                }
                            }
                            else if (!Convert.ToBoolean(Detail.defect))
                            {
                                if (line.DonationType == null || (line.DonationType != null && line.DonationType.Value != 0 && line.DonationType.Value != 1))
                                {
                                    SrcShipRMALineDTO rtnLine = new SrcShipRMALineDTO();
                                    rtnLine.DocLine = line.DocLineNo.ToString();
                                    rtnLine.SrcDocNo = ship.DocNo;
                                    rtnLine.Num = Convert.ToDecimal(Detail.num);
                                    rtnLine.DocNo = item.order_no;
                                    rtnLine.DocType = "H0004";
                                    rtnLine.WHCode = item.warehouse_no;
                                    rtnLine.CreateBy = ship.CreatedBy;
                                    rtnLine.LogisticsName = item.logistics_name;
                                    rtnLine.LogisticsNo = item.logistics_no;
                                    lines.Add(rtnLine);
                                }
                            }
                        }
                    }



                }
            }

            SrcShipToRtn.ShipData = lines;
            SrcShipToRtn.OrgCode = OrgCode;
            SrcShipToRtn.EntCode = CHelper.EntCode;
            SrcShipToRtn.UserCode = "A001";
            SrcShipToRtn.OptType = CHelper.RMAOptype;
            return JsonConvert.SerializeObject(SrcShipToRtn);
        }
        /// <param name="transfer"></param>
        /// <returns></returns>
        public static string GetEditTransferJson(TransferIn transfer)
        {
            List<object> list = new List<object>();
            bool is_check = true;
            StockTransferEditDto miscShipment = new StockTransferEditDto();
            miscShipment.outer_no = transfer.DocNo;
            miscShipment.from_warehouse_no = transfer.TransInLines[0] != null && transfer.TransInLines[0].TransInSubLines[0] != null && transfer.TransInLines[0].TransInSubLines[0].TransOutWh != null ? transfer.TransInLines[0].TransInSubLines[0].TransOutWh.Code : "";
            miscShipment.to_warehouse_no = transfer.TransInLines != null && transfer.TransInLines[0].TransInWh != null ? transfer.TransInLines[0].TransInWh.Code : ""; ;
            miscShipment.remark = transfer.Memo;
            List<StockTransferEditDetailDto> MiscShipLines = new List<StockTransferEditDetailDto>();
            foreach (TransInLine item in transfer.TransInLines)
            {
                StockTransferEditDetailDto MiscShipLine = new StockTransferEditDetailDto();
                ItemMaster itemMaster = ItemMaster.Finder.FindByID(item.ItemInfo.ItemID.ID);
                if (itemMaster != null)
                {
                    MiscShipLine.spec_no = itemMaster.Code1;
                }

                MiscShipLine.num = Convert.ToInt32(item.StoreUOMQty);
                MiscShipLines.Add(MiscShipLine);
            }
            list.Add(miscShipment);
            list.Add(MiscShipLines);
            list.Add(is_check);
            return JsonConvert.SerializeObject(list);
        }
        /// <summary>
        /// 创建无来源退回处理
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static string GetRMAQM(WdtWmsStockinRefundQuerywithdetailResponse.OrderDomain item, Organization org,bool AttachType=false)
        {
            Organization orga;
            NoSrcRMADto noSrcRMADto = new NoSrcRMADto();
            GenRMADto rmadto = new GenRMADto();
            SalePriceList priceList = null;
            rmadto.BusinessDate = !string.IsNullOrEmpty(item.Modified) ? item.Modified : DateTime.Now.ToString();
            rmadto.Status = "2";
            //Customer customer = Customer.Finder.Find("Name=@Name", new UFSoft.UBF.PL.OqlParam("Name", order.shop_no));         
            rmadto.DocumentType = "H0004";
             Customer customer = Customer.Finder.Find("Code=@Code and Org=@Org",new OqlParam(item.ShopNo.Trim()),new OqlParam(org?.ID));//1.店铺匹配客户的为普通平台单和本正客户 2.匹配值集和分销商的为平台单中带分销商；

            if (customer == null)
            {
                DefineValue dv = DefineValue.Finder.Find("ValueSetDef.Code = @ZJCode and Code = @Code", new OqlParam("ZJCode", "1033"), new OqlParam("Code", item.ShopNo));
                if (!string.IsNullOrEmpty(item.FenxiaoNick) && dv!=null)
                {
                    customer = Customer.Finder.Find("Name=@Name", new UFSoft.UBF.PL.OqlParam("Name", item.FenxiaoNick));
                }
                if (customer!=null)
                {
                    rmadto.Customer = customer?.Code;
                }
                else
                {
                    rmadto.Customer = item.ShopNo;
                }
            }
            else
            {
                if (customer.CustomerCategory?.Code == "105")
                {
                    priceList = SalePriceList.Finder.Find("Code=@Code and Org = @org", new UFSoft.UBF.PL.OqlParam("Code", customer.PriceListCode), new OqlParam("org", org.ID));

                }
                rmadto.Customer = customer?.Code;
            }

            rmadto.Memo = item.Remark;
            // rmadto.Currency = item.
            rmadto.CreateBy = "";
            PubPriSVData pubPriSVData = new PubPriSVData();
            pubPriSVData.PrivateDescSeg1 = item.TradeNoList;
            pubPriSVData.PrivateDescSeg2 = item.RefundNo;
            pubPriSVData.PrivateDescSeg8 = item.OrderNo;
            pubPriSVData.PrivateDescSeg3 = item.LogisticsName;
            pubPriSVData.PrivateDescSeg4 = item.LogisticsNo;
            pubPriSVData.PrivateDescSeg9 = AttachType? "true":"";//记录关联类型不为空,赋值关联预入库
            rmadto.PubPriDt = pubPriSVData;

            List<RMALineSVDTO> lines = new List<RMALineSVDTO>();
            int num = 10;
            decimal finalprice = 0;
            foreach (var detail in item.DetailsList)
            {
                orga = CHelper.GetOrg(detail.GoodsNo, "");
                if (orga != null && orga.ID == org.ID)
                {
                    RMALineSVDTO rmaLine = new RMALineSVDTO();
                    PubPriSVData pubPriDt = new PubPriSVData();
                    pubPriDt.PrivateDescSeg4 = Convert.ToBoolean(detail.Defect) ? "2" : "1";
                    rmaLine.Qty = detail.StockInNum;
                    rmaLine.WareHouse = item.WarehouseNo;
                    rmaLine.UOM = detail.GoodsUnit;
                    rmaLine.ItemInfo = detail.GoodsNo;
                    rmaLine.DocLineNo = num.ToString();
                    if (priceList != null)
                    {
                        SalePriceLine salePriceLine = SalePriceLine.Finder.Find("SalePriceList= @SalePriceList and ItemInfo.ItemCode=@ItemCode", new OqlParam("SalePriceList", priceList.ID), new OqlParam("ItemCode", detail.GoodsNo));
                        rmaLine.Price = salePriceLine?.Price.ToString();
                    }
                    else
                    {
                        if (Convert.ToDecimal(rmaLine.Qty) > 0)
                        {
                            finalprice = Convert.ToDecimal(detail.RefundAmount) / Convert.ToDecimal(rmaLine.Qty);
                        }

                        rmaLine.Price = finalprice.ToString();
                    }
                    rmaLine.PubPriDt = pubPriDt;
                    lines.Add(rmaLine);
                    num += 10;
                }
              
            }
            rmadto.DocLine = lines;
            noSrcRMADto.RMAData = rmadto;
            noSrcRMADto.EntCode = CHelper.EntCode;
            noSrcRMADto.UserCode = "A001";
            noSrcRMADto.OrgCode = org?.Code;
            noSrcRMADto.OptType = CHelper.NoSrcRMAOptype;
            return JsonConvert.SerializeObject(noSrcRMADto);

        }
        /// <summary>
        /// 创建无来源退回处理-历史入库单查询
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static string  GetRMAQMFromHis(StockinRefundOrderData item, Organization org)
        {
            NoSrcRMADto noSrcRMADto = new NoSrcRMADto();
            GenRMADto rmadto = new GenRMADto();
            SalePriceList priceList = null;
            rmadto.BusinessDate = DateTime.Now.ToString();
            rmadto.Status = "2";
            //Customer customer = Customer.Finder.Find("Name=@Name", new UFSoft.UBF.PL.OqlParam("Name", order.shop_no));         
            rmadto.DocumentType = "H0004";
            Customer customer = Customer.FindByCode(org, item.shop_no);//1.店铺匹配客户的为普通平台单和本正客户 2.匹配值集和分销商的为平台单中带分销商；

            if (customer == null)
            {
                DefineValue dv = DefineValue.Finder.Find("ValueSetDef.Code = @ZJCode and Code = @Code", new OqlParam("ZJCode", "1033"), new OqlParam("Code", item.shop_no));
                if (!string.IsNullOrEmpty(item.fenxiao_nick) && dv != null)
                {
                    customer = Customer.Finder.Find("Name=@Name", new UFSoft.UBF.PL.OqlParam("Name", item.fenxiao_nick));
                }
                rmadto.Customer = customer?.Code;
            }
            else
            {
                if (customer.CustomerCategory?.Code == "105")
                {
                    priceList = SalePriceList.Finder.Find("Code=@Code and Org = @org", new UFSoft.UBF.PL.OqlParam("Code", customer.PriceListCode), new OqlParam("org", org.ID));

                }
                rmadto.Customer = customer?.Code;
            }

            rmadto.Memo = item.remark;
            // rmadto.Currency = item.
            rmadto.CreateBy = "";
            PubPriSVData pubPriSVData = new PubPriSVData();
            //pubPriSVData.PrivateDescSeg1 =  ;
            pubPriSVData.PrivateDescSeg2 = item.refund_no;
            pubPriSVData.PrivateDescSeg8 = item.order_no;
            pubPriSVData.PrivateDescSeg3 = item.logistics_name;
            pubPriSVData.PrivateDescSeg4 = item.logistics_no;
            rmadto.PubPriDt = pubPriSVData;

            List<RMALineSVDTO> lines = new List<RMALineSVDTO>();
            int num = 10;
            decimal finalprice = 0;
            foreach (var detail in item.details_list)
            {
                RMALineSVDTO rmaLine = new RMALineSVDTO();
                PubPriSVData pubPriDt = new PubPriSVData();
                pubPriDt.PrivateDescSeg4 = Convert.ToBoolean(detail.defect) ? "2" : "1";
                rmaLine.Qty = detail.stockin_num.ToString();
                rmaLine.WareHouse = item.warehouse_no;
                rmaLine.UOM = detail.goods_unit;
                rmaLine.ItemInfo = detail.goods_no;
                rmaLine.DocLineNo = num.ToString();
                if (priceList != null)
                {
                    SalePriceLine salePriceLine = SalePriceLine.Finder.Find("SalePriceList= @SalePriceList and ItemInfo.ItemCode=@ItemCode", new OqlParam("SalePriceList", priceList.ID), new OqlParam("ItemCode", detail.goods_no));
                    rmaLine.Price = salePriceLine?.Price.ToString();
                }
                else
                {
                    if (Convert.ToDecimal(rmaLine.Qty) > 0)
                    {
                        finalprice = Convert.ToDecimal(detail.refund_amount) / Convert.ToDecimal(rmaLine.Qty);
                    }

                    rmaLine.Price = finalprice.ToString();
                }
                rmaLine.PubPriDt = pubPriDt;
                lines.Add(rmaLine);
                num += 10;
            }
            rmadto.DocLine = lines;
            noSrcRMADto.RMAData = rmadto;
            noSrcRMADto.EntCode = CHelper.EntCode;
            noSrcRMADto.UserCode = "A001";
            noSrcRMADto.OrgCode = org?.Code;
            noSrcRMADto.OptType = CHelper.NoSrcRMAOptype;
            return JsonConvert.SerializeObject(noSrcRMADto);

        }
        /// <summary>
        /// 旺店通有来源出货退回处理创建
        /// </summary>
        /// <param name="misc"></param>
        /// <param name="SrcOrderNo"></param>
        /// <returns></returns>
        public static string GetSrcShipRMAWdt(StockinRefundOrderData item, Ship ship, string OrgCode)
        {
            SrcShipRMA SrcShipToRtn = new SrcShipRMA();

            List<SrcShipRMALineDTO> lines = new List<SrcShipRMALineDTO>();
            foreach (ShipLine line in ship.ShipLines)
            {
                foreach (var Detail in item.details_list)
                {

                    if (line.ItemInfo.ItemCode == Detail.goods_no)
                    {
                        SrcShipRMALineDTO rtnLine = new SrcShipRMALineDTO();
                        rtnLine.DocLine = line.DocLineNo.ToString();
                        rtnLine.SrcDocNo = ship.DocNo;
                        rtnLine.Num = Convert.ToDecimal(Detail.stockin_num);
                        rtnLine.DocNo = item.order_no;
                        rtnLine.DocType = "H0004";
                        lines.Add(rtnLine);
                    }


                }
            }

            SrcShipToRtn.ShipData = lines;
            SrcShipToRtn.EntCode = CHelper.EntCode;
            SrcShipToRtn.UserCode = "A001";
            SrcShipToRtn.OrgCode = OrgCode;
            SrcShipToRtn.OptType = CHelper.RMAOptype;
            return JsonConvert.SerializeObject(SrcShipToRtn);
        }
        /// <summary>
        /// 出货计划创建原始订单拆行
        /// </summary>
        /// <param name="misc"></param>
        /// <param name="lines"></param>
        /// <param name="trate"></param>
        /// <returns></returns>
        public static string GetShipPlan(ShipPlan misc, List<ShipPlanLine> lines, string trate)
        {
            List<object> list = new List<object>();
            string shop_no = lines[0].OrderBy.Code;
            decimal TotalMny = 0;
            List<rawTradeOrderReq> rawTradeOrderReqs = new List<rawTradeOrderReq>();
            rawTradeOrderReq rawTradeOrderReq = new rawTradeOrderReq();
            rawTradeOrderReq.tid = misc.DocNo + trate;
            rawTradeOrderReq.order_count = lines.GroupBy(x => x.ItemInfo.ItemCode).Count();
            rawTradeOrderReq.goods_count = lines.Sum(x => x.PlanQtyPU);
            rawTradeOrderReq.pay_method = 1; //待确认 支付方式
            rawTradeOrderReq.trade_time = misc.BusinessDate.ToString();
            rawTradeOrderReq.buyer_nick = lines[0].OrderBy.Code;//待确认
            rawTradeOrderReq.receiver_name = "";//待确认
            rawTradeOrderReq.end_time = null;
            rawTradeOrderReq.receiver_area = "";
            rawTradeOrderReq.receiver_address = "";
            rawTradeOrderReq.post_amount = 0;
            rawTradeOrderReq.delivery_term = 0;//待确认
            rawTradeOrderReq.warehouse_no = lines[0].WH != null ? lines[0].WH.Code : "";//待确认 行数据          
            List<rawTradeOrderListReq> listSpec = new List<rawTradeOrderListReq>();
            foreach (ShipPlanLine shipPlanLine in lines)
            {
                rawTradeOrderListReq specInfoReq = new rawTradeOrderListReq();
                specInfoReq.tid = misc.DocNo + trate;
                specInfoReq.oid = Tools.GenerateId().ToString();
                specInfoReq.status = 30;
                specInfoReq.refund_status = 0;
                specInfoReq.goods_id = lines[0].ItemInfo.ItemCode;
                ItemMaster itemMaster = ItemMaster.Finder.FindByID(shipPlanLine.ItemInfo.ItemID.ID);
                if (itemMaster != null)
                {
                    specInfoReq.spec_id = itemMaster.Code1;
                }
                specInfoReq.goods_no = itemMaster.Code;
                specInfoReq.spec_no = itemMaster.SPECS;
                specInfoReq.goods_name = itemMaster.Name;
                specInfoReq.num = shipPlanLine.PlanQtyPU;
                SOLine soline = SOLine.Finder.FindByID(shipPlanLine.SrcDocLine);
                if (soline != null)
                {
                    specInfoReq.price = soline.FinallyPriceTC;
                    specInfoReq.total_amount = soline.FinallyPriceTC * shipPlanLine.PlanQtyPU;
                    specInfoReq.discount = soline.DiscountRate;
                    specInfoReq.share_discount = soline.DiscountAC;
                    specInfoReq.adjust_amount = soline.DiscountAC;
                    TotalMny += specInfoReq.total_amount;
                }
                specInfoReq.refund_amount = 0;
                specInfoReq.remark = "";
                specInfoReq.json = "";
                listSpec.Add(specInfoReq);
            }
            rawTradeOrderReq.receivable = TotalMny;//统计的是价税合计，行上的数据
            rawTradeOrderReqs.Add(rawTradeOrderReq);
            list.Add(shop_no);
            list.Add(rawTradeOrderReqs);
            list.Add(listSpec);
            return JsonConvert.SerializeObject(list);
        }
        /// <summary>
        /// 委外退料创建
        /// </summary>
        /// <param name="rcv"></param>
        /// <returns></returns>
        public static string GetRtnIssue(Receivement rcv)
        {
 
            RtnRcvToPMIssue ShipPlanToShip = new RtnRcvToPMIssue();
            PMIssueDTO shipPlanToShipDTO = new PMIssueDTO();
            if (rcv.RcvLines[0].ConfirmDate!=null)
            {
                shipPlanToShipDTO.IssueDate = rcv.RcvLines[0].ConfirmDate.ToString();
            }
           
            shipPlanToShipDTO.Status = 2;
            shipPlanToShipDTO.DocType ="0";


            PubPriSVData pubPriSVData = new PubPriSVData();
            pubPriSVData.PrivateDescSeg1 = rcv.DocNo;
            shipPlanToShipDTO.PubPriDt = pubPriSVData;
            int num = 10;
            List<PMIssueLineData>   lines = new List<PMIssueLineData>();
            foreach (RcvLine item in rcv.RcvLines)
            {
                POLine  Poline = POLine.Finder.FindByID(item.SrcPO.SrcDocLine.EntityID);
                if (Poline != null)
                {
                    PMIssueLineData issueLineData = new PMIssueLineData();
                    issueLineData.LineNum = num.ToString();
                    issueLineData.SrcDocNo = Poline.PurchaseOrder.DocNo;
                    issueLineData.SrcLineNo = Poline.DocLineNo.ToString();
                    issueLineData.IssueQty = item.RejectQtyTU;
                    issueLineData.IssueWH = item.Wh?.Code;
                    issueLineData.SrcLineID = item.SrcDoc.SrcDocLine!=null? item.SrcDoc.SrcDocLine.EntityID:0;
                    lines.Add(issueLineData);
                }
                
                num += 10;
            }
            ShipPlanToShip.EntCode = CHelper.EntCode;           
            ShipPlanToShip.UserCode = rcv.CreatedBy.Contains("admin")?"A001": rcv.CreatedBy;            
            ShipPlanToShip.OrgCode = rcv.Org.Code;
            ShipPlanToShip.OptType = CHelper.RtnPMOptype;
            shipPlanToShipDTO.PMIssueLineDTO = lines;
            ShipPlanToShip.RtnPMIssueDTO = shipPlanToShipDTO;
            return JsonConvert.SerializeObject(ShipPlanToShip);
        }
        /// <summary>
        /// 委外退料审核
        /// </summary>
        /// <param name="DocNo"></param>
        ///  /// <param name="rcv"></param>
        /// <returns></returns>
        public static string ApproveRtnIssue(string DocNo,Receivement rcv)
        {
            ApprovePMIssue ShipPlanToShip = new ApprovePMIssue();         
            ShipPlanToShip.EntCode = CHelper.EntCode;
            ShipPlanToShip.UserCode = "A001";
            ShipPlanToShip.OrgCode = rcv.Org.Code;
            ShipPlanToShip.OptType = CHelper.ApproveRtnPMOptype;         
            ShipPlanToShip.DocNo = DocNo;
            return JsonConvert.SerializeObject(ShipPlanToShip);
        }
        /// <summary>
        /// 委外收货审核
        /// </summary>
        /// <param name="DocNo"></param>
        /// <param name="rcv"></param>
        /// <returns></returns>
        public static string ApprovePMRcv(string DocNo, Receivement rcv)
        {
            ApprovePMIssue ShipPlanToShip = new ApprovePMIssue();
            ShipPlanToShip.EntCode = CHelper.EntCode;
            ShipPlanToShip.UserCode = "A001";
            ShipPlanToShip.OrgCode = rcv?.Org.Code;
            ShipPlanToShip.OptType = CHelper.ApprovePMRcvOptype;
            ShipPlanToShip.DocNo = DocNo;
            return JsonConvert.SerializeObject(ShipPlanToShip);
        }
        /// <summary>
        ///  销售订单到原始订单
        /// </summary>
        /// <param name="misc"></param>
        /// <param name="whcode"></param>
        /// <returns></returns>
        public static string GetOrginalSO(SO misc, string whcode)
        {
            List<object> list = new List<object>();
            //string shop_no = misc.ShipPlanLines[0].OrderBy.Name; //客户编码传商铺编码改为表头私有段15
            string shop_no = misc.DescFlexField.PrivateDescSeg15;
            decimal TotalMny = 0;
            decimal TotalPlanQty = 0;
            List<string> lines = new List<string>();
            List<rawTradeOrderReq> rawTradeOrderReqs = new List<rawTradeOrderReq>();
            rawTradeOrderReq rawTradeOrderReq = new rawTradeOrderReq();
            rawTradeOrderReq.tid = misc.DocNo + whcode;
            foreach (SOLine item in misc.SOLines)
            {
                if (item.SOShiplines[0].WH != null && item.SOShiplines[0].WH.Code == whcode)
                {
                    if (!lines.Contains(item.ItemInfo.ItemCode))
                    {
                        lines.Add(item.ItemInfo.ItemCode);
                    }
                    TotalPlanQty += item.OrderByQtyTU;
                }

            }
           
            rawTradeOrderReq.goods_count = TotalPlanQty;
            rawTradeOrderReq.receivable = misc.TotalMoneyTC;
            if (!string.IsNullOrEmpty(misc.DescFlexField.PrivateDescSeg4))
            {
                rawTradeOrderReq.pay_method = Convert.ToInt32(misc.DescFlexField.PrivateDescSeg4);
            }
            //misc.BusinessDate.ToString();
            rawTradeOrderReq.trade_time = DateTime.Now.ToString();
            rawTradeOrderReq.buyer_nick = misc.OrderBy?.Name;
            rawTradeOrderReq.end_time = null;
            if (!string.IsNullOrEmpty(misc.DescFlexField.PrivateDescSeg12))
            {
                rawTradeOrderReq.pay_method = Convert.ToInt32(misc.DescFlexField.PrivateDescSeg12);
            }
            Customer customer = Customer.Finder.FindByID(misc.OrderBy.Customer?.ID);
            if (customer != null)
            {
                CustomerSite site = CustomerSite.Finder.Find("Code=@Code and Customer=@Customer", new OqlParam("Code",misc.ShipToSite?.Code),new OqlParam("Customer", customer.ID));
                //rawTradeOrderReq.receiver_area = site.DescFlexField?.PrivateDescSeg6 + site.DescFlexField.PrivateDescSeg7 + site.DescFlexField.PrivateDescSeg8;
                //rawTradeOrderReq.receiver_address = site.DescFlexField?.PrivateDescSeg3;
                //rawTradeOrderReq.receiver_telno = site.DescFlexField?.PrivateDescSeg5;
                //rawTradeOrderReq.receiver_mobile = site.DescFlexField?.PrivateDescSeg5;
                //rawTradeOrderReq.receiver_name = site.DescFlexField?.PrivateDescSeg4;
                if (!string.IsNullOrEmpty(misc.DescFlexField.PrivateDescSeg18))
                {
                    char[] delimiterChars = { ' ', '\t' };
                    string[] substrings = misc.DescFlexField.PrivateDescSeg18.Split(delimiterChars);
                    if (substrings != null && substrings.Length > 1)
                    {
                        string doc = substrings[1];
                        //var res = CHelper.Analysis(substrings[0]);
                        string regex = "(?<province>[^省]+自治区|.*?省|.*?行政区|.*?市)?(?<city>[^市]+自治州|.*?地区|.*?行政单位|.+盟|市辖区|.*?市|.*?县)?(?<county>[^县]+县|.+区|.+市|.+旗|.+海域|.+岛)?(?<town>[^区]+区|.+镇)?(?<village>.*)";
                        var m = Regex.Match(substrings[0], regex, RegexOptions.IgnoreCase);
                        var province = m.Groups["province"].Value;
                        var city = m.Groups["city"].Value;
                        var county = m.Groups["county"].Value;
                        var town = m.Groups["town"].Value;
                        var village = m.Groups["village"].Value;
                        rawTradeOrderReq.receiver_area = province + city + county;
                        rawTradeOrderReq.receiver_address = village;
                        rawTradeOrderReq.receiver_mobile = Regex.Replace(doc, @"[^0-9]+", "");
                        rawTradeOrderReq.receiver_name = Regex.Replace(doc, @"\d|\W|[A-Za-z]", "");
                    }

                }
                else if (site != null)
                {
                    rawTradeOrderReq.receiver_area = site.DescFlexField?.PrivateDescSeg6 + site.DescFlexField.PrivateDescSeg7 + site.DescFlexField.PrivateDescSeg8;
                    rawTradeOrderReq.receiver_address = site.DescFlexField?.PrivateDescSeg3;
                   // rawTradeOrderReq.receiver_telno = site.DescFlexField?.PrivateDescSeg5;
                    rawTradeOrderReq.receiver_mobile = site.DescFlexField?.PrivateDescSeg5;
                    rawTradeOrderReq.receiver_name = site.DescFlexField?.PrivateDescSeg4;
                }
            }
            rawTradeOrderReq.post_amount = 0;
            rawTradeOrderReq.delivery_term = 1;
            rawTradeOrderReq.process_status = 20;
            rawTradeOrderReq.trade_status = 30;
            rawTradeOrderReq.refund_status = 0;
            rawTradeOrderReq.pay_status = 2;
            rawTradeOrderReq.remark = misc.OrderBy?.Name;
            SOMemo memo = SOMemo.Finder.Find("SO=@SO",new OqlParam("SO",misc.ID));
            rawTradeOrderReq.buyer_message = memo?.Description;
            //订单来源待确认
            rawTradeOrderReq.warehouse_no = whcode;
            List<rawTradeOrderListReq> listSpec = new List<rawTradeOrderListReq>();
            ItemMaster itemMaster;
            foreach (SOLine soline in misc.SOLines)
            {
                if (soline.SOShiplines[0].WH != null && soline.SOShiplines[0].WH.Code == whcode)
                {
                    rawTradeOrderListReq specInfoReq = new rawTradeOrderListReq();
                    specInfoReq.tid = misc.DocNo + whcode;
                    specInfoReq.oid = misc.DocNo + whcode + soline.DocLineNo.ToString();
                    specInfoReq.status = 30;
                    specInfoReq.refund_status = 0;
                    if (!string.IsNullOrEmpty(soline.DescFlexField.PrivateDescSeg21))
                    {
                        itemMaster = ItemMaster.Finder.Find("Code=@Code and Org=@Org",new OqlParam("Code",soline.DescFlexField.PrivateDescSeg21),new OqlParam("Org",misc.Org.ID));
                        if (itemMaster != null)
                        {
                            specInfoReq.spec_id = itemMaster.Code1;
                            specInfoReq.goods_id = itemMaster.Code1;
                            specInfoReq.goods_no = itemMaster.Code1;
                            specInfoReq.spec_no = itemMaster.Code1;
                        }
                    }
                    else
                    {
                        itemMaster = ItemMaster.Finder.FindByID(soline.ItemInfo.ItemID.ID);
                        if (itemMaster != null)
                        {
                            specInfoReq.spec_id = itemMaster.Code1;
                            specInfoReq.goods_id = itemMaster.Code1;
                            specInfoReq.goods_no = itemMaster.Code1;
                            specInfoReq.spec_no = itemMaster.Code1;
                        }
                    }
                    
                    specInfoReq.goods_name = itemMaster?.Name;
                    specInfoReq.num = soline.OrderByQtyTU;
                    //SOLine soline = SOLine.Finder.FindByID(shipPlanLine.SrcDocLine);
                    if (soline != null)
                    {
                        ////specInfoReq.price = soline.FinallyPriceTC;
                        //specInfoReq.total_amount = soline.FinallyPriceTC * soline.OrderByQtyTU;
                        //specInfoReq.discount = soline.DiscountRate;
                        //specInfoReq.share_discount = soline.DiscountAC;
                        //specInfoReq.adjust_amount = soline.DiscountAC;
                        //specInfoReq.price = soline.FinallyPriceTC;
                        //specInfoReq.total_amount = soline.TotalMoneyTC;
                        specInfoReq.price = 0;
                        specInfoReq.total_amount = 0;
                        specInfoReq.discount = 0;
                        specInfoReq.share_discount = 0;
                        specInfoReq.adjust_amount = 0;
                      //  TotalMny += specInfoReq.total_amount;
                    }
                    specInfoReq.refund_amount = 0;
                    specInfoReq.json = "";
                    specInfoReq.remark = soline.DescFlexField.PrivateDescSeg8;
                    if (!string.IsNullOrEmpty(soline.DescFlexField.PrivateDescSeg8) && soline.DescFlexField.PrivateDescSeg8.Length>250)
                    {
                        throw new Exception("行明细备注超长，请修改后推送！");
                    }
                    listSpec.Add(specInfoReq);
                }

            }
            rawTradeOrderReq.order_count = listSpec.Count;
            // rawTradeOrderReq.receivable = TotalMny;//统计的是价税合计，行上的数据
            rawTradeOrderReqs.Add(rawTradeOrderReq);
            list.Add(shop_no);
            list.Add(rawTradeOrderReqs);
            list.Add(listSpec);
            return JsonConvert.SerializeObject(list);
        }
        /// <summary>
        /// 出货计划创建原始订单不拆行
        /// </summary>
        /// <param name="misc"></param>
        /// <param name="lines"></param>
        /// <param name="trate"></param>
        /// <returns></returns>
        public static string GetShipPlan(ShipPlan misc)
        {
            List<object> list = new List<object>();
            //string shop_no = misc.ShipPlanLines[0].OrderBy.Name; //客户编码传商铺编码改为取私有字段2 
            string shop_no = misc.ShipPlanLines[0].DescFlexField.PrivateDescSeg2;
            decimal TotalMny = 0;
            decimal TotalPlanQty = 0;
            List<string> lines = new List<string>();
            List<rawTradeOrderReq> rawTradeOrderReqs = new List<rawTradeOrderReq>();
            rawTradeOrderReq rawTradeOrderReq = new rawTradeOrderReq();
            rawTradeOrderReq.tid = misc.DocNo;
            foreach (ShipPlanLine item in misc.ShipPlanLines)
            {
                if (!lines.Contains(item.ItemInfo.ItemCode))
                {
                    lines.Add(item.ItemInfo.ItemCode);
                }
                TotalPlanQty += item.PlanQtyPU;
            }
            rawTradeOrderReq.order_count = lines.Count;
            rawTradeOrderReq.goods_count = TotalPlanQty;
            if (!string.IsNullOrEmpty(misc.DescFlexField.PrivateDescSeg4))
            {
                rawTradeOrderReq.pay_method = Convert.ToInt32(misc.DescFlexField.PrivateDescSeg4);
            }
            //misc.BusinessDate.ToString();
            rawTradeOrderReq.trade_time = DateTime.Now.ToString();
            rawTradeOrderReq.buyer_nick = misc.ShipPlanLines[0].OrderBy.Name;//待确认
            rawTradeOrderReq.receiver_name = misc.DescFlexField.PrivateDescSeg1;
            rawTradeOrderReq.end_time = null;
            rawTradeOrderReq.receiver_area = misc.DescFlexField.PrivateDescSeg2;
            rawTradeOrderReq.receiver_address = misc.DescFlexField.PrivateDescSeg3;
            rawTradeOrderReq.receiver_telno = misc.DescFlexField.PrivateDescSeg5;
            rawTradeOrderReq.receiver_mobile = misc.DescFlexField.PrivateDescSeg6;
            rawTradeOrderReq.post_amount = 0;
            rawTradeOrderReq.delivery_term = 1;
            rawTradeOrderReq.process_status = 20;
            rawTradeOrderReq.trade_status = 30;
            rawTradeOrderReq.refund_status = 0;
            rawTradeOrderReq.pay_status = 2;
            rawTradeOrderReq.remark = misc.Remark;
            //订单来源待确认
            rawTradeOrderReq.warehouse_no = misc.ShipPlanLines[0].WH != null ? misc.ShipPlanLines[0].WH.Code : "";//待确认 行数据          
            List<rawTradeOrderListReq> listSpec = new List<rawTradeOrderListReq>();
            foreach (ShipPlanLine shipPlanLine in misc.ShipPlanLines)
            {
                rawTradeOrderListReq specInfoReq = new rawTradeOrderListReq();
                specInfoReq.tid = misc.DocNo;
                specInfoReq.oid = Tools.GenerateId().ToString();
                specInfoReq.status = 30;
                specInfoReq.refund_status = 0;

                ItemMaster itemMaster = ItemMaster.Finder.FindByID(shipPlanLine.ItemInfo.ItemID.ID);
                if (itemMaster != null)
                {
                    specInfoReq.spec_id = itemMaster.Code1;
                    specInfoReq.goods_id = itemMaster.Code1;
                    specInfoReq.goods_no = itemMaster.Code1;
                    specInfoReq.spec_no = itemMaster.Code1;
                }


                specInfoReq.goods_name = itemMaster.Name;
                specInfoReq.num = shipPlanLine.PlanQtyPU;
                SOLine soline = SOLine.Finder.FindByID(shipPlanLine.SrcDocLine);
                if (soline != null)
                {
                    specInfoReq.price = soline.FinallyPriceTC;
                    specInfoReq.total_amount = soline.FinallyPriceTC * shipPlanLine.PlanQtyPU;
                    specInfoReq.discount = soline.DiscountRate;
                    specInfoReq.share_discount = soline.DiscountAC;
                    specInfoReq.adjust_amount = soline.DiscountAC;
                    TotalMny += specInfoReq.total_amount;
                }
                specInfoReq.refund_amount = 0;
                specInfoReq.remark = "";
                specInfoReq.json = "";
                specInfoReq.remark = shipPlanLine.Remark;
                listSpec.Add(specInfoReq);
            }
            rawTradeOrderReq.receivable = TotalMny;//统计的是价税合计，行上的数据
            rawTradeOrderReqs.Add(rawTradeOrderReq);
            list.Add(shop_no);
            list.Add(rawTradeOrderReqs);
            list.Add(listSpec);
            return JsonConvert.SerializeObject(list);
        }

        /// <summary>
        /// 奇门销售出库销售订单创建
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetQMStockOutSO(WdtWmsStockoutSalesQuerywithdetailResponse.OrderDomain item)
        {
            SOReq Req = new SOReq();
            SOHeadDTO sodto = new SOHeadDTO();
            sodto.BusinessDate = DateTime.Now.ToString(); // item.TradeTime;
            sodto.Status = "2";
            sodto.DocumentType = "SM1";
            sodto.IsPriceIncludeTax = Convert.ToDecimal(item.TaxRate) > 0 ? true : false;
            sodto.OrderBy = item.ShopNo;
            sodto.Seller = item.SalesmanNo;
            // ItemMaster itemmaster = ItemMaster.fin
            Organization org = Organization.FindByCode("101");
            Customer customer = Customer.Finder.Find("Code=@Code and Org=@Org", new OqlParam(item.ShopNo), new OqlParam(org?.ID));
            if (customer != null && customer.ShippmentRule != null)
            {
                sodto.ShipRule = customer.ShippmentRule.Code;
            }
            if (customer != null && customer.Department != null)
            {
                sodto.SaleDepartment = customer.Department.Code;
            }

            List<DocLineDTO> lines = new List<DocLineDTO>();
            int num = 10;
            foreach (var Detail in item.DetailsList)
            {
                DocLineDTO rtnLine = new DocLineDTO();
                rtnLine.DocLineNo = num.ToString();
                rtnLine.ItemInfo = Detail.GoodsNo;
                rtnLine.Qty = Detail.GoodsCount;
                //rtnLine.Price = "10";  //待确认 价格取值
                rtnLine.UOM = Detail.UnitName;
                rtnLine.FinallyPrice = Detail.SellPrice;
                if (customer != null && customer.RecervalTerm != null)
                {
                    rtnLine.RecTerm = customer.RecervalTerm.Code;
                }

                lines.Add(rtnLine);
                num += 10;
            }
            sodto.DocLine = lines;
            Req.SODt = sodto;

            return JsonConvert.SerializeObject(Req);
        }
        /// <summary>
        /// 奇门订单查询销售订单创建
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetQMOrderQuerySO(WdtSalesTradequeryQuerywithdetailResponse.OrderDomain item, string OrgCode, long TradeFrom)
        {
            SOReq Req = new SOReq();
            Req.EntCode = CHelper.EntCode;
            Req.OrgCode = OrgCode;
            Req.UserCode = "A001";
            Req.OptType = "SOCreate";
            SOHeadDTO sodto = new SOHeadDTO();
            sodto.DocNo = item.SrcTids;
            sodto.BusinessDate = DateTime.Now.ToString(); // item.TradeTime;
            sodto.Status = "2";
            if (TradeFrom == 1)
            {
                sodto.DocumentType = "SO6";
            }
            else if (TradeFrom == 2 || TradeFrom == 3 || TradeFrom == 4 || TradeFrom == 6)
            {
                sodto.DocumentType = "SO7";
            }
            sodto.IsPriceIncludeTax = Convert.ToDecimal(item.TaxRate) > 0 ? true : false;
            Operators operators = Operators.Finder.Find("Name=@Name", new UFSoft.UBF.PL.OqlParam("Name", item.SalesmanName));
            if (operators != null)
            {
                sodto.Seller = operators.Code;//业务员编码如何取
            }

            // ItemMaster itemmaster = ItemMaster.fin
            Organization org = Organization.FindByCode(OrgCode);
            Customer customer = Customer.Finder.Find("Name=@Name", new UFSoft.UBF.PL.OqlParam("Name", item.ShopNo));
            sodto.OrderBy = customer?.Code;
            // Customer customer = Customer.FindByCode(org, item.ShopNo);
            if (customer != null && customer.ShippmentRule != null)
            {
                sodto.ShipRule = customer.ShippmentRule.Code;
            }
            //if (customer != null && customer.Department != null)
            //{
            //    sodto.SaleDepartment = customer.Department.Code;
            //}
            sodto.Memo = item.CsRemark;
            //sodto.Seller 
            PubPriSVData pubPriSVData = new PubPriSVData();
            pubPriSVData.PrivateDescSeg1 = item.TradeNo;
            pubPriSVData.PrivateDescSeg2 = item.RefundStatus.ToString();
            pubPriSVData.PrivateDescSeg3 = item.FenxiaoNick;
            if (item.TradeType == 1)
            {
                pubPriSVData.PrivateDescSeg4 = "网店销售";
            }
            else if (item.TradeType == 2)
            {
                pubPriSVData.PrivateDescSeg4 = "线下订单";
            }
            else if (item.TradeType == 3)
            {
                pubPriSVData.PrivateDescSeg4 = "售后换货";
            }
            else if (item.TradeType == 4)
            {
                pubPriSVData.PrivateDescSeg4 = "批发业务";
            }
            else if (item.TradeType == 7)
            {
                pubPriSVData.PrivateDescSeg4 = "现款销售";
            }
            else if (item.TradeType == 8)
            {
                pubPriSVData.PrivateDescSeg4 = "分销订单";
            }
            pubPriSVData.PrivateDescSeg5 = item.LogisticsName;
            pubPriSVData.PrivateDescSeg6 = item.LogisticsNo;
            if (item.TradeFrom == 1)
            {
                pubPriSVData.PrivateDescSeg7 = "API抓单";
            }
            else if (item.TradeFrom == 2)
            {
                pubPriSVData.PrivateDescSeg7 = "手工建单";
            }
            else if (item.TradeFrom == 3)
            {
                pubPriSVData.PrivateDescSeg7 = "EXCEL导入";
            }
            else if (item.TradeFrom == 4)
            {
                pubPriSVData.PrivateDescSeg7 = "复制订单";
            }
            else if (item.TradeFrom == 6)
            {
                pubPriSVData.PrivateDescSeg7 = "补录订单";
            }
            pubPriSVData.PrivateDescSeg8 = item.StockoutNo;
            pubPriSVData.PrivateDescSeg9 = item.ReceiverArea;
            sodto.PubPriDt = pubPriSVData;
            List<DocLineDTO> lines = new List<DocLineDTO>();
            int num = 10;
            foreach (var Detail in item.DetailList)
            {
                DocLineDTO rtnLine = new DocLineDTO();
                rtnLine.DocLineNo = num.ToString();
                rtnLine.ItemInfo = Detail.GoodsNo;
                rtnLine.Qty = Detail.Num;
                //rtnLine.Price = "10";  //待确认 价格取值
                rtnLine.UOM = Detail.SpecNo;
                rtnLine.FinallyPrice = Detail.Price;
                if (customer != null && customer.RecervalTerm != null)
                {
                    rtnLine.RecTerm = customer.RecervalTerm.Code;
                }
                PubPriSVData pubPri = new PubPriSVData();
                pubPri.PrivateDescSeg1 = Detail.SrcOid;
                pubPri.PrivateDescSeg2 = Detail.SuiteNo;
                pubPri.PrivateDescSeg3 = Detail.SuiteNum;
                pubPri.PrivateDescSeg4 = Detail.Commission;
                pubPri.PrivateDescSeg5 = Detail.SuiteAmount;
                pubPri.PrivateDescSeg6 = Detail.SharePrice;
                pubPri.PrivateDescSeg7 = Detail.ShareAmount;
                pubPri.PrivateDescSeg8 = Detail.Remark;
                rtnLine.PubPriDt = pubPri;
                //pubPri.PrivateDescSeg9 = Detail.OrderPrice;
                lines.Add(rtnLine);
                num += 10;
            }
            sodto.DocLine = lines;
            Req.SODt = sodto;

            return JsonConvert.SerializeObject(Req);
        }
        /// <summary>
        /// 旺店通订单查询销售订单创建
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetWDTOrderQuerySO(SalesOrderData item, string OrgCode)
        {
            SOReq Req = new SOReq();
            Req.EntCode = CHelper.EntCode;
            Req.UserCode = "A001";
            Req.OrgCode = OrgCode;
            Req.OptType = "SOCreate";
            SOHeadDTO sodto = new SOHeadDTO();
            sodto.DocNo = item.src_tids;
            sodto.BusinessDate = DateTime.Now.ToString();
            sodto.Status = "2";
            sodto.DocumentType = "SO7";
            sodto.IsPriceIncludeTax = Convert.ToDecimal(item.tax_rate) > 0 ? true : false;
            Operators operators = Operators.Finder.Find("Name=@Name", new UFSoft.UBF.PL.OqlParam("Name", item.salesman_name));
            if (operators != null)
            {
                sodto.Seller = operators.Code;
            }
            // ItemMaster itemmaster = ItemMaster.fin
            Organization org = Organization.FindByCode(OrgCode);
            Customer customer = Customer.Finder.Find("Name=@Name", new UFSoft.UBF.PL.OqlParam("Name", item.shop_no));
            sodto.OrderBy = customer?.Code;
            // Customer customer = Customer.FindByCode(org, item.shop_no);
            if (customer != null && customer.ShippmentRule != null)
            {
                sodto.ShipRule = customer.ShippmentRule.Code;
            }
            //if (customer != null && customer.Department != null)
            //{
            //    sodto.SaleDepartment = customer.Department.Code;
            //}
            //sodto.Seller 
            PubPriSVData pubPriSVData = new PubPriSVData();
            pubPriSVData.PrivateDescSeg1 = item.trade_no;
            pubPriSVData.PrivateDescSeg2 = item.refund_status.ToString();
            pubPriSVData.PrivateDescSeg3 = item.fenxiao_nick;
            if (item.trade_type == 1)
            {
                pubPriSVData.PrivateDescSeg4 = "网店销售";
            }
            else if (item.trade_type == 2)
            {
                pubPriSVData.PrivateDescSeg4 = "线下订单";
            }
            else if (item.trade_type == 3)
            {
                pubPriSVData.PrivateDescSeg4 = "售后换货";
            }
            else if (item.trade_type == 4)
            {
                pubPriSVData.PrivateDescSeg4 = "批发业务";
            }
            else if (item.trade_type == 7)
            {
                pubPriSVData.PrivateDescSeg4 = "现款销售";
            }
            else if (item.trade_type == 8)
            {
                pubPriSVData.PrivateDescSeg4 = "分销订单";
            }
            pubPriSVData.PrivateDescSeg5 = item.logistics_name;
            pubPriSVData.PrivateDescSeg6 = item.logistics_no;
            if (item.trade_from == 1)
            {
                pubPriSVData.PrivateDescSeg7 = "API抓单";
            }
            else if (item.trade_from == 2)
            {
                pubPriSVData.PrivateDescSeg7 = "手工建单";
            }
            else if (item.trade_from == 3)
            {
                pubPriSVData.PrivateDescSeg7 = "EXCEL导入";
            }
            else if (item.trade_from == 4)
            {
                pubPriSVData.PrivateDescSeg7 = "复制订单";
            }
            else if (item.trade_from == 6)
            {
                pubPriSVData.PrivateDescSeg7 = "补录订单";
            }
            pubPriSVData.PrivateDescSeg8 = item.stockout_no;
            pubPriSVData.PrivateDescSeg9 = item.receiver_area;
            sodto.PubPriDt = pubPriSVData;
            List<DocLineDTO> lines = new List<DocLineDTO>();
            int num = 10;
            foreach (var Detail in item.detail_list)
            {
                DocLineDTO rtnLine = new DocLineDTO();
                rtnLine.DocLineNo = num.ToString();
                rtnLine.ItemInfo = Detail.goods_no;
                rtnLine.Qty = Detail.num.ToString();
                //rtnLine.Price = "10";  //待确认 价格取值
                rtnLine.UOM = Detail.spec_no;
                rtnLine.FinallyPrice = Detail.share_price.ToString();
                if (customer != null && customer.RecervalTerm != null)
                {
                    rtnLine.RecTerm = customer.RecervalTerm.Code;
                }
                PubPriSVData pubPri = new PubPriSVData();
                pubPri.PrivateDescSeg1 = Detail.src_oid;
                pubPri.PrivateDescSeg2 = Detail.suite_no;
                pubPri.PrivateDescSeg3 = Detail.suite_num.ToString();
                pubPri.PrivateDescSeg4 = Detail.commission.ToString();
                pubPri.PrivateDescSeg5 = item.post_amount.ToString();
                pubPri.PrivateDescSeg6 = Detail.share_price.ToString();
                pubPri.PrivateDescSeg7 = Detail.share_amount.ToString();
                pubPri.PrivateDescSeg8 = Detail.remark;
                //pubPri.PrivateDescSeg9 = Detail.OrderPrice;
                rtnLine.PubPriDt = pubPri;
                lines.Add(rtnLine);
                num += 10;
            }
            sodto.DocLine = lines;
            Req.SODt = sodto;

            return JsonConvert.SerializeObject(Req);
        }
        /// <summary>
        /// 奇门销售订单出库查询销售订单创建
        /// </summary>
        /// <param name="item"></param>
        /// <param name="OrgCode"></param>
        /// <returns></returns>
        public static string GetWDTOrderStockOutQuerySO(WdtWmsStockoutSalesQuerywithdetailResponse.OrderDomain item, string OrgCode)
        {
            SOReq Req = new SOReq();
            Customer customer = null;
            SalePriceList priceList = null;
            decimal price = 0m;
            Req.EntCode =CHelper.EntCode;
            Req.UserCode = "A001";
            Req.OrgCode = OrgCode;
            Req.OptType = "SOCreate";
            SOHeadDTO sodto = new SOHeadDTO();
            sodto.DocNo = item.SrcTradeNo;
            sodto.BusinessDate = item.Modified;
            sodto.Status = "2";
            if (item.TradeFrom == 1L)
            {
                sodto.DocumentType = "SO6";
            }
            else if (item.TradeFrom == 2L || item.TradeFrom == 3L || item.TradeFrom == 4L || item.TradeFrom == 6L || item.TradeFrom == 5L || item.TradeFrom == 8L)
            {
                sodto.DocumentType = "SO7";
            }
            sodto.IsPriceIncludeTax = true;
            Operators operators = Operators.Finder.Find("Name=@Name", new UFSoft.UBF.PL.OqlParam("Name", item.SalesmanNo));
            if (operators != null)
            {
                sodto.Seller = operators.Code;
            }
            // ItemMaster itemmaster = ItemMaster.fin
            Organization org = Organization.FindByCode(OrgCode);
            customer = Customer.Finder.Find("Code=@Code and Org=@Org", new OqlParam(item.ShopNo), new OqlParam(org?.ID));

            if (customer == null)
            {
                sodto.OrderBy = item.ShopNo;
            }
            else
            {
                if (customer.CustomerCategory?.Code == "105")
                {
                    priceList = SalePriceList.Finder.Find("Code=@Code and Org = @org", new UFSoft.UBF.PL.OqlParam("Code", customer.PriceListCode), new OqlParam("org", org.ID));

                }
                sodto.OrderBy = customer?.Code;
            }
            // Customer customer = Customer.FindByCode(org, item.shop_no);
            if (customer != null && customer.ShippmentRule != null)
            {
                sodto.ShipRule = customer.ShippmentRule.Code;
            }
            //if (customer != null && customer.Department != null)
            //{
            //    sodto.SaleDepartment = customer.Department.Code;
            //}
            //sodto.Seller 
            PubPriSVData pubPriSVData = new PubPriSVData();
            pubPriSVData.PrivateDescSeg1 = item.SrcOrderNo;
            if (item.RefundStatus == 0)
            {
                pubPriSVData.PrivateDescSeg2 = "无退款";
            }
            else if (item.RefundStatus == 1)
            {
                pubPriSVData.PrivateDescSeg2 = "申请退款";
            }
            else if (item.RefundStatus == 2)
            {
                pubPriSVData.PrivateDescSeg2 = "部分退款";
            }
            else if (item.RefundStatus == 3)
            {
                pubPriSVData.PrivateDescSeg2 = "全部退款";
            }
            // pubPriSVData.PrivateDescSeg3 = item.;
            if (item.TradeType == 1)
            {
                pubPriSVData.PrivateDescSeg4 = "网店销售";
            }
            else if (item.TradeType == 2)
            {
                pubPriSVData.PrivateDescSeg4 = "线下订单";
            }
            else if (item.TradeType == 3)
            {
                pubPriSVData.PrivateDescSeg4 = "售后换货";
            }
            else if (item.TradeType == 4)
            {
                pubPriSVData.PrivateDescSeg4 = "批发业务";
            }
            else if (item.TradeType == 7)
            {
                pubPriSVData.PrivateDescSeg4 = "现款销售";
            }
            else if (item.TradeType == 8)
            {
                pubPriSVData.PrivateDescSeg4 = "分销订单";
            }
            pubPriSVData.PrivateDescSeg5 = item.LogisticsName;
            pubPriSVData.PrivateDescSeg6 = item.LogisticsNo;
            if (item.TradeFrom == 1)
            {
                pubPriSVData.PrivateDescSeg7 = "API抓单";
            }
            else if (item.TradeFrom == 2)
            {
                pubPriSVData.PrivateDescSeg7 = "手工建单";
            }
            else if (item.TradeFrom == 3)
            {
                pubPriSVData.PrivateDescSeg7 = "EXCEL导入";
            }
            else if (item.TradeFrom == 4)
            {
                pubPriSVData.PrivateDescSeg7 = "复制订单";
            }
            else if (item.TradeFrom == 6)
            {
                pubPriSVData.PrivateDescSeg7 = "补录订单";
            }
            pubPriSVData.PrivateDescSeg8 = item.OrderNo;
            pubPriSVData.PrivateDescSeg9 = item.ReceiverArea;
            sodto.PubPriDt = pubPriSVData;
            List<DocLineDTO> lines = new List<DocLineDTO>();
            int num = 10;
            foreach (var Detail in item.DetailsList)
            {
                DocLineDTO rtnLine = new DocLineDTO();
                rtnLine.DocLineNo = num.ToString();
                rtnLine.ItemInfo = Detail.GoodsNo;
                rtnLine.Qty = Detail.GoodsCount;
                rtnLine.FreeType = Detail.GiftType==0?"2":"0";
                //rtnLine.Price = "10";  //待确认 价格取值
                rtnLine.UOM = Detail.SpecNo;
                if (priceList != null)
                {
                    SalePriceLine salePriceLine = SalePriceLine.Finder.Find("SalePriceList= @SalePriceList and ItemInfo.ItemCode=@ItemCode", new OqlParam("SalePriceList", priceList.ID), new OqlParam("ItemCode", Detail.GoodsNo));
                    rtnLine.FinallyPrice = salePriceLine?.Price.ToString();
                }
                else
                {
                    rtnLine.FinallyPrice = !string.IsNullOrEmpty(Detail.SharePrice)?(Convert.ToDecimal(Detail.SharePrice)/1.13m).ToString():"0";
                }

                if (customer != null && customer.RecervalTerm != null)
                {
                    rtnLine.RecTerm = customer.RecervalTerm.Code;
                }
                else
                {
                    rtnLine.RecTerm = "YZ01";
                }
                rtnLine.WHCode = item.WarehouseNo;
                PubPriSVData pubPri = new PubPriSVData();
                pubPri.PrivateDescSeg1 = Detail.SrcOid;
                pubPri.PrivateDescSeg2 = Detail.SuiteNo;
                pubPri.PrivateDescSeg3 = Detail.GoodsCount.ToString();
                pubPri.PrivateDescSeg4 = Detail.SellPrice.ToString();
                pubPri.PrivateDescSeg5 = item.PostAmount.ToString();
                pubPri.PrivateDescSeg6 = Detail.SellPrice.ToString();
                pubPri.PrivateDescSeg7 = Detail.ShareAmount.ToString();
                pubPri.PrivateDescSeg8 = Detail.Remark;
                //pubPri.PrivateDescSeg9 = Detail.OrderPrice;
                rtnLine.PubPriDt = pubPri;
                lines.Add(rtnLine);
                num += 10;
            }
            sodto.DocLine = lines;
            Req.SODt = sodto;

            return JsonConvert.SerializeObject(Req);
        }
        /// <summary>
        /// 奇门来源SO标准出货创建
        /// </summary>
        /// <param name="misc"></param>
        /// <param name="SrcOrderNo"></param>
        /// <returns></returns>
        public static string GetQMShipFromSO(SO misc, string SrcOrderNo, string OrgCode, WdtWmsStockoutSalesQuerywithdetailResponse.OrderDomain Domain)
        {
            SalePriceList priceList = null;
            ShipPlanToShip ShipPlanToShip = new ShipPlanToShip();
            ShipPlanToShipDTO shipPlanToShipDTO = new ShipPlanToShipDTO();
            shipPlanToShipDTO.BusinessDate = Domain.Modified;
            shipPlanToShipDTO.Status = 0;
            if (Domain.TradeFrom == 1 && !misc.DocNo.Contains("SO"))
            {
                shipPlanToShipDTO.DocType = "SM2";
            }
            else if (Domain.TradeFrom == 1 && misc.DocNo.Contains("SO"))
            {
                shipPlanToShipDTO.DocType = "SM4";
            }
            else if (Domain.TradeFrom == 2 || Domain.TradeFrom == 3 || Domain.TradeFrom == 4 || Domain.TradeFrom == 6 || Domain.TradeFrom == 5)
            {
                shipPlanToShipDTO.DocType = "SM3";
            }
            shipPlanToShipDTO.CreatedBy = misc.CreatedBy;
            shipPlanToShipDTO.OrgCode = misc.Org?.Code;
            Customer customer = Customer.Finder.Find("Code=@Code", new UFSoft.UBF.PL.OqlParam("Code", misc.OrderBy?.Code));
            if (customer != null)
            {
                shipPlanToShipDTO.CustomerCode = customer.Code;
                if (customer.CustomerCategory?.Code == "105")
                {
                    priceList = SalePriceList.Finder.Find("Code=@Code", new UFSoft.UBF.PL.OqlParam("Code", customer.PriceListCode));

                }
            }


            shipPlanToShipDTO.DocNo = SrcOrderNo;
            PubPriSVData pubPriSVData = new PubPriSVData();
            pubPriSVData.PrivateDescSeg1 = Domain.LogisticsName;
            pubPriSVData.PrivateDescSeg2 = Domain.LogisticsNo;
            pubPriSVData.PrivateDescSeg4 = Domain.ShopNo;
           
            List<ShipPlanToShipLineDTO> lines = new List<ShipPlanToShipLineDTO>();
            decimal a=0m, b = 0m;
            foreach (SOLine item in misc.SOLines)
            {
                foreach (var Detail in Domain.DetailsList)
                {
                    if (Detail.SrcTid!=null)
                    {
                        if (Detail.SrcTid.Contains(misc.DocNo))
                        {
                            if (!string.IsNullOrEmpty(Detail.SrcOid) && Detail.SrcOid.Length > 25)
                            {
                                bool isNumeric = Regex.IsMatch(Detail.SrcOid.Substring(26), @"^\d+$");
                                if (isNumeric)
                                {
                                    if (!string.IsNullOrEmpty(item.DescFlexField.PrivateDescSeg21))
                                    {
                                        if (item.DescFlexField.PrivateDescSeg21 == Detail.GoodsNo && item.DocLineNo == Convert.ToInt32(Detail.SrcOid.Substring(26)))
                                        {                                        
                                            PubPriSVData pubPriSVDataLine = new PubPriSVData();
                                            pubPriSVDataLine.PrivateDescSeg8 = item.DescFlexField.PrivateDescSeg21;
                                            pubPriSVDataLine.PrivateDescSeg11 = item.DescFlexField.PrivateDescSeg17;
                                            pubPriSVDataLine.PrivateDescSeg10 = item.DescFlexField.PrivateDescSeg19;
                                            ShipPlanToShipLineDTO rtnLine = new ShipPlanToShipLineDTO();
                                            rtnLine.ShipPlanLineNum = item.DocLineNo;
                                            rtnLine.ShipPlanDocNo = misc.DocNo;
                                            rtnLine.LineNo = item.DocLineNo;
                                            rtnLine.ConfirmDate = DateTime.Now.ToString();
                                            rtnLine.ShipQty = Convert.ToDecimal(Detail.GoodsCount);
                                            rtnLine.ItemCode = item.ItemInfo.ItemCode;
                                            rtnLine.PubPriDt = pubPriSVDataLine;
                                            if (!string.IsNullOrEmpty(pubPriSVDataLine.PrivateDescSeg10) && double.TryParse(pubPriSVDataLine.PrivateDescSeg10, out double result))
                                            {
                                                a += Convert.ToDecimal(pubPriSVDataLine.PrivateDescSeg10);
                                            }
                                            if (!string.IsNullOrEmpty(pubPriSVDataLine.PrivateDescSeg11) && double.TryParse(pubPriSVDataLine.PrivateDescSeg11, out double result1))
                                            {
                                                b += Convert.ToDecimal(pubPriSVDataLine.PrivateDescSeg11);
                                            }
                                            if (priceList != null)
                                            {
                                                SalePriceLine salePriceLine = SalePriceLine.Finder.Find("SalePriceList= @SalePriceList and ItemInfo.ItemCode=@ItemCode", new OqlParam("SalePriceList", priceList.ID), new OqlParam("ItemCode", Detail.GoodsNo));
                                                if (salePriceLine != null)
                                                {
                                                    rtnLine.TotalMoneyTC = rtnLine.ShipQty * salePriceLine.Price;
                                                }

                                            }
                                            else
                                            {
                                                rtnLine.TotalMoneyTC = rtnLine.ShipQty * Convert.ToDecimal(Detail.SharePrice);
                                            }

                                            rtnLine.ShipWH = Domain.WarehouseNo;
                                            lines.Add(rtnLine);
                                        }
                                    }
                                    else
                                    {
                                        if (item.ItemInfo.ItemCode == Detail.GoodsNo && item.DocLineNo == Convert.ToInt32(Detail.SrcOid.Substring(26)))
                                        {
                                            PubPriSVData pubPriSVDataLine = new PubPriSVData();
                                            pubPriSVDataLine.PrivateDescSeg11 = item.DescFlexField.PrivateDescSeg17;
                                            pubPriSVDataLine.PrivateDescSeg10 = item.DescFlexField.PrivateDescSeg19;
                                            ShipPlanToShipLineDTO rtnLine = new ShipPlanToShipLineDTO();
                                            rtnLine.ShipPlanLineNum = item.DocLineNo;
                                            rtnLine.ShipPlanDocNo = misc.DocNo;
                                            rtnLine.LineNo = item.DocLineNo;
                                            rtnLine.ConfirmDate = DateTime.Now.ToString();
                                            rtnLine.ShipQty = Convert.ToDecimal(Detail.GoodsCount);
                                            rtnLine.ItemCode = item.ItemInfo.ItemCode;
                                            if (!string.IsNullOrEmpty(pubPriSVDataLine.PrivateDescSeg10) && double.TryParse(pubPriSVDataLine.PrivateDescSeg10, out double result))
                                            {
                                                a += Convert.ToDecimal(pubPriSVDataLine.PrivateDescSeg10);
                                            }
                                            if (!string.IsNullOrEmpty(pubPriSVDataLine.PrivateDescSeg11) && double.TryParse(pubPriSVDataLine.PrivateDescSeg11, out double result1))
                                            {
                                                b += Convert.ToDecimal(pubPriSVDataLine.PrivateDescSeg11);
                                            }
                                            if (priceList != null)
                                            {
                                                SalePriceLine salePriceLine = SalePriceLine.Finder.Find("SalePriceList= @SalePriceList and ItemInfo.ItemCode=@ItemCode", new OqlParam("SalePriceList", priceList.ID), new OqlParam("ItemCode", Detail.GoodsNo));
                                                if (salePriceLine != null)
                                                {
                                                    rtnLine.TotalMoneyTC = rtnLine.ShipQty * salePriceLine.Price;
                                                }

                                            }
                                            else
                                            {
                                                rtnLine.TotalMoneyTC = rtnLine.ShipQty * Convert.ToDecimal(Detail.SharePrice);
                                            }

                                            rtnLine.ShipWH = Domain.WarehouseNo;
                                            rtnLine.PubPriDt = pubPriSVDataLine;
                                            lines.Add(rtnLine);
                                        }
                                    }

                                }
                            }
                            else if (!string.IsNullOrEmpty(Detail.SrcOid))
                            {
                                bool isNumeric = Regex.IsMatch(Detail.SrcOid, @"^\d+$");
                                if (isNumeric)
                                {
                                    if (!string.IsNullOrEmpty(item.DescFlexField.PrivateDescSeg21))
                                    {
                                        if (item.DescFlexField.PrivateDescSeg21 == Detail.GoodsNo && item.DocLineNo == Convert.ToInt32(Detail.SrcOid))
                                        {
                                            PubPriSVData pubPriSVDataLine = new PubPriSVData();
                                            pubPriSVDataLine.PrivateDescSeg8 = item.DescFlexField.PrivateDescSeg21;
                                            pubPriSVDataLine.PrivateDescSeg11 = item.DescFlexField.PrivateDescSeg17;
                                            pubPriSVDataLine.PrivateDescSeg10 = item.DescFlexField.PrivateDescSeg19;
                                            ShipPlanToShipLineDTO rtnLine = new ShipPlanToShipLineDTO();
                                            rtnLine.ShipPlanLineNum = item.DocLineNo;
                                            rtnLine.ShipPlanDocNo = misc.DocNo;
                                            rtnLine.LineNo = item.DocLineNo;
                                            rtnLine.ConfirmDate = DateTime.Now.ToString();
                                            rtnLine.ShipQty = Convert.ToDecimal(Detail.GoodsCount);
                                            rtnLine.ItemCode = item.ItemInfo.ItemCode;
                                            rtnLine.PubPriDt = pubPriSVDataLine;
                                            if (!string.IsNullOrEmpty(pubPriSVDataLine.PrivateDescSeg10) && double.TryParse(pubPriSVDataLine.PrivateDescSeg10, out double result))
                                            {
                                                a += Convert.ToDecimal(pubPriSVDataLine.PrivateDescSeg10);
                                            }
                                            if (!string.IsNullOrEmpty(pubPriSVDataLine.PrivateDescSeg11) && double.TryParse(pubPriSVDataLine.PrivateDescSeg11, out double result1))
                                            {
                                                b += Convert.ToDecimal(pubPriSVDataLine.PrivateDescSeg11);
                                            }
                                            if (priceList != null)
                                            {
                                                SalePriceLine salePriceLine = SalePriceLine.Finder.Find("SalePriceList= @SalePriceList and ItemInfo.ItemCode=@ItemCode", new OqlParam("SalePriceList", priceList.ID), new OqlParam("ItemCode", Detail.GoodsNo));
                                                if (salePriceLine != null)
                                                {
                                                    rtnLine.TotalMoneyTC = rtnLine.ShipQty * salePriceLine.Price;
                                                }

                                            }
                                            else
                                            {
                                                rtnLine.TotalMoneyTC = rtnLine.ShipQty * Convert.ToDecimal(Detail.SharePrice);
                                            }

                                            rtnLine.ShipWH = Domain.WarehouseNo;
                                            lines.Add(rtnLine);
                                        }
                                    }
                                    else
                                    {
                                        if (item.ItemInfo.ItemCode == Detail.GoodsNo && item.DocLineNo == Convert.ToInt32(Detail.SrcOid))
                                        {
                                            PubPriSVData pubPriSVDataLine = new PubPriSVData();
                                            pubPriSVDataLine.PrivateDescSeg11 = item.DescFlexField.PrivateDescSeg17;
                                            pubPriSVDataLine.PrivateDescSeg10 = item.DescFlexField.PrivateDescSeg19;
                                            ShipPlanToShipLineDTO rtnLine = new ShipPlanToShipLineDTO();
                                            rtnLine.ShipPlanLineNum = item.DocLineNo;
                                            rtnLine.ShipPlanDocNo = misc.DocNo;
                                            rtnLine.LineNo = item.DocLineNo;
                                            rtnLine.ConfirmDate = DateTime.Now.ToString();
                                            rtnLine.ShipQty = Convert.ToDecimal(Detail.GoodsCount);
                                            rtnLine.ItemCode = item.ItemInfo.ItemCode;
                                            if (!string.IsNullOrEmpty(pubPriSVDataLine.PrivateDescSeg10) && double.TryParse(pubPriSVDataLine.PrivateDescSeg10, out double result))
                                            {
                                                a += Convert.ToDecimal(pubPriSVDataLine.PrivateDescSeg10);
                                            }
                                            if (!string.IsNullOrEmpty(pubPriSVDataLine.PrivateDescSeg11) && double.TryParse(pubPriSVDataLine.PrivateDescSeg11, out double result1))
                                            {
                                                b += Convert.ToDecimal(pubPriSVDataLine.PrivateDescSeg11);
                                            }
                                            if (priceList != null)
                                            {
                                                SalePriceLine salePriceLine = SalePriceLine.Finder.Find("SalePriceList= @SalePriceList and ItemInfo.ItemCode=@ItemCode", new OqlParam("SalePriceList", priceList.ID), new OqlParam("ItemCode", Detail.GoodsNo));
                                                if (salePriceLine != null)
                                                {
                                                    rtnLine.TotalMoneyTC = rtnLine.ShipQty * salePriceLine.Price;
                                                }

                                            }
                                            else
                                            {
                                                rtnLine.TotalMoneyTC = rtnLine.ShipQty * Convert.ToDecimal(Detail.SharePrice);
                                            }

                                            rtnLine.ShipWH = Domain.WarehouseNo;
                                            rtnLine.PubPriDt = pubPriSVDataLine;
                                            lines.Add(rtnLine);
                                        }
                                    }

                                }
                            }
                        }
                    }
                   
                   
                }

            }
            pubPriSVData.PrivateDescSeg6 = a.ToString();
            pubPriSVData.PrivateDescSeg7 = b.ToString();
            shipPlanToShipDTO.PubPriDt = pubPriSVData;
            ShipPlanToShip.EntCode = CHelper.EntCode;
            ShipPlanToShip.UserCode = "A001";
            ShipPlanToShip.OrgCode = OrgCode;
            ShipPlanToShip.OptType = CHelper.ShipSrcSOOptype;
            shipPlanToShipDTO.ShipLineDt = lines;
            ShipPlanToShip.ShipDTO = shipPlanToShipDTO;
            return JsonConvert.SerializeObject(ShipPlanToShip);
        }
        /// <summary>
        /// 旺店通
        /// </summary>
        /// <param name="misc"></param>
        /// <param name="SrcOrderNo"></param>
        /// <param name="OrgCode"></param>
        /// <param name="Domain"></param>
        /// <returns></returns>
        public static string GetWDTShipFromSO(SO misc, string SrcOrderNo, string OrgCode, MiscSalesOrderDto Domain)
        {
            Customer customer = null;
            SalePriceList priceList = null;
            ShipPlanToShip ShipPlanToShip = new ShipPlanToShip();
            ShipPlanToShipDTO shipPlanToShipDTO = new ShipPlanToShipDTO();
            shipPlanToShipDTO.BusinessDate = Domain.consign_time;
            shipPlanToShipDTO.Status = 0;
            if (Domain.trade_from == 1 && !misc.DocNo.Contains("SO"))
            {
                shipPlanToShipDTO.DocType = "SM2";
            }
            else if (Domain.trade_from == 1 && misc.DocNo.Contains("SO"))
            {
                shipPlanToShipDTO.DocType = "SM4";
            }
            else if (Domain.trade_from == 2 || Domain.trade_from == 3 || Domain.trade_from == 4 || Domain.trade_from == 6 || Domain.trade_from == 5 || Domain.trade_from == 8)
            {
                shipPlanToShipDTO.DocType = "SM3";
            }
            shipPlanToShipDTO.CreatedBy = Domain.created;
            shipPlanToShipDTO.OrgCode = OrgCode;
            //Customer customer = Customer.Finder.Find("Name=@Name", new UFSoft.UBF.PL.OqlParam("Name", Domain.shop_no));
            shipPlanToShipDTO.CustomerCode = misc.OrderBy?.Code;
            Organization org = Organization.FindByCode(OrgCode);
            customer = Customer.Finder.Find("Code=@Code", new UFSoft.UBF.PL.OqlParam("Code", misc.OrderBy?.Code));
            if (customer != null)
            {
                if (customer.CustomerCategory?.Code == "105")
                {
                    priceList = SalePriceList.Finder.Find("Code=@Code", new UFSoft.UBF.PL.OqlParam("Code", customer.PriceListCode));

                }
            }

            shipPlanToShipDTO.DocNo = SrcOrderNo;

            PubPriSVData pubPriSVData = new PubPriSVData();
            pubPriSVData.PrivateDescSeg1 = Domain.logistics_name;
            pubPriSVData.PrivateDescSeg2 = Domain.logistics_no;
            pubPriSVData.PrivateDescSeg4 = Domain.shop_no;
            pubPriSVData.PrivateDescSeg12 = "2" ;
            shipPlanToShipDTO.PubPriDt = pubPriSVData;
            List<ShipPlanToShipLineDTO> lines = new List<ShipPlanToShipLineDTO>();
            foreach (SOLine item in misc.SOLines)
            {
                foreach (var Detail in Domain.details_list)
                {
                    if (item.ItemInfo.ItemCode == Detail.goods_no && item.DescFlexField.PrivateDescSeg1==Detail.src_oid)
                    {
                        ShipPlanToShipLineDTO rtnLine = new ShipPlanToShipLineDTO();
                        rtnLine.ShipPlanLineNum = item.DocLineNo;
                        rtnLine.ShipPlanDocNo = misc.DocNo;
                        rtnLine.LineNo = item.DocLineNo;
                        rtnLine.ConfirmDate = DateTime.Now.ToString();
                        rtnLine.ShipQty = Convert.ToDecimal(Detail.goods_count);
                        rtnLine.ItemCode = item.ItemInfo.ItemCode;
                        if (priceList != null)
                        {
                            SalePriceLine salePriceLine = SalePriceLine.Finder.Find("SalePriceList= @SalePriceList and ItemInfo.ItemCode=@ItemCode", new OqlParam("SalePriceList", priceList.ID), new OqlParam("ItemCode", Detail.goods_no));
                            if (salePriceLine != null)
                            {
                                rtnLine.TotalMoneyTC = rtnLine.ShipQty * salePriceLine.Price;
                            }

                        }
                        else
                        {
                            rtnLine.TotalMoneyTC = rtnLine.ShipQty * Convert.ToDecimal(Detail.share_price);
                        }

                        rtnLine.ShipWH = Domain.warehouse_no;
                        lines.Add(rtnLine);
                    }
                }

            }
            ShipPlanToShip.EntCode = CHelper.EntCode;
            ShipPlanToShip.UserCode = "A001";
            ShipPlanToShip.OrgCode = OrgCode;
            ShipPlanToShip.OptType = CHelper.ShipSrcSOOptype;
            shipPlanToShipDTO.ShipLineDt = lines;
            ShipPlanToShip.ShipDTO = shipPlanToShipDTO;
            return JsonConvert.SerializeObject(ShipPlanToShip);
        }
        /// <summary>
        /// 融合出库生成so
        /// </summary>
        /// <param name="ds"></param>
        ///   /// <param name="Org"></param>
        ///     ///   /// <param name="CusCode"></param>
        /// <returns></returns>
        public static string MergeSOJson(DataSet ds, Organization Org, string CusCode)
        {
            SOReq Req = new SOReq();
            Customer customer = null;
            SalePriceList priceList = null;
            Req.EntCode = CHelper.EntCode;
            Req.UserCode = "A001";
            Req.OrgCode = Org?.Code;
            Req.OptType = CHelper.SOOptype;
            SOHeadDTO sodto = new SOHeadDTO();
            sodto.DocNo = string.Format("{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), GetUniqueKey()); ;
            sodto.BusinessDate = DateTime.Now.ToString();
            sodto.Status = "2";
            //if (TradeFrom == 1)
            //{
            //    sodto.DocumentType = "SO6";
            //}
            //else if (TradeFrom == 2 || TradeFrom == 3 || TradeFrom == 4 || TradeFrom == 6)
            //{
            //    sodto.DocumentType = "SO7";
            //}
            //单据类型默认为SO6
            sodto.DocumentType = "SO6";
            customer = Customer.Finder.Find("Code=@Code", new UFSoft.UBF.PL.OqlParam("Code", CusCode));
            if (customer == null)
            {
                customer = Customer.Finder.Find("DescFlexField.PrivateDescSeg11=@S1 and Org=@Org", new OqlParam("S1", CusCode), new OqlParam("Org", Org?.ID));
            }

            if (customer != null)
            {
                sodto.OrderBy = customer.Code;
                //客户分类为105，价格取客户维护的销售价目表，否则取旺店通的价格
                if (customer.CustomerCategory?.Code == "105")
                {
                    priceList = SalePriceList.Finder.Find("Code=@Code", new UFSoft.UBF.PL.OqlParam("Code", customer.PriceListCode));

                }
            }
            if (customer != null && customer.Saleser != null)
            {
                sodto.Seller = customer.Saleser.Code;
            }
            // Customer customer = Customer.FindByCode(org, item.shop_no);
            if (customer != null && customer.ShippmentRule != null)
            {
                sodto.ShipRule = customer.ShippmentRule.Code;
            }


            List<DocLineDTO> lines = new List<DocLineDTO>();
            int num = 10;
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                DocLineDTO rtnLine = new DocLineDTO();
                rtnLine.DocLineNo = num.ToString();
                rtnLine.ItemInfo = row["ItemCode"].ToString();
                rtnLine.Qty = row["数量"].ToString();
                rtnLine.WHCode = row["WHCode"].ToString();
                if (priceList != null)
                {
                    SalePriceLine salePriceLine = SalePriceLine.Finder.Find("SalePriceList= @SalePriceList and ItemInfo.ItemCode=@ItemCode", new OqlParam("SalePriceList", priceList.ID), new OqlParam("ItemCode", row["ItemCode"].ToString()));
                    rtnLine.FinallyPrice = salePriceLine?.Price.ToString();
                }
                else
                {
                    rtnLine.FinallyPrice = row["SellPrice"].ToString();
                }

                if (customer != null && customer.RecervalTerm != null)
                {
                    rtnLine.RecTerm = customer.RecervalTerm.Code;
                }
                else
                {
                    rtnLine.RecTerm = "YZ01";
                }

                lines.Add(rtnLine);
                num += 10;
            }
            sodto.DocLine = lines;
            Req.SODt = sodto;

            return JsonConvert.SerializeObject(Req);
        }
        /// <summary>
        /// 合并来源销售出货单
        /// </summary>
        /// <param name="so"></param>
        /// <returns></returns>
        public static string MergeSrcShipJson(SO so)
        {

            #region 原逻辑，来源销售订单
            Customer customer = null;
            SalePriceList priceList = null;
            ShipPlanToShip ShipPlanToShip = new ShipPlanToShip();
            ShipPlanToShipDTO shipPlanToShipDTO = new ShipPlanToShipDTO();
            shipPlanToShipDTO.BusinessDate = DateTime.Now.ToString();
            shipPlanToShipDTO.Status = 2;
            //单据类型默认SM2
            shipPlanToShipDTO.DocType = "SM2";
            //if (Domain.trade_from == 1 && !misc.DocNo.Contains("SO"))
            //{
            //    shipPlanToShipDTO.DocType = "SM2";
            //}
            //else if (Domain.trade_from == 1 && misc.DocNo.Contains("SO"))
            //{
            //    shipPlanToShipDTO.DocType = "SM4";
            //}
            //else if (Domain.trade_from == 2 || Domain.trade_from == 3 || Domain.trade_from == 4 || Domain.trade_from == 6 || Domain.trade_from == 5)
            //{
            //    shipPlanToShipDTO.DocType = "SM3";
            //}
            shipPlanToShipDTO.CreatedBy = so.CreatedBy;
            shipPlanToShipDTO.OrgCode = so.Org?.Code;
            //Customer customer = Customer.Finder.Find("Name=@Name", new UFSoft.UBF.PL.OqlParam("Name", Domain.shop_no));
            shipPlanToShipDTO.CustomerCode = so.OrderBy?.Code;
            customer = Customer.Finder.Find("Code=@Code", new UFSoft.UBF.PL.OqlParam("Code", so.OrderBy?.Code));
            if (customer != null)
            {
                if (customer.CustomerCategory?.Code == "105")
                {
                    priceList = SalePriceList.Finder.Find("Code=@Code", new UFSoft.UBF.PL.OqlParam("Code", customer.PriceListCode));

                }
            }
            shipPlanToShipDTO.DocNo = string.Format("{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), GetUniqueKey());

            //PubPriSVData pubPriSVData = new PubPriSVData();
            //pubPriSVData.PrivateDescSeg1 = Domain.logistics_name;
            //pubPriSVData.PrivateDescSeg2 = Domain.logistics_no;
            //shipPlanToShipDTO.PubPriDt = pubPriSVData;
            List<ShipPlanToShipLineDTO> lines = new List<ShipPlanToShipLineDTO>();
            foreach (SOLine item in so.SOLines)
            {
                ShipPlanToShipLineDTO rtnLine = new ShipPlanToShipLineDTO();
                rtnLine.ShipPlanLineNum = item.DocLineNo;
                rtnLine.ShipPlanDocNo = so.DocNo;
                rtnLine.LineNo = item.DocLineNo;
                rtnLine.ConfirmDate = DateTime.Now.ToString();
                rtnLine.ShipQty = Convert.ToDecimal(item.OrderByQtyTU);
                rtnLine.ItemCode = item.ItemInfo.ItemCode;
                if (priceList != null)
                {
                    SalePriceLine salePriceLine = SalePriceLine.Finder.Find("SalePriceList= @SalePriceList and ItemInfo.ItemCode=@ItemCode", new OqlParam("SalePriceList", priceList.ID), new OqlParam("ItemCode", item.ItemInfo.ItemCode));
                    if (salePriceLine != null)
                    {
                        rtnLine.TotalMoneyTC = rtnLine.ShipQty * salePriceLine.Price;
                    }

                }
                else
                {
                    rtnLine.TotalMoneyTC = rtnLine.ShipQty * item.FinallyPriceTC;
                }

                rtnLine.ShipWH = item.SOShiplines[0].WH?.Code;
                lines.Add(rtnLine);

            }
            ShipPlanToShip.EntCode = CHelper.EntCode;
            ShipPlanToShip.UserCode = "A001";
            ShipPlanToShip.OrgCode = so.Org?.Code;
            ShipPlanToShip.OptType = CHelper.ShipSrcSOOptype;
            shipPlanToShipDTO.ShipLineDt = lines;
            ShipPlanToShip.ShipDTO = shipPlanToShipDTO;
            return JsonConvert.SerializeObject(ShipPlanToShip);
            #endregion
        }
        /// <summary>
        /// 合并生成无来源出货单
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="Org"></param>
        /// <param name="CusCode"></param>
        ///    /// <param name="ShipDate"></param>
        /// <returns></returns>
        public static string MergeShipJson(DataSet ds, Organization Org, string CusCode,string ShipDate)
        {
            #region 无来源出货单
            ShipNoSrc shipNo = new ShipNoSrc();
            Customer customer = null;
            SalePriceList priceList = null;
            NoSrcShipDTO noSrcShip = new NoSrcShipDTO();
            noSrcShip.BusinessDate = ShipDate; // order.CreatedTime;
            noSrcShip.Status = 0;
            noSrcShip.DocType = "SM5";
            noSrcShip.DocNo = string.Format("SM{0}{1}", Org.Code, DateTime.Now.ToString("yyyyMMddHHmmss"));
            ///暂时定为取客户私有段，后续需改为客户编码

            customer = Customer.Finder.Find("Code=@Code", new UFSoft.UBF.PL.OqlParam("Code", CusCode));
            if (customer != null)
            {
                noSrcShip.CustomerCode = customer.Code;
                //客户分类为105，价格取客户维护的销售价目表，否则取旺店通的价格
                if (customer.CustomerCategory?.Code == "105")
                {
                    priceList = SalePriceList.Finder.Find("Code=@Code", new UFSoft.UBF.PL.OqlParam("Code", customer.PriceListCode));

                }
            }
            else
            {
                customer = Customer.Finder.Find("DescFlexField.PrivateDescSeg11=@S1 and Org=@Org", new OqlParam("S1", CusCode), new OqlParam("Org", Org?.ID));
                noSrcShip.CustomerCode = customer?.Code;
                //客户分类为105，价格取客户维护的销售价目表，否则取旺店通的价格
                if (customer!=null && customer.CustomerCategory?.Code == "105")
                {
                    priceList = SalePriceList.Finder.Find("Code=@Code", new UFSoft.UBF.PL.OqlParam("Code", customer?.PriceListCode));

                }
            }
            // noSrcShip.CreatedBy = misc.CreatedBy;
            PubPriSVData pubPriSVData = new PubPriSVData();
            pubPriSVData.PrivateDescSeg4 = ds.Tables[0].Rows[0]["SO"]!=null? ds.Tables[0].Rows[0]["SO"].ToString():"";
            noSrcShip.PubPriDt = pubPriSVData;

            List<NoSrcShipLineDTO> lines = new List<NoSrcShipLineDTO>();
            int num = 10;
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                NoSrcShipLineDTO rtnLine = new NoSrcShipLineDTO();
                rtnLine.LineNo = num;
                rtnLine.ConfirmDate = ShipDate;
                rtnLine.ShipQty = Convert.ToDecimal(row["数量"]);
                rtnLine.ItemCode = row["ItemCode"].ToString();
                if (priceList != null)
                {
                    SalePriceLine salePriceLine = SalePriceLine.Finder.Find("SalePriceList= @SalePriceList and ItemInfo.ItemCode=@ItemCode", new OqlParam("SalePriceList", priceList.ID), new OqlParam("ItemCode", row["ItemCode"].ToString()));
                    rtnLine.TotalMoneyTC = (decimal)salePriceLine?.Price * rtnLine.ShipQty;
                }
                else
                {
                    rtnLine.TotalMoneyTC = Convert.ToDecimal(row["总支付金额"]);
                }

                rtnLine.ShipWH = row["WHCode"].ToString();
                lines.Add(rtnLine);
                num += 10;
            }
            noSrcShip.ShipLineDt = lines;
            shipNo.ShipDTO = noSrcShip;
            shipNo.EntCode = CHelper.EntCode;
            shipNo.UserCode = "A001";
            shipNo.OrgCode = Org?.Code;
            shipNo.OptType = CHelper.ShipOptype;
            return JsonConvert.SerializeObject(shipNo);
            #endregion
        }
        /// <summary>
        /// 销售出库单包材生成无来源出货
        /// </summary>
        /// <param name="details"></param>
        /// <param name="Org"></param>
        /// <param name="CusCode"></param>
        /// <param name="ShipDate"></param>
        /// <returns></returns>
        public static string  WdtNoSrcShipJson(List<WdtWmsStockoutSalesQuerywithdetailResponse.DetailsListDomain> details, Organization Org, string CusCode, string ShipDate,string WHCode,string ShipDocNo="")
        {
            #region 无来源出货单
            ShipNoSrc shipNo = new ShipNoSrc();
            Customer customer = null;
            SalePriceList priceList = null;
            NoSrcShipDTO noSrcShip = new NoSrcShipDTO();
            if (ShipDate!=null)
            {
                noSrcShip.BusinessDate = CHelper.GetDateTimeMilliseconds(Convert.ToInt64(ShipDate)); // order.CreatedTime;
            }
          
            noSrcShip.Status = 0;
            noSrcShip.DocType = "SM5";
            ///暂时定为取客户私有段，后续需改为客户编码

            customer = Customer.Finder.Find("Code=@Code", new UFSoft.UBF.PL.OqlParam("Code", CusCode));
            if (customer != null)
            {
                noSrcShip.CustomerCode = customer.Code;
                //客户分类为105，价格取客户维护的销售价目表，否则取旺店通的价格
                if (customer.CustomerCategory?.Code == "105")
                {
                    priceList = SalePriceList.Finder.Find("Code=@Code", new UFSoft.UBF.PL.OqlParam("Code", customer.PriceListCode));

                }
            }
            else
            {
                customer = Customer.Finder.Find("DescFlexField.PrivateDescSeg11=@S1 and Org=@Org", new OqlParam("S1", CusCode), new OqlParam("Org", Org?.ID));
                noSrcShip.CustomerCode = customer?.Code;
                //客户分类为105，价格取客户维护的销售价目表，否则取旺店通的价格
                if (customer != null && customer.CustomerCategory?.Code == "105")
                {
                    priceList = SalePriceList.Finder.Find("Code=@Code", new UFSoft.UBF.PL.OqlParam("Code", customer?.PriceListCode));

                }
            }
            // noSrcShip.CreatedBy = misc.CreatedBy;
            PubPriSVData pubPriSVData = new PubPriSVData();
            pubPriSVData.PrivateDescSeg5 = ShipDocNo;
            noSrcShip.PubPriDt = pubPriSVData;

            List<NoSrcShipLineDTO> lines = new List<NoSrcShipLineDTO>();
            int num = 10;
            foreach (WdtWmsStockoutSalesQuerywithdetailResponse.DetailsListDomain row in details)
            {
                NoSrcShipLineDTO rtnLine = new NoSrcShipLineDTO();
                rtnLine.LineNo = num;
                rtnLine.ConfirmDate = CHelper.GetDateTimeMilliseconds(Convert.ToInt64(ShipDate));
                rtnLine.ShipQty =Convert.ToDecimal(row.GoodsCount);
                rtnLine.ItemCode = row.GoodsNo;
                if (priceList != null)
                {
                    SalePriceLine salePriceLine = SalePriceLine.Finder.Find("SalePriceList= @SalePriceList and ItemInfo.ItemCode=@ItemCode", new OqlParam("SalePriceList", priceList.ID), new OqlParam("ItemCode", row.GoodsNo));
                    rtnLine.TotalMoneyTC = (decimal)salePriceLine?.Price * rtnLine.ShipQty;
                }
                else
                {
                    rtnLine.TotalMoneyTC = Convert.ToDecimal(row.ShareAmount);
                }

                rtnLine.ShipWH =WHCode ;
                lines.Add(rtnLine);
                num += 10;
            }
            noSrcShip.ShipLineDt = lines;
            shipNo.ShipDTO = noSrcShip;
            shipNo.EntCode = CHelper.EntCode;
            shipNo.UserCode = "A001";
            shipNo.OrgCode = Org?.Code;
            shipNo.OptType = CHelper.ShipOptype;
            return JsonConvert.SerializeObject(shipNo);
            #endregion
        }
        /// <summary>
        /// 审核出货单
        /// </summary>
        /// <param name="Org"></param>
        /// <param name="Doc"></param>
        /// <returns></returns>
        public static string  ApproveShipJson(Organization Org, string  Doc)
        {
            #region 无来源出货单
            ShipApproveNoSrc shipNo = new ShipApproveNoSrc();
            shipNo.EntCode = CHelper.EntCode;
            shipNo.UserCode = "A001";
            shipNo.OrgCode = Org?.Code;
            shipNo.OptType = CHelper.ShipApproveOptype;
            shipNo.DocNo = Doc;
            return JsonConvert.SerializeObject(shipNo);
            #endregion
        }
        /// <summary>
        /// 审核料品转换单
        /// </summary>
        /// <param name="Org"></param>
        /// <param name="Doc"></param>
        /// <returns></returns>
        public static string  ApproveTransferFormJson(Organization Org, string Doc)
        {
            #region 料品转换
            ShipApproveNoSrc shipNo = new ShipApproveNoSrc();
            shipNo.EntCode = CHelper.EntCode;
            shipNo.UserCode = "A001";
            shipNo.OrgCode = Org?.Code;
            shipNo.OptType = CHelper.TransferFormApproveOptype;
            shipNo.DocNo = Doc;
            return JsonConvert.SerializeObject(shipNo);
            #endregion
        }
        /// <summary>
        /// 获取唯一码,开发测试用，与客户实际业务无关
        /// </summary>
        /// <returns></returns>
        private static string GetUniqueKey()
        {
            int maxSize = 8;
            int minSize = 5;
            char[] chars = new char[62];
            string a;
            a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            chars = a.ToCharArray();
            int size = maxSize;
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            size = maxSize;
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length - 1)]);
            }
            return result.ToString();
        }
        /// <summary>
        /// 旺店通销售出库SO创建
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string GetWdtSO(MiscSalesOrderDto item)
        {
            SOReq Req = new SOReq();
            SOHeadDTO sodto = new SOHeadDTO();
            sodto.BusinessDate = DateTime.Now.ToString(); // item.trade_time;
            sodto.Status = "2";
            sodto.DocumentType = "SM1";
            sodto.IsPriceIncludeTax = Convert.ToDecimal(item.tax_rate) > 0 ? true : false;
            sodto.OrderBy = item.shop_no;
            sodto.Seller = item.salesman_no;
            // ItemMaster itemmaster = ItemMaster.fin
            Organization org = Organization.FindByCode("101");
            Customer customer = Customer.FindByCode(org, item.shop_no);
            if (customer != null && customer.ShippmentRule != null)
            {
                sodto.ShipRule = customer.ShippmentRule.Code;
            }
            if (customer != null && customer.Department != null)
            {
                sodto.SaleDepartment = customer.Department.Code;
            }

            List<DocLineDTO> lines = new List<DocLineDTO>();
            int num = 10;
            foreach (var Detail in item.details_list)
            {
                DocLineDTO rtnLine = new DocLineDTO();
                rtnLine.DocLineNo = num.ToString();
                rtnLine.ItemInfo = Detail.goods_no;
                rtnLine.Qty = Detail.goods_count.ToString();
                //rtnLine.Price = "10";  //待确认 价格取值
                rtnLine.UOM = Detail.unit_name;
                rtnLine.FinallyPrice = Detail.sell_price.ToString();
                if (customer != null && customer.RecervalTerm != null)
                {
                    rtnLine.RecTerm = customer.RecervalTerm.Code;
                }

                lines.Add(rtnLine);
                num += 10;
            }
            sodto.DocLine = lines;
            Req.SODt = sodto;

            return JsonConvert.SerializeObject(Req);
        }
        /// <summary>
        /// 旺店通其它入库盘盈入库创建U9杂收
        /// </summary>
        /// <param name="misc"></param>
        /// <param name="SrcOrderNo"></param>
        /// <returns></returns>
        public static string GetWdtProfitMiscRcvTrans(OrderDto item, Organization org)
        {
            PreStockinMiscRcvTransDto ShipPlanToShip = new PreStockinMiscRcvTransDto();
            MiscRcvSVData MiscRcvDTO = new MiscRcvSVData();
            Warehouse WH = Warehouse.FindByCode(org, item.warehouse_no);
            MiscRcvDTO.BussinessDate = DateTime.Now.ToString();
            MiscRcvDTO.Status = 2;
            MiscRcvDTO.DocType = "MiscRcv003";
            MiscRcvDTO.DocNo = item.order_no;
            MiscRcvDTO.Memo = item.remark;
            MiscRcvDTO.OrgCode = org.Code;
            MiscRcvDTO.WHMan = WH?.Manager?.Code;

            List<MiscRcvLineData> lines = new List<MiscRcvLineData>();
            int num = 10;
            foreach (var Detail in item.detail_list)
            {
                MiscRcvLineData rtnLine = new MiscRcvLineData();
                rtnLine.LineNum = num;
                rtnLine.ItemCode = Detail.goods_no;
                rtnLine.RcvQty = Convert.ToInt32(Detail.goods_count);
                rtnLine.DeptCode = WH.Department?.Code;
                rtnLine.RcvWH = item.warehouse_no;
                lines.Add(rtnLine);
                num += 10;
            }
            ShipPlanToShip.OrgCode = org.Code;
            ShipPlanToShip.EntCode = CHelper.EntCode;
            ShipPlanToShip.UserCode = "A001";
            ShipPlanToShip.OptType = CHelper.MiscRcvTransOptype;
            MiscRcvDTO.MiscRcvLineDt = lines;
            ShipPlanToShip.MiscRcvDTO = MiscRcvDTO;
            return JsonConvert.SerializeObject(ShipPlanToShip);
        }
        /// <summary>
        /// 旺店通其它入库业务单盘盈入库创建U9杂收
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static string GetWdtStockInProfitMiscRcvTrans(StockInOrderDto item, Organization org)
        {
            PreStockinMiscRcvTransDto ShipPlanToShip = new PreStockinMiscRcvTransDto();
            MiscRcvSVData MiscRcvDTO = new MiscRcvSVData();
            Warehouse WH = Warehouse.FindByCode(org, item.warehouse_no);
            MiscRcvDTO.BussinessDate = DateTime.Now.ToString();
            MiscRcvDTO.Status = 2;
            MiscRcvDTO.DocType = "MiscRcv003";
            MiscRcvDTO.DocNo = item.other_in_no;
            MiscRcvDTO.Memo = item.remark;
            MiscRcvDTO.OrgCode = org.Code;
            MiscRcvDTO.WHMan = WH?.Manager?.Code;

            List<MiscRcvLineData> lines = new List<MiscRcvLineData>();
            int num = 10;
            foreach (var Detail in item.detail_list)
            {
                MiscRcvLineData rtnLine = new MiscRcvLineData();
                rtnLine.LineNum = num;
                rtnLine.ItemCode = Detail.goods_no;
                rtnLine.RcvQty = Convert.ToDecimal(Detail.in_num);
                rtnLine.DeptCode = WH.Department?.Code;
                rtnLine.RcvWH = item.warehouse_no;
                rtnLine.StoreType = 4;
                lines.Add(rtnLine);
                num += 10;
            }
            ShipPlanToShip.OrgCode = org.Code;
            ShipPlanToShip.EntCode = CHelper.EntCode;
            ShipPlanToShip.UserCode = "A001";
            ShipPlanToShip.OptType = CHelper.MiscRcvTransOptype;
            MiscRcvDTO.MiscRcvLineDt = lines;
            ShipPlanToShip.MiscRcvDTO = MiscRcvDTO;
            return JsonConvert.SerializeObject(ShipPlanToShip);
        }
        /// <summary>
        /// 其他入库 正残转换 杂收
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static string GetWdtShiftMiscRcvTrans(StockInOrderDto item, Organization org)
        {
            PreStockinMiscRcvTransDto ShipPlanToShip = new PreStockinMiscRcvTransDto();
            MiscRcvSVData MiscRcvDTO = new MiscRcvSVData();
            Warehouse WH = Warehouse.FindByCode(org, item.warehouse_no);
            MiscRcvDTO.BussinessDate = item.modified != null ? CHelper.GetDateTimeMilliseconds(Convert.ToInt64(item.modified)) : DateTime.Now.ToString();
            MiscRcvDTO.Status = 2;
            MiscRcvDTO.DocType = "MiscRcv005";
            MiscRcvDTO.DocNo = item.other_in_no;
            MiscRcvDTO.Memo = item.remark;
            MiscRcvDTO.OrgCode = org.Code;
            MiscRcvDTO.WHMan = WH?.Manager?.Code;
            MiscRcvDTO.CreatedBy = item.employee_name;
            List<MiscRcvLineData> lines = new List<MiscRcvLineData>();
            int num = 10;
            foreach (var Detail in item.detail_list)
            {
                MiscRcvLineData rtnLine = new MiscRcvLineData();
                rtnLine.LineNum = num;
                rtnLine.ItemCode = Detail.goods_no;
                rtnLine.RcvQty = Convert.ToDecimal(Detail.in_num);
                rtnLine.DeptCode = WH.Department?.Code;
                rtnLine.RcvWH = item.warehouse_no;
                rtnLine.StoreType = Detail.defect ? 2 : 4;

                lines.Add(rtnLine);
                num += 10;
            }
            ShipPlanToShip.OrgCode = org.Code;
            ShipPlanToShip.EntCode = CHelper.EntCode;
            ShipPlanToShip.UserCode = "A001";
            ShipPlanToShip.OptType = CHelper.MiscRcvTransOptype;
            MiscRcvDTO.MiscRcvLineDt = lines;
            ShipPlanToShip.MiscRcvDTO = MiscRcvDTO;
            return JsonConvert.SerializeObject(ShipPlanToShip);
        }
        /// <summary>
        /// 其他入库 正残转换 杂收 拆单逻辑
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static string  GetWdtShiftMiscRcvTranss(StockInOrderDto item, Organization org)
        {
            Organization orga;
            PreStockinMiscRcvTransDto ShipPlanToShip = new PreStockinMiscRcvTransDto();
            MiscRcvSVData MiscRcvDTO = new MiscRcvSVData();
            Warehouse WH = Warehouse.FindByCode(org, item.warehouse_no);
            MiscRcvDTO.BussinessDate = item.modified != null ? CHelper.GetDateTimeMilliseconds(Convert.ToInt64(item.modified)) : DateTime.Now.ToString();
            MiscRcvDTO.Status = 2;
            MiscRcvDTO.DocType = "MiscRcv005";
            MiscRcvDTO.DocNo = item.other_in_no;
            MiscRcvDTO.Memo = item.remark;
            MiscRcvDTO.OrgCode = org.Code;
            MiscRcvDTO.WHMan = WH?.Manager?.Code;
            MiscRcvDTO.CreatedBy = item.employee_name;
            List<MiscRcvLineData> lines = new List<MiscRcvLineData>();
            int num = 10;
            foreach (var Detail in item.detail_list)
            {
                orga = CHelper.GetOrg(Detail.goods_no,"");
                if (orga!=null && orga.ID==org.ID)
                {
                    MiscRcvLineData rtnLine = new MiscRcvLineData();
                    rtnLine.LineNum = num;
                    rtnLine.ItemCode = Detail.goods_no;
                    rtnLine.RcvQty = Convert.ToDecimal(Detail.in_num);
                    rtnLine.DeptCode = WH.Department?.Code;
                    rtnLine.RcvWH = item.warehouse_no;
                    rtnLine.StoreType = Detail.defect ? 2 : 4;

                    lines.Add(rtnLine);
                    num += 10;
                }
            
            }
            ShipPlanToShip.OrgCode = org.Code;
            ShipPlanToShip.EntCode = CHelper.EntCode;
            ShipPlanToShip.UserCode = "A001";
            ShipPlanToShip.OptType = CHelper.MiscRcvTransOptype;
            MiscRcvDTO.MiscRcvLineDt = lines;
            ShipPlanToShip.MiscRcvDTO = MiscRcvDTO;
            return JsonConvert.SerializeObject(ShipPlanToShip);
        }
        /// <summary>
        /// 旺店通其它入库业务单库存异动入库创建U9杂收
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static string GetWdtAbnormalMiscRcvTrans(StockInOrderDto item, Organization org)
        {
            PreStockinMiscRcvTransDto ShipPlanToShip = new PreStockinMiscRcvTransDto();
            MiscRcvSVData MiscRcvDTO = new MiscRcvSVData();
            Warehouse WH = Warehouse.FindByCode(org, item.warehouse_no);
            MiscRcvDTO.BussinessDate = item.modified != null ? CHelper.GetDateTimeMilliseconds(Convert.ToInt64(item.modified)) : DateTime.Now.ToString();
            MiscRcvDTO.Status = 2;
            MiscRcvDTO.DocType = "MiscRcv007";
            MiscRcvDTO.DocNo = item.other_in_no;
            MiscRcvDTO.Memo = item.remark;
            MiscRcvDTO.OrgCode = org.Code;
            MiscRcvDTO.WHMan = WH?.Manager?.Code;
            MiscRcvDTO.CreatedBy = item.employee_name;
            List<MiscRcvLineData> lines = new List<MiscRcvLineData>();
            int num = 10;
            foreach (var Detail in item.detail_list)
            {
                MiscRcvLineData rtnLine = new MiscRcvLineData();
                rtnLine.LineNum = num;
                rtnLine.ItemCode = Detail.goods_no;
                rtnLine.RcvQty = Convert.ToDecimal(Detail.in_num);
                rtnLine.DeptCode = WH.Department?.Code;
                rtnLine.RcvWH = item.warehouse_no;
                rtnLine.StoreType = Detail.defect ? 2 : 4;
                lines.Add(rtnLine);
                num += 10;
            }
            ShipPlanToShip.OrgCode = org.Code;
            ShipPlanToShip.EntCode = CHelper.EntCode;
            ShipPlanToShip.UserCode = "A001";
            ShipPlanToShip.OptType = CHelper.MiscRcvTransOptype;
            MiscRcvDTO.MiscRcvLineDt = lines;
            ShipPlanToShip.MiscRcvDTO = MiscRcvDTO;
            return JsonConvert.SerializeObject(ShipPlanToShip);
        }

        /// <summary>
        /// 旺店通其它入库业务单库存异动入库创建U9杂收 拆单逻辑
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static string  GetWdtAbnormalMiscRcvTranss(StockInOrderDto item, Organization org)
        {
            Organization orga;
            PreStockinMiscRcvTransDto ShipPlanToShip = new PreStockinMiscRcvTransDto();
            MiscRcvSVData MiscRcvDTO = new MiscRcvSVData();
            Warehouse WH = Warehouse.FindByCode(org, item.warehouse_no);
            MiscRcvDTO.BussinessDate = item.modified != null ? CHelper.GetDateTimeMilliseconds(Convert.ToInt64(item.modified)) : DateTime.Now.ToString();
            MiscRcvDTO.Status = 2;
            MiscRcvDTO.DocType = "MiscRcv007";
            MiscRcvDTO.DocNo = item.other_in_no;
            MiscRcvDTO.Memo = item.remark;
            MiscRcvDTO.OrgCode = org.Code;
            MiscRcvDTO.WHMan = WH?.Manager?.Code;
            MiscRcvDTO.CreatedBy = item.employee_name;
            List<MiscRcvLineData> lines = new List<MiscRcvLineData>();
            int num = 10;
            foreach (var Detail in item.detail_list)
            {
                orga = CHelper.GetOrg(Detail.goods_no,"");
                if (orga!=null  && orga.ID==org.ID)
                {
                    MiscRcvLineData rtnLine = new MiscRcvLineData();
                    rtnLine.LineNum = num;
                    rtnLine.ItemCode = Detail.goods_no;
                    rtnLine.RcvQty = Convert.ToDecimal(Detail.in_num);
                    rtnLine.DeptCode = WH.Department?.Code;
                    rtnLine.RcvWH = item.warehouse_no;
                    rtnLine.StoreType = Detail.defect ? 2 : 4;
                    lines.Add(rtnLine);
                    num += 10;
                }
             
            }
            ShipPlanToShip.OrgCode = org.Code;
            ShipPlanToShip.EntCode = CHelper.EntCode;
            ShipPlanToShip.UserCode = "A001";
            ShipPlanToShip.OptType = CHelper.MiscRcvTransOptype;
            MiscRcvDTO.MiscRcvLineDt = lines;
            ShipPlanToShip.MiscRcvDTO = MiscRcvDTO;
            return JsonConvert.SerializeObject(ShipPlanToShip);
        }
        /// <summary>
        /// 旺店通外部调整入库业务单盘盈入库创建U9杂收
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static string GetWdtOuterInProfitMiscRcvTrans(OuterInOrderDto item, Organization org)
        {
            PreStockinMiscRcvTransDto ShipPlanToShip = new PreStockinMiscRcvTransDto();
            MiscRcvSVData MiscRcvDTO = new MiscRcvSVData();
            Warehouse WH = Warehouse.FindByCode(org, item.warehouse_no);
            MiscRcvDTO.BussinessDate = item.modified;
            MiscRcvDTO.Status = 2;
            MiscRcvDTO.DocType = "MiscRcv003";
            if (item.src_order_type == 0 && item.reason == "销退入库")
            {
                MiscRcvDTO.DocType = "MiscRcv008";
            }
            MiscRcvDTO.DocNo = item.outer_in_no;
            MiscRcvDTO.Memo = item.remark;
            MiscRcvDTO.OrgCode = org.Code;
            MiscRcvDTO.WHMan = WH?.Manager?.Code;
            MiscRcvDTO.CreatedBy = item.creator_name;
            List<MiscRcvLineData> lines = new List<MiscRcvLineData>();
            int num = 10;
            foreach (var Detail in item.detail_list)
            {
                MiscRcvLineData rtnLine = new MiscRcvLineData();
                rtnLine.LineNum = num;
                rtnLine.ItemCode = Detail.goods_no;
                rtnLine.RcvQty = Convert.ToDecimal(Detail.num);
                rtnLine.DeptCode = WH.Department?.Code;
                rtnLine.RcvWH = item.warehouse_no;
                rtnLine.StoreType = Detail.defect ? 2 : 4;
                lines.Add(rtnLine);
                num += 10;
            }
            ShipPlanToShip.OrgCode = org.Code;
            ShipPlanToShip.EntCode = CHelper.EntCode;
            ShipPlanToShip.UserCode = "A001";
            ShipPlanToShip.OptType = CHelper.MiscRcvTransOptype;
            MiscRcvDTO.MiscRcvLineDt = lines;
            ShipPlanToShip.MiscRcvDTO = MiscRcvDTO;
            return JsonConvert.SerializeObject(ShipPlanToShip);
        }
        /// <summary>
        /// 旺店通外部调整入库业务单盘盈入库创建U9杂收 拆弹逻辑
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static string  GetWdtOuterInProfitMiscRcvTranss(OuterInOrderDto item, Organization org)
        {
            Organization orga;
            PreStockinMiscRcvTransDto ShipPlanToShip = new PreStockinMiscRcvTransDto();
            MiscRcvSVData MiscRcvDTO = new MiscRcvSVData();
            Warehouse WH = Warehouse.FindByCode(org, item.warehouse_no);
            MiscRcvDTO.BussinessDate = item.modified;
            MiscRcvDTO.Status = 2;
            MiscRcvDTO.DocType = "MiscRcv003";
            if (item.src_order_type == 0 && item.reason == "销退入库")
            {
                MiscRcvDTO.DocType = "MiscRcv008";
            }
            MiscRcvDTO.DocNo = item.outer_in_no;
            MiscRcvDTO.Memo = item.remark;
            MiscRcvDTO.OrgCode = org.Code;
            MiscRcvDTO.WHMan = WH?.Manager?.Code;
            MiscRcvDTO.CreatedBy = item.creator_name;
            List<MiscRcvLineData> lines = new List<MiscRcvLineData>();
            int num = 10;
            foreach (var Detail in item.detail_list)
            {
                orga = CHelper.GetOrg(Detail.goods_no, "");
                if (orga != null && orga.ID == org.ID)
                {
                    MiscRcvLineData rtnLine = new MiscRcvLineData();
                    rtnLine.LineNum = num;
                    rtnLine.ItemCode = Detail.goods_no;
                    rtnLine.RcvQty = Convert.ToDecimal(Detail.num);
                    rtnLine.DeptCode = WH.Department?.Code;
                    rtnLine.RcvWH = item.warehouse_no;
                    rtnLine.StoreType = Detail.defect ? 2 : 4;
                    lines.Add(rtnLine);
                    num += 10;
                }
               
            }
            ShipPlanToShip.OrgCode = org.Code;
            ShipPlanToShip.EntCode = CHelper.EntCode;
            ShipPlanToShip.UserCode = "A001";
            ShipPlanToShip.OptType = CHelper.MiscRcvTransOptype;
            MiscRcvDTO.MiscRcvLineDt = lines;
            ShipPlanToShip.MiscRcvDTO = MiscRcvDTO;
            return JsonConvert.SerializeObject(ShipPlanToShip);
        }
        /// <summary>
        /// 旺店通汇总入库单【盘点入库】创建U9杂收
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static string GetWdtBaseStockInMiscRcvTrans(StockInBaseSearchOrder item, Organization org)
        {
            PreStockinMiscRcvTransDto ShipPlanToShip = new PreStockinMiscRcvTransDto();
            MiscRcvSVData MiscRcvDTO = new MiscRcvSVData();
            Warehouse WH = Warehouse.FindByCode(org, item.warehouse_no);
            MiscRcvDTO.BussinessDate = DateTime.Now.ToString();
            MiscRcvDTO.Status = 2;
            MiscRcvDTO.DocType = "MiscRcv006";
            MiscRcvDTO.DocNo = item.stockin_no;
            MiscRcvDTO.Memo = item.remark;
            MiscRcvDTO.OrgCode = org.Code;
            MiscRcvDTO.WHMan = WH?.Manager?.Code;

            List<MiscRcvLineData> lines = new List<MiscRcvLineData>();
            int num = 10;
            foreach (var Detail in item.detail_list)
            {
                MiscRcvLineData rtnLine = new MiscRcvLineData();
                rtnLine.LineNum = num;
                rtnLine.ItemCode = Detail.goods_no;
                rtnLine.RcvQty = Convert.ToDecimal(Detail.num);
                rtnLine.DeptCode = WH.Department?.Code;
                rtnLine.RcvWH = item.warehouse_no;
                rtnLine.StoreType = Detail.defect ? 2 : 4;
                lines.Add(rtnLine);
                num += 10;
            }
            ShipPlanToShip.OrgCode = org.Code;
            ShipPlanToShip.EntCode = CHelper.EntCode;
            ShipPlanToShip.UserCode = "A001";
            ShipPlanToShip.OptType = CHelper.MiscRcvTransOptype;
            MiscRcvDTO.MiscRcvLineDt = lines;
            ShipPlanToShip.MiscRcvDTO = MiscRcvDTO;
            return JsonConvert.SerializeObject(ShipPlanToShip);
        }
        /// <summary>
        /// 旺店通汇总入库单【正残转换】创建U9杂收
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// /// <param name="order"></param>
        /// <returns></returns>
        public static string  GetWdtBaseSearchStockInMiscRcvTrans(StockInBaseSearchOrder item, Organization org, bool order)
        {
            PreStockinMiscRcvTransDto ShipPlanToShip = new PreStockinMiscRcvTransDto();
            MiscRcvSVData MiscRcvDTO = new MiscRcvSVData();
            Warehouse WH = Warehouse.FindByCode(org, item.warehouse_no);
            MiscRcvDTO.BussinessDate = item.check_time;
            MiscRcvDTO.Status = 2;
            MiscRcvDTO.DocType = "MiscRcv005";
            MiscRcvDTO.DocNo = item.stockin_no;
            MiscRcvDTO.Memo = item.remark;
            MiscRcvDTO.OrgCode = org.Code;
            MiscRcvDTO.WHMan = WH?.Manager?.Code;
            MiscRcvDTO.CreatedBy = item.operator_name;
            List<MiscRcvLineData> lines = new List<MiscRcvLineData>();
            int num = 10;
            foreach (var Detail in item.detail_list)
            {
                MiscRcvLineData rtnLine = new MiscRcvLineData();
                rtnLine.LineNum = num;
                rtnLine.ItemCode = Detail.goods_no;
                rtnLine.RcvQty = Convert.ToDecimal(Detail.num);
                rtnLine.DeptCode = WH.Department?.Code;
                rtnLine.RcvWH = item.warehouse_no;
                //if (order!=null)
                //{
                //    foreach (DefectChangeSearchDetail searchDetail in order.details_list)
                //    {
                //        if (Detail.spec_no == searchDetail.spec_no)
                //        {
                //            rtnLine.StoreType = searchDetail.defect ? 2 : 4;
                //        }

                //    }
                //}
                rtnLine.StoreType = order ? 4 : 2;

                lines.Add(rtnLine);
                num += 10;
            }
            ShipPlanToShip.OrgCode = org.Code;
            ShipPlanToShip.EntCode = CHelper.EntCode;
            ShipPlanToShip.UserCode = "A001";
            ShipPlanToShip.OptType = CHelper.MiscRcvTransOptype;
            MiscRcvDTO.MiscRcvLineDt = lines;
            ShipPlanToShip.MiscRcvDTO = MiscRcvDTO;
            return JsonConvert.SerializeObject(ShipPlanToShip);
        }
        /// <summary>
        /// 旺店通汇总入库单【盘点入库】创建U9杂收
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// /// <param name="order"></param>
        /// <returns></returns>
        public static string  GetWdtBaseSearchStockInMiscRcvTransByPD(StockInBaseSearchOrder item, Organization org, bool order)
        {
            PreStockinMiscRcvTransDto ShipPlanToShip = new PreStockinMiscRcvTransDto();
            MiscRcvSVData MiscRcvDTO = new MiscRcvSVData();
            Warehouse WH = Warehouse.FindByCode(org, item.warehouse_no);
            MiscRcvDTO.BussinessDate = item.check_time;
            MiscRcvDTO.Status = 2;
            MiscRcvDTO.DocType = "MiscRcv006";
            MiscRcvDTO.DocNo = item.stockin_no;
            MiscRcvDTO.Memo = item.remark;
            MiscRcvDTO.OrgCode = org.Code;
            MiscRcvDTO.WHMan = WH?.Manager?.Code;
            MiscRcvDTO.CreatedBy = item.operator_name;
            List<MiscRcvLineData> lines = new List<MiscRcvLineData>();
            int num = 10;
            foreach (var Detail in item.detail_list)
            {
                MiscRcvLineData rtnLine = new MiscRcvLineData();
                rtnLine.LineNum = num;
                rtnLine.ItemCode = Detail.goods_no;
                rtnLine.RcvQty = Convert.ToDecimal(Detail.num);
                rtnLine.DeptCode = WH.Department?.Code;
                rtnLine.RcvWH = item.warehouse_no;
                //if (order!=null)
                //{
                //    foreach (DefectChangeSearchDetail searchDetail in order.details_list)
                //    {
                //        if (Detail.spec_no == searchDetail.spec_no)
                //        {
                //            rtnLine.StoreType = searchDetail.defect ? 2 : 4;
                //        }

                //    }
                //}
                rtnLine.StoreType = order ? 4 : 2;

                lines.Add(rtnLine);
                num += 10;
            }
            ShipPlanToShip.OrgCode = org.Code;
            ShipPlanToShip.EntCode = CHelper.EntCode;
            ShipPlanToShip.UserCode = "A001";
            ShipPlanToShip.OptType = CHelper.MiscRcvTransOptype;
            MiscRcvDTO.MiscRcvLineDt = lines;
            ShipPlanToShip.MiscRcvDTO = MiscRcvDTO;
            return JsonConvert.SerializeObject(ShipPlanToShip);
        }
        /// <summary>
        /// 生产入库单创建U9杂收
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// /// <param name="order"></param>
        /// <returns></returns>
        public static string  GetWdtProcessStockInMiscRcvTrans(StockinProcessOrder item, Organization org)
        {
            PreStockinMiscRcvTransDto ShipPlanToShip = new PreStockinMiscRcvTransDto();
            MiscRcvSVData MiscRcvDTO = new MiscRcvSVData();
            Warehouse WH = Warehouse.FindByCode(org, item.warehouse_no);
            MiscRcvDTO.BussinessDate = DateTime.Now.ToString();
            MiscRcvDTO.Status = 2;
            MiscRcvDTO.DocType = "MiscRcv005";
            MiscRcvDTO.DocNo = item.stockin_no;
            MiscRcvDTO.Memo = item.remark;
            MiscRcvDTO.OrgCode = org.Code;
            MiscRcvDTO.WHMan = WH?.Manager?.Code;
            MiscRcvDTO.CreatedBy = item.operator_name;
            List<MiscRcvLineData> lines = new List<MiscRcvLineData>();
            int num = 10;
            foreach (var Detail in item.detail_list)
            {
                MiscRcvLineData rtnLine = new MiscRcvLineData();
                rtnLine.LineNum = num;
                rtnLine.ItemCode = Detail.goods_no;
                rtnLine.RcvQty = Convert.ToDecimal(Detail.num);
                rtnLine.DeptCode = WH.Department?.Code;
                rtnLine.RcvWH = item.warehouse_no;
                rtnLine.StoreType = Detail.defect ? 2 : 4;


                lines.Add(rtnLine);
                num += 10;
            }
            ShipPlanToShip.OrgCode = org.Code;
            ShipPlanToShip.EntCode = CHelper.EntCode;
            ShipPlanToShip.UserCode = "A001";
            ShipPlanToShip.OptType = CHelper.MiscRcvTransOptype;
            MiscRcvDTO.MiscRcvLineDt = lines;
            ShipPlanToShip.MiscRcvDTO = MiscRcvDTO;
            return JsonConvert.SerializeObject(ShipPlanToShip);
        }
        /// <summary>
        /// 旺店通汇总入库单【正残转换】创建U9杂收
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// /// <param name="order"></param>
        /// <returns></returns>
        public static string  GetWdtBaseSearchStockInMiscRcvTranss(StockInBaseSearchOrder item, Organization org, bool order)
        {
            Organization orga;
            PreStockinMiscRcvTransDto ShipPlanToShip = new PreStockinMiscRcvTransDto();
            MiscRcvSVData MiscRcvDTO = new MiscRcvSVData();
            Warehouse WH = Warehouse.FindByCode(org, item.warehouse_no);
            MiscRcvDTO.BussinessDate = item.check_time;
            MiscRcvDTO.Status = 2;
            MiscRcvDTO.DocType = "MiscRcv005";
            MiscRcvDTO.DocNo = item.stockin_no;
            MiscRcvDTO.Memo = item.remark;
            MiscRcvDTO.OrgCode = org.Code;
            MiscRcvDTO.WHMan = WH?.Manager?.Code;
            MiscRcvDTO.CreatedBy = item.operator_name;
            List<MiscRcvLineData> lines = new List<MiscRcvLineData>();
            int num = 10;
            foreach (var Detail in item.detail_list)
            {
                orga = CHelper.GetOrg(Detail.goods_no,"");
                if (orga!=null && orga.ID==org.ID)
                {
                    MiscRcvLineData rtnLine = new MiscRcvLineData();
                    rtnLine.LineNum = num;
                    rtnLine.ItemCode = Detail.goods_no;
                    rtnLine.RcvQty = Convert.ToDecimal(Detail.num);
                    rtnLine.DeptCode = WH.Department?.Code;
                    rtnLine.RcvWH = item.warehouse_no;
                    //if (order != null)
                    //{
                    //    foreach (DefectChangeSearchDetail searchDetail in order.details_list)
                    //    {
                    //        if (Detail.spec_no == searchDetail.spec_no)
                    //        {
                    //            rtnLine.StoreType = searchDetail.defect ? 2 : 4;
                    //        }

                    //    }
                    //}
                    rtnLine.StoreType = order ? 4 : 2;

                    lines.Add(rtnLine);
                    num += 10;
                }
                
            }
            ShipPlanToShip.OrgCode = org.Code;
            ShipPlanToShip.EntCode = CHelper.EntCode;
            ShipPlanToShip.UserCode = "A001";
            ShipPlanToShip.OptType = CHelper.MiscRcvTransOptype;
            MiscRcvDTO.MiscRcvLineDt = lines;
            ShipPlanToShip.MiscRcvDTO = MiscRcvDTO;
            return JsonConvert.SerializeObject(ShipPlanToShip);
        }
        /// <summary>
        /// 旺店通汇总入库单【盘点入库】创建U9杂收
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// /// <param name="order"></param>
        /// <returns></returns>
        public static string GetWdtBaseSearchStockInMiscRcvTranssByPD(StockInBaseSearchOrder item, Organization org, List<DefectPDdetailSearchDetail> order)
        {
            Organization orga;
            PreStockinMiscRcvTransDto ShipPlanToShip = new PreStockinMiscRcvTransDto();
            MiscRcvSVData MiscRcvDTO = new MiscRcvSVData();
            Warehouse WH = Warehouse.FindByCode(org, item.warehouse_no);
            MiscRcvDTO.BussinessDate = item.check_time;
            MiscRcvDTO.Status = 2;
            MiscRcvDTO.DocType = "MiscRcv006";
            MiscRcvDTO.DocNo = item.stockin_no;
            MiscRcvDTO.Memo = item.remark;
            MiscRcvDTO.OrgCode = org.Code;
            MiscRcvDTO.WHMan = WH?.Manager?.Code;
            MiscRcvDTO.CreatedBy = item.operator_name;
            List<MiscRcvLineData> lines = new List<MiscRcvLineData>();
            int num = 10;
            foreach (var Detail in item.detail_list)
            {
                orga = CHelper.GetOrg(Detail.goods_no, "");
                if (orga != null && orga.ID == org.ID)
                {
                    MiscRcvLineData rtnLine = new MiscRcvLineData();
                    rtnLine.LineNum = num;
                    rtnLine.ItemCode = Detail.goods_no;
                    rtnLine.RcvQty = Convert.ToDecimal(Detail.num);
                    rtnLine.DeptCode = WH.Department?.Code;
                    rtnLine.RcvWH = item.warehouse_no;
                    if (order != null)
                    {
                        foreach (DefectPDdetailSearchDetail searchDetail in order)
                        {
                            if (Detail.goods_no == searchDetail.goods_no)
                            {
                                rtnLine.StoreType = searchDetail.defect==1 ? 2 : 4;
                            }

                        }
                    }

                    lines.Add(rtnLine);
                    num += 10;
                }

            }
            ShipPlanToShip.OrgCode = org.Code;
            ShipPlanToShip.EntCode = CHelper.EntCode;
            ShipPlanToShip.UserCode = "A001";
            ShipPlanToShip.OptType = CHelper.MiscRcvTransOptype;
            MiscRcvDTO.MiscRcvLineDt = lines;
            ShipPlanToShip.MiscRcvDTO = MiscRcvDTO;
            return JsonConvert.SerializeObject(ShipPlanToShip);
        }
        /// <summary>
        /// 旺店通盘点入库单创建U9杂收
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static string GetWdtStockPdInMiscRcvTrans(QueryStockPdInOrder item, Organization org)
        {          
            PreStockinMiscRcvTransDto ShipPlanToShip = new PreStockinMiscRcvTransDto();
            MiscRcvSVData MiscRcvDTO = new MiscRcvSVData();
            Warehouse WH = Warehouse.FindByCode(org, item.warehouse_no);
            MiscRcvDTO.BussinessDate = item.check_time != null?CHelper.GetDateTimeMilliseconds(Convert.ToInt64(item.check_time)):DateTime.Now.ToString();
            MiscRcvDTO.Status = 2;
            MiscRcvDTO.DocType = "MiscRcv006";
            MiscRcvDTO.DocNo = item.order_no;
            MiscRcvDTO.Memo = item.remark;
            MiscRcvDTO.OrgCode = org.Code;
            MiscRcvDTO.WHMan = WH?.Manager?.Code;
            MiscRcvDTO.CreatedBy = item.operator_name;
            List<MiscRcvLineData> lines = new List<MiscRcvLineData>();
            int num = 10;
            foreach (var Detail in item.detail_list)
            {
                MiscRcvLineData rtnLine = new MiscRcvLineData();
                rtnLine.LineNum = num;
                rtnLine.ItemCode = Detail.goods_no;
                rtnLine.RcvQty = Convert.ToDecimal(Detail.goods_count);
                rtnLine.DeptCode = WH.Department?.Code;
                rtnLine.RcvWH = item.warehouse_no;
                rtnLine.StoreType = Detail.defect ? 2 : 4;
                lines.Add(rtnLine);
                num += 10;
            }
            ShipPlanToShip.OrgCode = org.Code;
            ShipPlanToShip.EntCode = CHelper.EntCode;
            ShipPlanToShip.UserCode = "A001";
            ShipPlanToShip.OptType = CHelper.MiscRcvTransOptype;
            MiscRcvDTO.MiscRcvLineDt = lines;
            ShipPlanToShip.MiscRcvDTO = MiscRcvDTO;
            return JsonConvert.SerializeObject(ShipPlanToShip);
        }
        /// <summary>
        /// 旺店通盘点入库单创建U9杂收 包含拆单逻辑
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static string  GetWdtStockPdInMiscRcvTranss(QueryStockPdInOrder item, Organization org)
        {
            Organization  orga = null;
            PreStockinMiscRcvTransDto ShipPlanToShip = new PreStockinMiscRcvTransDto();
            MiscRcvSVData MiscRcvDTO = new MiscRcvSVData();
            Warehouse WH = Warehouse.FindByCode(org, item.warehouse_no);
            MiscRcvDTO.BussinessDate = item.check_time != null ? CHelper.GetDateTimeMilliseconds(Convert.ToInt64(item.check_time)) : DateTime.Now.ToString();
            MiscRcvDTO.Status = 2;
            MiscRcvDTO.DocType = "MiscRcv006";
            MiscRcvDTO.DocNo = item.order_no;
            MiscRcvDTO.Memo = item.remark;
            MiscRcvDTO.OrgCode = org.Code;
            MiscRcvDTO.WHMan = WH?.Manager?.Code;
            MiscRcvDTO.CreatedBy = item.operator_name;
            List<MiscRcvLineData> lines = new List<MiscRcvLineData>();
            int num = 10;
            foreach (var Detail in item.detail_list)
            {
                orga = CHelper.GetOrg(Detail.goods_no, "");
                if (orga != null && orga.ID == org.ID)
                {
                    MiscRcvLineData rtnLine = new MiscRcvLineData();
                    rtnLine.LineNum = num;
                    rtnLine.ItemCode = Detail.goods_no;
                    rtnLine.RcvQty = Convert.ToDecimal(Detail.goods_count);
                    rtnLine.DeptCode = WH.Department?.Code;
                    rtnLine.RcvWH = item.warehouse_no;
                    rtnLine.StoreType = Detail.defect ? 2 : 4;
                    lines.Add(rtnLine);
                    num += 10;
                }


            }
            ShipPlanToShip.OrgCode = org.Code;
            ShipPlanToShip.EntCode = CHelper.EntCode;
            ShipPlanToShip.UserCode = "A001";
            ShipPlanToShip.OptType = CHelper.MiscRcvTransOptype;
            MiscRcvDTO.MiscRcvLineDt = lines;
            ShipPlanToShip.MiscRcvDTO = MiscRcvDTO;
            return JsonConvert.SerializeObject(ShipPlanToShip);
        }
        /// <summary>
        /// 旺店通其它出库盘亏出库创建U9杂发
        /// </summary>
        /// <param name="misc"></param>
        /// <param name="SrcOrderNo"></param>
        /// <returns></returns>
        public static string GetWdtLossMiscShip(MiscShipOrderDto item, Organization org)
        {
            LossMiscShipDto LossMiscShip = new LossMiscShipDto();
            MiscShipSVData MiscShipDTO = new MiscShipSVData();
            Warehouse WH = Warehouse.FindByCode(org, item.warehouse_no);
            MiscShipDTO.BussinessDate = DateTime.Now.ToString();
            MiscShipDTO.Status = 2;
            MiscShipDTO.DocType = "MiscShip004";
            MiscShipDTO.DocNo = item.order_no;
            MiscShipDTO.OrgCode = org.Code;
            MiscShipDTO.WHMan = WH?.Manager?.Code;
            List<MiscShipLineData> lines = new List<MiscShipLineData>();
            int num = 10;
            foreach (var Detail in item.detail_list)
            {
                MiscShipLineData rtnLine = new MiscShipLineData();
                rtnLine.LineNum = num;
                rtnLine.ItemCode = Detail.goods_no;
                rtnLine.IssueQty = Convert.ToInt32(Detail.goods_count);
                rtnLine.DeptCode = WH.Department?.Code;
                rtnLine.IssueWH = item.warehouse_no;
                lines.Add(rtnLine);
                num += 10;
            }
            LossMiscShip.OrgCode = org.Code;
            LossMiscShip.EntCode = CHelper.EntCode;
            LossMiscShip.UserCode = "A001";
            LossMiscShip.OptType = CHelper.MiscShipOptype;
            MiscShipDTO.MiscShipLineDt = lines;
            LossMiscShip.MiscShipDTO = MiscShipDTO;
            return JsonConvert.SerializeObject(LossMiscShip);
        }
        /// <summary>
        /// 旺店通其它出库业务单盘亏出库创建U9杂发
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static string GetWdtStockOutLossMiscShip(StockOutOrderDto item, Organization org)
        {
            LossMiscShipDto LossMiscShip = new LossMiscShipDto();
            MiscShipSVData MiscShipDTO = new MiscShipSVData();
            Warehouse WH = Warehouse.FindByCode(org, item.warehouse_no);
            MiscShipDTO.BussinessDate = DateTime.Now.ToString();
            MiscShipDTO.Status = 2;
            MiscShipDTO.DocType = "MiscShip004";
            MiscShipDTO.DocNo = item.other_out_no;
            MiscShipDTO.OrgCode = org.Code;
            MiscShipDTO.WHMan = WH?.Manager?.Code;
            List<MiscShipLineData> lines = new List<MiscShipLineData>();
            int num = 10;
            foreach (var Detail in item.detail_list)
            {
                MiscShipLineData rtnLine = new MiscShipLineData();
                rtnLine.LineNum = num;
                rtnLine.ItemCode = Detail.goods_no;
                rtnLine.IssueQty = Convert.ToDecimal(Detail.out_num);
                rtnLine.DeptCode = WH.Department?.Code;
                rtnLine.IssueWH = item.warehouse_no;
                rtnLine.StoreType = 4;
                lines.Add(rtnLine);
                num += 10;
            }
            LossMiscShip.OrgCode = org.Code;
            LossMiscShip.EntCode = CHelper.EntCode;
            LossMiscShip.UserCode = "A001";
            LossMiscShip.OptType = CHelper.MiscShipOptype;
            MiscShipDTO.MiscShipLineDt = lines;
            LossMiscShip.MiscShipDTO = MiscShipDTO;
            return JsonConvert.SerializeObject(LossMiscShip);
        }

        /// <summary>
        /// 旺店通其它出库业务单库存异动出库创建U9杂发
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static string GetWdtAbnormalLossMiscShip(StockOutOrderDto item, Organization org)
        {
            LossMiscShipDto LossMiscShip = new LossMiscShipDto();
            MiscShipSVData MiscShipDTO = new MiscShipSVData();
            Warehouse WH = Warehouse.FindByCode(org, item.warehouse_no);
            MiscShipDTO.BussinessDate = DateTime.Now.ToString();
            MiscShipDTO.Status = 2;
            MiscShipDTO.DocType = "MiscShip007";
            MiscShipDTO.DocNo = item.other_out_no;
            MiscShipDTO.OrgCode = org.Code;
            MiscShipDTO.WHMan = WH?.Manager?.Code;
            MiscShipDTO.CreatedBy = item.employee_name;
            List<MiscShipLineData> lines = new List<MiscShipLineData>();
            int num = 10;
            foreach (var Detail in item.detail_list)
            {
                MiscShipLineData rtnLine = new MiscShipLineData();
                rtnLine.LineNum = num;
                rtnLine.ItemCode = Detail.goods_no;
                rtnLine.IssueQty = Convert.ToDecimal(Detail.out_num);
                rtnLine.DeptCode = WH.Department?.Code;
                rtnLine.IssueWH = item.warehouse_no;
                rtnLine.StoreType = Detail.defect ? 2 : 4;
                lines.Add(rtnLine);
                num += 10;
            }
            LossMiscShip.OrgCode = org.Code;
            LossMiscShip.EntCode = CHelper.EntCode;
            LossMiscShip.UserCode = "A001";
            LossMiscShip.OptType = CHelper.MiscShipOptype;
            MiscShipDTO.MiscShipLineDt = lines;
            LossMiscShip.MiscShipDTO = MiscShipDTO;
            return JsonConvert.SerializeObject(LossMiscShip);
        }

        /// <summary>
        /// 旺店通出库明细单【盘亏出库】创建U9杂发
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static string GetWdtBaseStockOutLossMiscShip(StockOutBaseSearchOrder item, Organization org)
        {
            LossMiscShipDto LossMiscShip = new LossMiscShipDto();
            MiscShipSVData MiscShipDTO = new MiscShipSVData();
            Warehouse WH = Warehouse.FindByCode(org, item.warehouse_no);
            MiscShipDTO.BussinessDate = DateTime.Now.ToString();
            MiscShipDTO.Status = 2;
            MiscShipDTO.DocType = "MiscShip006";
            MiscShipDTO.DocNo = item.stockout_no;
            MiscShipDTO.OrgCode = org.Code;
            MiscShipDTO.WHMan = WH?.Manager?.Code;
            List<MiscShipLineData> lines = new List<MiscShipLineData>();
            int num = 10;
            foreach (var Detail in item.detail_list)
            {
                MiscShipLineData rtnLine = new MiscShipLineData();
                rtnLine.LineNum = num;
                rtnLine.ItemCode = Detail.goods_no;
                rtnLine.IssueQty = Convert.ToDecimal(Detail.num);
                rtnLine.DeptCode = WH.Department?.Code;
                rtnLine.IssueWH = item.warehouse_no;
                rtnLine.StoreType = Detail.defect ? 2 : 4;
                lines.Add(rtnLine);
                num += 10;
            }
            LossMiscShip.OrgCode = org.Code;
            LossMiscShip.EntCode = CHelper.EntCode;
            LossMiscShip.UserCode = "A001";
            LossMiscShip.OptType = CHelper.MiscShipOptype;
            MiscShipDTO.MiscShipLineDt = lines;
            LossMiscShip.MiscShipDTO = MiscShipDTO;
            return JsonConvert.SerializeObject(LossMiscShip);
        }
        /// <summary>
        /// 旺店通出库明细单【正残转换出库】创建U9杂发
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static string  GetWdtBaseSearchStockOutLossMiscShip(StockOutBaseSearchOrder item, Organization org, bool order)
        {
            LossMiscShipDto LossMiscShip = new LossMiscShipDto();
            MiscShipSVData MiscShipDTO = new MiscShipSVData();
            Warehouse WH = Warehouse.FindByCode(org, item.warehouse_no);
            MiscShipDTO.BussinessDate = item.consign_time != null ? item.consign_time : DateTime.Now.ToString();
            MiscShipDTO.Status = 2;
            MiscShipDTO.DocType = "MiscShip005";
            MiscShipDTO.DocNo = item.stockout_no;
            MiscShipDTO.OrgCode = org.Code;
            MiscShipDTO.WHMan = WH?.Manager?.Code;
            MiscShipDTO.CreatedBy = item.operator_name;
            List<MiscShipLineData> lines = new List<MiscShipLineData>();
            int num = 10;
            foreach (var Detail in item.detail_list)
            {
                MiscShipLineData rtnLine = new MiscShipLineData();
                rtnLine.LineNum = num;
                rtnLine.ItemCode = Detail.goods_no;
                rtnLine.IssueQty = Convert.ToDecimal(Detail.num);
                rtnLine.DeptCode = WH.Department?.Code;
                rtnLine.IssueWH = item.warehouse_no;
                //if (order!=null)
                //{
                //    foreach (DefectChangeSearchDetail searchDetail in order.details_list)
                //    {
                //        if (Detail.spec_no == searchDetail.spec_no)
                //        {
                //            rtnLine.StoreType = searchDetail.defect ? 2 : 4;
                //        }

                //    }
                //}
                rtnLine.StoreType = order ? 2 : 4;
                lines.Add(rtnLine);
                num += 10;
            }
            LossMiscShip.OrgCode = org.Code;
            LossMiscShip.EntCode = CHelper.EntCode;
            LossMiscShip.UserCode = "A001";
            LossMiscShip.OptType = CHelper.MiscShipOptype;
            MiscShipDTO.MiscShipLineDt = lines;
            LossMiscShip.MiscShipDTO = MiscShipDTO;
            return JsonConvert.SerializeObject(LossMiscShip);
        }
        /// <summary>
        /// 旺店通出库明细单【盘点出库】创建U9杂发
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static string  GetWdtBaseSearchStockOutLossMiscShipByPD(StockOutBaseSearchOrder item, Organization org, bool order)
        {
            LossMiscShipDto LossMiscShip = new LossMiscShipDto();
            MiscShipSVData MiscShipDTO = new MiscShipSVData();
            Warehouse WH = Warehouse.FindByCode(org, item.warehouse_no);
            MiscShipDTO.BussinessDate = item.consign_time != null ? item.consign_time : DateTime.Now.ToString();
            MiscShipDTO.Status = 2;
            MiscShipDTO.DocType = "MiscShip006";
            MiscShipDTO.DocNo = item.stockout_no;
            MiscShipDTO.OrgCode = org.Code;
            MiscShipDTO.WHMan = WH?.Manager?.Code;
            MiscShipDTO.CreatedBy = item.operator_name;
            List<MiscShipLineData> lines = new List<MiscShipLineData>();
            int num = 10;
            foreach (var Detail in item.detail_list)
            {
                MiscShipLineData rtnLine = new MiscShipLineData();
                rtnLine.LineNum = num;
                rtnLine.ItemCode = Detail.goods_no;
                rtnLine.IssueQty = Convert.ToDecimal(Detail.num);
                rtnLine.DeptCode = WH.Department?.Code;
                rtnLine.IssueWH = item.warehouse_no;
                //if (order!=null)
                //{
                //    foreach (DefectChangeSearchDetail searchDetail in order.details_list)
                //    {
                //        if (Detail.spec_no == searchDetail.spec_no)
                //        {
                //            rtnLine.StoreType = searchDetail.defect ? 2 : 4;
                //        }

                //    }
                //}
                rtnLine.StoreType = order ? 2 : 4;
                lines.Add(rtnLine);
                num += 10;
            }
            LossMiscShip.OrgCode = org.Code;
            LossMiscShip.EntCode = CHelper.EntCode;
            LossMiscShip.UserCode = "A001";
            LossMiscShip.OptType = CHelper.MiscShipOptype;
            MiscShipDTO.MiscShipLineDt = lines;
            LossMiscShip.MiscShipDTO = MiscShipDTO;
            return JsonConvert.SerializeObject(LossMiscShip);
        }
        /// <summary>
        /// 生产出库单创建U9杂发
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static string  GetWdtProcessStockOutLossMiscShip(StockOutProcessOrder item, Organization org)
        {
            LossMiscShipDto LossMiscShip = new LossMiscShipDto();
            MiscShipSVData MiscShipDTO = new MiscShipSVData();
            Warehouse WH = Warehouse.FindByCode(org, item.warehouse_no);
            MiscShipDTO.BussinessDate = DateTime.Now.ToString();
            MiscShipDTO.Status = 2;
            MiscShipDTO.DocType = "MiscShip005";
            MiscShipDTO.DocNo = item.stockout_no;
            MiscShipDTO.OrgCode = org.Code;
            MiscShipDTO.WHMan = WH?.Manager?.Code;
            MiscShipDTO.CreatedBy = item.operator_name;
            List<MiscShipLineData> lines = new List<MiscShipLineData>();
            int num = 10;
            foreach (var Detail in item.detail_list)
            {
                MiscShipLineData rtnLine = new MiscShipLineData();
                rtnLine.LineNum = num;
                rtnLine.ItemCode = Detail.goods_no;
                rtnLine.IssueQty = Convert.ToDecimal(Detail.num);
                rtnLine.DeptCode = WH.Department?.Code;
                rtnLine.IssueWH = item.warehouse_no;
                rtnLine.StoreType = Detail.defect ? 2 : 4;

                lines.Add(rtnLine);
                num += 10;
            }
            LossMiscShip.OrgCode = org.Code;
            LossMiscShip.EntCode = CHelper.EntCode;
            LossMiscShip.UserCode = "A001";
            LossMiscShip.OptType = CHelper.MiscShipOptype;
            MiscShipDTO.MiscShipLineDt = lines;
            LossMiscShip.MiscShipDTO = MiscShipDTO;
            return JsonConvert.SerializeObject(LossMiscShip);
        }
        /// <summary>
        /// 生产单-形态转换单
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static string  GetWdtProcessStockShiftDoc(StockProcessOrder item, Organization org)
        {
            bool isproduct = false;
            ShiftDocDto LossMiscShip = new ShiftDocDto();
            TransferFormSVData MiscShipDTO  =new TransferFormSVData();
            Warehouse WH = Warehouse.FindByCode(org, item.in_warehouse_no);
            MiscShipDTO.BussinessDate = item.modified;
            MiscShipDTO.Status = 0;
            MiscShipDTO.DocType = "TransForm001";
            MiscShipDTO.DocNo = item.process_no;
            MiscShipDTO.CreatedBy = item.operator_name;
            Department dept = Department.Finder.Find("Name=@Name",new OqlParam("Name",item.remark));
            MiscShipDTO.Memo = item.remark;
            PubPriSVData pub = new PubPriSVData();
            pub.PrivateDescSeg1 = item.process_no;
            MiscShipDTO.PubPriDt = pub;
            List<TransferFormLSVData> lines = new List<TransferFormLSVData>();
            var result = item.detail_list.GroupBy(p => p.is_product)  
                    .Where(g => g.Count() == 1);
            List<IGrouping<bool, StockProcessDetail>> filteredResult = result.ToList();
            foreach (var  item1 in filteredResult)
            {
                foreach (var  StockProcessDetail in item1)
                {
                    isproduct = StockProcessDetail.is_product;
                }
            }
            int num = 10;
            TransferFormLSVData transferFormLSVData = new TransferFormLSVData();
            List<TransferFormSLSVData>  TransferFormChilds =new List<TransferFormSLSVData>();
            foreach (var Detail in item.detail_list)
            {
                if (Detail.is_product == isproduct)
                {                  
                    transferFormLSVData.DocLineNo = "10";
                    ItemMaster itemMaster = ItemMaster.Finder.Find("Code1=@Code1 and Org=@Org",new OqlParam("Code1",Detail.spec_no),new OqlParam("Org",org.ID));
                    transferFormLSVData.ItemCode = itemMaster?.Code;
                    transferFormLSVData.HandleDept = dept?.Code;
                    transferFormLSVData.StoreUOMQty = isproduct == false ? Detail.out_num : Detail.in_num;
                    transferFormLSVData.WhCode = isproduct == false ? item.out_warehouse_no : item.in_warehouse_no;
                    transferFormLSVData.Memo = Detail.remark;
                    transferFormLSVData.TransferType = isproduct == false ? 0 : 1;
                }
                else
                {
                    TransferFormSLSVData transferFormSLSV = new TransferFormSLSVData();
                    transferFormSLSV.DocSubLineNo = num.ToString();
                    ItemMaster itemMaster = ItemMaster.Finder.Find("Code1=@Code1 and Org=@Org", new OqlParam("Code1", Detail.spec_no), new OqlParam("Org", org.ID));
                    transferFormSLSV.ItemCode = itemMaster?.Code;
                    transferFormSLSV.StoreUOMQty = isproduct == true ? Detail.out_num : Detail.in_num;
                    transferFormSLSV.WhCode = isproduct == true ? item.out_warehouse_no : item.in_warehouse_no;
                    transferFormSLSV.Memo = Detail.remark;
                    transferFormSLSV.HandleDept = dept?.Code;
                    transferFormSLSV.TransferType = isproduct == true ? 0 : 1;
                    TransferFormChilds.Add(transferFormSLSV);
                    num += 10;
                }
               
            }
            transferFormLSVData.TransferFormChildDt = TransferFormChilds;
            lines.Add(transferFormLSVData);
            LossMiscShip.OrgCode = org.Code;
            LossMiscShip.EntCode = CHelper.EntCode;
            LossMiscShip.UserCode = "A001";
            LossMiscShip.OptType = CHelper.ShiftDocOptype;
            MiscShipDTO.TransferFormLineData = lines;
            LossMiscShip.TransferFormDTO = MiscShipDTO;
            return JsonConvert.SerializeObject(LossMiscShip);
        }
        /// <summary>
        /// 旺店通出库明细单【正残转换出库】创建U9杂发 cc
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static string  GetWdtBaseSearchStockOutLossMiscShips(StockOutBaseSearchOrder item, Organization org, bool order)
        {
            Organization orga;
            LossMiscShipDto LossMiscShip = new LossMiscShipDto();
            MiscShipSVData MiscShipDTO = new MiscShipSVData();
            Warehouse WH = Warehouse.FindByCode(org, item.warehouse_no);
            MiscShipDTO.BussinessDate = item.consign_time != null ? item.consign_time : DateTime.Now.ToString();
            MiscShipDTO.Status = 2;
            MiscShipDTO.DocType = "MiscShip005";
            MiscShipDTO.DocNo = item.stockout_no;
            MiscShipDTO.OrgCode = org.Code;
            MiscShipDTO.WHMan = WH?.Manager?.Code;
            MiscShipDTO.CreatedBy = item.operator_name;
            List<MiscShipLineData> lines = new List<MiscShipLineData>();
            int num = 10;
            foreach (var Detail in item.detail_list)
            {
                orga = CHelper.GetOrg(Detail.goods_no,"");
                if (orga!=null && orga.ID==org.ID)
                {
                    MiscShipLineData rtnLine = new MiscShipLineData();
                    rtnLine.LineNum = num;
                    rtnLine.ItemCode = Detail.goods_no;
                    rtnLine.IssueQty = Convert.ToDecimal(Detail.num);
                    rtnLine.DeptCode = WH.Department?.Code;
                    rtnLine.IssueWH = item.warehouse_no;
                    //if (order != null)
                    //{
                    //    foreach (DefectChangeSearchDetail searchDetail in order.details_list)
                    //    {
                    //        if (Detail.spec_no == searchDetail.spec_no)
                    //        {
                    //            rtnLine.StoreType = searchDetail.defect ? 2 : 4;
                    //        }

                    //    }
                    //}
                    rtnLine.StoreType = order ? 2 : 4;
                    lines.Add(rtnLine);
                    num += 10;
                }
               
            }
            LossMiscShip.OrgCode = org.Code;
            LossMiscShip.EntCode = CHelper.EntCode;
            LossMiscShip.UserCode = "A001";
            LossMiscShip.OptType = CHelper.MiscShipOptype;
            MiscShipDTO.MiscShipLineDt = lines;
            LossMiscShip.MiscShipDTO = MiscShipDTO;
            return JsonConvert.SerializeObject(LossMiscShip);
        }
        /// <summary>
        /// 旺店通出库明细单【盘点出库】创建U9杂发 cc
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static string GetWdtBaseSearchStockOutLossMiscShipsByPD(StockOutBaseSearchOrder item, Organization org, List<DefectPDdetailSearchDetail> order)
        {
            Organization orga;
            LossMiscShipDto LossMiscShip = new LossMiscShipDto();
            MiscShipSVData MiscShipDTO = new MiscShipSVData();
            Warehouse WH = Warehouse.FindByCode(org, item.warehouse_no);
            MiscShipDTO.BussinessDate = item.consign_time != null ? item.consign_time : DateTime.Now.ToString();
            MiscShipDTO.Status = 2;
            MiscShipDTO.DocType = "MiscShip006";
            MiscShipDTO.DocNo = item.stockout_no;
            MiscShipDTO.OrgCode = org.Code;
            MiscShipDTO.WHMan = WH?.Manager?.Code;
            MiscShipDTO.CreatedBy = item.operator_name;
            List<MiscShipLineData> lines = new List<MiscShipLineData>();
            int num = 10;
            foreach (var Detail in item.detail_list)
            {
                orga = CHelper.GetOrg(Detail.goods_no, "");
                if (orga != null && orga.ID == org.ID)
                {
                    MiscShipLineData rtnLine = new MiscShipLineData();
                    rtnLine.LineNum = num;
                    rtnLine.ItemCode = Detail.goods_no;
                    rtnLine.IssueQty = Convert.ToDecimal(Detail.num);
                    rtnLine.DeptCode = WH.Department?.Code;
                    rtnLine.IssueWH = item.warehouse_no;
                    if (order != null)
                    {
                        foreach (DefectPDdetailSearchDetail searchDetail in order)
                        {
                            if (Detail.goods_no == searchDetail.goods_no)
                            {
                                rtnLine.StoreType = searchDetail.defect==1 ? 2 : 4;
                            }

                        }
                    }
                    lines.Add(rtnLine);
                    num += 10;
                }

            }
            LossMiscShip.OrgCode = org.Code;
            LossMiscShip.EntCode = CHelper.EntCode;
            LossMiscShip.UserCode = "A001";
            LossMiscShip.OptType = CHelper.MiscShipOptype;
            MiscShipDTO.MiscShipLineDt = lines;
            LossMiscShip.MiscShipDTO = MiscShipDTO;
            return JsonConvert.SerializeObject(LossMiscShip);
        }
        /// <summary>
        /// 旺店通出库明细单【盘亏出库】创建U9杂发
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static string GetWdtStockPdOutMiscShip(QueryStockPdOutOrder item, Organization org)
        {
            LossMiscShipDto LossMiscShip = new LossMiscShipDto();
            MiscShipSVData MiscShipDTO = new MiscShipSVData();
            Warehouse WH = Warehouse.FindByCode(org, item.warehouse_no);
            MiscShipDTO.BussinessDate = item.consign_time != null ? CHelper.GetDateTimeMilliseconds(Convert.ToInt64(item.consign_time)) : DateTime.Now.ToString();
            MiscShipDTO.Status = 2;
            MiscShipDTO.DocType = "MiscShip006";
            MiscShipDTO.DocNo = item.order_no;
            MiscShipDTO.OrgCode = org.Code;
            MiscShipDTO.WHMan = WH?.Manager?.Code;
            MiscShipDTO.CreatedBy = item.operator_name;
            List<MiscShipLineData> lines = new List<MiscShipLineData>();
            int num = 10;
            foreach (var Detail in item.detail_list)
            {
                MiscShipLineData rtnLine = new MiscShipLineData();
                rtnLine.LineNum = num;
                rtnLine.ItemCode = Detail.goods_no;
                rtnLine.IssueQty = Convert.ToDecimal(Detail.goods_count);
                rtnLine.DeptCode = WH.Department?.Code;
                rtnLine.IssueWH = item.warehouse_no;
                rtnLine.StoreType = Detail.defect ? 2 : 4;
                lines.Add(rtnLine);
                num += 10;
            }
            LossMiscShip.OrgCode = org.Code;
            LossMiscShip.EntCode = CHelper.EntCode;
            LossMiscShip.UserCode = "A001";
            LossMiscShip.OptType = CHelper.MiscShipOptype;
            MiscShipDTO.MiscShipLineDt = lines;
            LossMiscShip.MiscShipDTO = MiscShipDTO;
            return JsonConvert.SerializeObject(LossMiscShip);
        }
        /// <summary>
        /// 旺店通出库明细单【盘亏出库】创建U9杂发拆单逻辑
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static string  GetWdtStockPdOutMiscShips(QueryStockPdOutOrder item, Organization org)
        {
            Organization orga;
            LossMiscShipDto LossMiscShip = new LossMiscShipDto();
            MiscShipSVData MiscShipDTO = new MiscShipSVData();
            Warehouse WH = Warehouse.FindByCode(org, item.warehouse_no);
            MiscShipDTO.BussinessDate = item.consign_time != null ? CHelper.GetDateTimeMilliseconds(Convert.ToInt64(item.consign_time)) : DateTime.Now.ToString();
            MiscShipDTO.Status = 2;
            MiscShipDTO.DocType = "MiscShip006";
            MiscShipDTO.DocNo = item.order_no;
            MiscShipDTO.OrgCode = org.Code;
            MiscShipDTO.WHMan = WH?.Manager?.Code;
            MiscShipDTO.CreatedBy = item.operator_name;
            List<MiscShipLineData> lines = new List<MiscShipLineData>();
            int num = 10;
            foreach (var Detail in item.detail_list)
            {
                orga = CHelper.GetOrg(Detail.goods_no,"");
                if (orga!=null && orga.ID==org.ID)
                {
                    MiscShipLineData rtnLine = new MiscShipLineData();
                    rtnLine.LineNum = num;
                    rtnLine.ItemCode = Detail.goods_no;
                    rtnLine.IssueQty = Convert.ToDecimal(Detail.goods_count);
                    rtnLine.DeptCode = WH.Department?.Code;
                    rtnLine.IssueWH = item.warehouse_no;
                    rtnLine.StoreType = Detail.defect ? 2 : 4;
                    lines.Add(rtnLine);
                    num += 10;
                }
              
            }
            LossMiscShip.OrgCode = org.Code;
            LossMiscShip.EntCode = CHelper.EntCode;
            LossMiscShip.UserCode = "A001";
            LossMiscShip.OptType = CHelper.MiscShipOptype;
            MiscShipDTO.MiscShipLineDt = lines;
            LossMiscShip.MiscShipDTO = MiscShipDTO;
            return JsonConvert.SerializeObject(LossMiscShip);
        }
        /// <summary>
        /// 其他出库 正残转换 杂发
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static string GetWdtShiftMiscShip(StockOutOrderDto item, Organization org)
        {
            LossMiscShipDto LossMiscShip = new LossMiscShipDto();
            MiscShipSVData MiscShipDTO = new MiscShipSVData();
            Warehouse WH = Warehouse.FindByCode(org, item.warehouse_no);
            MiscShipDTO.BussinessDate = item.modified != null ? CHelper.GetDateTimeMilliseconds(Convert.ToInt64(item.modified)) : DateTime.Now.ToString();
            MiscShipDTO.Status = 2;
            MiscShipDTO.DocType = "MiscShip005";
            MiscShipDTO.DocNo = item.other_out_no;
            MiscShipDTO.OrgCode = org.Code;
            MiscShipDTO.WHMan = WH?.Manager?.Code;
            MiscShipDTO.Memo = item.remark;
            MiscShipDTO.CreatedBy = item.employee_name;
            List<MiscShipLineData> lines = new List<MiscShipLineData>();
            int num = 10;
            foreach (var Detail in item.detail_list)
            {
                MiscShipLineData rtnLine = new MiscShipLineData();
                rtnLine.LineNum = num;
                rtnLine.ItemCode = Detail.goods_no;
                rtnLine.IssueQty = Convert.ToDecimal(Detail.out_num);
                rtnLine.DeptCode = WH.Department?.Code;
                rtnLine.IssueWH = item.warehouse_no;
                rtnLine.StoreType = Detail.defect ? 2 : 4;

                lines.Add(rtnLine);
                num += 10;
            }
            LossMiscShip.OrgCode = org.Code;
            LossMiscShip.EntCode = CHelper.EntCode;
            LossMiscShip.UserCode = "A001";
            LossMiscShip.OptType = CHelper.MiscShipOptype;
            MiscShipDTO.MiscShipLineDt = lines;
            LossMiscShip.MiscShipDTO = MiscShipDTO;
            return JsonConvert.SerializeObject(LossMiscShip);
        }
        /// <summary>
        /// 其他出库 正残转换 杂发 cd
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static string GetWdtShiftMiscShipsZC(StockOutOrderDto item, Organization org)
        {
            Organization orga;
            LossMiscShipDto LossMiscShip = new LossMiscShipDto();
            MiscShipSVData MiscShipDTO = new MiscShipSVData();
            Warehouse WH = Warehouse.FindByCode(org, item.warehouse_no);
            MiscShipDTO.BussinessDate = item.modified != null ? CHelper.GetDateTimeMilliseconds(Convert.ToInt64(item.modified)) : DateTime.Now.ToString();
            MiscShipDTO.Status = 2;
            MiscShipDTO.DocType = "MiscShip005";
            MiscShipDTO.DocNo = item.other_out_no;
            MiscShipDTO.OrgCode = org.Code;
            MiscShipDTO.WHMan = WH?.Manager?.Code;
            MiscShipDTO.Memo = item.remark;
            MiscShipDTO.CreatedBy = item.employee_name;
            List<MiscShipLineData> lines = new List<MiscShipLineData>();
            int num = 10;
            foreach (var Detail in item.detail_list)
            {
                orga = CHelper.GetOrg(Detail.goods_no, "");
                if (orga != null && orga.ID == org.ID)
                {
                    MiscShipLineData rtnLine = new MiscShipLineData();
                    rtnLine.LineNum = num;
                    rtnLine.ItemCode = Detail.goods_no;
                    rtnLine.IssueQty = Convert.ToDecimal(Detail.out_num);
                    rtnLine.DeptCode = WH.Department?.Code;
                    rtnLine.IssueWH = item.warehouse_no;
                    rtnLine.StoreType = Detail.defect ? 2 : 4;

                    lines.Add(rtnLine);
                    num += 10;
                }

            }
            LossMiscShip.OrgCode = org.Code;
            LossMiscShip.EntCode = CHelper.EntCode;
            LossMiscShip.UserCode = "A001";
            LossMiscShip.OptType = CHelper.MiscShipOptype;
            MiscShipDTO.MiscShipLineDt = lines;
            LossMiscShip.MiscShipDTO = MiscShipDTO;
            return JsonConvert.SerializeObject(LossMiscShip);
        }
        /// <summary>
        /// 其他出库 正残转换 杂发 cd
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static string  GetWdtShiftMiscShips(StockOutOrderDto item, Organization org)
        {
            Organization orga;
            LossMiscShipDto LossMiscShip = new LossMiscShipDto();
            MiscShipSVData MiscShipDTO = new MiscShipSVData();
            Warehouse WH = Warehouse.FindByCode(org, item.warehouse_no);
            MiscShipDTO.BussinessDate = item.modified != null ? CHelper.GetDateTimeMilliseconds(Convert.ToInt64(item.modified)) : DateTime.Now.ToString();
            MiscShipDTO.Status = 2;
            MiscShipDTO.DocType = "MiscShip007";
            MiscShipDTO.DocNo = item.other_out_no;
            MiscShipDTO.OrgCode = org.Code;
            MiscShipDTO.WHMan = WH?.Manager?.Code;
            MiscShipDTO.Memo = item.remark;
            MiscShipDTO.CreatedBy = item.employee_name;
            List<MiscShipLineData> lines = new List<MiscShipLineData>();
            int num = 10;
            foreach (var Detail in item.detail_list)
            {
                orga = CHelper.GetOrg(Detail.goods_no,"");
                if (orga!=null && orga.ID==org.ID)
                {
                    MiscShipLineData rtnLine = new MiscShipLineData();
                    rtnLine.LineNum = num;
                    rtnLine.ItemCode = Detail.goods_no;
                    rtnLine.IssueQty = Convert.ToDecimal(Detail.out_num);
                    rtnLine.DeptCode = WH.Department?.Code;
                    rtnLine.IssueWH = item.warehouse_no;
                    rtnLine.StoreType = Detail.defect ? 2 : 4;

                    lines.Add(rtnLine);
                    num += 10;
                }
              
            }
            LossMiscShip.OrgCode = org.Code;
            LossMiscShip.EntCode = CHelper.EntCode;
            LossMiscShip.UserCode = "A001";
            LossMiscShip.OptType = CHelper.MiscShipOptype;
            MiscShipDTO.MiscShipLineDt = lines;
            LossMiscShip.MiscShipDTO = MiscShipDTO;
            return JsonConvert.SerializeObject(LossMiscShip);
        }
        /// <summary>
        /// 外仓调整出库 杂发
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static string GetWdtOuterOutLossMiscShip(OuterOutOrderDto item, Organization org)
        {
            LossMiscShipDto LossMiscShip = new LossMiscShipDto();
            MiscShipSVData MiscShipDTO = new MiscShipSVData();
            Warehouse WH = Warehouse.FindByCode(org, item.warehouse_no);
            MiscShipDTO.BussinessDate = item.modified;
            MiscShipDTO.Status = 2;
            MiscShipDTO.DocType = "MiscShip004";
            MiscShipDTO.DocNo = item.outer_out_no;
            MiscShipDTO.OrgCode = org.Code;
            MiscShipDTO.WHMan = WH?.Manager?.Code;
            MiscShipDTO.CreatedBy = item.creator_name;
            List<MiscShipLineData> lines = new List<MiscShipLineData>();
            int num = 10;
            foreach (var Detail in item.detail_list)
            {
                MiscShipLineData rtnLine = new MiscShipLineData();
                rtnLine.LineNum = num;
                rtnLine.ItemCode = Detail.goods_no;
                rtnLine.IssueQty = Convert.ToDecimal(Detail.num);
                rtnLine.DeptCode = WH.Department?.Code;
                rtnLine.IssueWH = item.warehouse_no;
                rtnLine.StoreType = Detail.defect ? 2 : 4;
                lines.Add(rtnLine);
                num += 10;
            }
            LossMiscShip.OrgCode = org.Code;
            LossMiscShip.EntCode = CHelper.EntCode;
            LossMiscShip.UserCode = "A001";
            LossMiscShip.OptType = CHelper.MiscShipOptype;
            MiscShipDTO.MiscShipLineDt = lines;
            LossMiscShip.MiscShipDTO = MiscShipDTO;
            return JsonConvert.SerializeObject(LossMiscShip);
        }
        /// <summary>
        /// 外仓调整出库 杂发 拆单逻辑
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static string  GetWdtOuterOutLossMiscShips(OuterOutOrderDto item, Organization org)
        {
            Organization orga;
            LossMiscShipDto LossMiscShip = new LossMiscShipDto();
            MiscShipSVData MiscShipDTO = new MiscShipSVData();
            Warehouse WH = Warehouse.FindByCode(org, item.warehouse_no);
            MiscShipDTO.BussinessDate = item.modified;
            MiscShipDTO.Status = 2;
            MiscShipDTO.DocType = "MiscShip004";
            MiscShipDTO.DocNo = item.outer_out_no;
            MiscShipDTO.OrgCode = org.Code;
            MiscShipDTO.WHMan = WH?.Manager?.Code;
            MiscShipDTO.CreatedBy = item.creator_name;
            List<MiscShipLineData> lines = new List<MiscShipLineData>();
            int num = 10;
            foreach (var Detail in item.detail_list)
            {
                orga = CHelper.GetOrg(Detail.goods_no,"");
                if (orga!=null && orga.ID==org.ID)
                {
                    MiscShipLineData rtnLine = new MiscShipLineData();
                    rtnLine.LineNum = num;
                    rtnLine.ItemCode = Detail.goods_no;
                    rtnLine.IssueQty = Convert.ToDecimal(Detail.num);
                    rtnLine.DeptCode = WH.Department?.Code;
                    rtnLine.IssueWH = item.warehouse_no;
                    rtnLine.StoreType = Detail.defect ? 2 : 4;
                    lines.Add(rtnLine);
                    num += 10;
                }
          
            }
            LossMiscShip.OrgCode = org.Code;
            LossMiscShip.EntCode = CHelper.EntCode;
            LossMiscShip.UserCode = "A001";
            LossMiscShip.OptType = CHelper.MiscShipOptype;
            MiscShipDTO.MiscShipLineDt = lines;
            LossMiscShip.MiscShipDTO = MiscShipDTO;
            return JsonConvert.SerializeObject(LossMiscShip);
        }
        /// <summary>
        /// 创建标准收货单
        /// </summary>
        /// <param name="item"></param>
        /// <param name="OrgCode"></param>
        /// <returns></returns>
        public static string GetWDTReceivement(PurchaseOrder misc, StockinPurchaseOrderDto item, Organization org)
        {
            QMRcvDto QMRcvDto = new QMRcvDto();
            QMRcvDto.EntCode = CHelper.EntCode;
            QMRcvDto.UserCode = "A001";
            QMRcvDto.OrgCode = org.Code;
            QMRcvDto.OptType = CHelper.ReceivementSrcPOOptype;
            RCVDTO rcvdto = new RCVDTO();
            rcvdto.CreatedBy = item.operator_name;
            rcvdto.DocType = "RCV90";
            rcvdto.BusinessDate = item.modified != null ? CHelper.GetDateTimeMilliseconds(Convert.ToInt64(item.modified)) : DateTime.Now.ToString();

            rcvdto.DocNo = item.order_no;
            rcvdto.Memo = item.remark;
            rcvdto.CurrencyCode = "C001";
            PubPriSVData pubPriSVData = new PubPriSVData();
            pubPriSVData.PrivateDescSeg1 = item.logistics_no;
            pubPriSVData.PrivateDescSeg2 = item.logistics_name;
            pubPriSVData.PrivateDescSeg3 = item.order_no;
            rcvdto.PubPriDt = pubPriSVData;
            List<RCVLineSVDto> lines = new List<RCVLineSVDto>();
            Warehouse WH = Warehouse.FindByCode(org, item.warehouse_no);
            if (WH!=null && WH.DescFlexField.PrivateDescSeg4=="True")
            {
                rcvdto.Status = 2;
            }
            else
            {
                rcvdto.Status = 0;
            }
            foreach (POLine line in misc.POLines)
            {
                foreach (var Detail in item.details_list)
                {
                    if (line.ItemInfo.ItemCode == Detail.goods_no)
                    {
                        RCVLineSVDto rtnLine = new RCVLineSVDto();
                        rtnLine.SrcPODocNo = misc.DocNo;
                        rtnLine.SrcPOLineNo = line.DocLineNo;

                        rtnLine.ItemCode = line.ItemInfo.ItemCode;
                        rtnLine.ConfirmDate = CHelper.GetDateTimeMilliseconds(Convert.ToInt64(item.check_time));
                        rtnLine.PurQtyTU = Detail.num;
                        rtnLine.WHCode = item.warehouse_no;
                        rtnLine.WhMan = WH?.Manager?.Code;
                        rtnLine.LotCode = Detail.batch_no;
                        rtnLine.Memo = Detail.remark;
                        rtnLine.Defect = Detail.defect;
                        PubPriSVData publinePriSVData = new PubPriSVData();
                        publinePriSVData.PrivateDescSeg1 = Detail.remark;
                        rtnLine.PubPriDt = publinePriSVData;
                        lines.Add(rtnLine);
                    }
                    else if (line.DescFlexSegments.PrivateDescSeg1 == Detail.spec_no)
                    {
                        RCVLineSVDto rtnLine = new RCVLineSVDto();
                        rtnLine.SrcPODocNo = item.purchase_no;
                        rtnLine.SrcPOLineNo = line.DocLineNo;

                        rtnLine.ItemCode = line.ItemInfo.ItemCode;
                        rtnLine.ConfirmDate = CHelper.GetDateTimeMilliseconds(Convert.ToInt64(item.check_time));
                        rtnLine.PurQtyTU = Detail.num;
                        rtnLine.WHCode = item.warehouse_no;
                        rtnLine.WhMan = WH?.Manager?.Code;
                        rtnLine.LotCode = Detail.batch_no;
                        rtnLine.Memo = Detail.remark;
                        rtnLine.Defect = Detail.defect;
                        PubPriSVData publinePriSVData = new PubPriSVData();
                        publinePriSVData.PrivateDescSeg1 = Detail.remark;
                        rtnLine.PubPriDt = publinePriSVData;
                        lines.Add(rtnLine);
                    }

                }
            }

            rcvdto.RCVLineDto = lines;
            QMRcvDto.RCVDTO = rcvdto;
            return JsonConvert.SerializeObject(QMRcvDto);
        }
        /// <summary>
        /// 无来源收货
        /// </summary>
        /// <param name="misc"></param>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static string  GetWDTNoSrcReceivement(StockinPurchaseOrderDto item, Organization org)
        {
            Warehouse WH = Warehouse.FindByCode(org, item.warehouse_no);
            QMRcvDto QMRcvDto = new QMRcvDto();
            QMRcvDto.EntCode = CHelper.EntCode;
            QMRcvDto.UserCode = "A001";
            QMRcvDto.OrgCode = org.Code;
            QMRcvDto.OptType = CHelper.ReceivementNoSrcPOOptype;
            RCVDTO rcvdto = new RCVDTO();
            rcvdto.CreatedBy = item.operator_name;
            rcvdto.DocType = "RCV90";
            rcvdto.BusinessDate = CHelper.GetDateTimeMilliseconds(Convert.ToInt64(item.check_time)); ; // CHelper.GetDateTimeMilliseconds(Convert.ToInt64(item.created_time));
            rcvdto.SupplyCode = item.provider_no;
            rcvdto.WhManCode = WH?.Manager?.Code; ;
            rcvdto.DocNo = item.order_no;
            rcvdto.Memo = item.remark;
            PubPriSVData pubPriSVData = new PubPriSVData();
            pubPriSVData.PrivateDescSeg1 = item.logistics_no;
            pubPriSVData.PrivateDescSeg2 = item.logistics_name;
            pubPriSVData.PrivateDescSeg3 = item.order_no;
            rcvdto.PubPriDt = pubPriSVData;
            List<RCVLineSVDto> lines = new List<RCVLineSVDto>();

            if (WH != null && WH.DescFlexField.PrivateDescSeg4 == "True")
            {
                rcvdto.Status = 2;
            }
            else
            {
                rcvdto.Status = 0;
            }

            foreach (var Detail in item.details_list)
            {
                RCVLineSVDto rtnLine = new RCVLineSVDto();
                rtnLine.ItemCode = Detail.goods_no;
                rtnLine.ConfirmDate = CHelper.GetDateTimeMilliseconds(Convert.ToInt64(item.check_time));
                rtnLine.PurQtyTU = Detail.num;
                rtnLine.WHCode = item.warehouse_no;
                rtnLine.WhMan = WH?.Manager?.Code;
                rtnLine.LotCode = Detail.batch_no;
                rtnLine.Memo = Detail.remark;
                rtnLine.Defect = Detail.defect;
                PubPriSVData publinePriSVData = new PubPriSVData();
                publinePriSVData.PrivateDescSeg1 = Detail.remark;
                rtnLine.PubPriDt = publinePriSVData;
                lines.Add(rtnLine);

            }


            rcvdto.RCVLineDto = lines;
            QMRcvDto.RCVDTO = rcvdto;
            return JsonConvert.SerializeObject(QMRcvDto);
        }
        /// <summary>
        /// 更新杂收单
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static string GetWDTUpdateMiscRcvTrans(OrderDto item, Organization org, long ID)
        {
            UpdateDocDto docDto = new UpdateDocDto();
            docDto.DocNo = item.order_no;
            docDto.ID = ID;
            docDto.OrgCode = org.Code;
            List<UpdateDocLineDto> lines = new List<UpdateDocLineDto>();
            foreach (OrderDeatail line in item.detail_list)
            {
                UpdateDocLineDto docLineDto = new UpdateDocLineDto();
                docLineDto.Amount = line.goods_count;
                docLineDto.ItemCode = line.goods_no;
                docLineDto.WHCode = item.warehouse_no;
                lines.Add(docLineDto);
            }
            docDto.DocLines = lines;
            return JsonConvert.SerializeObject(docDto);
        }
        /// <summary>
        /// 更新杂收单新方法
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static string GetWDTStockInUpdateMiscRcvTrans(StockInOrderDto item, Organization org, long ID)
        {
            UpdateDocDto docDto = new UpdateDocDto();
            docDto.DocNo = item.other_in_no;
            docDto.ID = ID;
            docDto.OrgCode = org.Code;
            docDto.Remark = item.remark;
            List<UpdateDocLineDto> lines = new List<UpdateDocLineDto>();
            foreach (StockInOrderDeatail line in item.detail_list)
            {
                UpdateDocLineDto docLineDto = new UpdateDocLineDto();
                docLineDto.Amount = line.in_num;
                docLineDto.ItemCode = line.goods_no;
                docLineDto.WHCode = item.warehouse_no;
                docLineDto.remark = line.remark;
                lines.Add(docLineDto);
            }
            docDto.DocLines = lines;
            return JsonConvert.SerializeObject(docDto);
        }
        /// <summary>
        /// 更新调入单
        /// </summary>
        /// <param name="transfer"></param>
        /// <returns></returns>
        public static string GetWDTUpdateTransferIn(TransferIn transfer)
        {
            if (transfer == null)
            {
                return "";
            }
            UpdateTransferDocDto docDto = new UpdateTransferDocDto();
            docDto.ID = (long)transfer?.ID;
            docDto.DocNo = transfer?.DocNo;
            docDto.OrgCode = transfer.Org?.Code;
            //bool  IsTransInOnLine = false;
            //bool  IsTransOutOnLine = false;
            List<UpdateTransferDocLineDto> linesdto = new List<UpdateTransferDocLineDto>();
            foreach (TransInLine line in transfer.TransInLines)
            {
                foreach (TransInSubLine lnSubLine in line.TransInSubLines)
                {
                    if (line.ID == lnSubLine.TransInLine.ID)
                    {
                        if (line.DescFlexSegments.PrivateDescSeg2!="True" || lnSubLine.DescFlexSegments.PrivateDescSeg1!="True")
                        {
                            return "";                          
                        }
                        //if (line.TransInWh != null)
                        //{
                        //    Warehouse wh = Warehouse.Finder.FindByID(line.TransInWh.ID);
                        //    if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10")
                        //    {
                        //        if (line.DescFlexSegments.PrivateDescSeg10 != "已更新")
                        //        {
                        //            return "";
                        //        }
                        //    }
                        //    else if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "11")
                        //    {
                        //        IsTransInOnLine = true;
                        //    }
                        //}
                        //if (lnSubLine.TransOutWh != null)
                        //{
                        //    Warehouse wh = Warehouse.Finder.FindByID(lnSubLine.TransOutWh.ID);
                        //    if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "10")
                        //    {
                        //        if (lnSubLine.DescFlexSegments.PrivateDescSeg10 != "已更新")
                        //        {
                        //            return "";
                        //        }
                        //    }
                        //    else if (wh != null && wh.DescFlexField.PrivateDescSeg1 == "11")
                        //    {
                        //        IsTransOutOnLine= true;
                        //    }
                        //}
                        //if (IsTransInOnLine && IsTransOutOnLine)
                        //{
                        //    return "";
                        //}
                    }
                }
            }
            return JsonConvert.SerializeObject(docDto);
        }
        /// <summary>
        /// 更新杂发单
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static string GetWDTUpdateMiscShip(MiscShipOrderDto item, Organization org, long ID)
        {
            UpdateDocDto docDto = new UpdateDocDto();
            docDto.DocNo = item.order_no;
            docDto.ID = ID;
            docDto.OrgCode = org.Code;
            List<UpdateDocLineDto> lines = new List<UpdateDocLineDto>();
            foreach (PurchaseReturnOrderDetail line in item.detail_list)
            {
                UpdateDocLineDto docLineDto = new UpdateDocLineDto();
                docLineDto.Amount = line.goods_count;
                docLineDto.ItemCode = line.goods_no;
                docLineDto.WHCode = item.warehouse_no;
                lines.Add(docLineDto);
            }
            docDto.DocLines = lines;
            return JsonConvert.SerializeObject(docDto);
        }
        /// <summary>
        /// 更新杂发单新方法
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static string GetWDTStockOutUpdateMiscShip(StockOutOrderDto item, Organization org, long ID)
        {
            UpdateDocDto docDto = new UpdateDocDto();
            docDto.DocNo = item.other_out_no;
            docDto.ID = ID;
            docDto.OrgCode = org.Code;
            docDto.Remark = item.remark;
            List<UpdateDocLineDto> lines = new List<UpdateDocLineDto>();
            foreach (StockOutOrderDeatail line in item.detail_list)
            {
                UpdateDocLineDto docLineDto = new UpdateDocLineDto();
                docLineDto.Amount = line.out_num;
                docLineDto.ItemCode = line.goods_no;
                docLineDto.WHCode = item.warehouse_no;
                docLineDto.remark = line.remark;
                lines.Add(docLineDto);
            }
            docDto.DocLines = lines;
            return JsonConvert.SerializeObject(docDto);
        }
        /// <summary>
        /// 更新委外退货
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static string GetWDTUpdatePurRtnRcv(PurchaseReturnOrderData item, Organization org, long ID)
        {
            UpdateDocDto docDto = new UpdateDocDto();
            docDto.DocNo = item.order_no;
            docDto.ID = ID;
            docDto.OrgCode = org.Code;
            docDto.Remark = item.remark;
            List<UpdateDocLineDto> lines = new List<UpdateDocLineDto>();
            foreach (PurchaseReturnOrderDeatail line in item.details_list)
            {
                UpdateDocLineDto docLineDto = new UpdateDocLineDto();
                //docLineDto.DocLineNo = Convert.ToInt32(line.remark);
                docLineDto.Amount = line.goods_count;
                docLineDto.ItemCode = line.goods_no;
                docLineDto.specno = line.spec_no;
                docLineDto.WHCode = item.warehouse_no;
                docLineDto.defect = line.defect;
                docLineDto.remark = line.remark;
                lines.Add(docLineDto);
            }
            docDto.DocLines = lines;
            return JsonConvert.SerializeObject(docDto);
        }
        /// <summary>
        /// 更新退回处理(退货入库单查询)
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static string GetStockInWDTUpdateRMA(WdtWmsStockinRefundQuerywithdetailResponse.OrderDomain item, Organization org, long ID)
        {
            UpdateDocDto docDto = new UpdateDocDto();
            docDto.DocNo = item.RefundNo;
            docDto.ID = ID;
            docDto.OrgCode = org.Code;
            docDto.RefundNo = item.RefundNo;
            docDto.RKDocNo = item.OrderNo;
            docDto.Remark = item.Remark;
            List<UpdateDocLineDto> lines = new List<UpdateDocLineDto>();
            foreach (DetailsListDomain line in item.DetailsList)
            {
                UpdateDocLineDto docLineDto = new UpdateDocLineDto();
                docLineDto.Amount = Convert.ToDecimal(line.StockInNum);
                docLineDto.ItemCode = line.GoodsNo;
                docLineDto.WHCode = item.WarehouseNo;
                docLineDto.defect = Convert.ToBoolean(line.Defect);
                lines.Add(docLineDto);
            }
            docDto.DocLines = lines;
            return JsonConvert.SerializeObject(docDto);
        }
        /// <summary>
        /// 更新退回处理(其它业务入库单查询)
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static string GetWDTStockOtherUpdateRMA(StockInOrderDto item, Organization org, long ID)
        {
            UpdateDocDto docDto = new UpdateDocDto();
            docDto.DocNo = item.other_in_no;
            docDto.ID = ID;
            docDto.OrgCode = org.Code;
            docDto.RKDocNo = item.other_in_no;
            List<UpdateDocLineDto> lines = new List<UpdateDocLineDto>();
            foreach (StockInOrderDeatail line in item.detail_list)
            {
                UpdateDocLineDto docLineDto = new UpdateDocLineDto();
                docLineDto.Amount = line.num;
                docLineDto.ItemCode = line.goods_no;
                docLineDto.WHCode = item.warehouse_no;
                docLineDto.defect = line.defect;
                lines.Add(docLineDto);
            }
            docDto.DocLines = lines;
            return JsonConvert.SerializeObject(docDto);
        }
        /// <summary>
        /// 更新调入单
        /// </summary>
        /// <param name="item"></param>
        /// <param name="org"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static string GetWDTUpdateTransferIn(PurchaseReturnOrderData item, Organization org, long ID)
        {
            UpdateDocDto docDto = new UpdateDocDto();
            docDto.DocNo = item.order_no;
            docDto.ID = ID;
            docDto.OrgCode = org.Code;
            List<UpdateDocLineDto> lines = new List<UpdateDocLineDto>();
            foreach (PurchaseReturnOrderDeatail line in item.details_list)
            {
                UpdateDocLineDto docLineDto = new UpdateDocLineDto();
                docLineDto.Amount = line.goods_count;
                docLineDto.ItemCode = line.spec_no;
                docLineDto.WHCode = item.warehouse_no;
                lines.Add(docLineDto);
            }
            docDto.DocLines = lines;
            return JsonConvert.SerializeObject(docDto);
        }
        /// <summary>
        /// 奇门创建无来源出货
        /// </summary>
        /// <param name="order"></param>
        /// <param name="OrgCode"></param>
        /// <returns></returns>
        public static string GetQMShip(WdtWmsStockinRefundQuerywithdetailResponse.OrderDomain order, string OrgCode)
        {
            Customer customer = null;
            ShipNoSrc shipNo = new ShipNoSrc();
            NoSrcShipDTO noSrcShip = new NoSrcShipDTO();
            noSrcShip.BusinessDate = DateTime.Now.ToString(); // order.CreatedTime;
            noSrcShip.Status = 2;
            noSrcShip.DocType = "SM5";
            noSrcShip.DocNo = order.OrderNo;
            Organization org = Organization.FindByCode(OrgCode);
            if (org != null)
            {
                customer = Customer.Finder.Find("Code=@Code and Org=@Org", new UFSoft.UBF.PL.OqlParam("Code", order.ShopNo), new OqlParam("Org", org.ID));
                if (customer == null)
                {
                    customer = Customer.Finder.Find("DescFlexField.PrivateDescSeg11=@S1 and Org=@Org", new OqlParam("S1", order.ShopNo), new OqlParam("Org", org.ID));
                }

                noSrcShip.CustomerCode = customer?.Code;
            }

            // noSrcShip.CreatedBy = misc.CreatedBy;
            PubPriSVData pubPriSVData = new PubPriSVData();
            pubPriSVData.PrivateDescSeg4 = order.LogisticsName;
            pubPriSVData.PrivateDescSeg5 = order.LogisticsNo;
            noSrcShip.PubPriDt = pubPriSVData;

            List<NoSrcShipLineDTO> lines = new List<NoSrcShipLineDTO>();
            int num = 10;
            foreach (var detail in order.DetailsList)
            {
                NoSrcShipLineDTO rtnLine = new NoSrcShipLineDTO();
                rtnLine.LineNo = num;
                rtnLine.ConfirmDate = DateTime.Now.ToString();
                //order.CreatedTime;
                rtnLine.ShipQty = Convert.ToDecimal(detail.StockInNum);
                rtnLine.ItemCode = detail.GoodsNo;
                rtnLine.TotalMoneyTC = Convert.ToDecimal(detail.TotalCost);
                rtnLine.ShipWH = order.WarehouseNo;
                lines.Add(rtnLine);
                num += 10;
            }
            noSrcShip.ShipLineDt = lines;
            shipNo.ShipDTO = noSrcShip;
            shipNo.EntCode = CHelper.EntCode;
            shipNo.UserCode = "A001";
            shipNo.OrgCode = OrgCode;
            shipNo.OptType = CHelper.ShipOptype;
            return JsonConvert.SerializeObject(shipNo);
        }
        /// <summary>
        /// 奇门退回出库有关联类型创建杂发冲预入库
        /// </summary>
        /// <param name="order"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static string  GetQMMiscShip(WdtWmsStockinRefundQuerywithdetailResponse.OrderDomain order, Organization org)
        {
            LossMiscShipDto LossMiscShip = new LossMiscShipDto();
            MiscShipSVData MiscShipDTO = new MiscShipSVData();
            Warehouse WH = Warehouse.FindByCode(org, order.WarehouseNo);
            MiscShipDTO.BussinessDate = !string.IsNullOrEmpty(order.Modified) ? order.Modified : DateTime.Now.ToString();
            MiscShipDTO.Status = 2;
            MiscShipDTO.DocType = "MiscShip008";
            MiscShipDTO.DocNo = order.RefundNo;
            MiscShipDTO.OrgCode = org.Code;
            MiscShipDTO.OrderNo = order.OrderNo;
            MiscShipDTO.WHMan = WH?.Manager?.Code;
            List<MiscShipLineData> lines = new List<MiscShipLineData>();
            int num = 10;
            foreach (var Detail in order.DetailsList)
            {
                MiscShipLineData rtnLine = new MiscShipLineData();
                rtnLine.LineNum = num;
                rtnLine.ItemCode = Detail.GoodsNo;
                rtnLine.IssueQty = Convert.ToDecimal(Detail.Num);
                rtnLine.DeptCode = WH.Department?.Code;
                rtnLine.IssueWH = order.WarehouseNo;
                rtnLine.StoreType = Convert.ToBoolean(Detail.Defect) ? 2 : 4;
                lines.Add(rtnLine);
                num += 10;
            }
            LossMiscShip.OrgCode = org.Code;
            LossMiscShip.EntCode = CHelper.EntCode;
            LossMiscShip.UserCode = "A001";
            LossMiscShip.OptType = CHelper.MiscShipOptype;
            MiscShipDTO.MiscShipLineDt = lines;
            LossMiscShip.MiscShipDTO = MiscShipDTO;
            return JsonConvert.SerializeObject(LossMiscShip);
        }
        /// <summary>
        /// 奇门退回出库有关联类型创建杂发冲预入库-历史入库单查询
        /// </summary>
        /// <param name="order"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public static string  GetQMMiscShipFromHis(StockinRefundOrderData order, Organization org)
        {
            LossMiscShipDto LossMiscShip = new LossMiscShipDto();
            MiscShipSVData MiscShipDTO = new MiscShipSVData();
            Warehouse WH = Warehouse.FindByCode(org, order.warehouse_no);
            MiscShipDTO.BussinessDate = DateTime.Now.ToString();
            MiscShipDTO.Status = 2;
            MiscShipDTO.DocType = "MiscShip008";
            MiscShipDTO.DocNo = order.refund_no;
            MiscShipDTO.OrgCode = org.Code;
            MiscShipDTO.WHMan = WH?.Manager?.Code;
            List<MiscShipLineData> lines = new List<MiscShipLineData>();
            int num = 10;
            foreach (var Detail in order.details_list)
            {
                MiscShipLineData rtnLine = new MiscShipLineData();
                rtnLine.LineNum = num;
                rtnLine.ItemCode = Detail.goods_no;
                rtnLine.IssueQty = Convert.ToDecimal(Detail.num);
                rtnLine.DeptCode = WH.Department?.Code;
                rtnLine.IssueWH = order.warehouse_no;
                rtnLine.StoreType = 4;
                lines.Add(rtnLine);
                num += 10;
            }
            LossMiscShip.OrgCode = org.Code;
            LossMiscShip.EntCode = CHelper.EntCode;
            LossMiscShip.UserCode = "A001";
            LossMiscShip.OptType = CHelper.MiscShipOptype;
            MiscShipDTO.MiscShipLineDt = lines;
            LossMiscShip.MiscShipDTO = MiscShipDTO;
            return JsonConvert.SerializeObject(LossMiscShip);
        }
        /// <summary>
        /// 旺店通创建无来源出货
        /// </summary>
        /// <param name="order"></param>
        /// <param name="OrgCode"></param>
        /// <returns></returns>
        public static string GetWDTShip(StockinRefundOrderData order, string OrgCode)
        {
            ShipNoSrc shipNo = new ShipNoSrc();
            NoSrcShipDTO noSrcShip = new NoSrcShipDTO();
            noSrcShip.BusinessDate = DateTime.Now.ToString(); // order.created_time;
            noSrcShip.Status = 2;
            noSrcShip.DocType = "SM5";
            Customer customer = Customer.Finder.Find("Name=@Name", new UFSoft.UBF.PL.OqlParam("Name", order.shop_no));
            noSrcShip.CustomerCode = customer?.Code;
            // noSrcShip.CreatedBy = misc.CreatedBy;
            PubPriSVData pubPriSVData = new PubPriSVData();
            pubPriSVData.PrivateDescSeg4 = order.logistics_name;
            pubPriSVData.PrivateDescSeg5 = order.logistics_no;
            noSrcShip.PubPriDt = pubPriSVData;

            List<NoSrcShipLineDTO> lines = new List<NoSrcShipLineDTO>();
            int num = 10;
            foreach (var detail in order.details_list)
            {
                NoSrcShipLineDTO rtnLine = new NoSrcShipLineDTO();
                rtnLine.LineNo = num;
                rtnLine.ConfirmDate = order.created_time;
                rtnLine.ShipQty = Convert.ToDecimal(detail.stockin_num);
                rtnLine.ItemCode = detail.goods_no;
                lines.Add(rtnLine);
                num += 10;
            }
            noSrcShip.ShipLineDt = lines;
            shipNo.ShipDTO = noSrcShip;
            shipNo.EntCode = CHelper.EntCode;
            shipNo.UserCode = "A001";
            shipNo.OrgCode = OrgCode;
            shipNo.OptType = CHelper.ShipOptype;
            return JsonConvert.SerializeObject(noSrcShip);
        }
    }



}
