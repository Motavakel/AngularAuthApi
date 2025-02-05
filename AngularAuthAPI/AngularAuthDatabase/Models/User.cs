﻿using System.ComponentModel.DataAnnotations;
namespace AngularAuthAPI.Models;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ResetPasswordCode { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty ;
}