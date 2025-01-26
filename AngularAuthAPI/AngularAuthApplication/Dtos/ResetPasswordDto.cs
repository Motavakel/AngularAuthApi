using System.ComponentModel.DataAnnotations;

namespace AngularAuthApplication.Dtos;

public class ResetPasswordDto
{
    [Required(ErrorMessage = "وارد کردن نام کاربری الزامی است")]
    [StringLength(50, MinimumLength = 5, ErrorMessage = "نام کاربری باید بین 5 تا 50 کاراکتر باشد")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "وارد کردن کد الزامی است")]
    [MinLength(5, ErrorMessage = "طول کد ارسالی 5 کاراکتر است")]
    [MaxLength(5, ErrorMessage = "طول کد ارسالی 5 کاراکتر است")]
    public string ResetCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "وارد کردن رمز ورود الزامی است")]
    [MinLength(6, ErrorMessage = "طول رمز ورود باید حداقل 8 کاراکتر باشد")]
    [MaxLength(100, ErrorMessage = "طول رمز ورود نباید بیشتر از 100 کاراکتر باشد")]
    public string NewPassword { get; set; } = string.Empty;
}