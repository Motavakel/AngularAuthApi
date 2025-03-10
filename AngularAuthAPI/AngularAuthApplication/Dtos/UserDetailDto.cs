﻿using System.ComponentModel.DataAnnotations;

namespace AngularAuthApplication.Dtos;

public class UserDetailDto
{
    public string Username { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
