using Application.ServiceContracts.Captcha;
using Application.ViewModels.Captcha;
using Common.Exceptions.UserManagement;
using Common.Wrappers;
using Microsoft.Extensions.Configuration;

namespace Service.Captcha
{
    public class CaptchaService : ICaptchaService
    {
        private readonly CaptchaWrapper _wrapper;
        private readonly IConfiguration _configuration;

        public CaptchaService(CaptchaWrapper wrapper, IConfiguration configuration)
        {
            _wrapper = wrapper;
            _configuration = configuration;
        }

        public async Task<CheckCaptchaBaseViewModel> Check(CheckCaptchaAddCommandModel model)
        {
            try
            {
                var wrapper = _wrapper
            .WithUrl(_configuration["Url:Captcha"] + "/check/" + model.Id + "/" + model.Code)
            .WithHttpMethod(HttpMethod.Get).WithUserId(0);

                var res = await wrapper.Fetch<CheckCaptchaBaseViewModel>();
                return res;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Captcha Exception: {exception}");
                throw UserManagementExceptions.CaptchaserverException;
            }
        
        }
    }
}