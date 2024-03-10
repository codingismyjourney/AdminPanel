using AdminPanelWebAPI.Entities;

namespace AdminPanelWebAPI.Interface;

public interface ITokenService
{
    string CreateToken(AppUser user);
}
