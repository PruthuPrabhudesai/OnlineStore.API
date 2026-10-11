using Microsoft.AspNetCore.Identity;

namespace OnlineStore.API.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
