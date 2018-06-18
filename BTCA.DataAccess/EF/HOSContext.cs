using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BTCA.Common.Entities;

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

        public virtual DbSet<StateProvinceCode> StateProvinceCodes { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<DutyStatus> DutyStatuses { get; set; }
        public virtual DbSet<DutyStatusActivity> DutyStatusActivities { get; set; }
        public virtual DbSet<DutyStatusChangeLocation> DutyStatusChangeLocations { get; set; }
        public virtual DbSet<LoadAssignment> Loads { get; set; }
        public virtual DbSet<DailyLog> DailyLogs { get; set; }
        public virtual DbSet<DailyLogDetail> DailyLogDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder); 

            if (!optionsBuilder.IsConfigured)
            {
                var connection = @"Server=tcp:mssql-svr,1433;Database=HOSv3;User ID=sa;Password=Info99Gum;Connection Timeout=30";
                optionsBuilder.UseSqlServer(connection, options => options.EnableRetryOnFailure());
            }
        }

        public static async Task CreateAdminAccount(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            UserManager<AppUser> userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            RoleManager<AppRole> roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();

            string username = configuration["Data:AdminUser:Name"];
            string email = configuration["Data:AdminUser:Email"];
            string password = configuration["Data:AdminUser:Password"];
            string role = configuration["Data:AdminUser:Role"];

            if (await userManager.FindByNameAsync(username) == null)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                {
                    await roleManager.CreateAsync(new AppRole(role));
                }

                AppUser user = new AppUser { UserName = username, Email = email, CompanyID = 1 };

                IdentityResult result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
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
                entity.Property(dl => dl.StartTime).HasColumnType("smalldatetime");                    
                entity.Property(dl => dl.StopTime).HasColumnType("smalldatetime");
            });                                                                  
        }                 
    }
}
