using JobPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using System.Collections.Generic;

namespace JobPortal.Controllers
{
    public class JobPostController : Controller
    {
      
        JobSeekerContext dc = new JobSeekerContext();

        /*  [HttpGet]
          public IActionResult Index(int companyId)
          {
              //var jobPosts = dc.JobPosts.ToList(); // Get all job posts
             // return View(jobPosts); // Return the view with the list of job posts

          }
  */
        private JobSeekerContext CreateContext()
        {
            return new JobSeekerContext(); // Ensure this is correctly instantiated
        }
      

        [HttpGet]
        public IActionResult Index()
        {
            // Assume you have a way to get the logged-in user's company ID
            int loggedInCompanyId = GetLoggedInCompanyId(); // Replace with your actual method to get the logged-in company ID

            var jobPosts = dc.JobPosts
                .Where(j => j.CompanyId == loggedInCompanyId) // Filter by the logged-in company's ID
                .ToList();

           


            return View(jobPosts);
        }

        // Method to get the logged-in company's ID (you'll need to implement this)
        private int GetLoggedInCompanyId()
        {
            // For example, you might retrieve it from the user's session or claims
            // return HttpContext.Session.GetInt32("CompanyId") ?? 0;
            return 2; // Replace with actual implementation
        }


        [HttpGet]
        public IActionResult SearchCandidates()
        {
            PopulateSkills();
            return View(new List<JobApplicant>()); // Return an empty list initially
        }

        [HttpPost]
        public IActionResult SearchCandidates(string primarySkills)
        {
            PopulateSkills(); // Repopulate skills for the dropdowns

            // Fetch candidates based on the selected primary skills
            var candidates = dc.Skills
                .Where(s => s.PrimarySkill == primarySkills)
                .GroupBy(s => s.CandidateId) // Group by candidate ID
                .Select(g => g.FirstOrDefault().Candidate) // Select the first candidate from each group
                .ToList();

            // Handle the case where candidates might be null
            if (candidates == null || !candidates.Any())
            {
                ViewBag.Message = "No candidates found with the specified primary skill.";
                return View(new List<JobApplicant>()); // Return an empty list
            }

            return View("SearchCandidates", candidates);
        }

        private void PopulateSkills()
        {
            var primarySkills = dc.Skills
                .Select(s => s.PrimarySkill)
                .Distinct()
                .ToList();

            ViewBag.PrimarySkills = primarySkills;
        }




        [HttpGet]
        public IActionResult PostJob()
        {
            return View();
        }
        [HttpPost]
        public IActionResult PostJob(JobPost r)
        {
            if (ModelState.IsValid)
            {
                dc.JobPosts.Add(r);

                // Attempt to save changes to the database
                int i = dc.SaveChanges();
                if (i > 0)
                {
                    // Redirect to the Index action to show all job posts
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["Message"] = "Failed to add job.";
                }
            }

            // Return the model to retain form data in case of validation errors
            return View(r);
        }

       




        [HttpGet]
        public IActionResult EditJob(int id)
        {
            var jobPost = dc.JobPosts.Find(id);
            if (jobPost == null)
            {
                return NotFound();
            }

            ViewBag.Companies = dc.Companies.ToList(); // Populate companies for dropdown
            return View(jobPost);
        }

        [HttpPost]
        public IActionResult EditJob(JobPost jobPost)
        {
            if (ModelState.IsValid)
            {
                // Check if the company exists
                if (!dc.Companies.Any(c => c.CompanyId == jobPost.CompanyId))
                {
                    ModelState.AddModelError("CompanyId", "The selected company does not exist.");
                    ViewBag.Companies = dc.Companies.ToList(); // Repopulate companies
                    return View(jobPost);
                }

                dc.JobPosts.Update(jobPost);
                dc.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Companies = dc.Companies.ToList(); // Repopulate companies on error
            return View(jobPost);
        }



        [HttpGet]
        public IActionResult ViewJobDescription(int id)
        {
            var jobPost = dc.JobPosts.Find(id);
            if (jobPost == null)
            {
                return NotFound();
            }
            return View(jobPost);
        }



        [HttpGet]
        public IActionResult DeleteJob(int id)
        {
            var jobPost = dc.JobPosts.Find(id);
            if (jobPost == null)
            {
                return NotFound();
            }
            return View(jobPost);
        }

        [HttpPost, ActionName("DeleteJob")]
        public IActionResult DeleteConfirmed(int id)
        {
            var jobPost = dc.JobPosts.Find(id);
            if (jobPost != null)
            {
                dc.JobPosts.Remove(jobPost);
                dc.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ViewJobApplications(int jobId)
        {
            var applications = await dc.JobApplications
                .Include(a => a.Applicant) // Ensure applicant details are included
                .Where(a => a.JobId == jobId)
                .Select(a => new JobApplication 
                {
                   //ApplicantName = $"{a.Applicant.FirstName} {a.Applicant.LastName}",
                   // Email = a.Applicant.EmailId,
                    AppliedDate = a.AppliedDate,
                    Status = a.Status,
                    Resume = a.Resume,
                    CoverLetter = a.CoverLetter
                })
                .ToListAsync();

            if (!applications.Any())
            {
                return NotFound("No applications found for this job posting.");
            }

            return View(applications);
        }





    }
}

