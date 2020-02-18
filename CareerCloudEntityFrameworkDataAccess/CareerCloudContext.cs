using CareerCloud.Pocos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CareerCloud.EntityFrameworkDataAccess
{
    public class CareerCloudContext : DbContext
    {
        public DbSet<ApplicantEducationPoco> ApplicationEducations { get; set; }
        public DbSet<ApplicantJobApplicationPoco> ApplicantJobApplications { get; set; }
        public DbSet<ApplicantProfilePoco> ApplicantProfiles { get; set; }
        public DbSet<ApplicantResumePoco> ApplicantResumes { get; set; }
        public DbSet<ApplicantSkillPoco> ApplicantSkills { get; set; }
        public DbSet<ApplicantWorkHistoryPoco> ApplicantWorkHistorys { get; set; }
        public DbSet<CompanyDescriptionPoco> CompanyDescriptions { get; set; }
        public DbSet<CompanyJobDescriptionPoco> CompanyJobDescriptions { get; set; }
        public DbSet<CompanyJobEducationPoco> CompanyJobEducations { get; set; }
        public DbSet<CompanyJobPoco> CompanyJobs { get; set; }
        public DbSet<CompanyJobSkillPoco> CompanyJobSkills { get; set; }
        public DbSet<CompanyLocationPoco> CompanyLocations { get; set; }
        public DbSet<CompanyProfilePoco> CompanyProfiles { get; set; }
        public DbSet<SecurityLoginPoco> SecurityLogins { get; set; }
        public DbSet<SecurityLoginsLogPoco> SecurityLoginsLogs { get; set; }
        public DbSet<SecurityLoginsRolePoco> SecurityLoginsRoles { get; set; }
        public DbSet<SecurityRolePoco> SecurityRoles { get; set; }
        public DbSet<SystemCountryCodePoco> SystemCountryCodes { get; set; }
        public DbSet<SystemLanguageCodePoco> SystemLanguageCodes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            string _connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
            optionsBuilder.UseSqlServer(_connStr);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicantEducationPoco>
                (entry => 
                {
                    entry.HasOne(e => e.ApplicantProfiles)
                    .WithMany(p => p.ApplicantEducations)
                    .HasForeignKey(e => e.Applicant);
                });

            modelBuilder.Entity<ApplicantProfilePoco>
                (entry =>
                {
                    entry.HasOne(p => p.SystemCountryCodes)
                    .WithMany(c => c.ApplicantProfiles)
                    .HasForeignKey(p => p.Country);
                });

            modelBuilder.Entity<ApplicantProfilePoco>
                (entry =>
                {
                    entry.HasOne(p => p.SecurityLogins)
                    .WithMany(s => s.ApplicantProfiles)
                    .HasForeignKey(p => p.Login);
                });

            modelBuilder.Entity<ApplicantResumePoco>
                (entry =>
                {
                    entry.HasOne(r => r.ApplicantProfiles)
                    .WithMany(p => p.ApplicantResumes)
                    .HasForeignKey(r => r.Applicant);
                });

            modelBuilder.Entity<ApplicantSkillPoco>
                (entry =>
                {
                    entry.HasOne(s => s.ApplicantProfiles)
                    .WithMany(p => p.ApplicantSkills)
                    .HasForeignKey(s => s.Applicant);
                });

            modelBuilder.Entity<ApplicantWorkHistoryPoco>
                (entry =>
                {
                    entry.HasOne(w => w.ApplicantProfiles)
                    .WithMany(s => s.ApplicantWorkHistorys)
                    .HasForeignKey(w => w.Applicant);
                });

            modelBuilder.Entity<ApplicantWorkHistoryPoco>
                (entry =>
                {
                    entry.HasOne(c => c.SystemCountryCodes)
                    .WithMany(s => s.ApplicantWorkHistorys)
                    .HasForeignKey(c => c.CountryCode);
                });

            modelBuilder.Entity<ApplicantJobApplicationPoco>
                (entry =>
                {
                    entry.HasOne(j => j.ApplicantProfiles)
                    .WithMany(a => a.ApplicantJobApplications)
                    .HasForeignKey(j => j.Applicant);
                });

            modelBuilder.Entity<ApplicantJobApplicationPoco>
                (entry =>
                {
                    entry.HasOne(j => j.CompanyJobs)
                    .WithMany(c => c.ApplicantJobApplications)
                    .HasForeignKey(j => j.Job);
                });

            modelBuilder.Entity<CompanyDescriptionPoco>
                (entry =>
                {
                    entry.HasOne(d => d.CompanyProfiles)
                    .WithMany(p => p.CompanyDescriptions)
                    .HasForeignKey(d => d.Company);
                });

            modelBuilder.Entity<CompanyDescriptionPoco>
                (entry =>
                {
                    entry.HasOne(d => d.SystemLanguageCodes)
                    .WithMany(l => l.CompanyDescriptions)
                    .HasForeignKey(d => d.LanguageId);
                });

            modelBuilder.Entity<CompanyJobDescriptionPoco>
                (entry =>
                {
                    entry.HasOne(j => j.CompanyJobs)
                    .WithMany(c => c.CompanyJobDescriptions)
                    .HasForeignKey(j => j.Job);
                });

            modelBuilder.Entity<CompanyJobEducationPoco>
                (entry =>
                {
                    entry.HasOne(j => j.CompanyJobs)
                    .WithMany(c => c.CompanyJobEducations)
                    .HasForeignKey(j => j.Job);
                });

            modelBuilder.Entity<CompanyJobPoco>
                (entry =>
                {
                    entry.HasOne(j => j.CompanyProfiles)
                    .WithMany(p => p.CompanyJobs)
                    .HasForeignKey(j => j.Company);
                });

            modelBuilder.Entity<CompanyJobSkillPoco>
                (entry =>
                {
                    entry.HasOne(j => j.CompanyJobs)
                    .WithMany(c => c.CompanyJobSkills)
                    .HasForeignKey(j => j.Job);
                });

            modelBuilder.Entity<CompanyLocationPoco>
               (entry =>
               {
                   entry.HasOne(j => j.CompanyProfiles)
                   .WithMany(l => l.CompanyLocations)
                   .HasForeignKey(j => j.Company);
               });

            modelBuilder.Entity<SecurityLoginsLogPoco>
               (entry =>
               {
                   entry.HasOne(l => l.SecurityLogins)
                   .WithMany(s => s.SecurityLoginsLogs)
                   .HasForeignKey(l => l.Login);
               });

            modelBuilder.Entity<SecurityLoginsRolePoco>
               (entry =>
               {
                   entry.HasOne(l => l.SecurityLogins)
                   .WithMany(s => s.SecurityLoginsRoles)
                   .HasForeignKey(l => l.Login);
               });

            modelBuilder.Entity<SecurityLoginsRolePoco>
               (entry =>
               {
                   entry.HasOne(l => l.SecurityRoles)
                   .WithMany(s => s.SecurityLoginsRoles)
                   .HasForeignKey(l => l.Role); ;
               });
            /*
            modelBuilder.Entity<SystemCountryCodePoco>
                (entry =>
                {
                    entry.HasMany(e => e.ApplicantProfiles)
                    .WithOne(c => c.SystemCountryCodes);
                });
            modelBuilder.Entity<SystemCountryCodePoco>
                (entry =>
                {
                    entry.HasMany(e => e.ApplicantWorkHistorys)
                    .WithOne(c => c.SystemCountryCodes);
                });
                */
            base.OnModelCreating(modelBuilder);
        }
    }
}
