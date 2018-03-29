using STGS.Internet.Core;

namespace STGS.Internet.App.ViewModels
{
    public class ReleaseVM
    {
        /// <summary>
        /// 物品名称
        /// </summary>
        public string GoodsName { get; set; }

        /// <summary>
        /// 物品类型
        /// </summary>
        public GoodsType GoodsType { get; set; }

        /// <summary>
        /// 期望出售价格
        /// </summary>
        public decimal SellPrice { get; set; }

        /// <summary>
        /// 物品描述
        /// </summary>
        public string SellDescribe { get; set; }
    }
}
