using Furion.JsonSerialization;
using Masa.Blazor;
using Nest;
using NewLife.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.Service.Cache.Interface;
using System.Diagnostics.CodeAnalysis;
using static SKIT.FlurlHttpClient.Wechat.Api.Models.WxaBusinessGetUserEncryptKeyResponse.Types;

namespace ServiceEngineMasaCore.Blazor.Pages.App.PlatformManagement
{
    public partial class CacheManagement : IDisposable
    {
        [Inject]
        [NotNull]
        IPopupService? _popupService { get; set; }

        [Inject]
        [NotNull]
        ISysCacheService? _sysCacheService { get; set; }

        List<CacheData> _sysCacheList = new List<CacheData>();

        private object Options = new
        {
            value = "",
            language = "json",
            lineNumbers = "on",
            theme = "vs",
            automaticLayout = true,
            scrollBeyondLastLine = false,
            readOnly = true,
            wordWrap = "wordWrapColumn",
            wordWrapColumn = 40,
            wrappingIndent = "indent",
        };
        private MMonacoEditor _editor;
        List<string> _active = new List<string>();

        string cacheTitle = "";
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _GlobalConfig.NavigationStyleChanged += NavigationStyleChanged;
                _popupService.ShowProgressLinear();
                var res = await _sysCacheService.GetSysCachePageAsync();
                if (res != null) {
                    foreach (var item in res.Result)
                    {
                        string[] keyNames = item.Split(':');
                        string pName = keyNames[0];

                        if (_sysCacheList.FirstOrDefault(x => x.Name == pName) == null)
                        {
                            _sysCacheList.Add(new CacheData()
                            {
                                Id = pName,
                                Name = pName,
                                Children = null
                            });
                        }
                        if (keyNames.Length > 1)
                        {
                            var pNode = _sysCacheList.FirstOrDefault(x => x.Name == pName);
                            if (pNode != null)
                            {
                                if (pNode.Children == null) pNode.Children = new List<CacheData>();
                                pNode.Children.Add(new CacheData()
                                {
                                    Id = item,
                                    Name = item.Substring(pName.Length + 1)
                                });
                            }
                        }
                        //_sysCacheList.Add(cacheData);
                    }
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
        public async Task FetchCache(string Id)
        {
            cacheTitle = string.Format("缓存数据-{0}", Id);
            var res = await _sysCacheService.GetSysCacheValueAsync(Id);
            if (res != null && res.Result != null)
            {
                _editor.Value = res.Result.ToString();
                StateHasChanged();
            }
        }
      
        private void OpenAddDialog(long parentId, string parentName)
        {

        }

        public void Dispose()
        {
            _sysCacheList.Clear();
        }
    }
    public class CacheData { 
        public string Id { get; set; }
        public string? Name { get; set; }
        public List<CacheData>? Children { get; set; }
    }
}
