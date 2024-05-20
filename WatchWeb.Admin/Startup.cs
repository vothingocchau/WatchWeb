using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using WatchWeb.Service.IServices;
using WatchWeb.Service.Models.Mapper;
using WatchWeb.Service.Services;

namespace WatchWeb.Admin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<Model.EFContext.DataContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Database"));
            });
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            services.AddDistributedMemoryCache();

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddMvc();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = "/Login";
                options.LogoutPath = "/Logout";
                options.AccessDeniedPath = "/AccessDenied";
                options.ReturnUrlParameter = "/";
            });

            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IRoleService, RoleService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseRouting();
            app.UseEndpoints(endpoints => {

                endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                   name: "role",
                   pattern: "{controller=role}/{action=Index}/{id?}");
            });
            //app.UseMvc();
            //app.UseAuthorization();
            
        }
    }
}
