using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UFIDA.U9.LH.LHPubBP.Model
{
   public class  ARBillDto
    {
        public string EntCode { get; set; }
        public string OrgCode { get; set; }
        public string UserCode { get; set; }
        public string OptType { get; set; }
        public ARBillHeadSVDTO  ARBillDTO { get; set; }
    }
    /// <summary>
    /// 应收单
    /// </summary>
    public class ARBillHeadSVDTO
    {
        /// <summary>
        /// 部门
        /// </summary>
        public string Dept { get; set; }

        /// <summary>
        /// 业务员
        /// </summary>
        public string Transactor { get; set; }

        /// <summary>
        /// OALineNo 用于集合返回单号和ID
        /// 2022年3月10日09:00:16 刘英 加
        /// </summary>
        public string OALineNo { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// 应收单号
        /// </summary>
        public string DocNo { get; set; }

        /// <summary>
        /// 单据类型
        /// </summary>
        public string DocumentType { get; set; }

        /// <summary>
        /// 单据状态
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 业务类型
        /// </summary>
        public int BusinessType { get; set; }

        /// <summary>
        /// 立账日期
        /// </summary>
        public string AccrueDate { get; set; }

        /// <summary>
        /// 立账客户
        /// </summary>
        public string AccrueCust { get; set; }

        /// <summary>
        /// 立账客户Site
        /// </summary>
        public string AccrueCustSite { get; set; }

        /// <summary>
        /// 付款客户
        /// </summary>
        public string PayCust { get; set; }

        /// <summary>
        /// 付款客户Site
        /// </summary>
        public string PayCustSite { get; set; }

        /// <summary>
        /// 免税证号
        /// </summary>
        public string TaxFreeLicense { get; set; }

        /// <summary>
        /// 核算币种
        /// </summary>
        public string AC { get; set; }

        /// <summary>
        /// 核币对本币汇率类型
        /// Int
        ///0：买入
        ///1：卖出
        ///2：中间价
        /// </summary>
        public int ACToFCERType { get; set; }

        /// <summary>
        /// 核币对本币汇率
        /// </summary>
        public decimal ACToFCExRate { get; set; }

        /// <summary>
        /// 来源单据标识
        /// </summary>
        public long SrcBillID { get; set; }

        /// <summary>
        /// 来源单据编号
        /// </summary>
        public string SrcBillNum { get; set; }

        /// <summary>
        /// 来源业务类型
        /// </summary>
        public int SrcBusinessType { get; set; }

        /// <summary>
        /// 来源组织
        /// </summary>
        public string SrcOrg { get; set; }

        /// <summary>
        /// 业务组织
        /// </summary>
        public string BizOrg { get; set; }

        /// <summary>
        /// 结算组织
        /// </summary>
        public string SettleOrg { get; set; }

        /// <summary>
        /// 优先等级  0：A 1：B 2：C 3：D   4：E
        /// </summary>
        public int PriorityGrade { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 税组合编码
        /// 2022年3月10日09:30:37 刘英 加
        /// </summary>
        public string TaxSchedule { get; set; }

        /// <summary>
        /// 描述性弹性域
        /// </summary>
        public PubPriSVData PubPriDt { get; set; }

        /// <summary>
        /// 记账期间
        /// </summary>
        public string PostPeriod { get; set; }

        /// <summary>
        /// 收入确认规则  3：同期确认  4：无收入
        /// </summary>
        public int RCR { get; set; }

        /// <summary>
        /// 信用控制对象Site类型
        /// </summary>
        public int CreditControlSiteType { get; set; }

        /// <summary>
        /// 信用控制对象Site
        /// </summary>
        public string CreditControlSite { get; set; }

        /// <summary>
        /// 应收单地址信息
        /// </summary>
        public List<ARBillAddressDTO> ARBillAddress { get; set; }

        /// <summary>
        /// 应收单联系人
        /// </summary>
        public List<ARBillContactManDTO> ARBillContactMan { get; set; }

        /// <summary>
        /// 应收单行
        /// </summary>
        public List<ARBillLineDTO> ARBillLines { get; set; }
    }

    /// <summary>
    /// 应收单地址信息 2022年3月10日14:32:04 刘英 加
    /// </summary>
    public class ARBillAddressDTO
    {
        /// <summary>
        /// 地址1
        /// </summary>
        public string Address1 { get; set; }

        /// <summary>
        /// 地址2
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// 地址3
        /// </summary>
        public string Address3 { get; set; }

        /// <summary>
        /// 地址类型 0:立账客户地址；1;	付款客户地址
        /// </summary>
        public int? AddressType { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 区县
        /// </summary>
        public string County { get; set; }

        /// <summary>
        /// 国家/地区
        /// </summary>
        public string Nation { get; set; }

        /// <summary>
        /// 邮政编码
        /// </summary>
        public string Postalcode { get; set; }

        /// <summary>
        /// 省/自治区
        /// </summary>
        public string Province { get; set; }

    }

    /// <summary>
    /// 应收单联系人  2022年3月10日14:32:13 刘英 加
    /// </summary>
    public class ARBillContactManDTO
    {
        /// <summary>
        /// 联系人类型 0:立账客户联系人；1:	付款客户联系人
        /// </summary>
        public int? ContactManType { get; set; }

        /// <summary>
        /// 电子邮件
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 联系人姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Tel { get; set; }
    }

    /// <summary>
    /// 应收单行明细
    /// </summary>
    public class ARBillLineDTO
    {
        /// <summary>
        /// 状态  0：新增    1：修改    2：删除
        /// </summary>
        public int SysState { get; set; }

        /// <summary>
        /// 行号
        /// </summary>
        public int? LineNum { get; set; }

        /// <summary>
        /// 收款员
        /// </summary>
        public string Receiver { get; set; }

        /// <summary>
        /// 应收立账条件
        /// </summary>
        public string AccrueTerm { get; set; }

        /// <summary>
        /// 到期日
        /// </summary>
        public string Maturity { get; set; }

        /// <summary>
        /// 收款条件
        /// </summary>
        public string RecTerm { get; set; }

        /// <summary>
        /// 税组合编码
        /// </summary>
        public string TaxSchedule { get; set; }

        /// <summary>
        /// 任务
        /// </summary>
        public string Task { get; set; }

        /// <summary>
        /// 项目
        /// </summary>
        public string Project { get; set; }

        /// <summary>
        /// 物料
        /// </summary>
        public string Item { get; set; }

        /// <summary>
        /// 数量（计价单位）
        /// </summary>
        public decimal? PUAmount { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal? UnitPrice { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal? Money { get; set; }

        /// <summary>
        /// 计价主单位
        /// </summary>
        public string PUom { get; set; }

        /// <summary>
        /// 计价副单位
        /// </summary>
        public string PBUom { get; set; }

        /// <summary>
        /// 计价主副单位换算率
        /// </summary>
        public decimal PUomToPBUom { get; set; }

        /// <summary>
        /// 是否含税
        /// </summary>
        public bool? IsIncludeTax { get; set; }

        /// <summary>
        /// 来源单据日期
        /// </summary>
        public string SrcAccrueDate { get; set; }

        /// <summary>
        /// 来源单据标识
        /// </summary>
        public long SrcBillID { get; set; }

        /// <summary>
        /// 来源单据编号
        /// </summary>
        public string SrcBillNum { get; set; }

        /// <summary>
        /// 来源单据行标识
        /// </summary>
        public long SrcBillLineID { get; set; }

        /// <summary>
        /// 来源单据行号
        /// </summary>
        public string SrcBillLineNum { get; set; }

        /// <summary>
        /// 来源数据标识
        /// </summary>
        public long SrcDataID { get; set; }

        /// <summary>
        /// 来源数据行号
        /// </summary>
        public string SrcDataLineNum { get; set; }

        /// <summary>
        /// 收支项目
        /// </summary>
        public string IncExpItem { get; set; }

        /// <summary>
        /// 固定资产编号
        /// </summary>
        public string FACode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 描述性弹性域
        /// </summary>
        public PubPriSVData PubPriDt { get; set; }

        /// <summary>
        /// 是否(押单)质保金
        /// </summary>
        public bool? IsQA { get; set; }

        /// <summary>
        /// 是否占用额度
        /// </summary>
        public bool? IsOccupyCredit { get; set; }

        /// <summary>
        /// 收货客户Site
        /// </summary>
        public string ShipToCustSite { get; set; }

        /// <summary>
        /// 开票依据
        /// </summary>
        public string InvoiceBase { get; set; }

        /// <summary>
        /// 发票号
        /// </summary>
        public string InvoicInt { get; set; }

        /// <summary>
        /// 税控发票号
        /// </summary>
        public string TaxCtrlInvoicInt { get; set; }

        /// <summary>
        /// 开票日期
        /// </summary>
        public string InvoiceDate { get; set; }

        /// <summary>
        /// 收票日期
        /// </summary>
        public string ReceiveInvoiceDate { get; set; }

        /// <summary>
        /// 预算专项
        /// </summary>
        public string BudgetSpecialProject { get; set; }

        /// <summary>
        /// 贸易方式
        /// </summary>
        public int TDMode { get; set; }

        /// <summary>
        /// 税明细
        /// </summary>
        public List<ARBillTaxDetail> ARBillTaxDetail { get; set; }

        /// <summary>
        /// 费明细
        /// </summary>
        public List<ARBillFeeDetail> ARBillFeeDetail { get; set; }
    }

    /// <summary>
    /// 税明细
    /// </summary>
    public class ARBillTaxDetail
    {
        /// <summary>
        /// 行号
        /// </summary>
        public int? LineNum { get; set; }

        /// <summary>
        /// 税别
        /// </summary>
        public string Tax { get; set; }

        /// <summary>
        /// 税额
        /// </summary>
        public decimal? TaxMoney { get; set; }

        /// <summary>
        /// 税率
        /// </summary>
        public decimal? TaxRate { get; set; }

        /// <summary>
        /// 确认应收税额
        /// </summary>
        public decimal? CfmARTax { get; set; }

        /// <summary>
        /// 未确认应收税额
        /// </summary>
        public decimal? UnCfmARTax { get; set; }

        /// <summary>
        /// 未确认收入税额
        /// </summary>
        public decimal? UnRCTax { get; set; }
    }

    /// <summary>
    /// 费明细
    /// </summary>
    public class ARBillFeeDetail
    {
        /// <summary>
        /// 可开票费用
        /// </summary>
        public decimal? CanInvoiceFee { get; set; }

        /// <summary>
        /// 可开票费用税额
        /// </summary>
        public decimal? CanInvoiceFeeTax { get; set; }

        /// <summary>
        /// 总费用
        /// </summary>
        public decimal? Fee { get; set; }

        /// <summary>
        /// 费用项目
        /// </summary>
        public string FeeProject { get; set; }

        /// <summary>
        /// 费用率
        /// </summary>
        public decimal? FeeRate { get; set; }

        /// <summary>
        /// 费用税额
        /// </summary>
        public decimal? FeeTax { get; set; }

        /// <summary>
        /// 费用性质    0:代收代付；1:我方负担；2:客户负担    
        /// </summary>
        public int? FeeType { get; set; }

        /// <summary>
        /// 已开票费用
        /// </summary>
        public decimal? InvoicedFee { get; set; }

        /// <summary>
        /// 已开票费用税额
        /// </summary>
        public decimal? InvoicedFeeTax { get; set; }

        /// <summary>
        /// 计税
        /// </summary>
        public bool? IsCalcTax { get; set; }

        /// <summary>
        /// 来源费用明细标识
        /// </summary>
        public int SrcFeeDetailTag { get; set; }

        /// <summary>
        /// 税组合
        /// </summary>
        public string TaxSchedule { get; set; }

        /// <summary>
        /// 行号
        /// </summary>
        public int? LineNum { get; set; }
    }
}
