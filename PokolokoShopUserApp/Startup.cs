// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.




using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace PokolokoShopUserApp
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            // uncomment, if you want to add an MVC-based UI
            
            services.AddControllersWithViews();
            var migrationAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            var builder = services.AddIdentityServer()
                //.AddInMemoryIdentityResources(Config.IdentityResources)
                //.AddInMemoryApiResources(Config.ApiResource)
                //.AddInMemoryApiScopes(Config.ApiScopes)
                //.AddInMemoryClients(Config.Clients)
                //.AddTestUsers(TestUsers.Users)
            .AddConfigurationStore(ops =>
             {
                 ops.ConfigureDbContext = builder => builder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), option => option.MigrationsAssembly(migrationAssembly));
             }); 


            // not recommended for production - you need to store your key material somewhere secure
            builder.AddDeveloperSigningCredential();
            
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            InitializeDatabase(app);
            // uncomment if you want to add MVC
            app.UseStaticFiles();
            app.UseRouting();
            
            app.UseIdentityServer();

            // uncomment, if you want to add MVC
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }


        private async void InitializeDatabase(IApplicationBuilder app)
        {
            using(var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                if(! await context.Clients.AnyAsync())
                {
                    foreach (var client in Config.Clients)
                    {
                        await context.Clients.AddAsync(client.ToEntity());
                    }
                    await context.SaveChangesAsync();
                }

                if (!await context.IdentityResources.AnyAsync())
                {
                    foreach (var resource in Config.IdentityResources)
                    {
                        await context.IdentityResources.AddAsync(resource.ToEntity());
                    }
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
