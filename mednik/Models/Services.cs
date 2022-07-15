using mednik.Data.Base;

namespace mednik.Models;

public class Services : IEntityBase
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }

    public string Link { get; set; }
}