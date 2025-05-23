using BootstrapBlazor.Components;

namespace BootstrapBlazorApp1.Server.Data
{
    /// <summary>
    /// Contributor
    /// </summary>
    public class Contributor
    {
        /// <summary>
        /// Gets or sets Name
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Gets or sets Avatar
        /// </summary>
        public string? Avatar { get; set; }

        /// <summary>
        /// Gets or sets Description
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets Sheet data
        /// </summary>
        public UniverSheetData? Data { get; set; }
    }
}
