using CarrerSolutions.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using DNTCaptcha.Core.Providers;
using DNTCaptcha.Core;
using Microsoft.EntityFrameworkCore;

namespace CarrerSolutions.Controllers
{
    public class CompanyRegistrationController : Controller
    {
        private readonly jobPortalNewContext _context;
        private readonly IDNTCaptchaValidatorService _validatorService;

        public CompanyRegistrationController(jobPortalNewContext context, IDNTCaptchaValidatorService validatorService)
        {
            _context = context;
            _validatorService = validatorService;
        }

        private byte[] HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        [HttpGet]
        public IActionResult CRegistration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateDNTCaptcha(ErrorMessage = "Please enter a valid Captcha.",
   CaptchaGeneratorLanguage = Language.English,
   CaptchaGeneratorDisplayMode = DisplayMode.ShowDigits)]
        public IActionResult CRegistration(CompanyRegistration registration)
        {
            // Validate Captcha
            if (!_validatorService.HasRequestValidCaptchaEntry(Language.English, DisplayMode.ShowDigits))
            {
                ModelState.AddModelError(DNTCaptchaTagHelper.CaptchaInputName, "Please enter a valid Captcha.");
            }

            // Check for existing company by email or name
            if (_context.CompanyRegistrations.Any(c => c.Email == registration.Email || c.CompanyName == registration.CompanyName))
            {
                ModelState.AddModelError(string.Empty, "A company with this email or name already exists.");
            }

            if (ModelState.IsValid)
            {
                // Hash the password before saving
                registration.Password = HashPassword(registration.NormPassword);

                try
                {
                    // Add the new registration to the context
                    _context.CompanyRegistrations.Add(registration);

                    // Save changes to the database
                    if (_context.SaveChanges() > 0)
                    {
                        ViewData["MessageAppRegistration"] = "User registered successfully.";
                        return RedirectToAction("CLogin", "CompanyLogin"); // Redirect to the login action
                    }
                    else
                    {
                        ViewData["MessageAppRegistration"] = "Registration failed. Please try again.";
                    }
                }
                catch (DbUpdateException ex)
                {
                    // Log the exception (consider using a logging framework)
                    ViewData["MessageAppRegistration"] = "An error occurred while registering the user: " + ex.Message;
                }
            }

            // Return view with current model to show validation messages
            return View(registration);
        }

    }
}


