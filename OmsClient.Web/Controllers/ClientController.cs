using OmsClient.Web.ControllersBase;
using OmsClient.Web.Filters;
using System.Web.Mvc;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ClientController : BaseUserController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseNoCacheFilter]
        public ActionResult Index()
        {
            return View();
        }
    }
}
