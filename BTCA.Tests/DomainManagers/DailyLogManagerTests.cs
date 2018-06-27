using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Xunit;
using BTCA.Common.BusinessObjects;
using BTCA.DataAccess.Core;
using BTCA.DataAccess.EF;
using BTCA.DomainLayer.Managers.Interface;
using BTCA.DomainLayer.Managers.Implementation;

namespace BTCA.Tests.DomainManagers
{
    public class DailyLogManagerTest
    {
        [Fact]
        public void Test_DailyLogMgr_CreateDailyLog()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try {

                var options = new DbContextOptionsBuilder<HOSContext>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new HOSContext(options))
                {
                    context.Database.EnsureCreated();
                }

                using (var context = new HOSContext(options))
                {
                    HOSTestData.LoadStateProvinceCodeTable(context);
                    HOSTestData.LoadDutyStatusTable(context);
                    HOSTestData.LoadDutyStatusActivityTable(context);
                    HOSTestData.LoadCompanyTable(context);
                    HOSTestData.LoadAppUserTable(context);
                    HOSTestData.LoadDailyLogTable(context);
                    HOSTestData.LoadDailyLogDetailTable(context);                                     
                }

                using (var context = new HOSContext(options))
                {
                    IDailyLogManager logMgr = new DailyLogManager(new Repository(context));

                    var dailyLogModel = new DailyLogModel 
                    {
                        LogID = 4,
                        LogDate = new DateTime(2016,9,10),
                        BeginningMileage = 963000,
                        TruckNumber = "3082",
                        TrailerNumber = "9225",
                        IsSigned = false,
                        Notes = "Dropped trailer  9225 at Whirlpool and picked up loaded trailer 9159",
                        DriverID = 4,
                        CreatedBy = "sysadmin",
                        CreatedOn = new DateTime(2016,9,10,8,0,0),
                        UpdatedBy = "sysadmin",
                        UpdatedOn = new DateTime(2016,9,10,8,0,0),                        
                    };

                    logMgr.Create(dailyLogModel);
                    logMgr.SaveChanges();

                    var test = logMgr.GetDailyLog(log => log.LogID == dailyLogModel.LogID);
                    Assert.NotNull(test);
                    Assert.Equal(new DateTime(2016,9,10), test.LogDate.Date);
                }

            } finally {
                connection.Close();
            } 
        }

        [Fact]
        public void Test_DailyLogMgr_UpdateDailyLog()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try {

                var options = new DbContextOptionsBuilder<HOSContext>()
                    .UseSqlite(connection)
                    .EnableSensitiveDataLogging()
                    .Options;

                using (var context = new HOSContext(options))
                {
                    context.Database.EnsureCreated();
                }

                using (var context = new HOSContext(options))
                {
                    HOSTestData.LoadStateProvinceCodeTable(context);
                    HOSTestData.LoadDutyStatusTable(context);
                    HOSTestData.LoadDutyStatusActivityTable(context);
                    HOSTestData.LoadCompanyTable(context);
                    HOSTestData.LoadAppUserTable(context);
                    HOSTestData.LoadDailyLogTable(context);
                    HOSTestData.LoadDailyLogDetailTable(context);                                   
                }

                using (var context = new HOSContext(options))
                {
                    IDailyLogManager logMgr = new DailyLogManager(new Repository(context));

                    var dailyLogModel = logMgr.GetDailyLog(log => log.LogID == 1);
                    var currentTimestamp = DateTime.Now;
                    dailyLogModel.UpdatedOn = currentTimestamp;

                    logMgr.Update(dailyLogModel);
                    logMgr.SaveChanges();

                    dailyLogModel = logMgr.GetDailyLog(log => log.LogID == 1);
                    Assert.Equal(currentTimestamp, dailyLogModel.UpdatedOn);
                }

            } finally {
                connection.Close();
            } 
        }        

        [Fact]
        public void Test_DailyLogMgr_DeleteDailyLog()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try {

                var options = new DbContextOptionsBuilder<HOSContext>()
                    .UseSqlite(connection)
                    .EnableSensitiveDataLogging()
                    .Options;

                using (var context = new HOSContext(options))
                {
                    context.Database.EnsureCreated();
                }

                using (var context = new HOSContext(options))
                {
                    HOSTestData.LoadStateProvinceCodeTable(context);
                    HOSTestData.LoadDutyStatusTable(context);
                    HOSTestData.LoadDutyStatusActivityTable(context);
                    HOSTestData.LoadCompanyTable(context);
                    HOSTestData.LoadAppUserTable(context);
                    HOSTestData.LoadDailyLogTable(context);
                    HOSTestData.LoadDailyLogDetailTable(context);                                    
                }

                using (var context = new HOSContext(options))
                {
                    IDailyLogManager logMgr = new DailyLogManager(new Repository(context));

                    var dailyLogModel = logMgr.GetDailyLog(log => log.LogID == 1);
                    Assert.NotNull(dailyLogModel);

                    logMgr.Delete(dailyLogModel);
                    logMgr.SaveChanges();

                    dailyLogModel = logMgr.GetDailyLog(log => log.LogID == 1);
                    Assert.Null(dailyLogModel);
                }

            } finally {
                connection.Close();
            }
        }        

        [Fact]
        public void Test_DailyLogMgr_SelectDailyLog()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try {

                var options = new DbContextOptionsBuilder<HOSContext>()
                    .UseSqlite(connection)
                    .EnableSensitiveDataLogging()
                    .Options;

                using (var context = new HOSContext(options))
                {
                    context.Database.EnsureCreated();
                }

                using (var context = new HOSContext(options))
                {
                    HOSTestData.LoadStateProvinceCodeTable(context);
                    HOSTestData.LoadDutyStatusTable(context);
                    HOSTestData.LoadDutyStatusActivityTable(context);
                    HOSTestData.LoadCompanyTable(context);
                    HOSTestData.LoadAppUserTable(context);
                    HOSTestData.LoadDailyLogTable(context);
                    HOSTestData.LoadDailyLogDetailTable(context);                                    
                }

                using (var context = new HOSContext(options))
                {
                    IDailyLogManager logMgr = new DailyLogManager(new Repository(context));

                    var allLogs = logMgr.GetAll().ToList();
                    var allLogsForDrv = logMgr.GetDailyLogs(log => log.DriverID == 4);

                    Assert.Equal(3, allLogs.Count());
                    Assert.Equal(3, allLogsForDrv.Count());
                }

            } finally {
                connection.Close();
            }
        } 

        [Fact]
        public void Test_DailyLogMgr_SelectAllDetailsForOneDay()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try {

                var options = new DbContextOptionsBuilder<HOSContext>()
                    .UseSqlite(connection)
                    .EnableSensitiveDataLogging()
                    .Options;

                using (var context = new HOSContext(options))
                {
                    context.Database.EnsureCreated();
                }

                using (var context = new HOSContext(options))
                {
                    HOSTestData.LoadStateProvinceCodeTable(context);
                    HOSTestData.LoadDutyStatusTable(context);
                    HOSTestData.LoadDutyStatusActivityTable(context);
                    HOSTestData.LoadCompanyTable(context);
                    HOSTestData.LoadAppUserTable(context);
                    HOSTestData.LoadDailyLogTable(context);
                    HOSTestData.LoadDailyLogDetailTable(context);                                     
                }

                using (var context = new HOSContext(options))
                {
                    IDailyLogManager logMgr = new DailyLogManager(new Repository(context));

                    var dailyLogModel = logMgr.GetDailyLog(log => log.LogDate == new DateTime(2016,9,7));
                    Assert.NotNull(dailyLogModel);

                    var dailyLogDetailModel = logMgr.GetDailyLogDetails(details => details.LogID == dailyLogModel.LogID);
                    Assert.NotNull(dailyLogDetailModel);
                    Assert.Equal(8, dailyLogDetailModel.ToList().Count());
                }

            } finally {
                connection.Close();
            }
        }

        [Fact]
        public void Test_DailyLogMgr_CreateDailyLogDetail()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try {

                var options = new DbContextOptionsBuilder<HOSContext>()
                    .UseSqlite(connection)
                    .EnableSensitiveDataLogging()
                    .Options;

                using (var context = new HOSContext(options))
                {
                    context.Database.EnsureCreated();
                }

                using (var context = new HOSContext(options))
                {
                    HOSTestData.LoadStateProvinceCodeTable(context);
                    HOSTestData.LoadDutyStatusTable(context);
                    HOSTestData.LoadDutyStatusActivityTable(context);
                    HOSTestData.LoadCompanyTable(context);
                    HOSTestData.LoadAppUserTable(context);
                    HOSTestData.LoadDailyLogTable(context);
                    HOSTestData.LoadDailyLogDetailTable(context);                                     
                }

                using (var context = new HOSContext(options))
                {
                    IDailyLogManager logMgr = new DailyLogManager(new Repository(context));
                    var dailyLog = logMgr.GetDailyLog(log => log.LogID == 3);

                    // Retrieve the last pre-trip entry for this driver
                    var mostRecentPreTrip = logMgr.GetLastPreTripInspection(drv => drv.DriverID == dailyLog.DriverID);
                    
                    // Determine hours available on 14
                    // Determine hours available on 11
                    // Determine if in violation of 30 min break rule
                    // Determine current duty status
                    // Determine duty status driver is attempting to move to

                    var dailyLogDetailModel = new DailyLogDetailModel
                    {
                        LogDetailID = 23,
                        LogID = dailyLog.LogID,
                        DutyStatusID = 4,
                        StartTime = new DateTime(2016,9,9,8,0,0),
                        LocationCity = "Fort Worth",
                        StateProvinceId = 45,
                        DutyStatusActivityID = 1
                    };

                    logMgr.CreateLogDetail(dailyLogDetailModel);
                    logMgr.SaveChanges();

                    dailyLogDetailModel = logMgr.GetDailyLogDetail(details => details.LogID == dailyLog.LogID);
                    Assert.NotNull(dailyLogDetailModel);
                }

            } finally {
                connection.Close();
            }            
        }

        // [Fact]
        // public void Test_DailyLogMgr_UpdateDailyLogDetail()
        // {
        //     var connection = new SqliteConnection("DataSource=:memory:");
        //     connection.Open();

        //     try {

        //         var options = new DbContextOptionsBuilder<HOSContext>()
        //             .UseSqlite(connection)
        //             .EnableSensitiveDataLogging()
        //             .Options;

        //         using (var context = new HOSContext(options))
        //         {
        //             context.Database.EnsureCreated();
        //         }

        //         using (var context = new HOSContext(options))
        //         {
        //             HOSTestData.LoadStateProvinceCodeTable(context);
        //             HOSTestData.LoadDutyStatusTable(context);
        //             HOSTestData.LoadDutyStatusActivityTable(context);
        //             HOSTestData.LoadCompanyTable(context);
        //             HOSTestData.LoadAppUserTable(context);
        //             HOSTestData.LoadDailyLogTable(context);
        //             HOSTestData.LoadDailyLogDetailTable(context);                                     
        //         }

        //         using (var context = new HOSContext(options))
        //         {
        //             IDailyLogManager logMgr = new DailyLogManager(new Repository(context));

        //             var logDetail = logMgr.GetDailyLogDetail(details => details.StartTime == new DateTime(2016,9,8, 21,30,0));
        //             logDetail.StopTime = new DateTime(2016,9,9, 7,30,0);
        //             logMgr.UpdateLogDetail(logDetail);
        //             logMgr.SaveChanges();

        //             logDetail = logMgr.GetDailyLogDetail(details => details.StartTime == new DateTime(2016,9,8, 21,30,0));
        //             Assert.Equal(new DateTime(2016,9,9, 7,30,0), logDetail.StopTime);                 
        //         }

        //     } finally {
        //         connection.Close();
        //     }             
        // }

        // [Fact]
        // public void Test_DailyLogMgr_DeleteDailyLogDetail()
        // {
        //     var connection = new SqliteConnection("DataSource=:memory:");
        //     connection.Open();

        //     try {

        //         var options = new DbContextOptionsBuilder<HOSContext>()
        //             .UseSqlite(connection)
        //             .EnableSensitiveDataLogging()
        //             .Options;

        //         using (var context = new HOSContext(options))
        //         {
        //             context.Database.EnsureCreated();
        //         }

        //         using (var context = new HOSContext(options))
        //         {
        //             HOSTestData.LoadStateProvinceCodeTable(context);
        //             HOSTestData.LoadDutyStatusTable(context);
        //             HOSTestData.LoadDutyStatusActivityTable(context);
        //             HOSTestData.LoadCompanyTable(context);
        //             HOSTestData.LoadAppUserTable(context);
        //             HOSTestData.LoadDailyLogTable(context);
        //             HOSTestData.LoadDailyLogDetailTable(context);                                     
        //         }

        //         using (var context = new HOSContext(options))
        //         {
        //             IDailyLogManager logMgr = new DailyLogManager(new Repository(context));

        //             var logDetail = logMgr.GetDailyLogDetail(details => details.StartTime == new DateTime(2016,9,8, 21,30,0));
        //             Assert.NotNull(logDetail);
        //             logMgr.DeleteLogDetail(logDetail);
        //             logMgr.SaveChanges();

        //             logDetail = logMgr.GetDailyLogDetail(details => details.StartTime == new DateTime(2016,9,8, 21,30,0));
        //             Assert.Null(logDetail);                 
        //         }

        //     } finally {
        //         connection.Close();
        //     }             
        // }

        // [Fact]
        // public void Test_DailyLogMgr_SelectAllLogDetailsForOneDailyLog()
        // {
        //     var connection = new SqliteConnection("DataSource=:memory:");
        //     connection.Open();

        //     try {

        //         var options = new DbContextOptionsBuilder<HOSContext>()
        //             .UseSqlite(connection)
        //             .EnableSensitiveDataLogging()
        //             .Options;

        //         using (var context = new HOSContext(options))
        //         {
        //             context.Database.EnsureCreated();
        //         }

        //         using (var context = new HOSContext(options))
        //         {
        //             HOSTestData.LoadStateProvinceCodeTable(context);
        //             HOSTestData.LoadDutyStatusTable(context);
        //             HOSTestData.LoadDutyStatusActivityTable(context);
        //             HOSTestData.LoadCompanyTable(context);
        //             HOSTestData.LoadAppUserTable(context);
        //             HOSTestData.LoadDailyLogTable(context);
        //             HOSTestData.LoadDailyLogDetailTable(context);                                     
        //         }

        //         using (var context = new HOSContext(options))
        //         {
        //             IDailyLogManager logMgr = new DailyLogManager(new Repository(context));
        //             var dailyLog = logMgr.GetDailyLog(d => d.LogID == 2);

        //             Assert.NotNull(dailyLog);
        //             Assert.Equal(14, dailyLog.DailyLogDetails.Count());              
        //         }

        //     } finally {
        //         connection.Close();
        //     }
        // }                  
    }
}
