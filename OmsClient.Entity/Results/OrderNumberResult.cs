using System;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Entity.Results
{
    public class OrderNumberResult
    {
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 用户（销售员、采购员）
        /// </summary>
        public Guid UserId { get; set; }
    }
}
