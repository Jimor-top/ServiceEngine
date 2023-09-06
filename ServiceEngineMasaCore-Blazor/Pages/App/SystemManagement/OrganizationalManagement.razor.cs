using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.Service.Org.Interface;
using ServiceEngineMasaCore.Blazor.Service.UserInfo.Dto;
using ServiceEngineMasaCore.Blazor.Service.UserInfo.Interface;
using System.Diagnostics.CodeAnalysis;

namespace ServiceEngineMasaCore.Blazor.Pages.App.SystemManagement
{
    public partial class OrganizationalManagement
    {
        [Inject]
        [NotNull]
        IPopupService? _popupService { get; set; }

        [Inject]
        [NotNull]
        ISysOrgService? _sysOrgService { get; set; }

        List<SysOrg> _sysOrgList = new List<SysOrg>();

        List<long> _active = new List<long>();
        static private string? _account { get; set; }
        static private string? _phone { get; set; }
        private bool _singleExpand;
        readonly List<DataTableHeader<SysOrg>> _headers = new()
        {
            new (){ Text="",Value="data-table-expand",Width="10%"},
            new() { Text = "机构名称", Value = nameof(SysOrg.Name),Width="10%" },
            new() { Text = "机构编码", Value = nameof(SysOrg.Code) ,Width="10%"},
            new() { Text = "机构类型", Value = nameof(SysOrg.OrgType) ,Width="10%"},
            new() { Text = "排序", Value = nameof(SysOrg.OrderNo) , Width = "10%"},
            new() { Text = "状态", Value = nameof(SysOrg.Status),Width="10%"},
            new() { Text = "修改时间", Value = nameof(SysOrg.UpdateTime) , Width = "10%"},
            new() { Text = "备注", Value = nameof(SysOrg.Remark) , Width = "10%"},
            new() { Text = "操作", Value = "Action", Sortable = false ,Width="10%"}
        };
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _GlobalConfig.NavigationStyleChanged += NavigationStyleChanged;
                _popupService.ShowProgressLinear();
                var res = await _sysOrgService.GetSysOrgList(0, "", "", "");
                if (res != null && res.Result != null)
                {
                    _sysOrgList = res.Result;
                }
                _popupService.HideProgressLinear();
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }
        private void NavigationStyleChanged(object? sender, EventArgs e)
        {
            InvokeAsync(StateHasChanged);
        }
        private async Task ActiveUpdated(List<SysOrg> activedItems)
        {

        }

        private void OpenAddDialog(long parentId, string parentName)
        {

        }
        public void Dispose()
        {
            _sysOrgList.Clear();
        }
    }
}
