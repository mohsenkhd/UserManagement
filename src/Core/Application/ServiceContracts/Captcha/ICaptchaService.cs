using Application.ViewModels.Captcha;

namespace Application.ServiceContracts.Captcha
{
    public interface ICaptchaService
    {
        public Task<CheckCaptchaBaseViewModel> Check(CheckCaptchaAddCommandModel model);
    }
}