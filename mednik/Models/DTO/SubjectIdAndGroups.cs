namespace mednik.Models.DTO;

public class SubjectIdAndGroups
{
    public Guid SubjectId { get; set; }

    public IEnumerable<Group> Groups { get; set; }
}