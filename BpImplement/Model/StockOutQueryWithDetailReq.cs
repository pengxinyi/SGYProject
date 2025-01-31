using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    /// <summary>
    /// 请求  旺店通出货单查询    
    /// </summary>
    public class StockOutQueryWithDetailReq
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public string start_time { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string end_time { get; set; }

        /// <summary>
        /// 出库单状态
        /// </summary>
        public int status_type { get; set; }

        /// <summary>
        /// 出库单状态详细
        /// </summary>
        [JsonIgnore]
        public string status { get; set; }

        /// <summary>
        /// 仓库编号
        /// </summary>
        [JsonIgnore]
        public string warehouse_no { get; set; }

        /// <summary>
        /// 出库单号
        /// </summary>
        [JsonIgnore]
        public string stockout_no { get; set; }

        /// <summary>
        /// 店铺编号
        /// </summary>
        [JsonIgnore]
        public string shop_nos { get; set; }

        /// <summary>
        /// 销售订单号	
        /// </summary>
        [JsonIgnore]
        public string src_order_no { get; set; }

        /// <summary>
        /// 物流单号
        /// </summary>
        [JsonIgnore]
        public string logistics_no { get; set; }

        /// <summary>
        /// 是否返回sn信息
        /// </summary>
        [JsonIgnore]
        public bool need_sn { get; set; }

        /// <summary>
        /// 是否按照货位排序
        /// </summary>
        [JsonIgnore]
        public int position { get; set; }

        /// <summary>
        /// 是否使用从库查询
        /// </summary>
        [JsonIgnore]
        public bool is_slave { get; set; }
    }
}
