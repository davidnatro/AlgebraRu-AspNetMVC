using System.ComponentModel.DataAnnotations.Schema;

namespace mednik.Models;

public class Group
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public ICollection<Post>? Pdfs { get; set; }

    public Guid? SubjectId { get; set; }

    [ForeignKey("SubjectId")] public Subject? Subject { get; set; }
}