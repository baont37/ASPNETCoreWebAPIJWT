using ASPNETCoreWebAPIJWT.Model;

namespace ASPNETCoreWebAPIJWT.Repository
{
    public interface IJWTManagerRepository
    {
        Tokens Authenticate(Users users);
        Users GetUserById(string userId);
    }
}
