using STGS.Internet.Tool;

namespace STGS.Internet.Core
{
    public enum BuyState
    {
        /// <summary>
        /// 缺省值
        /// </summary>
        [EnumTextMeta]
        None,

        /// <summary>
        /// 未付款
        /// </summary>
        [EnumTextMeta]
        NotPaying,

        /// <summary>
        /// 已付款
        /// </summary>
        [EnumTextMeta]
        HasPaying,

        /// <summary>
        /// 付款失败
        /// </summary>
        [EnumTextMeta]
        PayFailure
    }
}
