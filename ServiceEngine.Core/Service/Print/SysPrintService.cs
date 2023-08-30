﻿namespace ServiceEngine.Core.Service;

/// <summary>
/// 系统打印模板服务
/// </summary>
[ApiDescriptionSettings(Order = 305)]
public class SysPrintService : IDynamicApiController, ITransient
{
    private readonly SqlSugarRepository<SysPrint> _sysPrintRep;

    public SysPrintService(SqlSugarRepository<SysPrint> sysPrintRep)
    {
        _sysPrintRep = sysPrintRep;
    }

    /// <summary>
    /// 获取打印模板列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [DisplayName("获取打印模板列表")]
    public async Task<SqlSugarPagedList<SysPrint>> Page(PagePrintInput input)
    {
        return await _sysPrintRep.AsQueryable()
            .WhereIF(!string.IsNullOrWhiteSpace(input.Name), u => u.Name.Contains(input.Name))
            .OrderBy(u => u.OrderNo)
            .ToPagedListAsync(input.Page, input.PageSize);
    }

    /// <summary>
    /// 增加打印模板
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Add"), HttpPost]
    [DisplayName("增加打印模板")]
    public async Task AddPrint(AddPrintInput input)
    {
        var isExist = await _sysPrintRep.IsAnyAsync(u => u.Name == input.Name);
        if (isExist)
            throw Oops.Oh(ErrorCodeEnum.D1800);

        await _sysPrintRep.InsertAsync(input.Adapt<SysPrint>());
    }

    /// <summary>
    /// 更新打印模板
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Update"), HttpPost]
    [DisplayName("更新打印模板")]
    public async Task UpdatePrint(UpdatePrintInput input)
    {
        var isExist = await _sysPrintRep.IsAnyAsync(u => u.Name == input.Name && u.Id != input.Id);
        if (isExist)
            throw Oops.Oh(ErrorCodeEnum.D1800);

        await _sysPrintRep.AsUpdateable(input.Adapt<SysPrint>()).IgnoreColumns(true).ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除打印模板
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [ApiDescriptionSettings(Name = "Delete"), HttpPost]
    [DisplayName("删除打印模板")]
    public async Task DeletePrint(DeletePrintInput input)
    {
        await _sysPrintRep.DeleteAsync(u => u.Id == input.Id);
    }
}