using System.Reflection;
using Orders.Api.Commands;
using Orders.Api.Commands.CommandHandlers;
using Orders.Api.Queries.QueryHandlers;

namespace Orders.Api;

public static class Extensions
{
    public static IServiceCollection AddCommandHandlers(this IServiceCollection collection, Type assemblyType)
    {
        if (assemblyType == null) throw new ArgumentNullException(nameof(assemblyType));
        var assembly = assemblyType.Assembly;
        var scanType = typeof(ICommandHandler<>);
        
        RegisterScanTypeWithImplementations(collection, assembly, scanType);

        return collection;
    }

    public static IServiceCollection AddQueryHandlers(this IServiceCollection collection, Type assemblyType)
    {
        if (assemblyType == null) throw new ArgumentNullException(nameof(assemblyType));
        var assembly = assemblyType.Assembly;
        var scanType = typeof(IQueryHandler<,>);
        
        RegisterScanTypeWithImplementations(collection, assembly, scanType);

        return collection;
    }
    
    private static void RegisterScanTypeWithImplementations(IServiceCollection collection, Assembly assembly, Type scanType)
    {
        var commandHandlers = ScanTypes(assembly, scanType);

        foreach (var handler in commandHandlers)
        {
            var abstraction = handler.GetTypeInfo().ImplementedInterfaces
                .First(type => type.IsGenericType && type.GetGenericTypeDefinition() == scanType);
            
            collection.AddScoped(abstraction, handler);
        }
    }

    private static IEnumerable<Type> ScanTypes(Assembly assembly, Type typeToScanFor)
    {
        return assembly.GetTypes()
            .Where(type => type is
                           {
                               IsClass: true,
                               IsAbstract: false
                           } &&
                           type.GetInterfaces()
                               .Any(i=>i.IsGenericType && i.GetGenericTypeDefinition() == typeToScanFor));
    }
}