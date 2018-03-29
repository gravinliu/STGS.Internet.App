using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STGS.Internet.App.ViewResponse
{
    public class LoginResponse
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 访问令牌
        /// </summary>
        public string JwtToken { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public long TokenExpirationDate { get; set; }
    }
}
