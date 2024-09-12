using Microsoft.AspNetCore.Identity;

namespace api_screenvault.Model
{
    public class User : IdentityUser
    {
        bool IsPremium { get; set; }

        ICollection<Post>? Posts { get; set; }

        public User() { 
            IsPremium = false;
        }
    }
}
