using Hcs.Platform.Flow;
namespace Hcs.Platform.Data
{

    public static partial class IPlatformModuleBuilderExtensions
    {
        public class PostApiConfigurations<TKey, TEntity> : ApiConfigurations where TEntity : class
        {
            internal DIFlowBuilderHandler<TEntity, TEntity> ModelGeted { get; set; } = x => x;
            public PostApiConfigurations<TKey, TEntity> OnModelGeted(DIFlowBuilderHandler<TEntity, TEntity> options)
            {
                var o = ModelGeted;
                ModelGeted = x => options(o(x));
                return this;
            }
            internal DIFlowBuilderHandler<TEntity, TEntity> Created { get; set; } = x => x;
            public PostApiConfigurations<TKey, TEntity> OnCreated(DIFlowBuilderHandler<TEntity, TEntity> options)
            {
                var o = Created;
                Created = x => options(o(x));
                return this;
            }
            internal DIFlowBuilderHandler<EntityValidationResult<TEntity>, EntityValidationResult<TEntity>> Validate { get; set; } = x => x;
            public PostApiConfigurations<TKey, TEntity> OnValidate(DIFlowBuilderHandler<EntityValidationResult<TEntity>, EntityValidationResult<TEntity>> options)
            {
                var o = Validate;
                Validate = x => options(o(x));
                return this;
            }
        }
    }

}