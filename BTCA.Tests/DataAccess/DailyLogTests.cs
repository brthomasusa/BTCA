using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Xunit;
using BTCA.Common.Entities;
using BTCA.DataAccess.Core;
using BTCA.DataAccess.EF;

namespace BTCA.Tests.DataAccess
{
    public class DailyLogTests
    {
        [Fact]
        public void Test_DailyLogCreate()
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
                    HOSTestData.CreateViews(context);                                     
                }

                using (var context = new HOSContext(options))
                {
                    IRepository repository = new Repository(context);
                    
                    var dailyLog = new DailyLog 
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
                    };

                    repository.Create<DailyLog>(dailyLog);
                    repository.Save();

                    var test = repository.Find<DailyLog>(log => log.LogID == dailyLog.LogID);

                    Assert.NotNull(test);
                    Assert.Equal(new DateTime(2016,9,7), dailyLog.LogDate);
                }

            } finally {
                connection.Close();
            }            
        }

        [Fact]
        public void Test_DailyLogUpdate()
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
                    HOSTestData.CreateViews(context);                                     
                }

                using (var context = new HOSContext(options))
                {
                    IRepository repository = new Repository(context);
                    
                    var dailyLog = new DailyLog 
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
                    };

                    repository.Create<DailyLog>(dailyLog);
                    repository.Save();

                    var test = repository.Find<DailyLog>(log => log.LogID == dailyLog.LogID);

                    Assert.NotNull(test);
                    Assert.Equal(new DateTime(2016,9,7), dailyLog.LogDate);

                    test.LogDate = new DateTime(2016,9,9);
                    repository.Update<DailyLog>(test);
                    repository.Save();

                    test = repository.Find<DailyLog>(log => log.LogID == dailyLog.LogID);
                    Assert.Equal(new DateTime(2016,9,9), test.LogDate);
                }

            } finally {
                connection.Close();
            }            
        }

        [Fact]
        public void Test_DailyLogDeleteById()
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
                    HOSTestData.CreateViews(context);                                    
                }

                using (var context = new HOSContext(options))
                {
                    IRepository repository = new Repository(context);
                    
                    var dailyLog = new DailyLog 
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
                    };

                    repository.Create<DailyLog>(dailyLog);
                    repository.Save();

                    var test = repository.Find<DailyLog>(log => log.LogID == dailyLog.LogID);

                    Assert.NotNull(test);

                    repository.Delete<DailyLog>(log => log.LogID == dailyLog.LogID);
                    repository.Save();

                    test = repository.Find<DailyLog>(log => log.LogID == dailyLog.LogID);
                    Assert.Null(test);
                }

            } finally {
                connection.Close();
            }              
        }

        [Fact]
        public void Test_DailyLogDeleteObject()
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
                    HOSTestData.CreateViews(context);                                     
                }

                using (var context = new HOSContext(options))
                {
                    IRepository repository = new Repository(context);
                    
                    var dailyLog = new DailyLog 
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
                    };

                    repository.Create<DailyLog>(dailyLog);
                    repository.Save();

                    var test = repository.Find<DailyLog>(log => log.LogID == dailyLog.LogID);

                    Assert.NotNull(test);

                    repository.Delete<DailyLog>(test);
                    repository.Save();

                    test = repository.Find<DailyLog>(log => log.LogID == dailyLog.LogID);
                    Assert.Null(test);
                }

            } finally {
                connection.Close();
            }            
        }

        [Fact]
        public void Test_DailyLogGetAll()
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
                    HOSTestData.LoadDailyLogTable(context);
                    HOSTestData.CreateViews(context);
                }

                using (var context = new HOSContext(options))
                {
                    IRepository repository = new Repository(context);
                    
                    var dailyLogs = repository.All<DailyLog>();

                    Assert.NotNull(dailyLogs);
                    Assert.Equal(3, dailyLogs.ToList().Count());
                }

            } finally {
                connection.Close();
            }             
        }

        [Fact]
        public void Test_DailyLogFilter()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try {

                var options = new DbContextOptionsBuilder<HOSContext>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new HOSContext(options))
                {
                    HOSTestData.CreateViews(context);
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
                    IRepository repository = new Repository(context);
                    
                    var lowDate = new DateTime(2016, 9, 1);
                    var highDate = new DateTime(2016,9,30);

                    var dailyLogs = repository.Filter<DailyLog>(log => log.LogDate > lowDate && log.LogDate < highDate );

                    Assert.NotNull(dailyLogs);
                    Assert.Equal(3, dailyLogs.ToList().Count());
                }

            } finally {
                connection.Close();
            }
        }

        [Fact]
        public void Test_DailyLogDetailCreate()
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
                    HOSTestData.LoadDutyStatusActivityTable(context);
                    HOSTestData.LoadDutyStatusTable(context);
                    HOSTestData.LoadStateProvinceCodeTable(context);
                    HOSTestData.LoadCompanyTable(context);
                    HOSTestData.LoadAppUserTable(context);
                    HOSTestData.CreateViews(context);                                     
                }

                using (var context = new HOSContext(options))
                {
                    IRepository repository = new Repository(context);
                    
                    var dailyLog = new DailyLog 
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
                    };

                    var logDetail = new DailyLogDetail 
                    { 
                        LogDetailID = 1, 
                        DutyStatusID = 4, 
                        StartTime = new DateTime(2016,9,7, 12,45,0),
                        StopTime = new DateTime(2016,9,7, 13,0,0),
                        LocationCity = "Ft Worth",
                        StateProvinceId = 45,
                        DutyStatusActivityID = 1,
                        LogID = 1  
                    };

                    dailyLog.DailyLogDetails.Add(logDetail);
                    repository.Create<DailyLog>(dailyLog);
                    repository.Save();

                    var test = repository.Find<DailyLog>(log => log.LogID == dailyLog.LogID);
                    
                    Assert.NotNull(test);
                    Assert.Equal(new DateTime(2016,9,7), dailyLog.LogDate);

                    var logDetailTest = test.DailyLogDetails.FirstOrDefault();

                    Assert.NotNull(logDetailTest);
                    Assert.Equal(new DateTime(2016,9,7, 12,45,0), logDetailTest.StartTime);                    
                }

            } finally {
                connection.Close();
            }            
        }        

        [Fact]
        public static void Test_DailyLogDetailUpdate()
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
                    HOSTestData.LoadDutyStatusActivityTable(context);
                    HOSTestData.LoadDutyStatusTable(context);
                    HOSTestData.LoadStateProvinceCodeTable(context);
                    HOSTestData.LoadCompanyTable(context);
                    HOSTestData.LoadAppUserTable(context);
                    HOSTestData.CreateViews(context);                                     
                }

                using (var context = new HOSContext(options))
                {
                    IRepository repository = new Repository(context);
                    
                    var dailyLog = new DailyLog 
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
                    };

                    var logDetail = new DailyLogDetail 
                    { 
                        LogDetailID = 1, 
                        DutyStatusID = 4, 
                        StartTime = new DateTime(2016,9,7, 12,45,0),
                        StopTime = new DateTime(2016,9,7, 13,0,0),
                        LocationCity = "Ft Worth",
                        StateProvinceId = 45,
                        DutyStatusActivityID = 1,
                        LogID = 1  
                    };

                    dailyLog.DailyLogDetails.Add(logDetail);
                    repository.Create<DailyLog>(dailyLog);
                    repository.Save();

                    var test = repository.Find<DailyLog>(log => log.LogID == dailyLog.LogID);
                    var logDetailTest = test.DailyLogDetails.FirstOrDefault();

                    logDetailTest.StartTime = new DateTime(2016,9,7, 13,45,0);
                    logDetailTest.StopTime = new DateTime(2016,9,7, 14,0,0);


                    Assert.Equal(new DateTime(2016,9,7, 13,45,0), logDetailTest.StartTime);
                    Assert.Equal(new DateTime(2016,9,7, 14,0,0), logDetailTest.StopTime);                    
                }

            } finally {
                connection.Close();
            }             
        }

        [Fact]
        public void Test_DailyLogDetailDelete()
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
                    HOSTestData.LoadDutyStatusActivityTable(context);
                    HOSTestData.LoadDutyStatusTable(context);
                    HOSTestData.LoadStateProvinceCodeTable(context);
                    HOSTestData.LoadCompanyTable(context);
                    HOSTestData.LoadAppUserTable(context);
                    HOSTestData.CreateViews(context);                                     
                }

                using (var context = new HOSContext(options))
                {
                    IRepository repository = new Repository(context);
                    
                    var dailyLog = new DailyLog 
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
                    };

                    var logDetail = new DailyLogDetail 
                    { 
                        LogDetailID = 1, 
                        DutyStatusID = 4, 
                        StartTime = new DateTime(2016,9,7, 12,45,0),
                        StopTime = new DateTime(2016,9,7, 13,0,0),
                        LocationCity = "Ft Worth",
                        StateProvinceId = 45,
                        DutyStatusActivityID = 1,
                        LogID = 1  
                    };

                    dailyLog.DailyLogDetails.Add(logDetail);
                    repository.Create<DailyLog>(dailyLog);
                    repository.Save();

                    var test = repository.Find<DailyLog>(log => log.LogID == dailyLog.LogID);
                    var logDetailTest = test.DailyLogDetails.FirstOrDefault();

                    Assert.NotNull(logDetailTest);
                    test.DailyLogDetails.Remove(logDetailTest);
                    repository.Save(); 
                    logDetailTest = test.DailyLogDetails.FirstOrDefault();

                    Assert.Null(logDetailTest);                   
                }

            } finally {
                connection.Close();
            }              
        }

        [Fact]
        public void Test_DailyLogDetailAllForUser()
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
                    HOSTestData.LoadDutyStatusActivityTable(context);
                    HOSTestData.LoadDutyStatusTable(context);
                    HOSTestData.LoadStateProvinceCodeTable(context);
                    HOSTestData.LoadCompanyTable(context);
                    HOSTestData.LoadAppUserTable(context);
                    HOSTestData.LoadDailyLogTable(context);
                    HOSTestData.CreateViews(context);                                     
                }

                using (var context = new HOSContext(options))
                {
                    IRepository repository = new Repository(context);

                    var user = repository.Find<AppUser>(u => u.UserName == "leadfoot");
                    Assert.NotNull(user);

                    var dailyLogs = repository.Filter<DailyLog>(dl => dl.DriverID == user.Id);
                    Assert.Equal(3, dailyLogs.ToList().Count());
                }

            } finally {
                connection.Close();
            }            
        }
    }
}
