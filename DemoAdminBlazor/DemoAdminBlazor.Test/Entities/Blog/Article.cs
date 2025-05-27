using FreeSql.DataAnnotations;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DemoAdminBlazor.Test.Blog
{
    partial class Tag2
    {
        [Navigate(ManyToMany = typeof(TagArticle))]
        public List<Article> Articles { get; set; }
        [Table(Name = "blog_tag_article")]
        public class TagArticle
        {
            public long TagId { get; set; }
            public long ArticleId { get; set; }

            public Tag2 Tag { get; set; }
            public Article Article { get; set; }
        }
    }

    /// <summary>
    /// 随笔
    /// </summary>
    [Table(Name = "blog_article")]
    public partial class Article : EntityModified
    {
        [Navigate(ManyToMany = typeof(Tag2.TagArticle))]
        [DisplayName("标签")]
        public List<Tag2> Tags { get; set; }

        [Navigate(nameof(UserLike.SubjectId))]
        [DisplayName("用户点赞记录")]
        public List<UserLike> UserLikes { get; set; }

        /// <summary>
        /// 随笔专栏
        /// </summary>
        [DisplayName("随笔专栏")]
        [Required]
        public long? ClassifyId { get; set; }

        public Classify Classify { get; set; }

        /// <summary>
        /// 技术频道
        /// </summary>
        [DisplayName("技术频道")]
        public long? ChannelId { get; set; }

        public Channel Channel { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Column(StringLength = 200)]
        [DisplayName("标题")]
        public string Title { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        [Column(StringLength = 400)]
        [DisplayName("关键字")]
        public string Keywords { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        [Column(StringLength = 400)]
        [DisplayName("来源")]
        public string Source { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        [Column(StringLength = 500)]
        [DisplayName("摘要")]
        public string Excerpt { get; set; }

        /// <summary>
        /// 正文
        /// </summary>
        [Column(StringLength = -2)]
        [DisplayName("正文")]
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// 浏览量
        /// </summary>
        [DisplayName("浏览量")]
        public int ViewHits { get; set; }

        /// <summary>
        /// 评论数量
        /// </summary>
        [DisplayName("评论数量")]
        public int CommentQuantity { get; set; }

        /// <summary>
        /// 点赞数量
        /// </summary>
        [DisplayName("点赞数量")]
        public int LikesQuantity { get; set; }

        /// <summary>
        /// 收藏数量
        /// </summary>
        [DisplayName("收藏数量")]
        public int CollectQuantity { get; set; }

        /// <summary>
        /// 缩略图
        /// </summary>
        [Column(StringLength = 400)]
        [DisplayName("缩略图")]
        public string Thumbnail { get; set; }

        /// <summary>
        /// 是否审核
        /// </summary>
        [DisplayName("是否审核")]
        public bool IsAudit { get; set; }

        /// <summary>
        /// 是否推荐
        /// </summary>
        [DisplayName("是否推荐")]
        public bool Recommend { get; set; }

        /// <summary>
        /// 是否置顶
        /// </summary>
        [DisplayName("是否置顶")]
        public bool IsStickie { get; set; }

        /// <summary>
        /// 随笔类型
        /// </summary>
        [Column(MapType = typeof(int))]
        [DisplayName("随笔类型")]
        public ArticleType ArticleType { get; set; }

        /// <summary>
        /// 字数
        /// </summary>
        [DisplayName("字数")]
        public long WordNumber { get; set; }

        /// <summary>
        /// 阅读时长
        /// </summary>
        [DisplayName("阅读时长")]
        public long ReadingTime { get; set; }

        /// <summary>
        /// 开启评论
        /// </summary>
        [DisplayName("开启评论")]
        public bool Commentable { get; set; } = true;
    }

    /// <summary>
    /// 随笔类型
    /// </summary>
    public enum ArticleType
    {
        原创,
        转载,
        翻译,
    }
}
