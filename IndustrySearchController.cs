using JobPortal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Controllers
{
    public class IndustrySearchController : Controller
    {
        JobSeekerContext dc = new JobSeekerContext();

        [HttpGet]
        public IActionResult Index()
        {
           
            return View();
        }
        [HttpGet]

        public IActionResult IndustrySearch()
        {
            PopulateIndustries(); // Populate industries for the dropdown
            PopulateJobTypes(); // Populate job types for the dropdown
            return View();
        }

        [HttpPost]
        public IActionResult IndustrySearch(string selectedIndustry, string selectedJobType)
        {
            PopulateIndustries(); // Repopulate industries for the dropdown
            PopulateJobTypes(); // Repopulate job types for the dropdown

            // Fetch job posts based on selected industry and job type
            var jobResults = dc.JobPosts
                .Include(j => j.Company) // Include company details
                .Where(j => j.Company.Type == selectedIndustry && j.JobType == selectedJobType)
                .ToList();

            ViewBag.JobResults = jobResults;

            return View(); // Return to the same view
        }

        private void PopulateIndustries()
        {
            var industries = dc.Companies
                .Select(c => c.Type)
                .Distinct()
                .ToList();

            ViewBag.Industries = industries;
        }

        private void PopulateJobTypes()
        {
            var jobTypes = dc.JobPosts
                .Select(j => j.JobType)
                .Distinct()
                .ToList();

            ViewBag.JobTypes = jobTypes;
        }

        [HttpGet]
        public IActionResult SkillSearch()
        {
            PopulateSkills(); // Populate skills for the dropdowns
            return View();
        }

        [HttpPost]
        public IActionResult SkillSearch(string primarySkill, string secondarySkill)
        {
            PopulateSkills(); // Repopulate skills for the dropdowns

            // Fetch jobs based on selected primary and secondary skills
            var jobResults = dc.JobSkills
           .Include(js => js.Job) // Include job details
           .Where(js => js.PSkill == primarySkill || js.SSkill == secondarySkill)
           .Select(js => js.Job) // Remove Distinct
           .ToList();

            ViewBag.JobResults = jobResults;

            return View(); // Return to the same view
        }

        private void PopulateSkills()
        {
            var primarySkills = dc.JobSkills
                .Select(js => js.PSkill)
                .Distinct()
                .ToList();

            var secondarySkills = dc.JobSkills
                .Select(js => js.SSkill)
                .Distinct()
                .ToList();

            ViewBag.PrimarySkills = primarySkills;
            ViewBag.SecondarySkills = secondarySkills;
        }



    }
}
