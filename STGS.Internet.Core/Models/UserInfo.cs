using Sino.Domain.Entities;
using System;

namespace STGS.Internet.Core
{
    public class UserInfo : Entity<string>
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
        /// 发布物品数
        /// </summary>
        public int SendGoodsNumber { get; set; }

        /// <summary>
        /// 购买物品数
        /// </summary>
        public int BuyGoodsNumber { get; set; }
        
        /// <summary>
        ///学校名称 
        /// </summary>
        public string SchoolName { get; set; }

        /// <summary>
        /// 收件地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 系统角色
        /// </summary>
        public SysRole SystemRole { get; set; }

        /// <summary>
        /// 是否注销
        /// </summary>
        public YesOrNo IsDelete { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}
