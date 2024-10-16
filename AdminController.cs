
using Microsoft.AspNetCore.Mvc;
using JobPortalApp1.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace JobPortalApp1.Controllers
{
    public class AdminController : Controller
    {
        jobPortalNewContext context = new jobPortalNewContext();

        // Manage Admins
        [HttpGet]
        public IActionResult ManageAdmins()
        {
            var admins = context.Admins.ToList();
            return View(admins);
        }
        // GET: Edit Admin
        [HttpGet]
        public IActionResult EditAdmin(int id)
        {
            var admin = context.Admins.Find(id);
            if (admin == null)
            {
                return NotFound();
            }
            return View(admin);
        }

        // POST: Edit Admin
        [HttpPost]
        public IActionResult EditAdmin(Admin admin)
        {
            if (ModelState.IsValid)
            {
                var existingAdmin = context.Admins.Find(admin.AdminId);
                if (existingAdmin == null)
                {
                    return NotFound();
                }

                // Update fields
                existingAdmin.Username = admin.Username;
                existingAdmin.Email = admin.Email;
                existingAdmin.Role = admin.Role;

                context.SaveChanges();
                return RedirectToAction("ManageAdmins"); // Redirect to the admin list after editing
            }
            return View(admin); // Return the view with validation errors
        }




        // POST: Delete Admin
        [HttpPost]
        public IActionResult DeleteAdmin(int id)
        {
            var admin = context.Admins.Find(id);
            if (admin != null)
            {
                context.Admins.Remove(admin);
                context.SaveChanges();
            }
            return RedirectToAction("ManageAdmins");
        }

        [HttpGet]
        public IActionResult ManageCompanies(string searchTerm = null)
        {
            var companies = context.Companies.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                companies = companies.Where(c => c.CompanyName.Contains(searchTerm) || c.Email.Contains(searchTerm));
            }

            return View(companies.ToList());
        }

        // Edit Company
        [HttpGet]
        public IActionResult EditCompany(int id)
        {
            var company = context.Companies.Find(id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        [HttpPost]
        public IActionResult EditCompany(Company company)
        {
            if (ModelState.IsValid)
            {
                var existingCompany = context.Companies.Find(company.CompanyId);
                if (existingCompany == null)
                {
                    return NotFound();
                }

                // Update fields
                existingCompany.CompanyName = company.CompanyName;
                existingCompany.Email = company.Email;
                existingCompany.Type = company.Type;
                existingCompany.Website = company.Website;
                existingCompany.Location = company.Location;
                existingCompany.Phone = company.Phone;

                context.SaveChanges();
                return RedirectToAction("ManageCompanies"); // Redirect to the company list after editing
            }
            return View(company); // Return the view with validation errors
        }

        // Delete Company
        [HttpPost]
        public IActionResult DeleteCompany(int id)
        {
            var company = context.Companies.Find(id);
            if (company != null)
            {
                context.Companies.Remove(company);
                context.SaveChanges();
            }
            return RedirectToAction("ManageCompanies");
        }

        // Manage Job Applicants
        [HttpGet]
        public IActionResult ManageJobApplicants(string searchTerm = null)
        {
            var jobApplicants = context.JobApplicants.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                jobApplicants = jobApplicants.Where(j => j.FirstName.Contains(searchTerm) || j.LastName.Contains(searchTerm) || j.EmailId.Contains(searchTerm));
            }

            return View(jobApplicants.ToList());
        }

        // Edit Job Applicant
        [HttpGet]
        public IActionResult EditJobApplicant(int id)
        {
            var applicant = context.JobApplicants.Find(id);
            if (applicant == null)
            {
                return NotFound();
            }
            return View(applicant);
        }

        [HttpPost]
        public IActionResult EditJobApplicant(JobApplicant applicant)
        {
            if (ModelState.IsValid)
            {
                var existingApplicant = context.JobApplicants.Find(applicant.UserId);
                if (existingApplicant == null)
                {
                    return NotFound();
                }

                // Update fields
                existingApplicant.FirstName = applicant.FirstName;
                existingApplicant.LastName = applicant.LastName;
                existingApplicant.Phone = applicant.Phone;
                existingApplicant.Address = applicant.Address;
                existingApplicant.EmailId = applicant.EmailId;
                existingApplicant.Dob = applicant.Dob;
                existingApplicant.Resume = applicant.Resume;
                existingApplicant.Gender = applicant.Gender;

                context.SaveChanges();
                return RedirectToAction("ManageJobApplicants"); // Redirect to the job applicants list after editing
            }
            return View(applicant); // Return the view with validation errors
        }


        // Delete Job Applicant
        [HttpPost]
        public IActionResult DeleteJobApplicant(int id)
        {
            var applicant = context.JobApplicants.Find(id);
            if (applicant != null)
            {
                context.JobApplicants.Remove(applicant);
                context.SaveChanges();
            }
            return RedirectToAction("ManageJobApplicants");
        }

        
    }
}









