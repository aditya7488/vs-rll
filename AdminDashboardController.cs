using JobPortalApp1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace JobPortal.Controllers
{
    public class AdminDashboardController : Controller
    {
        jobPortalNewContext dc = new jobPortalNewContext();
        private readonly jobPortalNewContext _context;

        public AdminDashboardController( jobPortalNewContext context)
        {
            _context = context;
        }

        public AdminDashboardController()
        {
        }

        // GET: Admin Dashboard
        [HttpGet]
        public IActionResult Index()
        {
            var jobSeekers = _context.JobApplicants.ToList();
            var employers = _context.Companies.ToList();
            ViewBag.JobSeekers = jobSeekers;
            ViewBag.Employers = employers;
            return View();
        }

        // GET: View Job Seeker
        [HttpGet]
        public IActionResult ViewJobSeeker(int id)
        {
            var jobSeeker = _context.JobApplicants.Find(id);
            if (jobSeeker == null)
            {
                return NotFound();
            }
            return View(jobSeeker);
        }

        // GET: Edit Job Seeker
        [HttpGet]
        public IActionResult EditJobSeeker(int id)
        {
            var jobSeeker = _context.JobApplicants.Find(id);
            if (jobSeeker == null)
            {
                return NotFound();
            }
            return View(jobSeeker);
        }

        [HttpPost]
        public IActionResult EditJobSeeker(JobApplicant jobSeeker)
        {
            if (ModelState.IsValid)
            {
                _context.JobApplicants.Update(jobSeeker);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(jobSeeker);
        }

        // GET: Deactivate Job Seeker
        [HttpGet]
        public IActionResult DeactivateJobSeeker(int id)
        {
            var jobSeeker = _context.JobApplicants.Find(id);
            if (jobSeeker == null)
            {
                return NotFound();
            }
            

            return RedirectToAction("Index");
        }

        // GET: Delete Job Seeker
        [HttpGet]
        public IActionResult DeleteJobSeeker(int id)
        {
            var jobSeeker = _context.JobApplicants.Find(id);
            if (jobSeeker == null)
            {
                return NotFound();
            }
            return View(jobSeeker);
        }

        [HttpPost, ActionName("DeleteJobSeeker")]
        public IActionResult DeleteConfirmed(int id)
        {
            var jobSeeker = _context.JobApplicants.Find(id);
            if (jobSeeker != null)
            {
                _context.JobApplicants.Remove(jobSeeker);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult ViewEmployer(int id)
        {
            var employer = _context.Companies.Find(id);
            if (employer == null)
            {
                return NotFound();
            }
            return View(employer);
        }

        // GET: Edit Employer
        [HttpGet]
        public IActionResult EditEmployer(int id)
        {
            var employer = _context.Companies.Find(id);
            if (employer == null)
            {
                return NotFound();
            }
            return View(employer);
        }

        [HttpPost]
        public IActionResult EditEmployer(Company employer)
        {
            if (ModelState.IsValid)
            {
                _context.Companies.Update(employer);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employer);
        }

        // GET: Deactivate Employer
        [HttpGet]
        public IActionResult DeactivateEmployer(int id)
        {
            var employer = _context.Companies.Find(id);
            if (employer == null)
            {
                return NotFound();
            }
            // Implement your deactivation logic here if needed
            return RedirectToAction("Index");
        }

        // GET: Delete Employer
        [HttpGet]
        public IActionResult DeleteEmployer(int id)
        {
            var employer = _context.Companies.Find(id);
            if (employer == null)
            {
                return NotFound();
            }
            return View(employer);
        }

        [HttpPost, ActionName("DeleteEmployer")]
        public IActionResult DeleteConfirmedEmployer(int id)
        {
            var employer = _context.Companies.Find(id);
            if (employer != null)
            {
                _context.Companies.Remove(employer);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public IActionResult Admindashboard()
        {
            var admins = _context.Admins.ToList();
            var companies = _context.Companies.ToList();
            var applicants = _context.JobApplicants.ToList();

            var model = new Tuple<IEnumerable<Admin>, IEnumerable<Company>, IEnumerable<JobApplicant>>(admins, companies, applicants);
            return View(model);
        }
    }
}
