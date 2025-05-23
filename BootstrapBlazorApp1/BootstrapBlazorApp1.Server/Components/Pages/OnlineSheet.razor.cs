using BootstrapBlazor.Components;
using BootstrapBlazorApp1.Server.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System.Diagnostics.CodeAnalysis;

namespace BootstrapBlazorApp1.Server.Components.Pages
{

   
    /// <summary>
    /// Online sheet sample code
    /// </summary>
    public partial class OnlineSheet : IDisposable
    {
        [Inject, NotNull]
        private IWebHostEnvironment? WebHost { get; set; }

        [Inject, NotNull]
        private ToastService? ToastService { get; set; }

        [Inject, NotNull]
        private IStringLocalizer<OnlineSheet>? Localizer { get; set; }

        [Inject]
        [NotNull]
        private IDispatchService<Contributor>? DispatchService { get; set; }

        [NotNull]
        private UniverSheet? _sheetExcel = null;

        private UniverSheetData? _data = null;

        private bool _inited = false;

        private string? _jsonData = null;

        private static string? _reportData = null;


        [NotNull]
        private UniverSheet? _sheetPlugin = null;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            var reportFile = Path.Combine(WebHost.WebRootPath, "univer-sheet", "report.json");
            if (File.Exists(reportFile))
            {
                var sheetData = File.ReadAllText(reportFile);

                _data = new UniverSheetData()
                {
                    WorkbookData = sheetData
                };
            }
        }

        private async Task OnReadyAsync()
        {
            _inited = true;
            await ToastService.Information(Localizer["ToastOnReadyTitle"], Localizer["ToastOnReadyContent"]);
            DispatchService.Subscribe(Dispatch);
        }

        private async Task Dispatch(DispatchEntry<Contributor> entry)
        {
            if (!_inited)
            {
                return;
            }

            if (entry.Entry != null)
            {
                await ToastService.Show(new ToastOption()
                {
                    Title = "Dispatch 服务测试",
                    ChildContent = BootstrapDynamicComponent.CreateComponent<OnlineContributor>(new Dictionary<string, object?>()
                {
                    { "Contributor", entry.Entry }
                }).Render(),
                    Category = ToastCategory.Information,
                    Delay = 3000,
                    ForceDelay = true
                });

                await _sheetExcel.PushDataAsync(entry.Entry.Data);
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                DispatchService.UnSubscribe(Dispatch);
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        private static Task<UniverSheetData> OnPostDataAsync(UniverSheetData data)
        {
            // 这里可以根据 data 的内容进行处理然后返回处理后的数据
            // 本例返回与时间相关的数据
            var result = new UniverSheetData()
            {
                MessageName = data.MessageName,
                CommandName = data.CommandName,
                Data = new
                {
                    key = "datetime",
                    Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                }
            };
            return Task.FromResult(result);
        }

        private async Task OnPushExcelData()
        {
            await _sheetExcel.PushDataAsync(new UniverSheetData()
            {
                CommandName = "SetWorkbook",
                WorkbookData = _reportData
            });
        }

        private async Task OnSaveExcelData()
        {
            var result = await _sheetExcel.PushDataAsync(new UniverSheetData()
            {
                CommandName = "GetWorkbook"
            });
            _jsonData = result?.Data?.ToString();
            StateHasChanged();
        }

        private async Task OnPushPluginData()
        {
            await _sheetPlugin.PushDataAsync(new UniverSheetData()
            {
                Data = new
                {
                    Id = "1",
                    Name = "Test",
                    Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                }
            });
        }
    }
}
