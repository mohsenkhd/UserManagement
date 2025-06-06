using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions.UserManagement
{
    public static class UserManagementExceptions
    {
        public static IdentityException UserNotFoundException => new(-1400, "کاربر مورد نظر یافت نشد", 404);
        public static IdentityException RoleNotFoundException => new(-1401, "Role موردنظر پیدا نشد", 404);

        public static IdentityException RoleNameExistException => new(-1402, "Role تکراری می باشد  ", 400);

        public static IdentityException CptchaIsWrongException => new(-1403, "کد captcha اشتباه می باشد", 400);

        public static IdentityException UseralreadyregisteredException => new(-1404, "ثبت نام از قبل کامل شده است", 400);
        public static IdentityException MobileNumberNotValidException => new(-1405, "شماره موبایل معتبر نمی باشد", 400);
        public static IdentityException PasswordNotValidException => new(-1406, "رمز عبور معتبر نمی باشد", 400);
        public static IdentityException NationalCodeNotValidException => new(-1407, "کد ملی معتبر نمی باشد", 400);
        public static IdentityException RegisterTypeNotFoundException => new(-1408, "تایپ ثبت نام موجود نمی باشد", 404);
        public static IdentityException InvalidLoginException => new(-1409, "نام کاربری یا رمز عبور اشتباه می باشد", 400);
        public static IdentityException OTPNotValidException => new(-1410, "کد وارد شده صحیح نمی باشد", 400);
        public static IdentityException CaptchaserverException => new(-1411, "captcha server is down", 500);
        public static IdentityException TokenNotValidException => new(-1412, "Token is not valid", 400);
        public static IdentityException permissionDeniedException => new(-1413, "اجازه دسترسی ندارید", 403);
        public static IdentityException JwtNotValidException => new(-1414, "توکن نامعتبر است", 401);
        public static IdentityException RegisterBadRequestException => new(-1415, "اطلاعات وارد شده کامل نیست", 400);
        public static IdentityException RolesNotFoundException => new(-1416, "Roles موردنظر پیدا نشد", 404);
        public static IdentityException CantAssignRoleToUserException => new(-1417, "somthing went wrong", 400);
        public static IdentityException RoleUpdateException => new(-1418, "نقش NormalUser قابل ویرایش نمی باشد ", 400);
        public static IdentityException CantAssignPermissionsToRoleException => new(-1419, "somthing went wrong", 400);
        public static IdentityException UserNotActiveException => new(-1420, "کاربر فعال نمی باشد ", 400);
        public static IdentityException RefreshTokenNotValidException => new(-1421, "RefreshToken is not valid", 400);
        public static IdentityException SendMessageFaildException => new(-1422, "Send Message  Faild", 500);
        public static IdentityException ShahkarServiceFaildException => new(-1423, "Shahkar Service  Faild", 500);
        public static IdentityException CustomerServiceFaildException => new(-1424, "Customer Service  Faild", 500);
        public static IdentityException RegisterNotCompletedException => new(-1428, "ثبت نام شما کامل نشده است", 400);
        public static IdentityException ForgotTokenNotValidException => new(-1429, "ForgotToken is not valid", 400);
        public static IdentityException ClientNotFoundException => new(-1430, "Client NotFound ", 404);
        public static IdentityException ClientKeyException => new(-1431, "Api-Key Or ApiSecret NotFound ", 401);
        public static IdentityException AddAdministratorRoleToUserException => new(-1432, "شما اجازه اختصاص این نقش را ندارید", 404);
        public static IdentityException MobileNumberOrNationalCodeException => new(-1433, "شماره موبایل یا کد ملی تکراری می باشد", 400);
        public static IdentityException OtpCodeException => new(-1434, "کد قبلا برای شما ارسال گردیده است لطفا بعد از 2 دقیقه امتحان کنید", 400);
        public static IdentityException OrderNotFoundException => new(-1434, "سفارش مورد نظر یافت نشد", 404);
        public static IdentityException InvoiceNotFoundException => new(-1435, "فاکتور مورد نظر یافت نشد", 404);
        public static IdentityException WalletNotFoundException => new(-1435, "کیف پول مورد نظر یافت نشد", 404);
    }
}
