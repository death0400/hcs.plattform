using System;
using System.Collections.Generic;
using System.Linq;
using Hcs.Platform.Data;
using Hcs.Platform.Flow;
using Hcs.Platform.PlatformModule;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace Hcs.Platform.Core.PlatformModule
{
    internal class PlatformModuleBuilder : Hcs.Platform.PlatformModule.IPlatformModuleBuilder
    {
        private IServiceCollection services;
        public IServiceCollection Services => services;
        List<GenericDataProcessorEntityTaskDefinition> dataProcessorEntityTasks = new List<GenericDataProcessorEntityTaskDefinition>();
        internal IReadOnlyList<GenericDataProcessorEntityTaskDefinition> GenericDataProcessorEntityTasks => dataProcessorEntityTasks;
        public PlatformModuleBuilder(IServiceCollection services, PermissionBuilder everyone)
        {
            Everyone = everyone;
            this.services = services;
        }

        public Role.IRoleToken AddApi<TController>(string apiKey, string flowKey, DIFlowBuilderHandler<HttpRequest, IActionResult> buildFlow) where TController : ControllerBase
        {
            var context = new EntityApiContext(apiKey, flowKey, buildFlow, typeof(TController));
            EntityApiContextContainer.Add(context);
            return new KeyRoleToken(flowKey);
        }
        public IPlatformModuleBuilder ConfigDataProcessor<TEntity>(Action<DataProcessorConfigBuilder<TEntity>> options) where TEntity : class
        {
            var builder = new DataProcessorConfigBuilder<TEntity>(services);
            options(builder);
            dataProcessorEntityTasks.AddRange(builder.GenricTasks);
            return this;
        }
        public void AddModel<TModelConfig>() where TModelConfig : IModelConfig, new()
        {
            var config = new TModelConfig();
            DbContextBuilder.Config(config.BuildModel);
            DbContextBuilder.Seed(config.BuildSeedData);
        }
        List<ModelFunctionBuilder> functions = new List<ModelFunctionBuilder>();
        internal IReadOnlyList<ModelFunctionBuilder> Functions => functions;
        public PermissionBuilder Everyone { get; private set; }
        public Hcs.Platform.PlatformModule.IPlatformModuleBuilder AddModuleFuncion(string code, Action<ModelFunctionBuilder> build)
        {
            var builder = new ModelFunctionBuilder(code);
            build(builder);
            functions.Add(builder);
            return this;
        }

    }
}