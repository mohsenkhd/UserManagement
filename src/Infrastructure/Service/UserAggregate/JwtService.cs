using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Application.ServiceContracts.UserAggregate;
using Application.ViewModels.AccountAggregate;
using Common.Exceptions.UserManagement;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Service.UserAggregate;

public class JwtService : IJwtService
{
    private readonly string _privateKeyString;
    private readonly string _publicKeyString;


    public JwtService(IConfiguration configuration)
    {
        _privateKeyString = File.ReadAllText(Path.Combine(
            configuration["KeyStorePath"], configuration["Keys:PrivateKey"]));
        _publicKeyString = File.ReadAllText(Path.Combine(
            configuration["KeyStorePath"], configuration["Keys:PublicKey"]));
    }

    public async Task<DecodeToken> DecodeToken(string tokenString)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var token = tokenHandler.ReadJwtToken(tokenString);

            var permissionIdsFromToken = token.Claims
                .Where(claim => claim.Type == "Permission")
                .Select(claim => claim.Value)
                .ToList();

            var userIdClaim = token.Claims.FirstOrDefault(claim => claim.Type == "UserId")!.Value;
            var groupIdClaim = token.Claims.FirstOrDefault(claim => claim.Type == "GroupId")!.Value;
            var firstNameClaim = token.Claims.FirstOrDefault(claim => claim.Type == "FirstName")!.Value;
            var lastNameClaim = token.Claims.FirstOrDefault(claim => claim.Type == "LastName")!.Value;
            var phoneClaim = token.Claims.FirstOrDefault(claim => claim.Type == "Phone")!.Value;
            var customerNumberClaim = token.Claims.FirstOrDefault(claim => claim.Type == "CustomerNumber")!.Value;

            return await Task.FromResult(new DecodeToken()
            {
                PermissionIds = permissionIdsFromToken,
                UserId = userIdClaim,
                GroupId = groupIdClaim,
                FirstName = firstNameClaim,
                LastName = lastNameClaim,
                Phone = phoneClaim,
                CustomerNumber = customerNumberClaim
            });
        }
        catch (Exception)
        {
            // Handle exceptions, log them, and possibly throw custom exceptions
            throw UserManagementExceptions.TokenNotValidException;
        }
    }

    public GenerateTokenResponseBaseViewModel GenerateToken(GenerateTokenAddCommandModel model)
    {
        var key = RSA.Create();
        key.ImportFromPem(_privateKeyString.ToCharArray());

        var handler = new JsonWebTokenHandler();

        var claims = new List<Claim>
        {
            new("UserId", model.UserId.ToString()),
            new("GroupId", model.ApplicationId.ToString()),
            new("IsAdmin", model.IsAdmin.ToString()),
            new("FirstName", model.FirstName ?? ""),
            new("LastName", model.LastName ?? ""),
            new("Phone", model.Phone),
            new("CustomerNumber", model.CustomerNumber.ToString() ?? "")
        };
        claims.AddRange(model.PermissionIds.Select(permission => new Claim("Permission", permission.ToString())));

        var claimsIdentity = new ClaimsIdentity(claims);


        var accessToken = handler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = "Ava",
            Audience = "UserManagement",
            Subject = claimsIdentity,
            Expires = DateTime.Now.AddMinutes(30),
            SigningCredentials = new SigningCredentials(new RsaSecurityKey(key),
                SecurityAlgorithms.RsaSha256)
        });
        return new GenerateTokenResponseBaseViewModel
        {
            Token = accessToken,
        };
    }

    public bool Validate(string token)
    {
        var key = RSA.Create();
        key.ImportFromPem(_publicKeyString.ToCharArray());

        var handler = new JsonWebTokenHandler();
        var result = handler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateLifetime = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = "Ava",
            ValidAudience = "UserManagement",
            IssuerSigningKey = new RsaSecurityKey(key),
        });
        return result.IsValid;
    }

    public string GenerateRefreshToken(GenerateRefreshTokenAddCommandModel model)
    {
        var key = RSA.Create();
        key.ImportFromPem(_privateKeyString.ToCharArray());

        var handler = new JsonWebTokenHandler();

        // Create Claims of Refresh Token
        var refreshClaims = new List<Claim>
        {
            new Claim("UserId", model.UserId.ToString()),
        };

        var refreshClaimsIdentity = new ClaimsIdentity(refreshClaims);


        var refreshToken = handler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = "Ava",
            Audience = "UserManagement",
            Subject = refreshClaimsIdentity,
            SigningCredentials = new SigningCredentials(new RsaSecurityKey(key),
                SecurityAlgorithms.RsaSha256)
        });

        return refreshToken;
    }

    public bool ValidateRefreshToken(string refreshToken)
    {
        var key = RSA.Create();
        key.ImportFromPem(_privateKeyString.ToCharArray());
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            IssuerSigningKey = new RsaSecurityKey(key),
            ValidIssuer = "Ava",
            ValidAudience = "UserManagement"
        };

        try
        {
            SecurityToken validatedToken;
            tokenHandler.ValidateToken(refreshToken, validationParameters, out validatedToken);
            return true;
        }
        catch (SecurityTokenValidationException)
        {
            return false;
        }
    }


    public async Task<DecodeRefreshToken> DecodeRefreshToken(string refreshToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var token = tokenHandler.ReadJwtToken(refreshToken);

            var userIdClaim = token.Claims.FirstOrDefault(claim => claim.Type == "UserId")!.Value;

            return await Task.FromResult(new DecodeRefreshToken()
            {
                UserId = userIdClaim,
            });
        }
        catch (Exception)
        {
            // Handle exceptions, log them, and possibly throw custom exceptions
            throw UserManagementExceptions.RefreshTokenNotValidException;
        }
    }

    public FullTokenResponseBaseViewModel GenerateFullToken(FullTokenResponseCommandModel model)
    {
        var tokenModel = new GenerateTokenAddCommandModel()
        {
            ApplicationId = model.ApplicationId,
            FirstName = model.FirstName,
            LastName = model.LastName,
            IsAdmin = model.IsAdmin,
            PermissionIds = model.PermissionIds,
            Phone = model.Phone,
            UserId = model.UserId,
            CustomerNumber = model.CustomerNumber
        };
        var token = GenerateToken(tokenModel);
        var refreshTokenModel = new GenerateRefreshTokenAddCommandModel()
        {
            UserId = model.UserId,
        };
        var refreshToken = GenerateRefreshToken(refreshTokenModel);

        return new FullTokenResponseBaseViewModel()
        {
            Token = token.Token,
            RefreshToken = refreshToken,
        };
    }
    public string GenerateTokenForForgotPassword(GenerateTokenForForgotPasswordCommandModel model)
    {
        var key = RSA.Create();
        key.ImportFromPem(_privateKeyString.ToCharArray());

        var handler = new JsonWebTokenHandler();

        
        var refreshClaims = new List<Claim>
        {
            new Claim("UserId", model.UserId.ToString()),
        };

        var forgotTokenClaimsIdentity = new ClaimsIdentity(refreshClaims);


        var forgotToken = handler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = "ForgotPassword",
            Audience = "UserManagement",
            Subject = forgotTokenClaimsIdentity,
            SigningCredentials = new SigningCredentials(new RsaSecurityKey(key),
                SecurityAlgorithms.RsaSha256)
        });

        return forgotToken;
    }

    public bool ValidateTokenForForgotPassword(string forgotToken)
    {
        var key = RSA.Create();
        key.ImportFromPem(_privateKeyString.ToCharArray());

        var tokenHandler = new JwtSecurityTokenHandler();

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new RsaSecurityKey(key),
            ValidIssuer = "ForgotPassword",
            ValidAudience = "UserManagement"
        };

        try
        {
            SecurityToken validatedToken;
            tokenHandler.ValidateToken(forgotToken, validationParameters, out validatedToken);

            // چک کردن تاریخ انقضاء
            if (validatedToken.ValidTo < DateTime.Now)
            {
                // تاریخ انقضاء گذشته است، این توکن منقضی شده است
                return false;
            }

            // اگر توکن معتبر باشد و تاریخ انقضاء هم به انقضاء نرسیده باشد، اعتبارسنجی موفقیت‌آمیز است
            return true;
        }
        catch (SecurityTokenValidationException)
        {
            // اعتبارسنجی ناموفق بوده است
            return false;
        }
    }
    public async Task<DecodeTokenForForgotPassword> DecodeTokenForForgotPassword(string forgotToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var token = tokenHandler.ReadJwtToken(forgotToken);

            var userIdClaim = token.Claims.FirstOrDefault(claim => claim.Type == "UserId")!.Value;

            return await Task.FromResult(new DecodeTokenForForgotPassword()
            {
                UserId = userIdClaim,
            });
        }
        catch (Exception)
        {
            // Handle exceptions, log them, and possibly throw custom exceptions
            throw UserManagementExceptions.ForgotTokenNotValidException;
        }
    }
}