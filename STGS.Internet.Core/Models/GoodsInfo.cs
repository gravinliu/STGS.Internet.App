using Sino.Domain.Entities;
using System;

namespace STGS.Internet.Core
{
    public class GoodsInfo : Entity<string>
    {

        /// <summary>
        /// 卖家Id
        /// </summary>
        public string UserId { get; set; }

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

        /// <summary>
        /// 是否删除
        /// </summary>
        public YesOrNo IsDelete { get; set; }

        /// <summary>
        /// 更新发布时间
        /// </summary>
        public DateTime UpdateTime { get; set; }


        public virtual UserInfo UserInfo { get; set; }
    }
}
