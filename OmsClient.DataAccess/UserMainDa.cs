using Dapper;
using DwrUtility;
using OmsClient.Entity;
using OmsClient.Entity.Enums;
using OmsClient.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.DataAccess
{
    public class UserMainDa : RepositoryBase
    {
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
        /// <param name="model"></param>
        /// <returns></returns>
        public User CheckUser(User model)
        {
            return Db(db => db.GetList<User>(new { model.UserEmail, model.UserPwd, IsEnabled = true }).FirstOrDefault());
        }

        public User GetUser(Guid userId)
        {
            return Db(db => db.Get<User>(userId));
        }

        public List<User> GetUserList()
        {
            return GetList<User>(new {sEnabled = true});
        }

        public Guid? AddUser(User model)
        {
            return Db(db =>
            {
                //如果Email是有效或禁用的，不能添加，除非是删除的才能添加
                var rows = db.GetList<User>().Count(p => (p.Status == (int)UserStatusEnum.Ok || p.Status == (int)UserStatusEnum.Disabled) && p.UserEmail.IsEquals(model.UserEmail));
                return rows > 0 ? null : db.Insert<Guid?, User>(model);
            });
        }

        public void UpdatePwd(User model)
        {
            Db(db => db.Update<User>(model.UserId, m => { m.UserPwd = model.UserPwd; }));
        }

        public void UpdateUser(User model)
        {
            Db(db => db.Update<User>(model.UserId, m =>
            {
                m.UserChnName = model.UserChnName;
                m.UserRoleId = model.UserRoleId;
            }));
        }

        public bool DeleteUser(User model)
        {
            return Db(db =>
            {
                var m = db.Get<User>(model.UserId);
                if (m == null || !m.IsAdmin)
                {
                    return false;
                }
                db.Update<User>(model.UserId, g => { g.Status = (int)UserStatusEnum.Delete; });
                return true;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpsertUserRoleMenu(UserRoleSettingModel model)
        {
            Db((db, tran) =>
            {
                db.DeleteList<UserRoleMenu>(new { model.UserRoleId }, tran);
                model.UserMenuIds?.ForEach(id =>
                {
                    db.Insert<Guid, UserRoleMenu>(new UserRoleMenu()
                    {
                        UserRoleMenuId = SeqGuid.NewGuid(),
                        UserRoleId = model.UserRoleId,
                        UserMenuId = id
                    }, tran);
                });
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

        public void AddUserMenu(UserMenu model)
        {
            Db((db, tran) =>
            {
                var userMenuId = db.Insert(model, tran);
                if (!userMenuId.HasValue)
                {
                    return;
                }
                var user = db.GetList<User>(new { UserEmail = "User" }, tran).FirstOrDefault();
                if (user != null)
                {
                    db.Insert<Guid, UserRoleMenu>(new UserRoleMenu()
                    {
                        UserRoleMenuId = SeqGuid.NewGuid(),
                        UserMenuId = userMenuId.Value,
                        UserRoleId = user.UserRoleId
                    }, tran);
                }
            });
        }

        public void UpdateUserMenu(UserMenu model)
        {
            Db(db => db.Update<UserMenu>(model.UserMenuId, m =>
            {
                m.UserMenuParentId = model.UserMenuParentId;
                m.UserMenuText = model.UserMenuText;
                m.UserMenuName = model.UserMenuName;
                m.UserMenuHref = model.UserMenuHref;
                m.UserMenuRemark = model.UserMenuRemark;
                m.UserMenuSort = model.UserMenuSort;
            }));
        }

        public bool DeleteUserMenu(UserMenu model)
        {
            return Db((db, tran) =>
            {
                var param = new { UserMenuParentId = model.UserMenuId };
                var rows = db.GetList<UserMenu>(param, tran).Count();
                if (rows > 0)
                {
                    return false;
                }

                db.Delete<UserMenu>(model.UserMenuId, tran);
                return true;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userRoleId"></param>
        /// <returns></returns>
        public List<UserMenu> GetUserRoleSetting(Guid userRoleId)
        {
            return Db(db =>
            {
                var userMenus = db.GetList<UserMenu>().ToList();
                var userRoleMenus = db.GetList<UserRoleMenu>(new { UserRoleId = userRoleId }).ToList();
                userMenus.ForEach(item =>
                {
                    item.Checked = userRoleMenus.Exists(p => p.UserMenuId == item.UserMenuId);
                });

                return userMenus;
            });
        }
        
#if DEBUG
        /// <summary>
        /// 测试：清空角色和角色菜单
        /// </summary>
        public void DeleteUserRoleMenu(int companyId)
        {
            var sqlStr = @"
DELETE FROM dbo.UserRole WHERE CompanyId=@CompanyId;
DELETE FROM dbo.UserRoleMenu WHERE CompanyId=@CompanyId;
UPDATE dbo.Company SET UserSolutionId=NULL WHERE CompanyId=@CompanyId;
";
            var param = new
            {
                CompanyId = companyId
            };

            Db((db, tran) => db.Execute(sqlStr, param, tran));
        }
#endif

    }
}
