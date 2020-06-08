using OmsClient.Biz.Users;
using OmsClient.Common.Utilitys;
using OmsClient.Web.ControllersBase;
using System;
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
    public class ProductImageController : BaseController
    {
        private static readonly UserProductBiz Biz = new UserProductBiz();

        /// <summary>
        /// 查看图片
        /// </summary>
        /// <param name="id">ProductId</param>
        /// <returns></returns>
        public ActionResult Index(Guid id)
        {
            var list = Biz.GetProductImages(id, out var product);
            if (product == null)
            {
                return null;
            }

            var m = TokenUserHandle.GetToken();
            ViewBag.IsUser = m != null;
            ViewBag.Product = product;

            return View(list);
        }
    }
}
