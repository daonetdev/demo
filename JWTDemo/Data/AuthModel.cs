namespace JWTDemo.Data
{
    public class AuthModel
    {
        /// <summary>
        /// token令牌
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        ///  token类型
        /// </summary>
        public string token_type { get; set; }
        /// <summary>
        /// 有效期
        /// </summary>
        public int expires_in { get; set; }
        /// <summary>
        /// 权限
        /// </summary>
        public string scope { get; set; }

        /// <summary>
        /// 错误码
        /// </summary>
        public string error { get; set; }


        /// <summary>
        /// 错误描述
        /// </summary>
        public string error_description { get; set; }

    }
}
