using System.Collections.Generic;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Entity.Models
{
    public class MainModel
    {
        public string id { get; set; }
        public string homePage { get; set; }
        public string text { get; set; }
        public List<mn> menu { get; set; }


        public class mn
        {
            public string text { get; set; }
            public List<item> items { get; set; }
        }

        public class item
        {
            public string id { get; set; }
            public string text { get; set; }
            public string href { get; set; }
        }
    }
}
