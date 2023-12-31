﻿using ServiceEngine.Core.Service;
using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.JWTAuthentication;
using WebApiClientCore;
using WebApiClientCore.Attributes;
using ServiceEngineMasaCore.Blazor.Service.Org.Dto;

namespace ServiceEngineMasaCore.Blazor.Service.Org.Callers
{
    [JwtAuthentication]
    public interface ISysOrgClient : IHttpApi
    {

        /// <summary>
        /// 获取机构列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/sysOrg/list")]
        ITask<AdminResult<List<SysOrg>>> GetSysOrgListAsync(long Id,string Name,string Code,string OrgType, CancellationToken token = default);

        /// <summary>
        /// 增加机构
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysOrg/add")]
        ITask<AdminResult<long>> AddSysOrgAsync([JsonContent] AddOrgInput orgInput, CancellationToken token = default);

        /// <summary>
        /// 更新机构
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysOrg/update")]
        ITask<AdminResult<string>> UpdateSysOrgAsync([JsonContent] UpdateOrgInput orgInput, CancellationToken token = default);

        /// <summary>
        /// 删除机构
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/sysOrg/delete")]
        ITask<AdminResult<string>> DeleteSysOrgAsync([JsonContent] DltOrgInput orgInput, CancellationToken token = default);
    }
}
