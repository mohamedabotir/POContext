using Common.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Utils;

public class ServiceProviderFactory : IServiceProviderFactory, IDisposable
{
    private readonly IServiceProvider _rootServiceProvider;

    public ServiceProviderFactory(IServiceProvider rootServiceProvider)
    {
        _rootServiceProvider = rootServiceProvider;
    }

    public IServiceProvider CreateScope()
    {
        var scope = _rootServiceProvider.CreateScope();
        return scope.ServiceProvider;
    }

    public void Dispose()
    {
        if (_rootServiceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}
