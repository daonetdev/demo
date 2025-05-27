namespace DemoAdminBlazor;

public interface IHasParentId<T>
{
    T Id { get; }
    T ParentId { get; }
}
