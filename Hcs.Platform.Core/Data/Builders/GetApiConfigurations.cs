using Hcs.Platform.Flow;
namespace Hcs.Platform.Data
{

    public static partial class IPlatformModuleBuilderExtensions
    {
        public class GetApiConfigurations<TKey, TEntity> : ApiConfigurations where TEntity : class
        {

            internal DIFlowBuilderHandler<TKey, TKey> KeyGeted { get; private set; } = x => x;
            internal DIFlowBuilderHandler<TEntity, TEntity> Geted { get; private set; } = x => x;
            internal DIFlowBuilderHandler<EntityValidationResult<TEntity>, EntityValidationResult<TEntity>> Validate { get; set; } = x => x;

            public GetApiConfigurations<TKey, TEntity> OnValidate(DIFlowBuilderHandler<EntityValidationResult<TEntity>, EntityValidationResult<TEntity>> options)
            {
                var o = Validate;
                Validate = x => options(o(x));
                return this;
            }


            public GetApiConfigurations<TKey, TEntity> OnKeyGeted(DIFlowBuilderHandler<TKey, TKey> options)
            {
                var o = KeyGeted;
                KeyGeted = x => options(o(x));
                return this;
            }
            public GetApiConfigurations<TKey, TEntity> OnGeted(DIFlowBuilderHandler<TEntity, TEntity> options)
            {
                var o = Geted;
                Geted = x => options(o(x));
                return this;
            }

        }
    }
}