using System.ComponentModel.DataAnnotations;

namespace AngularAuthApplication.Dtos;

public class AuthenticateUserDto
{
    [Required(ErrorMessage = "وارد کردن نام کاربری الزامی است")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "وارد کردن رمز ورود الزامی است")]
    [MinLength(6, ErrorMessage = "طول رمز ورود باید بیشتر از 6 کاراکتر باشد")]
    public string Password { get; set; } = string.Empty;
}
