using ServiceEngine.Core;

namespace ServiceEngineMasaCore.Blazor.Pages.Components
{
    public partial class MSysOrgDataTable
    {
        [Parameter]
        public bool MyExpand { get; set; }

        [Parameter]
        public List<SysOrg> HItems { get; set; }

        [Parameter]
        public List<DataTableHeader<SysOrg>> Hheaders { get; set; }

        [Parameter]
        public List<SysDictData> DictDatas { get; set; }

        [Parameter]
        public EventCallback<SysOrg> EditOClick { get; set; }

        // 调用EditOClick事件，并将context.Item作为参数传递给该事件
        private async Task HandleEditOrgOnClick(SysOrg org)
        {
            await EditOClick.InvokeAsync(org);
        }


        [Parameter]
        public EventCallback<SysOrg> DltOnClick { get; set; }

        // 调用EditOClick事件，并将context.Item作为参数传递给该事件
        private async Task HandleDltOrgOnClick(SysOrg org)
        {
            await DltOnClick.InvokeAsync(org);
        }
    }
}
