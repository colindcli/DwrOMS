using System.ComponentModel;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Entity.Enums
{
    public enum UserRoleTypeEnum
    {
        /// <summary>
        /// 管理
        /// </summary>
        [Description("管理")]
        Admin = 1,
        /// <summary>
        /// 销售
        /// </summary>
        [Description("销售")]
        Sale = 2,
        /// <summary>
        /// 采购
        /// </summary>
        [Description("采购")]
        Purchase = 3,
        /// <summary>
        /// 出纳（收支）
        /// </summary>
        [Description("出纳（收支）")]
        Cashier = 4,
        /// <summary>
        /// 仓管（入库和出库）
        /// </summary>
        [Description("仓管（入库和出库）")]
        Warehouse = 5,
    }
}
