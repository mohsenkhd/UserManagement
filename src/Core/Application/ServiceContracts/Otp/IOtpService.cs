using Application.ViewModels.Otp;

namespace Application.ServiceContracts.Otp
{
    public interface IOtpService
    {
        string GenerateOtp(long userId);
        //public bool ValidateOtp(string otpCode, long userId);
    }
}