using JWT.Builder;
using Logic.Interfaces;

using Logic.ProductLogic;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Repository.Functions;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User_Prototype
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            var jwtSettings = new Logic.Options.JWTSettings();
            Configuration.GetSection("JWTSettings").Bind(jwtSettings);
            services.AddSingleton(jwtSettings);

            var sessionCookieLifetime = Configuration.GetValue("SessionCookieLifetime", 60);
            services.AddScoped<IToken, TokenRepo>();
            services.AddScoped<IUser, UserRepo>();
            services.AddScoped<IUserInterfaces, UserService>();
            

            var facebookSettings = new Repository.FacebookSettings.FacebookAuthSettings();
            Configuration.GetSection("FacebookAuthSettings").Bind(facebookSettings);
            services.AddSingleton(facebookSettings);
            services.AddHttpClient("APIClient", client =>
            {
                client.BaseAddress = new Uri("https://localhost:5201/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            });
            services.AddSingleton<IFacebookInterface, FacebookRepo>();

            services.AddRouting();
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            var tokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true,

            };
            services.AddSingleton(tokenValidationParameters);
            services.AddAuthentication(option =>
            {
                
                option.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            }
            ).AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = tokenValidationParameters;
            }
            ).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, setup => setup.ExpireTimeSpan = TimeSpan.FromMinutes(sessionCookieLifetime)
            ).AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, option =>
            {
                option.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                option.Authority = "https://localhost:5101/";
                option.ClientId = "pokolokoshop";
                option.ResponseType = "code";
                option.UsePkce = false;
                option.Scope.Add("openid");
                option.Scope.Add("profile");
                option.SaveTokens = true;
                option.ClientSecret = "secret";
                //option.CallbackPath = new PathString("...");
            });
            services.AddControllers();
            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
        }

        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
               
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseHttpsRedirection();
        }
    }
}
