using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain.Identity;

namespace ProEventos.Application;
public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly SymmetricSecurityKey _key;

    public TokenService(IConfiguration configuration, UserManager<User> userManager, IMapper mapper)
    {
        _mapper = mapper;
        _userManager = userManager;
        _configuration = configuration;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"])); // chave para criptografar e descriptografar o token direto do appsettings.json
    }
    public async Task<string> CreateToken(UserUpdateDto userUpdateDto)
    {
        var user = _mapper.Map<User>(userUpdateDto);

        var claims = new List<Claim> // informações que serão armazenadas no token
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName)
        };

        var roles = _userManager.GetRolesAsync(user);
        // claims.AddRange(roles.Result.Select(role => new Claim(ClaimTypes.Role, role))); 
        foreach (var role in await roles) // adicionando as roles do usuário no token
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature); // criptografando a chave

        var tokenDescription = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims), // informações do usuário
            Expires = DateTime.UtcNow.AddHours(2), // tempo de expiração do token
            SigningCredentials = creds // credenciais do token
        };

        var tokenHandler = new JwtSecurityTokenHandler(); // manipulador do token

        var token = tokenHandler.CreateToken(tokenDescription); // criando o token

        return tokenHandler.WriteToken(token); // retornando o token
    }
}