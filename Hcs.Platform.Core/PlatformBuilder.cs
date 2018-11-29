using System;
using System.Collections.Generic;
using System.Linq;
using Hcs.Platform.Core.PlatformModule;
using Hcs.Platform.PlatformModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Hcs.Platform
{
    public class PlatformBuilder : Hcs.Platform.PlatformModule.IPlatformFunctionService
    {
        private IServiceCollection services;
        List<GenericDataProcessorEntityTaskDefinition> dataProcessorEntityTasks = new List<GenericDataProcessorEntityTaskDefinition>();
        internal IReadOnlyList<GenericDataProcessorEntityTaskDefinition> GenericDataProcessorEntityTasks => dataProcessorEntityTasks;
        private PermissionBuilder everyone;
        internal PlatformBuilder(IServiceCollection services)
        {
            this.services = services;
            var everyoneFunctionBuilder = new ModelFunctionBuilder("Everyone");
            everyoneFunctionBuilder.AddPermission("All", opt => { everyone = opt; });
            functions.Add(everyoneFunctionBuilder);
        }
        public IServiceCollection Services => services;
        public Action<DbContextOptionsBuilder> DbOptions { get; set; }
        public void ConfigDbOptions(Action<DbContextOptionsBuilder> options)
        {
            DbOptions = options;
        }
        List<IPlatformFunction> functions = new List<IPlatformFunction>();

        internal Action<MemoryDistributedCacheOptions> distributedMemoryCacheOptions;
        public PlatformBuilder ConfigDistributedMemoryCache(Action<MemoryDistributedCacheOptions> options)
        {
            distributedMemoryCacheOptions = options;
            return this;
        }

        internal JwtConfigBuilder JwtConfigBuilder { get; } = new JwtConfigBuilder();
        public PlatformBuilder ConfigJwtOptions(Action<JwtConfigBuilder> options)
        {
            options(JwtConfigBuilder);
            return this;
        }

        public PlatformBuilder AddModule<TModule>(TModule module) where TModule : IPlatformModule
        {
            var builder = new PlatformModuleBuilder(services, everyone);
            module.Build(builder);
            functions.AddRange(builder.Functions);
            dataProcessorEntityTasks.AddRange(builder.GenericDataProcessorEntityTasks);
            return this;
        }



        public IEnumerable<IPlatformFunction> Functions => functions;
    }
}