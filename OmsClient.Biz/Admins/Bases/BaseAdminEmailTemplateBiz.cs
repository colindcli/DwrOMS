using OmsClient.DataAccess.Admins;
using OmsClient.Entity;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Biz.Admins.Bases
{
    public abstract class BaseAdminEmailTemplateBiz
    {
        protected static readonly AdminEmailTemplateDa Da = new AdminEmailTemplateDa();

        /// <summary>
        /// 获取记录
        /// </summary>
        /// <returns></returns>
        public virtual EmailTemplate GetEmailTemplate()
        {
            return Da.GetEmailTemplate();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool UpdateEmailTemplate(EmailTemplate model)
        {
            return Da.Update(model);
        }
    }
}
