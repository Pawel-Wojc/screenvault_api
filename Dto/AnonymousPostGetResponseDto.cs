namespace api_screenvault.Dto
{
    public class AnonymousPostGetResponseDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? LinkId { get; set; }
        public string? ErrorMessage { get; set; }
        public bool Error { get; set; }

        public Stream File { get; set; }
    }
}
