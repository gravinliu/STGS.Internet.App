using Sino.Domain.Repositories;
using STGS.Internet.Core;
using System.Threading.Tasks;

namespace STGS.Internet.Repositories.IRepository
{
    public interface IUsersRepository : IRepository<UserInfo,string>
    {
        Task<UserInfo> FindByLoginKeyAsync(string KeyWord);

        Task<ReturnValue> CreateAsync(UserInfo user);
    }
}
