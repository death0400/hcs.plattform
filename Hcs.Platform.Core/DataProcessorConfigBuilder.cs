using System;
using System.Collections.Generic;
using Hcs.Platform.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Hcs.Platform
{
    public class DataProcessorConfigBuilder<TEntity> where TEntity : class
    {

        private const string genericMessage = "interface or abastract class entity task must use AddTask(Type serviceType) and use open geneic type";
        IServiceCollection services;
        List<GenericDataProcessorEntityTaskDefinition> genericTasks = new List<GenericDataProcessorEntityTaskDefinition>();
        internal IReadOnlyList<GenericDataProcessorEntityTaskDefinition> GenricTasks => genericTasks;
        internal DataProcessorConfigBuilder(IServiceCollection services)
        {
            this.services = services;
        }
        public DataProcessorConfigBuilder<TEntity> AddTask<TDataProcessorEntityTask>() where TDataProcessorEntityTask : class, IDataProcessorEntityTask<TEntity>
        {
            if (typeof(TEntity).IsAbstract || typeof(TEntity).IsInterface)
            {
                throw new InvalidOperationException(genericMessage);
            }
            services.AddTransient<IDataProcessorEntityTask<TEntity>, TDataProcessorEntityTask>();
            return this;
        }
        public DataProcessorConfigBuilder<TEntity> AddTask<TDataProcessorEntityTask>(Func<IServiceProvider, TDataProcessorEntityTask> activator) where TDataProcessorEntityTask : class, IDataProcessorEntityTask<TEntity>
        {
            if (typeof(TEntity).IsAbstract || typeof(TEntity).IsInterface)
            {
                throw new InvalidOperationException(genericMessage);
            }
            services.AddTransient<IDataProcessorEntityTask<TEntity>>(activator);
            return this;
        }
        public DataProcessorConfigBuilder<TEntity> AddTask(IDataProcessorEntityTask<TEntity> service)
        {
            if (typeof(TEntity).IsAbstract || typeof(TEntity).IsInterface)
            {
                throw new InvalidOperationException(genericMessage);
            }
            services.AddSingleton<IDataProcessorEntityTask<TEntity>>(service);
            return this;
        }
        public DataProcessorConfigBuilder<TEntity> AddTask(Type serviceType)
        {
            if (serviceType.IsGenericTypeDefinition)
            {
                if (serviceType.GetGenericArguments().Length != 1)
                {
                    throw new ArgumentException($"only support generic type that only has one parameter argument", nameof(serviceType));
                }
                var cts = serviceType.GetGenericArguments()[0].GetGenericParameterConstraints();
                if (cts.Length != 1)
                {
                    throw new ArgumentException($"{serviceType.GetFriendlyName()}'s generic parameter must be {typeof(TEntity)}", nameof(serviceType));
                }
                genericTasks.Add(new GenericDataProcessorEntityTaskDefinition(serviceType, cts[0]));
            }
            else
            {
                if (typeof(TEntity).IsAbstract || typeof(TEntity).IsInterface)
                {
                    throw new InvalidOperationException(genericMessage);
                }
                services.AddTransient(typeof(IDataProcessorEntityTask<TEntity>), serviceType);
            }

            return this;
        }
    }
}