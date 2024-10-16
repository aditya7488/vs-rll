using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using CarrerSolutions.Models;
using Microsoft.EntityFrameworkCore;

namespace CarrerSolutions.Controllers
{
    public class ProfileController : Controller
    {
        private readonly jobPortalNewContext _context;
        private readonly ILogger<ProfileController> _logger; // Declare the logger

        public ProfileController(jobPortalNewContext context, ILogger<ProfileController> logger)
        {
            _context = context;
            _logger = logger; // Initialize the logger
        }

        public IActionResult Create()
        {
            var model = new CompositeProfile
            {
                JobApplicant = new JobApplicant(),
                ProfessionalDetail = new ProfessionalDetail(),
                EducationalQualification = new EducationalQualification(),
                Skill = new Skill()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
     JobApplicant jobApplicant,
     ProfessionalDetail professionalDetail,
     EducationalQualification educationalQualification,
     Skill skill,
     IFormFile ResumePath)
        {

            educationalQualification.UserId = jobApplicant.UserId;
            professionalDetail.UserId = jobApplicant.UserId;
            skill.UserId = jobApplicant.UserId;
            professionalDetail.UserId = jobApplicant.UserId;

            // if (ModelState.IsValid)
            { 
                try
                {
                    // Save the uploaded resume if necessary
                    if (ResumePath != null && ResumePath.Length > 0)
                    {
                        var fileExt = Path.GetExtension(ResumePath.FileName).ToUpper();

                        // Check if the file format is PDF
                        if (fileExt == ".PDF")
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                // Copy the uploaded file to the memory stream
                                await ResumePath.CopyToAsync(memoryStream);
                                jobApplicant.Resumee = memoryStream.ToArray(); // Save the file content as a byte array
                            }
                        }
                        //else
                        //{
                        //    ViewData["Message"] = "Invalid file format. Only PDF files are allowed.";
                        //    return View(); // Return the view with the model for corrections
                        //}
                    }

                    // Add to context
                    _context.JobApplicants.Add(jobApplicant);
                    _context.ProfessionalDetails.Add(professionalDetail);
                    _context.EducationalQualifications.Add(educationalQualification);
                    _context.Skills.Add(skill);

                    // Save changes to the database
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Success");
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Database update error occurred.");
                    ModelState.AddModelError(string.Empty, "An error occurred while saving the data. Please try again.");
                }
                catch (IOException ex)
                {
                    _logger.LogError(ex, "File upload error occurred.");
                    ModelState.AddModelError(string.Empty, "An error occurred while uploading the file. Please try again.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An unexpected error occurred.");
                    ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again.");
                }
            }

            // Return view with the model to show validation messages
            return View(new CompositeProfile
            {
                JobApplicant = jobApplicant,
                ProfessionalDetail = professionalDetail,
                EducationalQualification = educationalQualification,
                Skill = skill
            });
        }
    }
}


