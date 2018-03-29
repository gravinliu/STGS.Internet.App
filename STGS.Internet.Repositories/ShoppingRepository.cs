using Sino.Dapper;
using Sino.Dapper.Repositories;
using STGS.Internet.Core;
using STGS.Internet.Repositories.IRepository;

namespace STGS.Internet.Repositories
{
    public class ShoppingRepository : DapperRepositoryBase<BuyGoodsInfo, string>, IShoppingRepository
    {
        public ShoppingRepository(IDapperConfiguration configuration) : base(configuration, true)
        {

        }


    }
}
