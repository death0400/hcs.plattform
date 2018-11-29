using System;
using Hcs.Platform.Flow;
namespace Hcs.Platform.Data
{

    public static partial class IPlatformModuleBuilderExtensions
    {
        public class QueryApiConfigurations<TEntity> : ApiConfigurations where TEntity : class
        {

            internal DIFlowBuilderHandler<System.Linq.IQueryable<TEntity>, System.Linq.IQueryable<TEntity>> Queryed { get; private set; } = x => x;
            internal DIFlowBuilderHandler<object[], object[]> OdataFiltered { get; private set; } = x => x;

            public QueryApiConfigurations<TEntity> OnQueryed(DIFlowBuilderHandler<System.Linq.IQueryable<TEntity>, System.Linq.IQueryable<TEntity>> options)
            {
                var o = Queryed;
                Queryed = x => options(o(x));
                return this;
            }
            internal Action<ExportSettings<object>> ExportSettings { get; private set; } = x => { };
            public QueryApiConfigurations<TEntity> ConfigExportSetting(Action<ExportSettings<object>> settings)
            {
                var o = settings;
                this.ExportSettings = x =>
                {
                    o(x);
                    settings(x);
                };
                return this;
            }
            public QueryApiConfigurations<TEntity> OnOdataFiltered(DIFlowBuilderHandler<object[], object[]> options)
            {
                var o = OdataFiltered;
                OdataFiltered = x => options(o(x));
                return this;
            }

        }
    }

}