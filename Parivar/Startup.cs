using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Parivar.Data.DbContext;
using Parivar.Repository.Interface;
using Parivar.Repository.Service;
using Parivar.Repository.Utility;
using Parivar.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;

namespace Parivar
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
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var cultures = new List<CultureInfo> {
                    new CultureInfo("en"),
                    new CultureInfo("gu")
                };
                options.DefaultRequestCulture = new RequestCulture("gu");
                options.SupportedCultures = cultures;
                options.SupportedUICultures = cultures;
            });



            services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton(Configuration);
            services.AddDbContext<ParivarDb>(options => options.UseSqlServer(Configuration.GetConnectionString("LocalConnection")));
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //services.ini
            services.AddIdentity<FamilyUser, Role>()
                .AddRoles<Role>()
                .AddEntityFrameworkStores<ParivarDb>()
              .AddDefaultTokenProviders();
            services.AddScoped<RoleManager<Role>>();

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages().AddRazorRuntimeCompilation();

            services.AddScoped<IUserClaimsPrincipalFactory<FamilyUser>, ClaimsPrincipalFactory>();

            #region Identity Configuration
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            //Seting the Account Login page
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.LoginPath = "/Account/Login"; // If the LoginPath is not set here, ASP.NET Core will default to /Account/Login
                options.LogoutPath = "/Account/Logout"; // If the LogoutPath is not set here, ASP.NET Core will default to /Account/Logout
                options.AccessDeniedPath = "/Account/AccessDenied"; // If the AccessDeniedPath is not set here, ASP.NET Core will default to /Account/AccessDenied
                options.SlidingExpiration = true;
            });
            //Seting the Post Configure
            services.PostConfigure<CookieAuthenticationOptions>(IdentityConstants.ApplicationScheme, options =>
            {
                //configure your other properties
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.LoginPath = "/Account/Login"; // If the LoginPath is not set here, ASP.NET Core will default to /Account/Login
                options.LogoutPath = "/Account/Logout"; // If the LogoutPath is not set here, ASP.NET Core will default to /Account/Logout
                options.AccessDeniedPath = "/Account/AccessDenied"; // If the AccessDeniedPath is not set here, ASP.NET Core will default to /Account/AccessDenied
                options.SlidingExpiration = true;
            });
            services.Configure<CookieTempDataProviderOptions>(options =>
            {
                options.Cookie.IsEssential = true;
            });
            #endregion

            services.AddAuthentication();

            #region Dependency Injection
            #region _A_
            #endregion

            #region _B_
            #endregion

            #region _C_
            services.AddScoped<ICategoriesMasterService, CategoriesMasterRepository>();
            services.AddScoped<ICityService, CityRepository>();
            services.AddScoped<IContactUsService, ContactUsRepository>();
            services.AddScoped<ICountyService, CountyRepository>();
            #endregion

            #region _D_
            services.AddScoped<IDistrictService, DistrictRepository>();
            #endregion

            #region _E_
            services.AddScoped<IErrorLogService, ErrorLogRepository>();
            #endregion

            #region _F_
            services.AddScoped<IFamilyUserService, FamilyUserRepository>();
            services.AddScoped<IFamilyMemberDetailsService, FamilyMemberDetailsRepository>();
            #endregion

            #region _G_
            #endregion

            #region _H_
            #endregion

            #region _I_

            #endregion

            #region _J_
            #endregion

            #region _K_
            #endregion

            #region _L_
            #endregion

            #region _M_
            #endregion

            #region _N_
            #endregion

            #region _O_
            #endregion

            #region _P_
            #endregion

            #region _Q_
            #endregion

            #region _R_
            services.AddScoped<IRelationShipMasterService, RelationShipMasterRepository>();
            #endregion

            #region _S_
            services.AddScoped<IStateService, StateRepository>();
            #endregion

            #region _T_
            #endregion

            #region _U_
            #endregion

            #region _V_
            services.AddScoped<IVillageService, VillageRepository>();
            #endregion

            #region _W_
            #endregion

            #region _X_
            #endregion

            #region _Y_
            #endregion

            #region _Z_
            #endregion
            #endregion



            services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
            /**Add Automapper**/
            services.AddAutoMapper(typeof(Startup));
            services.AddSession(opts =>
            {
                opts.Cookie.IsEssential = true; // make the session cookie Essential
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
            .AddSessionStateTempDataProvider();
            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
            services.AddSession();

            #region Configure App Settings

            /**Email Settings**/
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.Configure<EmailSettingsGmail>(Configuration.GetSection("EmailSettingsGmail"));

            /**Settings**/
            services.Configure<AppSettings>(Configuration.GetSection("Appsettings"));

            #endregion

            #region Web Jobs

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<FamilyUser> userManager, RoleManager<Role> roleManager, IRelationShipMasterService relationShip)
        {
            app.UseDeveloperExceptionPage();
            app.UseDatabaseErrorPage();

            if (env.IsDevelopment() || env.IsStaging())
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


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            //app.UseCookiePolicy();
            app.UseSession();
            app.UseRequestLocalization(app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(name: "Admin", areaName: "Admin", pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(name: "Member", areaName: "Member", pattern: "Member/{controller=Dashboard}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            ParivarIdentityDataInitializer.SeedData(userManager, roleManager, relationShip);
            //RotativaConfiguration.Setup(env);
        }
    }

}
