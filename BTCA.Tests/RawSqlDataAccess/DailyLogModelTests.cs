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
            Assert.Equal(2, dailyLogs.Count());
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
            var queryByRawSql2View = _repository.DBContext.DailyLogModels.FromSql(sql).ToList();

            Assert.NotEmpty(queryByRawSql2View);
            Assert.Equal(2, queryByRawSql2View.Count());           
        } 

        [Fact]
        public void Repo_ReturnAllDailyLogsFromViewFiltered()
        {
            _repository = new Repository(new HOSContext());

            var dailyLogs = _repository.DBContext.DailyLogModels.Where(log => log.DriverID == 4).ToList();
            Assert.NotEmpty(dailyLogs);
            Assert.Equal(2, dailyLogs.Count());
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
        public void LogMgr_ReturnAllDailyLogsFromView()
        {
            _repository = new Repository(new HOSContext());
            IDailyLogManager logMgr = new DailyLogManager(_repository);

            // var dailyLogs = logMgr.GetDailyLogs();
            // Assert.NotEmpty(dailyLogs);
            // Assert.Equal(2, dailyLogs.Count());
        }            
    }
}
