using Hcs.Platform.Flow;

namespace Hcs.Platform.Data
{
    public class ValidationStart<TEntity> : FlowProcessor<TEntity, EntityValidationResult<TEntity>> where TEntity : class
    {
        public override EntityValidationResult<TEntity> Process(TEntity input)
        {
            return new EntityValidationResult<TEntity>(input);
        }
    }
}