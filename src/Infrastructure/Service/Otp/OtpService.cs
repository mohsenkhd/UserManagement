using Application.RepositoryContracts.UserAggregate;
using Application.ServiceContracts.Otp;
using Application.ViewModels.Otp;
using Common.Exceptions.UserManagement;
using Domain.Entities;
using System.Security.Cryptography;

namespace Service.Otp
{
    public class OtpService : IOtpService
    {
        //private byte[] SecretBytes { get; } = Convert.FromBase64String("MTIzNDU2");
        private readonly IUserRepository _userRepository;
        public OtpService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        //private int GenerateOtpForTimestamp(long timestamp, long userId)
        //{
        //    byte[] timestampBytes = BitConverter.GetBytes(timestamp);

        //    if (BitConverter.IsLittleEndian)
        //    {
        //        Array.Reverse(timestampBytes);
        //    }
        //    byte[] userIdBytes = BitConverter.GetBytes(userId);
        //    if (BitConverter.IsLittleEndian)
        //    {
        //        Array.Reverse(userIdBytes);
        //    }
        //    byte[] combinedBytes = SecretBytes.Concat(userIdBytes).ToArray();
        //    using (var hmac = new HMACSHA256(combinedBytes))
        //    {
        //        byte[] hashBytes = hmac.ComputeHash(timestampBytes);
        //        int offset = hashBytes[hashBytes.Length - 1] & 0x0F;
        //        int otp = BitConverter.ToInt16(hashBytes, offset) & 0x7FFF;
        //        return otp;
        //    }
        //}

        //public string GenerateOtp(long userId)
        //{
        //    long currentTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
        //    int otp = GenerateOtpForTimestamp(currentTimestamp / 120, userId);
        //    int otpLength = 5;

        //    string otpCode = (otp % (int)Math.Pow(10, otpLength)).ToString($"D{otpLength}");

        //    return otpCode;
        //}

        //public bool ValidateOtp(string otpCode, long userId)
        //{
        //    long currentTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();

        //    for (int i = 0; i < 2; i++) // Check the current and previous intervals (for redundancy)
        //    {
        //        int otpToCheck = GenerateOtpForTimestamp((currentTimestamp - i * 120) / 120, userId);

        //        for (int j = -2; j <= 2; j++)
        //        {
        //            short otpToCheckWithOffset = (short)((otpToCheck + j) % 100000);
        //            string otpCodeToCheck = otpToCheckWithOffset.ToString("D5");

        //            if (otpCodeToCheck == otpCode)
        //            {
        //                return true;
        //            }
        //        }
        //    }

        //    return false;
        //}
        public  string GenerateOtp(long userId)
        {         
            Random rnd = new Random();
            string otpCode = rnd.Next(10000, 99999).ToString();
            return otpCode;
        }

    }
}