using System.ComponentModel;

/*
 * Copyright (c) 2020 深圳市德旺荣科技发展有限公司
 * All rights reserved.
 */
namespace OmsClient.Entity.Enums
{
    /// <summary>
    /// 
    /// </summary>
    public enum ReturnCode
    {

        /// <summary>
        /// 返回报错消息
        /// </summary>
        [Description("返回报错消息")]
        Message = -100,

        /// <summary>
        /// 页面未授权
        /// </summary>
        [Description("页面未授权")]
        PageInvalid = -3,

        /// <summary>
        /// token无效
        /// </summary>
        [Description("登录已无效")]
        TokenInvalid = -2,

        /// <summary>
        /// 系统繁忙
        /// </summary>
        [Description("系统繁忙")]
        SystemError = -1,

        /// <summary>
        /// 请求成功
        /// </summary>
        [Description("成功")]
        Ok = 0,
        /// <summary>
        /// 参数不正确
        /// </summary>
        [Description("参数不正确")]
        QueryParamError = 1,
        /// <summary>
        /// 图片上传失败
        /// </summary>
        [Description("图片上传失败")]
        UploadFileError = 2,
        /// <summary>
        /// 账号或密码不正确
        /// </summary>
        [Description("账号或密码不正确")]
        LoginFailed = 3,
        /// <summary>
        /// 添加失败
        /// </summary>
        [Description("添加失败")]
        AddFailed = 4,
        /// <summary>
        /// 更新失败
        /// </summary>
        [Description("更新失败")]
        UpdateFailed = 5,
        /// <summary>
        /// 删除失败
        /// </summary>
        [Description("删除失败")]
        DeleteFailed = 6,
        /// <summary>
        /// 密码不能为空
        /// </summary>
        [Description("密码不能为空")]
        PasswordFailed = 7,
        /// <summary>
        /// 账号或密码错误
        /// </summary>
        [Description("账号或密码错误")]
        UserLoginFailed = 8,
        /// <summary>
        /// 邮箱不存在
        /// </summary>
        [Description("邮箱不存在")]
        UserEmailNoExists = 9,
        /// <summary>
        /// 密码修改失败
        /// </summary>
        [Description("密码修改失败")]
        UserChangePwdFailed = 10,
        /// <summary>
        /// 邮箱已存在
        /// </summary>
        [Description("邮箱已存在")]
        EmailIsExists = 11,
        /// <summary>
        /// 不是邮箱地址
        /// </summary>
        [Description("邮箱地址错误")]
        IsNotEmail = 12,
        /// <summary>
        /// 验证码不正确
        /// </summary>
        [Description("验证码不正确")]
        SafeCodeError = 13,
        /// <summary>
        /// 登录超时，请重试
        /// </summary>
        [Description("登录超时，请重试")]
        TicksTimeout = 14,
    }
}
