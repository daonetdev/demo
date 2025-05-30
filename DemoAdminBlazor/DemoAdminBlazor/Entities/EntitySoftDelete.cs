﻿using BootstrapBlazor.Components;
using FreeSql.DataAnnotations;
using System.ComponentModel;

namespace DemoAdminBlazor;

public interface IEntitySoftDelete
{
    /// <summary>
    /// 是否删除
    /// </summary>
    bool IsDeleted { get; set; }
}

/// <summary>
/// 实体删除
/// </summary>
public abstract class EntitySoftDelete<TKey> : EntityModified<TKey>, IEntitySoftDelete
{
    /// <summary>
    /// 是否删除
    /// </summary>
    [Column(Position = -9)]
    [AutoGenerateColumn(Ignore = true)]
    public virtual bool IsDeleted { get; set; } = false;
}

/// <summary>
/// 实体删除
/// </summary>
public abstract class EntitySoftDelete : EntitySoftDelete<long>
{
}
