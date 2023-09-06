using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace ServiceEngineMasaCore.Blazor.Pages.Components
{
    public partial class MInputNumber
    {
        private int _value;
        [Parameter]
        public int Value
        {
            get => _value;
            set
            {
                this._value = value;
                Console.WriteLine(_value);
                StateHasChanged(); // 更新组件状态
            }
        }

        private void Increment()
        {
            Value += 1;
        }

        private void Decrement()
        {
            Value -= 1;
        }
    }
}
