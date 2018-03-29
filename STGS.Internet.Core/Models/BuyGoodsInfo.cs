using Sino.Domain.Entities;
using System;

namespace STGS.Internet.Core
{
    public class BuyGoodsInfo : Entity<string>
    {

        /// <summary>
        /// 物品Id
        /// </summary>
        public string GoodsId { get; set; }

        /// <summary>
        /// 买家Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 订单是否删除
        /// </summary>
        public YesOrNo IsDelete { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 购买状态
        /// </summary>
        public BuyState BuyState { get; set; }


        /// <summary>
        /// 是否收藏
        /// </summary>
        public YesOrNo Collection { get; set; }


        /// <summary>
        /// 是否添加到购物车
        /// </summary>
        public YesOrNo ShoppingCart { get; set; }
    }
}
