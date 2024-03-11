using System.ComponentModel.DataAnnotations;

namespace AdminPanelWebAPI.DTOs;

public class UpdateUserDto
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    [Phone]
    public string PhoneNumber { get; set; }

    public string Gender { get; set; }
}
