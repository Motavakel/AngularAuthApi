using AngularAuthAPI.Controllers;
using AngularAuthApplication.Contracts;
using AngularAuthApplication.Dtos;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json.Linq;

namespace AngularAuthTest.Controller;

public class AuthenticateControllerTest
{
    private readonly Mock<IAuthenticate> _serviceMock;
    private readonly AuthenticateController _controller;


    public AuthenticateControllerTest()
    {
        _serviceMock = new Mock<IAuthenticate>();
        _controller =  new AuthenticateController(_serviceMock.Object);
    }

    [Fact]
    public async Task Return_Authenticate_Corect_Response()
    {
        //arrang
        var responseMock = new AuthResponse<string>
        {
            IsSuccess = true,
            Message = "ورود با موفقیت انجام شد",
        };

        var requestMock = new AuthenticateUserDto
        {
            Password = "",
            Username = "milad_mo",
        };

        _serviceMock.Setup(s => s.UserAuthenticateAsync(It.IsAny<AuthenticateUserDto>())).ReturnsAsync(responseMock);
       
        // act
        var result = await _controller.Authenticate(requestMock);


        // asset
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<AuthResponse<string>>(okResult.Value);
        Assert.NotNull(response);
        Assert.True(response.IsSuccess);
        Assert.Equal("ورود با موفقیت انجام شد", response.Message);
       
    }
}
