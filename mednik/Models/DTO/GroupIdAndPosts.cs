namespace mednik.Models.DTO;

public class GroupIdAndPosts
{
    public Guid GroupId { get; set; }

    public IEnumerable<Post> Posts { get; set; }
}