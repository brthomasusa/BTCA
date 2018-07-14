using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BTCA.Common.Entities;
using BTCA.Common.BusinessObjects;

namespace BTCA.DataAccess.EF
{
    public class HOSContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public HOSContext()
        {
        }   

        public HOSContext(DbContextOptions<HOSContext> options) : base(options)
        {
        }        

        public virtual void SetModified(object entity)
        {
            var entry = this.Entry(entity);
            entry.State = EntityState.Modified;
        }

        public virtual DbSet<StateProvinceCode> StateProvinceCodes { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<DutyStatus> DutyStatuses { get; set; }
        public virtual DbSet<DutyStatusActivity> DutyStatusActivities { get; set; }
        public virtual DbSet<DutyStatusChangeLocation> DutyStatusChangeLocations { get; set; }
        public virtual DbSet<LoadAssignment> LoadAssignments { get; set; }
        public virtual DbSet<DailyLog> DailyLogs { get; set; }
        public virtual DbSet<DailyLogDetail> DailyLogDetails { get; set; }

        public virtual DbQuery<CompanyAddress> CompanyAddresses { get; set; }
        public virtual DbQuery<DailyLogModel> DailyLogModels { get; set; }
        public virtual DbQuery<DailyLogDetailModel> DailyLogDetailModels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder); 

            if (!optionsBuilder.IsConfigured)
            {
                var connection = @"Server=tcp:localhost,1433;Database=BTCAv1;User ID=sa;Password=Info99Gum;Connection Timeout=30";
                optionsBuilder.UseSqlServer(connection, options => options.EnableRetryOnFailure())
                              .EnableSensitiveDataLogging();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Company>(entity => 
            {
                entity.HasIndex(c => c.CompanyCode)
                    .HasName("Uniq_CompanyCode")
                    .IsUnique();
            });

            modelBuilder.Entity<StateProvinceCode>(entity => 
            {
                entity.HasIndex(s => s.StateCode)
                    .HasName("Uniq_StateCode")
                    .IsUnique();

                entity.HasIndex(c => c.StateName)
                    .HasName("Uniq_StateName")
                    .IsUnique(); 

                entity.Property(p => p.StateCode).HasColumnType("nchar(2)");  
                entity.Property(p => p.CountryCode).HasColumnType("nchar(3)");                 
            });

            modelBuilder.Entity<AppUser>(entity => 
            {
                entity.HasIndex(u => new {u.UserName, u.CompanyID})
                    .HasName("Uniq_UserName")
                    .IsUnique();
            });

            modelBuilder.Entity<DutyStatus>(entity => 
            {
                entity.HasIndex(d => d.ShortName)
                    .HasName("Uniq_DutyStatusShortName")
                    .IsUnique();
            });

            modelBuilder.Entity<DutyStatusActivity>(entity => 
            {
                entity.HasIndex(d => d.Activity)
                    .HasName("Uniq_DutyStatusActivityName")
                    .IsUnique();
            });

            modelBuilder.Entity<LoadAssignment>(la => 
            {
                la.HasIndex(load => load.LoadNumber).HasName("Idx_LoadNumber");
                la.HasIndex(load => load.DispatchDate).HasName("Idx_LoadDispatchDate");
                la.Property(load => load.DispatchDate).HasDefaultValue(DateTime.Now);
                la.Property(load => load.EmptyBeginMiles).HasDefaultValue(0);
                la.Property(load => load.EmptyEndMiles).HasDefaultValue(0); 
                la.Property(load => load.LoadedBeginMiles).HasDefaultValue(0);
                la.Property(load => load.LoadedEndMiles).HasDefaultValue(0);
                la.Property(load => load.FuelSurchargeRate).HasColumnType("decimal(5, 3)").HasDefaultValue(0.0);
                la.Property(load => load.EmptyMilesRate).HasColumnType("decimal(5, 3)").HasDefaultValue(0.0);
                la.Property(load => load.LoadedMilesRate).HasColumnType("decimal(5, 3)").HasDefaultValue(0.0);
            });

            modelBuilder.Entity<DailyLog>(entity => 
            {
                entity.HasIndex(d => new { d.LogDate, d.TruckNumber, d.DriverID })
                    .HasName("Uniq_DateTrkNumDrvID")
                    .IsUnique();

                entity.HasIndex(d => d.LogDate)
                    .HasName("Idx_DailyLogDate");

                entity.Property(dl => dl.LogDate).HasColumnType("date");                    
            });

            modelBuilder.Entity<DailyLogDetail>(entity => 
            {
                entity.HasIndex(d => d.StartTime)
                    .HasName("Idx_DailyDetailStartTime");                

                entity.HasIndex(d => d.StopTime)
                    .HasName("Idx_DailyDetailStopTime");

                entity.Property(dl => dl.StartTime).HasColumnType("datetime");                    
                entity.Property(dl => dl.StopTime).HasColumnType("datetime");
            });

            // Mapping BusinessObjects to database Views

            modelBuilder.Query<CompanyAddress>().ToView("CompanyAddress");                                                                  
            modelBuilder.Query<DailyLogModel>().ToView("DailyLogModel");
            modelBuilder.Query<DailyLogDetailModel>().ToView("DailyLogDetailModel");
        }                 
    }
}
