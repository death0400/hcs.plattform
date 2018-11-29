using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hcs.Platform.Flow;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using static Hcs.Platform.Data.IPlatformModuleBuilderExtensions;
using Microsoft.EntityFrameworkCore;
using Hcs.Platform.Data;
using Hcs.Platform.Core;
using System.Linq.Expressions;
namespace Hcs.Platform.File
{
    public static class FilePipeExtensions
    {
        const string FilePipeExtensionsHttpContextItemKey = nameof(FilePipeExtensionsHttpContextItemKey);
        public static EntityApiBuildContext<TKey, TEntity> SetupFilePipes<TKey, TEntity>(this EntityApiBuildContext<TKey, TEntity> apiBuildContext,
           Expression<Func<TEntity, string>> property)
           where TEntity : class => apiBuildContext.SetupFilePipes<TKey, TEntity, PlatformDbContext>(property);
        public static EntityApiBuildContext<TKey, TEntity> SetupFilePipes<TKey, TEntity, TDbContext>(
            this EntityApiBuildContext<TKey, TEntity> apiBuildContext,
            Expression<Func<TEntity, string>> property)
            where TEntity : class
            where TDbContext : DbContext
        {
            var compiled = property.Compile();
            return apiBuildContext.SetupFilePipes<TKey, TEntity>(
                (sp, x) => Task.FromResult((new string[] { compiled(x) }).AsEnumerable()),
                async (sp, db, key) =>
                {
                    var info = sp.GetRequiredService<IModelInfo<TDbContext, TEntity, TKey>>();
                    var exp = CreateWhereExpression<TKey, TEntity, TDbContext>(info, key);
                    return await db.Set<TEntity>().Where(exp).Select(property).ToArrayAsync();
                },
                async (sp, db, model) =>
                {
                    var info = sp.GetRequiredService<IModelInfo<TDbContext, TEntity, TKey>>();
                    var exp = CreateWhereExpression<TKey, TEntity, TDbContext>(info, info.PrimaryKeyAccessor.Getter(model), true);
                    return await db.Set<TEntity>().Where(exp).Select(property).ToArrayAsync();
                }
                );
        }

        private static Expression<Func<TEntity, bool>> CreateWhereExpression<TKey, TEntity, TDbContext>(IModelInfo<TDbContext, TEntity, TKey> info, TKey key, bool not = false)
            where TEntity : class
            where TDbContext : DbContext
        {
            var properties = new Queue<System.Reflection.PropertyInfo>(info.PrimaryKey.Properties.Select(x => x.PropertyInfo));
            var keyConst = Expression.Constant(key, typeof(TKey));
            var inProp = Expression.Parameter(typeof(TEntity), "x");
            Expression body = null;
            do
            {
                var p = properties.Dequeue();
                if (body == null)
                {
                    body = Expression.Equal(Expression.Property(inProp, p), keyConst);
                }
                else
                {
                    body = Expression.AndAlso(body, Expression.Equal(Expression.Property(inProp, p), keyConst));
                }
            } while (properties.Count > 0);
            if (not)
            {
                body = Expression.Not(body);
            }
            var exp = Expression.Lambda<Func<TEntity, bool>>(body, inProp);
            return exp;
        }

        public static EntityApiBuildContext<TKey, TEntity> SetupFilePipes<TKey, TEntity>(
          this EntityApiBuildContext<TKey, TEntity> apiBuildContext,
          Func<TEntity, Task<IEnumerable<string>>> findFilesFromModel,
          Func<DbContext, TKey, Task<IEnumerable<string>>> findFilesFromDb,
          Func<DbContext, TEntity, Task<IEnumerable<string>>> findReferencedFilesFromDb) where TEntity : class
         => apiBuildContext.SetupFilePipes((sp, model) => findFilesFromModel(model), (sp, db, key) => findFilesFromDb(db, key), (sp, db, model) => findReferencedFilesFromDb(db, model));
        public static EntityApiBuildContext<TKey, TEntity> SetupFilePipes<TKey, TEntity>(
            this EntityApiBuildContext<TKey, TEntity> apiBuildContext,
            Func<IServiceProvider, TEntity, Task<IEnumerable<string>>> findFilesFromModel,
            Func<IServiceProvider, DbContext, TKey, Task<IEnumerable<string>>> findFilesFromDb,
            Func<IServiceProvider, DbContext, TEntity, Task<IEnumerable<string>>> findReferencedFilesFromDb) where TEntity : class
        {
            apiBuildContext.ConfigPostApi(x => x.OnCreated(y => y.ConfirmFilesFor(findFilesFromModel)));
            apiBuildContext.ConfigDeleteApi(x => x.OnKeyGeted(y => y.StageOldFilesFor(findFilesFromDb)).OnDeleted(y => y.DeleteAllStagedFilesFor(findReferencedFilesFromDb)));
            apiBuildContext.ConfigPutApi(x => x.OnKeySet(y => y.Pipe<TEntity, IServiceProvider>(async (sp, model) =>
            {
                var keyContext = sp.GetRequiredService<KeyRequestContext<TKey>>();
                await StageOldFilesFor(findFilesFromDb, sp, keyContext.Key);
                return model;
            })).OnUpdated(y => y.ConfirmFilesFor(findFilesFromModel).CheckDeleteFileOnUpdateFor(findFilesFromDb, findReferencedFilesFromDb)));
            return apiBuildContext;
        }
        public static IDependencyInjectionFlowBuilderContext<T> ConfirmFilesFor<T>(this IDependencyInjectionFlowBuilderContext<T> builder, Func<IServiceProvider, T, Task<IEnumerable<string>>> findFiles)
        {
            return builder.Pipe<T, IServiceProvider>(async (sp, model) =>
            {
                var fm = sp.GetRequiredService<Hcs.Platform.File.IFileManager>();
                foreach (var file in ParseFileString(await findFiles(sp, model)))
                {
                    await fm.ConfirmFile(file);
                }
                return model;
            });
        }
        public static IDependencyInjectionFlowBuilderContext<TKey> StageOldFilesFor<TKey>(this IDependencyInjectionFlowBuilderContext<TKey> builder, Func<IServiceProvider, DbContext, TKey, Task<IEnumerable<string>>> findFiles)
        {
            return builder.Pipe<TKey, IServiceProvider>(async (sp, key) =>
            {
                await StageOldFilesFor(findFiles, sp, key);
                return key;
            });
        }

        private static async Task StageOldFilesFor<TKey>(Func<IServiceProvider, DbContext, TKey, Task<IEnumerable<string>>> findFiles, IServiceProvider sp, TKey key)
        {
            var ctx = sp.GetRequiredService<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
            var files = ParseFileString(await findFiles(sp, sp.GetRequiredService<DbContext>(), key));
            ctx.HttpContext.Items.Add(FilePipeExtensionsHttpContextItemKey, files);
        }

        static string[] ParseFileString(IEnumerable<string> filesString) => filesString.Where(x => !string.IsNullOrWhiteSpace(x)).SelectMany(x => x.Split(',')).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        public static IDependencyInjectionFlowBuilderContext<T> DeleteAllStagedFilesFor<T>(this IDependencyInjectionFlowBuilderContext<T> builder, Func<IServiceProvider, DbContext, T, Task<IEnumerable<string>>> findReferencedFiles)
        {
            return builder.Pipe<T, IServiceProvider>(async (sp, model) =>
           {
               var ctx = sp.GetRequiredService<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
               var oldFiles = ctx.HttpContext.Items[FilePipeExtensionsHttpContextItemKey] as string[];
               var fm = sp.GetRequiredService<Hcs.Platform.File.IFileManager>();
               var hasRef = await findReferencedFiles(sp, sp.GetRequiredService<DbContext>(), model);
               foreach (var del in oldFiles.Except(hasRef))
               {
                   await fm.Delete(del);
               }
               return model;
           });
        }
        public static IDependencyInjectionFlowBuilderContext<T> CheckDeleteFileOnUpdateFor<T, TKey>(this IDependencyInjectionFlowBuilderContext<T> builder, Func<IServiceProvider, DbContext, TKey, Task<IEnumerable<string>>> findFiles, Func<IServiceProvider, DbContext, T, Task<IEnumerable<string>>> findReferencedFiles)
        {
            return builder.Pipe<T, IServiceProvider>(async (sp, model) =>
             {
                 var dbCtx = sp.GetRequiredService<DbContext>();
                 var ctx = sp.GetRequiredService<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
                 var newFiles = await findFiles(sp, dbCtx, sp.GetRequiredService<KeyRequestContext<TKey>>().Key);
                 var oldFiles = ctx.HttpContext.Items[FilePipeExtensionsHttpContextItemKey] as string[];
                 var fm = sp.GetRequiredService<Hcs.Platform.File.IFileManager>();
                 var dbctx = sp.GetRequiredService<DbContext>();
                 var toBeDelete = oldFiles.Except(newFiles).ToArray();
                 var hasRef = ParseFileString(await findReferencedFiles(sp, dbctx, model));
                 foreach (var del in toBeDelete.Except(hasRef))
                 {
                     await fm.Delete(del);
                 }
                 return model;
             });
        }
    }
}