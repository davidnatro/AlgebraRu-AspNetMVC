namespace mednik.Models.DTO;

public class PostDTO
{
    public string Name { get; set; }

    public string? Description { get; set; }

    public IFormFile FileData { get; set; }
}