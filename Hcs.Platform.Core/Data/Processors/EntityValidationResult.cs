using System.Collections.Generic;
using Hcs.Platform.Core;

namespace Hcs.Platform.Data
{
    public class EntityValidationResult<TData> where TData : class
    {
        public EntityValidationResult(TData entit)
        {
            Data = entit;
        }
        public TData Data { get; }
        public void AddError(ValidationError error)
        {
            this.ValidationErrors.AddIfNotExists(error.Title, () => new List<ValidationError>());
            this.ValidationErrors[error.Title].Add(error);
        }
        public IDictionary<string, IList<ValidationError>> ValidationErrors { get; } = new Dictionary<string, IList<ValidationError>>();
    }
}