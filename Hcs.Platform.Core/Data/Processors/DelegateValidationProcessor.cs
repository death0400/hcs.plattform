using System;
using System.Threading.Tasks;
using Hcs.Platform.Core;

namespace Hcs.Platform.Data
{
    public class DelegateValidationProcessor<TEntity> : ValidationProcessor<TEntity> where TEntity : class
    {
        private readonly Func<TEntity, Action<ValidationError>, Task> validate;

        public DelegateValidationProcessor(Func<TEntity, Action<ValidationError>, Task> validate)
        {
            this.validate = validate ?? throw new ArgumentNullException(nameof(validate));
        }
        public override async Task Validate(TEntity entit, Action<ValidationError> addError)
        {
            await validate(entit, addError);
        }
    }
}