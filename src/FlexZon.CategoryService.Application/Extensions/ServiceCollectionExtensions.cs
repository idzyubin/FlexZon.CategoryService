using FlexZon.CategoryService.Application.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace FlexZon.CategoryService.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection services) => services
        .RegisterHandler<
            GetCategory.Handler,
            GetCategory.Request,
            GetCategory.Response
        >()
        .RegisterHandler<
            GetCategories.Handler,
            GetCategories.Request,
            GetCategories.Response
        >()
        .RegisterHandler<
            CreateCategory.Handler,
            CreateCategory.Request,
            CreateCategory.Response
        >()
        .RegisterHandler<
            UpdateCategory.Handler,
            UpdateCategory.Request,
            UpdateCategory.Response
        >()
        .RegisterHandler<
            DeleteCategory.Handler,
            DeleteCategory.Request,
            DeleteCategory.Response
        >()
    ;

    private static IServiceCollection RegisterHandler<THandler, TRequest, TResponse>(this IServiceCollection services)
        where THandler : class, IHandler<TRequest, TResponse>
        where TRequest : notnull
        where TResponse : notnull
    {
        services.AddScoped<THandler>();
        services.AddScoped<IHandler<TRequest, TResponse>>(provider =>
            new ValidationHandlerDecorator<TRequest, TResponse>(
                provider.GetRequiredService<THandler>()));
        return services;
    }
}