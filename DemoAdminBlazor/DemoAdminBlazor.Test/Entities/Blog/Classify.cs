using FreeSql.DataAnnotations;
using System.ComponentModel;

namespace DemoAdminBlazor.Test.Blog
{

    /// <summary>
    /// 随笔专栏
    /// </summary>
    [Table(Name = "blog_classify")]
    public class Classify : EntityCreated
    {
        /// <summary>
        /// 分类专栏名称
        /// </summary>
        [Column(StringLength = 50)]
        [DisplayName("专栏名称")]
        public string ClassifyName { get; set; }
        /// <summary>
        /// 封面图
        /// </summary>
        [Column(StringLength = 100)]
        [DisplayName("封面图")]
        public string Thumbnail { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [DisplayName("排序")]
        public int SortCode { get; set; }

        /// <summary>
        /// 随笔数量
        /// </summary>
        [DisplayName("排序")]
        public int ArticleCount { get; set; }
    }
}
