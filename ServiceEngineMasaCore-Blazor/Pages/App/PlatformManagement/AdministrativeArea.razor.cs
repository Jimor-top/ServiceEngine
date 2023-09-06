using FluentEmail.Core;
using Newtonsoft.Json;
using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.Service.Log.Dto;
using ServiceEngineMasaCore.Blazor.Service.Region.Dto;
using ServiceEngineMasaCore.Blazor.Service.Region.Interface;
using SqlSugar;
using System.Diagnostics.CodeAnalysis;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.CgibinUserInfoBatchGetRequest.Types;

namespace ServiceEngineMasaCore.Blazor.Pages.App.PlatformManagement
{
    public partial class AdministrativeArea : IDisposable
    {
        [Inject]
        [NotNull]
        IPopupService? _popupService { get; set; }

        [Inject]
        [NotNull]
        ISysRegionService? _sysRegionService { get; set; }


        List<SysRegion> _sysRegionList = new List<SysRegion>();
        List<SysRegion> _sysRegionPage = new List<SysRegion>();
        PRegionInput input = new PRegionInput();

        List<long> _active = new List<long>();

        int _tatolCount = 0;
        int _tatolPage = 1;
        int _currentPage = 1;
        private string _paginationSelect = "10";

        readonly List<DataTableHeader<SysRegion>> _headers = new()
        {
          new() { Text = "序号", Value = nameof(SysRegion.Index) },
            new() { Text = "行政名称", Value = nameof(SysRegion.Name) },
            new() { Text = "行政代码", Value = nameof(SysRegion.Code) },
            new() { Text = "区号", Value = nameof(SysRegion.CityCode) },
            new() { Text = "排序", Value = nameof(SysRegion.OrderNo) },
            new() { Text = "备注", Value = nameof(SysRegion.Remark) },
            new() { Text = "操作", Value = "Action", Sortable = false }
        };
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _GlobalConfig.NavigationStyleChanged += NavigationStyleChanged;
                _popupService.ShowProgressLinear();
                var res = await _sysRegionService.GetSysRegionListAsync(0);
                if (res != null && res.Result != null)
                {
                    foreach (var item in res.Result)
                    {
                        if (item?.Id != null)
                        {
                            item.Children = new List<SysRegion>();
                        }
                    }
                    _sysRegionList = res.Result;
                }

                input = new() { 
                    Page = 1,
                    PageSize = int.Parse(_paginationSelect),
                };
                await LoadData(input);
                _popupService.HideProgressLinear();
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task LoadData(PRegionInput input) {
            var res = await _sysRegionService.GetSysRegionPageAsync(input);
            if (res != null && res.Result != null && res.Result.Items != null)
            {
                _tatolPage = res.Result.TotalPages == 0 ? 1 : res.Result.TotalPages;
                _tatolCount = res.Result.Total;
                _sysRegionPage = res.Result.Items.ToList();
                _sysRegionPage = _sysRegionPage.Select((item, index) => {
                    item.Index = (input.Page - 1) * input.PageSize + index + 1;
                    return item;
                }).ToList();
            }
        }

        private void NavigationStyleChanged(object? sender, EventArgs e)
        {
            InvokeAsync(StateHasChanged);
        }
        private async Task OnPaginationValueChange(int value)
        {
            _currentPage = value;
            input = new()
            {
                Page = value,
                PageSize = int.Parse(_paginationSelect),
            };
            await LoadData(input);
        }
        private async Task OnSelectValueChange(string value)
        {
            _paginationSelect = value;
            _currentPage = 1;
            input = new()
            {
                Page = 1,
                PageSize = int.Parse(value),
            };
            await LoadData(input);
        }
        public async Task FetchRegion(SysRegion item)
        {
            var res = await _sysRegionService.GetSysRegionListAsync(item.Id);
            if (res != null && res?.Result.Count != 0)
            {
                foreach (var resItem in res.Result)
                {
                    if (resItem?.Id != null)
                    {
                        resItem.Children = new List<SysRegion>();
                    }
                }
                item.Children = res.Result;
            }
            else {
                item.Children = null;
            }
        }

        public void Dispose()
        {
            _sysRegionList.Clear();
            _sysRegionPage.Clear();
        }
    }
}
