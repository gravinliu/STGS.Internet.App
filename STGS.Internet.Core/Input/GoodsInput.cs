using Dapper;
using Sino.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace STGS.Internet.Core
{
    public class GoodsInput : QueryObject<GoodsInfo>
    {
        /// <summary>
        /// 关键字查询
        /// </summary>
        public string KeyWord { get; set; }

        /// <summary>
        /// 物品类型
        /// </summary>
        public GoodsType GoodsType { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public string orderBy { get; set; }

        /// <summary>
        /// 升序或者降序
        /// </summary>
        public int Asc { get; set; } = 0;

        public override List<Expression<Func<GoodsInfo, bool>>> QueryExpression
        {
            get;
        }

        public override Dictionary<string, DynamicParameters> QuerySql
        {
            get
            {
                DynamicParameters param = new DynamicParameters();
                StringBuilder sql = new StringBuilder();
                bool keyWordIsNNull = !string.IsNullOrWhiteSpace(KeyWord);
                bool GoodsTypeIsNull = GoodsType != GoodsType.None;
                bool orderByIsNNull = !string.IsNullOrWhiteSpace(orderBy);
                bool acsIsNNull = Asc != 0;
                if (keyWordIsNNull)
                {
                    sql.Append(" AND g.GoodsName like @KeyWord");
                    param.Add("@KeyWord", "%" + KeyWord + "%");
                }
                if (GoodsTypeIsNull)
                {
                    sql.Append(" AND g.GoodsType=@GoodsType");
                    param.Add("@GoodsType", GoodsType);
                }
                if (orderByIsNNull)
                {
                    sql.AppendFormat(" ORDER BY `{0}` ", orderBy);
                    if (acsIsNNull)
                    {
                        if (Asc == 1)
                        {
                            sql.Append("ASC");
                        }
                        else
                        {
                            sql.Append("DESC");
                        }
                    }
                }
                else
                {
                    sql.AppendFormat(" ORDER BY g.UpdateTime DESC");
                }
                Dictionary<string, DynamicParameters> querySql = new Dictionary<string, DynamicParameters>();
                querySql.Add(sql.ToString(), param);
                return querySql;

            }
        }
    }
}
