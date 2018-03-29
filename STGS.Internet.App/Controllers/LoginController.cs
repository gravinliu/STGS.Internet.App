using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using STGS.Internet.App.ViewModels;
using STGS.Internet.App.ViewResponse;
using STGS.Internet.Core;
using STGS.Internet.Repositories.TokenMethod;
using STGS.Internet.Services.Users;
using STGS.Internet.Tool;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace STGS.Internet.App.Controllers
{
    [Route("api/[controller]")]
    public class LoginController : Controller
    {

        private readonly TokenAuthOption _jwtParametersOptions;
        protected IUsersService UsersService { get; set; }
        public IConfigurationRoot Configuration { get; }
        public LoginController(IOptions<TokenAuthOption> jwtParametersOptions, IUsersService usersService, IHostingEnvironment env)
        {
            _jwtParametersOptions = jwtParametersOptions.Value;
            UsersService = usersService;
            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
               .AddEnvironmentVariables();
            Configuration = builder.Build();
        }


        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="login">传入登陆对象</param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<LoginResponse> Login([FromBody]LoginVM login)
        {
            //查找用户是否已注册
            var user = await UsersService.FindByLoginKeyAsync(login.LoginKey);
            if(user == null)
            {
                throw new Exception(ExceptionCode.EC101);
            }
            if(user.Password == Encryption.MD5Hash(login.PassWordHash))
            {
                //生成JWT Token
                var now = DateTime.Now;
                var claims = new Claim[]
                {
                new Claim(JwtRegisteredClaimNames.Sub,"STGS"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sid, user.Id),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtParametersOptions.Expiration).ToString(), ClaimValueTypes.Integer64)
                };
                var jwt = new JwtSecurityToken(
                    issuer: _jwtParametersOptions.Issuer,
                    audience: _jwtParametersOptions.Audience,
                    claims: claims,
                    notBefore: _jwtParametersOptions.NotBefore,
                    expires: _jwtParametersOptions.Expiration,
                    signingCredentials: _jwtParametersOptions.SigningKey
                );
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                var identity = new ClaimsIdentity(claims, "app");
                //将用户数据放入Identity
                User.AddIdentity(identity);
                var response = new LoginResponse()
                {
                    JwtToken = encodedJwt,
                    TokenExpirationDate = ToUnixEpochDate(_jwtParametersOptions.Expiration),
                    UserName = user.UserName,
                    UserId = user.Id,
                    PhoneNumber = user.PhoneNumber,
                };
                return await Task.FromResult(response);
            }
            else
            {
                throw new Exception(ExceptionCode.EC105);
            }

        }
        private static long ToUnixEpochDate(DateTime date)
     => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <returns></returns>
        [HttpPost("Regesiter")]
        public async Task<string> Regesiter([FromBody]RegesiterVM regesiter)
        {
            var user1 = await UsersService.FindByLoginKeyAsync(regesiter.PhoneNumber);
            var user2 = await UsersService.FindByLoginKeyAsync(regesiter.Email);
            if(user1 != null || user2 != null)
            {
                throw new Exception(ExceptionCode.EC102);
            }
            var Id = Guid.NewGuid().ToString();
            var newuser = new UserInfo
            {
                Id = Id,
                UserName = regesiter.UserName,
                PhoneNumber = regesiter.PhoneNumber,
                Password = Encryption.MD5Hash(regesiter.Password),
                Sex = regesiter.Sex,
                Email = regesiter.Email,
                QqNumber = regesiter.QqNumber,
                SendGoodsNumber = 0,
                BuyGoodsNumber = 0,
                SchoolName = regesiter.SchoolName,
                Address = regesiter.Address,
                SystemRole = SysRole.User,
                IsDelete = YesOrNo.No,
                CreationTime = DateTime.Now
            };
            var result = await UsersService.CreateAsync(newuser);
            if (!result.IsSuccess)
            {
                throw new Exception(ExceptionCode.EC103);
            }
            return ExceptionCode.EC104;
        }

    }
}
