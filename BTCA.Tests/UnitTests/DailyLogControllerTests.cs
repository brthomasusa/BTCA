using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Moq;
using BTCA.Common.BusinessObjects;
using BTCA.DomainLayer.Managers.Interface;
using BTCA.WebApi.Controllers;

namespace BTCA.Tests.UnitTests
{
    public class DailyLogControllerTests : BaseTestClass
    {
        private readonly ILogger<DailyLogController> _logger;
        private Mock<IDailyLogManager> _mockDailyLogMgr; 

        public DailyLogControllerTests()
        {
            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            _logger = factory.CreateLogger<DailyLogController>();                        
            _mockDailyLogMgr = new Mock<IDailyLogManager>();             
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpGet_DailyLog_GetAll()
        {
            var drvID = 4;

            _mockDailyLogMgr.Setup(mgr => mgr.GetDailyLogs(It.Is<int>(i => i == drvID)))
                           .Returns(GetDailyLogModels().Where(m => m.DriverID == drvID).ToList());

            var controller = new DailyLogController(_mockDailyLogMgr.Object, _logger);

            var result = controller.GetAll(drvID);
            Assert.IsType<OkObjectResult>(result);
            _mockDailyLogMgr.Verify(m => m.GetDailyLogs(It.Is<int>(i => i == drvID)), Times.Once());

            var okResult = (OkObjectResult)result;
            Assert.NotNull(okResult);

            var dailyLogs = (List<DailyLogModel>)okResult.Value;            
            Assert.Equal(2, dailyLogs.Count());              
        }        

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpGet_DailyLog_GetById()
        {
            var drvID = 4;
            var logDate = new DateTime(2016,9,7).Date;
            
            _mockDailyLogMgr.Setup(mgr => mgr.GetDailyLog(It.IsAny<Expression<Func<DailyLogModel, bool>>>()))
                           .Returns(GetDailyLogModels().Where(m => m.DriverID == drvID && m.LogDate == logDate).SingleOrDefault());

            var controller = new DailyLogController(_mockDailyLogMgr.Object, _logger);            
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
