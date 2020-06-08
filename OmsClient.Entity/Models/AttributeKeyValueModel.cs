using System.Collections.Generic;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Entity.Models
{
    public class AttributeKeyValueModel
    {
        public string Name { get; set; }
        public List<AttributeKeyValueSelectedModel> Values { get; set; }
    }

    public class AttributeKeyValueSelectedModel
    {
        public string Value { get; set; }
        public bool IsSelected { get; set; }
    }
}
