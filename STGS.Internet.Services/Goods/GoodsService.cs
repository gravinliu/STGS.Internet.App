using STGS.Internet.Core;
using STGS.Internet.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace STGS.Internet.Services.Goods
{
    public class GoodsService : IGoodsService
    {
        protected IGoodsRepository GoodsRepository { get; set; }

        public GoodsService(IGoodsRepository goodsRepository)
        {
            GoodsRepository = goodsRepository;
        }

        public async Task<Tuple<int, IList<GoodsInfo>>> GetGoods(GoodsInput input)
        {
            return await GoodsRepository.GetListAsync(input);
        }

        public async Task<GoodsInfo> GetGoodsAsync(string Id)
        {
            return await GoodsRepository.GetAsync(Id);
        }
    }
}
