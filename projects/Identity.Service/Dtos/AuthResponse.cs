namespace Identity.Service.Dtos
{
    public class AuthResponse
    {
        public string Token { get; set; } 
        public string? RefreshToken { get; set; }  
        public DateTime ExpiresAt { get; set; }  
    }
}
