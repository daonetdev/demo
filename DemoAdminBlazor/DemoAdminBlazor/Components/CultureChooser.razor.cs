﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License
// See the LICENSE file in the project root for more information.
// Maintainer: Argo Zhang(argo@live.ca) Website: https://www.blazor.zone

using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace DemoAdminBlazor.Components;

/// <summary>
/// 
/// </summary>
public partial class CultureChooser
{
    [Inject]
    [NotNull]
    private IOptionsMonitor<BootstrapBlazorOptions>? BootstrapOptions { get; set; }

    [Inject]
    [NotNull]
    private IStringLocalizer<CultureChooser>? Localizer { get; set; }

    [Inject]
    [NotNull]
    private NavigationManager? NavigationManager { get; set; }

    private string? ClassString => CssBuilder.Default("culture-selector")
        .AddClassFromAttributes(AdditionalAttributes)
        .Build();

    private string SelectedCulture { get; set; } = CultureInfo.CurrentCulture.Name;

    [NotNull]
    private string? Label { get; set; }

    /// <summary>
    /// OnInitialized 方法
    /// </summary>
    protected override void OnInitialized()
    {
        base.OnInitialized();

        Label ??= Localizer[nameof(Label)];
    }

    private async Task SetCulture(SelectedItem item)
    {
        if (RendererInfo.Name == "Server")
        {
            // 使用 api 方式 适用于 Server-Side 模式
            if (SelectedCulture != item.Value)
            {
                var culture = item.Value;
                var uri = new Uri(NavigationManager.Uri).GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped);

                await admin.SetCookie(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture, culture)), 365);

                try
                {
                    // use a path that matches your culture redirect controller from the previous steps
                    NavigationManager.NavigateTo(uri, forceLoad: true);
                }
                catch { }
            }
            else
            {
                if (SelectedCulture != item.Value)
                {
                    var culture = item.Value;
                    await JSRuntime.InvokeVoidAsync("bbCulture.set", culture);

                    NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
                }
            }
        }
    }

    private string GetDisplayName(CultureInfo culture)
    {
        string? ret;
        if (RendererInfo.Name == "Server")
        {
            ret = culture.NativeName;
        }
        else
        {
            ret = culture.Name switch
            {
                "zh-CN" => "中文（中国）",
                "en-US" => "English (United States)",
                _ => ""
            };
        }
        return ret;
    }
}
