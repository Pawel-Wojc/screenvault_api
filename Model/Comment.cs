namespace api_screenvault.Model
{
    public class Comment
    {
        public Guid Id { get; set; }

        public string Value { get; set; }

        public User User { get; set; }
        public Post Post { get; set; }
    }
}
