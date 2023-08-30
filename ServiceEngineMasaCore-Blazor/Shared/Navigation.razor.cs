using Masa.Blazor;
using ServiceEngine.Core.Service;
using ServiceEngine.Core.Util;
using ServiceEngineMasaCore.Blazor.Service.Menu.Interface;
using ServiceEngineMasaCore.Blazor.Service.UserInfo.Interface;
using SqlSugar;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Mapster;
using ServiceEngineMasaCore.Blazor.Global.Nav.Model;
using FluentEmail.Core;
using ServiceEngine.Core;

namespace ServiceEngineMasaCore.Blazor.Shared
{
    public partial class Navigation
    {
        [Inject]
        public MasaBlazor Masa { get; set; } = default!;

        [Inject]
        [NotNull]
        IPopupService? PopupService { get; set; }


        [Inject]
        [NotNull]
        IMenuStore? _IMenuStore { get; set; }

        public bool? Visible { get; set; } = true;

        public string ComputedNavigationClass => (_GlobalConfig.NavigationStyle == NavigationStyles.Rounded ? "rounded-r-xl" : string.Empty);

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _GlobalConfig.NavigationStyleChanged += NavigationStyleChanged;

            List<MenuOutput>? menuList = _IMenuStore.GetMenu();
            List<NavModel> navList = new List<NavModel>();

            if (menuList != null) {

                navList.AddRange(ConvertMenuOutputToNavModel(menuList));
                NavHelper.Initialization(navList);
            }
        }
        public static List<NavModel> ConvertMenuOutputToNavModel(List<MenuOutput> menuOutputs)
        {
            List<NavModel> navModels = new List<NavModel>();

            foreach (var menuOutput in menuOutputs)
            {
                NavModel navModel = new NavModel();
                navModel.Id = (int)menuOutput.Id;
                navModel.Icon = menuOutput.Meta.Icon;
                navModel.Title = menuOutput.Name;
                if (menuOutput.Type != MenuTypeEnum.Dir) {
                    navModel.Href = menuOutput.Path;
                }
                if (menuOutput.Children != null && menuOutput.Children.Count > 0)
                {
                    navModel.Children = ConvertMenuOutputToNavModel(menuOutput.Children).ToArray();
                }
                navModels.Add(navModel);
            }

            return navModels;
        }
        private void NavigationStyleChanged(object? sender, EventArgs e)
        {
            InvokeAsync(StateHasChanged);
        }

        public void Switch()
        {
            if (Visible is true)
            {
                _GlobalConfig.ExpandOnHover = !_GlobalConfig.ExpandOnHover;
            }
            else
            {
                _GlobalConfig.ExpandOnHover = false;
                Visible = true;
            }
        }

        void IDisposable.Dispose()
        {
            _GlobalConfig.NavigationStyleChanged -= NavigationStyleChanged;
        }

    }
}
