namespace mednik.Models;

public class Subject
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public ICollection<Group>? Groups { get; set; }
}