using AngularAuthAPI.Context;
using AngularAuthAPI.Models;
using AngularAuthApplication.Contracts;
using AngularAuthApplication.Dtos;
using AngularAuthInfrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AngularAuthInfrastructure.Services;

public class Authenticate : IAuthenticate
{

    #region Dependency Injection (DI) 

    private readonly AppDbContext _context;
    public Authenticate(AppDbContext context)
    {
        _context = context;
    }

    #endregion

    #region UserAuthenticateAsync
    public async Task<AuthResponse<string>> UserAuthenticateAsync(AuthenticateUserDto user)
    {
        if (user == null)
            return new AuthResponse<string> { IsSuccess = false, Message = "درخواست نامعتبر", Data = string.Empty };

        var userResult = await _context.Users
            .FirstOrDefaultAsync(x =>
            x.UserName == user.Username);

        if (userResult == null)
            return new AuthResponse<string> { IsSuccess = false, Message = "کاربری با این مشخصات یافت نشد", Data = string.Empty };



        if (!HashPasswordExtensions.VerifyPassword(user.Password, userResult.Password))
        {
            return new AuthResponse<string> { IsSuccess = false, Message = "رمز وارد شده اشتباه است", Data = string.Empty };
        }


        var token = GenerateJwtToken(userResult, TokenType.Authentication);

        return new AuthResponse<string>
        {
            IsSuccess = true,
            Message = "ورود با موفقیت انجام شد",
            Data = token
        };
    }

    #endregion

    #region User Register Async
    public async Task<AuthResponse> UserRegisterAsync(RegisterUserDto model)
    {

        if (await IsUserExistsAsync(model))
            return new AuthResponse { IsSuccess = false, Message = "کاربری با این مشخصات قبلا ثبت نام کرده است" };

        if (!ValidatePassword(model.Password, out string errorMessage))
        {
            return new AuthResponse { IsSuccess = false, Message = errorMessage };
        }



        User user = new();
        user.UserName = model.Username;
        user.Password = model.Password.HashPasswordPbkdf2();
        user.Email = model.Email;
        user.LastName = model.LastName;
        user.FirstName = model.FirstName;


        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return new AuthResponse
        {
            IsSuccess = true,
            Message = "ثبت نام با موفقیت انجام شد",
        };
    }
    #endregion

    #region Get User Detail
    public async Task<AuthResponse<UserDetailDto>> GetUserDetailAsync(string userName)
    {
        var result = await GetUserByUserNameAsync(userName);

        if (result == null)
        {
            return new AuthResponse<UserDetailDto>
            {
                IsSuccess = false,
                Message = "کاربری با این نام کاربری یافت نشد",
            };
        }

        return new AuthResponse<UserDetailDto>
        {
            IsSuccess = true,
            Data = new UserDetailDto
            {
                Email = result.Email,
                LastName = result.LastName,
                FirstName = result.FirstName,
                Username = userName,
            }
        };
    }
    #endregion

    #region Forget Password
    public async Task<AuthResponse<string>> ForgetPasswordAsync(string identifier)
    {
        User? result = null;

        if (identifier.Contains("@"))
        {
             result = await GetUserByEmailAsync(identifier);
            if (result == null)
            {
                return new AuthResponse<string>
                {
                    IsSuccess = false,
                    Message = "کاربری با این ایمیل یافت نشد",
                };
            }
        }
        else
        {
             result = await GetUserByUserNameAsync(identifier);

            if (result == null)
            {
                return new AuthResponse<string>
                {
                    IsSuccess = false,
                    Message = "کاربری با این نام کاربری یافت نشد",
                };
            }
        }

        result.ResetPasswordCode = GenerateRandomCode();
        _context.Users.Update(result);
        await _context.SaveChangesAsync();

        if (result != null)
        {
            await VerifyUserBySendEmailAsync(result);
        }
        else
        {
            return new AuthResponse<string>
            {
                IsSuccess = false,
                Message = "خطا در پیدا کردن کاربر",
            };
        }

        return new AuthResponse<string>
        {
            IsSuccess = true,
            Message = "رمز یکبار مصرف به ایمیل تان ارسال شد",
            Data = GenerateJwtToken(result, TokenType.ResetPassword)
        };

    }
    #endregion


    #region Reset Password
    public async Task<AuthResponse> ResetPasswordAsync(ResetPasswordDto dto)
    {
        var result = await GetUserByUserNameAsync(dto.UserName);

        if (result == null)
        {
            return new AuthResponse
            {
                IsSuccess = false,
                Message = "کاربری با این نام کاربری یافت نشد",
            };
        }

        if (!ValidatePassword(dto.NewPassword, out string errorMessage))
        {
            return new AuthResponse { IsSuccess = false, Message = errorMessage };
        }

        if (result.ResetPasswordCode != dto.ResetCode)
        {
            return new AuthResponse
            {
                IsSuccess = false,
                Message = "کد وارد شده صحیح نیست",
            };
        }

        result.ResetPasswordCode = GenerateRandomCode();
        result.Password = dto.NewPassword.HashPasswordPbkdf2();

        _context.Users.Update(result);
        await _context.SaveChangesAsync();

        return new AuthResponse
        {
            IsSuccess = true,
            Message = "رمز کاربری با موفقیت تغییر پیدا کرد",
        };
    }
    #endregion

    #region Is User Exists
    private async Task<bool> IsUserExistsAsync(RegisterUserDto user)
    {
        return await _context.Users
            .AnyAsync(x => x.UserName == user.Username || x.Email == user.Email);
    }
    #endregion

    #region Get User By Username
    private async Task<User?> GetUserByUserNameAsync(string userName)
    {
        return await _context.Users
            .FirstOrDefaultAsync(x => x.UserName == userName);
    }
    #endregion

    #region Get User By Email
    private async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(x => x.Email == email);
    }
    #endregion

    #region Validate Password
    /*چون باید هم خروجی بول داشته باشیم و هم خروجی رشته و البته اولویت بولین به رشته 
     */
    private bool ValidatePassword(string password, out string errorMessage)
    {
        if (!Regex.IsMatch(password, @"[a-z]"))
        {
            errorMessage = "رمز ورود باید حداقل شامل یک حرف کوچک باشد.";
            return false;
        }

        if (!Regex.IsMatch(password, @"[A-Z]"))
        {
            errorMessage = "رمز ورود باید حداقل شامل یک حرف بزرگ باشد.";
            return false;
        }

        if (!Regex.IsMatch(password, @"[0-9]"))
        {
            errorMessage = "رمز ورود باید حداقل شامل یک عدد باشد.";
            return false;
        }

        if (!Regex.IsMatch(password, @"[@!#$%^&*(),.?""':{}|<>]"))
        {
            errorMessage = "رمز ورود باید شامل حداقل یک کاراکتر خاص باشد.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }
    #endregion

    #region Generate Jwt Token 
    //استفاده هم برای احراز هویت کاربر پس  از لاگین و هم برای ارسال توکن امنیتی جهت فراموشی رمز 
    private string GenerateJwtToken(User user, TokenType tokenType)
    {
        //ایجاد امضای دیجیتال
        var secretKey = Encoding.UTF8.GetBytes("!@#34sdf&*78kjh^rghw%$mnKJLN23a1234567890abcdefghijklmnopqrstuvwxyz");

        // تنظیم زمان انقضای توکن بر اساس نوع
        TimeSpan tokenLifetime = tokenType switch
        {
            TokenType.Authentication => TimeSpan.FromDays(1), 
            TokenType.ResetPassword => TimeSpan.FromMinutes(15),
            _ => throw new ArgumentOutOfRangeException(nameof(tokenType), "Invalid token type")
        };

        //ایجاد پی لود و یا اطلاعات کابر
        var claims = new ClaimsIdentity(new[]
        {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim("TokenType", tokenType.ToString()) 
         });

        //تعریف توکن با این جزئیات
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(secretKey),
            SecurityAlgorithms.HmacSha256
        );

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claims,
            Expires = DateTime.UtcNow.Add(tokenLifetime),
            SigningCredentials = signingCredentials
        };

        //ایجاد توکن براساس اطلاعات تعریف شده
        //ساخت توکن 
        //و برگشت دادن توکن به صورت رشته
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(securityToken);
    }


    public enum TokenType
    {
        Authentication,
        ResetPassword
    }


    #endregion

    #region Verify User By Send Email
    private async Task VerifyUserBySendEmailAsync(User user)
    {
        string emailSubject = "فعالسازی حساب کاربری";
        string emailBody = $@"
            <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
                <h2 style='color: #333;'>کاربر گرامی،</h2>
                <p>برای فعال‌سازی حساب کاربری خود، می‌توانید از شماره فعال‌سازی زیر استفاده کنید:</p>
                <p style='font-size: 1.2em; font-weight: bold; color: #007BFF;'>{user.ResetPasswordCode}</p>
                <p>این شماره را در صفحه فعال‌سازی سایت وارد کنید.</p>
                <hr style='border: none; border-top: 1px solid #ccc; margin: 20px 0;' />
                <p style='margin-top: 20px; font-size: 0.9em; color: #999;'>اگر شما این درخواست را انجام نداده‌اید، این ایمیل را نادیده بگیرید.</p>
            </div>";


        await SendEmailExtention.SendAsync(user.Email, emailSubject, emailBody);
    }
    #endregion

    #region Generate Random Code
    private static string GenerateRandomCode()
    {
        var rand = new Random();
        var code = rand.Next(10000, 99999).ToString();
        return code;
    }
    #endregion
}
