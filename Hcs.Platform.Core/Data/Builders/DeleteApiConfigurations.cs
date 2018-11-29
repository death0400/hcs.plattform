using Hcs.Platform.Flow;
namespace Hcs.Platform.Data
{

    public static partial class IPlatformModuleBuilderExtensions
    {
        public class DeleteApiConfigurations<TKey, TEntity> : ApiConfigurations where TEntity : class
        {

            internal DIFlowBuilderHandler<TKey, TKey> KeyGeted { get; private set; } = x => x;

            internal DIFlowBuilderHandler<TEntity, TEntity> Deleted { get; private set; } = x => x;

            internal DIFlowBuilderHandler<EntityValidationResult<TEntity>, EntityValidationResult<TEntity>> Validate { get; set; } = x => x;
            public DeleteApiConfigurations<TKey, TEntity> OnValidate(DIFlowBuilderHandler<EntityValidationResult<TEntity>, EntityValidationResult<TEntity>> options)
            {
                var o = Validate;
                Validate = x => options(o(x));
                return this;
            }
            public DeleteApiConfigurations<TKey, TEntity> OnKeyGeted(DIFlowBuilderHandler<TKey, TKey> options)
            {
                var o = KeyGeted;
                KeyGeted = x => options(o(x));
                return this;
            }
            public DeleteApiConfigurations<TKey, TEntity> OnDeleted(DIFlowBuilderHandler<TEntity, TEntity> options)
            {
                var o = Deleted;
                Deleted = x => options(o(x));
                return this;
            }

        }
    }

}