namespace api_screenvault.Model
{
    public class Post
    {
        public required Guid Id { get; set; }
        public User? User { get; set; }
        public string? Title { get; set; }
        public int? Views { get; set; }
        public int? Votes { get; set; }
        public bool IsAnonymous { get; set; }
        public ICollection<string>? Tags { get; set; }
        public string? Uri { get; set; }
        public string? LinkId { get; set; } //a 6 digits and letters code to store image
        public DateTime CreatedAt { get; set; }

        public Post() {

            Views = 0;
            Votes = 0;
            CreatedAt = DateTime.Now;
            }


    }
}
