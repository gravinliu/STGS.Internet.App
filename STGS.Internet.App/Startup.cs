using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NLog;
using NLog.Extensions.Logging;
using Sino.Web.Filter;
using Sino.Web.ViewModels;
using STGS.Internet.Core;
using STGS.Internet.Repositories;
using STGS.Internet.Repositories.IRepository;
using STGS.Internet.Repositories.TokenMethod;
using STGS.Internet.Services.Goods;
using STGS.Internet.Services.Shopping;
using STGS.Internet.Services.Users;
using STGS.Internet.Tool;

namespace STGS.Internet.App
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;
        public Startup(IHostingEnvironment env)
        {
            _env = env;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IServiceProvider Services { get; set; }
        private const string SecretKey = "5E9B1DFB4152415BAC6FF4CA6EEA42A8";
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);


            services.Configure<FormOptions>(x =>
            {
                //x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
            });
            services.AddDapper(Configuration.GetConnectionString("WriteConnection"), Configuration.GetConnectionString("ReadConnection"));


            //服务层注入
            services.AddTransient<IGoodsService, GoodsService>();
            services.AddTransient<IShoppingService, ShoppingService>();
            services.AddTransient<IUsersService, UsersService>();


            //仓储层注入
            services.AddTransient<IGoodsRepository, GoodsRepository>();
            services.AddTransient<IShoppingRepository, ShoppingRepository>();
            services.AddTransient<IUsersRepository, UsersRepository>();

            CheckSignatureFilter.TimeOut = 10;
            CheckSignatureFilter.Token = Configuration["Token"];
            services.AddMvc(x =>
            {
                x.Filters.Add(typeof(GlobalResultFilter));
            });
            services.AddOptions();
            services.AddAuthorization();
            if (_env.IsDevelopment())
            {
                services.AddSwaggerGen(c =>
                {
                    c.SingleApiVersion(new Swashbuckle.Swagger.Model.Info
                    {
                        Version = "v1",
                        Title = "STGS Api说明",
                        Description = "Api说明以及测试",
                        TermsOfService = "stgs",
                    });
                });

            }

            var jwtSection = Configuration.GetSection(nameof(TokenAuthOption));

            services.Configure<TokenAuthOption>(options =>
            {
                options.Audience = jwtSection[nameof(TokenAuthOption.Audience)];
                options.Issuer = jwtSection[nameof(TokenAuthOption.Issuer)];
                options.SigningKey = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });
            Services = services.BuildServiceProvider();
            return Services;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddNLog();
            var jwtSection = Configuration.GetSection(nameof(TokenAuthOption));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                ValidateIssuer = true,
                ValidIssuer = jwtSection[nameof(TokenAuthOption.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtSection[nameof(TokenAuthOption.Audience)],

                RequireExpirationTime = true,
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };

            var bearerOptions = new JwtBearerOptions()
            {
                AuthenticationScheme = "Automatic",
                AutomaticAuthenticate = true,
                TokenValidationParameters = tokenValidationParameters,
                Events = new JwtBearerOverrideEvents()
            };
            if (env.IsDevelopment())
            {
                app.UseCors(builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowCredentials();
                });
                app.UseDeveloperExceptionPage();


            }
            app.UseGlobalExceptionHandler(LogManager.GetCurrentClassLogger());
            app.UseMvc();
            app.UseMvcWithDefaultRoute();
            app.UseSwagger();
            app.UseSwaggerUi();
            app.UseStaticFiles();

        }
    }
    /// <summary>
    /// 重写JWT触发函数
    /// </summary>
    public class JwtBearerOverrideEvents : JwtBearerEvents
    {
        /// <summary>
        /// JwtToken失效触发
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task AuthenticationFailed(AuthenticationFailedContext context)
        {
            context.State = Microsoft.AspNetCore.Authentication.EventResultState.Skipped;
            return Task.CompletedTask;
        }
        /// <summary>
        /// 没有JwtToken时触发
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task Challenge(JwtBearerChallengeContext context)
        {
            context.Response.Clear();
            context.Response.StatusCode = 200;
            context.Response.ContentType = "application/json";
            BaseResponse response = new BaseResponse()
            {
                success = false,
                errorCode = nameof(ExceptionCode.EC107).GetCode().ToString(),
                errorMessage = ExceptionCode.EC107
            };
            context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            context.HandleResponse();
            return base.Challenge(context);
        }
    }
}
