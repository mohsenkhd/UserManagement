namespace Common.Helper
{
    public interface IPasswordHelper
    {
        string EncodePasswordBcrypt(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}