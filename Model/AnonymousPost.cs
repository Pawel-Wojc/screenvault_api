using System.Reflection.Metadata;

namespace api_screenvault.Model
{
    public class AnonymousPost
    {
        public required string Id { get; set; }
        public  string ? Title { get; set; }
        public required Blob Picture { get; set; }
    }
}
