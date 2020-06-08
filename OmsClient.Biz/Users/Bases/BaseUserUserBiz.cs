using DwrUtility;
using DwrUtility.Strings;
using OmsClient.DataAccess.Users;
using OmsClient.Entity;
using OmsClient.Entity.Enums;
using OmsClient.Entity.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Biz.Users.Bases
{
    public abstract class BaseUserUserBiz
    {
        protected static readonly UserUserDa Da = new UserUserDa();

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public virtual bool AddUser(UserModel userModel, User model, out string msg)
        {
            if (model.UserEmail.IsWhiteSpace() || !model.UserEmail.IsEmail())
            {
                msg = "邮箱不正确";
                return false;
            }

            if (model.UserPwd.IsWhiteSpace())
            {
                msg = "请输入密码";
                return false;
            }
            //密码加密
            model.UserPwd = model.UserPwd.ToMd5();

            if (model.UserNickName.IsWhiteSpace())
            {
                msg = "请输入英文名";
                return false;
            }

            if (model.UserChnName.IsWhiteSpace())
            {
                msg = "请输入中文名";
                return false;
            }
            
            //验证角色
            var roles = Da.GetList<UserRole>();
            if (!roles.Exists(p => p.UserRoleId == model.UserRoleId))
            {
                msg = "角色不正确";
                return false;
            }

            model.UserId = SeqGuid.NewGuid();
            model.Status = (int)UserStatusEnum.Ok;
            model.CreateDate = DateTime.Now;
            Da.Add<Guid, User>(model);

            msg = null;
            return true;
        }

        /// <summary>
        /// 获取一条记录
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual User GetUserById(UserModel userModel, Guid userId)
        {
            return Da.Get<User>(userId);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public virtual bool UpdateUser(UserModel userModel, User model, out string msg)
        {
            if (model.UserEmail.IsWhiteSpace() || !model.UserEmail.IsEmail())
            {
                msg = "邮箱不正确";
                return false;
            }

            if (model.UserNickName.IsWhiteSpace())
            {
                msg = "请输入英文名";
                return false;
            }

            if (model.UserChnName.IsWhiteSpace())
            {
                msg = "请输入中文名";
                return false;
            }

            //验证角色
            var roles = Da.GetList<UserRole>();
            if (!roles.Exists(p => p.UserRoleId == model.UserRoleId))
            {
                msg = "角色不正确";
                return false;
            }

            var m = Da.Get<User>(model.UserId);
            if (m == null)
            {
                msg = "用户不存在";
                return false;
            }

            var admin = CanCancelAdmin(userModel, m, model, out msg);
            if (!admin)
            {
                return false;
            }

            var status = CanCancelStatus(userModel, m, model, out msg);
            if (!status)
            {
                return false;
            }

            //如果状态不存在，不修改
            var userStatus = EnumHelper.GetValueNameDict<UserStatusEnum>().Select(p => p.Key).ToList();
            if (!userStatus.Exists(p => p == model.Status))
            {
                model.Status = m.Status;
            }

            m.UserRoleId = model.UserRoleId;
            m.UserEmail = model.UserEmail;
            //m.UserPwd = model.UserPwd;
            m.UserNickName = model.UserNickName;
            m.UserChnName = model.UserChnName;
            m.IsAdmin = model.IsAdmin;
            m.Remark = model.Remark;
            m.Status = model.Status;

            msg = null;
            return Da.Update(m);
        }

        /// <summary>
        /// 取消管理员
        /// </summary>
        /// <returns></returns>
        private static bool CanCancelAdmin(UserModel userModel, User m, User model, out string msg)
        {
            if (m.Status == (int)UserStatusEnum.Ok)
            {
                //原来是系统管理员
                if (m.IsAdmin)
                {
                    //取消管理员，需要判断至少存在一个系统管理员
                    if (!model.IsAdmin)
                    {
                        //除自己以外没有管理员了
                        if (!HasOtherAdmin(userModel, model))
                        {
                            msg = "该账号是系统唯一管理员，不能取消系统管理身份";
                            return false;
                        }
                    }
                }
            }
            else if (m.Status == (int)UserStatusEnum.Disabled)
            {
                //让他取消
            }
            else if (m.Status == (int)UserStatusEnum.Delete)
            {
                //让他取消
            }

            msg = null;
            return true;
        }


        /// <summary>
        /// 设置为非有效状态
        /// </summary>
        /// <returns></returns>
        private static bool CanCancelStatus(UserModel userModel, User m, User model, out string msg)
        {
            if (m.Status == (int)UserStatusEnum.Ok)
            {
                //设置为非有效状态
                if (model.Status != (int)UserStatusEnum.Ok)
                {
                    //原来是系统管理员
                    if (m.IsAdmin)
                    {
                        //除自己以外没有管理员了
                        if (!HasOtherAdmin(userModel, model))
                        {
                            msg = "该账号是系统唯一管理员，不能设置为非有效状态";
                            return false;
                        }
                    }
                }
                else
                {
                    //让他设置为其他状态
                }
            }

            msg = null;
            return true;
        }

        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public virtual bool UpdateUserPwd(UserModel userModel, User model, out string msg)
        {
            if (model.UserPwd.IsWhiteSpace())
            {
                msg = "请输入密码";
                return false;
            }
            //密码加密
            model.UserPwd = model.UserPwd.ToMd5();

            var m = Da.Get<User>(model.UserId);
            if (m == null)
            {
                msg = "用户不存在";
                return false;
            }

            m.UserPwd = model.UserPwd;

            msg = null;
            return Da.Update(m);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public virtual bool DeleteUser(UserModel userModel, User model, out string msg)
        {
            var m = Da.Get<User>(model.UserId);
            if (m == null)
            {
                msg = "记录不存在";
                return false;
            }

            if (m.Status == (int)UserStatusEnum.Delete)
            {
                msg = "记录已标记为删除";
                return false;
            }

            if (m.UserId == userModel.UserId)
            {
                msg = "不能删除自己";
                return false;
            }

            var flag = CanDeleteAdmin(userModel, m, model, out msg);
            if (!flag)
            {
                return false;
            }

            m.Status = (int)UserStatusEnum.Delete;
            msg = null;
            return Da.Update(m);
        }

        /// <summary>
        /// 删除记录判断系统管理员
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="m"></param>
        /// <param name="model"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private static bool CanDeleteAdmin(UserModel userModel, User m, User model, out string msg)
        {
            if (m.Status == (int)UserStatusEnum.Ok)
            {
                if (m.IsAdmin)
                {
                    //除自己以外没有管理员了
                    if (!HasOtherAdmin(userModel, model))
                    {
                        msg = "该账号是系统唯一管理员，不能删除该账号";
                        return false;
                    }
                }
            }
            else
            {
                //可用直接删除
            }

            msg = null;
            return true;
        }

        /// <summary>
        /// 除自己以外还有其他管理员
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private static bool HasOtherAdmin(UserModel userModel, User model)
        {
            var list = Da.GetList<User>(new
            {
                IsAdmin = true,
                Status = (int)UserStatusEnum.Ok,
            }).Where(p => p.UserId != model.UserId).ToList();

            return list.Count > 0;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual PagenationResult<User> GetUserList(UserModel userModel, int pageIndex, int pageSize, string keyword)
        {
            var roles = Da.GetListAsync<UserRole>();

            var list = GetUserList(userModel);
            if (!keyword.IsWhiteSpace())
            {
                var keys = keyword.ToKeys(StringComparer.OrdinalIgnoreCase).ToList();
                list = list.Where(p => $"{p.UserEmail} {p.UserNickName} {p.UserChnName} {p.Remark}".HasSearchKeys(keys, StringComparison.OrdinalIgnoreCase, false)).ToList();
            }
            var result = new PagenationResult<User>()
            {
                count = list.Count,
                data = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList()
            };

            if (result.data.Count > 0)
            {
                Task.WaitAll(roles);
                foreach (var item in result.data)
                {
                    item.UserRoleName = roles.Result.Find(p => p.UserRoleId == item.UserRoleId)?.UserRoleName;
                }
            }

            return result;
        }

        /// <summary>
        /// 获取所有记录
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public virtual List<User> GetUserList(UserModel userModel)
        {
            return Da.GetList<User>().OrderBy(p => p.Status).ThenByDescending(p => p.IsAdmin).ToList();
        }
    }
}
