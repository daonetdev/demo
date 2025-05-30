using BootstrapBlazor.Components;
using DemoAdminBlazor;
using DemoAdminBlazor.Components;
using DemoAdminBlazor.Test.Blog;
using DemoAdminBlazor.Test.Components;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.AddDemoAdminBlazor(new DemoAdminBlazorOptions
{
    Assemblies = [typeof(Program).Assembly],
    EnableLocalization = true,//启用多
    UseAutoSyncStructure = true,
    DataType= FreeSql.DataType.Sqlite
});

// Add services to the container.
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

// Add services to the container.
builder.Services.AddRazorPages();

// 配置 HttpClient
builder.Services.AddScoped<HttpClient>();

builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services.AddBootstrapBlazorTableExportService();

builder.Services.Configure<HubOptions>(option => {
    option.MaximumReceiveMessageSize = null;
});

var app = builder.Build();

// 如果EnableLocalization=true，请把下面注释打开
var option = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
if (option != null)
{
    app.UseRequestLocalization(option.Value);
}

app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.All });

app.UseStaticFiles();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddAdditionalAssemblies(typeof(DemoAdminBlazor._Imports).Assembly)
    .AddInteractiveServerRenderMode();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllers();

#region 博客示例测试数据
var fsql = app.Services.GetService<IFreeSql>();
if (!fsql.Select<Classify>().Any() &&
    !fsql.Select<SysMenu>().Any(a => new[]
    {
        "Blog/Article",
        "Blog/Comment",
        "Blog/Classify",
        "Blog/Channel",
        "Blog/Tag2",
        "Blog/UserLike",
    }.Contains(a.Path)))
{
    var adminUser = fsql.Select<SysUser>().Where(a => a.Roles.Any(b => b.IsAdministrator)).First();

    List<SysMenu> getCudButtons(params SysMenu[] btns) => new[]
    {
        new SysMenu { Label = "添加", Path = "add", Sort = 10011, Type = SysMenuType.按钮, },
        new SysMenu { Label = "编辑", Path = "edit", Sort = 10012, Type = SysMenuType.按钮, },
        new SysMenu { Label = "删除", Path = "remove", Sort = 10013, Type = SysMenuType.按钮, }
    }.Concat(btns ?? new SysMenu[0]).ToList();
    var repo = fsql.GetAggregateRootRepository<SysMenu>();
    repo.Insert(new[]
    {
        new SysMenu
        {
            Label = "博客示例",
            Path = "",
            Sort = 998,
            Type = SysMenuType.菜单,
            Childs = new List<SysMenu>
            {
                new SysMenu { Label = "随笔文章", Path = "Blog/Article", Sort = 10001, Type = SysMenuType.菜单, Childs = getCudButtons() },
                new SysMenu { Label = "评论", Path = "Blog/Comment", Sort = 10002, Type = SysMenuType.菜单, Childs = getCudButtons() },
                new SysMenu { Label = "随笔专栏", Path = "Blog/Classify", Sort = 10003, Type = SysMenuType.菜单, Childs = getCudButtons() },
                new SysMenu { Label = "技术频道", Path = "Blog/Channel", Sort = 10004, Type = SysMenuType.菜单, Childs = getCudButtons() },
                new SysMenu { Label = "标签", Path = "Blog/Tag2", Sort = 10006, Type = SysMenuType.菜单, Childs = getCudButtons() },
                new SysMenu { Label = "用户点赞", Path = "Blog/UserLike", Sort = 10007, Type = SysMenuType.菜单, Childs = getCudButtons() },
            }
        },
    });

    fsql.Insert(new[]
    {
        new Classify { Id = 510337284071493, ClassifyName = "FreeSql", CreatedUserId = adminUser.Id, CreatedUserName = adminUser.Username },
        new Classify { Id = 510337332621381, ClassifyName = "FreeRedis", CreatedUserId = adminUser.Id, CreatedUserName = adminUser.Username },
        new Classify { Id = 510337373491269, ClassifyName = "FreeScheduler", CreatedUserId = adminUser.Id, CreatedUserName = adminUser.Username },
        new Classify { Id = 510337418735685, ClassifyName = "CSRedis", CreatedUserId = adminUser.Id, CreatedUserName = adminUser.Username },
        new Classify { Id = 510337460719685, ClassifyName = "AdminBlazor", CreatedUserId = adminUser.Id, CreatedUserName = adminUser.Username },
    }).ExecuteAffrows();

    fsql.Insert(new[]
    {
        new Channel { Id = 510338108866629, ChannelName = ".NET", ChannelCode = "net", Remark = ".NET技术频道", Status = true, CreatedUserId = adminUser.Id, CreatedUserName = adminUser.Username },
        new Channel { Id = 510338191179845, ChannelName = "前端", ChannelCode = "html", Remark = "前端技术频道", Status = true, CreatedUserId = adminUser.Id, CreatedUserName = adminUser.Username },
        new Channel { Id = 510338291052613, ChannelName = "数据库", ChannelCode = "db", Remark = "数据库技术频道", Status = true, CreatedUserId = adminUser.Id, CreatedUserName = adminUser.Username },
    }).ExecuteAffrows();

    fsql.Insert(new[]
    {
        new Tag2 { Id = 510340412510277, TagName = "orm", Remark =  "orm 文章内容", Status = true, CreatedUserId = adminUser.Id, CreatedUserName = adminUser.Username },
        new Tag2 { Id = 510340482543685, TagName = "js", Remark =  "js 有关内容", Status = true, CreatedUserId = adminUser.Id, CreatedUserName = adminUser.Username },
        new Tag2 { Id = 510340574564421, TagName = "vue", Remark =  "vue 有关内容", Status = false, CreatedUserId = adminUser.Id, CreatedUserName = adminUser.Username },
        new Tag2 { Id = 510340626989125, TagName = "react", Remark = "react 技术", Status = true, CreatedUserId = adminUser.Id, CreatedUserName = adminUser.Username },
    }).ExecuteAffrows();

    fsql.Insert(new[]
    {
        new Tag2.ChannelTag2 { ChannelId = 510338108866629, TagId = 510340412510277 },
        new Tag2.ChannelTag2 { ChannelId = 510338291052613, TagId = 510340412510277 },
        new Tag2.ChannelTag2 { ChannelId = 510338191179845, TagId = 510340482543685 },
        new Tag2.ChannelTag2 { ChannelId = 510338191179845, TagId = 510340574564421 },
        new Tag2.ChannelTag2 { ChannelId = 510338191179845, TagId = 510340626989125 },
    }).ExecuteAffrows();


    fsql.Insert(new[]
    {
        new Article {
            Id = 510359705468997, ClassifyId = 510337460719685, ChannelId = 510338191179845,
            Title = "国产首个支持 AOT 发布的 ORM?", Excerpt = "...",
            Content = "FreeSql\r\n是一款功能强大的对象关系映射（O/RM）组件，支持 .NET Core 2.1+、.NET Framework 4.0+、Xamarin，国产首个支持 AOT 发布的 ORM?\r\n\r\n进入文档 ??视频教程 ??\r\n\r\n开发者优先\r\n??以开发者为中心的设计理念，想你所想，亦享你所享。\r\n\r\n多场景实现\r\n?? 支持 CodeFirst / DbFirst / DbContext / Repository / UnitOfWork / AOP / 支持 .NETCore 2.1+, .NETFramework 4.0+, AOT, Xamarin\r\n\r\n多数据库支持\r\n??MySql、SqlServer、PostgreSQL、Oracle、Sqlite、Firebird、达梦、人大金仓、南大通用、虚谷、神舟、翰高、ClickHouse、QuestDB、Access 等数据库\r\n\r\n丰富的表达式函数\r\n? 支持 丰富的表达式函数，以及灵活的自定义解析；\r\n\r\nDbFirst\r\n?? 支持 DbFirst 模式，支持从数据库导入实体类，或使用实体类生成工具生成实体类；\r\n\r\n类型映射\r\n? 支持 深入的类型映射，比如 Pgsql 的数组类型等；\r\n\r\n导航属性\r\n?? 支持 导航属性一对多、多对多贪婪加载，以及延时加载；\r\n\r\n读写分离\r\n?? 支持 读写分离、分表分库、过滤器、乐观锁、悲观锁；", IsAudit = true, CreatedUserId = adminUser.Id, CreatedUserName = adminUser.Username
        },
    }).ExecuteAffrows();

    fsql.Insert(new[]
    {
        new Tag2.TagArticle { ArticleId = 510359705468997, TagId = 510340412510277 },
        new Tag2.TagArticle { ArticleId = 510359705468997, TagId = 510340574564421 },
    }).ExecuteAffrows();

    fsql.Insert(new[]
    {
        new Comment { Id = 510365667639365, ArticleId = 510359705468997, Text = "非常好。。。~~", IsAudit = true, CreatedUserId = adminUser.Id, CreatedUserName = adminUser.Username },
    }).ExecuteAffrows();

    fsql.Insert(new[]
    {
        new UserLike { Id = 510365571252293, SubjectId = 510359705468997, SubjectType = UserLikeSubjectType.点赞随笔, CreatedUserId = adminUser.Id, CreatedUserName = adminUser.Username },
        new UserLike { Id = 510366600106053, SubjectId = 510365667639365, SubjectType = UserLikeSubjectType.点赞评论, CreatedUserId = adminUser.Id, CreatedUserName = adminUser.Username },
    }).ExecuteAffrows();

}
#endregion

app.Run();