using BootstrapBlazor.Components;

namespace BootstrapBlazorApp1.Server.Components.Pages
{
    public partial class PrintChartDialog : IResultDialog
    {

        private readonly Random _randomer = new();

        private int _lineDataCount = 7;

        private int _lineDatasetCount = 2;

        private async Task<ChartDataSource> OnInitTension(float tension, bool hasNull)
        {
            var ds = new ChartDataSource();
            ds.Options.Title = "Line Chart";
            ds.Options.X.Title = "days";
            ds.Options.Y.Title = "Numerical value";
            ds.Labels = Enumerable.Range(1, _lineDataCount).Select(i => i.ToString());
            for (var index = 0; index < _lineDatasetCount; index++)
            {
                ds.Data.Add(new ChartDataset()
                {
                    Tension = tension,
                    Label = $"Set {index}",
                    Data = Enumerable.Range(1, _lineDataCount).Select((i, index) => (index == 2 && hasNull) ? null! : (object)_randomer.Next(20, 37))
                });
            }

            // 模拟异步
            await Task.Delay(100);
            return ds;
        }

        public async Task OnClose(DialogResult result)
        {

        }
    }
}
