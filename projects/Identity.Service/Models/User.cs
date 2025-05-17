using Microsoft.AspNetCore.Identity;

namespace Identity.Service.Models
{
    public class User :IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
