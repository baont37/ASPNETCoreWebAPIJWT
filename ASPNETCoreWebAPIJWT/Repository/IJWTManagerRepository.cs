using ASPNETCoreWebAPIJWT.Model;

namespace ASPNETCoreWebAPIJWT.Repository
{
    public interface IJWTManagerRepository
    {
        Token Authenticate(User users);
        User GetUserById(string userId);

        bool ValidateToken(string authToken);

        Token NewAccessToken();

    }
}
