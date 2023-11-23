using AutoMapper;
using Microsoft.AspNetCore.Identity;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain.Identity;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application;
public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IUserPersistence _userPersist;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    public UserService(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, IUserPersistence userPersist)
    {
        _userPersist = userPersist;
        _mapper = mapper;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<SignInResult> CheckUserPasswordAsync(UserUpdateDto userUpdateDto, string password)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(userUpdateDto.Username.ToLower());

            return await _signInManager.CheckPasswordSignInAsync(user, password, false);
        }
        catch (System.Exception e)
        {

            throw new Exception(e.Message);
        }
    }

    public async Task<UserDto> CreateAccountAsync(UserDto userDto)
    {
        try
        {
            var user = _mapper.Map<User>(userDto);
            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (result.Succeeded)
            {
                var userToReturn = _mapper.Map<UserDto>(user);
                return userToReturn;
            }
            return null;
        }
        catch (System.Exception e)
        {

            throw new Exception(e.Message);
        }
    }

    public async Task<UserUpdateDto> GetUserByUsernameAsync(string username)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(username.ToLower());
            if (user == null) return null;
            return _mapper.Map<UserUpdateDto>(user);
        }
        catch (System.Exception e)
        {

            throw new Exception(e.Message);
        }
    }

    public async Task<UserUpdateDto> UpdateAccount(UserUpdateDto userUpdateDto)
    {
        try
        {
            var user = await _userPersist.GetUserByUsernameAsync(userUpdateDto.Username);
            if (user == null) return null;
            userUpdateDto.Id = user.Id;

            _mapper.Map(userUpdateDto, user);

            if (userUpdateDto.Password != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, userUpdateDto.Password);
            }

            _userPersist.Update<User>(user);

            if (await _userPersist.SaveChangesAsync())
            {
                var userToReturn = await _userPersist.GetUserByUsernameAsync(user.UserName);
                return _mapper.Map<UserUpdateDto>(userToReturn);
            }

            return null;
        }
        catch (System.Exception e)
        {

            throw new Exception(e.Message);
        }
    }

    public async Task<bool> UserExists(string username)
    {
        try
        {
            return await _userManager.FindByNameAsync(username.ToLower()) != null;
        }
        catch (System.Exception e)
        {

            throw new Exception(e.Message);
        }
    }
}