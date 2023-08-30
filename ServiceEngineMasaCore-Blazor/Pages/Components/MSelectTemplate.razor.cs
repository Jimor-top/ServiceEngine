using Newtonsoft.Json.Linq;

namespace ServiceEngineMasaCore.Blazor.Pages.Components
{
    public partial class MSelectTemplate
    {
        [Parameter]
        public string Value { get; set; }

        [Parameter]
        public EventCallback<string> ValueChanged { get; set; }

        List<SelectItem> _items = new()
        {
            new SelectItem("10条/页", "10"),
            new SelectItem("20条/页", "20"),
            new SelectItem("50条/页", "50"),
            new SelectItem("100条/页", "100"),
        };
        private async Task HandleValueChanged(ChangeEventArgs e)
        {
            Value = e.Value.ToString();
            await ValueChanged.InvokeAsync(Value);
        }
    }
    public class SelectItem
    {
        public string Label { get; set; }
        public string Value { get; set; }
        public SelectItem(string label, string value)
        {
            Label = label;
            Value = value;
        }
    }
}
