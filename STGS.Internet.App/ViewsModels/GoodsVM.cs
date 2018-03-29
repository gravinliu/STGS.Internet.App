namespace STGS.Internet.App.ViewModels
{
    /// <summary>
    /// 发布商品模型
    /// </summary>
    public class GoodsVM
    {
        /// <summary>
        /// 商品Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 发布人Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 物品名称
        /// </summary>
        public string GoodsName { get; set; }

        /// <summary>
        /// 物品类型
        /// </summary>
        public string GoodsType { get; set; }

        /// <summary>
        /// 期望出售价格
        /// </summary>
        public decimal SellPrice { get; set; }

        /// <summary>
        /// 物品描述
        /// </summary>
        public string SellDescribe { get; set; }
       
        /// <summary>
        /// 发布人联系方式
        /// </summary>
        public string UserPhone { get; set; }
    }
}
