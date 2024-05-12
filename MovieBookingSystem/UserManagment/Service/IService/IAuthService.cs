using UserManagment.Models;
using UserManagment.Models.Dtos;

namespace UserManagment.Service.IService
{
    public interface IAuthService
    {
        Task<Response<object>> Register(RegisterationRequestDto requestDto);

        Task<Response<LoginResponseDto>> Login(LoginRequestDto requestDto);
    }
}
