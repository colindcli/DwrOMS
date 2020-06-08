using DwrUtility;
using OmsClient.Common;
using OmsClient.DataAccess;
using OmsClient.Entity;
using OmsClient.Entity.Enums;
using OmsClient.Entity.Models;
using OmsClient.Entity.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using OmsClient.Common.Utilitys;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Biz
{
    public class UserMainBiz
    {
        private static readonly UserMainDa Da = new UserMainDa();

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool CheckLogin(User user, out string msg)
        {
            var m = Da.GetList<User>(new
            {
                user.UserEmail,
                Status = (int)UserStatusEnum.Ok,
                UserPwd = user.UserPwd.ToMd5()
            }).FirstOrDefault();

            if (m == null)
            {
                msg = "账号或密码错误";
                return false;
            }

            TokenUserHandle.SetToken(new UserModel()
            {
                UserId = m.UserId,
                ExpiryDate = DateTime.Today.AddDays(1).AddHours(5),
                UserNickName = m.UserNickName,
            });

            msg = null;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public UserMenu GetBuyCheckUserMenu(List<UserMenu> list)
        {
            return list.Find(p => p.UserMenuId == 20);
        }

        /// <summary>
        /// 菜单
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public List<MainModel> GetMainMenu(UserModel userModel)
        {
            var menuIds = GetUserMenuList(userModel.UserId);
            var menus = Da.GetUserMenuList();

            //采购单是否需要审核
            if (!Config.IsBuyCheck)
            {
                var m = GetBuyCheckUserMenu(menus);
                menus.RemoveAll(p => p.Equals(m));
            }

            //menu
            var items = (from menu in menus
                         join menuId in menuIds on menu.UserMenuId equals menuId
                         select menu).ToList();

            //父id
            var parents = (from menu in menus
                           join parentId in items.Select(p => p.UserMenuParentId).Distinct() on menu.UserMenuId equals parentId
                           select menu).ToList();

            //祖父id
            var grandfathers = (from menu in menus
                                join parentId in parents.Select(p => p.UserMenuParentId).Distinct() on menu.UserMenuId equals parentId
                                select menu).ToList();

            var rows = grandfathers.Select(gf => new MainModel()
            {
                id = gf.UserMenuName,
                text = gf.UserMenuText,
                menu = parents.Where(k => k.UserMenuParentId == gf.UserMenuId).Select(p => new MainModel.mn()
                {
                    text = p.UserMenuText,
                    items = items.Where(s => s.UserMenuParentId == p.UserMenuId).Select(it => new MainModel.item()
                    {
                        id = it.UserMenuName,
                        text = it.UserMenuText,
                        href = $"{it.UserMenuHref}?v={Config.StaticVersion}"
                    }).ToList()
                }).ToList()
            }).ToList();

            //设置首页
            var row = rows.FirstOrDefault();
            if (row != null)
            {
                row.homePage = items.FirstOrDefault()?.UserMenuName;
            }

            return rows;
        }

        public UserInfoModel GetUserInfo(UserModel userModel)
        {
            var m = Da.Get<User>(userModel.UserId);
            var model = new UserInfoModel()
            {
                UserEmail = m.UserEmail,
                UserNickName = m.UserNickName,
                UserChnName = m.UserChnName,
                UserRoleName = Da.Get<UserRole>(m.UserRoleId)?.UserRoleName
            };
            return model;
        }

        public List<int> GetUserMenuList(Guid userId)
        {
            return Da.GetUserMenuList(userId);
        }

        public User CheckUser(User model)
        {
            return Da.CheckUser(model);
        }

        public PagenationResult<UserRole> GetUserRoleList(string roleName, int pageIndex, int pageSize)
        {
            var lists = Da.GetList<UserRole>().Where(p => string.IsNullOrWhiteSpace(roleName) || p.UserRoleName.IsContains(roleName)).ToList();
            var result = new PagenationResult<UserRole>()
            {
                count = lists.Count,
                data = lists.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList()
            };
            return result;
        }

        public PagenationResult<User> GetUserList(string userEmail, Guid? userRoleId, int pageIndex, int pageSize)
        {
            var lists = Da.GetUserList().Where(p => (string.IsNullOrWhiteSpace(userEmail) || p.UserEmail.IsContains(userEmail)) && (!userRoleId.HasValue || userRoleId.Value == Guid.Empty || p.UserRoleId == userRoleId)).ToList();
            var result = new PagenationResult<User>()
            {
                count = lists.Count,
                data = lists.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList()
            };
            return result;
        }

        public Guid? AddUser(User model)
        {
            model.UserId = SeqGuid.NewGuid();
            model.UserPwd = model.UserPwd.ToMd5();
            model.Status = (int)UserStatusEnum.Ok;
            model.IsAdmin = false;
            model.CreateDate = DateTime.Now;

            return Da.AddUser(model);
        }

        public void UpdatePwd(User model)
        {
            Da.UpdatePwd(model);
        }

        public void UpdateUser(User model)
        {
            Da.UpdateUser(model);
        }

        public bool DeleteUser(User model)
        {
            return Da.DeleteUser(model);
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpsertUserRoleMenu(UserRoleSettingModel model)
        {
            Da.UpsertUserRoleMenu(model);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<UserMenu> GetUserMenuList()
        {
            return Da.GetUserMenuList();
        }

        public void AddUserMenu(UserMenu model)
        {
            model.UserMenuParentId = model.UserMenuParentId;
            model.CreateDate = DateTime.Now;
            Da.AddUserMenu(model);
        }

        public void UpdateUserMenu(UserMenu model)
        {
            Da.UpdateUserMenu(model);
        }

        public bool DeleteUserMenu(UserMenu model)
        {
            return Da.DeleteUserMenu(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userRoleId"></param>
        /// <returns></returns>
        public List<UserMenu> GetUserRoleSetting(Guid userRoleId)
        {
            return Da.GetUserRoleSetting(userRoleId);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="pwd"></param>
        /// <param name="msg"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UpdatePwd(UserModel userModel, Guid userId, string pwd, out string msg)
        {
            if (pwd.IsWhiteSpace())
            {
                msg = "密码不能为空";
                return false;
            }

            var m = Da.Get<User>(userId);
            if (m == null)
            {
                msg = "用户不存在";
                return false;
            }

            if (m.Status != (int)UserStatusEnum.Ok)
            {
                msg = "账号已禁用或已删除";
                return false;
            }

            m.UserPwd = pwd.ToMd5();

            msg = null;
            Da.Update(m);
            return true;
        }
    }
}
