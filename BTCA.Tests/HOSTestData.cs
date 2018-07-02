using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using BTCA.Common.Entities;
using BTCA.DataAccess.EF;

namespace BTCA.Tests
{
    public static class HOSTestData
    {
        public static void CreateViews(HOSContext ctx)
        {
            ctx.Database.ExecuteSqlCommand(
                @"CREATE VIEW CompanyAddress
                AS
                SELECT A.ID, A.addressline1, A.addressline2, A.city, S.statecode, A.stateprovinceid, A.zipcode, 
                       S.countrycode, A.ishq, A.companyid, A.CreatedBy, A.CreatedOn, A.UpdatedBy, A.UpdatedOn                    
                FROM Addresses AS A
                    JOIN StateProvinceCodes AS S ON A.StateProvinceId = S.ID;"
            ); 

            ctx.Database.ExecuteSqlCommand(
                @"CREATE VIEW DailyLogModel
                  AS
                    SELECT DL.LogID, DL.LogDate, DL.BeginningMileage, DL.EndingMileage, DL.TruckNumber, DL.TrailerNumber,
                       DL.IsSigned, DL.DriverID, DL.Notes, U.FirstName, U.LastName, U.MiddleInitial,
                       DL.CreatedBy, DL.CreatedOn, DL.UpdatedBy, DL.UpdatedOn
                From DailyLogs DL
                    JOIN AspNetUsers AS U
                        ON DL.DriverID = U.Id;"
            );

            ctx.Database.ExecuteSqlCommand(
                @"CREATE VIEW DailyLogDetailModel
                AS
                SELECT DLD.LogDetailID, DLD.LogID, DLD.DutyStatusID, DS.ShortName, DLD.StartTime, DLD.StopTime, 
                    DLD.LocationCity, DLD.StateProvinceId, SPC.StateCode,
                    DLD.DutyStatusActivityID, DSA.Activity, DLD.Notes, DLD.Longitude, DLD.Latitude,
                    DLD.CreatedBy, DLD.CreatedOn, DLD.UpdatedBy, DLD.UpdatedOn
                FROM DailyLogDetails AS DLD
                    JOIN DutyStatuses AS DS
                        ON DLD.DutyStatusID = DS.DutyStatusID
                    JOIN StateProvinceCodes AS SPC
                        ON DLD.StateProvinceId = SPC.ID
                    JOIN DutyStatusActivities AS DSA
                        ON DLD.DutyStatusActivityID = DSA.DutyStatusActivityID;"                
            );



        } 

        public static void LoadDailyLogTable(HOSContext ctx)
        {
            var data = new List<DailyLog> 
            {
                new DailyLog 
                {
                    LogID = 1,
                    LogDate = new DateTime(2016,9,7),
                    BeginningMileage = 899201,
                    EndingMileage = 899423,
                    TruckNumber = "3082",
                    TrailerNumber = "9225",
                    IsSigned = true,
                    Notes = "Dropped trailer  9225 at Whirlpool and picked up loaded trailer 9159",
                    DriverID = 4
                },
                new DailyLog 
                {
                    LogID = 2,
                    LogDate = new DateTime(2016,9,8),
                    BeginningMileage = 899423,
                    EndingMileage = 900065,
                    TruckNumber = "3082",
                    TrailerNumber = "9159",
                    IsSigned = true,
                    DriverID = 4
                },
                new DailyLog 
                {
                    LogID = 3,
                    LogDate = new DateTime(2016,9,9),
                    BeginningMileage = 900065,
                    TruckNumber = "3082",
                    TrailerNumber = "9159",
                    IsSigned = false,
                    DriverID = 4
                }                                                  
            };

            ctx.DailyLogs.AddRange(data);
            ctx.SaveChanges();             
        }

        public static void LoadAppUserTable(HOSContext ctx)
        {
            var data = new List<AppUser> 
            {
                new AppUser 
                { 
                    Id = 1, 
                    CompanyID = 1, 
                    ConcurrencyStamp = "d804c50d-5ed4-46f4-b63e-cad6314271d7", 
                    Email = "admin@btechnical-consulting.com", 
                    FirstName = "System", 
                    LastName = "Administrator", 
                    PhoneNumber = "214-687-9000",
                    PasswordHash = "AQAAAAEAACcQAAAAEGdu5p7gEArfjWr4O8J69pUsFE/SRU2bBhtf8Axq5W9SKQ8VIMxlJPxk71HN0xVhIg==",
                    SecurityStamp = "198469f7-7243-48ff-9649-a5dbe6236280",
                    UserName = "SysAdmin" 
                },
                new AppUser 
                { 
                    Id = 2, 
                    CompanyID = 6, 
                    ConcurrencyStamp = "9cd70dde-f381-4a8b-b93f-28a840514d32", 
                    Email = "j.doe@cardlog.com", 
                    FirstName = "John", 
                    LastName = "Doe", 
                    PhoneNumber = "555-555-5555",
                    PhoneExtension = "5555",
                    PasswordHash = "AQAAAAEAACcQAAAAEBwwUr9zA3rjwMHWX9muXLMVHwNQhEUoezBLea5jmaGNAT5sH8zuIz04qjA9zH4R7Q==",
                    SecurityStamp = "3966bcfa-59dd-44be-b83e-cab75312b0e6",
                    UserName = "jdoe" 
                },
                new AppUser 
                { 
                    Id = 3, 
                    CompanyID = 6, 
                    ConcurrencyStamp = "93080d23-4460-4fd8-9dec-48c81e988cb2", 
                    Email = "f.castro@cardlog.com", 
                    FirstName = "Fidel", 
                    LastName = "Castro", 
                    PhoneNumber = "555-555-1235",
                    PhoneExtension = "6844",
                    PasswordHash = "AQAAAAEAACcQAAAAEBwwUr9zA3rjwMHWX9muXLMVHwNQhEUoezBLea5jmaGNAT5sH8zuIz04qjA9zH4R7Q==",
                    SecurityStamp = "c4cd4b43-7f5d-49a5-8354-982c4a254814",
                    UserName = "fcastro" 
                },
                new AppUser 
                { 
                    Id = 4, 
                    CompanyID = 2, 
                    ConcurrencyStamp = "96f0496c-ed03-4329-aa20-b3d362aeed41", 
                    Email = "j.firstchoicetransport.com", 
                    FirstName = "Jeffery", 
                    LastName = "Superdriver", 
                    PhoneNumber = "800-555-5555",
                    PhoneExtension = "999",
                    PasswordHash = "AQAAAAEAACcQAAAAEBwwUr9zA3rjwMHWX9muXLMVHwNQhEUoezBLea5jmaGNAT5sH8zuIz04qjA9zH4R7Q==",
                    SecurityStamp = "5e6423e8-5b47-4104-87ae-a399ee129926",
                    UserName = "leadfoot" 
                },
                new AppUser 
                { 
                    Id = 5, 
                    CompanyID = 6, 
                    ConcurrencyStamp = "5f5af8e0-0bb5-4515-98e0-91ba923f3166", 
                    Email = "j.thompson@cardlog.com", 
                    FirstName = "Josef", 
                    LastName = "Thompsonski", 
                    PhoneNumber = "800-555-5555",
                    PhoneExtension = "999",
                    PasswordHash = "AQAAAAEAACcQAAAAEBwwUr9zA3rjwMHWX9muXLMVHwNQhEUoezBLea5jmaGNAT5sH8zuIz04qjA9zH4R7Q==",
                    SecurityStamp = "d23a1ac9-8a65-4566-8be7-c0c36d971b1c",
                    UserName = "jthompsonski" 
                }                                                                
            };

            ctx.Users.AddRange(data);
            ctx.SaveChanges();             
        }

        public static void LoadAppRoleTable(HOSContext ctx)
        {
            var data = new List<AppRole> 
            {
                new AppRole { Id = 1, ConcurrencyStamp = "08e72399-bb2f-49f5-bbe3-693de3d5c617", Name = "Admin", NormalizedName = "ADMIN" },
                new AppRole { Id = 2, ConcurrencyStamp = "1298a579-c85a-4723-bcc4-f0d6b2b64123", Name = "Driver", NormalizedName = "DRIVER" },
                new AppRole { Id = 3, ConcurrencyStamp = "46797f8c-5830-4fb2-8fc5-81b20b630fb4", Name = "Compliance", NormalizedName = "COMPLIANCE" },
                new AppRole { Id = 4, ConcurrencyStamp = "3fa3d51e-4d5c-461e-99c3-3ca5f2e6b38b", Name = "Accounting", NormalizedName = "ACCOUNTING" },
                new AppRole { Id = 5, ConcurrencyStamp = "4044c66b-081f-4db0-9daf-f8cf6910a438", Name = "Users", NormalizedName = "USERS" }
            };

            ctx.Roles.AddRange(data);
            ctx.SaveChanges(); 
        }        

        public static void LoadDailyLogDetailTable(HOSContext ctx)
        {
            var data = new List<DailyLogDetail> 
            {
                new DailyLogDetail 
                { 
                    LogDetailID = 1, 
                    DutyStatusID = 4, 
                    StartTime = new DateTime(2016,9,7, 12,45,0),
                    StopTime = new DateTime(2016,9,7, 13,0,0),
                    LocationCity = "Ft Worth",
                    StateProvinceId = 45,
                    DutyStatusActivityID = 1,
                    LogID = 1  
                },
                new DailyLogDetail 
                { 
                    LogDetailID = 2, 
                    DutyStatusID = 3, 
                    StartTime = new DateTime(2016,9,7, 13,0,0),
                    StopTime = new DateTime(2016,9,7, 14,0,0),
                    LocationCity = "Wilmer",
                    StateProvinceId = 45,
                    DutyStatusActivityID = 11,
                    LogID = 1  
                },     
                new DailyLogDetail 
                { 
                    LogDetailID = 3, 
                    DutyStatusID = 4, 
                    StartTime = new DateTime(2016,9,7, 14,0,0),
                    StopTime = new DateTime(2016,9,7, 14,15,0),
                    LocationCity = "Wilmer",
                    StateProvinceId = 45,
                    DutyStatusActivityID = 3,
                    LogID = 1  
                },
                new DailyLogDetail 
                { 
                    LogDetailID = 4, 
                    DutyStatusID = 1, 
                    StartTime = new DateTime(2016,9,7, 14,15,0),
                    StopTime = new DateTime(2016,9,7, 18,30,0),
                    LocationCity = "Wilmer",
                    StateProvinceId = 45,
                    DutyStatusActivityID = 9,
                    LogID = 1 
                },
                new DailyLogDetail 
                { 
                    LogDetailID = 5, 
                    DutyStatusID = 4, 
                    StartTime = new DateTime(2016,9,7, 18,30,0),
                    StopTime = new DateTime(2016,9,7, 18,45,0),
                    LocationCity = "Dallas",
                    StateProvinceId = 45,
                    DutyStatusActivityID = 7,
                    LogID = 1 
                },
                new DailyLogDetail 
                { 
                    LogDetailID = 6, 
                    DutyStatusID = 3, 
                    StartTime = new DateTime(2016,9,7, 18,45,0),
                    StopTime = new DateTime(2016,9,7, 18,45,0),
                    LocationCity = "Dallas",
                    StateProvinceId = 45,
                    DutyStatusActivityID = 11,
                    LogID = 1 
                },
                new DailyLogDetail 
                { 
                    LogDetailID = 7, 
                    DutyStatusID = 4, 
                    StartTime = new DateTime(2016,9,7, 21,30,0),
                    StopTime = new DateTime(2016,9,7, 21,45,0),
                    LocationCity = "Jolly",
                    StateProvinceId = 45,
                    DutyStatusActivityID = 2,
                    LogID = 1 
                },
                new DailyLogDetail 
                { 
                    LogDetailID = 8, 
                    DutyStatusID = 2, 
                    StartTime = new DateTime(2016,9,7, 21,30,0),
                    StopTime = new DateTime(2016,9,7, 23,59,0),
                    LocationCity = "Jolly",
                    StateProvinceId = 45,
                    DutyStatusActivityID = 10,
                    LogID = 1 
                },
                new DailyLogDetail 
                { 
                    LogDetailID = 9, 
                    DutyStatusID = 4, 
                    StartTime = new DateTime(2016,9,8, 7,45,0),
                    StopTime = new DateTime(2016,9,8, 8,00,0),
                    LocationCity = "Jolly",
                    StateProvinceId = 45,
                    DutyStatusActivityID = 1,
                    LogID = 2 
                },
                new DailyLogDetail 
                { 
                    LogDetailID = 10, 
                    DutyStatusID = 3, 
                    StartTime = new DateTime(2016,9,8, 8,0,0),
                    StopTime = new DateTime(2016,9,8, 9,0,0),
                    LocationCity = "Jolly",
                    StateProvinceId = 45,
                    DutyStatusActivityID = 11,
                    LogID = 2 
                },
                new DailyLogDetail 
                { 
                    LogDetailID = 11, 
                    DutyStatusID = 1, 
                    StartTime = new DateTime(2016,9,8, 9,0,0),
                    StopTime = new DateTime(2016,9,8, 9,30,0),
                    LocationCity = "Quanah",
                    StateProvinceId = 45,
                    DutyStatusActivityID = 9,
                    LogID = 2
                },
                new DailyLogDetail 
                { 
                    LogDetailID = 12, 
                    DutyStatusID = 3, 
                    StartTime = new DateTime(2016,9,8, 9,30,0),
                    StopTime = new DateTime(2016,9,8, 12,0,0),
                    LocationCity = "Quanah",
                    StateProvinceId = 45,
                    DutyStatusActivityID = 11,
                    LogID = 2 
                },
                new DailyLogDetail 
                { 
                    LogDetailID = 13, 
                    DutyStatusID = 4, 
                    StartTime = new DateTime(2016,9,8, 12,0,0),
                    StopTime = new DateTime(2016,9,8, 12,15,0),
                    LocationCity = "Amarillo",
                    StateProvinceId = 45,
                    DutyStatusActivityID = 7,
                    LogID = 2 
                },
                new DailyLogDetail 
                { 
                    LogDetailID = 14, 
                    DutyStatusID = 1, 
                    StartTime = new DateTime(2016,9,8, 12,15,0),
                    StopTime = new DateTime(2016,9,8, 12,30,0),
                    LocationCity = "Amarillo",
                    DutyStatusActivityID = 9,
                    StateProvinceId = 45,
                    LogID = 2
                },
                new DailyLogDetail 
                { 
                    LogDetailID = 15, 
                    DutyStatusID = 3, 
                    StartTime = new DateTime(2016,9,8, 12,30,0),
                    StopTime = new DateTime(2016,9,8, 16,30,0),
                    LocationCity = "Amarillo",
                    StateProvinceId = 45,
                    DutyStatusActivityID = 11,
                    LogID = 2
                },
                new DailyLogDetail 
                { 
                    LogDetailID = 16, 
                    DutyStatusID = 4, 
                    StartTime = new DateTime(2016,9,8, 16,30,0),
                    StopTime = new DateTime(2016,9,8, 16,45,0),
                    LocationCity = "Lamar",
                    StateProvinceId = 6,
                    DutyStatusActivityID = 5,
                    LogID = 2
                },                
                new DailyLogDetail 
                { 
                    LogDetailID = 17, 
                    DutyStatusID = 1, 
                    StartTime = new DateTime(2016,9,8, 16,45,0),
                    StopTime = new DateTime(2016,9,8, 17,30,0),
                    LocationCity = "Lamar",
                    StateProvinceId = 6,
                    DutyStatusActivityID = 9,
                    LogID = 2
                },
                new DailyLogDetail 
                { 
                    LogDetailID = 18, 
                    DutyStatusID = 3, 
                    StartTime = new DateTime(2016,9,8, 17,30,0),
                    StopTime = new DateTime(2016,9,8, 20,45,0),
                    LocationCity = "Aurora",
                    StateProvinceId = 6,
                    DutyStatusActivityID = 11,
                    LogID = 2
                },
                new DailyLogDetail 
                { 
                    LogDetailID = 19, 
                    DutyStatusID = 4, 
                    StartTime = new DateTime(2016,9,8, 20,45,0),
                    StopTime = new DateTime(2016,9,8, 21,00,0),
                    LocationCity = "Aurora",
                    StateProvinceId = 6,
                    DutyStatusActivityID = 4,
                    LogID = 2
                },
                new DailyLogDetail 
                { 
                    LogDetailID = 20, 
                    DutyStatusID = 1, 
                    StartTime = new DateTime(2016,9,8, 21,00,0),
                    StopTime = new DateTime(2016,9,8, 21,15,0),
                    LocationCity = "Aurora",
                    StateProvinceId = 6,
                    DutyStatusActivityID = 9,
                    LogID = 2
                },
                new DailyLogDetail 
                { 
                    LogDetailID = 21, 
                    DutyStatusID = 4, 
                    StartTime = new DateTime(2016,9,8, 21,15,0),
                    StopTime = new DateTime(2016,9,8, 21,30,0),
                    LocationCity = "Aurora",
                    StateProvinceId = 6,
                    DutyStatusActivityID = 2,
                    LogID = 2
                },
                new DailyLogDetail 
                { 
                    LogDetailID = 22, 
                    DutyStatusID = 2, 
                    StartTime = new DateTime(2016,9,8, 21,30,0),
                    LocationCity = "Aurora",
                    StateProvinceId = 6,
                    DutyStatusActivityID = 10,
                    LogID = 2
                }               
            };

            ctx.DailyLogDetails.AddRange(data);
            ctx.SaveChanges();            
        }

        public static void LoadDutyStatusTable(HOSContext ctx)
        {
            var data = new List<DutyStatus>
            {
                new DutyStatus {DutyStatusID = 1, ShortName = "Off Duty", LongName = "Off Duty", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new DutyStatus {DutyStatusID = 2, ShortName = "Sleeper Berth", LongName = "Sleeper Berth", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new DutyStatus {DutyStatusID = 3, ShortName = "Driving", LongName = "On Duty Driving", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new DutyStatus {DutyStatusID = 4, ShortName = "On Duty", LongName = "On Duty Not Driving", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now}
            };

            ctx.DutyStatuses.AddRange(data);
            ctx.SaveChanges();            
        }

        public static void LoadDutyStatusActivityTable(HOSContext ctx)
        {
            var data = new List<DutyStatusActivity>
            {
                new DutyStatusActivity {DutyStatusActivityID = 1, Activity = "Pre-Trip", Description = "Pre-trip inspection", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new DutyStatusActivity {DutyStatusActivityID = 2, Activity = "Post-Trip", Description = "Post-trip inspection", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new DutyStatusActivity {DutyStatusActivityID = 3, Activity = "Loading", Description = "Arrive shipper/loading", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new DutyStatusActivity {DutyStatusActivityID = 4, Activity = "Unloading", Description = "Arrive receiver/unloading", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new DutyStatusActivity {DutyStatusActivityID = 5, Activity = "D.O.T.", Description = "D.O.T. inspection", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new DutyStatusActivity {DutyStatusActivityID = 6, Activity = "Maint.", Description = "Vehicle maintenance", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new DutyStatusActivity {DutyStatusActivityID = 7, Activity = "Fueling", Description = "Vehicle fueling", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new DutyStatusActivity {DutyStatusActivityID = 8, Activity = "Misc", Description = "Unspecified on duty activity", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new DutyStatusActivity {DutyStatusActivityID = 9, Activity = "Off Duty", Description = "Off Duty", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new DutyStatusActivity {DutyStatusActivityID = 10, Activity = "Sleeper", Description = "Sleeper berth", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new DutyStatusActivity {DutyStatusActivityID = 11, Activity = "Driving", Description = "Driving", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now}
            };

            ctx.DutyStatusActivities.AddRange(data);
            ctx.SaveChanges();
        }

        public static void LoadCompanyTable(HOSContext ctx)
        {
            var data = new List<Company> 
            {
                new Company {ID = 1, CompanyCode = "ADMIN001", CompanyName = "Btechnical Consulting", DOT_Number = "000000", MC_Number = "MC-000000", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new Company {ID = 2, CompanyCode = "FCT001", CompanyName = "First Choice Transport", DOT_Number = "123456", MC_Number = "MC-123456", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new Company {ID = 3, CompanyCode = "SWIFT001", CompanyName = "Swift Transportation", DOT_Number = "937712", MC_Number = "MC-987665", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new Company {ID = 4, CompanyCode = "SWIFT004", CompanyName = "Swift Trans LLC", DOT_Number = "712025", MC_Number = "MC-987665", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new Company {ID = 5, CompanyCode = "GWTM001", CompanyName = "GreatWide Truckload Management", DOT_Number = "430147", MC_Number = "MC-014987", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new Company {ID = 6, CompanyCode = "CARD001", CompanyName = "Cardinal Logistics", DOT_Number = "703028", MC_Number = "MC-654321", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},                                
                new Company {ID = 7, CompanyCode = "GWLS001", CompanyName = "Greatwide Logistics Services", DOT_Number = "380085", MC_Number = "MC-665871", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now}
            };

            ctx.Companies.AddRange(data);
            ctx.SaveChanges();
        }

        public static void LoadStateProvinceCodeTable(HOSContext ctx)
        {
            var data = new List<StateProvinceCode> 
            { 
                new StateProvinceCode {ID = 1,StateCode = "AK", StateName = "Alaska",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 2,StateCode = "AL", StateName = "Alabama",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 3,StateCode = "AR", StateName = "Arkansas",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 4,StateCode = "AZ", StateName = "Arizona",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 5,StateCode = "CA", StateName = "California",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 6,StateCode = "CO", StateName = "Colorado",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 7,StateCode = "CT", StateName = "Connecticut",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 8,StateCode = "DC", StateName = "District Of Columbia",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 9,StateCode = "DE", StateName = "Delaware",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 10,StateCode = "FL", StateName = "FlorIDa",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 11,StateCode = "GA", StateName = "Georgia",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 12,StateCode = "HI", StateName = "Hawaii",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 13,StateCode = "IA", StateName = "Iowa",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 14,StateCode = "ID", StateName = "IDaho",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 15,StateCode = "IL", StateName = "Illinois",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 16,StateCode = "IN", StateName = "Indiana",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 17,StateCode = "KS", StateName = "Kansas",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 18,StateCode = "KY", StateName = "Kentucky",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 19,StateCode = "LA", StateName = "Louisiana",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 20,StateCode = "MA", StateName = "Massachusetts",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 21,StateCode = "ME", StateName = "Maine",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 22,StateCode = "MD", StateName = "Maryland",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 23,StateCode = "MI", StateName = "Michigan",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 24,StateCode = "MN", StateName = "Minnesota",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 25,StateCode = "MO", StateName = "Missouri",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 26,StateCode = "MS", StateName = "Mississippi",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 27,StateCode = "MT", StateName = "Montana",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 28,StateCode = "NC", StateName = "North Carolina",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 29,StateCode = "ND", StateName = "North Dakota",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 30,StateCode = "NE", StateName = "Nebraska",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 31,StateCode = "NH", StateName = "New Hampshire",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 32,StateCode = "NJ", StateName = "New Jersey",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 33,StateCode = "NM", StateName = "New Mexico",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 34,StateCode = "NV", StateName = "Nevada",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 35,StateCode = "NY", StateName = "New York",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 36,StateCode = "OH", StateName = "Ohio",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 37,StateCode = "OK", StateName = "Oklahoma",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 38,StateCode = "OR", StateName = "Oregon",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 39,StateCode = "PA", StateName = "Pennsylvania",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 40,StateCode = "PR", StateName = "Puerto Rico",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 41,StateCode = "RI", StateName = "Rhode Island",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 42,StateCode = "SC", StateName = "South Carolina",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 43,StateCode = "SD", StateName = "South Dakota",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 44,StateCode = "TN", StateName = "Tennessee",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 45,StateCode = "TX", StateName = "Texas",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 46,StateCode = "UT", StateName = "Utah",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 47,StateCode = "VT", StateName = "Vermont",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 48,StateCode = "VA", StateName = "Virginia",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 49,StateCode = "WA", StateName = "Washington",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 50,StateCode = "WI", StateName = "Wisconsin",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 51,StateCode = "WV", StateName = "West Virginia",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 52,StateCode = "WY", StateName = "Wyoming",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 53,StateCode = "AB", StateName = "Alberta", CountryCode = "CAN", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 54,StateCode = "BC", StateName = "British Columbia", CountryCode = "CAN", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 55,StateCode = "MB", StateName = "Manitoba", CountryCode = "CAN", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 56,StateCode = "NB", StateName = "New Brunswick", CountryCode = "CAN", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 57,StateCode = "NL", StateName = "Newfoundland and Labrador", CountryCode = "CAN", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 58,StateCode = "NT", StateName = "Northwest Territories", CountryCode = "CAN", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 59,StateCode = "NS", StateName = "Nova Zcotia", CountryCode = "CAN", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 60,StateCode = "NU", StateName = "Nunavut", CountryCode = "CAN", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 61,StateCode = "ON", StateName = "Ontario", CountryCode = "CAN", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 62,StateCode = "PE", StateName = "Prince Edward Island", CountryCode = "CAN", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 63,StateCode = "QC", StateName = "Québec", CountryCode = "CAN", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 64,StateCode = "SK", StateName = "Saskatchewan", CountryCode = "CAN", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 65,StateCode = "YT", StateName = "Yukon", CountryCode = "CAN", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
            };

            ctx.StateProvinceCodes.AddRange(data);       
            ctx.SaveChanges();
        }           


        public static void LoadCompanyAddresses(HOSContext ctx)
        {
            var data = new List<Address>()
            {
                new Address() 
                { 
                    ID = 1,
                    AddressLine1 = "1346 Markum Ranch Rd",
                    AddressLine2 = "Ste 100",
                    City = "Fort Worth",
                    StateProvinceId = 45,
                    Zipcode = "76126",
                    IsHQ = true,
                    CompanyId = 2,
                    CreatedBy = "admin", 
                    CreatedOn = DateTime.Now, 
                    UpdatedBy = "admin", 
                    UpdatedOn = DateTime.Now                    
                },
                
                new Address() 
                { 
                    ID = 2,
                    AddressLine1 = "6591 Brighton Blvd",
                    City = "Commerce City",
                    StateProvinceId = 6,
                    Zipcode = "80022",
                    IsHQ = false,
                    CompanyId = 2,
                    CreatedBy = "admin", 
                    CreatedOn = DateTime.Now, 
                    UpdatedBy = "admin", 
                    UpdatedOn = DateTime.Now                                         
                },   
                new Address() 
                { 
                    ID = 3,
                    AddressLine1 = "12404 Park Central D",
                    AddressLine2 = "Ste 300",
                    City = "Dallas",
                    StateProvinceId = 45,
                    Zipcode = "75251",
                    IsHQ = true,
                    CompanyId = 7,
                    CreatedBy = "admin", 
                    CreatedOn = DateTime.Now, 
                    UpdatedBy = "admin", 
                    UpdatedOn = DateTime.Now                                         
                },
                new Address() 
                { 
                    ID = 4,
                    AddressLine1 = "2150 Cabot Boulevard West",
                    City = "Langhorne",
                    StateProvinceId = 39,
                    Zipcode = "19047",
                    IsHQ = true,
                    CompanyId = 5,
                    CreatedBy = "admin", 
                    CreatedOn = DateTime.Now, 
                    UpdatedBy = "admin", 
                    UpdatedOn = DateTime.Now                                         
                }, 
                new Address() 
                { 
                    ID = 5,
                    AddressLine1 = "3250 N Longhorn Dr",
                    City = "Lancaster",
                    StateProvinceId = 45,
                    Zipcode = "75134",
                    IsHQ = false,
                    CompanyId = 3,
                    CreatedBy = "admin", 
                    CreatedOn = DateTime.Now, 
                    UpdatedBy = "admin", 
                    UpdatedOn = DateTime.Now                                         
                },
                new Address() 
                { 
                    ID = 6,
                    AddressLine1 = "22 South 75th Street",
                    City = "Phoenix",
                    StateProvinceId = 4,
                    Zipcode = "85043",
                    IsHQ = true,
                    CompanyId = 3,
                    CreatedBy = "admin", 
                    CreatedOn = DateTime.Now, 
                    UpdatedBy = "admin", 
                    UpdatedOn = DateTime.Now                                         
                }, 
                                                          
            };

            ctx.AddRange(data);
            ctx.SaveChanges();
        }        
    }    
}
