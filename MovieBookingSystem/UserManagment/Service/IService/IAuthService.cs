using UserManagment.Models;
using UserManagment.Models.Dtos;

namespace UserManagment.Service.IService
{
    public interface IAuthService
    {
        Task<Response<object>> Register(RegisterationRequestDto requestDto);

        Task<Response<LoginResponseDto>> Login(LoginRequestDto requestDto);
        Task<Response<string>> GenerateOtp(VerificationRequest requestDto);

        Task<Response<object>> GetAllUsers();
    }
}
