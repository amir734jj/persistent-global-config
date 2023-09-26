using System.ComponentModel.DataAnnotations.Schema;

namespace PersistentGlobalConfig;

public interface IGlobalConfig
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public DateTimeOffset Changed { get; set; }
}
