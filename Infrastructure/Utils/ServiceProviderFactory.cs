using Common.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Utils;

public class ServiceProviderFactory(IServiceProvider rootServiceProvider) : IServiceProviderFactory, IDisposable
{
    public IServiceProvider CreateScope()
    {
        var scope = rootServiceProvider.CreateScope();
        return scope.ServiceProvider;
    }

    public void Dispose()
    {
        if (rootServiceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}
