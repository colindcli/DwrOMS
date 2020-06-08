using DwrUtility;
using OmsClient.DataAccess.Admins;
using OmsClient.Entity;
using OmsClient.Entity.Results;
using System;
using System.Collections.Generic;
using System.Linq;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Biz.Admins.Bases
{
    public abstract class AdminBaseFriendLinkBiz
    {
        protected static readonly AdminFriendLinkDa Da = new AdminFriendLinkDa();

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool AddFriendLink(FriendLink model)
        {
			model.FriendLinkId = SeqGuid.NewGuid();
			model.IsEnabled = true;
			model.CreateDate = DateTime.Now;
            Da.Add<Guid, FriendLink>(model);
            return true;
        }

        /// <summary>
        /// 获取一条记录
        /// </summary>
        /// <param name="friendLinkId"></param>
        /// <returns></returns>
        public virtual FriendLink GetFriendLinkById(Guid friendLinkId)
        {
            return Da.Get<FriendLink>(friendLinkId);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool UpdateFriendLink(FriendLink model)
        {
            var m = Da.Get<FriendLink>(model.FriendLinkId);
            if (m == null)
            {
                return false;
            }

			m.FriendLinkName = model.FriendLinkName;
			m.FriendLinkUrl = model.FriendLinkUrl;
			m.FriendLinkRemarkText = model.FriendLinkRemarkText;
			m.FriendLinkSort = model.FriendLinkSort;
			m.FriendLinkLogoImage = model.FriendLinkLogoImage;
			m.IsTop = model.IsTop;
			m.IsEnabled = model.IsEnabled;
			m.ExpirationDate = model.ExpirationDate;

            return Da.Update(m);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual bool DeleteFriendLink(FriendLink model)
        {
            return Da.Delete(model);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual PagenationResult<FriendLink> GetFriendLinkList(int pageIndex, int pageSize, string keyword)
        {
            var list = GetFriendLinkList().Where(p => p.FriendLinkName.IsContains(keyword)).ToList();
            var result = new PagenationResult<FriendLink>()
            {
                count = list.Count,
                data = list.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList()
            };
            return result;
        }

        /// <summary>
        /// 获取所有记录
        /// </summary>
        /// <returns></returns>
        public virtual List<FriendLink> GetFriendLinkList()
        {
            return Da.GetList<FriendLink>().OrderBy(p => p.FriendLinkSort).ToList();
        }
    }
}
