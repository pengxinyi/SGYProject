using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
    public class GoodsReq
    {
        /// <summary>
        /// 货品编号
        /// </summary>
        public string goods_no { get; set; }

        /// <summary>
        /// 货品名称
        /// </summary>
        public string goods_name { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string class_name { get; set; }

        /// <summary>
        /// 品牌名称
        /// </summary>
        public string brand_name { get; set; }

        /// <summary>
        /// 基本单位名称
        /// </summary>
        public string unit_name { get; set; }


        /// <summary>
        /// 辅助单位名称
        /// </summary>
        public string aux_unit_name { get; set; }

        /// <summary>
        /// 货品类别
        /// </summary>
        public int goods_type { get; set; }

        /// <summary>
        /// 货品简称
        /// </summary>
        public string short_name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 货品自定义属性1
        /// </summary>
        public string prop1 { get; set; }

        /// <summary>
        /// 货品自定义属性2
        /// </summary>
        public string prop2 { get; set; }
        /// <summary>
        /// 货品自定义属性3
        /// </summary>
        public string prop3 { get; set; }
        /// <summary>
        /// 货品自定义属性4
        /// </summary>
        public string prop4 { get; set; }
        /// <summary>
        /// 货品自定义属性5
        /// </summary>
        public string prop5 { get; set; }
        /// <summary>
        /// 货品自定义属性6
        /// </summary>
        public string prop6 { get; set; }
        /// <summary>
        /// 是否自动创建品牌和分类
        /// </summary>
        public bool auto_create_bc { get; set; }
    }

    public class SpecInfoReq
    {
        /// <summary>
        /// 基本单位名称
        /// </summary>
        public string unit_name { get; set; }


        /// <summary>
        /// 辅助单位名称
        /// </summary>
        public string aux_unit_name { get; set; }
        /// <summary>
        /// 商家编码
        /// </summary>
        public string spec_no { get; set; }
        /// <summary>
        /// 规格码
        /// </summary>
        public string spec_code { get; set; }

        /// <summary>
        /// 有效期天数
        /// </summary>
        public int validity_days { get; set; }

        /// <summary>
        /// 条码
        /// </summary>
        public string barcode { get; set; }

        /// <summary>
        /// 规格名称
        /// </summary>
        public string spec_name { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public decimal weight { get; set; }

        /// <summary>
        /// 高
        /// </summary>
        public decimal height { get; set; }

        /// <summary>
        /// 长
        /// </summary>
        public decimal length { get; set; }

        /// <summary>
        /// 宽
        /// </summary>
        public decimal width { get; set; }

        

        /// <summary>
        /// 自定义价格1
        /// </summary>
        public decimal custom_price1 { get; set; }

 
        /// <summary>
        /// 零售价
        /// </summary>
        public decimal retail_price { get; set; }
        /// <summary>
        /// 批发价
        /// </summary>
        public decimal wholesale_price { get; set; }
        /// <summary>
        /// 货品自定义属性1
        /// </summary>
        public string prop1 { get; set; }

        /// <summary>
        /// 货品自定义属性2
        /// </summary>
        public string prop2 { get; set; }
        /// <summary>
        /// 货品自定义属性3
        /// </summary>
        public string prop3 { get; set; }
        /// <summary>
        /// 货品自定义属性4
        /// </summary>
        public string prop4 { get; set; }
        /// <summary>
        /// 货品自定义属性5
        /// </summary>
        public string prop5 { get; set; }
        /// <summary>
        /// 货品自定义属性6
        /// </summary>
        public string prop6 { get; set; }
    }
}
