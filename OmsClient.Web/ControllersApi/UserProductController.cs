using DwrUtility;
using OmsClient.Biz.Users;
using OmsClient.Common;
using OmsClient.Entity;
using OmsClient.Entity.Enums;
using OmsClient.Entity.Results;
using OmsClient.Web.ControllersBase;
using System;
using System.IO;
using System.Net.Http;
using System.Web;
using System.Web.Http;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Web.ControllersApi
{
    /// <summary>
    /// 
    /// </summary>
    //[MenuApiFilter(MenuEnum.Product)]
    public class UserProductController : BaseUserApiController
    {
        private static readonly UserCategoryBiz BizCategory = new UserCategoryBiz();
        private static readonly UserProductBiz Biz = new UserProductBiz();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetCategoryList()
        {
            var result = BizCategory.GetCategoryList(UserModel);
            return ReturnResult(result);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult AddProduct(Product model)
        {
            if (model == null)
            {
                return ReturnResult(false, ReturnCode.QueryParamError);
            }
            if (model.CategoryId <= 0)
            {
                return ReturnMsg("请选择分类");
            }

            var result = Biz.AddProduct(UserModel, model, out var msg);
            return ReturnResult(result, msg);
        }

        /// <summary>
        /// 获取一条记录
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetProductById(Guid productId)
        {
            var result = Biz.GetProductById(UserModel, productId);
            return ReturnResult(result);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult UpdateProduct(Product model)
        {
            if (model == null)
            {
                return ReturnResult(false, ReturnCode.QueryParamError);
            }
            if (model.CategoryId <= 0)
            {
                return ReturnMsg("请选择分类");
            }

            var result = Biz.UpdateProduct(UserModel, model, out var msg);
            return ReturnResult(result, msg);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult DeleteProduct(Product model)
        {
            var result = Biz.DeleteProduct(UserModel, model, out var msg);
            return ReturnResult(result, msg);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="categoryId"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetProductList(int page, int limit, int? categoryId = null, string keyword = null)
        {
            return Biz.GetProductList(UserModel, page, limit, categoryId, keyword);
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="categoryId"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetProductQueryList(int page, int limit, int? categoryId = null, string keyword = null)
        {
            return Biz.GetProductQueryList(UserModel, page, limit, categoryId, keyword);
        }

        /// <summary>
        /// 获取所有记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetProductList()
        {
            var result = Biz.GetProductList(UserModel);
            return ReturnResult(result);
        }

        /// <summary>
        /// 获取产品图片
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetProductImageList(Guid productId)
        {
            var result = Biz.GetProductImageList(UserModel, productId);
            return ReturnResult(result);
        }

        /// <summary>
        /// 上传附件
        /// api/UserFile/Uplad
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult Upload(Guid productId)
        {
            var files = HttpContext.Current.Request.Files;
            if (files.Count == 0)
            {
                return null;
            }

            var fullPath = $"{Config.Root}/Files/Temp/{Guid.NewGuid()}.jpg";
            fullPath.CreateDirByFilePath();
            files[0].SaveAs(fullPath);

            var result = Biz.UpdateImage(UserModel, productId, fullPath, out var path, out var msg);
            if (!result)
            {
                return ReturnResult(false, msg);
            }

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            return ReturnResult(path);
        }

        /// <summary>
        /// 产品图片排序
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult ImageSort(Product model)
        {
            if (model == null || model.ProductId == Guid.Empty || model.Paths == null || model.Paths.Count == 0)
            {
                return ReturnResult(false, "参数错误");
            }

            var result = Biz.ImageSort(UserModel, model, out var msg);
            return ReturnResult(result, msg);
        }

        /// <summary>
        /// 删除产品图片
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult DeleteImage(ProductImage model)
        {
            var result = Biz.DeleteProductImage(UserModel, model, out var msg);
            return ReturnResult(result, msg);
        }

        /// <summary>
        /// 获取在途数量
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetInTransitQty(Guid productId)
        {
            var result = Biz.GetInTransitQty(UserModel, productId);
            return ReturnResult(result);
        }

        /// <summary>
        /// 获取挂起数量
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet]
        public IResponseResult GetHoldQty(Guid productId)
        {
            var result = Biz.GetHoldQty(UserModel, productId);
            return ReturnResult(result);
        }

        /// <summary>
        /// 导入产品
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IResponseResult ImportProduct()
        {
            var categoryId = HttpContext.Current.Request.Form["CategoryId"].ToInt();
            var files = HttpContext.Current.Request.Files;
            if (files.Count == 0)
            {
                return ReturnResult(false, "请选择附件");
            }

            var item = files[0];
            var tempPath = $"{Config.Root}/Files/Temp/{Guid.NewGuid()}";
            item.SaveAs(tempPath);

            //
            var result = Biz.ImportProduct(UserModel, categoryId, tempPath, out var msg);

            //删除文件
            if (File.Exists(tempPath))
            {
                File.Delete(tempPath);
            }

            return ReturnResult(result, msg);
        }

        /// <summary>
        /// 导出产品
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage ExportProduct(int? categoryId, string keyword)
        {
            var result = Biz.ExportProduct(UserModel, categoryId, keyword, out var msg, out var bt);
            if (!result)
            {
                return new HttpResponseMessage()
                {
                    Content = new StringContent(msg)
                };
            }

            return ResponseAttachment(bt, "导出产品.xlsx");
        }
    }
}
