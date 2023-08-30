﻿using Nest;

namespace ServiceEngine.Core;

/// <summary>
/// ES日志写入器
/// </summary>
public class ElasticSearchLoggingWriter : IDatabaseLoggingWriter
{
    private readonly ElasticClient _esClient;
    private readonly SysConfigService _sysConfigService; // 参数配置服务

    public ElasticSearchLoggingWriter(ElasticClient esClient, SysConfigService sysConfigService)
    {
        _esClient = esClient;
        _sysConfigService = sysConfigService;
    }

    public async void Write(LogMessage logMsg, bool flush)
    {
        // 是否启用操作日志
        var sysOpLogEnabled = await _sysConfigService.GetConfigValue<bool>(CommonConst.SysOpLog);
        if (!sysOpLogEnabled) return;

        var jsonStr = logMsg.Context.Get("loggingMonitor").ToString();
        var loggingMonitor = JSON.Deserialize<dynamic>(jsonStr);

        // 不记录登录登出日志
        if (loggingMonitor.actionName == "userInfo" || loggingMonitor.actionName == "logout")
            return;

        await _esClient.IndexDocumentAsync(jsonStr);
    }
}