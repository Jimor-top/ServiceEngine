﻿namespace ServiceEngine.Core;

/// <summary>
/// HTTP请求类型
/// </summary>
[Description("HTTP请求类型")]
public enum RequestTypeEnum
{
    /// <summary>
    /// 执行内部方法
    /// </summary>
    Run = 0,

    /// <summary>
    /// GET
    /// </summary>
    Get = 1,

    /// <summary>
    /// POST
    /// </summary>
    Post = 2,

    /// <summary>
    /// PUT
    /// </summary>
    Put = 3,

    /// <summary>
    /// DELETE
    /// </summary>
    Delete = 4
}