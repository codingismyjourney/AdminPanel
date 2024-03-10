using Microsoft.AspNetCore.Identity;

namespace AdminPanelWebAPI.Entities;

public class AppUser : IdentityUser<int>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Gender { get; set; }
}
