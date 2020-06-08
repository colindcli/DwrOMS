using OmsClient.Biz.Bases;
using OmsClient.DataAccess.Manages;
using OmsClient.Entity;
using OmsClient.Entity.Enums;
using OmsClient.Entity.Models;
using OmsClient.Entity.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using OmsClient.DataAccess.Users;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Biz.Manages
{
    public class ManageBuyOrderBiz : BaseBuyBiz
    {
        private static readonly ManageBuyOrderDa Da = new ManageBuyOrderDa();

        /// <summary>
        /// 统计订单数量
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public List<int> GetMyOrderNumber(UserModel userModel)
        {
            var list = new UserBuyOrderDa().GetSaleOrderNumberList();

            var result = new List<int>();

            //待审核采购单
            var a = list.Count(p => p.Status == (int)BuyOrderStatusEnum.SubmitCheck);
            result.Add(a);

            //已审核采购单
            var b = list.Count(p => p.Status == (int)BuyOrderStatusEnum.SubmitPass);
            result.Add(b);

            return result;
        }

        /// <summary>
        /// 待审核采购单
        /// </summary>
        /// <returns></returns>
        public PagenationResult<BuyOrderListResult> GetUncheckOrder(UserModel userModel, int pageIndex, int pageSize, string keyword)
        {
            return GetListByStatus(userModel, pageIndex, pageSize, keyword, new List<int>()
            {
                (int) BuyOrderStatusEnum.SubmitCheck
            }, list => list.OrderByDescending(p => p.PostCheckDate).ToList());
        }

        /// <summary>
        /// 已审核采购单
        /// </summary>
        /// <returns></returns>
        public PagenationResult<BuyOrderListResult> GetCheckOrder(UserModel userModel, int pageIndex, int pageSize, string keyword)
        {
            return GetListByStatus(userModel, pageIndex, pageSize, keyword, new List<int>()
            {
                (int) BuyOrderStatusEnum.SubmitPass,
            }, list => list.OrderByDescending(p => p.CheckDate).ToList());
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="request"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool CheckBuyOrder(UserModel userModel, BuyOrderCheckModel request, out string msg)
        {
            var order = Da.Get<BuyOrder>(request.BuyOrderId);
            var b1 = CheckAuthOrder(userModel, order, out msg);
            if (!b1)
            {
                return false;
            }

            if (order.Status != (int)BuyOrderStatusEnum.SubmitCheck)
            {
                msg = "状态已改变，操作失败";
                return false;
            }

            order.CheckRemark = request.Remark;
            order.CheckUserName = userModel.UserNickName;
            order.CheckDate = DateTime.Now;

            if (request.Pass)
            {
                order.Status = (int)BuyOrderStatusEnum.SubmitPass;
            }
            else
            {
                order.Status = (int)BuyOrderStatusEnum.SubmitBack;
            }

            Da.Update(order);
            return true;
        }
    }
}
