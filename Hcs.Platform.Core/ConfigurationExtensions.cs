using System;
using Hcs.Platform.Core;
using Hcs.Platform.Flow;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Hcs.Platform.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Hcs.Platform;
using Hcs.Platform.Security;
using Hcs.Platform.Policy;
using Microsoft.AspNetCore.Authorization;
using Hcs.Platform.User;
using Hcs.Platform.Role;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;
using Hcs.Platform.File;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ConfigurationExtensions
    {
        public static IMvcBuilder AddHcsPlatform(this IServiceCollection services, Action<Hcs.Platform.PlatformBuilder> config)
        {
            var mvcBuilder = services.AddMvc();
            mvcBuilder.AddJsonOptions(options => options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver());
            mvcBuilder.AddJsonOptions(options => options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc);
            mvcBuilder.AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            mvcBuilder.ConfigureApplicationPartManager(p => p.FeatureProviders.Add(new Hcs.Platform.Core.ApplicationFeatureProvider.ControllerFeatureProvider()));

            ConfigServices(services);
            //modules
            var builder = new Hcs.Platform.PlatformBuilder(mvcBuilder.Services);
            DefaultPlatformConfig.DefaultConfig(builder);
            config(builder);
            ConfigBuilderConfigService(services, builder);
            return mvcBuilder;
        }

        private static void ConfigBuilderConfigService(IServiceCollection services, PlatformBuilder builder)
        {
            services.AddSingleton(new PlatformConfigContext
            {
                JwtConfig = builder.JwtConfigBuilder
            });
            services.AddSingleton<Hcs.Platform.PlatformModule.IPlatformFunctionService>(builder);
            services.AddDbContext<PlatformDbContext>(builder.DbOptions);
            services.AddDistributedMemoryCache(options => builder.distributedMemoryCacheOptions?.Invoke(options));
            services.AddMemoryCache();
            ConfigAuthorizaion(services, builder);
            ConfigAuthentication(services, builder);
            ConfigDataProcessorTasks(services, builder.GenericDataProcessorEntityTasks, services.BuildServiceProvider());
        }

        private static void ConfigDataProcessorTasks(IServiceCollection services, IEnumerable<GenericDataProcessorEntityTaskDefinition> definitions, ServiceProvider sp)
        {
            var dpetInterface = typeof(IDataProcessorEntityTask<>);
            var entityTypes = services.Where(x => typeof(DbContext).IsAssignableFrom(x.ServiceType))
                .Select(x => (DbContext)sp.GetRequiredService(x.ServiceType)).Select(x => x.Model.GetEntityTypes().Select(y => y.ClrType)).SelectMany(x => x).Distinct().ToArray();
            foreach (var entityType in entityTypes)
            {
                foreach (var task in definitions.Where(x => x.EntityTypeConstraint.IsAssignableFrom(entityType)))
                {
                    services.AddTransient(dpetInterface.MakeGenericType(entityType), task.TaskType.MakeGenericType(entityType));
                }
            }
        }

        private static void ConfigServices(IServiceCollection services)
        {
            services.TryAddSingleton<AspNetCore.Http.IHttpContextAccessor, AspNetCore.Http.HttpContextAccessor>();
            services.AddScoped<System.Security.Principal.IPrincipal>(sp => sp.GetRequiredService<AspNetCore.Http.IHttpContextAccessor>().HttpContext.User);
            services.AddTransient<IFileManager, Hcs.Platform.Core.File.DbManager.DbFileManager>();
            services.AddTransient(typeof(IModelInfo<,,>), typeof(ModelInfo<,,>));
            services.AddTransient(typeof(IModelInfo<,>), typeof(ModelInfo<,>));
            services.AddSingleton<IPasswordHashService, Sha512PasswordHashService>();
            services.AddSingleton<IXlsxTEmplateFactory, WebApplicationXlsxTemplateFactory>();
            services.RegistFlowServiceBundle();
            services.AddOData();
            services.AddScoped<IPlatformUser>(sp =>
            {
                var userService = sp.GetRequiredService<IUserService>();
                var httpContext = sp.GetRequiredService<AspNetCore.Http.IHttpContextAccessor>().HttpContext;
                if (httpContext.User.Identity.IsAuthenticated)
                {
                    return sp.GetRequiredService<IUserService>().Get(long.Parse(httpContext.User.Identity.Name));
                }
                else
                {
                    return null;
                }
            });
            services.AddScoped<IUserRoleAccessor, UserRoleAccessor>();
            services.AddScoped(typeof(IScopedDbContext<>), typeof(ScopedDbContext<>));
            services.AddScoped<DbContext>(sp => sp.GetRequiredService<IScopedDbContext<PlatformDbContext>>().DbContext);
            services.AddScoped(typeof(IDbOperation<>), typeof(DbOperation<>));
            services.AddScoped<IJwtSecurityTokenService, JwtSecurityTokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRoleService, UserRoleService>();
            services.AddSingleton(typeof(Hcs.Platform.Odata.IOdataValidator<>), typeof(Hcs.Platform.Odata.OdataValidator<>));
            services.AddSingleton<IDbModelCreatingConfig, DbContextBuilder>();
            services.AddScoped<IAuthorizationHandler, RoleTokenHandler>();
            services.AddTransient(typeof(GetServiceOutput<,>));
            services.AddScoped(typeof(OdataQueryRequestContext<>));
            services.AddScoped(typeof(KeyRequestContext<>));
            services.AddScoped(typeof(InputRequestContext<>));
            services.AddScoped<IFlowBundle, FlowBundle>();
            services.AddScoped<IJwtSecurityTokenService, JwtSecurityTokenService>();
            services.AddScoped<IUserOdataPermissionService, UserOdataPermissionService>();
            services.AddScoped<IUserPlatformRoleAccessor, UserPlatformRoleAccessor>();
            services.AddScoped<Hcs.Platform.Odata.IOdataQueryPermission>(sp => sp.GetRequiredService<IUserOdataPermissionService>().OdataQueryPermission);
            services.AddSingleton<IUserCacheManager, UserCacheManager>();
            // processors
            services.AddTransient(typeof(GetKeyAndModel<,,>));
            services.AddTransient(typeof(GetKeyAndModel<,>));
            services.AddTransient(typeof(GetService<,>));
            services.AddTransient(typeof(QueryData<,,>));
            services.AddTransient(typeof(CreateData<,>));
            services.AddTransient(typeof(DeleteData<,>));
            services.AddTransient(typeof(UpdateData<,>));
            services.AddTransient(typeof(GetData<,,>));
            services.AddTransient(typeof(SetModelKey<,,>));
            services.AddTransient(typeof(FilterQueryData<>));
            services.AddTransient(typeof(ApplyOdataFilter<>));
            services.AddTransient(typeof(IDataProcessorEntityTaskCollection<>), typeof(DataProcessorEntityTaskCollection<>));


        }

        private static void ConfigAuthorizaion(IServiceCollection services, Hcs.Platform.PlatformBuilder builder)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Const.Policy.ContollerMethod, policy => policy.Requirements.Add(new ControllerMethodRequirement()));
            });
        }

        private static void ConfigAuthentication(IServiceCollection services, Hcs.Platform.PlatformBuilder builder)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.JwtConfigBuilder.IssuerSigningKey));
            builder.JwtConfigBuilder.SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.JwtConfigBuilder.Issuer,
                    ValidAudience = builder.JwtConfigBuilder.Audience,
                    LifetimeValidator = ValidateTokenLifeTime,
                    IssuerSigningKey = key
                };
            });
        }

        private static bool ValidateTokenLifeTime(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (!notBefore.HasValue || string.IsNullOrWhiteSpace(securityToken.Id))
            {
                return false;
            }
            var now = DateTime.UtcNow;
            if (notBefore.Value <= now && (!expires.HasValue || (expires.HasValue && expires.Value >= now)))
            {
                if (UserDataChangeTime.Get(long.Parse(securityToken.Id)) <= notBefore.Value)
                {
                    return true;
                }
            }
            return false;
        }

        public static IApplicationBuilder UseHcsPlatform(this IApplicationBuilder app)
        {
            DbInit(app);
            app.Use(Middlewares.AngularRouteCheckMiddleware);
            app.UseStaticFiles();
            app.Use(Middlewares.PlatformControllerMiddleware);
            app.UseAuthentication();
            var edmBuilder = new Microsoft.AspNet.OData.Builder.ODataConventionModelBuilder();
            using (var ctx = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<PlatformDbContext>())
            {
                foreach (var t in QueryApiEntities.Types.Distinct())
                {
                    var properties = t.GetProperties();
                    if (properties.Any(p => p.Name == "Id" || p.Name == t.Name + "Id" || p.GetCustomAttribute<System.ComponentModel.DataAnnotations.KeyAttribute>() != null))
                    {
                        var entityTypeConfiguration = edmBuilder.AddEntityType(t);
                        if (!entityTypeConfiguration.Keys.Any())
                        {
                            var entityType = ctx.Model.FindEntityType(t);
                            if (entityType != null)
                            {
                                foreach (var k in entityType.GetKeys())
                                {
                                    foreach (var p in k.Properties)
                                    {
                                        entityTypeConfiguration.HasKey(p.PropertyInfo);
                                    }
                                }
                            }
                            else
                            {
                                var keyp = properties.FirstOrDefault(x => x.GetCustomAttribute<System.ComponentModel.DataAnnotations.KeyAttribute>() != null);
                                if (keyp != null)
                                {
                                    entityTypeConfiguration.HasKey(keyp);
                                }
                                else
                                {
                                    keyp = properties.FirstOrDefault(p => p.Name == "Id");
                                    if (keyp != null)
                                    {
                                        entityTypeConfiguration.HasKey(keyp);
                                    }
                                    else
                                    {
                                        keyp = properties.FirstOrDefault(p => p.Name == t.Name + "Id");
                                        if (keyp != null)
                                        {
                                            entityTypeConfiguration.HasKey(keyp);
                                        }
                                    }
                                }
                            }
                        }
                        edmBuilder.AddEntitySet(t.Name, entityTypeConfiguration);
                    }
                }
                var emdModel = edmBuilder.GetEdmModel();
                app.UseMvc(b =>
                {
                    b.SetTimeZoneInfo(TimeZoneInfo.Utc);

                    b.EnableDependencyInjection(opt =>
                    {
                        opt.AddService(Microsoft.OData.ServiceLifetime.Singleton, typeof(OData.UriParser.ODataUriResolver), sp => new OData.UriParser.StringAsEnumResolver());
                        opt.AddService(Microsoft.OData.ServiceLifetime.Singleton, typeof(OData.Edm.IEdmModel), sp => emdModel);
                    });
                });
            }
            return app;
        }

        private static void DbInit(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetRequiredService<PlatformDbContext>())
                {
                    context.Database.EnsureCreated();
                    var flags = context.Set<Hcs.Platform.BaseModels.PlatformFlag>();
                    if (!flags.Any(x => x.Flag == "SeedData" && x.Value == "true"))
                    {
                        flags.Add(new Hcs.Platform.BaseModels.PlatformFlag { Flag = "SeedData", Value = "true" });
                        context.SaveChanges();
                        DbContextBuilder.RunSeeds(context);
                    }
                    foreach (var u in context.Set<Hcs.Platform.BaseModels.PlatformUser>())
                    {
                        UserDataChangeTime.Update(u.Id, u.LastUpdatedTime ?? DateTime.MinValue);
                    }
                }
            }
        }


    }

}