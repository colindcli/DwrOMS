using DwrUtility;
using DwrUtility.Strings;
using OmsClient.DataAccess.Users;
using OmsClient.Entity;
using OmsClient.Entity.Models;
using OmsClient.Entity.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OmsClient.Entity.Enums;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Biz.Users.Bases
{
    public abstract class BaseUserUserRoleBiz
    {
        protected static readonly UserUserRoleDa Da = new UserUserRoleDa();

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool AddUserRole(UserModel userModel, UserRole model)
        {
            model.UserRoleId = SeqGuid.NewGuid();
            model.CreateDate = DateTime.Now;
            Da.Add<Guid, UserRole>(model);
            return true;
        }

        /// <summary>
        /// 获取一条记录
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="userRoleId"></param>
        /// <returns></returns>
        public virtual UserRole GetUserRoleById(UserModel userModel, Guid userRoleId)
        {
            var m = Da.Get<UserRole>(userRoleId);
            return m;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool UpdateUserRole(UserModel userModel, UserRole model)
        {
            var m = Da.Get<UserRole>(model.UserRoleId);
            if (m == null)
            {
                return false;
            }

            m.UserRoleName = model.UserRoleName;
            m.UserRoleSort = model.UserRoleSort;

            return Da.Update(m);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public virtual bool DeleteUserRole(UserModel userModel, UserRole model, out string msg)
        {
            var m = Da.Get<UserRole>(model.UserRoleId);
            if (m == null)
            {
                msg = "角色不存在，删除失败";
                return false;
            }

            var list = Da.GetList<User>(new
            {
                m.UserRoleId
            });

            if (list.Count > 0)
            {
                msg = $"有{list.Count}个用户关联了此角色，请先解除关联后才能删除";
                return false;
            }

            msg = null;
            return Da.DeleteUserRole(userModel, model);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual PagenationResult<UserRole> GetUserRoleList(UserModel userModel, int pageIndex, int pageSize, string keyword)
        {
            var users = Da.GetListAsync<User>();
            var list = GetUserRoleList(userModel);
            if (!keyword.IsWhiteSpace())
            {
                var keys = keyword.ToKeys(StringComparer.OrdinalIgnoreCase).ToList();
                list = list.Where(p => $"{p.UserRoleName}".HasSearchKeys(keys, StringComparison.OrdinalIgnoreCase, false)).ToList();
            }
            var result = new PagenationResult<UserRole>()
            {
                count = list.Count,
                data = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList()
            };

            if (result.data.Count > 0)
            {
                Task.WaitAll(users);
                var userLists = users.Result.OrderBy(p => p.Status).ThenByDescending(p => p.IsAdmin).ToList();
                foreach (var item in result.data)
                {
                    var us = userLists.Where(p => p.UserRoleId == item.UserRoleId).Select(p =>
                    {
                        string str;
                        if (p.Status == (int) UserStatusEnum.Disabled)
                        {
                            str = $"<span class=\"disText\">{p.UserNickName}</span>";
                        }
                        else if (p.Status == (int) UserStatusEnum.Delete)
                        {
                            str = $"<span class=\"delText\">{p.UserNickName}</span>";
                        }
                        else
                        {
                            str = p.UserNickName;
                        }
                        return str;
                    }).ToList();
                    item.UserNames = string.Join("、", us);
                }
            }

            return result;
        }

        /// <summary>
        /// 获取所有记录
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public virtual List<UserRole> GetUserRoleList(UserModel userModel)
        {
            return Da.GetList<UserRole>().OrderBy(p => p.UserRoleSort).ToList();
        }

        /// <summary>
        /// 获取一条记录
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="userRoleId"></param>
        /// <returns></returns>
        public virtual List<UserRoleMenuModel> GetUserRoleSetById(UserModel userModel, Guid userRoleId)
        {
            var m = Da.Get<UserRole>(userRoleId);
            if (m == null)
            {
                return null;
            }

            var checks = Da.GetList<UserRoleMenu>(new
            {
                UserRoleId = userRoleId,
            });
            var list = Da.GetList<UserMenu>().OrderBy(p => p.UserMenuSort).ToList();
            var rows = list.Where(p => p.UserMenuParentId == 0)
                .ToList()
                .Join(list, p => p.UserMenuId, p => p.UserMenuParentId, (p, q) => q)
                .ToList();
            var res = rows.Select(p => new UserRoleMenuModel()
            {
                Name = p.UserMenuText,
                Lists = list.Where(q => q.UserMenuParentId == p.UserMenuId).Select(k => new UserRoleMenuList()
                {
                    UserMenuId = k.UserMenuId,
                    UserMenuText = k.UserMenuText,
                    IsChecked = checks.Exists(g => g.UserMenuId == k.UserMenuId)
                }).ToList()
            }).ToList();

            return res;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool UpdateUserRoleSet(UserModel userModel, UserRoleMenuRequest model, out string msg)
        {
            var m = Da.Get<UserRole>(model.UserRoleId);
            if (m == null)
            {
                msg = "角色不存在";
                return false;
            }

            var ids = Da.GetList<UserMenu>().Select(p => p.UserMenuId).ToList();
            model.UserMenuIds = model.UserMenuIds.Join(ids, p => p, p => p, (p, q) => p).ToList();

            msg = null;
            return Da.UpdateUserRoleSet(userModel, model);
        }
    }
}
