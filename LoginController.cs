using JobServicePortal.Models;
using Microsoft.AspNetCore.Mvc;
using DNTCaptcha.Core;
using DNTCaptcha.Core.Providers;
using System.Text;
using System.Security.Cryptography;
using Org.BouncyCastle.Pqc.Crypto.Lms;

namespace JobServicePortal.Controllers
{
    public class LoginController : Controller
    {
        public readonly IDNTCaptchaValidatorService _validatorService;
        public LoginController(IDNTCaptchaValidatorService validatorService)
        {
            _validatorService = validatorService;
        }

        jobPortalNewContext dv = new jobPortalNewContext();

        [HttpGet]
        public IActionResult App_Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateDNTCaptcha(ErrorMessage = "Please Enter Valid Captcha",
            CaptchaGeneratorLanguage = Language.English,
            CaptchaGeneratorDisplayMode = DisplayMode.ShowDigits)]


        public IActionResult App_Login(string t1, string t2)
        {
            byte[] p = Encoding.UTF8.GetBytes(t2);


            if (ModelState.IsValid)
            {
                if (!_validatorService.HasRequestValidCaptchaEntry(Language.English, DisplayMode.ShowDigits))
                {
                    this.ModelState.AddModelError(DNTCaptchaTagHelper.CaptchaInputName, "Please Enter Valid Captcha.");

                }

                var res = (from t in dv.ApplicantRegistrations
                           where t.Username == t1 && t.Password == p
                select t).Count();

                if (res >0)
                {

                    HttpContext.Session.SetString("uid", t1);

                    //code to navigate
                    return RedirectToAction("About", "Home");

                }
                else
                {
                    ViewData["MessageAppLogin"] = "Invalid User..!!!!";
                }

            }
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Logout(string a)
        {
            HttpContext.Session.Clear(); // Clear all session data
            return RedirectToAction("App_Login", "Login"); // Redirect to Login page


        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]

        public IActionResult ResetPassword(string t, string newPassword, string conPassword, string oldPassword, ApplicantRegistration u)
        {


            //byte[] p = Encoding.UTF8.GetBytes(oldPassword);

            var res = (from r in dv.ApplicantRegistrations
                       where r.Username == t 
                       select r).FirstOrDefault();

            if (res != null)
            {
                if (newPassword == conPassword)
                {
                    res.Password = Encoding.UTF8.GetBytes(newPassword);

                    int i = dv.SaveChanges();

                    if (i > 0)
                    {
                        ViewData["x"] = "Reset successfully";
                    }
                    else
                    {
                        ViewData["x"] = "Failed to reset password";
                    }
                }
                else
                {
                    ViewData["x"] = "New Password and Confirm Password Mismatch";
                }

            }
            else
            {
                ViewData["x"] = "User not found";
            }

            return View();
        }
       
    }
       
       
}


