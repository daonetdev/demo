using System.Diagnostics.CodeAnalysis;

namespace BootstrapBlazorApp1.Server.Data
{
    /// <summary>
    /// 样式项目类
    /// </summary>
    public class ThemeOption
    {
        /// <summary>
        /// 获得/设置 样式键值
        /// </summary>
        [NotNull]
        public string? Key { get; set; }

        /// <summary>
        /// 获得/设置 样式名称
        /// </summary>
        [NotNull]
        public string? Name { get; set; }

        /// <summary>
        /// 获得/设置 样式文件集合
        /// </summary>
        [NotNull]
        public string[]? Files { get; set; }
    }
}
