using OmsClient.DataAccess.Users;
using OmsClient.Entity;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Biz.Users.Bases
{
    public abstract class BaseUserSaleConfigBiz
    {
        protected static readonly UserSaleConfigDa Da = new UserSaleConfigDa();

        /// <summary>
        /// 获取记录
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public virtual SaleConfig GetSaleConfig(UserModel userModel)
        {
            return Da.GetSaleConfig(userModel);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool UpdateSaleConfig(UserModel userModel, SaleConfig model)
        {
            var m = Da.Get<SaleConfig>(model.SaleConfigId);
            if (m == null ||  m.UserId != userModel.UserId)
            {
                return false;
            }

            m.PiCompanyName = model.PiCompanyName;
            m.PiCompanyAddress = model.PiCompanyAddress;
            m.PiMyContact = model.PiMyContact;

            return Da.Update(m);
        }
    }
}
