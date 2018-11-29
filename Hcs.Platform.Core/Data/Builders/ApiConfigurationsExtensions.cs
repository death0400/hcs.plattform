using Hcs.Platform.Flow;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace Hcs.Platform.Data
{
    public static class ApiConfigurationsExtensions
    {
        public static T OnRequest<T>(this T cofig, DIFlowBuilderHandler<HttpRequest, HttpRequest> options) where T : ApiConfigurations
        {
            var o = cofig.Request;
            cofig.Request = x => options(o(x));
            return cofig;
        }
        public static T OnResponse<T>(this T cofig, DIFlowBuilderHandler<IActionResult, IActionResult> options) where T : ApiConfigurations
        {
            var o = cofig.Response;
            cofig.Response = x => options(o(x));
            return cofig;
        }
    }
}