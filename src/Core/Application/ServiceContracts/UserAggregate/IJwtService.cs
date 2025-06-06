using Application.ViewModels.AccountAggregate;

namespace Application.ServiceContracts.UserAggregate;

public interface IJwtService
{
    GenerateTokenResponseBaseViewModel GenerateToken(GenerateTokenAddCommandModel model);
    bool Validate(string token);
    public Task<DecodeToken> DecodeToken(string tokenString);
    string GenerateRefreshToken(GenerateRefreshTokenAddCommandModel model);
    bool ValidateRefreshToken(string refreshToken);
    Task<DecodeRefreshToken> DecodeRefreshToken(string refreshToken);
    FullTokenResponseBaseViewModel GenerateFullToken(FullTokenResponseCommandModel model);
    string GenerateTokenForForgotPassword(GenerateTokenForForgotPasswordCommandModel model);
    bool ValidateTokenForForgotPassword(string forgotToken);
    Task<DecodeTokenForForgotPassword> DecodeTokenForForgotPassword(string forgotToken);

}