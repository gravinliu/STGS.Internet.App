using Dapper;
using Sino.Dapper;
using Sino.Dapper.Repositories;
using STGS.Internet.Core;
using STGS.Internet.Repositories.IRepository;
using System;
using System.Threading.Tasks;

namespace STGS.Internet.Repositories
{
    public class UsersRepository : DapperRepositoryBase<UserInfo,string>, IUsersRepository
    {
        public UsersRepository(IDapperConfiguration configuration) : base(configuration, true)
        {

        }


        public async Task<UserInfo> FindByLoginKeyAsync(string keyWord)
        {
            DynamicParameters param = new DynamicParameters();
            string sql = @"select * from userinfo
                         where PhoneNumber=@keyWord or Email=@KeyWord";
            param.Add("@keyWord", keyWord);
            param.Add("@KeyWord", keyWord);
            using (ReadConnection)
            {
                var result = await ReadConnection.QueryFirstOrDefaultAsync<UserInfo>(sql, param);
                return result;
            }

        }

        public async Task<ReturnValue> CreateAsync(UserInfo user)
        {
            using (WriteConnection)
            {
                var result = await WriteConnection.InsertAsync(user);
                if (result >= 0)
                {
                    return await Task.FromResult(new ReturnValue { IsSuccess = true });
                }
                else throw new Exception(ExceptionCode.EC103);
            }
        }

    }
}
