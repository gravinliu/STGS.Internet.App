using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STGS.Internet.App.ViewModels
{
    public class LoginVM
    {

        /// <summary>
        /// 登陆名（手机号或邮箱地址）
        /// </summary>
        public string LoginKey { get; set; }

        /// <summary>
        /// 登陆密码
        /// </summary>
        public string PassWordHash { get; set; }
    }
}
