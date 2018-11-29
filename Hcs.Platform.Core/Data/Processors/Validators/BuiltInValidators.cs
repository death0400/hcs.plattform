using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Hcs.Platform.Flow;
using Hcs.Platform.Data.Processors.Validators;
using Microsoft.AspNetCore.Http;

namespace Hcs.Platform.Data
{
    public static class BuiltInValidators
    {
        public static IDependencyInjectionFlowBuilderContext<EntityValidationResult<TEntity>> Unique<TEntity, TDbContext>(this IDependencyInjectionFlowBuilderContext<EntityValidationResult<TEntity>> builder, Action<UniqueValidateConfigurator<TEntity, TDbContext>> options) where TEntity : class where TDbContext : DbContext
        {
            return builder.Then((IServiceProvider sp) =>
            {
                var validator = new UniqueValidate<TEntity, TDbContext>(sp.GetRequiredService<IScopedDbContext<TDbContext>>());
                var optionContext = new UniqueValidateConfigurator<TEntity, TDbContext>(validator);
                options(optionContext);
                return validator;
            });
        }
        public static IDependencyInjectionFlowBuilderContext<EntityValidationResult<TEntity>> Unique<TEntity>(this IDependencyInjectionFlowBuilderContext<EntityValidationResult<TEntity>> builder, Action<UniqueValidateConfigurator<TEntity, PlatformDbContext>> options) where TEntity : class
        {
            return builder.Unique<TEntity, PlatformDbContext>(options);
        }
        public static IDependencyInjectionFlowBuilderContext<EntityValidationResult<HttpRequest>> ValidOdata<TEntity>(this IDependencyInjectionFlowBuilderContext<EntityValidationResult<HttpRequest>> builder) where TEntity : class
        {
            return builder.Pipe<Odata.IOdataValidator<TEntity>, Odata.IOdataQueryPermission, Core.OdataQueryRequestContext<TEntity>>((validator, permissions, context, errors) =>
            {
                var result = validator.Validate(context.ODataQueryOptions, permissions);
                if (!result.Valid)
                {
                    errors.AddError(new Core.ValidationError
                    {
                        Title = nameof(Odata.IOdataValidator<TEntity>),
                        Message = result.Message
                    });
                }
            });
        }
    }
}