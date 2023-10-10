using Masa.Blazor;
using Masa.Blazor.Presets;
using Newtonsoft.Json;
using ServiceEngine.Core;
using ServiceEngine.Core.Service;
using ServiceEngineMasaCore.Blazor.Service.Org.Dto;
using ServiceEngineMasaCore.Blazor.Service.Pos.Dto;
using ServiceEngineMasaCore.Blazor.Service.Pos.Interface;
using ServiceEngineMasaCore.Blazor.Service.Role.Dto;
using System.Diagnostics.CodeAnalysis;

namespace ServiceEngineMasaCore.Blazor.Pages.App.SystemManagement
{
    public partial class PositionManagement : IDisposable
    {
        PEnqueuedSnackbars? _enqueuedSnackbars;

        [Inject]
        [NotNull]
        IPopupService? _popupService { get; set; }

        [Inject]
        [NotNull]
        ISysPosService? _sysPosService { get; set; }

        List<SysPos> _sysPosList = new List<SysPos>();
        PosInput _input = new PosInput();
        UpdatePosInput _updateInput = new UpdatePosInput();
        string _name { get; set; } = string.Empty;
        string _code { get; set; } = string.Empty;
        private bool _dialog { get; set; }
        private string _dialogTitle { get; set; } = string.Empty;

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
            new() { Text = "操作", Value = "Action", Sortable = false ,Width="10%"}
        };
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _GlobalConfig.NavigationStyleChanged += NavigationStyleChanged;
                _isLoading = true;
                _popupService.ShowProgressLinear();
                await LoadData();
                _popupService.HideProgressLinear();
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task LoadData() {
            _input = new PosInput() { 
                Name = _name,
                Code = _code
            };
            var res = await _sysPosService.GetSysPosListAsync(_input);
            if (res != null && res.Result != null)
            {
                _sysPosList = res.Result;
                _sysPosList = _sysPosList.Select((item, index) =>
                {
                    item.Index = index + 1;
                    return item;
                }).ToList();
                _isLoading = false;
            }
            Console.WriteLine(_sysPosList.Count);
        }

        private void NavigationStyleChanged(object? sender, EventArgs e)
        {
            InvokeAsync(StateHasChanged);
        }
        private void ResetOnClick() {
            _name = string.Empty;
            _code = string.Empty;
        }
        private void AddPosOnClick() {
            _dialogTitle = "增加职位";
            _updateInput = new UpdatePosInput();
            _dialog = true;
        }
        private async Task QueryOnClick() {
            await LoadData();
        }
        private void EditPosOnClick(SysPos pos)
        {
            _dialogTitle = "编辑职位";
            string str = JsonConvert.SerializeObject(pos);
            var userInput = JsonConvert.DeserializeObject<UpdatePosInput>(str);
            _updateInput = userInput ?? new UpdatePosInput();
            _dialog = true;
        }
        private async Task DltPosOnClick(SysPos pos)
        {
            var confirmed = await _popupService.ConfirmAsync(param =>
            {
                param.Title = "提示";
                param.Content = $"是否删除职位【{pos.Name}】?";
                param.OkText = @T("Confirm");
                param.CancelText = @T("Cancel");
            });
            if (confirmed)
            {
                DltPosInput dltPosInput = new DltPosInput()
                {
                    Id = pos.Id
                };
                var res = await _sysPosService.DeleteSysPosAsync(dltPosInput);
                if (res != null && res.Code == 200)
                {
                    Enqueue(true, "删除成功");
                    var dltItem = _sysPosList.Where(i => i.Id == pos.Id)?.FirstOrDefault();
                    if (dltItem != null)
                    {
                        _sysPosList.Remove(dltItem);
                    }
                    await LoadData();
                }
                else
                {
                    Enqueue(true, "删除失败");
                }
            }
        }
        private async Task SubmitOnClick()
        {
            if (_dialogTitle.Equals("增加职位"))
            {
                var res = await _sysPosService.AddSysPosAsync(_updateInput);
                if (res != null && res.Code == 200)
                {
                    Enqueue(true, "职位添加成功");
                    _sysPosList.Add(_updateInput);
                }
                else
                    Enqueue(false, "职位添加失败");

            }
            else
            {
                var res = await _sysPosService.UpdateSysPosAsync(_updateInput);
                if (res != null && res.Code == 200)
                {
                    Enqueue(true, "职位修改成功");
                    for (int i = 0; i < _sysPosList.Count; i++)
                    {
                        if (_sysPosList[i].Id == _updateInput.Id)
                        {
                            _sysPosList[i] = _updateInput;
                            break;
                        }
                    }
                }
                else
                    Enqueue(false, "职位修改失败");
            }
            StateHasChanged();
            _dialog = false;
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
            _sysPosList.Clear();
        }
    }
}
