using System;
using System.Threading.Tasks;
using Hcs.Platform.Core;
using Hcs.Platform.Flow;

namespace Hcs.Platform.Data
{
    public abstract class ValidationProcessor<TEntity> : AsyncFlowProcessor<EntityValidationResult<TEntity>, EntityValidationResult<TEntity>> where TEntity : class
    {
        public abstract Task Validate(TEntity entit, Action<ValidationError> addError);
        public override async Task ProcessAsync(EntityValidationResult<TEntity> input, Action<EntityValidationResult<TEntity>> done)
        {
            await Validate(input.Data, input.AddError);
            done(input);
        }
    }
}