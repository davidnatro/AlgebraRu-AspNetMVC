namespace mednik.Models;

public class Post
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }

    public string Description { get; set; }

    public string ImageURL { get; set; }
}