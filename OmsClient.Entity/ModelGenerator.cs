using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Entity
{
    /// <summary>
    /// dbo.Account
    /// </summary>
    [Table("Account", Schema = "dbo")]
    public partial class Account
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public virtual Guid AccountId { get; set; }
        /// <summary>
        /// 账户类型：3收付款账户；1收款账户；2付款账户；
        /// </summary>
        public virtual int AccountType { get; set; }
        /// <summary>
        /// 账户姓名
        /// </summary>
        public virtual string AccountName { get; set; }
        /// <summary>
        /// 账户银行
        /// </summary>
        public virtual string AccountBank { get; set; }
        /// <summary>
        /// 账户卡号
        /// </summary>
        public virtual string AccountNumber { get; set; }
        /// <summary>
        /// 开户地址
        /// </summary>
        public virtual string AccountAddress { get; set; }
        /// <summary>
        /// 账户备注
        /// </summary>
        public virtual string AccountRemark { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public virtual int Sort { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateDate { get; set; }
    }

    /// <summary>
    /// dbo.Attachment
    /// </summary>
    [Table("Attachment", Schema = "dbo")]
    public partial class Attachment
    {
        /// <summary>
        /// 主键Guid
        /// </summary>
        [Key]
        public virtual Guid AttachmentId { get; set; }
        /// <summary>
        /// 文件名：201709
        /// </summary>
        public virtual string Folder { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public virtual string Title { get; set; }
        /// <summary>
        /// 源文件大小
        /// </summary>
        public virtual long SizeOriginal { get; set; }
        /// <summary>
        /// 小缩略图大小
        /// </summary>
        public virtual long SizeSmall { get; set; }
        /// <summary>
        /// 大缩略图大小
        /// </summary>
        public virtual long SizeBig { get; set; }
        /// <summary>
        /// 文件扩展名
        /// </summary>
        public virtual string FileExt { get; set; }
        /// <summary>
        /// 下载量
        /// </summary>
        public virtual int Downloads { get; set; }
        /// <summary>
        /// 状态：0上传文件（超24小时就删除）；1可用的文件；2标志为不可用文件（保留系统文件，不会删除）
        /// </summary>
        public virtual int Status { get; set; }
        /// <summary>
        /// 0未知（游客）；1用户；2管理员
        /// </summary>
        public virtual int CreateObjectTypeId { get; set; }
        /// <summary>
        /// 创建人: UserId; AdminId
        /// </summary>
        public virtual Guid? CreateObjectId { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateDate { get; set; }
    }

    /// <summary>
    /// dbo.BuyOrder
    /// </summary>
    [Table("BuyOrder", Schema = "dbo")]
    public partial class BuyOrder
    {
        /// <summary>
        /// 采购单
        /// </summary>
        [Key]
        public virtual Guid BuyOrderId { get; set; }
        /// <summary>
        /// 所属用户
        /// </summary>
        public virtual Guid UserId { get; set; }
        /// <summary>
        /// 采购单号
        /// </summary>
        public virtual string BuyOrderNumber { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public virtual string Title { get; set; }
        /// <summary>
        /// 预计到货日期
        /// </summary>
        public virtual DateTime? ArrivalsDate { get; set; }
        /// <summary>
        /// 运费。（订单币种）
        /// </summary>
        public virtual decimal ShipFeight { get; set; }
        /// <summary>
        /// 其他费用。（订单币种）
        /// </summary>
        public virtual decimal ShipFee { get; set; }
        /// <summary>
        /// 折扣费用。（订单币种）
        /// </summary>
        public virtual decimal Discount { get; set; }
        /// <summary>
        /// 币种
        /// </summary>
        public virtual Guid CurrencyId { get; set; }
        /// <summary>
        /// 币种，如：人民币
        /// </summary>
        public virtual string CurrencyName { get; set; }
        /// <summary>
        /// 英文：RMB/USD
        /// </summary>
        public virtual string CurrencyText { get; set; }
        /// <summary>
        /// 币种符号：$
        /// </summary>
        public virtual string CurrencySymbol { get; set; }
        /// <summary>
        /// 汇率，美元6.10
        /// </summary>
        public virtual decimal CurrencyRate { get; set; }
        /// <summary>
        /// 结款方式：1付款后发货；2自提并付款；3收货后付款；4周付；5半月付；6月付；7季付；8半年付；9年付；10其他
        /// </summary>
        public virtual int DefaultSettlement { get; set; }
        /// <summary>
        /// 发货方式。业务员告诉仓库发货方式
        /// </summary>
        public virtual string DefaultShip { get; set; }
        /// <summary>
        /// 供应商账号。业务员告诉出纳收款账号
        /// </summary>
        public virtual string DefaultAccount { get; set; }
        /// <summary>
        /// 订单备注。告诉仓库注意收货事项
        /// </summary>
        public virtual string DefaultRemark { get; set; }
        /// <summary>
        /// 状态：1草稿采购单；2提交审核；3审核驳回；4审核通过；5采购中不用付款；6采购中等待付款；7采购中已付款；8已入库未付款；9已入库已付款；
        /// </summary>
        public virtual int Status { get; set; }
        /// <summary>
        /// 供应商Id
        /// </summary>
        public virtual Guid? BuySupplierId { get; set; }
        /// <summary>
        /// 供应商公司
        /// </summary>
        public virtual string SupplierCompany { get; set; }
        /// <summary>
        /// 供应商联系人
        /// </summary>
        public virtual string SupplierName { get; set; }
        /// <summary>
        /// 供应商固话
        /// </summary>
        public virtual string SupplierTel { get; set; }
        /// <summary>
        /// 供应商手机
        /// </summary>
        public virtual string SupplierMobilePhone { get; set; }
        /// <summary>
        /// 供应商邮箱
        /// </summary>
        public virtual string SupplierEmail { get; set; }
        /// <summary>
        /// 供应商QQ
        /// </summary>
        public virtual string SupplierQQ { get; set; }
        /// <summary>
        /// 供应商微信
        /// </summary>
        public virtual string SupplierWechat { get; set; }
        /// <summary>
        /// 供应商地址
        /// </summary>
        public virtual string SupplierAddress { get; set; }
        /// <summary>
        /// 采购员
        /// </summary>
        public virtual string UserNickName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateDate { get; set; }
        /// <summary>
        /// 添加审核日期
        /// </summary>
        public virtual DateTime? PostCheckDate { get; set; }
        /// <summary>
        /// 审核备注
        /// </summary>
        public virtual string CheckRemark { get; set; }
        /// <summary>
        /// 审核人员
        /// </summary>
        public virtual string CheckUserName { get; set; }
        /// <summary>
        /// 审核时间（通过或不通过时间）
        /// </summary>
        public virtual DateTime? CheckDate { get; set; }
        /// <summary>
        /// 提交采购
        /// </summary>
        public virtual DateTime? PostPurcharseDate { get; set; }
        /// <summary>
        /// 付款时间
        /// </summary>
        public virtual DateTime? PayFinishDate { get; set; }
        /// <summary>
        /// 付款操作
        /// </summary>
        public virtual string PayFinishUserName { get; set; }
        /// <summary>
        /// 验货人员
        /// </summary>
        public virtual string StockInQcUserName { get; set; }
        /// <summary>
        /// 入库备注
        /// </summary>
        public virtual string StockInRemark { get; set; }
        /// <summary>
        /// 操作员
        /// </summary>
        public virtual string StockInUserName { get; set; }
        /// <summary>
        /// 入库时间
        /// </summary>
        public virtual DateTime? StockInDate { get; set; }
    }

    /// <summary>
    /// dbo.BuyOrderProduct
    /// </summary>
    [Table("BuyOrderProduct", Schema = "dbo")]
    public partial class BuyOrderProduct
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public virtual Guid BuyOrderProductId { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public virtual Guid UserId { get; set; }
        /// <summary>
        /// 采购Id
        /// </summary>
        public virtual Guid BuyOrderId { get; set; }
        /// <summary>
        /// 产品Id
        /// </summary>
        public virtual Guid ProductId { get; set; }
        /// <summary>
        /// 价格。（币种为采购单币种）
        /// </summary>
        public virtual decimal Price { get; set; }
        /// <summary>
        /// 采购数量
        /// </summary>
        public virtual int Qty { get; set; }
        /// <summary>
        /// 入库数量
        /// </summary>
        public virtual int InQty { get; set; }
        /// <summary>
        /// 采购备注。采购员备注信息
        /// </summary>
        public virtual string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateDate { get; set; }
        /// <summary>
        /// 是否已入库
        /// </summary>
        public virtual bool IsStockIn { get; set; }
        /// <summary>
        /// 仓库入库添加的产品
        /// </summary>
        public virtual bool IsNewAdd { get; set; }
        /// <summary>
        /// 入库备注。创建验货备注
        /// </summary>
        public virtual string InStockRemark { get; set; }
    }

    /// <summary>
    /// dbo.BuyPay
    /// </summary>
    [Table("BuyPay", Schema = "dbo")]
    public partial class BuyPay
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public virtual Guid BuyPayId { get; set; }
        /// <summary>
        /// 采购单号
        /// </summary>
        public virtual Guid BuyOrderId { get; set; }
        /// <summary>
        /// 账户Id
        /// </summary>
        public virtual Guid AccountId { get; set; }
        /// <summary>
        /// 银行+账号
        /// </summary>
        public virtual string AccountInfo { get; set; }
        /// <summary>
        /// 付款金额
        /// </summary>
        public virtual decimal Amount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual Guid CurrencyId { get; set; }
        /// <summary>
        /// 币种符号：$
        /// </summary>
        public virtual string CurrencySymbol { get; set; }
        /// <summary>
        /// 汇率，美元6.10
        /// </summary>
        public virtual decimal CurrencyRate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual string TransactionNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual string Remark { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public virtual string CreateName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateDate { get; set; }
    }

    /// <summary>
    /// dbo.BuySupplier
    /// </summary>
    [Table("BuySupplier", Schema = "dbo")]
    public partial class BuySupplier
    {
        /// <summary>
        /// 供应商
        /// </summary>
        [Key]
        public virtual Guid BuySupplierId { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public virtual Guid UserId { get; set; }
        /// <summary>
        /// 公司名
        /// </summary>
        public virtual string SupplierCompany { get; set; }
        /// <summary>
        /// 供应商联系人
        /// </summary>
        public virtual string SupplierName { get; set; }
        /// <summary>
        /// 供应商固话
        /// </summary>
        public virtual string SupplierTel { get; set; }
        /// <summary>
        /// 供应商手机
        /// </summary>
        public virtual string SupplierMobilePhone { get; set; }
        /// <summary>
        /// 供应商邮箱
        /// </summary>
        public virtual string SupplierEmail { get; set; }
        /// <summary>
        /// 供应商QQ
        /// </summary>
        public virtual string SupplierQQ { get; set; }
        /// <summary>
        /// 供应商微信
        /// </summary>
        public virtual string SupplierWechat { get; set; }
        /// <summary>
        /// 供应商地址
        /// </summary>
        public virtual string SupplierAddress { get; set; }
        /// <summary>
        /// 供应商备注
        /// </summary>
        public virtual string SupplierRemark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateDate { get; set; }
    }

    /// <summary>
    /// dbo.Category
    /// </summary>
    [Table("Category", Schema = "dbo")]
    public partial class Category
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public virtual int CategoryId { get; set; }
        /// <summary>
        /// 所属分类Id
        /// </summary>
        public virtual int CategoryParentId { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public virtual string CategoryName { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public virtual int Sort { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateDate { get; set; }
    }

    /// <summary>
    /// dbo.Currency
    /// </summary>
    [Table("Currency", Schema = "dbo")]
    public partial class Currency
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public virtual Guid CurrencyId { get; set; }
        /// <summary>
        /// 币种。如：人民币
        /// </summary>
        public virtual string CurrencyName { get; set; }
        /// <summary>
        /// 英文。RMB/USD
        /// </summary>
        public virtual string CurrencyText { get; set; }
        /// <summary>
        /// 币种符号。$
        /// </summary>
        public virtual string CurrencySymbol { get; set; }
        /// <summary>
        /// 汇率。美元6.10
        /// </summary>
        public virtual decimal CurrencyRate { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public virtual int Sort { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateDate { get; set; }
    }

    /// <summary>
    /// dbo.Product
    /// </summary>
    [Table("Product", Schema = "dbo")]
    public partial class Product
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public virtual Guid ProductId { get; set; }
        /// <summary>
        /// 所属分类
        /// </summary>
        public virtual int CategoryId { get; set; }
        /// <summary>
        /// 商品编码
        /// </summary>
        public virtual string ProductSku { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public virtual string ProductName { get; set; }
        /// <summary>
        /// 商品规格
        /// </summary>
        public virtual string ProductSpecification { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public virtual string ProductRemark { get; set; }
        /// <summary>
        /// 重量(克)
        /// </summary>
        public virtual int ProductWeight { get; set; }
        /// <summary>
        /// 在途数量
        /// </summary>
        public virtual int InTransitQty { get; set; }
        /// <summary>
        /// 可售数量
        /// </summary>
        public virtual int SaleQty { get; set; }
        /// <summary>
        /// 挂起数量
        /// </summary>
        public virtual int HoldQty { get; set; }
        /// <summary>
        /// 起订数量
        /// </summary>
        public virtual int OrderQty { get; set; }
        /// <summary>
        /// 售价1
        /// </summary>
        public virtual decimal Price1 { get; set; }
        /// <summary>
        /// 售价10
        /// </summary>
        public virtual decimal Price10 { get; set; }
        /// <summary>
        /// 售价100
        /// </summary>
        public virtual decimal Price100 { get; set; }
        /// <summary>
        /// 图片数
        /// </summary>
        public virtual int ImageQty { get; set; }
        /// <summary>
        /// 存仓位置
        /// </summary>
        public virtual string AreaPosition { get; set; }
        /// <summary>
        /// 状态：1正常；2删除
        /// </summary>
        public virtual int Status { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public virtual string CreateName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateDate { get; set; }
        /// <summary>
        /// 最近入库
        /// </summary>
        public virtual DateTime StockInDate { get; set; }
        /// <summary>
        /// 采购均价
        /// </summary>
        public virtual decimal PurcharseAvgPrice { get; set; }
    }

    /// <summary>
    /// dbo.ProductImage
    /// </summary>
    [Table("ProductImage", Schema = "dbo")]
    public partial class ProductImage
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public virtual Guid ProductImageId { get; set; }
        /// <summary>
        /// 商品Id
        /// </summary>
        public virtual Guid ProductId { get; set; }
        /// <summary>
        /// 原图
        /// </summary>
        public virtual string PathImage { get; set; }
        /// <summary>
        /// logo图
        /// </summary>
        public virtual string PathLogoImage { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public virtual int Sort { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateDate { get; set; }
    }

    /// <summary>
    /// dbo.SaleConfig
    /// </summary>
    [Table("SaleConfig", Schema = "dbo")]
    public partial class SaleConfig
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public virtual Guid SaleConfigId { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public virtual Guid UserId { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public virtual string PiCompanyName { get; set; }
        /// <summary>
        /// 公司地址
        /// </summary>
        public virtual string PiCompanyAddress { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public virtual string PiMyContact { get; set; }
    }

    /// <summary>
    /// dbo.SaleOrder
    /// </summary>
    [Table("SaleOrder", Schema = "dbo")]
    public partial class SaleOrder
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public virtual Guid SaleOrderId { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public virtual Guid UserId { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public virtual string SaleOrderNumber { get; set; }
        /// <summary>
        /// 币种Id
        /// </summary>
        public virtual Guid CurrencyId { get; set; }
        /// <summary>
        /// 币种。如：人民币
        /// </summary>
        public virtual string CurrencyName { get; set; }
        /// <summary>
        /// 英文：RMB/USD
        /// </summary>
        public virtual string CurrencyText { get; set; }
        /// <summary>
        /// 币种符号：$
        /// </summary>
        public virtual string CurrencySymbol { get; set; }
        /// <summary>
        /// 汇率。美元6.10
        /// </summary>
        public virtual decimal CurrencyRate { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public virtual string Title { get; set; }
        /// <summary>
        /// 运费。（订单币种）
        /// </summary>
        public virtual decimal ShipFeight { get; set; }
        /// <summary>
        /// 其他费用。（订单币种）
        /// </summary>
        public virtual decimal ShipFee { get; set; }
        /// <summary>
        /// 折扣费用。（订单币种）
        /// </summary>
        public virtual decimal Discount { get; set; }
        /// <summary>
        /// 发货日期。告诉仓库和客户发货日期
        /// </summary>
        public virtual DateTime? ShipDate { get; set; }
        /// <summary>
        /// 结款方式：1先收款后发货；2提货结款；3送货结；4邮到结；5周结；6半月结；7月结；8季结；9半年结；10年结；11其他
        /// </summary>
        public virtual int DefaultSettlement { get; set; }
        /// <summary>
        /// 发货方式。业务员告诉仓库发货方式
        /// </summary>
        public virtual string DefaultShip { get; set; }
        /// <summary>
        /// 收款账号。业务员告诉出纳收款账号
        /// </summary>
        public virtual string DefaultAccount { get; set; }
        /// <summary>
        /// 订单备注。告诉仓库备货信息
        /// </summary>
        public virtual string DefaultRemark { get; set; }
        /// <summary>
        /// 收货人
        /// </summary>
        public virtual string ToConsignee { get; set; }
        /// <summary>
        /// 收货公司
        /// </summary>
        public virtual string ToCompanyName { get; set; }
        /// <summary>
        /// 收货电话
        /// </summary>
        public virtual string ToTelphone { get; set; }
        /// <summary>
        /// 收货邮编
        /// </summary>
        public virtual string ToZipcode { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        public virtual string ToAddress { get; set; }
        /// <summary>
        /// 发货备注
        /// </summary>
        public virtual string ToRemark { get; set; }
        /// <summary>
        /// 销售员
        /// </summary>
        public virtual string UserNickName { get; set; }
        /// <summary>
        /// 状态：1草稿订单；2提交备货未收款；3提交备货后收款；4提交备货已收款；5已备货未收款；6已备货后收款；7已备货已收款；8已发货后收款；9已发货已收款；
        /// </summary>
        public virtual int Status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateDate { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public virtual DateTime? PostDate { get; set; }
        /// <summary>
        /// 开始备货
        /// </summary>
        public virtual DateTime? StockStart { get; set; }
        /// <summary>
        /// 开始备货人
        /// </summary>
        public virtual string StockStartName { get; set; }
        /// <summary>
        /// 完成备货
        /// </summary>
        public virtual DateTime? StockEnd { get; set; }
        /// <summary>
        /// 完成备货人
        /// </summary>
        public virtual string StockEndName { get; set; }
        /// <summary>
        /// 财务提交日期
        /// </summary>
        public virtual DateTime? FinancePostDate { get; set; }
        /// <summary>
        /// 提交操作人
        /// </summary>
        public virtual string FinancePostName { get; set; }
        /// <summary>
        /// 收款备注。如果是后收款是添加备注
        /// </summary>
        public virtual string FinanceRemark { get; set; }
        /// <summary>
        /// 发货日期。仓库发货日期
        /// </summary>
        public virtual DateTime? StockShipDate { get; set; }
        /// <summary>
        /// 备货备注
        /// </summary>
        public virtual string StockRemark { get; set; }
        /// <summary>
        /// 出库时间
        /// </summary>
        public virtual DateTime? StockOutDate { get; set; }
        /// <summary>
        /// 出库操作人
        /// </summary>
        public virtual string StockOutName { get; set; }
    }

    /// <summary>
    /// dbo.SaleOrderProduct
    /// </summary>
    [Table("SaleOrderProduct", Schema = "dbo")]
    public partial class SaleOrderProduct
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public virtual Guid SaleOrderProductId { get; set; }
        /// <summary>
        /// 用户Id。销售员Id
        /// </summary>
        public virtual Guid UserId { get; set; }
        /// <summary>
        /// 销售单Id
        /// </summary>
        public virtual Guid SaleOrderId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual Guid ProductId { get; set; }
        /// <summary>
        /// 价格。（币种为销售单币种）
        /// </summary>
        public virtual decimal Price { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public virtual int Qty { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateDate { get; set; }
        /// <summary>
        /// 是否已出库
        /// </summary>
        public virtual bool IsStockOut { get; set; }
    }

    /// <summary>
    /// dbo.SaleReceive
    /// </summary>
    [Table("SaleReceive", Schema = "dbo")]
    public partial class SaleReceive
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public virtual Guid SaleReceiveId { get; set; }
        /// <summary>
        /// 销售单Id
        /// </summary>
        public virtual Guid SaleOrderId { get; set; }
        /// <summary>
        /// 账户Id
        /// </summary>
        public virtual Guid AccountId { get; set; }
        /// <summary>
        /// 银行+账号
        /// </summary>
        public virtual string AccountInfo { get; set; }
        /// <summary>
        /// 收款金额
        /// </summary>
        public virtual decimal Amount { get; set; }
        /// <summary>
        /// 交易费用
        /// </summary>
        public virtual decimal TranFee { get; set; }
        /// <summary>
        /// 币种Id
        /// </summary>
        public virtual Guid CurrencyId { get; set; }
        /// <summary>
        /// 币种符号。$
        /// </summary>
        public virtual string CurrencySymbol { get; set; }
        /// <summary>
        /// 汇率。美元6.10
        /// </summary>
        public virtual decimal CurrencyRate { get; set; }
        /// <summary>
        /// 交易号
        /// </summary>
        public virtual string TransactionNumber { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
        /// <summary>
        /// 财务员
        /// </summary>
        public virtual string CreateName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateDate { get; set; }
    }

    /// <summary>
    /// dbo.SaleTrack
    /// </summary>
    [Table("SaleTrack", Schema = "dbo")]
    public partial class SaleTrack
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public virtual Guid SaleTrackId { get; set; }
        /// <summary>
        /// 销售单Id
        /// </summary>
        public virtual Guid SaleOrderId { get; set; }
        /// <summary>
        /// 快递名称
        /// </summary>
        public virtual string TrackName { get; set; }
        /// <summary>
        /// 快递单号
        /// </summary>
        public virtual string TrackNumber { get; set; }
        /// <summary>
        /// 运费。（币种为录入币种）
        /// </summary>
        public virtual decimal Feight { get; set; }
        /// <summary>
        /// 快递备注
        /// </summary>
        public virtual string TrackRemark { get; set; }
        /// <summary>
        /// 币种Id
        /// </summary>
        public virtual Guid CurrencyId { get; set; }
        /// <summary>
        /// 币种。如：人民币
        /// </summary>
        public virtual string CurrencyName { get; set; }
        /// <summary>
        /// 英文。RMB/USD
        /// </summary>
        public virtual string CurrencyText { get; set; }
        /// <summary>
        /// 币种符号。$
        /// </summary>
        public virtual string CurrencySymbol { get; set; }
        /// <summary>
        /// 汇率。美元6.10
        /// </summary>
        public virtual decimal CurrencyRate { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public virtual string CreateName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateDate { get; set; }
    }

    /// <summary>
    /// dbo.Site
    /// </summary>
    [Table("Site", Schema = "dbo")]
    public partial class Site
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public virtual Guid SiteId { get; set; }
        /// <summary>
        /// 站名
        /// </summary>
        public virtual string SiteName { get; set; }
        /// <summary>
        /// 网址
        /// </summary>
        public virtual string SiteUrl { get; set; }
        /// <summary>
        /// 网站公告
        /// </summary>
        public virtual string SiteNotifyText { get; set; }
        /// <summary>
        /// 网站logo：保存logo路径
        /// </summary>
        public virtual string LogoImage { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public virtual string Company { get; set; }
        /// <summary>
        /// 固定电话
        /// </summary>
        public virtual string Telphone { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        public virtual string MobilePhone { get; set; }
        /// <summary>
        /// 微信号码
        /// </summary>
        public virtual string Wechat { get; set; }
        /// <summary>
        /// 微信二维码
        /// </summary>
        public virtual string WechatImage { get; set; }
        /// <summary>
        /// QQ号码
        /// </summary>
        public virtual string QQ { get; set; }
        /// <summary>
        /// QQ二维码
        /// </summary>
        public virtual string QQImage { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public virtual string Email { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        public virtual string Fax { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public virtual string Address { get; set; }
        /// <summary>
        /// 首页标题
        /// </summary>
        public virtual string HomeTitle { get; set; }
        /// <summary>
        /// 首页关键词
        /// </summary>
        public virtual string HomeKeywords { get; set; }
        /// <summary>
        /// 首页描述
        /// </summary>
        public virtual string HomeDescription { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public virtual string HomeContent { get; set; }
        /// <summary>
        /// 附件
        /// </summary>
        public virtual string HomeFiles { get; set; }
        /// <summary>
        /// 首页焦点图
        /// </summary>
        public virtual string HomeFocusImages { get; set; }
    }

    /// <summary>
    /// dbo.StockIn
    /// </summary>
    [Table("StockIn", Schema = "dbo")]
    public partial class StockIn
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public virtual int StockInId { get; set; }
        /// <summary>
        /// 采购单Id
        /// </summary>
        public virtual Guid BuyOrderId { get; set; }
        /// <summary>
        /// 用户Id。采购员
        /// </summary>
        public virtual Guid UserId { get; set; }
        /// <summary>
        /// 采购员
        /// </summary>
        public virtual string UserNickName { get; set; }
        /// <summary>
        /// 采购单产品Id
        /// </summary>
        public virtual Guid BuyOrderProductId { get; set; }
        /// <summary>
        /// 采购单号
        /// </summary>
        public virtual string BuyOrderNumber { get; set; }
        /// <summary>
        /// 产品Id
        /// </summary>
        public virtual Guid ProductId { get; set; }
        /// <summary>
        /// 采购价格。已转为人民币
        /// </summary>
        public virtual decimal Price { get; set; }
        /// <summary>
        /// 入库数量
        /// </summary>
        public virtual int InQty { get; set; }
        /// <summary>
        /// 采购备注
        /// </summary>
        public virtual string Remark { get; set; }
        /// <summary>
        /// 入库备注
        /// </summary>
        public virtual string InStockRemark { get; set; }
        /// <summary>
        /// 入库时间
        /// </summary>
        public virtual DateTime CreateDate { get; set; }
    }

    /// <summary>
    /// dbo.StockOut
    /// </summary>
    [Table("StockOut", Schema = "dbo")]
    public partial class StockOut
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public virtual int StockOutId { get; set; }
        /// <summary>
        /// 销售单Id
        /// </summary>
        public virtual Guid SaleOrderId { get; set; }
        /// <summary>
        /// 用户Id。销售员Id
        /// </summary>
        public virtual Guid UserId { get; set; }
        /// <summary>
        /// 销售员
        /// </summary>
        public virtual string UserNickName { get; set; }
        /// <summary>
        /// 销售单产品Id
        /// </summary>
        public virtual Guid SaleOrderProductId { get; set; }
        /// <summary>
        /// 销售单号
        /// </summary>
        public virtual string SaleOrderNumber { get; set; }
        /// <summary>
        /// 产品Id
        /// </summary>
        public virtual Guid ProductId { get; set; }
        /// <summary>
        /// 价格。已转为人民币
        /// </summary>
        public virtual decimal Price { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public virtual int Qty { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateDate { get; set; }
    }

    /// <summary>
    /// dbo.User
    /// </summary>
    [Table("User", Schema = "dbo")]
    public partial class User
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public virtual Guid UserId { get; set; }
        /// <summary>
        /// 业务角色
        /// </summary>
        public virtual Guid UserRoleId { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public virtual string UserEmail { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public virtual string UserPwd { get; set; }
        /// <summary>
        /// 英文名
        /// </summary>
        public virtual string UserNickName { get; set; }
        /// <summary>
        /// 中文名
        /// </summary>
        public virtual string UserChnName { get; set; }
        /// <summary>
        /// 系统管理
        /// </summary>
        public virtual bool IsAdmin { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }
        /// <summary>
        /// 状态：1有效；2禁用；3删除
        /// </summary>
        public virtual int Status { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public virtual DateTime CreateDate { get; set; }
    }

    /// <summary>
    /// dbo.UserMenu
    /// </summary>
    [Table("UserMenu", Schema = "dbo")]
    public partial class UserMenu
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public virtual int UserMenuId { get; set; }
        /// <summary>
        /// 所属节点
        /// </summary>
        public virtual int UserMenuParentId { get; set; }
        /// <summary>
        /// Key
        /// </summary>
        public virtual string UserMenuText { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public virtual string UserMenuName { get; set; }
        /// <summary>
        /// 链接
        /// </summary>
        public virtual string UserMenuHref { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string UserMenuRemark { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public virtual int UserMenuSort { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateDate { get; set; }
    }

    /// <summary>
    /// dbo.UserRole
    /// </summary>
    [Table("UserRole", Schema = "dbo")]
    public partial class UserRole
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public virtual Guid UserRoleId { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public virtual string UserRoleName { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public virtual int UserRoleSort { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateDate { get; set; }
    }

    /// <summary>
    /// dbo.UserRoleMenu
    /// </summary>
    [Table("UserRoleMenu", Schema = "dbo")]
    public partial class UserRoleMenu
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public virtual Guid UserRoleMenuId { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        public virtual Guid UserRoleId { get; set; }
        /// <summary>
        /// 菜单Id
        /// </summary>
        public virtual int UserMenuId { get; set; }
    }

}

