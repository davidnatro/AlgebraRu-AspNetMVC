namespace mednik.Models.DTO;

public class GroupPostDTO
{
    public Guid GroupdId { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public IFormFile FileData { get; set; }
}