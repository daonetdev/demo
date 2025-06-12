using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace BootstrapBlazorApp1.Server.Components.Pages
{
    public partial class WinboxPage
    {
        [Inject, NotNull]
        private WinBoxService? WinBoxService { get; set; }

        private ConsoleLogger _logger = default!;

        private static WinBoxOption DefaultOptions => new()
        {
            Top = "50px",
            Class = "bb-win-box",
            Border = 2,
            Background = "var(--bb-primary-color)"
        };

        private Task OpenModalWinBox()
        {
            var option = DefaultOptions;
            option.Modal = true;
            return OpenWinBox(option);
        }

        private async Task OpenWinBox(WinBoxOption? option)
        {
            option ??= DefaultOptions;
            option.OnCloseAsync = () =>
            {
                _logger.Log($"WinBoxOption({option.Id}) Trigger OnCloseAsync");
                return Task.CompletedTask;
            };
            option.OnCreateAsync = () =>
            {
                _logger.Log($"WinBoxOption({option.Id}) Trigger OnCreateAsync");
                return Task.CompletedTask;
            };
            option.OnShowAsync = () =>
            {
                _logger.Log($"WinBoxOption({option.Id}) Trigger OnShownAsync");
                return Task.CompletedTask;
            };
            option.OnHideAsync = () =>
            {
                _logger.Log($"WinBoxOption({option.Id}) Trigger OnHideAsync");
                return Task.CompletedTask;
            };
            option.OnFocusAsync = () =>
            {
                _logger.Log($"WinBoxOption({option.Id}) Trigger OnFocusAsync");
                return Task.CompletedTask;
            };
            option.OnBlurAsync = () =>
            {
                _logger.Log($"WinBoxOption({option.Id}) Trigger OnBlurAsync");
                return Task.CompletedTask;
            };
            option.OnFullscreenAsync = () =>
            {
                _logger.Log($"WinBoxOption({option.Id}) Trigger OnFullscreenAsync");
                return Task.CompletedTask;
            };
            option.OnRestoreAsync = () =>
            {
                _logger.Log($"WinBoxOption({option.Id}) Trigger OnRestoreAsync");
                return Task.CompletedTask;
            };
            option.OnMaximizeAsync = () =>
            {
                _logger.Log($"WinBoxOption({option.Id}) Trigger OnMaximizeAsync");
                return Task.CompletedTask;
            };
            option.OnMinimizeAsync = () =>
            {
                _logger.Log($"WinBoxOption({option.Id}) Trigger OnMinimizeAsync");
                return Task.CompletedTask;
            };

            await WinBoxService.Show<CustomWinBox>("Test", new Dictionary<string, object?>() {
            { "Option", option }
        }, option);
        }
    }
}
