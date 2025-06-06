namespace Common.Helper
{
    public class PasswordHelper : IPasswordHelper
    {
        public string EncodePasswordBcrypt(string password)
        {
            // تولید هش با bcrypt با استفاده از تنظیمات پیشفرض
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            return hashedPassword;
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            // بررسی صحت رمز عبور با هش ذخیره شده در پایگاه داده
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
