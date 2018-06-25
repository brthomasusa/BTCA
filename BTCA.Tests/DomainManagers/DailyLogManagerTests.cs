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
                    HOSTestData.LoadCompanyTable(context);
                    HOSTestData.LoadAppUserTable(context);                                     
                }

                using (var context = new HOSContext(options))
                {
                    IDailyLogManager logMgr = new DailyLogManager(new Repository(context));

                    var dailyLogModel = new DailyLogModel 
                    {
                        LogDate = new DateTime(2016,9,7),
                        BeginningMileage = 899201,
                        EndingMileage = 899423,
                        TruckNumber = "3082",
                        TrailerNumber = "9225",
                        IsSigned = false,
                        Notes = "Dropped trailer  9225 at Whirlpool and picked up loaded trailer 9159",
                        DriverID = 4,
                        CreatedBy = "sysadmin",
                        CreatedOn = DateTime.Now,
                        UpdatedBy = "sysadmin",
                        UpdatedOn = DateTime.Now,                        
                    };

                    logMgr.Create(dailyLogModel);
                    logMgr.SaveChanges();

                    var test = logMgr.GetDailyLog(log => log.DriverID == dailyLogModel.DriverID);
                    Assert.NotNull(test);
                    Assert.Equal(new DateTime(2016,9,7), test.LogDate);
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
                    HOSTestData.LoadCompanyTable(context);
                    HOSTestData.LoadAppUserTable(context);
                    HOSTestData.LoadDailyLogTable(context);                                     
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

        }        

        [Fact]
        public void Test_DailyLogMgr_SelectDailyLog()
        {

        }        
    }
}
