using Backend_AYNI.ResponseModels;

namespace Backend_AYNI.Services
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(string userName, string email, string password);
        Task<AuthenticationResult> LoginAsync(string email, string password);
    }
}
