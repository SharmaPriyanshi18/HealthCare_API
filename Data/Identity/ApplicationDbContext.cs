using HealthCare_Data.Identity;
using HealthCareData;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCareData.Identity
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {

        }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Therapist> Therapists { get; set; }
        public DbSet<Case> Cases { get; set; }
        public DbSet<treatment> treatments { get; set; }
        public DbSet<Disease> Diseases { get; set; }
        public DbSet<ImageProfile> ImageProfiles { get; set; }
        public DbSet<schedulerDate> schedulers { get; set; }
        public DbSet<SchedulerTherapist> schedulerTherapists { get; set; }
        public DbSet<Assessment> Assessments { get; set; }

    }
}
