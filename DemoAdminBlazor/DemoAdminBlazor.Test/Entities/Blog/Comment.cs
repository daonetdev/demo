using FreeSql.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DemoAdminBlazor.Test.Blog
{
    /// <summary>
    /// 评论信息
    /// </summary>
    [Table(Name = "blog_comment")]
    public class Comment : EntityModified
    {
        [Navigate(nameof(UserLike.SubjectId))]
        public List<UserLike> UserLikes { get; set; }

        /// <summary>
        /// 关联随笔
        /// </summary>
        [DisplayName("关联随笔")]
        public long? ArticleId { get; set; }
        [DisplayName("关联随笔")]
        [Required]
        public Article Article { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [Column(StringLength = 500)]
        [DisplayName("内容")]
        [Required]
        public string Text { get; set; }

        /// <summary>
        /// 点赞量
        /// </summary>
        [DisplayName("点赞量")]
        public int LikesQuantity { get; set; }

        /// <summary>
        /// 是否已审核
        /// </summary>
        [DisplayName("已审核")]
        public bool IsAudit { get; set; } = true;

    }
}