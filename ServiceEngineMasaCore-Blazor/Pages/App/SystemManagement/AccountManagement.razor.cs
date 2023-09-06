using Mapster;
using MapsterMapper;
using Masa.Blazor.Presets;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using ServiceEngine.Core;
using ServiceEngine.Core.Service;
using ServiceEngineMasaCore.Blazor.Service.Org.Interface;
using ServiceEngineMasaCore.Blazor.Service.Pos.Interface;
using ServiceEngineMasaCore.Blazor.Service.Role.Interface;
using ServiceEngineMasaCore.Blazor.Service.UserInfo.Dto;
using ServiceEngineMasaCore.Blazor.Service.UserInfo.Interface;
using System.Diagnostics.CodeAnalysis;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.WeDataQueryBindListResponse.Types;

namespace ServiceEngineMasaCore.Blazor.Pages.App.SystemManagement
{
    public partial class AccountManagement : IDisposable
    {
        PEnqueuedSnackbars? _enqueuedSnackbars;

        [Inject]
        [NotNull]
        IPopupService? _popupService { get; set; }

        [Inject]
        [NotNull]
        ISysOrgService? _sysOrgService { get; set; }  
        
        [Inject]
        [NotNull]
        ISysUserService? _sysUserServices { get; set; }

        [Inject]
        [NotNull]
        ISysPosService? _sysPosService { get; set; }

        [Inject]
        [NotNull]
        ISysRoleService? _sysRoleService { get; set; }

        List<SysOrg> _sysOrgList = new List<SysOrg>();
        List<SysUser> _sysUserList = new List<SysUser>();
        UpdateUserInput _userInput = new UpdateUserInput();
        PageInputDto input = new PageInputDto();

        List<long> _active = new List<long>();

        int _tatolCount = 0;
        int _tatolPage = 1;
        int _currentPage = 1;
        long _OrgId = -1;
        private string _paginationSelect = "10";

        private string? _account { get; set; }
        private string? _phone { get; set; }
        private bool _status { get; set; }
        private bool _dialog { get; set; }
        private string? _dialogTitle { get; set; }
     
        private EventCallback<bool> SwitchToggledCallback;
        private StringNumber _tab = 0;

        private bool _TimeMenu1;
        private bool _TimeMenu2;
        private DateOnly _Date { get; set; }

        private List<RoleOutput> _roleData = new List<RoleOutput>();
        List<SysPos> _posData = new List<SysPos>();
        List<CardTypeEnum> _cardTypeOptions = new List<CardTypeEnum>();
        List<CultureLevelEnum> _cultureLevelOptions = new List<CultureLevelEnum>();

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
            new() { Text = "操作", Value = "Action", Sortable = false, Align=DataTableHeaderAlign.Center }
        };

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _GlobalConfig.NavigationStyleChanged += NavigationStyleChanged;
                _popupService.ShowProgressLinear();
                foreach (CardTypeEnum cardType in Enum.GetValues(typeof(CardTypeEnum)))
                {
                    _cardTypeOptions.Add(cardType);
                }
                foreach (CultureLevelEnum cultureLevel in Enum.GetValues(typeof(CultureLevelEnum)))
                {
                    _cultureLevelOptions.Add(cultureLevel);
                }
                _userInput.RoleIdList = new List<long>();
                _userInput.ExtOrgIdList = new List<SysUserExtOrg>();
                SwitchToggledCallback = EventCallback.Factory.Create<bool>(this, OnSwitchChange);
                var res = await _sysOrgService.GetSysOrgList(0, "", "", "");
                if (res != null && res.Result != null)
                {
                    _sysOrgList = res.Result;
                }
               
                await LoadData();
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
            _popupService.HideProgressLinear();
        }

        private async Task LoadData() {
            input = new PageInputDto()
            {
                Account = _account ?? string.Empty,
                Phone = _phone ?? string.Empty,
                Page = _currentPage,
                PageSize = int.Parse(_paginationSelect),
                OrgId = _OrgId,
            };
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
            await LoadData();
        }
        private async Task OnSelectValueChange(string value)
        {
            _paginationSelect = value;
            _currentPage = 1;
            await LoadData();
        }
        private async Task OpenSwitchOrg(long orgId) {
            _OrgId = orgId;
            await LoadData();
        }
        private void OnSwitchChange(bool value) {
            _status = value;
        }
        private async Task OnSwitchToggled(long Id) {
            var input = new UInput
            {
                Id = Id,
                Status = _status ? StatusEnum.Enable:StatusEnum.Disable
            };
            var res = await  _sysUserServices.SetSysUserStatusAsync(input);
            if (res != null && res.Result > 0) {
                Enqueue(true, "状态更新成功");
            }
            else 
                Enqueue(false,"状态更新失败" );
        }
        private async Task AddUserOnClick()
        {
            _tab = 0;
            _userInput = new();
            _userInput.RoleIdList = new List<long>();
            _userInput.ExtOrgIdList = new List<SysUserExtOrg>();
            _dialogTitle = "添加账号";
            Task[] tasks = new Task[] {
                _sysRoleService.GetSysRoleListAsync(),
                _sysPosService.GetSysPosListAsync(new PosInput())
            };

            await Task.WhenAll(tasks);

            var res = await (Task<AdminResult<List<RoleOutput>>>)tasks[0];
            if (res != null)
            {
                _roleData = res.Result;
            }
            var res1 = await (Task<AdminResult<List<SysPos>>>)tasks[1];
            if (res1 != null)
            {
                _posData = res1.Result;
            }
            _dialog = true;
        }
        private async void EditUserOnClick(SysUser sysUser) {
            _tab = 0;
            var str = JsonConvert.SerializeObject(sysUser);
            var userInput = JsonConvert.DeserializeObject<UpdateUserInput>(str);
            _userInput = userInput ?? new UpdateUserInput();
            _userInput.RoleIdList = new List<long>();
            _userInput.ExtOrgIdList = new List<SysUserExtOrg>();
            _dialogTitle = "编辑账号";
            _dialog = true;
            Task[] tasks = new Task[] {
                _sysUserServices.GetSysUserRoleListAsync(sysUser.Id),
                _sysRoleService.GetSysRoleListAsync(),
                _sysPosService.GetSysPosListAsync(new PosInput()),
                _sysUserServices.GetSysUserExtRoleListAsync(sysUser.Id)
            };

            await Task.WhenAll(tasks);

            var res = await (Task<AdminResult<List<long>>>)tasks[0];
            if (res != null) {
                _userInput.RoleIdList = res.Result;
            }
            var res1 = await (Task<AdminResult<List<RoleOutput>>>)tasks[1];
            if (res1 != null) {
                _roleData = res1.Result;
            }
            var res2 = await (Task<AdminResult<List<SysPos>>>)tasks[2];
            if (res2 != null) {
                _posData = res2.Result;
            }
            var res3 = await (Task<AdminResult<List<SysUserExtOrg>>>)tasks[3];
            if(res3 != null)
            {
                _userInput.ExtOrgIdList = res3.Result;
            }
        }
        private async Task DltUserOnClick(string account,long id,long orgId)
        {
            var confirmed = await _popupService.ConfirmAsync(param =>
            {
                param.Title = "提示";
                param.Content = $"是否删除账号【{account}】?";
                param.OkText = @T("Confirm");
                param.CancelText = @T("Cancel");
            });
            if (confirmed) {
                DeleteInput input = new DeleteInput()
                {
                    Id = id,
                    OrgId = orgId
                };
                var res = await _sysUserServices.DeleteSysUserAsync(input);
                if (res != null && res.Code == 200)
                {
                    Enqueue(true, "删除成功");
                    var dltItem = _sysUserList.Where(i => i.Id == id && i.OrgId == orgId)?.FirstOrDefault();
                    if (dltItem != null)
                    {
                        _sysUserList.Remove(dltItem);
                    }
                }
                else
                {
                    Enqueue(true, "删除失败");
                }
            }
        }
        private async Task ResetPwdOnClick(string account, long id) {
            var confirmed = await _popupService.ConfirmAsync(param =>
            {
                param.Title = "提示";
                param.Content = $"是否重置账号【{account}】的密码?";
                param.OkText = @T("Confirm");
                param.CancelText = @T("Cancel");
            });
            if(confirmed)
            {
                var input = new ResetPwdInput()
                {
                    Id = id
                };
                var res = await _sysUserServices.ResetPwdAsync(input);
                if (res != null && res.Code == 200)
                {
                    Enqueue(true, $"密码重置为：123456");
                }
                else
                {
                    Enqueue(true, "密码重置失败");
                }
            }
        }
        private async Task AddAccountSubmit() {

            if (_userInput?.Id != null && _userInput.Id > 0)
            {
                var res = await _sysUserServices.UpdateSysUserAsync(_userInput);
                if (res != null && res.Code == 200)
                {
                    for (int i = 0; i < _sysUserList.Count; i++)
                    {
                        if (_sysUserList[i].Id == _userInput.Id)
                        {
                            _sysUserList[i] = _userInput;
                            break;
                        }
                    }
                    Enqueue(true,"用户账号更新成功");
                }
                else {
                    Enqueue(false,"用户账号更新失败");
                }
            }
            else {
                if (_userInput != null) { 
                    AdminResult<object>? res = await _sysUserServices.AddSysUserAsync(_userInput);
                    if (res != null && res.Code == 200)
                    {
                        _sysUserList.Add(_userInput);
                        Enqueue(true, "用户账号添加成功");
                    }
                    else {
                        Enqueue(false, "用户账号添加失败");
                    }
                }
            }
            StateHasChanged();
            _dialog = false;
        }
        private void OpenAddDialog(long parentId, string parentName)
        {
            
        }
        private void AddExtOrgListOnClick() {
            _userInput.ExtOrgIdList.Add(new ());
        }
        private void DltExtOrgListOnClick(SysUserExtOrg sysUserExtOrg) {
            _userInput.ExtOrgIdList.Remove(sysUserExtOrg);
        }
        private void ResetOnClick() {
            _account = null;
            _phone = null;
        }
       
        private async Task QueryOnClick() {
            _currentPage = 1;
            await LoadData();
        }
        private void SetBirthdayDateTime()
        {
            _TimeMenu1 = false;
            _userInput.Birthday = Convert.ToDateTime(_Date.ToString("yyyy-MM-dd"));
        }
        private void SetJoinDateTime()
        {
            _TimeMenu2 = false;
            _userInput.JoinDate = Convert.ToDateTime(_Date.ToString("yyyy-MM-dd"));
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
            _sysOrgList.Clear();
            _sysUserList.Clear();
        }
    }
}
