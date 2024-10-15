using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobServicePortal.Models;

public partial class JobApplicant
{
    public int UserId { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3)]
    public string FirstName { get; set; }

    public string LastName { get; set; }

    [Required]
    [Phone]
    public string Phone { get; set; }

    [Required]
    public string Address { get; set; }


    [Required]
    [EmailAddress]
    public string EmailId { get; set; }

    public DateOnly? Dob { get; set; }

    [NotMapped]
    [Required(ErrorMessage = "Please upload a resume.")]
    public IFormFile ResumePath { get; set; }

    public byte[] Resumee { get; set; }
    

    public string Gender { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual ICollection<EducationalQualification> EducationalQualifications { get; set; } = new List<EducationalQualification>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ProfessionalDetail ProfessionalDetail { get; set; }

    public virtual ICollection<SavedJob> SavedJobs { get; set; } = new List<SavedJob>();

    public virtual ICollection<Skill> Skills { get; set; } = new List<Skill>();
}


