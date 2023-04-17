using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FPTMallsProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FPTMallsProject
{
    public class Startup
    {
        private readonly IConfiguration _config;
        
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession(
                options =>
                {
                    options.IdleTimeout = TimeSpan.FromMinutes(10);
                }
             );

            string path = Directory.GetCurrentDirectory();
            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(_config.GetConnectionString("AppDB").Replace("[Dir]", path))
            );

            services.AddIdentity<IdentityUser, IdentityRole>(
                options => {
                    options.Password.RequiredUniqueChars = 3;
                    options.Password.RequireNonAlphanumeric = false;
                }
            ).AddEntityFrameworkStores<AppDbContext>()
			    .AddDefaultTokenProviders(); 

            services.AddControllersWithViews();

            services.AddScoped<IMovieRepository, SQLMovieRepository>();

            services.AddScoped<IShowRepository, SQLShowRepository>();

            services.AddScoped<IBookingRepository, SQLBookingRepository>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
			// Gọi phương thức tạo user admin
			CreateAdminUser(userManager, roleManager).Wait();


			if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });


		}


        //Seed UserAdmin
        private async Task CreateAdminUser(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			//Tạo role Admin
			string roleName = "Admin";
			if (await roleManager.FindByNameAsync(roleName) == null)
			{
				await roleManager.CreateAsync(new IdentityRole(roleName));
			}

			//Tạo user admin
			string userName = "admin@example.com";
			IdentityUser user = await userManager.FindByNameAsync(userName);

			if (user == null)
			{
				user = new IdentityUser
				{
					UserName = userName,
					Email = userName
				};
				await userManager.CreateAsync(user, "StrongPassword1!");
			}

			//Gán quyền admin cho user admin
			if (!await userManager.IsInRoleAsync(user, roleName))
			{
				await userManager.AddToRoleAsync(user, roleName);
			}

		}

	}
}
