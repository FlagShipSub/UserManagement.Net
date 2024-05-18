namespace UserManagment.Models.Dtos
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;

        public UserDto User { get; set; }

        public LoginResponseDto(UserDto user ,string token)
        {
            User = user;
            Token = token;
        }

        public LoginResponseDto()
        {
        }
    }
}   
