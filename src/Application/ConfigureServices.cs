using Application.Common.Behaviours;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;
public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {

        services.AddMediatR(p =>
        {
            p.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            p.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            p.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            p.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        });
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

        TypeAdapterConfig.GlobalSettings.Scan(typeof(ConfigureServices).Assembly);
        TypeAdapterConfig.GlobalSettings.Default.MapToConstructor(true);
        return services;
    }
}
