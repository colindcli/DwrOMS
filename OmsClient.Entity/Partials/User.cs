using System.ComponentModel.DataAnnotations.Schema;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Entity
{
    public partial class User
    {
        [NotMapped]
        public string SafeCode { get; set; }
        [NotMapped]
        public string Token { get; set; }
        [NotMapped]
        public long Salt { get; set; }
        [NotMapped]
        public string Code { get; set; }
        [NotMapped]
        public string UserRoleName { get; set; }
    }
}
