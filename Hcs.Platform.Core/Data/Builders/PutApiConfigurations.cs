using Hcs.Platform.Flow;
namespace Hcs.Platform.Data
{

    public static partial class IPlatformModuleBuilderExtensions
    {
        public class PutApiConfigurations<TEntity> : ApiConfigurations where TEntity : class
        {
            internal DIFlowBuilderHandler<TEntity, TEntity> ModelGeted { get; set; } = x => x;
            public PutApiConfigurations<TEntity> OnModelGeted(DIFlowBuilderHandler<TEntity, TEntity> options)
            {
                var o = ModelGeted;
                ModelGeted = x => options(o(x));
                return this;
            }
            internal DIFlowBuilderHandler<TEntity, TEntity> KeySet { get; set; } = x => x;
            public PutApiConfigurations<TEntity> OnKeySet(DIFlowBuilderHandler<TEntity, TEntity> options)
            {
                var o = KeySet;
                KeySet = x => options(o(x));
                return this;
            }
            internal DIFlowBuilderHandler<TEntity, TEntity> Updated { get; set; } = x => x;
            public PutApiConfigurations<TEntity> OnUpdated(DIFlowBuilderHandler<TEntity, TEntity> options)
            {
                var o = Updated;
                Updated = x => options(o(x));
                return this;
            }


            internal DIFlowBuilderHandler<EntityValidationResult<TEntity>, EntityValidationResult<TEntity>> Validate { get; set; } = x => x;
            public PutApiConfigurations<TEntity> OnValidate(DIFlowBuilderHandler<EntityValidationResult<TEntity>, EntityValidationResult<TEntity>> options)
            {
                var o = Validate;
                Validate = x => options(o(x));
                return this;
            }
        }
    }

}