using STGS.Internet.Core;

namespace STGS.Internet.App.ViewModels
{
    public class RegesiterVM
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        ///用户密码 
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public Sex Sex { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// QQ号码
        /// </summary>
        public string QqNumber { get; set; }

        /// <summary>
        ///学校名称 
        /// </summary>
        public string SchoolName { get; set; }

        /// <summary>
        /// 收件地址
        /// </summary>
        public string Address { get; set; }

    }
}
