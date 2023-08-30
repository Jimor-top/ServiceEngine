using Microsoft.JSInterop;
using ServiceEngine.Core.Service;
using ServiceEngineMasaCore.Blazor.Service.Menu.Interface;
using System.Diagnostics.CodeAnalysis;

namespace ServiceEngineMasaCore.Blazor.Shared
{
    public partial class MainLayout
    {
        private static readonly string[] s_selfPatterns =
        {
            "/app/todo"
        };

        private bool? _showSetting;

        private string? _pageTab;

        private PageTabs? _pageTabs;

        private string PageModeClass => _pageTab == PageModes.PageTab ? "page-mode--tab" : "page-mode--breadcrumb";

        void OnLanguageChanged(CultureInfo culture)
        {
            I18n.SetCulture(culture);
        }
        private async Task ToggleFullscreen()
        {
            var element = await jsRuntime.InvokeAsync<ElementReference>("document.querySelector", "#fullscreenDiv");
            await jsRuntime.InvokeVoidAsync("MasaBlazor.requestFullscreen", element);
        }
    }
}
