using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PokolokoShop_User.Data;
using System;
using Logic.Interfaces;
using Logic.ProductLogic;
using Repository.FacebookSettings;
using PokolokoShop_User.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Logic.UserServices;

namespace PokolokoShop_User
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
            services.AddCors();
            var jwtSettings = new JWTSettings();
            var identityUrl = Configuration.GetValue<string>("IdentityUrl");
            var callbackUrl = Configuration.GetValue<string>("CallBackUrl");
            var sessionCookieLifetime = Configuration.GetValue("SessionCookieLifetime", 60);
            services.Configure<FacebookAuthSettings>(Configuration.GetSection("JWTSettings"));
            
            Configuration.GetSection("JWTSettings").Bind(jwtSettings);
            var facebookAuthSettings = new FacebookAuthSettings();
            services.Configure<FacebookAuthSettings>(Configuration.GetSection("FacebookAuthSettings"));
            Configuration.GetSection("FacebookAuthSettings").Bind(facebookAuthSettings);
            services.AddSingleton(facebookAuthSettings);
            services.AddSingleton(jwtSettings);
            services.AddSingleton<IFacebookInterface, FacebookRepo>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUserInterfaces, UserService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddRouting();
            services.AddControllers();
            services.AddRazorPages();
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddSession(option => option.IdleTimeout = TimeSpan.FromSeconds(60 * 90));
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x => { 
                x.SaveToken = true;
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = false,

                };
            }).AddCookie(setup => setup.ExpireTimeSpan = TimeSpan.FromMinutes(sessionCookieLifetime));
            
            
            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();
            
            services.AddHttpClient();
            
            
            
            
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors(
        options => options.WithOrigins("http://localhost:4200").AllowAnyMethod());
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
           
        }
    }
}
