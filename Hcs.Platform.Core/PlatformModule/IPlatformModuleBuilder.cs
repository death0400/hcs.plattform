using System;
using Hcs.Platform.Data;
using Hcs.Platform.Core;
using Hcs.Platform.Flow;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Hcs.Platform.PlatformModule
{
    public interface IPlatformModuleBuilder
    {
        PermissionBuilder Everyone { get; }
        Role.IRoleToken AddApi<TController>(string apiKey, string flowKey, DIFlowBuilderHandler<HttpRequest, IActionResult> buildFlow) where TController : ControllerBase;
        void AddModel<TModelConfig>() where TModelConfig : IModelConfig, new();
        IPlatformModuleBuilder ConfigDataProcessor<TEntity>(Action<DataProcessorConfigBuilder<TEntity>> options) where TEntity : class;
        IPlatformModuleBuilder AddModuleFuncion(string code, Action<ModelFunctionBuilder> build);
        IServiceCollection Services { get; }
    }
    public static class IPlatformModuleBuilderExtensions
    {
        public static IPlatformModuleBuilder AddModuleFuncion(this IPlatformModuleBuilder builder, string module, string function, Action<ModelFunctionBuilder> build)
        {
            builder.AddModuleFuncion($"{module}.{function}", build);
            return builder;
        }
    }
}