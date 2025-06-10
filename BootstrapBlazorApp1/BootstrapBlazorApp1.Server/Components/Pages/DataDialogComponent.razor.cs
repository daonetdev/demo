using BootstrapBlazorApp1.Server.Data;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace BootstrapBlazorApp1.Server.Components.Pages
{
    public partial class DataDialogComponent
    {
        [Parameter]
        [NotNull]
        public Foo? Value { get; set; }

        [CascadingParameter(Name = "BodyContext")]
        private object? Parameter { get; set; }
    }
}
