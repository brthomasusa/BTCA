using System;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using BTCA.Common.BusinessObjects;
using BTCA.DataAccess.Core;
using BTCA.DataAccess.EF;
using BTCA.DomainLayer.Managers.Interface;
using BTCA.DomainLayer.Managers.Implementation;

namespace BTCA.Tests.RawSqlDataAccess
{
    public class DailyLogTests
    {
        private IRepository _repository;
        private IDailyLogManager _dailyLogMgr;

        [Fact]
        public void Repo_ReturnAllDailyLogsFromView()
        {
            _repository = new Repository(new HOSContext());

            // Using DbContext.DbQuery<DailyLogModel> ; Requires EF Core 2.1's new Query Types
            var dailyLogs = _repository.DBContext.DailyLogModels.ToList();
            Assert.NotEmpty(dailyLogs);
            Assert.True(dailyLogs.Count() > 0);
        }

        [Fact]
        public void Repo_ReturnDailyLogFromView()
        {
            _repository = new Repository(new HOSContext());

            DateTime logDate = new DateTime(2016,9,7);
            var dailyLog = _repository.DBContext.DailyLogModels
                                            .Where(log => log.LogDate == logDate)
                                            .SingleOrDefault();

            Assert.NotNull(dailyLog);
            Assert.Equal(899201, dailyLog.BeginningMileage);
        }

        [Fact]
        public void Repo_ReturnDailyLogFromSql()
        {
            _repository = new Repository(new HOSContext());
            int driverId = 4;
            DateTime logDate = new DateTime(2016,9,7);
            var strDate = logDate.Date.ToString("d");
            string sql = $"SELECT * FROM dbo.DailyLogModel WHERE LogDate = \'{strDate}\' AND DriverID = {driverId}";  
            var dailyLog = _repository.DBContext.DailyLogModels.FromSql(sql).SingleOrDefault();

            Assert.NotNull(dailyLog);
            Assert.Equal(899201, dailyLog.BeginningMileage);
        }

        [Fact]
        public void Repo_ReturnAllDailyLogsFromSql()
        {
            _repository = new Repository(new HOSContext());

            int driverID = 4;
            string sql = $"SELECT * FROM dbo.DailyLogModel WHERE DriverID = {driverID};";  
            var dailyLogs = _repository.DBContext.DailyLogModels.FromSql(sql).ToList();

            Assert.NotEmpty(dailyLogs);
            Assert.True(dailyLogs.Count() > 0);           
        } 

        [Fact]
        public void Repo_ReturnAllDailyLogsFromViewFiltered()
        {
            _repository = new Repository(new HOSContext());

            var dailyLogs = _repository.DBContext.DailyLogModels.Where(log => log.DriverID == 4).ToList();
            Assert.NotEmpty(dailyLogs);
            Assert.True(dailyLogs.Count() > 0);
        }  

        [Fact]
        public void Repo_ReturnDailyLogModelFromTableValuedFunc()
        {
            _repository = new Repository(new HOSContext());

            int driverId = 4;
            DateTime logDate = new DateTime(2016,9,7);
            var strDate = logDate.Date.ToString("d");

            var dailyLog = _repository.DBContext.DailyLogModels
                                                .FromSql($"SELECT * FROM dbo.DailyLogModelByLogDateAndDrvId ({strDate}, {driverId})")
                                                .SingleOrDefault(); 

            Assert.NotNull(dailyLog);
            Assert.Equal(899201, dailyLog.BeginningMileage);                                                                                      

        } 

        [Fact]
        public void Repo_ReturnDailyLogModelsFromTableValuedFunc()
        {
            _repository = new Repository(new HOSContext());

            int driverId = 4;
            var dailyLogs = _repository.DBContext.DailyLogModels
                                                .FromSql($"SELECT * FROM dbo.DailyLogModelByDriverId ({driverId})")
                                                .ToList(); 

            Assert.NotEmpty(dailyLogs);
            Assert.True(dailyLogs.Count() > 0);                                                                                      
        } 

        [Fact]
        public void LogMgr_ReturnAllDailyLogsUsingLinqExpression()
        {
            _repository = new Repository(new HOSContext());
            IDailyLogManager logMgr = new DailyLogManager(_repository);

            var dailyLogs = logMgr.GetDailyLogs(log => log.DriverID == 4);
            Assert.NotEmpty(dailyLogs);
            Assert.True(dailyLogs.Count() > 0);
        }

        [Fact]
        public void LogMgr_ReturnAllDailyLogsUsingDriverID()            
        {
            _repository = new Repository(new HOSContext());
            IDailyLogManager logMgr = new DailyLogManager(_repository);

            int driverId = 4;
            var dailyLogs = logMgr.GetDailyLogs(driverId); 

            Assert.NotEmpty(dailyLogs);
            Assert.True(dailyLogs.Count() > 0);             
        }

        [Fact]
        public void LogMgr_ReturnDailyLogUsingLinqExpression()
        {
            _repository = new Repository(new HOSContext());
            IDailyLogManager logMgr = new DailyLogManager(_repository);

            var dailyLog = logMgr.GetDailyLog(log => log.LogDate == new DateTime(2016,9,7));            
            Assert.NotNull(dailyLog);
            Assert.Equal(new DateTime(2016,9,7), dailyLog.LogDate);
        }

        [Fact]
        public void LogMgr_ReturnDailyLogByLogDateAndDriverID()
        {
            _repository = new Repository(new HOSContext());
            IDailyLogManager logMgr = new DailyLogManager(_repository);

            var dailyLog = logMgr.GetDailyLog(new DateTime(2016,9,7), 4);            
            Assert.NotNull(dailyLog);
            Assert.Equal(899201, dailyLog.BeginningMileage);
        }                 

        [Fact]
        public void LogMgr_ReturnAllDailyLog()
        {
            _repository = new Repository(new HOSContext());
            IDailyLogManager logMgr = new DailyLogManager(_repository);

            var dailyLogs = logMgr.GetAll();
            Assert.NotEmpty(dailyLogs);
            Assert.True(dailyLogs.Count() > 0);            
        }

        // [Fact]
        // public void LogMgr_CreateDailyLog()
        // {
        //     _repository = new Repository(new HOSContext());
        //     IDailyLogManager logMgr = new DailyLogManager(_repository);

        //     var dailyLog = new DailyLogModel 
        //     {
        //         LogDate = new DateTime(2016,9,9),
        //         BeginningMileage = 900065,
        //         TruckNumber = "3082",
        //         TrailerNumber = "9159",
        //         IsSigned = false,
        //         DriverID = 4,
        //         CreatedBy = "admin",
        //         CreatedOn = new DateTime(2016,9,9),
        //         UpdatedBy = "admin",
        //         UpdatedOn = new DateTime(2016,9,9)
        //     };            

        //     logMgr.Create(dailyLog);
        //     logMgr.SaveChanges();

        //     var test = logMgr.GetDailyLog(log => log.LogDate == new DateTime(2016,9,9).Date);
        //     Assert.NotNull(test);
        //     Assert.Equal(new DateTime(2016,9,9), test.CreatedOn);            
        // } 

        // [Fact]
        // public void LogMgr_UpdateDailyLog()
        // {
        //     _repository = new Repository(new HOSContext());
        //     IDailyLogManager logMgr = new DailyLogManager(_repository);

        //     var test = logMgr.GetDailyLog(log => log.LogDate == new DateTime(2016,9,9).Date);
        //     Assert.NotNull(test);
        //     Assert.Equal(new DateTime(2016,9,9), test.LogDate);

        //     int logid = test.LogID;
        //     test.UpdatedOn = new DateTime(2016,9,10).Date;
        //     logMgr.Update(test);
        //     logMgr.SaveChanges();

        //     var dailyLog = logMgr.GetDailyLog(log => log.LogID == logid);
        //     Assert.Equal(new DateTime(2016,9,10).Date, dailyLog.UpdatedOn);
        // } 

        // [Fact]
        // public void LogMgr_DeleteDailyLog()
        // {
        //     _repository = new Repository(new HOSContext());
        //     IDailyLogManager logMgr = new DailyLogManager(_repository);

        //     var dailyLog = logMgr.GetDailyLog(log => log.LogDate == new DateTime(2016,9,9).Date);
        //     Assert.NotNull(dailyLog);

        //     logMgr.Delete(dailyLog);
        //     logMgr.SaveChanges();
        //     dailyLog = logMgr.GetDailyLog(log => log.LogDate == new DateTime(2016,9,9).Date);
        //     Assert.Null(dailyLog);            
        // }
        

        // [Fact]
        // public void LogMgr_CreateDailyLogDetail()
        // {
        //     _repository = new Repository(new HOSContext());
        //     IDailyLogManager logMgr = new DailyLogManager(_repository);

        //     var createdate = new DateTime(2016,9,9,22,15,0);
        //     var dailyLogDetail = new DailyLogDetailModel 
        //     {
        //         LogID = 2,
        //         DutyStatusID = 4,
        //         StartTime = new DateTime(2016,9,9,8,15,0),
        //         StopTime = new DateTime(2016,9,9,8,45,0),
        //         LocationCity = "Eads",
        //         StateProvinceId = 6,
        //         DutyStatusActivityID = 1,
        //         Notes = "Hello, World!",
        //         CreatedBy = "admin",
        //         CreatedOn = createdate,
        //         UpdatedBy = "admin",
        //         UpdatedOn = createdate
        //     };

        //     logMgr.CreateLogDetail(dailyLogDetail);
        //     logMgr.SaveChanges();

        //     var newDailyLogDetail = logMgr.GetDailyLogDetail(detail => detail.CreatedOn == createdate);
        //     Assert.NotNull(newDailyLogDetail);             
        // } 

        // [Fact]
        // public void LogMgr_UpdateDailyLogDetail()
        // {
        //     _repository = new Repository(new HOSContext());
        //     IDailyLogManager logMgr = new DailyLogManager(_repository);

        //     var detail = logMgr.GetDailyLogDetail(36);
        //     Assert.NotNull(detail);
        //     Assert.Equal(1, detail.DutyStatusActivityID);
        
        //     detail.DutyStatusActivityID = 7;
        //     logMgr.UpdateLogDetail(detail);
        //     logMgr.SaveChanges();

        //     detail = logMgr.GetDailyLogDetail(36);
        //     Assert.NotNull(detail);
        //     Assert.Equal(7, detail.DutyStatusActivityID);
        // }

        // [Fact]
        // public void LogMgr_DeleteDailyLogDetail()
        // {
        //     _repository = new Repository(new HOSContext());
        //     IDailyLogManager logMgr = new DailyLogManager(_repository);

        //     var detail = logMgr.GetDailyLogDetail(36);
        //     Assert.NotNull(detail);

        //     logMgr.DeleteLogDetail(detail);
        //     logMgr.SaveChanges();

        //     detail = logMgr.GetDailyLogDetail(36);
        //     Assert.Null(detail);
        // }                       
    }
}
