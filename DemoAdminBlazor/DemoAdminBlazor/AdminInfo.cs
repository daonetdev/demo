using BootstrapBlazor.Components;
using FreeSql;

namespace DemoAdminBlazor
{
    public class AdminRemoveEventArgs<TItem> where TItem : class
    {
        public List<TItem> Items { get; set; }
        public bool Cancel { get; set; }
    }

    public class AdminImportEventArgs<TItem> where TItem : class
    {
        public List<TItem> Items { get; set; }
        public bool Cancel { get; set; }
    }

    public class AdminSaveEventArgs<TItem> where TItem : class
    {
        public ItemChangedType ChangedType { get; set; }
        public TItem Item { get; set; }
        public bool Cancel { get; set; }
    }

    public record AdminQueryEventArgs<TItem>(ISelect<TItem> Select, QueryPageOptions options);
}
