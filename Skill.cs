﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace JobServicePortal.Models;

public partial class Skill
{
    public int SkillId { get; set; }

    public int CandidateId { get; set; }

    public string PrimarySkill { get; set; }

    public string SecondarySkill { get; set; }

    public virtual JobApplicant Candidate { get; set; }
}