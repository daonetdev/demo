using BootstrapBlazor.Components;
using BootstrapBlazorApp1.Server.Data;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace BootstrapBlazorApp1.Server.Components.Pages
{
    /// <summary>
    /// CustomWinBoxContent 组件
    /// </summary>
    public partial class CustomWinBox
    {
        [Inject, NotNull]
        private WinBoxService? WinBoxService { get; set; }

        /// <summary>
        /// WinBoxOption 实例
        /// </summary>
        [Parameter, NotNull]
        public WinBoxOption? Option { get; set; }

        private Task StackWinBox() => WinBoxService.Stack();

        private async Task MinWinBox()
        {
            if (Option != null)
            {
                await WinBoxService.Minimize(Option);
            }
        }

        private async Task MaxWinBox()
        {
            if (Option != null)
            {
                await WinBoxService.Maximize(Option);
            }
        }

        private async Task RestoreWinBox()
        {
            if (Option != null)
            {
                await WinBoxService.Restore(Option);
            }
        }

        private async Task SetIconWinBox()
        {
            if (Option != null)
            {
                Option.Icon = $"{WebsiteOption.CurrentValue.AssetRootPath}images/Argo-C.png";
                await WinBoxService.SetIcon(Option);
            }
        }

        private async Task SetTitleWinBox()
        {
            if (Option != null)
            {
                Option.Title = $"{DateTime.Now}";
                await WinBoxService.SetTitle(Option);
            }
        }
    }

}
