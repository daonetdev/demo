using BootstrapBlazor.Components;
using BootstrapBlazorApp1.Server.Data;
using System.Diagnostics.CodeAnalysis;

namespace BootstrapBlazorApp1.Server.Components.Pages
{
    public partial class SelectObjectDemo
    {
        [NotNull]
        private IEnumerable<Product>? Products { get; set; }

        private Product? _value;

        private Product? _widthValue;

        private Product? _heightValue;

        private int _counter;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void OnInitialized()
        {
            base.OnInitialized();

            Products = Enumerable.Range(1, 8).Select(i => new Product()
            {
                ImageUrl = $"{WebsiteOption.CurrentValue.AssetRootPath}images/Pic{i}.jpg",
                Description = $"Pic{i}.jpg",
                Category = $"Group{(i % 4) + 1}"
            });
        }

        private static async Task OnListViewItemClick(Product product, ISelectObjectContext<Product?> context)
        {
            // 设置组件值
            context.SetValue(product);

            // 当前模式为单选，主动关闭弹窗
            await context.CloseAsync();
        }

        private static string? GetTextCallback(Product? product) => product?.ImageUrl;

        private static string? GetCounterTextCallback(int v) => v.ToString();

        private static async Task OnSubmit(int v, ISelectObjectContext<int> context)
        {
            context.SetValue(v);

            // 当前模式为单选，主动关闭弹窗
            await context.CloseAsync();
        }
    }
}
