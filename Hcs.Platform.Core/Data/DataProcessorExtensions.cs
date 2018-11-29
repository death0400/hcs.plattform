using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hcs.Platform.Core;
using Hcs.Platform.Data;
using Hcs.Platform.Flow;
using Hcs.Serialize.Xlsx;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hcs.Platform.Flow
{
    public static partial class DataProcessorExtensions
    {
        public static IDependencyInjectionFlowBuilderContext<EntityValidationResult<TEntity>> Validate<TEntity>(this IDependencyInjectionFlowBuilderContext<EntityValidationResult<TEntity>> c, Func<TEntity, Action<ValidationError>, Task> validate) where TEntity : class
        => c.Then<EntityValidationResult<TEntity>>(() => new DelegateValidationProcessor<TEntity>(validate));
        public static IDependencyInjectionFlowBuilderContext<EntityValidationResult<TEntity>> StartValidation<TEntity>(this IDependencyInjectionFlowBuilderContext<TEntity> c) where TEntity : class
        => c.Then<EntityValidationResult<TEntity>>(() => new ValidationStart<TEntity>());
        public static IDependencyInjectionFlowBuilderContext<IActionResult> EndValidation<TEntity>(this IDependencyInjectionFlowBuilderContext<EntityValidationResult<TEntity>> c, DIFlowBuilderHandler<TEntity, IActionResult> ifPass) where TEntity : class
        {
            return c.Branch<IActionResult>(cases =>
            {
                cases.AddCase(x => x.ValidationErrors.Any(), builder => builder.Pipe(x => x.ValidationErrors).BadRequest(true));
                cases.AddCase(x => !x.ValidationErrors.Any(), builder => ifPass(builder.Pipe(x => x.Data)));
            });
        }
        public static IDependencyInjectionFlowBuilderContext<GetKeyAndModelOutput<TKey, TEntity>> GetKeyAndModel<TKey, TEntity>(this IDependencyInjectionFlowBuilderContext<object> c)
        => c.Then<GetKeyAndModelOutput<TKey, TEntity>, GetKeyAndModel<TKey, TEntity>>();
        public static IDependencyInjectionFlowBuilderContext<GetKeyAndModelOutput<T, TKey, TEntity>> GetKeyAndModel<T, TKey, TEntity>(this IDependencyInjectionFlowBuilderContext<T> c)
        => c.Then<GetKeyAndModelOutput<T, TKey, TEntity>, GetKeyAndModel<T, TKey, TEntity>>();
        public static IDependencyInjectionFlowBuilderContext<TKey> GetRequestKey<TKey>(this IDependencyInjectionFlowBuilderContext<HttpRequest> c)
        => c.GetService<KeyRequestContext<TKey>>().Pipe(x => x.Service.Key);
        public static IDependencyInjectionFlowBuilderContext<TEntity> GetRequestModel<TEntity>(this IDependencyInjectionFlowBuilderContext<HttpRequest> c)
        => c.GetService<InputRequestContext<TEntity>>().Pipe(x => x.Service.Input);
        public static IDependencyInjectionFlowBuilderContext<TEntity> SetModelKey<TKey, TEntity, TDbContext>(this IDependencyInjectionFlowBuilderContext<TEntity> c) where TDbContext : DbContext where TEntity : class
        => c.Then<TEntity, SetModelKey<TKey, TEntity, TDbContext>>();
        public static IDependencyInjectionFlowBuilderContext<TEntity> SetModelKey<TKey, TEntity>(this IDependencyInjectionFlowBuilderContext<TEntity> c) where TEntity : class
        => c.SetModelKey<TKey, TEntity, PlatformDbContext>();
        public static IDependencyInjectionFlowBuilderContext<TEntity> GetData<TKey, TEntity, TDbContext>(this IDependencyInjectionFlowBuilderContext<TKey> builder) where TEntity : class where TDbContext : DbContext
        => builder.Then<TEntity, GetData<TKey, TEntity, TDbContext>>();
        public static IDependencyInjectionFlowBuilderContext<TEntity> GetData<TKey, TEntity>(this IDependencyInjectionFlowBuilderContext<TKey> builder) where TEntity : class
        => builder.GetData<TKey, TEntity, PlatformDbContext>();
        public static IDependencyInjectionFlowBuilderContext<TEntity> DeleteData<TEntity>(this IDependencyInjectionFlowBuilderContext<TEntity> c) where TEntity : class
        => c.DeleteData<TEntity, PlatformDbContext>();
        public static IDependencyInjectionFlowBuilderContext<TEntity> DeleteData<TEntity, TDbContext>(this IDependencyInjectionFlowBuilderContext<TEntity> c) where TEntity : class where TDbContext : DbContext
        => c.Then<TEntity, DeleteData<TEntity, TDbContext>>();
        public static IDependencyInjectionFlowBuilderContext<TEntity> CreateData<TEntity, TDbContext>(this IDependencyInjectionFlowBuilderContext<TEntity> c) where TEntity : class where TDbContext : DbContext
        => c.Then<TEntity, CreateData<TEntity, TDbContext>>();
        public static IDependencyInjectionFlowBuilderContext<TEntity> CreateData<TEntity>(this IDependencyInjectionFlowBuilderContext<TEntity> c) where TEntity : class => c.CreateData<TEntity, PlatformDbContext>();
        public static IDependencyInjectionFlowBuilderContext<TEntity> UpdateData<TEntity, TDbContext>(this IDependencyInjectionFlowBuilderContext<TEntity> c) where TEntity : class where TDbContext : DbContext
        => c.Then<TEntity, UpdateData<TEntity, TDbContext>>();
        public static IDependencyInjectionFlowBuilderContext<TEntity> UpdateData<TEntity>(this IDependencyInjectionFlowBuilderContext<TEntity> c) where TEntity : class => c.UpdateData<TEntity, PlatformDbContext>();
        public static IDependencyInjectionFlowBuilderContext<IQueryable<TEntity>> Query<TCurrnet, TEntity, TDbContext>(this IDependencyInjectionFlowBuilderContext<TCurrnet> builder) where TDbContext : DbContext where TEntity : class
        => builder.Then<IQueryable<TEntity>, QueryData<TCurrnet, TEntity, TDbContext>>();
        public static IDependencyInjectionFlowBuilderContext<object[]> ApplyOdataFilter<TEntity>(this IDependencyInjectionFlowBuilderContext<IQueryable<TEntity>> builder) where TEntity : class
        => builder.Then<object[], ApplyOdataFilter<TEntity>>();
        public static IDependencyInjectionFlowBuilderContext<IQueryable<TEntity>> Query<TEntity, TDbContext>(this IDependencyInjectionFlowBuilderContext<HttpRequest> builder) where TDbContext : DbContext where TEntity : class
         => builder.Query<HttpRequest, TEntity, TDbContext>();
        public static IDependencyInjectionFlowBuilderContext<IQueryable<TEntity>> Query<TEntity>(this IDependencyInjectionFlowBuilderContext<HttpRequest> builder) where TEntity : class
        => builder.Query<TEntity, PlatformDbContext>();
        public static IDependencyInjectionFlowBuilderContext<IQueryable<TEntity>> Filter<TEntity>(this IDependencyInjectionFlowBuilderContext<IQueryable<TEntity>> builder, System.Linq.Expressions.Expression<Func<TEntity, bool>> filter)
        => builder.Then(new FilterQueryData<TEntity>(filter));
        public static IDependencyInjectionFlowBuilderContext<IActionResult> OkOrNotFound(this IDependencyInjectionFlowBuilderContext<object> builder)
        {
            return builder.Branch<IActionResult>(sCase =>
            {
                sCase.AddCase(x => x == null, cb => cb.NotFound());
                sCase.AddCase(x => x != null, cb => cb.Ok());
            });
        }
        public static IDependencyInjectionFlowBuilderContext<IActionResult> QueryOutout<T>(this IDependencyInjectionFlowBuilderContext<IEnumerable<T>> builder, Action<ExportSettings<object>> settings)
        {
            return builder.Pipe(new Func<IHttpContextAccessor, IEnumerable<T>, IActionResult>((IHttpContextAccessor httpCtx, IEnumerable<T> data) =>
            {
                var req = httpCtx.HttpContext.Request;
                if (req.Query.ContainsKey("export") && req.Query["export"] == "true")
                {
                    if (!req.Query.ContainsKey("columns"))
                    {
                        return new ObjectResult("columns not define") { StatusCode = 400 };
                    }
                    var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    var exportColumns = ExportColumn.Parse(req.Query["columns"]);
                    var set = new ExportSettings<object>();
                    settings(set);
                    var selset = new XlsxSerializeSettings<IEnumerable<object>>();
                    var processes = new List<IXlsxProcess<IEnumerable<object>>>();
                    var head = new WriteXlsxConst<IEnumerable<object>>(0, 0, 0, exportColumns.Select(x => x.DisplayName).ToArray());
                    head.PostSetValue = c => c.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    processes.Add(head);
                    if (data.Any())
                    {
                        if (data.First() is Microsoft.AspNet.OData.Query.ISelectExpandWrapper)
                        {
                            var listOfWriters = exportColumns.Select(c => new OdataCellWriter(c.PropertyName)).ToArray();
                            set.ConfigCellWriter(listOfWriters);
                            var rowp = new ObjectWriteXlsxRow<Microsoft.AspNet.OData.Query.ISelectExpandWrapper>(0, 1, 0, listOfWriters);
                            processes.Add(rowp);
                        }
                        else
                        {
                            var listOfWriters = exportColumns.Select(c => new PropertyCellWriter<object>(c.PropertyName, data.First().GetType())).ToArray();
                            set.ConfigCellWriter(listOfWriters);
                            var rowp = new WriteXlsxRow<object>(0, 1, 0, listOfWriters);
                            processes.Add(rowp);
                        }

                    }
                    processes.Add(new AutoFitColumns<IEnumerable<object>>(0));
                    selset.Processes = set.MapProcess(processes.ToArray());
                    selset.Template = set.MapTemplate(null);
                    return new FileStreamResult(new XlsxSerializer<IEnumerable<object>>(selset).Serialize(data.Cast<object>()), contentType);
                }
                return new OkObjectResult(data);
            }));
        }
    }

}