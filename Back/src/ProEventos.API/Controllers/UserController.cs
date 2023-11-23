using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;

namespace ProEventos.API.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private IUserService _userService;
    private ITokenService _tokenService;

    public UserController(IUserService userService, ITokenService tokenService)
    {
        _userService = userService;
        _tokenService = tokenService;
    }

    [HttpGet("GetUser", Name = "GetUser")]
    public async Task<IActionResult> GetUser()
    {
        try
        {
            var username = User.GetUserName();
            var user = await _userService.GetUserByUsernameAsync(username);
            return Ok(user);
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro {e.Message}");
        }
    }

    [HttpPost("Register", Name = "RegisterUser")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(UserDto userDto)
    {
        try
        {
            if (await _userService.UserExists(userDto.Username)) return BadRequest("Usuário já cadastrado");

            var user = await _userService.CreateAccountAsync(userDto);
            if (user != null)
            {
                var userToReturn = _userService.GetUserByUsernameAsync(userDto.Username);
                return Created("", userToReturn);
            }
            return BadRequest("Erro ao cadastrar usuário");

        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro {e.Message}");
        }
    }

    [HttpPost("Login", Name = "LoginUser")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(UserLoginDto userLoginDto)
    {
        try
        {
            var user = await _userService.GetUserByUsernameAsync(userLoginDto.Username);
            if (user == null) return Unauthorized("Usuário não cadastrado");

            var result = await _userService.CheckUserPasswordAsync(user, userLoginDto.Password);
            if (result.Succeeded)
            {
                return Ok(new
                {
                    username = user.Username,
                    primeiroNome = user.PrimeiroNome,
                    token = _tokenService.CreateToken(user).Result
                });
            }
            return Unauthorized("Usuário ou senha incorretos");
        }
        catch (System.Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro {e.Message}");
        }
    }

    [HttpPut("UpdateUser")]
    public async Task<IActionResult> UpdateUser(UserUpdateDto userUpdateDto)
    {
        try
        {
            if (userUpdateDto.Username != User.GetUserName())
                return Unauthorized("Usuário Inválido");

            var user = await _userService.GetUserByUsernameAsync(User.GetUserName());
            if (user == null) return Unauthorized("Usuário Inválido");

            var userReturn = await _userService.UpdateAccount(userUpdateDto);
            if (userReturn == null) return NoContent();

            return Ok(userReturn);
        }
        catch (Exception e)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro {e.Message}");
        }
    }

}