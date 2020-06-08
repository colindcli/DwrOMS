using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Entity.Results
{
    public class SaleTrackResult
    {
        public List<SaleTrack> Tracks { get; set; }

        /// <summary>
        /// 合计（人民币）
        /// </summary>
        public decimal TotalRmb { get; set; }
    }
}
