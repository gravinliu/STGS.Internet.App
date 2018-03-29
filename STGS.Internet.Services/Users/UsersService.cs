using STGS.Internet.Core;
using STGS.Internet.Repositories.IRepository;
using System;
using System.Threading.Tasks;

namespace STGS.Internet.Services.Users
{
    public class UsersService : IUsersService
    {
        protected IUsersRepository UsersRepository { get; set; }

        public UsersService(IUsersRepository usersRepository)
        {
            UsersRepository = usersRepository;
        }

        public async Task<UserInfo> FindByLoginKeyAsync(string KeyWord)
        {
            return await UsersRepository.FindByLoginKeyAsync(KeyWord);
        }

        public async Task<ReturnValue> CreateAsync(UserInfo user)
        {
            var result = await UsersRepository.CreateAsync(user);
            if (!result.IsSuccess)
            {
                throw new Exception(ExceptionCode.EC103);
            }
            return await Task.FromResult(result);
        }
    }
}
