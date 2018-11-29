using System.Linq;
using System.Collections.Generic;
namespace Hcs.Platform.Core
{
    public class ValidationException : System.Exception
    {
        public ValidationException(string message, IDictionary<string, ValidationError[]> errors) : base(message)
        {
            ValidationErrors = errors;
        }
        public ValidationException(string message) : this(message, null) { }
        public IDictionary<string, ValidationError[]> ValidationErrors { get; }

    }
    public class ValidationError
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
    }
}