﻿﻿﻿using BootstrapBlazor.Components;
using FreeSql.DataAnnotations;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace DemoAdminBlazor;

/// <summary>
/// 角色
/// </summary>
public class SysRole : Entity
{
    /// <summary>
    /// 名称
    /// </summary>
    [Column(StringLength = 50)]
    [DisplayName("角色名称")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string Name { get; set; }
    /// <summary>
    /// 描述
    /// </summary>
    [Column(StringLength = 500)]
    [Display(Name ="描述")]
    [Required(ErrorMessage = "{0}不能为空")]
    public string Description { get; set; }
    /// <summary>
    /// 系统
    /// </summary>
    public bool IsAdministrator { get; set; }

    [Navigate(ManyToMany = typeof(SysRoleUser))]
    public List<SysUser> Users { get; set; }

    [Navigate(ManyToMany = typeof(SysRoleMenu))]
    public List<SysMenu> Menus { get; set; }
}

/// <summary>
/// 角色用户关联类，用于表示角色和用户之间的多对多关系。
/// </summary>
public class SysRoleUser
{
    /// <summary>
    /// 获取或设置角色的 ID。
    /// </summary>
    public long RoleId { get; set; }
    /// <summary>
    /// 获取或设置用户的 ID。
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 获取或设置关联的角色。
    /// </summary>
    public SysRole Role { get; set; }
    /// <summary>
    /// 获取或设置关联的用户。
    /// </summary>
    public SysUser User { get; set; }
}

partial class SysMenu
{
    [Navigate(ManyToMany = typeof(SysRoleMenu))]
    public List<SysRole> Roles { get; set; }
    [Navigate(nameof(SysRoleMenu.MenuId))]
    public List<SysRoleMenu> RoleMenus { get; set; }
}
/// <summary>
/// 角色菜单关联类，用于表示角色和菜单之间的多对多关系。
/// </summary>
public class SysRoleMenu
{
    /// <summary>
    /// 获取或设置角色的 ID。
    /// </summary>
    public long RoleId { get; set; }
    /// <summary>
    /// 获取或设置菜单的 ID。
    /// </summary>
    public long MenuId { get; set; }

    /// <summary>
    /// 获取或设置关联的角色。
    /// </summary>
    public SysRole Role { get; set; }
    /// <summary>
    /// 获取或设置关联的菜单。
    /// </summary>
    public SysMenu Menu { get; set; }
}

partial class SysUser
{
    [Navigate(ManyToMany = typeof(SysRoleUser))]
    [DisplayName("角色")]
    [AutoGenerateColumn(IgnoreWhenExport =true)]
    public List<SysRole> Roles { get; set; }

    [Navigate(nameof(SysRoleUser.UserId))]
    [AutoGenerateColumn(Ignore = true)]
    public List<SysRoleUser> RoleUsers { get; set; }
}