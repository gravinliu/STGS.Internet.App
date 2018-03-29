using STGS.Internet.Core;
using System.Threading.Tasks;

namespace STGS.Internet.Services.Users
{
    public interface IUsersService
    {
        /// <summary>
        /// 根据手机号或邮箱地址查找用户
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        Task<UserInfo> FindByLoginKeyAsync(string keyword);

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<ReturnValue> CreateAsync(UserInfo user);
    }
}
