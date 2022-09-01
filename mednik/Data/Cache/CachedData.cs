using mednik.Models;

namespace mednik.Data.Cache;

public static class CachedData
{
    public static IEnumerable<Services>? CachedServices { get; set; }
    
    public static IEnumerable<Post>? CachedPosts { get; set; }
    
    public static IEnumerable<Subject>? CachedSubjects { get; set; }
}