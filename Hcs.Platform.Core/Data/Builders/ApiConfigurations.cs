using Hcs.Platform.Flow;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace Hcs.Platform.Data
{
    public class ApiConfigurations
    {
        internal DIFlowBuilderHandler<HttpRequest, HttpRequest> Request { get; set; } = x => x;

        internal DIFlowBuilderHandler<IActionResult, IActionResult> Response { get; set; } = x => x;


    }
}