using Dapper;
using OmsClient.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.DataAccess
{
    public class AdminUserDa : RepositoryBase
    {
        public User GetUser(Guid userId)
        {
            return Get<User>(userId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public User CheckUser(User model)
        {
            return Db(db => db.GetList<User>(new
            {
                model.UserEmail,
                IsEnabled = true
            }).FirstOrDefault());
        }

        public bool CheckEmailIsExists(string email)
        {
            return Db(db =>
            {
                var m = db.GetList<User>(new
                {
                    UserEmail = email,
                    IsEnabled = true
                }).FirstOrDefault();
                return m != null;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddUser(User model)
        {
            return Db(db =>
            {
                var m = db.GetList<User>(new
                {
                    model.UserEmail,
                    IsEnabled = true
                }).FirstOrDefault();
                if (m != null)
                {
                    return false;
                }

                db.Insert<Guid, User>(model);
                return true;

            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public User GetUserByEmail(User model)
        {
            return Db(db =>
            {
                var m = db.GetList<User>(new
                {
                    model.UserEmail,
                    IsEnabled = true
                }).FirstOrDefault();

                if (m == null)
                {
                    return null;
                }

                m.Code = new Random().Next(100000, 999999).ToString();
                db.Update(m);
                return m;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public User UpdatePassword(User model)
        {
            return Db(db =>
            {
                var m = db.GetList<User>(new
                {
                    model.UserId,
                    model.Code,
                    IsEnabled = true
                }).FirstOrDefault();

                if (m == null)
                {
                    return null;
                }

                m.UserPwd = model.UserPwd;
                m.Code = null;
                db.Update(m);

                return m;
            });
        }

        public List<int> GetUserMenuList(Guid userId)
        {
            return Db(db =>
            {
                var user = db.Get<User>(userId);
                if (user == null)
                {
                    return new List<int>();
                }

                return db.GetList<UserRoleMenu>(new { user.UserRoleId }).Select(p => p.UserMenuId).ToList();
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<UserMenu> GetUserMenuList()
        {
            return Db(db => db.GetList<UserMenu>().OrderBy(p => p.UserMenuSort).ThenBy(p => p.CreateDate).ToList());
        }
    }
}
