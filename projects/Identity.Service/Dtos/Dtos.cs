namespace Identity.Service.Dtos
{
    public record CreateUserDto(
     string Email,
     string Password,
     string FirstName,
     string LastName
 );
}
