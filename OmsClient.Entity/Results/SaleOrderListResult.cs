using DwrUtility;
using System;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Entity.Results
{
    public class SaleOrderListResult
    {
        /// <summary>
        /// 
        /// </summary>
        public virtual Guid SaleOrderId { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public virtual string SaleOrderNumber { get; set; }
        /// <summary>
        /// 币种，如：人民币
        /// </summary>
        public virtual string CurrencyName { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public virtual string Title { get; set; }
        /// <summary>
        /// 收货人
        /// </summary>
        public virtual string ToConsignee { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateDate { get; set; }
        /// <summary>
        /// 备注。告诉仓库备货信息
        /// </summary>
        public virtual string DefaultRemark { get; set; }
        /// <summary>
        /// 开始备货
        /// </summary>
        public virtual DateTime? StockStart { get; set; }
        /// <summary>
        /// 开始备货人
        /// </summary>
        public virtual string StockStartName { get; set; }
        /// <summary>
        /// 完成备货
        /// </summary>
        public virtual DateTime? StockEnd { get; set; }
        /// <summary>
        /// 完成备货人
        /// </summary>
        public virtual string StockEndName { get; set; }

        /// <summary>
        /// 备货状态
        /// </summary>
        public string StockInfo
        {
            get
            {
                var isStart = false;
                string startInfo;
                if (StockStart.HasValue && !StockStartName.IsWhiteSpace())
                {
                    startInfo = $"{StockStartName}({StockStart.Value:MM/dd HH:mm})";
                    isStart = true;
                }
                else
                {
                    startInfo = "-";
                }

                var isEnd = false;
                string endInfo;
                if (StockEnd.HasValue && !StockEndName.IsWhiteSpace())
                {
                    endInfo = $"{StockEndName}({StockEnd.Value:MM/dd HH:mm})";
                    isEnd = true;
                }
                else
                {
                    endInfo = "-";
                }

                string info;
                if (!isStart)
                {
                    info = "未开始";
                }
                else
                {
                    if (!isEnd)
                    {
                        info = $"进行中:{startInfo}";
                    }
                    else
                    {
                        info = $"已完成:{startInfo}-{endInfo}";
                    }
                }

                return info;
            }
        }

        /// <summary>
        /// 提交时间
        /// </summary>
        public virtual DateTime? PostDate { get; set; }
        /// <summary>
        /// 状态：1草稿订单；2提交备货未收款；3提交备货已收款；4已备货未收款；5已备货已收款；6已发货未收款；7已发货已收款；
        /// </summary>
        public virtual int Status { get; set; }
        /// <summary>
        /// 销售员
        /// </summary>
        public virtual string UserNickName { get; set; }

        /// <summary>
        /// 出库时间
        /// </summary>
        public virtual DateTime? StockOutDate { get; set; }
    }
}
