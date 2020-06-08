using OmsClient.Entity;
using System;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Common.Utilitys
{
    public class TokenUserHandle
    {
        private const string TokenName = "UserAccessToken";

        public static void SetToken(UserModel model)
        {
            model.ExpiryDate = DateTime.Today.AddDays(1).AddHours(5);
            var token = TokenHelper.GetToken(model);
            CookieHelper.SetCookie(TokenName, token, model.ExpiryDate);
        }

        public static UserModel GetToken()
        {
            var token = CookieHelper.GetCookie(TokenName);
            if (string.IsNullOrWhiteSpace(token))
                return null;

            var m = TokenHelper.GetModel<UserModel>(token);
            if (m == null)
                return null;

            if (m.ExpiryDate < DateTime.Now)
                return null;

            return m;
        }

        public static void ClearTaken()
        {
            CookieHelper.ClearCookie(TokenName);
        }
    }
}
