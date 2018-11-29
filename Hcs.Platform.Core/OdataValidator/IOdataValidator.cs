using Microsoft.AspNet.OData.Query;

namespace Hcs.Platform.Odata
{
    public interface IOdataValidator<TModel>
    {
        OdataValidationResult Validate(ODataQueryOptions<TModel> queryOptions, IOdataQueryPermission odataPermissions);
    }
}
