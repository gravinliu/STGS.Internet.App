using Dapper;
using Sino.Dapper;
using Sino.Dapper.Repositories;
using Sino.Domain.Repositories;
using STGS.Internet.Core;
using STGS.Internet.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STGS.Internet.Repositories
{
    public class GoodsRepository : DapperRepositoryBase<GoodsInfo, string>, IGoodsRepository
    {
        public GoodsRepository(IDapperConfiguration configuration) : base(configuration, true)
        {

        }

        public override async Task<Tuple<int, IList<GoodsInfo>>> GetListAsync(IQueryObject<GoodsInfo> query)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("select g.*, u.* from goodsinfo g LEFT JOIN userinfo u ON g.UserId=u.Id WHERE 1=1 and g.IsDelete=0 ");
            StringBuilder sbCount = new StringBuilder();
            sbCount.Append("select COUNT(g.Id) from goodsinfo g LEFT JOIN userinfo u ON g.UserId=u.Id WHERE 1=1 and g.IsDelete=0 ");
            var queryandsort = query.QuerySql.FirstOrDefault();
            if (query.QuerySql.Count > 0)
            {
                sb.Append(queryandsort.Key);
                sbCount.Append(queryandsort.Key);
            }
            if (query.Count <= 0)
            {
                query.Count = int.MaxValue;
            }
            sb.Append(" LIMIT " + query.Skip + "," + query.Count);
            using (ReadConnection)
            {
                var GoodsList = await ReadConnection.QueryAsync<GoodsInfo, UserInfo, GoodsInfo>(sb.ToString(), (goodsinfo, usersinfo) =>
                  {
                      if (goodsinfo != null)
                      {
                          goodsinfo.UserInfo = usersinfo;
                      }
                      return goodsinfo;
                  }, queryandsort.Value);
                int totalCount = await ReadConnection.QuerySingleAsync<int>(sbCount.ToString(), queryandsort.Value);
                return new Tuple<int, IList<GoodsInfo>>(totalCount, GoodsList.ToList());
            }
        }
    }
}
