using BlazorComponent;
using FluentValidation;
using Masa.Blazor;
using Masa.Blazor.Presets;
using Microsoft.JSInterop;
using Nest;
using Newtonsoft.Json;
using Qiniu.CDN;
using ServiceEngine.Core;
using ServiceEngine.Core.Service;
using ServiceEngineMasaCore.Blazor.Service.Dict.Interface;
using ServiceEngineMasaCore.Blazor.Service.Org.Dto;
using ServiceEngineMasaCore.Blazor.Service.Org.Interface;
using ServiceEngineMasaCore.Blazor.Service.UserInfo.Dto;
using ServiceEngineMasaCore.Blazor.Service.UserInfo.Interface;
using System.Diagnostics.CodeAnalysis;

namespace ServiceEngineMasaCore.Blazor.Pages.App.SystemManagement
{
    public partial class OrganizationalManagement
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
        ISysDictDataService? _sysDictDataService { get; set; }

        List<SysOrg> _sysOrgList = new List<SysOrg>();
        List<SysOrg> _sysOrgListCascader = new List<SysOrg>();

        List<long> _active = new List<long>();
        string? _name { get; set; }
        string? _code { get; set; }
        string? _type { get; set; }
        long _orgId = -1;

        UpdateOrgInput _updateOrgInput { get; set; } = new UpdateOrgInput();
        List<SysDictData> _sysDictDatas = new List<SysDictData>();

        private bool _singleExpand;

        private bool _dialog { get; set; }
        private string _dialogTitle { get; set; } = string.Empty;
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
                await Task.Run(() =>{
                    var res =  _sysDictDataService.GetSysDictDataListByCodeAsync("org_type");
                    if (res != null && res.Result.Code == 200)
                    {
                        _sysDictDatas = res.Result.Result ?? new List<SysDictData>();
                    }
                });
                await LoadData();
                _popupService.HideProgressLinear();
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }
        private async Task LoadData() {
            var res = await _sysOrgService.GetSysOrgListAsync(_orgId, _name, _code, _type);
            if (res != null && res.Result != null)
            {
                _sysOrgListCascader = _sysOrgList = res.Result;
                ChackNull(_sysOrgList);
            }
        }
        private void ChackNull(List<SysOrg> list) {
            list.ForEach(org =>
            {
                if (org != null && org.Children != null) {
                    if (org.Children.Count == 0)
                    {
                        org.Children = null;
                    }
                    else
                    {
                        ChackNull(org.Children);
                    }
                }
               
            });
        }

        private void NavigationStyleChanged(object? sender, EventArgs e)
        {
            InvokeAsync(StateHasChanged);
        }
        private async Task OpenSwitchOrg(SysOrg org)
        {
            _orgId = org.Id;
            await LoadData();
            StateHasChanged();
        }
        private void ResetOnClick()
        {
            _name = null;
            _code = null;
            _type = null;
        }

        private async Task QueryOnClick()
        {
            await LoadData();
        }
        private void AddOrgOnClick()
        {
            _dialogTitle = "增加机构";
            _updateOrgInput = new UpdateOrgInput();
            _dialog = true;
        }
        private void EditOrgOnClick(SysOrg org) {
            _dialogTitle = "编辑机构";
            string str = JsonConvert.SerializeObject(org);
            var userInput = JsonConvert.DeserializeObject<UpdateOrgInput>(str);
            _updateOrgInput = userInput ?? new UpdateOrgInput();
            _dialog = true;
        }
        private async Task DltOrgOnClick(SysOrg org) {
            var confirmed = await _popupService.ConfirmAsync(param =>
            {
                param.Title = "提示";
                param.Content = $"是否删除机构【{org.Name}】?";
                param.OkText = @T("Confirm");
                param.CancelText = @T("Cancel");
            });
            if (confirmed)
            {
                DltOrgInput dltOrgInput = new DltOrgInput()
                {
                    Id = org.Id
                };
                var res = await _sysOrgService.DeleteSysOrgAsync(dltOrgInput);
                if (res != null && res.Code == 200)
                {
                    Enqueue(true, "删除成功");
                    await LoadData();
                }
                else
                {
                    Enqueue(true, "删除失败");
                }
            }
        }
        private async Task SubmitOnClick() {
            if (_dialogTitle.Equals("增加机构"))
            {
                var res = await _sysOrgService.AddSysOrgAsync(_updateOrgInput);
                if (res != null && res.Code == 200)
                {
                    Enqueue(true, "机构添加成功");
                    await LoadData();
                }
                else
                    Enqueue(false, "机构添加失败");
                
            }
            else {
                var res = await _sysOrgService.UpdateSysOrgAsync(_updateOrgInput);
                if (res != null && res.Code == 200)
                {
                    Enqueue(true, "机构修改成功");
                    UpdateOrgListItem(_sysOrgList);
                }
                else
                    Enqueue(false, "机构修改失败");
            }
            StateHasChanged();
            _dialog = false;
        }
        private void UpdateOrgListItem(List<SysOrg> list) {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Id == _updateOrgInput.Id)
                {
                    list[i] = _updateOrgInput;
                    break;
                }
                else {
                    if (list[i].Children !=null && list[i].Children.Count > 0) { 
                        UpdateOrgListItem(list[i].Children);
                    }
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
        bool jsLoaded = false;
        int count = 0;
        private async Task Click() {
            if (!jsLoaded) {
                jsLoaded = true;
                await Task.Delay(100);
                await JSRuntime.InvokeVoidAsync("addMenuOptionClickHandlers");
            }
        }
        public void Dispose()
        {
            _sysOrgList.Clear();
        }
    }
}
