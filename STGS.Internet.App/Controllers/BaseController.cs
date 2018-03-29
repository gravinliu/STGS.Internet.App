using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using STGS.Internet.Core;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace STGS.Internet.App.Controllers
{
    public class BaseController : Controller
    {

        /// <summary>
        /// 用户ID
        /// </summary>
        public Claim SID { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public BaseController()
        {

        }

        /// <summary>
        /// 在进入方法之前 获取用户jwt中用户ID
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            SID = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sid);
            if (SID == null)
            {
                throw new Exception(ExceptionCode.EC106);
            }
            base.OnActionExecuting(context);
        }
    }
}
