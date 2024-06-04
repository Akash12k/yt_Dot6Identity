using yt_Dot6Identity.Models.DTO;

namespace yt_Dot6Identity.Repositories.Abstract
{
    public interface IUserAuthenticationService
    {
        Task<Status> LoginAsyc(LoginModel model);
        Task<Status>RegsitrationAsync(RegistrationModel model);

        Task LogoutAsync();
    }
}
