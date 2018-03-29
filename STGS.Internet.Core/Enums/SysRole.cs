using STGS.Internet.Tool;

namespace STGS.Internet.Core
{
    /// <summary>
    /// 角色枚举
    /// </summary>
    public enum SysRole
    {
        /// <summary>
        /// 缺省值
        /// </summary>
        [EnumTextMeta]
        None,

        /// <summary>
        /// 会员用户
        /// </summary>
        [EnumTextMeta]
        User,

        /// <summary>
        /// 系统管理员
        /// </summary>
        [EnumTextMeta]
        Admin
    }
}
