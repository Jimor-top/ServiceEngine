using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.Service.File.Dto;
using ServiceEngineMasaCore.Blazor.Service.File.Interface;
using ServiceEngineMasaCore.Blazor.Service.Job.Dto;
using ServiceEngineMasaCore.Blazor.Service.Log.Dto;
using System.Diagnostics.CodeAnalysis;

namespace ServiceEngineMasaCore.Blazor.Pages.App.PlatformManagement
{
    public partial class FileManagement : IDisposable
    {
        [Inject]
        [NotNull]
        ISysFileService? _sysFileService { get; set; }

        List<SysFile> _sysFileList = new List<SysFile>();
        PFileInput input = new PFileInput();

        int _tatolCount = 0;
        int _tatolPage = 1;
        int _currentPage = 1;
        private string _paginationSelect = "10";

        bool _isLoading = false;
        readonly List<DataTableHeader<SysFile>> _headers = new List<DataTableHeader<SysFile>>()
        {
                new() { Text = "序号", Value = nameof(SysFile.Index) },
            new() { Text = "名称", Value = nameof(SysFile.FileName) },
            new() { Text = "后缀", Value = nameof(SysFile.Suffix) },
            new() { Text = "大小kb", Value = nameof(SysFile.SizeKb) },
            new() { Text = "预览", Value = nameof(SysFile.Url) },
            new() { Text = "存储位置", Value = nameof(SysFile.BucketName) },
            new() { Text = "存储标识", Value = nameof(SysFile.Id)},
            new() { Text = "创建时间", Value = nameof(SysFile.CreateTime) },
        };
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _GlobalConfig.NavigationStyleChanged += NavigationStyleChanged;
                _isLoading = true;

                input = new PFileInput() {
                    Page = 1,
                    PageSize = int.Parse(_paginationSelect),
                };
                await LoadData(input);

                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task LoadData(PFileInput input) {
            var res = await _sysFileService.GetSysFilePageAsync(input);
            if (res != null && res.Result?.Items != null)
            {
                _tatolPage = res.Result.TotalPages == 0 ? 1 : res.Result.TotalPages;
                _tatolCount = res.Result.Total;
                _sysFileList = res.Result.Items.ToList();
                _sysFileList = _sysFileList.Select((item, index) => {
                    item.Index = (input.Page - 1) * input.PageSize + index + 1;
                    return item;
                }).ToList();
            }
            _isLoading = false;
        }
        private async Task OnPaginationValueChange(int value)
        {
            _currentPage = value;
            input = new PFileInput()
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
            input = new PFileInput()
            {
                Page = 1,
                PageSize = int.Parse(value),
            };
            await LoadData(input);
        }
        private void NavigationStyleChanged(object? sender, EventArgs e)
        {
            InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            _sysFileList.Clear();
        }
    }
}
