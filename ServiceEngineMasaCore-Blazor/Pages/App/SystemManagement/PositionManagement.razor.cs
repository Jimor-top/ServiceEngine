using ServiceEngine.Core;
using ServiceEngine.Core.Service;
using ServiceEngineMasaCore.Blazor.Service.Pos.Interface;
using ServiceEngineMasaCore.Blazor.Service.Role.Dto;
using System.Diagnostics.CodeAnalysis;

namespace ServiceEngineMasaCore.Blazor.Pages.App.SystemManagement
{
    public partial class PositionManagement : IDisposable
    {
        [Inject]
        [NotNull]
        ISysPosService? _sysPosService { get; set; }

        List<SysPos> _sysPosList = new List<SysPos>();
        PosInput input = new PosInput();

        bool _isLoading = false;
        readonly List<DataTableHeader<SysPos>> _headers = new List<DataTableHeader<SysPos>>()
        {
            new() { Text = "序号", Value = nameof(SysPos.Index) },
            new() { Text = "职位名称", Value = nameof(SysPos.Name) },
            new() { Text = "职位编码", Value = nameof(SysPos.Code) },
            new() { Text = "排序", Value = nameof(SysPos.OrderNo) },
            new() { Text = "状态", Value = nameof(SysPos.Status) },
            new() { Text = "修改时间", Value = nameof(SysPos.UpdateTime)},
            new() { Text = "备注", Value = nameof(SysPos.Remark) },
        };
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _GlobalConfig.NavigationStyleChanged += NavigationStyleChanged;
                _isLoading = true;
                await LoadData(input);
               
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task LoadData(PosInput input) {
            var res = await _sysPosService.GetSysPosListAsync(input);
            if (res != null && res.Result != null)
            {
                _sysPosList.AddRange(res.Result);
                _sysPosList = _sysPosList.Select((item, index) => {
                    item.Index = index + 1;
                    return item;
                }).ToList();
                _isLoading = false;
            }
        }

        private void NavigationStyleChanged(object? sender, EventArgs e)
        {
            InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            _sysPosList.Clear();
        }
    }
}
