﻿using System.ComponentModel.DataAnnotations;

namespace AdminPanelWebAPI.DTOs;

public class RegisterDto
{
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [Phone]
    public string PhoneNumber { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string Gender { get; set; }
}