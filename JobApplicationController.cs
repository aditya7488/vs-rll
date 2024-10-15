using JobServicePortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System.IO;
using System.Threading.Tasks;

namespace JobServicePortal.Controllers
{
    public class JobApplicationController : Controller
    {
        private readonly jobPortalNewContext dc;
        private readonly EmailService _emailService;

        public JobApplicationController(jobPortalNewContext context)
        {
            dc = context;
            _emailService = new EmailService();
        }

        // GET: JobApplication/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JobApplication/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobApplicant applicant, IFormFile ResumePath)
        {
            // Check if the model state is valid
            if (ModelState.IsValid)
            {
                // Handle resume upload
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
                            applicant.Resumee = memoryStream.ToArray(); // Save the file content as a byte array
                        }
                    }
                    else
                    {
                        ViewData["Message"] = "Invalid file format. Only PDF files are allowed.";
                        return View(applicant); // Return the view with the model for corrections
                    }
                }

                // Add the applicant to the database context
                dc.JobApplicants.Add(applicant);

                try
                {
                    // Save the applicant to the database
                    await dc.SaveChangesAsync();
                    ViewData["Message"] = "Job Applicant created successfully!";
                    return RedirectToAction(nameof(Index)); // Redirect after success
                }
                catch (DbUpdateException ex)
                {
                    ViewData["Message"] = "Error saving applicant: " + ex.Message; // Handle errors
                }
            }
            else
            {
                ViewData["Message"] = "Please correct the errors in the form."; // Validation errors
            }

            return View(applicant); // Return the view with the model for corrections
        }


        // GET: JobApplication/Index
        public async Task<IActionResult> Index()
        {
            var applicants = await dc.JobApplicants.ToListAsync();
            return View(applicants); // Return the list of applicants to the view
        }

        // GET: JobApplication/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var applicant = await dc.JobApplicants.FindAsync(id);
            if (applicant == null)
            {
                return NotFound();
            }
            return View(applicant); // Return the applicant to confirm deletion
        }

        // POST: JobApplication/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var applicant = await dc.JobApplicants.FindAsync(id);
            if (applicant != null)
            {
                dc.JobApplicants.Remove(applicant);
                await dc.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index)); // Redirect to Index after deletion
        }

        // GET: JobApplication/Details/5
        

        public IActionResult Details(int id)
        {
            var applicant = dc.JobApplicants.Find(id);
            if (applicant == null) return NotFound();
            return View(applicant);
        }

    }
}



