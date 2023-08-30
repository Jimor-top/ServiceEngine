using ServiceEngine.Core;

namespace ServiceEngineMasaCore.Blazor.Pages.Components
{
    public partial class MSysMenuDataTable
    {
        [Parameter]
        public List<SysMenu> HItems { get; set; }

        [Parameter]
        public List<DataTableHeader<SysMenu>> Hheaders { get; set; }
        private bool _singleExpand;
    }
}
