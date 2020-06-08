using OmsClient.DataAccess;
using System;
using System.Collections.Generic;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Biz
{
    public class AdminUserBiz
    {
        private static readonly AdminUserDa Da = new AdminUserDa();

        public List<int> GetUserMenuList(Guid userId)
        {
            return Da.GetUserMenuList(userId);
        }
    }
}
