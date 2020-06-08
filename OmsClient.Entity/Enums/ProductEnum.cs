/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Entity.Enums
{
    /// <summary>
    /// 产品状态
    /// </summary>
    public enum ProductStatusEnum
    {
        /// <summary>
        /// 正常
        /// </summary>
        Ok = 1,
        /// <summary>
        /// 删除
        /// </summary>
        Delete = 2
    }
    /// <summary>
    /// 用户状态
    /// </summary>
    public enum UserStatusEnum
    {
        /// <summary>
        /// 正常
        /// </summary>
        Ok = 1,
        /// <summary>
        /// 禁用
        /// </summary>
        Disabled = 2,
        /// <summary>
        /// 删除
        /// </summary>
        Delete = 3,
    }
}
