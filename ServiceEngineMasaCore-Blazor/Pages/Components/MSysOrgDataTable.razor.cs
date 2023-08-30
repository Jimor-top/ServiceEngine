using ServiceEngine.Core;

namespace ServiceEngineMasaCore.Blazor.Pages.Components
{
    public partial class MSysOrgDataTable
    {
        [Parameter]
        public List<SysOrg> HItems { get; set; }

        [Parameter]
        public List<DataTableHeader<SysOrg>> Hheaders { get; set; }
        private bool _singleExpand;
    }
}
