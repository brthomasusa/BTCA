﻿// <auto-generated />
using BTCA.DataAccess.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace BTCA.DataAccess.EF.Migrations
{
    [DbContext(typeof(HOSContext))]
    partial class HOSContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BTCA.Common.Entities.Address", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AddressLine1")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("AddressLine2")
                        .HasMaxLength(50);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(35);

                    b.Property<int>("CompanyId");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("IsHQ");

                    b.Property<int>("StateProvinceId");

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime>("UpdatedOn");

                    b.Property<string>("Zipcode")
                        .IsRequired()
                        .HasMaxLength(10);

                    b.HasKey("ID");

                    b.HasIndex("CompanyId");

                    b.HasIndex("StateProvinceId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("BTCA.Common.Entities.AppRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("BTCA.Common.Entities.AppUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<int>("CompanyID");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("MiddleInitial");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneExtension");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("CompanyID");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("UserName", "CompanyID")
                        .IsUnique()
                        .HasName("Uniq_UserName")
                        .HasFilter("[UserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("BTCA.Common.Entities.Company", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CompanyCode");

                    b.Property<string>("CompanyName");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("DOT_Number");

                    b.Property<string>("MC_Number");

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime>("UpdatedOn");

                    b.HasKey("ID");

                    b.HasIndex("CompanyCode")
                        .IsUnique()
                        .HasName("Uniq_CompanyCode")
                        .HasFilter("[CompanyCode] IS NOT NULL");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("BTCA.Common.Entities.DailyLog", b =>
                {
                    b.Property<int>("LogID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BeginningMileage");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<int>("DriverID");

                    b.Property<int>("EndingMileage");

                    b.Property<bool>("IsSigned");

                    b.Property<DateTime>("LogDate")
                        .HasColumnType("date");

                    b.Property<string>("Notes")
                        .HasMaxLength(4000);

                    b.Property<string>("TrailerNumber")
                        .HasMaxLength(25);

                    b.Property<string>("TruckNumber")
                        .IsRequired()
                        .HasMaxLength(25);

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime>("UpdatedOn");

                    b.HasKey("LogID");

                    b.HasIndex("DriverID");

                    b.HasIndex("LogDate")
                        .HasName("Idx_DailyLogDate");

                    b.HasIndex("LogDate", "TruckNumber", "DriverID")
                        .IsUnique()
                        .HasName("Uniq_DateTrkNumDrvID");

                    b.ToTable("DailyLogs");
                });

            modelBuilder.Entity("BTCA.Common.Entities.DailyLogDetail", b =>
                {
                    b.Property<int>("LogDetailID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<int>("DutyStatusActivityID");

                    b.Property<int>("DutyStatusID");

                    b.Property<decimal>("Latitude");

                    b.Property<string>("LocationCity")
                        .IsRequired()
                        .HasMaxLength(35);

                    b.Property<int>("LogID");

                    b.Property<decimal>("Longitude");

                    b.Property<string>("Notes");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("smalldatetime");

                    b.Property<int>("StateProvinceId");

                    b.Property<DateTime>("StopTime")
                        .HasColumnType("smalldatetime");

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime>("UpdatedOn");

                    b.HasKey("LogDetailID");

                    b.HasIndex("DutyStatusActivityID");

                    b.HasIndex("DutyStatusID");

                    b.HasIndex("LogID");

                    b.HasIndex("StateProvinceId");

                    b.ToTable("DailyLogDetails");
                });

            modelBuilder.Entity("BTCA.Common.Entities.DutyStatus", b =>
                {
                    b.Property<int>("DutyStatusID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("LongName")
                        .IsRequired()
                        .HasMaxLength(25);

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasMaxLength(8);

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime>("UpdatedOn");

                    b.HasKey("DutyStatusID");

                    b.HasIndex("ShortName")
                        .IsUnique()
                        .HasName("Uniq_DutyStatusShortName");

                    b.ToTable("DutyStatuses");
                });

            modelBuilder.Entity("BTCA.Common.Entities.DutyStatusActivity", b =>
                {
                    b.Property<int>("DutyStatusActivityID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Activity")
                        .IsRequired()
                        .HasMaxLength(18);

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(35);

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime>("UpdatedOn");

                    b.HasKey("DutyStatusActivityID");

                    b.HasIndex("Activity")
                        .IsUnique()
                        .HasName("Uniq_DutyStatusActivityName");

                    b.ToTable("DutyStatusActivities");
                });

            modelBuilder.Entity("BTCA.Common.Entities.DutyStatusChangeLocation", b =>
                {
                    b.Property<int>("StatusChangeLocationID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(35);

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<decimal>("Latitude");

                    b.Property<decimal>("Longitude");

                    b.Property<int>("StateProvinceId");

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime>("UpdatedOn");

                    b.HasKey("StatusChangeLocationID");

                    b.HasIndex("StateProvinceId");

                    b.ToTable("DutyStatusChangeLocations");
                });

            modelBuilder.Entity("BTCA.Common.Entities.LoadAssignment", b =>
                {
                    b.Property<int>("LoadID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BillOfLaden")
                        .HasMaxLength(30);

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime>("DispatchDate")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(new DateTime(2018, 6, 21, 8, 34, 12, 900, DateTimeKind.Local));

                    b.Property<int>("EmptyBeginMiles")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<int>("EmptyEndMiles")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<double>("EmptyMilesRate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(5, 3)")
                        .HasDefaultValue(0.0);

                    b.Property<double>("FuelSurchargeRate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(5, 3)")
                        .HasDefaultValue(0.0);

                    b.Property<int>("Id");

                    b.Property<string>("LoadNumber")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<int>("LoadedBeginMiles")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<int>("LoadedEndMiles")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<double>("LoadedMilesRate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(5, 3)")
                        .HasDefaultValue(0.0);

                    b.Property<string>("Notes")
                        .HasMaxLength(4000);

                    b.Property<DateTime>("SettlementDate");

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime>("UpdatedOn");

                    b.HasKey("LoadID");

                    b.HasIndex("DispatchDate")
                        .HasName("Idx_LoadDispatchDate");

                    b.HasIndex("Id");

                    b.HasIndex("LoadNumber")
                        .HasName("Idx_LoadNumber");

                    b.ToTable("LoadAssignments");
                });

            modelBuilder.Entity("BTCA.Common.Entities.StateProvinceCode", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CountryCode")
                        .IsRequired()
                        .HasColumnType("nchar(3)")
                        .HasMaxLength(3);

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("StateCode")
                        .IsRequired()
                        .HasColumnType("nchar(2)")
                        .HasMaxLength(2);

                    b.Property<string>("StateName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime>("UpdatedOn");

                    b.HasKey("ID");

                    b.HasIndex("StateCode")
                        .IsUnique()
                        .HasName("Uniq_StateCode");

                    b.HasIndex("StateName")
                        .IsUnique()
                        .HasName("Uniq_StateName");

                    b.ToTable("StateProvinceCodes");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<int>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("BTCA.Common.Entities.Address", b =>
                {
                    b.HasOne("BTCA.Common.Entities.Company", "Company")
                        .WithMany("Addresses")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BTCA.Common.Entities.StateProvinceCode", "StateProvinceCode")
                        .WithMany("Addresses")
                        .HasForeignKey("StateProvinceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BTCA.Common.Entities.AppUser", b =>
                {
                    b.HasOne("BTCA.Common.Entities.Company", "Company")
                        .WithMany("AppUsers")
                        .HasForeignKey("CompanyID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BTCA.Common.Entities.DailyLog", b =>
                {
                    b.HasOne("BTCA.Common.Entities.AppUser", "Driver")
                        .WithMany("DailyLogs")
                        .HasForeignKey("DriverID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BTCA.Common.Entities.DailyLogDetail", b =>
                {
                    b.HasOne("BTCA.Common.Entities.DutyStatusActivity", "DutyStatusActivity")
                        .WithMany("DailyLogDetails")
                        .HasForeignKey("DutyStatusActivityID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BTCA.Common.Entities.DutyStatus", "DutyStatus")
                        .WithMany("DailyLogDetails")
                        .HasForeignKey("DutyStatusID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BTCA.Common.Entities.DailyLog", "DailyLog")
                        .WithMany("DailyLogDetails")
                        .HasForeignKey("LogID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BTCA.Common.Entities.StateProvinceCode", "StateProvinceCode")
                        .WithMany("DailyLogDetails")
                        .HasForeignKey("StateProvinceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BTCA.Common.Entities.DutyStatusChangeLocation", b =>
                {
                    b.HasOne("BTCA.Common.Entities.StateProvinceCode", "StateProvinceCode")
                        .WithMany("DutyStatusChangeLocations")
                        .HasForeignKey("StateProvinceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BTCA.Common.Entities.LoadAssignment", b =>
                {
                    b.HasOne("BTCA.Common.Entities.AppUser", "Driver")
                        .WithMany("LoadAssignments")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("BTCA.Common.Entities.AppRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("BTCA.Common.Entities.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("BTCA.Common.Entities.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("BTCA.Common.Entities.AppRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BTCA.Common.Entities.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("BTCA.Common.Entities.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
