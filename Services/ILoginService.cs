using tokenAuth.Model;

namespace tokenAuth.Services
{
    public interface ILoginService
    {
        Task<string> LoginUser(RequestedUser requestedUser);
    }
}
