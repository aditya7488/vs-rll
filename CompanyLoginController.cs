using CarrerSolutions.Models;
using Microsoft.AspNetCore.Mvc;
using DNTCaptcha.Core;
using DNTCaptcha.Core.Providers;
using System.Text;
using System.Security.Cryptography;
using CarrerSolutions.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace CarrerSolutions.Controllers
{
    public class CompanyLoginController : Controller
    {


      
        jobPortalNewContext dv = new jobPortalNewContext();

        public readonly IDNTCaptchaValidatorService _validatorService;
        public CompanyLoginController(IDNTCaptchaValidatorService validatorService)
        {
            _validatorService = validatorService;
        }


        [HttpGet]
        public IActionResult CLogin()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateDNTCaptcha(ErrorMessage = "Please Enter Valid Captcha",
            CaptchaGeneratorLanguage = Language.English,
            CaptchaGeneratorDisplayMode = DisplayMode.ShowDigits)]

        public IActionResult CLogin(CompanyRegistration t, byte[] Password,string t1)
        {
            //t.Password = Encoding.UTF8.GetBytes(Password);

            if (ModelState.IsValid)
            {
                if (!_validatorService.HasRequestValidCaptchaEntry(Language.English, DisplayMode.ShowDigits))
                {
                    this.ModelState.AddModelError(DNTCaptchaTagHelper.CaptchaInputName, "Please Enter Valid Captcha.");
                }

                

                var res = dv.CompanyRegistrations.FirstOrDefault(t => t.Email == t1 && t.Password == Password);

                if (res != null)
                {
                    HttpContext.Session.SetString("uid", t1);

                    var claims = new List<Claim>
                   {
                     new Claim(ClaimTypes.Name, t1)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };

                   

                    return RedirectToAction("Home");
                }
                else
                {
                    ViewData["a"] = "Invalid User..!!!!";
                }
            }

            return View(t);
        }
    }

}
