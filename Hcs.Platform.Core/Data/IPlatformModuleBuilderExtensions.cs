using System;
using System.Collections.Generic;
using System.Linq;
using Hcs.Platform.Core;
using Hcs.Platform.Core.Controller;
using Hcs.Platform.Flow;
using Hcs.Platform.PlatformModule;
using Hcs.Platform.Role;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Hcs.Platform.Data
{

    public static partial class IPlatformModuleBuilderExtensions
    {
        public static EntityApiRoles AddEntityApi<TEntity>(this IPlatformModuleBuilder builder, Action<EntityApiBuildContext<long, TEntity>> config = null)
          where TEntity : class
        {
            return builder.AddEntityApi<long, TEntity, PlatformDbContext>(config);
        }
        public static EntityApiRoles AddEntityApi<TEntity, TDbContext>(this IPlatformModuleBuilder builder, Action<EntityApiBuildContext<long, TEntity>> config = null)
          where TEntity : class
         where TDbContext : DbContext
        {
            return builder.AddEntityApi<long, TEntity, TDbContext>(config);
        }
        public static EntityApiRoles AddEntityApi<TKey, TEntity>(this IPlatformModuleBuilder builder, Action<EntityApiBuildContext<TKey, TEntity>> config = null)
        where TEntity : class
        {
            return builder.AddEntityApi<TKey, TEntity, PlatformDbContext>(config);
        }
        public static EntityApiRoles AddEntityApi<TKey, TEntity, TDbContext>(this IPlatformModuleBuilder builder, Action<EntityApiBuildContext<TKey, TEntity>> config = null)
        where TEntity : class
        where TDbContext : DbContext
        {
            var ctx = new EntityApiBuildContext<TKey, TEntity>();
            config?.Invoke(ctx);
            return new EntityApiRoles
            {
                Get = builder.AddGetApi<TKey, TEntity, TDbContext>(ctx.GetConfig),
                Put = builder.AddPutApi<TKey, TEntity, TDbContext>(ctx.PutConfig),
                Delete = builder.AddDeleteApi<TKey, TEntity, TDbContext>(ctx.DeleteConfig),
                Post = builder.AddPostApi<TKey, TEntity, TDbContext>(ctx.PostConfig),
                Query = builder.AddQueryApi<TEntity, TDbContext>(ctx.QueryConfig)
            };
        }
        public static EntityApiRoles AddEntityApi<TKey, TEntity, TView>(this IPlatformModuleBuilder builder, System.Linq.Expressions.Expression<Func<TEntity, TView>> projection, Action<EntityApiBuildContext<TKey, TEntity>> config = null)
        where TEntity : class
        where TView : class
        {
            return builder.AddEntityApi<TKey, TEntity, TView, PlatformDbContext>(projection, config);
        }
        public static EntityApiRoles AddEntityApi<TKey, TEntity, TView, TDbContext>(this IPlatformModuleBuilder builder, System.Linq.Expressions.Expression<Func<TEntity, TView>> projection, Action<EntityApiBuildContext<TKey, TEntity>> config = null)
        where TEntity : class
          where TView : class
        where TDbContext : DbContext
        {
            var ctx = new EntityApiBuildContext<TKey, TEntity>();
            config?.Invoke(ctx);
            return new EntityApiRoles
            {
                Get = builder.AddGetApi<TKey, TEntity, TDbContext>(ctx.GetConfig),
                Put = builder.AddPutApi<TKey, TEntity, TDbContext>(ctx.PutConfig),
                Delete = builder.AddDeleteApi<TKey, TEntity, TDbContext>(ctx.DeleteConfig),
                Post = builder.AddPostApi<TKey, TEntity, TDbContext>(ctx.PostConfig),
                Query = builder.AddQueryApi<TKey, TEntity, TView, TDbContext>(projection, ctx.QueryConfig)
            };
        }
        public static IRoleToken AddQueryApi<TEntity>(this IPlatformModuleBuilder builder, string name, DIFlowBuilderHandler<HttpRequest, IActionResult> flowBuilder) where TEntity : class => builder.AddQueryApi<TEntity, PlatformDbContext>(name, flowBuilder);
        public static IRoleToken AddQueryApi<TEntity, TDbContext>(this IPlatformModuleBuilder builder, string name, DIFlowBuilderHandler<HttpRequest, IActionResult> flowBuilder)
                    where TEntity : class
                    where TDbContext : DbContext
        {
            QueryApiEntities.Types.Add(typeof(TEntity));
            return builder.AddApi<Platform.Core.Controller.QueryController<TEntity>>(name, name + ".Query", flowBuilder);
        }
        public static IRoleToken AddEntityApi<TKey, TEntity>(this IPlatformModuleBuilder builder, string name, string methodName, DIFlowBuilderHandler<HttpRequest, IActionResult> flowBuilder) where TEntity : class => builder.AddEntityApi<TKey, TEntity, PlatformDbContext>(name, methodName, flowBuilder);
        public static IRoleToken AddEntityApi<TKey, TEntity, TDbContext>(this IPlatformModuleBuilder builder, string name, string methodName, DIFlowBuilderHandler<HttpRequest, IActionResult> flowBuilder)
                    where TEntity : class
                    where TDbContext : DbContext
        {
            return builder.AddApi<Platform.Core.Controller.EntityController<TKey, TEntity>>(name, name + "." + methodName, flowBuilder);
        }
        public static IRoleToken AddGetFlowApi<TKey, TEntity>(this IPlatformModuleBuilder builder, string name, DIFlowBuilderHandler<HttpRequest, IActionResult> flowBuilder) where TEntity : class =>
            builder.AddEntityApi<TKey, TEntity, PlatformDbContext>(name, EntityControllerMethods.Get, flowBuilder);
        public static IRoleToken AddGetFlowApi<TKey, TEntity, TDbContext>(this IPlatformModuleBuilder builder, string name, DIFlowBuilderHandler<HttpRequest, IActionResult> flowBuilder) where TEntity : class where TDbContext : DbContext =>
            builder.AddEntityApi<TKey, TEntity, TDbContext>(name, EntityControllerMethods.Get, flowBuilder);

        public static IRoleToken AddDeleteFlowApi<TKey, TEntity>(this IPlatformModuleBuilder builder, string name, DIFlowBuilderHandler<HttpRequest, IActionResult> flowBuilder) where TEntity : class =>
            builder.AddEntityApi<TKey, TEntity, PlatformDbContext>(name, EntityControllerMethods.Delete, flowBuilder);
        public static IRoleToken AddDeleteFlowApi<TKey, TEntity, TDbContext>(this IPlatformModuleBuilder builder, string name, DIFlowBuilderHandler<HttpRequest, IActionResult> flowBuilder) where TEntity : class where TDbContext : DbContext =>
            builder.AddEntityApi<TKey, TEntity, TDbContext>(name, EntityControllerMethods.Delete, flowBuilder);
        public static IRoleToken AddPutFlowApi<TKey, TEntity>(this IPlatformModuleBuilder builder, string name, DIFlowBuilderHandler<HttpRequest, IActionResult> flowBuilder) where TEntity : class =>
                    builder.AddEntityApi<TKey, TEntity, PlatformDbContext>(name, EntityControllerMethods.Put, flowBuilder);
        public static IRoleToken AddPutFlowApi<TKey, TEntity, TDbContext>(this IPlatformModuleBuilder builder, string name, DIFlowBuilderHandler<HttpRequest, IActionResult> flowBuilder) where TEntity : class where TDbContext : DbContext =>
            builder.AddEntityApi<TKey, TEntity, TDbContext>(name, EntityControllerMethods.Put, flowBuilder);

        public static IRoleToken AddPostFlowApi<TKey, TEntity>(this IPlatformModuleBuilder builder, string name, DIFlowBuilderHandler<HttpRequest, IActionResult> flowBuilder) where TEntity : class =>
                builder.AddEntityApi<TKey, TEntity, PlatformDbContext>(name, EntityControllerMethods.Post, flowBuilder);
        public static IRoleToken AddPostFlowApi<TKey, TEntity, TDbContext>(this IPlatformModuleBuilder builder, string name, DIFlowBuilderHandler<HttpRequest, IActionResult> flowBuilder) where TEntity : class where TDbContext : DbContext =>
            builder.AddEntityApi<TKey, TEntity, TDbContext>(name, EntityControllerMethods.Post, flowBuilder);
        public static IRoleToken AddQueryFlowApi<TEntity>(this IPlatformModuleBuilder builder, string name, DIFlowBuilderHandler<HttpRequest, IActionResult> flowBuilder) where TEntity : class =>
            builder.AddQueryApi<TEntity, PlatformDbContext>(name, flowBuilder);
        public static IRoleToken AddQueryFlowApi<TKey, TEntity, TDbContext>(this IPlatformModuleBuilder builder, string name, DIFlowBuilderHandler<HttpRequest, IActionResult> flowBuilder) where TEntity : class where TDbContext : DbContext =>
            builder.AddQueryApi<TEntity, TDbContext>(name, flowBuilder);

        public static IRoleToken AddGetApi<TKey, TEntity>(this IPlatformModuleBuilder builder, Action<GetApiConfigurations<TKey, TEntity>> options = null) where TEntity : class => builder.AddGetApi<TKey, TEntity, PlatformDbContext>(options);

        public static IRoleToken AddGetApi<TKey, TEntity, TDbContext>(this IPlatformModuleBuilder builder, Action<GetApiConfigurations<TKey, TEntity>> options = null)
                    where TEntity : class
                    where TDbContext : DbContext
        {
            var config = new GetApiConfigurations<TKey, TEntity>();
            options?.Invoke(config);
            return builder.AddEntityApi<TKey, TEntity, TDbContext>(GetApiKey<TEntity>(), EntityControllerMethods.Get, c =>
                c.Then(config.Request)
                .GetRequestKey<TKey>()
                .Then(config.KeyGeted)
                .GetData<TKey, TEntity, TDbContext>()
                .Then(config.Geted)
                .StartValidation()
                .Then(config.Validate)
                .EndValidation(x => x.OkOrNotFound()));
        }

        public static IRoleToken AddDeleteApi<TKey, TEntity>(this IPlatformModuleBuilder builder, Action<DeleteApiConfigurations<TKey, TEntity>> options = null) where TEntity : class => builder.AddDeleteApi<TKey, TEntity, PlatformDbContext>(options);
        public static IRoleToken AddDeleteApi<TKey, TEntity, TDbContext>(this IPlatformModuleBuilder builder, Action<DeleteApiConfigurations<TKey, TEntity>> options = null)
              where TEntity : class
              where TDbContext : DbContext
        {
            var config = new DeleteApiConfigurations<TKey, TEntity>();
            options?.Invoke(config);
            return builder.AddEntityApi<TKey, TEntity, TDbContext>(GetApiKey<TEntity>(), EntityControllerMethods.Delete, c =>
                 c.Then(config.Request)
                 .GetRequestKey<TKey>()
                 .Then(config.KeyGeted)
                 .GetData<TKey, TEntity, TDbContext>()
                 .StartValidation()
                 .Then(config.Validate)
                 .EndValidation(x => x.DeleteData<TEntity, TDbContext>()
                    .Then(config.Deleted)
                    .OkOrNotFound()
                    .Then(config.Response))
                );
        }
        public static IRoleToken AddQueryApi<Tkey, TEntity, TView>(this IPlatformModuleBuilder builder, System.Linq.Expressions.Expression<Func<TEntity, TView>> projection, Action<QueryApiConfigurations<TEntity>> options = null)
          where TEntity : class
           where TView : class
           => builder.AddQueryApi<Tkey, TEntity, TView, PlatformDbContext>(projection, options);
        public static IRoleToken AddQueryApi<Tkey, TEntity, TView, TDbContext>(this IPlatformModuleBuilder builder, System.Linq.Expressions.Expression<Func<TEntity, TView>> projection, Action<QueryApiConfigurations<TEntity>> options = null)
            where TEntity : class
            where TView : class
            where TDbContext : DbContext
        {
            var config = new QueryApiConfigurations<TEntity>();
            options?.Invoke(config);
            return builder.AddQueryApi<TView, TDbContext>(GetApiKey<TView>(), c =>
               c.Then(config.Request)
               .StartValidation()
               .ValidOdata<TView>()
               .EndValidation(x => x.Query<TEntity>()
                     .Then(config.Queryed)
                     .Pipe(p => p.Select(projection))
                     .ApplyOdataFilter()
                     .Then(config.OdataFiltered)
                     .QueryOutout(config.ExportSettings)
                     .Then(config.Response)));
        }



        public static IRoleToken AddQueryApi<TEntity>(this IPlatformModuleBuilder builder, Action<QueryApiConfigurations<TEntity>> options = null) where TEntity : class => builder.AddQueryApi<TEntity, PlatformDbContext>(options);
        public static IRoleToken AddQueryApi<TEntity, TDbContext>(this IPlatformModuleBuilder builder, Action<QueryApiConfigurations<TEntity>> options = null)
            where TEntity : class
            where TDbContext : DbContext
        {
            var config = new QueryApiConfigurations<TEntity>();
            options?.Invoke(config);
            return builder.AddQueryApi<TEntity, TDbContext>(GetApiKey<TEntity>(), c =>
               c.Then(config.Request)
               .StartValidation()
               .ValidOdata<TEntity>()
               .EndValidation(x => x.Query<TEntity>()
                     .Then(config.Queryed)
                     .ApplyOdataFilter()
                     .Then(config.OdataFiltered)
                     .QueryOutout(config.ExportSettings)
                     .Then(config.Response)));
        }
        public static IRoleToken AddPostApi<TKey, TEntity>(this IPlatformModuleBuilder builder) where TEntity : class => builder.AddPostApi<TKey, TEntity, PlatformDbContext>();
        public static IRoleToken AddPostApi<TKey, TEntity, TDbContext>(this IPlatformModuleBuilder builder, Action<PostApiConfigurations<TKey, TEntity>> options = null)
                  where TEntity : class
                  where TDbContext : DbContext
        {
            var config = new PostApiConfigurations<TKey, TEntity>();
            options?.Invoke(config);
            return builder.AddEntityApi<TKey, TEntity, TDbContext>(GetApiKey<TEntity>(), EntityControllerMethods.Post, c =>
                 c.Then(config.Request)
                 .GetRequestModel<TEntity>()
                 .Then(config.ModelGeted)
                 .StartValidation<TEntity>()
                 .Then(config.Validate)
                 .EndValidation(x => x.CreateData<TEntity, TDbContext>().Then(config.Created).OkOrNotFound().Then(config.Response)));
        }

        private static string GetApiKey<TEntity>() where TEntity : class
        {
            return typeof(TEntity).FullName;
        }

        public static IRoleToken AddPutApi<TKey, TEntity>(this IPlatformModuleBuilder builder, Action<PutApiConfigurations<TEntity>> options = null) where TEntity : class => builder.AddPutApi<TKey, TEntity, PlatformDbContext>(options);
        public static IRoleToken AddPutApi<TKey, TEntity, TDbContext>(this IPlatformModuleBuilder builder, Action<PutApiConfigurations<TEntity>> options = null)
                   where TEntity : class
                   where TDbContext : DbContext
        {
            var config = new PutApiConfigurations<TEntity>();
            options?.Invoke(config);

            return builder.AddEntityApi<TKey, TEntity, TDbContext>(GetApiKey<TEntity>(), EntityControllerMethods.Put, c =>
                 c.Then(config.Request)
                 .GetRequestModel<TEntity>()
                 .Then(config.ModelGeted)
                 .SetModelKey<TKey, TEntity, TDbContext>()
                 .Then(config.KeySet)
                 .StartValidation<TEntity>()
                 .Then(config.Validate)
                 .EndValidation(x => x.UpdateData<TEntity, TDbContext>().Then(config.Updated).OkOrNotFound().Then(config.Response)));
            ;
        }
    }
}