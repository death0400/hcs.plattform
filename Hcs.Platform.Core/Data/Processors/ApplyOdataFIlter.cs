using System.Linq;
using System;
using Hcs.Platform.Flow;
using Hcs.Platform.Core;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Http;

namespace Hcs.Platform.Data
{
    public class ApplyOdataFilter<TEntity> : FlowProcessor<IQueryable<TEntity>, object[]> where TEntity : class
    {
        private readonly OdataQueryRequestContext<TEntity> odataQueryRequestContext;
        private readonly IHttpContextAccessor httpContext;

        public ApplyOdataFilter(OdataQueryRequestContext<TEntity> odataQueryRequestContext, IHttpContextAccessor httpContext)
        {
            this.httpContext = httpContext;
            this.odataQueryRequestContext = odataQueryRequestContext;

        }
        public override object[] Process(IQueryable<TEntity> input)
        {
            var queryOptions = odataQueryRequestContext.ODataQueryOptions;

            var data = queryOptions.ApplyTo(input);
            if (queryOptions?.Count?.Value == true)
            {

                var count = httpContext.HttpContext.ODataFeature().TotalCount;
                if (count.HasValue)
                {
                    httpContext.HttpContext.Response.Headers.Add("x-hcs-platform-query-total-count", count.Value.ToString("#0"));
                }
            }
            return data.Cast<object>().ToArray();
        }
    }
}