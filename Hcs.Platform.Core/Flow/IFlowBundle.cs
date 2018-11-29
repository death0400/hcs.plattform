using Hcs.Platform.Flow;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hcs.Platform.Flow
{
    public interface IFlowBundle
    {
        IFlowProcessor<HttpRequest, IActionResult> GetFlowProcessor(string key);
    }
}