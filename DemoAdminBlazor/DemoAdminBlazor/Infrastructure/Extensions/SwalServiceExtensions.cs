using BootstrapBlazor.Components;

public static  class SwalServiceExtensions
{
    /// <summary>
    /// Swal 调用成功快捷方法
    /// </summary>
    /// <param name="service"></param>
    /// <param name="title">Title 属性</param>
    /// <param name="content">Content 属性</param>
    /// <param name="autoHide">自动隐藏属性默认为 true</param>
    public static Task Success(this SwalService service, string? title = null, string? content = null, bool autoHide = true) => Success(service, title, content, autoHide, true);

    /// <summary>
    /// Swal 调用成功快捷方法
    /// </summary>
    /// <param name="service"></param>
    /// <param name="title">Title 属性</param>
    /// <param name="content">Content 属性</param>
    /// <param name="autoHide">自动隐藏属性默认为 true</param>
    /// <param name="showClose">是否显示关闭按钮 默认 true</param>
    public static Task Success(this SwalService service, string? title, string? content, bool autoHide, bool showClose) => service.Show(new SwalOption()
    {
        Category = SwalCategory.Success,
        IsAutoHide = autoHide,
        Title = title,
        Content = content,
        ShowClose = showClose
    });

    /// <summary>
    /// Swal 调用错误快捷方法
    /// </summary>
    /// <param name="service"></param>
    /// <param name="title">Title 属性</param>
    /// <param name="content">Content 属性</param>
    /// <param name="autoHide">自动隐藏属性默认为 true</param>
    public static Task Error(this SwalService service, string? title = null, string? content = null, bool autoHide = true) => Error(service, title, content, autoHide, true);

    /// <summary>
    /// Swal 调用错误快捷方法
    /// </summary>
    /// <param name="service"></param>
    /// <param name="title">Title 属性</param>
    /// <param name="content">Content 属性</param>
    /// <param name="autoHide">自动隐藏属性默认为 true</param>
    /// <param name="showClose">是否显示关闭按钮 默认 true</param>
    public static Task Error(this SwalService service, string? title, string? content, bool autoHide, bool showClose) => service.Show(new SwalOption()
    {
        Category = SwalCategory.Error,
        IsAutoHide = autoHide,
        Title = title,
        Content = content,
        ShowClose = showClose
    });

    /// <summary>
    /// Swal 调用提示信息快捷方法
    /// </summary>
    /// <param name="service"></param>
    /// <param name="title">Title 属性</param>
    /// <param name="content">Content 属性</param>
    /// <param name="autoHide">自动隐藏属性默认为 true</param>
    public static Task Information(this SwalService service, string? title = null, string? content = null, bool autoHide = true) => Information(service, title, content, autoHide, true);

    /// <summary>
    /// Swal 调用提示信息快捷方法
    /// </summary>
    /// <param name="service"></param>
    /// <param name="title">Title 属性</param>
    /// <param name="content">Content 属性</param>
    /// <param name="autoHide">自动隐藏属性默认为 true</param>
    /// <param name="showClose">是否显示关闭按钮 默认 true</param>
    public static Task Information(this SwalService service, string? title, string? content, bool autoHide, bool showClose) => service.Show(new SwalOption()
    {
        Category = SwalCategory.Information,
        IsAutoHide = autoHide,
        Title = title,
        Content = content,
        ShowClose = showClose
    });

    /// <summary>
    /// Swal 调用警告信息快捷方法
    /// </summary>
    /// <param name="service"></param>
    /// <param name="title">Title 属性</param>
    /// <param name="content">Content 属性</param>
    /// <param name="autoHide">自动隐藏属性默认为 true</param>
    public static Task Warning(this SwalService service, string? title = null, string? content = null, bool autoHide = true) => Warning(service, title, content, autoHide, true);

    /// <summary>
    /// Swal 调用警告信息快捷方法
    /// </summary>
    /// <param name="service"></param>
    /// <param name="title">Title 属性</param>
    /// <param name="content">Content 属性</param>
    /// <param name="autoHide">自动隐藏属性默认为 true</param>
    /// <param name="showClose">是否显示关闭按钮 默认 true</param>
    public static Task Warning(this SwalService service, string? title, string? content, bool autoHide, bool showClose) => service.Show(new SwalOption()
    {
        Category = SwalCategory.Warning,
        IsAutoHide = autoHide,
        Title = title,
        Content = content,
        ShowClose = showClose
    });

    /// <summary>
    /// 确认弹窗
    /// </summary>
    /// <param name="service"></param>
    /// <param name="title"></param>
    /// <param name="content"></param>
    /// <param name="autoHide"></param>
    /// <param name="showClose"></param>
    /// <returns></returns>
    public static Task<bool> Confirm(this SwalService service, string? title, string? content) => service.ShowModal(new SwalOption()
    {
        Category = SwalCategory.Question,
        Title = title,
        Content = content,
    });
}