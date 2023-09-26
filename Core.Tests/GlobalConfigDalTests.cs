using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PersistentGlobalConfig;

namespace Core.Tests;

public class GlobalConfigDalTests
{
    private static IGlobalConfigDal<GlobalConfig> ResolveGlobalConfigDal(VersionByEnum versionByEnum)
    {
        var serviceProvider = new ServiceCollection()
            .Configure<LoggerFilterOptions>(cfg => cfg.MinLevel = LogLevel.None)
            .AddDbContext<EntityDbContext>(x => x.UseInMemoryDatabase("db"))
            .AddGlobalConfig<EntityDbContext, GlobalConfig>(versionByEnum: versionByEnum)
            .BuildServiceProvider();

        return serviceProvider.GetRequiredService<IGlobalConfigDal<GlobalConfig>>();
    }
    
    [Theory]
    [InlineData(VersionByEnum.NoVersion)]
    [InlineData(VersionByEnum.TimeStamp)]
    public async Task Test_ShouldPersist(VersionByEnum versionByEnum)
    {
        var globalConfigDal = ResolveGlobalConfigDal(versionByEnum);

        var globalConfig = await globalConfigDal.Get();
        Assert.NotNull(globalConfig);
        
        globalConfig.Name = "foo";
        await globalConfigDal.Update(globalConfig);

        globalConfig = await globalConfigDal.Get();
        Assert.Equal("foo", globalConfig.Name);
        
        globalConfig.Name = "bar";
        await globalConfigDal.Update(globalConfig);
        Assert.Equal("bar", globalConfig.Name);
    }
}