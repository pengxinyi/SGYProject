using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    /// <summary>
    /// 返回  旺店通出货单查询
    /// </summary>
    public class StockOutQueryWithDetailRes : WDTRes
    {
        public int status { get; set; }
        public string message { get; set; }

        public StockOutSalesData data { get; set; }
    }
    public class StockOutSalesData
    {
        public List<MiscSalesOrderDto> order { get; set; }
        public int total_count { get; set; }
    }

    public class MiscSalesOrderDto
    {
        public int stockout_id { get; set; }
        public string order_no { get; set; }
        public string src_order_no { get; set; }
        public string warehouse_no { get; set; }
        public string warehouse_name { get; set; }
        public string consign_time { get; set; }
        public int order_type { get; set; }
        public decimal goods_count { get; set; }
        public string logistics_no { get; set; }
        public string receiver_name { get; set; }
        public string receiver_country { get; set; }
        public string receiver_province { get; set; }
        public string receiver_city { get; set; }
        public string receiver_district { get; set; }
        public string receiver_address { get; set; }
        public string receiver_mobile { get; set; }
        public string receiver_telno { get; set; }
        public string receiver_zip { get; set; }
        public string receiver_area { get; set; }
        public string remark { get; set; }
        public decimal weight { get; set; }
        public int block_reason { get; set; }
        public int logistics_type { get; set; }
        public string logistics_code { get; set; }
        public string logistics_name { get; set; }
        public int shop_id { get; set; }
        public int warehouse_id { get; set; }
        public int logistics_id { get; set; }
        public int bad_reason { get; set; }
        public string receiver_dtb { get; set; }
        public int refund_status { get; set; }
        public int trade_type { get; set; }
        public string salesman_no { get; set; }
        public string fullname { get; set; }
        public string picker_name { get; set; }
        public string examiner_name { get; set; }
        public string consigner_name { get; set; }
        public string printer_name { get; set; }
        public string packager_name { get; set; }
        public int trade_status { get; set; }
        public string trade_no { get; set; }
        public string src_trade_no { get; set; }
        public string nick_name { get; set; }
        public string customer_no { get; set; }
        public string customer_name { get; set; }
        public string trade_time { get; set; }
        public string pay_time { get; set; }
        public string flag_name { get; set; }
        public decimal post_amount { get; set; }
        public int id_card_type { get; set; }
        public string id_card { get; set; }
        public string shop_name { get; set; }
        public string shop_no { get; set; }
        public string shop_remark { get; set; }
        public int status { get; set; }
        public int invoice_type { get; set; }
        public int invoice_id { get; set; }
        public decimal cod_amount { get; set; }
        public int delivery_term { get; set; }
        public int platform_id { get; set; }
        public int trade_id { get; set; }
        public string employee_no { get; set; }
        public decimal discount { get; set; }
        public decimal tax { get; set; }
        public decimal tax_rate { get; set; }
        public string currency { get; set; }
        public string created { get; set; }
        public string stock_check_time { get; set; }
        public string print_remark { get; set; }
        public string buyer_message { get; set; }
        public string cs_remark { get; set; }
        public string invoice_title { get; set; }
        public string invoice_content { get; set; }
        public decimal post_fee { get; set; }
        public decimal package_fee { get; set; }
        public decimal receivable { get; set; }
        public decimal goods_total_cost { get; set; }
        public decimal goods_total_amount { get; set; }
        public string modified { get; set; }
        public string fenxiao_nick { get; set; }
        public string trade_label { get; set; }
        public int trade_from { get; set; }
        public string picklist_no { get; set; }
        public int picklist_seq { get; set; }
        public int logistics_print_status { get; set; }
        public decimal paid { get; set; }
        public int shop_platform_id { get; set; }
        public int sub_platform_id { get; set; }
        public string error_info { get; set; }
        public int custom_type { get; set; }
        public int sendbill_template_id { get; set; }
        public int customer_id { get; set; }
        public int warehouse_type { get; set; }
        public int operator_id { get; set; }
        public string outer_no { get; set; }
        public int consign_status { get; set; }
        public int goods_type_count { get; set; }
        public decimal calc_post_cost { get; set; }
        public string batch_no { get; set; }
        public string created_date { get; set; }
        public string fenxiao_tid { get; set; }
        public string fenxiao_nick_no { get; set; }
        public List<logisticslist> logistics_list { get; set; }
        public List<MiscSalesOrderDeatail> details_list { get; set; }
    }
    public class MiscSalesOrderDeatail
    {
        public int rec_id { get; set; }
        public int stockout_id { get; set; }
        public int src_order_detail_id { get; set; }
        public int spec_id { get; set; }
        public string spec_no { get; set; }
        public decimal goods_count { get; set; }
        public decimal total_amount { get; set; }
        public decimal sell_price { get; set; }
        public string remark { get; set; }
        public string goods_name { get; set; }
        public string goods_no { get; set; }
        public string spec_name { get; set; }
        public string spec_code { get; set; }
        public decimal cost_price { get; set; }
        public decimal weight { get; set; }
        public int goods_id { get; set; }
        public string prop1 { get; set; }
        public string prop2 { get; set; }
        public string prop3 { get; set; }
        public string prop4 { get; set; }
        public string prop5 { get; set; }
        public string prop6 { get; set; }
        public int platform_id { get; set; }
        public int refund_status { get; set; }
        public decimal market_price { get; set; }
        public decimal discount { get; set; }
        public decimal share_price { get; set; }
        public decimal share_amount { get; set; }
        public decimal tax_rate { get; set; }
        public string barcode { get; set; }
        public string unit_name { get; set; }
        public int sale_order_id { get; set; }
        public int gift_type { get; set; }
        public string src_oid { get; set; }
        public string src_tid { get; set; }
        public int from_mask { get; set; }
        public int goods_type { get; set; }
        public string good_prop1 { get; set; }
        public string good_prop2 { get; set; }
        public string good_prop3 { get; set; }
        public string good_prop4 { get; set; }
        public string good_prop5 { get; set; }
        public string good_prop6 { get; set; }
        public string sn_list { get; set; }
        public string suite_no { get; set; }
        public decimal share_post_amount { get; set; }
        public decimal paid { get; set; }
        public bool is_package { get; set; }
        public string brand_no { get; set; }
        public string brand_name { get; set; }
        public int src_order_type { get; set; }
        public int base_unit_id { get; set; }
        public int unit_id { get; set; }
        public decimal unit_ratio { get; set; }
        public decimal num2 { get; set; }
        public decimal num { get; set; }
        public int position_id { get; set; }
        public int batch_id { get; set; }
        public int is_examined { get; set; }
        public string expire_date { get; set; }
        public int scan_type { get; set; }
        public string modified_date { get; set; }
        public string created_date { get; set; }
        public List<positiondetailslist> position_details_list { get; set; }
    }
    public class logisticslist
    {
        public int rec_id { get; set; }
        public int stockout_id { get; set; }
        public decimal calc_weight { get; set; }
        public string logistics_no { get; set; }
        public decimal weight { get; set; }
        public string package_name { get; set; }
        public string logistics_name { get; set; }
        public int logistics_id { get; set; }
        public  decimal postage { get; set; }
        public string remark { get; set; }
        public decimal length { get; set; }
        public decimal width { get; set; }
        public decimal height { get; set; }
        public decimal volume { get; set; }
    }
}
