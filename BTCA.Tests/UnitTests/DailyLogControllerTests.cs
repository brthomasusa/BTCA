using System;
using System.Collections.Generic;
using System.Linq;
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
        public void HttpGet_DailyLog_GetAllByCompany()
        {
            var companyID = 2;

            _mockDailyLogMgr.Setup(mgr => mgr.GetDailyLogsForCompany(It.Is<int>(i => i == companyID) ))                                
                            .Returns(GetDailyLogModels().ToList());

            var controller = new DailyLogController(_mockDailyLogMgr.Object, _logger);
            IActionResult result = controller.GetAllByCompany(companyID);

            _mockDailyLogMgr.Verify( m => m.GetDailyLogsForCompany(It.Is<int>(i => i == companyID)),Times.Once());

            Assert.NotNull(result);
            var okResult = (OkObjectResult)result;
            Assert.IsType<OkObjectResult>(okResult);

            var logs = (List<DailyLogModel>)okResult.Value;
            Assert.Equal(2, logs.Count());          
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpGet_DailyLog_GetAllByDriver()
        {
            var drvID = 4;

            _mockDailyLogMgr.Setup(mgr => mgr.GetDailyLogsForDriver(It.Is<int>(i => i == drvID)))
                           .Returns(GetDailyLogModels().Where(m => m.DriverID == drvID).ToList());

            var controller = new DailyLogController(_mockDailyLogMgr.Object, _logger);
            var result = controller.GetAllByDriver(drvID);

            Assert.IsType<OkObjectResult>(result);
            _mockDailyLogMgr.Verify(m => m.GetDailyLogsForDriver(It.Is<int>(i => i == drvID)), Times.Once());

            var okResult = (OkObjectResult)result;
            Assert.NotNull(okResult);

            var dailyLogs = (List<DailyLogModel>)okResult.Value;            
            Assert.Equal(2, dailyLogs.Count());              
        }        

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpGet_DailyLog_GetForDriverAndDate()
        {
            var drvID = 4;
            var logDate = new DateTime(2016,9,7).Date;

            // Just playing around ...
            Func<DailyLogModel,bool> testDelegate = m => m.DriverID == drvID && m.LogDate == logDate; 
 
            _mockDailyLogMgr.Setup(mgr => mgr.GetDailyLog(
                                It.Is<DateTime>(i => i == logDate), 
                                It.Is<int>(i => i == drvID) ))
                           .Returns(GetDailyLogModels().Where(testDelegate).SingleOrDefault());

            var controller = new DailyLogController(_mockDailyLogMgr.Object, _logger);
            var result = controller.GetForDriverAndDate(drvID, logDate);

            Assert.IsType<OkObjectResult>(result);
            _mockDailyLogMgr.Verify( m => m.GetDailyLog(
                                                It.Is<DateTime>(i => i == logDate), 
                                                It.Is<int>(i => i == drvID)), 
                                            Times.Once()); 

            var okResult = (OkObjectResult)result;

            var testLog = (DailyLogModel)okResult.Value;
            Assert.NotNull(testLog);
            Assert.Equal(drvID, testLog.DriverID);                                                  
            Assert.Equal(logDate, testLog.LogDate);
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpGet_DailyLog_GetForDriverAndDate_NotFound()
        {
            var drvID = 4987;
            var logDate = new DateTime(2016,9,7).Date;
  
            _mockDailyLogMgr.Setup(mgr => mgr.GetDailyLog(It.Is<DateTime>(i => i == logDate), It.Is<int>(i => i == drvID) ));
                                                                 
            var controller = new DailyLogController(_mockDailyLogMgr.Object, _logger);
            var result = controller.GetForDriverAndDate(drvID, logDate);

            _mockDailyLogMgr.Verify( m => m.GetDailyLog(
                                                It.Is<DateTime>(i => i == logDate), 
                                                It.Is<int>(i => i == drvID)), 
                                            Times.Once()); 

            Assert.IsType<NotFoundObjectResult>(result); 
        }        

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpGet_DailyLog_GetForDateRange()        
        {
            var drvID = 4;
            var beginDate = new DateTime(2016,9,7).Date;            
            var endDate = new DateTime(2016,9,8).Date;

            _mockDailyLogMgr.Setup(mgr => mgr.GetDailyLogs(It.IsAny<Func<DailyLogModel, bool>>()))
                                             .Returns(GetDailyLogModels().ToList());  

            var controller = new DailyLogController(_mockDailyLogMgr.Object, _logger);
            var result = controller.GetForDateRange(drvID, beginDate, endDate);

            _mockDailyLogMgr.Verify(mgr => mgr.GetDailyLogs(It.IsAny<Func<DailyLogModel, bool>>()), Times.Once());                                             
            Assert.IsType<OkObjectResult>(result);

            var okResult = (OkObjectResult)result;
            var logs = (List<DailyLogModel>)okResult.Value;

            Assert.Equal(2, logs.Count());            
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpGet_DailyLog_GetForDateRange_NotFound()
        {
            var drvID = 4;
            var beginDate = new DateTime(2016,9,7).Date;            
            var endDate = new DateTime(2016,9,8).Date;

            _mockDailyLogMgr.Setup(mgr => mgr.GetDailyLogs(It.IsAny<Func<DailyLogModel, bool>>()));  

            var controller = new DailyLogController(_mockDailyLogMgr.Object, _logger);
            var result = controller.GetForDateRange(drvID, beginDate, endDate);

            _mockDailyLogMgr.Verify(mgr => mgr.GetDailyLogs(It.IsAny<Func<DailyLogModel, bool>>()), Times.Once());                                             
            Assert.IsType<NotFoundObjectResult>(result);            
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpGet_DailyLog_GetByLogId()
        {
            var logID = 1;
            _mockDailyLogMgr.Setup(mgr => mgr.GetDailyLog(It.Is<int>(i => i == logID))).Returns(GetOneDailyLogModel());            
            var controller = new DailyLogController(_mockDailyLogMgr.Object, _logger);

            var result = controller.GetByLogId(logID);

            _mockDailyLogMgr.Verify(mgr => mgr.GetDailyLog(It.Is<int>(i => i == logID)), Times.Once());
            Assert.IsType<OkObjectResult>(result);

            var okResult = (OkObjectResult)result;

            var test = (DailyLogModel)okResult.Value;
            Assert.NotNull(test);
            Assert.Equal(logID, test.LogID);            
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpGet_DailyLog_GetByLogId_NotFound()
        {
            var logID = 1;
            _mockDailyLogMgr.Setup(mgr => mgr.GetDailyLog(It.Is<int>(i => i == logID)));            
            var controller = new DailyLogController(_mockDailyLogMgr.Object, _logger);

            var result = controller.GetByLogId(logID);

            _mockDailyLogMgr.Verify(mgr => mgr.GetDailyLog(It.Is<int>(i => i == logID)), Times.Once());
            Assert.IsType<NotFoundObjectResult>(result);             
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpPost_DailyLogModel_Create()
        {
            var dailyLog = GetOneDailyLogModel();
            _mockDailyLogMgr.Setup(mgr => mgr.Create(It.Is<DailyLogModel>(i => i == dailyLog)));

            var controller = new DailyLogController(_mockDailyLogMgr.Object, _logger); 

            var result = controller.Create(dailyLog);

            _mockDailyLogMgr.Verify(m => m.Create(It.Is<DailyLogModel>(i => i == dailyLog)), Times.Once());
            _mockDailyLogMgr.Verify(m => m.SaveChanges(), Times.Once());            
            Assert.IsType<CreatedAtRouteResult>(result);             
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpPost_DailyLogModel_Create_NullDailyLog()
        {
            DailyLogModel dailyLog = null;
            _mockDailyLogMgr.Setup(mgr => mgr.Create(It.Is<DailyLogModel>(i => i == dailyLog)));

            var controller = new DailyLogController(_mockDailyLogMgr.Object, _logger); 

            var result = controller.Create(dailyLog);

            _mockDailyLogMgr.Verify(m => m.Create(It.Is<DailyLogModel>(i => i == dailyLog)), Times.Never());
            _mockDailyLogMgr.Verify(m => m.SaveChanges(), Times.Never());            
            Assert.IsType<BadRequestObjectResult>(result); 
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpPost_DailyLogModel_Create_InvalidModelState()
        {
            var dailyLog = GetOneDailyLogModel();
            dailyLog.DriverID = 0;
            _mockDailyLogMgr.Setup(mgr => mgr.Create(It.Is<DailyLogModel>(i => i == dailyLog)));

            var controller = new DailyLogController(_mockDailyLogMgr.Object, _logger); 
            controller.ModelState.AddModelError("DriverID", "Can not be zero");
            var result = controller.Create(dailyLog);

            _mockDailyLogMgr.Verify(m => m.Create(It.Is<DailyLogModel>(i => i == dailyLog)), Times.Never());
            _mockDailyLogMgr.Verify(m => m.SaveChanges(), Times.Never());            
            Assert.IsType<BadRequestObjectResult>(result);             
        }       

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpPut_DailyLogModel_Update()
        {
            var dailyLog = GetOneDailyLogModel();
            _mockDailyLogMgr.Setup(mgr => mgr.Update(It.Is<DailyLogModel>(i => i == dailyLog)));
            _mockDailyLogMgr.Setup(mgr => mgr.SaveChanges());

            var controller = new DailyLogController(_mockDailyLogMgr.Object, _logger); 
            var result = controller.Update(dailyLog.LogID, dailyLog);

            _mockDailyLogMgr.Verify(m => m.Update(It.Is<DailyLogModel>(i => i == dailyLog)), Times.Once());
            _mockDailyLogMgr.Verify(m => m.SaveChanges(), Times.Once()); 
            Assert.IsType<NoContentResult>(result); 
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpPut_DailyLogModel_Update_MisMatchedLogID()
        {
            var dailyLog = GetOneDailyLogModel();
            _mockDailyLogMgr.Setup(mgr => mgr.Update(It.Is<DailyLogModel>(i => i == dailyLog)));
            _mockDailyLogMgr.Setup(mgr => mgr.SaveChanges());

            var controller = new DailyLogController(_mockDailyLogMgr.Object, _logger); 
            var result = controller.Update(15, dailyLog);

            _mockDailyLogMgr.Verify(m => m.Update(It.Is<DailyLogModel>(i => i == dailyLog)), Times.Never());
            _mockDailyLogMgr.Verify(m => m.SaveChanges(), Times.Never()); 
            Assert.IsType<BadRequestObjectResult>(result);             
        }        

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpPut_DailyLogModel_Update_InvalidModelState()
        {
            var dailyLog = GetOneDailyLogModel();
            dailyLog.DriverID = 0;

            _mockDailyLogMgr.Setup(mgr => mgr.Update(It.Is<DailyLogModel>(i => i == dailyLog)));
            _mockDailyLogMgr.Setup(mgr => mgr.SaveChanges());

            var controller = new DailyLogController(_mockDailyLogMgr.Object, _logger);
            controller.ModelState.AddModelError("DriverID", "Can not be zero"); 
            var result = controller.Update(dailyLog.LogID, dailyLog);

            _mockDailyLogMgr.Verify(m => m.Update(It.Is<DailyLogModel>(i => i == dailyLog)), Times.Never());
            _mockDailyLogMgr.Verify(m => m.SaveChanges(), Times.Never()); 
            Assert.IsType<BadRequestObjectResult>(result);             
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpDelete_DailyLogModel_Delete()
        {
            var dailyLog = GetOneDailyLogModel();

            _mockDailyLogMgr.Setup(mgr => mgr.GetDailyLog(It.Is<int>(i => i == dailyLog.LogID))).Returns(dailyLog);
            _mockDailyLogMgr.Setup(mgr => mgr.Delete(It.Is<DailyLogModel>(i => i == dailyLog)));
            _mockDailyLogMgr.Setup(mgr => mgr.SaveChanges());

            var controller = new DailyLogController(_mockDailyLogMgr.Object, _logger);

            var result = controller.Delete(dailyLog.LogID);

            _mockDailyLogMgr.Verify(m => m.Delete(It.Is<DailyLogModel>(i => i == dailyLog)), Times.Once());
            _mockDailyLogMgr.Verify(m => m.SaveChanges(), Times.Once()); 
            Assert.IsType<NoContentResult>(result);            
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpDelete_DailyLogModel_Delete_NotFound()        
        {
            var dailyLog = GetOneDailyLogModel();
            dailyLog.LogID = -1;
            _mockDailyLogMgr.Setup(mgr => mgr.GetDailyLog(It.Is<int>(i => i == dailyLog.LogID)));
            _mockDailyLogMgr.Setup(mgr => mgr.Delete(It.Is<DailyLogModel>(i => i == dailyLog)));
            _mockDailyLogMgr.Setup(mgr => mgr.SaveChanges());

            var controller = new DailyLogController(_mockDailyLogMgr.Object, _logger);

            var result = controller.Delete(dailyLog.LogID);

            _mockDailyLogMgr.Verify(m => m.Delete(It.Is<DailyLogModel>(i => i == dailyLog)), Times.Never());
            _mockDailyLogMgr.Verify(m => m.SaveChanges(), Times.Never()); 
            Assert.IsType<NotFoundObjectResult>(result);               
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
