using System.Security.Cryptography;
using System.Text;
using DestifyMovies.Core.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DestifyMovies.Core.Services.Authorization;

public interface IApiKeyService
{
    IIdentityContext? IdentityContext { get; }
    User? ValidateKey(string key);
    TokenResponse GenerateKey();
}

public struct TokenResponse
{
    public TokenResponse(string userToken, string appToken)
    {
        UserToken = userToken;
        AppToken = appToken;
    }
    public string UserToken { get; }
    public string AppToken { get; }
}

public class ApplicationKeyService : IApiKeyService
{
    private readonly IConfiguration _configuration;
    private readonly DbContext _dbContext;

    public IIdentityContext? IdentityContext { get; }


    public ApplicationKeyService(IConfiguration configuration, DbContext dbContext, IIdentityContextFactory factory)
    {
        _configuration = configuration;
        _dbContext = dbContext;
        IdentityContext = factory.Create(this);
    }

    public User? ValidateKey(string key)
    {

        var hash = ComputeHash(ApplyAppPrefix(key));

        var user = _dbContext.Set<User>()
            .AsNoTracking()
            .FirstOrDefault(x => x.UserApiToken == hash);

        return user;
    }

    public TokenResponse GenerateKey()
    {
        var key = new byte[32];
        using var generator = RandomNumberGenerator.Create();
            generator.GetBytes(key);

        var userToken = Convert.ToBase64String(key);
        var completeToken = ApplyAppPrefix(userToken);
        var appToken = ComputeHash(completeToken);

        return new TokenResponse(userToken, appToken);
    }

    private string ApplyAppPrefix(string key) => $"{_configuration["ApiTokenPrefix"]}{key}";

    private static string ComputeHash(string message)
    {
        using var sha512 = SHA512.Create();
        var messageBytes = Encoding.UTF8.GetBytes(message);
        var bytes = sha512.ComputeHash(messageBytes);
        var hashedInputStringBuilder = new StringBuilder(128);
        foreach (var b in bytes)
            hashedInputStringBuilder.Append(b.ToString("X2"));

        return hashedInputStringBuilder.ToString();
    }
}