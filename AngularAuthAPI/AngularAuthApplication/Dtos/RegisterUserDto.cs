using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace AngularAuthApplication.Dtos;

public class RegisterUserDto
{
    [Required(ErrorMessage = "وارد کردن نام کاربری الزامی است")]
    [StringLength(50, MinimumLength = 5, ErrorMessage = "نام کاربری باید بین 5 تا 50 کاراکتر باشد")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "وارد کردن رمز ورود الزامی است")]
    [MinLength(6, ErrorMessage = "طول رمز ورود باید حداقل 8 کاراکتر باشد")]
    [MaxLength(100, ErrorMessage = "طول رمز ورود نباید بیشتر از 100 کاراکتر باشد")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "وارد کردن نام الزامی است")]
    [StringLength(50, ErrorMessage = "نام نمی‌تواند بیشتر از 50 کاراکتر باشد")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "وارد کردن نام خانوادگی الزامی است")]
    [StringLength(50, ErrorMessage = "نام خانوادگی نمی‌تواند بیشتر از 50 کاراکتر باشد")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "وارد کردن ایمیل الزامی است")]
    [EmailAddress(ErrorMessage = "ایمیل وارد شده معتبر نیست")]
    [StringLength(100, ErrorMessage = "ایمیل نمی‌تواند بیشتر از 100 کاراکتر باشد")]
    public string Email { get; set; } = string.Empty;
}
