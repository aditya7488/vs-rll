using JobServicePortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using DNTCaptcha.Core.Providers;
using DNTCaptcha.Core;

namespace JobServicePortal.Controllers
{
    public class RegistrationController : Controller
    {
        public readonly IDNTCaptchaValidatorService _validatorService;
        public RegistrationController(IDNTCaptchaValidatorService validatorService)
        {
            _validatorService = validatorService;
        }

        jobPortalNewContext dv = new jobPortalNewContext();

        private byte[] HashPassword(string password)
        {
            var sha256 = SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        [HttpGet]
        public IActionResult App_Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateDNTCaptcha(ErrorMessage = "Please Enter Valid Captcha",
           CaptchaGeneratorLanguage = Language.English,
           CaptchaGeneratorDisplayMode = DisplayMode.ShowDigits)]
        public IActionResult App_Registration(ApplicantRegistration r)
        {
            if (ModelState.IsValid)
            {
                if (!_validatorService.HasRequestValidCaptchaEntry(Language.English, DisplayMode.ShowDigits))
                {
                    this.ModelState.AddModelError(DNTCaptchaTagHelper.CaptchaInputName, "Please Enter Valid Captcha.");

                }


                r.Password = HashPassword(r.NormPassword);
                dv.ApplicantRegistrations.Add(r);
                int i = dv.SaveChanges();
                if (i > 0)
                {
                    ViewData["MessageAppRegistration"] = "User Registered Successfully";
                    return RedirectToAction("App_Login", "Login"); // Redirect to the Login action in HomeController
                }
                else
                {
                    ViewData["MessageAppRegistration"] = "Registration failed. Please try again.";
                }
               
            }
            return View();
        }
   
            
    }
}


