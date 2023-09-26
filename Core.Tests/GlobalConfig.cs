using PersistentGlobalConfig;

namespace Core.Tests;

public class GlobalConfig : IGlobalConfig
{
    public int Id { get; set; }
    
    public DateTimeOffset Changed { get; set; }
    
    public string? Name { get; set; }
}