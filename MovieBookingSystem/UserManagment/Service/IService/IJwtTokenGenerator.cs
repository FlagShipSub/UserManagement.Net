using UserManagment.Models;

namespace UserManagment.Service.IService
{
    public interface IJwtTokenGenerator
    {
        public string GeneraterToken(ApplicationUser applicationUser);
    }
}
