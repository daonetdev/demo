using BootstrapBlazor.Components;

namespace BootstrapBlazorApp1.Server.Components.Pages
{
    public partial class DropUploadPage
    {
        private bool _isMultiple = true;
        private bool _isDisabled = false;
        private bool _showProgress = true;
        private bool _showFooter = true;
        private bool _showUploadFileList = true;
        private bool _showDownloadButton = true;

        private Task OnDropUpload(UploadFile file)
        {
            // 模拟保存文件等处理
            if (file.File is { Size: > 5 * 1024 * 1024 })
            {
                file.Code = 1004;
                ToastService.Information("Error", "文件大小不超过 5Mb");
            }
            return Task.CompletedTask;
        }

    }
}
