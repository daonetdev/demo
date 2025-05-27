using System.ComponentModel.DataAnnotations;

namespace DemoAdminBlazor
{
    /// <summary>
    /// 消息
    /// </summary>
    public class SysChatMessage:Entity<long>
    {
        public long FromUserId { get; set; }
        public long RecUserId { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime SendTime { get; set; }
        public bool IsOfflineMsg { get; set; }
    }
}
