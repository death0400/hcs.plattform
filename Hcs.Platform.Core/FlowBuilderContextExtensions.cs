using Hcs.Platform.Flow;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hcs.Platform.Flow
{
    public static class FlowBuilderContextExtensions
    {
        public static IDependencyInjectionFlowBuilderContext<T> Then<T>(this IDependencyInjectionFlowBuilderContext<T> context, DIFlowBuilderHandler<T, T> insert) => insert(context);
        public static IDependencyInjectionFlowBuilderContext<IActionResult> Then<TFlowProcessor>(this IDependencyInjectionFlowBuilderContext<object> context) where TFlowProcessor : IFlowProcessor<object, IActionResult>
        {
            return context.Then<IActionResult, TFlowProcessor>();
        }
        public static IFlowBuilderContext<T> Then<T>(this IFlowBuilderContext<T> context, FlowBuilderHandler<T, T> insert) => insert(context);
        public static IDependencyInjectionFlowBuilderContext<IActionResult> Ok(this IDependencyInjectionFlowBuilderContext<object> builder, bool outputCurrentValue = true)
        {
            return builder.StatusCode(200, outputCurrentValue);
        }
        public static IDependencyInjectionFlowBuilderContext<IActionResult> NotFound(this IDependencyInjectionFlowBuilderContext<object> builder, bool outputCurrentValue = false)
        {
            return builder.StatusCode(404, outputCurrentValue);
        }
        public static IDependencyInjectionFlowBuilderContext<IActionResult> BadRequest(this IDependencyInjectionFlowBuilderContext<object> builder, bool outputCurrentValue = false)
        {
            return builder.StatusCode(400, outputCurrentValue);
        }
        public static IDependencyInjectionFlowBuilderContext<IActionResult> StatusCode(this IDependencyInjectionFlowBuilderContext<object> builder, int statusCode, bool outputCurrentValue = false)
        {
            if (outputCurrentValue)
            {
                return builder.Pipe(x => new ObjectResult(x) { StatusCode = statusCode });
            }
            else
            {
                return builder.Pipe(x => new StatusCodeResult(statusCode));
            }
        }
    }
}