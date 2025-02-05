using AngularAuthApplication.Contracts;
using AngularAuthApplication.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AngularAuthAPI.Controllers;
public class AuthenticateController : BaseApiController
{
    #region Dependency Injection (DI) 

    private readonly IAuthenticate _authenticateService;
    public AuthenticateController(IAuthenticate authenticateService)
    {
        _authenticateService = authenticateService;
    }

    #endregion

    #region User Authenticate
    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] AuthenticateUserDto userDto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values;
            errors.SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage)
                .ToList();

            return BadRequest(new
            {
                errors
            });
        }

        var result = await _authenticateService.UserAuthenticateAsync(userDto);
        if (!result.IsSuccess) { return BadRequest(new { Message = result.Message }); }

        return Ok(result);
    }
    #endregion

    #region User Register
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values;
            errors.SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage)
                .ToList();

            return BadRequest(new
            {
                errors
            });
        }

        var registerResult = await _authenticateService.UserRegisterAsync(model);

        if (!registerResult.IsSuccess)
        {
            return BadRequest(new { Message = registerResult.Message });
        }

        return Ok(registerResult);
    }
    #endregion

    #region Get User Detail
    [Authorize]
    [HttpGet("UserDetail/{userName}")]
    public async Task<IActionResult> GetUserDetail(string userName)
    {
        var result = await _authenticateService.GetUserDetailAsync(userName);
        if (!result.IsSuccess) { return BadRequest(new { Message = result.Message }); }

        return Ok(result);
    }
    #endregion

    #region Forget Password
    [HttpPost("forgetPassword")]
    public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordDto identifier)
    {
        var result = await _authenticateService.ForgetPasswordAsync(identifier.Identifier);
        if (!result.IsSuccess) { return BadRequest(new { Message = result.Message }); }

        return Ok(result);
    }
    #endregion

    #region Forget Password
    [HttpPost("resetPassword")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values;
            errors.SelectMany(x => x.Errors)
                .Select(x => x.ErrorMessage)
                .ToList();

            return BadRequest(new
            {
                errors
            });
        }

        var result = await _authenticateService.ResetPasswordAsync(dto);
        if (!result.IsSuccess) { return BadRequest(new { Message = result.Message }); }

        return Ok(result);
    }
    #endregion
}
