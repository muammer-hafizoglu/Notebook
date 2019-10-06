using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Notebook.Business.Managers.Abstract;
using Notebook.Business.Managers.Concrete;
using Notebook.Business.Tools.Logging;
using Notebook.Business.Tools.Mail;
using Notebook.Core.Aspects.SimpleProxy.Caching;
using Notebook.Core.Aspects.SimpleProxy.Logging;
using Notebook.Core.Aspects.SimpleProxy.Validation;
using Notebook.Core.CrossCuttingConcerns.Logging;
using Notebook.DataAccess.DataAccess.Abstract;
using Notebook.DataAccess.DataAccess.Concrete.EntityFramework;
using Notebook.DataAccess.DataContext;
using Notebook.Web.Filters;
using Notebook.Web.Middlewares;
using Notebook.Web.Tools.FileManager;
using SimpleProxy.Extensions;
using SimpleProxy.Strategies;
using System;
using System.Globalization;
using System.IO;
//using AutoMapper.Extensions.Microsoft.DependencyInjection;

namespace Notebook.Web
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
            #region DbContext

            services.AddDbContext<NotebookContext>(options => options.UseMySql(Configuration.GetConnectionString("NotebookContext")));
            services.AddScoped<DbContext, NotebookContext>();

            #endregion

            #region Aspects

            services.EnableSimpleProxy(p => p
                .AddInterceptor<ValidateAttribute, ValidateInterceptor>()
                .AddInterceptor<CacheAttribute, CacheInterceptor>()
                .AddInterceptor<LogAttribute, LogInterceptor>()
                .WithOrderingStrategy<PyramidOrderStrategy>());

            services.AddScoped<ILoggerService, FileLogger>();
            services.AddScoped<LogFilterAttribute>();
            services.AddScoped<AccountFilterAttribute>();
            services.AddScoped<ExceptionFilterAttribute>();
            services.AddScoped<IFileManager,FileManager>();
            services.AddScoped<IMailExtension, MailExtension>();
            #endregion

            #region Managers

            services.AddScoped<IUserDal, EfUserDal>();
            services.AddScoped<IUserManager, UserManager>();

            services.AddScoped<ILogDal, EfLogDal>();
            services.AddScoped<ILogManager, LogManager>();

            services.AddScoped<IGroupDal, EfGroupDal>();
            services.AddScoped<IGroupManager, GroupManager>();

            services.AddScoped<IFolderDal, EfFolderDal>();
            services.AddScoped<IFolderManager, FolderManager>();

            services.AddScoped<INoteDal, EfNoteDal>();
            services.AddScoped<INoteManager, NoteManager>();

            services.AddScoped<IUserGroupDal, EfUserGroupDal>();
            services.AddScoped<IUserGroupManager, UserGroupManager>();

            services.AddScoped<IUserNoteDal, EfUserNoteDal>();
            services.AddScoped<IUserNoteManager, UserNoteManager>();

            services.AddScoped<IFollowDal, EfFollowDal>();
            services.AddScoped<IFollowManager, FollowManager>();

            services.AddScoped<IEventDal, EfEventDal>();
            services.AddScoped<IEventManager, EventManager>();

            services.AddScoped<ISettingsDal, EfSettingsDal>();
            services.AddScoped<ISettingsManager, SettingsManager>();

            services.AddScoped<ICalendarDal, EfCalendarDal>();
            services.AddScoped<ICalendarManager, CalendarManager>();
            #endregion

            #region Session

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);
                options.CookieHttpOnly = true;
            });

            //services.AddDistributedMemoryCache();
            services.AddMemoryCache();

            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => false;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            #endregion

            #region Multi Language

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US"),
                    new CultureInfo("tr-TR")
                };

                options.DefaultRequestCulture = new RequestCulture(culture: "tr-TR", uiCulture: "tr-TR");

                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            #endregion

            #region AutoMapper

            services.AddAutoMapper();

            #endregion

            #region Social Login

            services.AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = Configuration.GetSection("Authentication").GetValue<string>("GoogleClientId");
                googleOptions.ClientSecret = Configuration.GetSection("Authentication").GetValue<string>("GoogleClientSecret");
            })
            .AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = Configuration.GetSection("Authentication").GetValue<string>("FacebookClientId");
                facebookOptions.AppSecret = Configuration.GetSection("Authentication").GetValue<string>("FacebookClientSecret");
            })
            .AddCookie();

            #endregion
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @".well-known")),
            //    RequestPath = new PathString("/.well-known"),
            //    ServeUnknownFileTypes = true // serve extensionless file
            //});
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
