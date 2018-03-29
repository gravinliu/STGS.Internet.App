using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STGS.Internet.App.ViewModels;
using STGS.Internet.App.ViewResponse;
using STGS.Internet.Core;
using STGS.Internet.Services.Goods;
using STGS.Internet.Tool;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace STGS.Internet.App.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class GoodsController : BaseController
    {

        protected IGoodsService GoodsService { get; set; }
        public GoodsController(IGoodsService goodsService)
        {
            GoodsService = goodsService;
        }

        /// <summary>
        /// 获取所有商品信息
        /// </summary>
        /// <param name="KeyWord">商品名称关键字</param>
        /// <param name="type">商品类型</param>
        /// <param name="OrderBy">排序字段</param>
        /// <param name="Asc">升序或降序</param>
        /// <param name="Skip">跳过数量</param>
        /// <param name="Count">显示数量</param>
        /// <returns></returns>
        [HttpGet("GetGoodsInfo")]
        [AllowAnonymous]
        public async Task<GoodsViewResponse> GetGoodsInfo(string KeyWord, GoodsType type, string OrderBy, int Asc = 0, int Skip = 0, int Count = 10)
        {
            var input = new GoodsInput()
            {
                KeyWord = KeyWord,
                GoodsType = type,
                orderBy = OrderBy,
                Asc = Asc,
                Skip = Skip,
                Count = Count
            };
            var list = await GoodsService.GetGoods(input);
            var response = new GoodsViewResponse()
            {
                Data = list.Item2.Select(x => new GoodsVM()
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    GoodsName = x.GoodsName,
                    GoodsType = x.GoodsType.GetCustomAttribute<EnumTextMetaAttribute>().Text,
                    SellPrice = x.SellPrice,
                    SellDescribe = x.SellDescribe,
                    UserPhone = x.UserInfo.PhoneNumber
                }).ToList(),
                Total = list.Item1               
            };
            return response;
        }


        /// <summary>
        /// 获取单个商品信息详情
        /// </summary>
        /// <param name="GoodsId">商品Id</param>
        /// <returns></returns>
        [HttpGet("GetGoodsAsync")]
        public async Task<GoodsVM> GetGoodsAsync(string GoodsId)
        {
            var good = await GoodsService.GetGoodsAsync(GoodsId);
            var response = new GoodsVM()
            {
                Id = good.Id,
                GoodsName = good.GoodsName,
                GoodsType = good.GoodsType.GetCustomAttribute<EnumTextMetaAttribute>().Text,
                SellDescribe = good.SellDescribe,
                SellPrice = good.SellPrice,
                UserId = good.UserId,
                UserPhone = good.UserInfo.PhoneNumber
            };
            return response;
        }

        /// <summary>
        /// 发布商品
        /// </summary>
        /// <param name="good"></param>
        /// <returns></returns>
        [HttpPost("AddGoods")]
        public async Task AddGoods([FromBody]ReleaseVM good)
        {
            var result = new GoodsInfo()
            {
                Id = Guid.NewGuid().ToString(),
                
            };
        }

    }
}
