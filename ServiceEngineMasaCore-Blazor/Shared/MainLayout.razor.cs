using Microsoft.JSInterop;
using ServiceEngine.Core.Service;
using ServiceEngineMasaCore.Blazor.Service.Menu.Interface;
using System.Diagnostics.CodeAnalysis;

namespace ServiceEngineMasaCore.Blazor.Shared
{
    public partial class MainLayout
    {
        [Inject]
        IJSRuntime jsRuntime { get; set; }

        private static readonly string[] s_selfPatterns =
        {
            "/app/todo"
        };

        private bool? _showSetting;

        private string? _pageTab;

        private PageTabs? _pageTabs;

        private string PageModeClass => _pageTab == PageModes.PageTab ? "page-mode--tab" : "page-mode--breadcrumb";
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            if (firstRender)
            {
                await GlobalConfig.InitFromStorage();
                //await jsRuntime.InvokeVoidAsync("eval", "function toggleFullScreen() { /* Your toggleFullScreen JavaScript code here */ }");
            }
        }
        void OnLanguageChanged(CultureInfo culture)
        {
            GlobalConfig.Culture = culture;
        }
        private async Task ToggleFullscreen()
        {
            await jsRuntime.InvokeVoidAsync("toggleFullScreen");
        }
    }
}
