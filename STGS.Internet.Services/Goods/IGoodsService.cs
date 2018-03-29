using STGS.Internet.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace STGS.Internet.Services.Goods
{
    public interface IGoodsService
    {
        /// <summary>
        /// 获取商品列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<Tuple<int, IList<GoodsInfo>>> GetGoods(GoodsInput input);

        /// <summary>
        /// 获取单个商品详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<GoodsInfo> GetGoodsAsync(string Id);
    }
}
