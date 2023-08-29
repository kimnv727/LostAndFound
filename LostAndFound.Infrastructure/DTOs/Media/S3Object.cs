using System.IO;

namespace LostAndFound.Infrastructure.DTOs.Media
{
    public class S3Object
    {
        public string Key { get; set; } = null!;
        public MemoryStream InputStream { get; set; } = null!;
        public string BucketName { get; set; } = null!;
    }
}
