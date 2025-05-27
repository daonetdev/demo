using BootstrapBlazor.Components;
using FreeSql;
using FreeSql.Internal.Model;
using System.Linq.Expressions;

namespace DemoAdminBlazor;

public static class FreeSqlExtensions
{
    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
    {
        var invokedExpr = Expression.Invoke(expr2, expr1.Parameters);
        return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
    }

    /// <summary>
    /// 排序处理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="select"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static ISelect<T> ApplyOrder<T,TKey>(this ISelect<T> select, QueryPageOptions options) where T : class, IEntity<TKey>
    {
        return select.OrderBy(string.Join(",", options.SortList.ToArray()))
                        .OrderByIf(options.SortOrder == SortOrder.Unset && options.SortList.Count == 0, x => x.Id, true)
                        .OrderByPropertyNameIf(options.SortOrder != SortOrder.Unset, options.SortName, options.SortOrder == SortOrder.Asc);
    }

    /// <summary>
    /// 对查询进行分页处理的扩展方法
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="select"></param>
    /// <param name="options"></param>
    /// <param name="ignoreColumns">要忽略自动筛选的属性名称</param>
    /// <returns></returns>
    public static async Task<QueryData<T>> GetPagedAsync<T,TKey>(this ISelect<T> select, QueryPageOptions options,params string[] ignoreColumns) where T : class, IEntity<TKey>
    {
        var query = select
           .WhereDynamicFilter(options.ToDynamicFilter(ignoreColumns))
           .ApplyOrder<T, TKey>(options)
           .Count(out var count);

        var items = options.IsPage ? await query.Page(options.PageIndex, options.PageItems).ToListAsync() : await query.ToListAsync();

        return new QueryData<T>()
        {
            Items = items,
            TotalCount = Convert.ToInt32(count)
        };
    }

    /// <summary>
    /// QueryPageOptions 转化为 FreeSql ORM DynamicFilterInfo 类型扩展方法
    /// </summary>
    /// <param name="option"></param>
    /// <param name="ignoreColumns">要忽略的属性</param>
    /// <returns></returns>
    public static DynamicFilterInfo ToDynamicFilter(this QueryPageOptions option,params string[] ignoreColumns)
    {
        var ret = new DynamicFilterInfo() { Filters = [] };

        // 处理模糊搜索
        if (option.Searches.Count > 0)
        {
            ret.Filters.Add(new()
            {
                Logic = DynamicFilterLogic.Or,
                Filters = option.Searches.Select(i => i.ToDynamicFilter(ignoreColumns)).ToList()
            });
        }

        // 处理自定义搜索
        if (option.CustomerSearches.Count > 0)
        {
            ret.Filters.AddRange(option.CustomerSearches.Select(i => i.ToDynamicFilter(ignoreColumns)));
        }

        // 处理高级搜索
        if (option.AdvanceSearches.Count > 0)
        {
            ret.Filters.AddRange(option.AdvanceSearches.Select(i => i.ToDynamicFilter(ignoreColumns)));
        }

        // 处理表格过滤条件
        if (option.Filters.Count > 0)
        {
            ret.Filters.AddRange(option.Filters.Select(i => i.ToDynamicFilter(ignoreColumns)));
        }

        ret.Filters = ret.Filters.Where(i => i != null).ToList();

        return ret;
    }

    /// <summary>
    /// IFilterAction 转化为 DynamicFilterInfo 扩展方法
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="ignoreColumns">要忽略的属性</param>
    /// <returns></returns>
    public static DynamicFilterInfo ToDynamicFilter(this IFilterAction filter, params string[] ignoreColumns)
    {
        var filterKeyValueAction = filter.GetFilterConditions();
        if(ignoreColumns == null)
        {
            return filterKeyValueAction.ParseDynamicFilterInfo();
        }
        if (ignoreColumns != null && !ignoreColumns.Contains(filterKeyValueAction.FieldKey))
        {
            return filterKeyValueAction.ParseDynamicFilterInfo();
        }

        return null;
    }

    private static DynamicFilterInfo ParseDynamicFilterInfo(this FilterKeyValueAction filterKeyValueAction) => new()
    {
        Operator = filterKeyValueAction.FilterAction.ToDynamicFilterOperator(),
        Logic = filterKeyValueAction.FilterLogic.ToDynamicFilterLogic(),
        Field = filterKeyValueAction.FieldKey,
        Value = filterKeyValueAction.FieldValue,
        Filters = filterKeyValueAction.Filters?.Select(i => i.ParseDynamicFilterInfo()).ToList()
    };

    private static DynamicFilterLogic ToDynamicFilterLogic(this FilterLogic logic) => logic switch
    {
        FilterLogic.And => DynamicFilterLogic.And,
        _ => DynamicFilterLogic.Or
    };

    private static DynamicFilterOperator ToDynamicFilterOperator(this FilterAction action) => action switch
    {
        FilterAction.Equal => DynamicFilterOperator.Equal,
        FilterAction.NotEqual => DynamicFilterOperator.NotEqual,
        FilterAction.Contains => DynamicFilterOperator.Contains,
        FilterAction.NotContains => DynamicFilterOperator.NotContains,
        FilterAction.GreaterThan => DynamicFilterOperator.GreaterThan,
        FilterAction.GreaterThanOrEqual => DynamicFilterOperator.GreaterThanOrEqual,
        FilterAction.LessThan => DynamicFilterOperator.LessThan,
        FilterAction.LessThanOrEqual => DynamicFilterOperator.LessThanOrEqual,
        _ => throw new NotSupportedException()
    };
}
