using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace PersistentGlobalConfig;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddGlobalConfig<TDbContext, TGlobalConfig>(this IServiceCollection collection, VersionByEnum versionByEnum = VersionByEnum.NoVersion, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) where TDbContext : DbContext where TGlobalConfig : class, IGlobalConfig, new()
    {
        collection.Add(
            ServiceDescriptor.Describe(
                typeof(IGlobalConfigDal<TGlobalConfig>),
                serviceProvider => new GlobalConfigDal<TGlobalConfig>(serviceProvider.GetRequiredService<TDbContext>(), versionByEnum),
                serviceLifetime));

        return collection;
    }
}