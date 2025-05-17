using BootstrapBlazor.Maui1.Services;
using BootstrapBlazor.Maui1.Shared.Services;
using Microsoft.Extensions.Logging;

namespace BootstrapBlazor.Maui1
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            // Add device-specific services used by the BootstrapBlazor.Maui1.Shared project
            builder.Services.AddSingleton<IFormFactor, FormFactor>();

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddHttpClient();

            builder.Services.AddBootstrapBlazor(options =>
            {
                options.DefaultCultureInfo = "zh-CN";
            });

            // 增加 Pdf 导出服务
            builder.Services.AddBootstrapBlazorTableExportService();

            // 增加 Html2Pdf 服务
            builder.Services.AddBootstrapBlazorHtml2PdfService();

            return builder.Build();
        }
    }
}
