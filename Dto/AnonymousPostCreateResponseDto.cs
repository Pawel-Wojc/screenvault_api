using api_screenvault.Model;

namespace api_screenvault.Dto
{
    public class AnonymousPostCreateResponseDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? LinkId { get; set; }
        public string? ErrorMessage { get; set; }
        public bool Error { get; set; }


    }
}
