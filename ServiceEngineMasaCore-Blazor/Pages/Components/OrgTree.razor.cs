using Nest;
using Qiniu.CDN;
using ServiceEngine.Core;
using ServiceEngineMasaCore.Blazor.Global;
using ServiceEngineMasaCore.Blazor.Service.Org.Interface;
using System.Diagnostics.CodeAnalysis;

namespace ServiceEngineMasaCore.Blazor.Pages.Components
{
    public partial class OrgTree
    {
        [Inject]
        [NotNull]
        ISysOrgService? _sysOrgService { get; set; }

        [Parameter]
        public List<SysOrg> SysOrgList{ get; set; } = new List<SysOrg>();

        [Parameter]
        public Func<SysOrg,Task> OpenSwitchOrgEvent { get; set; } = null;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadData();
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }
        private async Task LoadData() {
            var res = await _sysOrgService.GetSysOrgListAsync(0, null, null, null);
            if (res != null && res.Result != null)
            {
                SysOrgList = res.Result;
            }
        }
        private async Task NodeRefresh() {
            await LoadData();
        }
        private void RootClick(SysOrg org) {
            org.Id = 0;
            OpenSwitchOrgEvent.Invoke(org);
        }
    }
}
