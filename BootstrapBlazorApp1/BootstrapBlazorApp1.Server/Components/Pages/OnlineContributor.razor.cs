using BootstrapBlazorApp1.Server.Data;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace BootstrapBlazorApp1.Server.Components.Pages
{
    /// <summary>
    /// Online sheet sample code
    /// </summary>
    public partial class OnlineContributor
    {
        /// <summary>
        /// Gets or sets Contributor
        /// </summary>
        [Parameter]
        [NotNull]
        public Contributor? Contributor { get; set; }
    }
}
