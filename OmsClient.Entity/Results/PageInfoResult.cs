/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Entity.Results
{
    public class PageInfoResult
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 关键词
        /// </summary>
        public string Keywords { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 选中页：1首页；2产品页；3功能页；4解决方案页；5关于我们页
        /// </summary>
        public int Selected { get; set; }

        /// <summary>
        /// 是否登录
        /// </summary>
        public bool IsLogin { get; set; }
    }
}
