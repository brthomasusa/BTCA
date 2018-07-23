using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Moq;
using BTCA.Common.BusinessObjects;
using BTCA.DomainLayer.Managers.Implementation;
using BTCA.DataAccess.Core;


namespace BTCA.Tests.UnitTests
{
    public class DailyLogManagerTests : BaseTestClass
    {
        [Fact]
        [Trait("Category", "UnitTest.DailyLogManager")]
        public void DailyLogManager_GetAll()
        {
            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(repo => repo.AllQueryType<DailyLogModel>())
                                        .Returns(GetDailyLogModels());

            var logMgr = new DailyLogManager(mockRepo.Object);
            var results = logMgr.GetAll();

            mockRepo.Verify(repo => repo.AllQueryType<DailyLogModel>(), Times.Once());
            Assert.Equal(2, results.Count());            
        } 

        [Fact]
        [Trait("Category", "UnitTest.DailyLogManager")]
        public void DailyLogManager_GetDailyLogs_ByExpression()
        {
            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(repo => repo.FilterQuery<DailyLogModel>(It.IsAny<Func<DailyLogModel, bool>>()))
                                        .Returns(GetDailyLogModels().Where(dl => dl.IsSigned == true).ToList());


            // mockRepo.Setup(repo => repo.FilterQuery<DailyLogModel>(It.Is<Expression<Func<DailyLogModel, bool>>>(dl => dl.IsSigned == true)))
            //                             .Returns(GetDailyLogModels().Where(dl => dl.IsSigned == true).ToList());

            var logMgr = new DailyLogManager(mockRepo.Object);
            var dailyLogs = logMgr.GetDailyLogs(dl => dl.IsSigned == true); 

            mockRepo.Verify(repo => repo.FilterQuery<DailyLogModel>(It.IsAny<Func<DailyLogModel, bool>>()), Times.Once());
            Assert.Equal(2, dailyLogs.Count());
        }

        [Fact]
        [Trait("Category", "UnitTest.DailyLogManager")]
        public void DailyLogManager_GetDailyLog_ByExpression()
        {
            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(repo => repo.FindQuery<DailyLogModel>(It.IsAny<Func<DailyLogModel, bool>>()))
                                        .Returns(GetOneDailyLogModel());

            var logMgr = new DailyLogManager(mockRepo.Object);
            var dailyLog = logMgr.GetDailyLog(dl => dl.IsSigned == true); 

            mockRepo.Verify(repo => repo.FindQuery<DailyLogModel>(It.IsAny<Func<DailyLogModel, bool>>()), Times.Once());
            Assert.Equal(1, dailyLog.LogID);            
        }

        private DailyLogModel GetOneDailyLogModel() =>        
            new DailyLogModel 
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
        
        private IQueryable<DailyLogModel> GetDailyLogModels()
        {
            var data = new List<DailyLogModel> 
            {
                new DailyLogModel 
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
                new DailyLogModel 
                {
                    LogID = 2,
                    LogDate = new DateTime(2016,9,8),
                    BeginningMileage = 899423,
                    EndingMileage = 900065,
                    TruckNumber = "3082",
                    TrailerNumber = "9159",
                    IsSigned = true,
                    DriverID = 4
                }                                
            };
           
            return data.AsQueryable();            
        }               
    }
}
