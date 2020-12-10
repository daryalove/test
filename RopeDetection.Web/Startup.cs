using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RopeDetection.Entities.Configuration;
using RopeDetection.Entities.DataContext;
using RopeDetection.Entities.Logging;
using RopeDetection.Entities.Repository;
using RopeDetection.Entities.Repository.Base;
using RopeDetection.Entities.Repository.Interfaces;
using RopeDetection.Services.Interfaces;
using RopeDetection.Services.UserService;
using RopeDetection.Services.RopeService;
using RopeDetection.Web.AuthHelpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace RopeDetection.Web
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
            ConfigureAspnetRunServices(services);
            services.AddCors();
            services.AddControllers();
            services.AddControllersWithViews();
            //        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //.AddCookie(options =>
            //{

            //    //options.LoginPath = "/Admin/Login";
            //    //options.
            //});
            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddCookie()
            .AddJwtBearer(x =>
            {
                //x.Events = new JwtBearerEvents
                //{
                //    OnTokenValidated = context =>
                //    {
                //        var userService = context.HttpContext.RequestServices.GetRequiredService<IAuthService>();
                //        Guid userId;
                //        var result = Guid.TryParse(context.Principal.Identity.Name, out userId);
                //        var user = userService.GetUser(userId);
                //        if (user != null)
                //        {
                //            // return unauthorized if user no longer exists
                //            context.Fail("Unauthorized");
                //        }
                //        return Task.CompletedTask;
                //    }
                //};
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthorization(options => options.AddPolicy("editContent", policyBuilder =>
            {
                policyBuilder.RequireAuthenticatedUser()
                    .RequireAssertion(context => context.User.HasClaim("CanEditContent", "true"))
                    .Build();
            }));

        }

        private void ConfigureAspnetRunServices(IServiceCollection services)
        {
        //    services.AddControllers()
        //.AddControllersAsServices();
            // Add Core Layer
            // configure strongly typed settings object
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            //services.Configure<BaseProjectSettings>(Configuration.GetSection("BaseProjectSettings"));

            // Add Infrastructure Layer
            ConfigureDatabases(services);

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IModelTypeRepository, ModelTypeRepository>();
            services.AddScoped<IModelRepository, ModelRepository>();
            services.AddScoped<IModelObjectRepository, ModelObjectRepository>();
            //services.AddScoped<IFileDataRepository, FileDataRepository>();
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));

            // Add Application Layer
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IModelService, ModelService>();
            //services.AddScoped<IFileDataService, FileDataService>();

            // Add Web Layer
            services.AddAutoMapper(typeof(Startup)); // Add AutoMapper
            /*services.AddScoped<IIndexPageService, IndexPageService>();
            services.AddScoped<IProductPageService, ProductPageService>();
            services.AddScoped<ICategoryPageService, CategoryPageService>();*/

            // Add Miscellaneous
            services.AddHttpContextAccessor();
            /* services.AddHealthChecks()
                 .AddCheck<IndexPageHealthCheck>("home_page_health_check");*/
        }
        
        public void ConfigureDatabases(IServiceCollection services)
        {
            // use in-memory database
            /*  services.AddDbContext<AspnetRunContext>(c =>
                  c.UseInMemoryDatabase("AspnetRunConnection"));
                  */
            //// use real database
            services.AddDbContext<ModelContext>(c =>
              c.UseSqlServer(Configuration.GetConnectionString("BaseProjectConnection"), b => b.MigrationsAssembly("RopeDetection.Entities")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ModelContext context)
        {
            context.Database.Migrate();
            BaseProjectContextSeed.SeedLabels(context);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
