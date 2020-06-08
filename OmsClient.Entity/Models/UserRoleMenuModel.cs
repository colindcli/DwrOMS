using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Entity.Models
{
    public class UserRoleMenuModel
    {
        public string Name { get; set; }

        public List<UserRoleMenuList> Lists { get; set; }
    }

    public class UserRoleMenuList
    {
        /// <summary>
        /// 
        /// </summary>
        public int UserMenuId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UserMenuText { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsChecked { get; set; }
    }

    public class UserRoleMenuRequest
    {
        public List<int> UserMenuIds { get; set; }

        public Guid UserRoleId { get; set; }
    }

}
