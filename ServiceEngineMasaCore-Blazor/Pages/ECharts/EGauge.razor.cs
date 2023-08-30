namespace ServiceEngineMasaCore.Blazor.Pages.ECharts
{
    public partial class EGauge
    {
        [Parameter]
        public object? GaugeData { get; set; }
        [Parameter]
        public int width { get; set; }
        [Parameter]
        public int height { get; set; }
       
        protected override void OnParametersSet()
        {
            // 在参数设置时将传递给组件的 GaugeData 赋值给 InternalGaugeData
            GaugeData = GenOption(GaugeData);

            base.OnParametersSet();
        }
        private object GenOption(object? GaugeData)
        {
            return new
            {
                Series = new[]
                {
                    new
                    {
                        Type = "gauge",
                        Progress = new
                        {
                            Show = true,
                            RoundCap = true,
                            Width = 8
                        },
                        YAxis = new{
                            AxisLine = new{
                                Show = true,
                                Width = 8
                            }
                        },
                        SplitLine=new{
                            Show = false
                        },
                        AxisLabel = new {
                            Show = false
                        },
                        AxisTick=new{
                            Show = false
                        },
                        Anchor = new{
                            Show = false,
                            ShowAbove = false
                        },
                        XAxis = new{
                            AxisLine = new{
                                Show = true,
                                Width = 180
                            }
                        },
                        Label = new
                        {
                            Show = false
                        },
                        pointer = new{
                            Show = false
                        },
                        Detail = new{
                            ValueAnimation = true,
                            FontSize = 16,
                            OffsetCenter = new object[]{ 0,"-30%"},
                            Formatter = "{value}%"
                        },
                        Data = GaugeData
                    }
            }
            };

        }
    }
}
