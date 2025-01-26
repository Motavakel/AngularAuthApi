using AngularAuthApplication.Dtos;
namespace AngularAuthApplication.Contracts;

public interface IAuthenticate
{
    Task<AuthResponse<string>> UserAuthenticateAsync(AuthenticateUserDto user);
    Task<AuthResponse> UserRegisterAsync(RegisterUserDto user);
    Task<AuthResponse<UserDetailDto>> GetUserDetailAsync(string userName);
    Task<AuthResponse<string>> ForgetPasswordAsync(string identifier);
    Task<AuthResponse> ResetPasswordAsync(ResetPasswordDto dto);
}


