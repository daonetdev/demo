using System.Linq.Expressions;

namespace DemoAdminBlazor;

public static class ExpressionExtensions
{
    /// <summary>
    /// 从表达式中提取属性名并添加到目标列表
    /// </summary>
    /// <typeparam name="TItem">表达式输入类型</typeparam>
    /// <param name="expression">表达式</param>
    /// <param name="targetList">目标列表</param>
    public static void ExtractPropertyNames<TItem>(this Expression<Func<TItem, object>> expression, List<string> targetList)
    {
        switch (expression.Body.NodeType)
        {
            case ExpressionType.New:
                var newExpression = (NewExpression)expression.Body;
                foreach (var member in newExpression.Members)
                {
                    targetList.Add(member.Name);
                }
                break;
            case ExpressionType.MemberAccess:
                var memberExpression = (MemberExpression)expression.Body;
                targetList.Add(memberExpression.Member.Name);
                break;
        }
    }
}