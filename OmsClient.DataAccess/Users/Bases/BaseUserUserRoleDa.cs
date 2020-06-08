using Dapper;
using OmsClient.Entity;
using OmsClient.Entity.Models;
using System;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.DataAccess.Users.Bases
{
    public abstract class BaseUserUserRoleDa : RepositoryBase
    {
        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool DeleteUserRole(UserModel userModel, UserRole model)
        {
            Db((db, tran) =>
            {
                db.Delete(model, tran);
                db.DeleteList<UserRoleMenu>(new
                {
                    model.UserRoleId,
                }, tran);
            });
            return true;
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateUserRoleSet(UserModel userModel, UserRoleMenuRequest model)
        {
            Db((db, tran) =>
            {
                db.DeleteList<UserRoleMenu>(new
                {
                    model.UserRoleId,
                }, tran);

                foreach (var id in model.UserMenuIds)
                {
                    db.Insert<Guid, UserRoleMenu>(new UserRoleMenu()
                    {
                        UserRoleMenuId = SeqGuid.NewGuid(),
                        UserRoleId = model.UserRoleId,
                        UserMenuId = id
                    }, tran);
                }
            });

            return true;
        }
    }
}