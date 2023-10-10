using Masa.Blazor.Presets;
using Nest;
using NewLife.Xml;
using Newtonsoft.Json;
using ServiceEngine.Core;
using ServiceEngine.Core.Service;
using ServiceEngineMasaCore.Blazor.Service.Log.Dto;
using ServiceEngineMasaCore.Blazor.Service.Log.Interface;
using ServiceEngineMasaCore.Blazor.Service.Menu.Interface;
using ServiceEngineMasaCore.Blazor.Service.Org.Interface;
using ServiceEngineMasaCore.Blazor.Service.Role.Dto;
using ServiceEngineMasaCore.Blazor.Service.Role.Interface;
using ServiceEngineMasaCore.Blazor.Service.UserInfo.Dto;
using System.Diagnostics.CodeAnalysis;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.CgibinExpressBusinessAccountGetAllResponse.Types;

namespace ServiceEngineMasaCore.Blazor.Pages.App.SystemManagement
{
    public partial class RoleManagement : IDisposable
    {
        PEnqueuedSnackbars? _enqueuedSnackbars;

        [Inject]
        [NotNull]
        ISysMenuService? _sysMenuService { get; set; }

        [Inject]
        [NotNull]
        ISysOrgService? _sysOrgService { get; set; }

        [Inject]
        [NotNull]
        IPopupService? _popupService { get; set; }

        [Inject]
        [NotNull]
        ISysRoleService? _sysRoleService { get; set; }

        List<SysRole> _sysRoleList = new List<SysRole>();
        List<SysOrg> _sysOrgList = new List<SysOrg>();

        PRoleInput input = new PRoleInput();

        private string? _name { get; set; }
        private string? _code { get; set; }

        private bool _dialog { get; set; }
        private string _dialogTitle { get; set; } = string.Empty;

        int _tatolCount = 0;
        int _tatolPage = 1;
        int _currentPage = 1;
        private string _paginationSelect = "10";

        List<DataScopeEnum> _dataScopeEnum = new List<DataScopeEnum>();
        DataScopeEnum _dataScope = DataScopeEnum.All;
        DataScopeRoleInput _dataScopeRoleInput = new();

        UpdateRoleInput _updateRoleInput = new UpdateRoleInput();

        List<SysMenu> _sysMenu = new List<SysMenu>();

        bool _isLoading = false;
        readonly List<DataTableHeader<SysRole>> _headers = new List<DataTableHeader<SysRole>>()
        {
            new() { Text = "序号", Value = nameof(SysRole.Index) },
            new() { Text = "角色名称", Value = nameof(SysRole.Name) },
            new() { Text = "角色编码", Value = nameof(SysRole.Code) },
            new() { Text = "数据范围", Value = nameof(SysRole.DataScope) },
            new() { Text = "排序", Value = nameof(SysRole.OrderNo) },
            new() { Text = "状态", Value = nameof(SysRole.Status) },
            new() { Text = "修改时间", Value = nameof(SysRole.UpdateTime)},
            new() { Text = "备注", Value = nameof(SysRole.Remark) },
            new() { Text = "操作", Value = "Action", Sortable = false, Align=DataTableHeaderAlign.Center }
        };
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _GlobalConfig.NavigationStyleChanged += NavigationStyleChanged;
                foreach (DataScopeEnum dataScope in Enum.GetValues(typeof(DataScopeEnum)))
                {
                    _dataScopeEnum.Add(dataScope);
                }
                _isLoading = true;
                _popupService.ShowProgressLinear();
                _updateRoleInput.MenuIdList = new List<long>();

                Task[] tasks = new Task[] {
                    _sysOrgService.GetSysOrgListAsync(0, "", "", ""),
                    _sysMenuService.GetSysMenuListAsync(null, null),
                    _sysRoleService.GetSysRolePageAsync(input)
                };

                await Task.WhenAll(tasks);

                var res = await (Task<AdminResult<List<SysOrg>>>)tasks[0];
                if (res != null && res.Result != null)
                {
                    _sysOrgList = res.Result;
                }
                var res1 = await (Task<AdminResult<List<SysMenu>>>)tasks[1];
                if (res1 != null && res1.Result != null) {
                    _sysMenu = res1.Result;
                    ChackChildCount(_sysMenu);
                }
                var res2 = await (Task<AdminResult<SqlSugarPagedList<SysRole>>>)tasks[2];
                if (res2 != null && res2.Result?.Items != null)
                {
                    _tatolPage = res2.Result.TotalPages == 0 ? 1 : res2.Result.TotalPages;
                    _tatolCount = res2.Result.Total;
                    _sysRoleList = res2.Result.Items.ToList();
                    _sysRoleList = _sysRoleList.Select((item, index) => {
                        item.Index = index + 1;
                        return item;
                    }).ToList();
                }
                _popupService.HideProgressLinear();
                _isLoading = false;
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task LoadData()
        {
            input = new PRoleInput()
            {
                Page = _currentPage,
                PageSize = int.Parse(_paginationSelect),
                Name = _name ?? string.Empty,
                Code = _code ?? string.Empty,
            };
            var res = await _sysRoleService.GetSysRolePageAsync(input);
            if (res != null && res.Result?.Items != null)
            {
                _tatolPage = res.Result.TotalPages == 0 ? 1 : res.Result.TotalPages;
                _tatolCount = res.Result.Total;
                _sysRoleList = res.Result.Items.ToList();
                _sysRoleList = _sysRoleList.Select((item, index) => {
                    item.Index = index + 1;
                    return item;
                }).ToList();
            }
        }
        private void ChackChildCount(List<SysMenu> menus) {
            foreach (var item in menus)
            {
                item.Children = item.Children.Count == 0 ? null : item.Children;
                if (item.Children != null) { 
                    ChackChildCount(item.Children);
                }
            }
        }
        private void NavigationStyleChanged(object? sender, EventArgs e)
        {
            InvokeAsync(StateHasChanged);
        }
        private async Task OnPaginationValueChange(int value)
        {
            _currentPage = value;
            await LoadData();
        }
        private async Task OnSelectValueChange(string value)
        {
            _paginationSelect = value;
            _currentPage = 1;
            await LoadData();
        }
        private void ResetOnClick()
        {
            _name = null;
            _code = null;
        }
        private void AddRoleOnClick() {
            _dialogTitle = "添加角色";
            _updateRoleInput = new UpdateRoleInput();
            _updateRoleInput.MenuIdList = new List<long>();
            _dialog = true;
        }
        private async Task QueryOnClick()
        {
            _currentPage = 1;
            await LoadData();
        }
        private async void EditRoleOnClick(SysRole role) {
            string str = JsonConvert.SerializeObject(role);
            var userInput = JsonConvert.DeserializeObject<UpdateRoleInput>(str);
            _updateRoleInput = userInput ?? new UpdateRoleInput();
            _updateRoleInput.MenuIdList = new List<long>();
            var res = await _sysRoleService.GetOwnMenuListAsync(role.Id);
            if (res != null && res.Result != null) {
                _updateRoleInput.MenuIdList = res.Result;
            }
            _dialogTitle = "编辑角色";
            _dialog = true;
            StateHasChanged();
        }
        private async Task DataRangeSetOnClick(SysRole role) {
            _dialogTitle = "授权数据范围";
            _dataScope = role.DataScope;
            _dataScopeRoleInput.Id = role.Id;
            _dataScopeRoleInput.DataScope = (int)role.DataScope;
            _dataScopeRoleInput.OrgIdList = new List<long>();
            if (role.DataScope == DataScopeEnum.Define) {
                var res = await _sysRoleService.GetOwnOrgListAsync(role.Id);
                if (res != null && res.Code == 200)
                {
                    _dataScopeRoleInput.OrgIdList = res.Result ?? new List<long>();
                }
            }
            _dialog = true;
        }
        private async Task SubmitOnClick() {
            if (_dialogTitle.Equals("授权数据范围"))
            {
                var res = await _sysRoleService.SysRoleGrantDataScopeAsync(_dataScopeRoleInput);
                if (res != null && res.Code == 200)
                    Enqueue(true, "授权数据范围修改成功");
                else
                    Enqueue(false, "授权数据范围修改失败");
                var role = _sysRoleList.Where(i => i.Id == _dataScopeRoleInput.Id).FirstOrDefault();
                if (role != null)
                {
                    role.DataScope = (DataScopeEnum)_dataScopeRoleInput.DataScope;
                }
            }
            else if (_dialogTitle.Equals("编辑角色")) 
            {
                var res = await _sysRoleService.UpdateSysRoleAsync(_updateRoleInput);
                if (res != null && res.Code == 200) { 
                    Enqueue(true, "角色修改成功");
                    for (int i = 0; i < _sysRoleList.Count; i++)
                    {
                        if (_sysRoleList[i].Id == _updateRoleInput.Id) {
                            _sysRoleList[i] = _updateRoleInput;
                            break;
                        }
                    }
                }
                else 
                    Enqueue(false, "角色修改失败");
            }
            else if (_dialogTitle.Equals("添加角色")) 
            {
                var res = await _sysRoleService.AddSysRoleAsync(_updateRoleInput);
                if (res != null && res.Code == 200) { 

                    Enqueue(true, "角色添加成功");
                    _sysRoleList.Add(_updateRoleInput);
                }
                else
                    Enqueue(false, "角色添加失败");
            }
            _dialog = false;
        }
        private async Task DltRoleOnClick(SysRole role) {
            var confirmed = await _popupService.ConfirmAsync(param =>
            {
                param.Title = "提示";
                param.Content = $"是否删除角色【{role.Name}】?";
                param.OkText = @T("Confirm");
                param.CancelText = @T("Cancel");
            });
            if (confirmed)
            {
                DltRoleInput dltInput = new DltRoleInput()
                {
                    Id = role.Id
                };
                var res = await _sysRoleService.DeleteSysRoleAsync(dltInput);
                if (res != null && res.Code == 200)
                {
                    Enqueue(true, "删除成功");
                    var dltItem = _sysRoleList.Where(i => i.Id == role.Id)?.FirstOrDefault();
                    if (dltItem != null)
                    {
                        _sysRoleList.Remove(dltItem);
                    }
                }
                else
                {
                    Enqueue(true, "删除失败");
                }
            }
        }
        private void Enqueue(bool result, string? context)
        {
            _enqueuedSnackbars?.EnqueueSnackbar(new SnackbarOptions()
            {
                Content = context,
                Type = result ? AlertTypes.Success : AlertTypes.Error,
                Closeable = true
            });
        }
        public void Dispose()
        {
            _sysRoleList.Clear();
        }
    }
}
