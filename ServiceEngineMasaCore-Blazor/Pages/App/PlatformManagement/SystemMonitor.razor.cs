using Masa.Blazor.Presets;
using Newtonsoft.Json;
using ServiceEngineMasaCore.Blazor.Service.Server.Interface;
using System.Diagnostics.CodeAnalysis;

namespace ServiceEngineMasaCore.Blazor.Pages.App.PlatformManagement
{
    public partial class SystemMonitor : IDisposable
    {
        [Inject]
        [NotNull]
        ISysServerService? _sysServerService { get; set; }
        dynamic? _diskInfo;
        dynamic? _baseInfo;
        dynamic? _usedInfo;
        dynamic? _assemblyList;
        private object? _MemoryGaugeData;
        private object? _CpuGaugeData;
        List<object?> dickGaugeDataList = new List<object?>();
        IEnumerable<BlockTextTag> _tags = new List<BlockTextTag>();
        Timer _timer;
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _GlobalConfig.NavigationStyleChanged += NavigationStyleChanged;
                await LoadServerDiskAsync();
                await LoadServerBaseAsync();
                await LoadServerUsedAsync();
                await LoadServerAssemblyListAsync();
                _timer = new Timer(async (object? state) => { await LoadServerUsedAsync(); InvokeAsync(StateHasChanged); }, null, Timeout.Infinite, Timeout.Infinite);
                _timer.Change(5000, 10000);
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }
        async Task LoadServerDiskAsync() {
            var diskInfoRes = await _sysServerService.GetSysServerDiskAsync();
            if (diskInfoRes != null)
                _diskInfo = JsonConvert.DeserializeObject<dynamic>(Convert.ToString(diskInfoRes.Result));
            if (_diskInfo != null)
            {
                foreach (var item in _diskInfo)
                {
                    int availablePercent = (int)item.availablePercent;
                    var dickGaugeData = new[] {
                            new{
                                Value = availablePercent,
                                Name = $"已用:{item.used}\n"+
                                $"剩余:{item.availableFreeSpace}\n"+
                                $"{item.diskName}"
                            }
                        };
                    dickGaugeDataList.Add(dickGaugeData);
                }
            }
        }
        async Task LoadServerBaseAsync() {
            var baseInfoRes = await _sysServerService.GetSysServerBaseAsync();
            if (baseInfoRes != null)
                _baseInfo = JsonConvert.DeserializeObject<dynamic>(Convert.ToString(baseInfoRes.Result));
        }

        async Task LoadServerUsedAsync() {
            var usedInfoRes = await _sysServerService.GetSysServerUsedAsync();
            if (usedInfoRes != null)
                _usedInfo = JsonConvert.DeserializeObject<dynamic>(Convert.ToString(usedInfoRes.Result));
            string memoryData = "0";
            if (_usedInfo?.ramRate != null)
            {
                memoryData = ((string)_usedInfo?.ramRate)[0..^1];
            }
            _MemoryGaugeData = new[] {
                    new{
                        Value = memoryData,
                        Name = $"已用:{_usedInfo?.usedRam}\n" +
                        $"剩余:{_usedInfo?.freeRam}\n"+"内存使用率"
                    }
                };
            string cpuData = "0";
            if (_usedInfo?.cpuRate != null)
            {
                cpuData = ((string)_usedInfo?.cpuRate)[0..^1];
            }

            _CpuGaugeData = new[]
            {
                    new{
                        Value = cpuData,
                        Name = "CPU使用率"
                    }
            };
        }
        async Task LoadServerAssemblyListAsync() {
            var assemblyListRes = await _sysServerService.GetSysServerAssemblyListAsync();
            if (assemblyListRes != null)
                _assemblyList = JsonConvert.DeserializeObject<dynamic>(Convert.ToString(assemblyListRes.Result));

            List<BlockTextTag> tagList = _tags.ToList();
            foreach (var item in _assemblyList)
            {
                tagList.Add(new BlockTextTag($"{item.name}\t{item.version}", "orange", true));
                _tags = tagList.AsEnumerable();
            }
        }
        private void NavigationStyleChanged(object? sender, EventArgs e)
        {
            InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}
