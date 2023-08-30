using Newtonsoft.Json;
using ServiceEngine.Core;
using ServiceEngine.Core.Service;
using ServiceEngineMasaCore.Blazor.Service.Log.Dto;
using ServiceEngineMasaCore.Blazor.Service.Org.Interface;
using ServiceEngineMasaCore.Blazor.Service.UserInfo.Dto;
using ServiceEngineMasaCore.Blazor.Service.UserInfo.Interface;
using System.Diagnostics.CodeAnalysis;

namespace ServiceEngineMasaCore.Blazor.Pages.App.SystemManagement
{
    public partial class AccountManagement : IDisposable
    {
        [Inject]
        [NotNull]
        ISysOrgService? _sysOrgService { get; set; }  
        
        [Inject]
        [NotNull]
        ISysUserService? _sysUserServices { get; set; }

        List<SysOrg> _sysOrgList = new List<SysOrg>();
        List<SysUser> _sysUserList = new List<SysUser>();

        PageInputDto input = new PageInputDto();

        List<long> _active = new List<long>();

        int _tatolCount = 0;
        int _tatolPage = 1;
        int _currentPage = 1;
        private string _paginationSelect = "10";

        static private string? _account { get; set; }
        static private string? _phone { get; set; }
        readonly List<DataTableHeader<SysUser>> _headers = new()
        {
            new() { Text = "序号", Value = nameof(SysUser.Index) },
            new() { Text = "账号", Value = nameof(SysUser.Account) },
            new() { Text = "昵称", Value = nameof(SysUser.NickName) },
            new() { Text = "头像", Value = nameof(SysUser.Avatar) },
            new() { Text = "姓名", Value = nameof(SysUser.RealName) },
            new() { Text = "性别", Value = nameof(SysUser.Sex) },
            new() { Text = "手机号码", Value = nameof(SysUser.Phone)},
            new() { Text = "状态", Value = nameof(SysUser.Status) },
            new() { Text = "排序", Value = nameof(SysUser.OrderNo) },
            new() { Text = "修改时间", Value = nameof(SysUser.UpdateTime) },
            new() { Text = "备注", Value = nameof(SysUser.Remark) },
            new() { Text = "操作", Value = "Action", Sortable = false }
        };

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _GlobalConfig.NavigationStyleChanged += NavigationStyleChanged;
                var res = await _sysOrgService.GetSysOrgList(0, "", "", "");
                if (res != null && res.Result != null)
                {
                    _sysOrgList = res.Result;
                }
                input = new PageInputDto() {
                    Page = 1,
                    PageSize = int.Parse(_paginationSelect),
                    OrgId = -1,
                };
                await LoadData(input);
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task LoadData(PageInputDto input) {
            var res = await _sysUserServices.GetSysUserPageAsync(input);
            if (res != null && res.Result != null && res.Result.Items != null)
            {
                _tatolPage = res.Result.TotalPages == 0 ? 1 : res.Result.TotalPages;
                _tatolCount = res.Result.Total;
                _sysUserList = res.Result.Items.ToList();
                _sysUserList = _sysUserList.Select((item, index) => {
                    item.Index = index + 1;
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
            input = new PageInputDto()
            {
                Page = value,
                PageSize = int.Parse(_paginationSelect),
                OrgId = -1,
            };
            await LoadData(input);
        }
        private async Task OnSelectValueChange(string value)
        {
            _paginationSelect = value;
            _currentPage = 1;
            input = new PageInputDto()
            {
                Page = 1,
                PageSize = int.Parse(_paginationSelect),
                OrgId = -1,
            };
            await LoadData(input);
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
            _sysUserList.Clear();
        }  
    }
}
