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
    public class DailyLogDetailModelControllerTests
    {
        private readonly ILogger<DailyLogController> _logger;
        private Mock<IDailyLogManager> _mockDailyLogMgr;         

        public DailyLogDetailModelControllerTests()
        {
            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            _logger = factory.CreateLogger<DailyLogController>(); 

            _mockDailyLogMgr = new Mock<IDailyLogManager>();            
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpGet_DailyLogDetailModels_GetByLogId()
        {
            var logID = 1;
            _mockDailyLogMgr.Setup(mgr => mgr.GetDailyLogDetails(It.Is<int>(i => i == logID)))
                                             .Returns(GetDailyLogDetailModels().Where(d => d.LogID == logID).ToList());            

            var controller = new DailyLogDetailController(_mockDailyLogMgr.Object, _logger);

            var result = controller.GetDailyLogDetails(logID);

            _mockDailyLogMgr.Verify(mgr => mgr.GetDailyLogDetails(It.Is<int>(i => i == logID)), Times.Once());
            Assert.IsType<OkObjectResult>(result);

            var okResult = (OkObjectResult)result;
            var logDetails = (List<DailyLogDetailModel>)okResult.Value;
            Assert.Equal(8, logDetails.Count());
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpGet_DailyDetailLog_GetByLogDetailId()
        {
            var logDetailID = 1;

            _mockDailyLogMgr.Setup(mgr => mgr.GetDailyLogDetail(It.Is<int>(i => i == logDetailID)))
                                             .Returns(GetOneDailyLogDetailModel());

            var controller = new DailyLogDetailController(_mockDailyLogMgr.Object, _logger);

            var result = controller.GetDailyLogDetail(logDetailID);

            _mockDailyLogMgr.Verify(mgr => mgr.GetDailyLogDetail(It.Is<int>(i => i == logDetailID)), Times.Once());
            Assert.IsType<OkObjectResult>(result);

            var okResult = (OkObjectResult)result;

            var test = (DailyLogDetailModel)okResult.Value;
            Assert.NotNull(test);
            Assert.Equal(logDetailID, test.LogDetailID);              
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpGet_DailyDetailLog_GetByLogDetailId_NotFound()        
        {
            var logDetailID = 1;

            _mockDailyLogMgr.Setup(mgr => mgr.GetDailyLogDetail(It.Is<int>(i => i == logDetailID)));

            var controller = new DailyLogDetailController(_mockDailyLogMgr.Object, _logger);

            var result = controller.GetDailyLogDetail(logDetailID);

            _mockDailyLogMgr.Verify(mgr => mgr.GetDailyLogDetail(It.Is<int>(i => i == logDetailID)), Times.Once());
            Assert.IsType<NotFoundObjectResult>(result);            
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpPost_DailyLogDetailModel_Create()
        {
            var logDetail = GetOneDailyLogDetailModel();
            _mockDailyLogMgr.Setup(mgr => mgr.CreateLogDetail(It.Is<DailyLogDetailModel>(i => i == logDetail)));

            var controller = new DailyLogDetailController(_mockDailyLogMgr.Object, _logger); 

            var result = controller.Create(logDetail);

            _mockDailyLogMgr.Verify(m => m.CreateLogDetail(It.Is<DailyLogDetailModel>(i => i == logDetail)), Times.Once());
            _mockDailyLogMgr.Verify(m => m.SaveChanges(), Times.Once());            
            Assert.IsType<CreatedAtRouteResult>(result);             
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpPost_DailyLogDetailModel_Create_NullDailyLogDetail()
        {
            DailyLogDetailModel dailyLogDetail = null;
            _mockDailyLogMgr.Setup(mgr => mgr.CreateLogDetail(It.Is<DailyLogDetailModel>(i => i == dailyLogDetail)));

            var controller = new DailyLogDetailController(_mockDailyLogMgr.Object, _logger); 

            var result = controller.Create(dailyLogDetail);

            _mockDailyLogMgr.Verify(m => m.Create(It.Is<DailyLogDetailModel>(i => i == dailyLogDetail)), Times.Never());
            _mockDailyLogMgr.Verify(m => m.SaveChanges(), Times.Never());            
            Assert.IsType<BadRequestObjectResult>(result); 
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpPost_DailyLogDetailModel_Create_InvalidModelState()
        {
            var dailyLogDetail = GetOneDailyLogDetailModel();
            dailyLogDetail.LocationCity = String.Empty;

            _mockDailyLogMgr.Setup(mgr => mgr.CreateLogDetail(It.Is<DailyLogDetailModel>(i => i == dailyLogDetail)));

            var controller = new DailyLogDetailController(_mockDailyLogMgr.Object, _logger); 
            controller.ModelState.AddModelError("LocationCity", "The location of the duty status change is required");
            var result = controller.Create(dailyLogDetail);

            _mockDailyLogMgr.Verify(m => m.Create(It.Is<DailyLogDetailModel>(i => i == dailyLogDetail)), Times.Never());
            _mockDailyLogMgr.Verify(m => m.SaveChanges(), Times.Never());            
            Assert.IsType<BadRequestObjectResult>(result);            
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpPut_DailyLogDetailModel_Update()
        {
            var dailyLogDetail = GetOneDailyLogDetailModel();
            _mockDailyLogMgr.Setup(mgr => mgr.UpdateLogDetail(It.Is<DailyLogDetailModel>(i => i == dailyLogDetail)));
            _mockDailyLogMgr.Setup(mgr => mgr.SaveChanges());

            var controller = new DailyLogDetailController(_mockDailyLogMgr.Object, _logger);
            var result = controller.Update(dailyLogDetail.LogDetailID, dailyLogDetail);

            _mockDailyLogMgr.Verify(m => m.UpdateLogDetail(It.Is<DailyLogDetailModel>(i => i == dailyLogDetail)), Times.Once());
            _mockDailyLogMgr.Verify(m => m.SaveChanges(), Times.Once()); 
            Assert.IsType<NoContentResult>(result); 
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpPut_DailyLogDetailModel_Update_MisMatchedLogDetailID()
        {
            var dailyLogDetail = GetOneDailyLogDetailModel();
            _mockDailyLogMgr.Setup(mgr => mgr.UpdateLogDetail(It.Is<DailyLogDetailModel>(i => i == dailyLogDetail)));
            _mockDailyLogMgr.Setup(mgr => mgr.SaveChanges());

            var controller = new DailyLogDetailController(_mockDailyLogMgr.Object, _logger);
            var result = controller.Update(-1, dailyLogDetail);

            _mockDailyLogMgr.Verify(m => m.UpdateLogDetail(It.Is<DailyLogDetailModel>(i => i == dailyLogDetail)), Times.Never());
            _mockDailyLogMgr.Verify(m => m.SaveChanges(), Times.Never()); 
            Assert.IsType<BadRequestObjectResult>(result);             
        } 

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpPut_DailyLogModel_Update_InvalidModelState()
        {
            var dailyLogDetail = GetOneDailyLogDetailModel();
            dailyLogDetail.LocationCity = String.Empty;

            _mockDailyLogMgr.Setup(mgr => mgr.UpdateLogDetail(It.Is<DailyLogDetailModel>(i => i == dailyLogDetail)));
            _mockDailyLogMgr.Setup(mgr => mgr.SaveChanges());

            var controller = new DailyLogDetailController(_mockDailyLogMgr.Object, _logger);
            controller.ModelState.AddModelError("LocationCity", "The location of the duty status change is required");
            var result = controller.Update(dailyLogDetail.LogDetailID, dailyLogDetail);

            _mockDailyLogMgr.Verify(m => m.UpdateLogDetail(It.Is<DailyLogDetailModel>(i => i == dailyLogDetail)), Times.Never());
            _mockDailyLogMgr.Verify(m => m.SaveChanges(), Times.Never()); 
            Assert.IsType<BadRequestObjectResult>(result);              
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpDelete_DailyLogDetailModel_Delete()
        {
            var dailyLogDetail = GetOneDailyLogDetailModel();

            _mockDailyLogMgr.Setup(mgr => mgr.GetDailyLogDetail(It.Is<int>(i => i == dailyLogDetail.LogDetailID)))
                                             .Returns(dailyLogDetail);
            _mockDailyLogMgr.Setup(mgr => mgr.DeleteLogDetail(It.Is<DailyLogDetailModel>(i => i == dailyLogDetail)));
            _mockDailyLogMgr.Setup(mgr => mgr.SaveChanges());

            var controller = new DailyLogDetailController(_mockDailyLogMgr.Object, _logger);

            var result = controller.Delete(dailyLogDetail.LogDetailID);

            _mockDailyLogMgr.Verify(m => m.GetDailyLogDetail(It.Is<int>(i => i == dailyLogDetail.LogDetailID)), Times.Once());
            _mockDailyLogMgr.Verify(m => m.DeleteLogDetail(It.Is<DailyLogDetailModel>(i => i == dailyLogDetail)), Times.Once());
            _mockDailyLogMgr.Verify(m => m.SaveChanges(), Times.Once()); 
            Assert.IsType<NoContentResult>(result);            
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpDelete_DailyLogDetailModel_Delete_NotFound()
        {
            var dailyLogDetail = GetOneDailyLogDetailModel();
            dailyLogDetail.LogDetailID = -1;

            _mockDailyLogMgr.Setup(mgr => mgr.GetDailyLogDetail(It.Is<int>(i => i == dailyLogDetail.LogDetailID)));
            _mockDailyLogMgr.Setup(mgr => mgr.DeleteLogDetail(It.Is<DailyLogDetailModel>(i => i == dailyLogDetail)));
            _mockDailyLogMgr.Setup(mgr => mgr.SaveChanges());

            var controller = new DailyLogDetailController(_mockDailyLogMgr.Object, _logger);

            var result = controller.Delete(dailyLogDetail.LogDetailID);

            _mockDailyLogMgr.Verify(m => m.GetDailyLogDetail(It.Is<int>(i => i == dailyLogDetail.LogDetailID)), Times.Once());
            _mockDailyLogMgr.Verify(m => m.DeleteLogDetail(It.Is<DailyLogDetailModel>(i => i == dailyLogDetail)), Times.Never());
            _mockDailyLogMgr.Verify(m => m.SaveChanges(), Times.Never()); 
            Assert.IsType<NotFoundObjectResult>(result);
        }

        private DailyLogDetailModel GetOneDailyLogDetailModel() =>        
            new DailyLogDetailModel 
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

        
        private IQueryable<DailyLogDetailModel> GetDailyLogDetailModels()
        {
            var data = new List<DailyLogDetailModel> 
            {
                new DailyLogDetailModel 
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
                new DailyLogDetailModel 
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
                new DailyLogDetailModel 
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
                new DailyLogDetailModel 
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
                new DailyLogDetailModel 
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
                new DailyLogDetailModel 
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
                new DailyLogDetailModel 
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
                new DailyLogDetailModel 
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
                new DailyLogDetailModel 
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
                new DailyLogDetailModel 
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
                new DailyLogDetailModel 
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
                new DailyLogDetailModel 
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
                new DailyLogDetailModel 
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
                new DailyLogDetailModel 
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
                new DailyLogDetailModel 
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
                new DailyLogDetailModel 
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
                new DailyLogDetailModel 
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
                new DailyLogDetailModel 
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
                new DailyLogDetailModel 
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
                new DailyLogDetailModel 
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
                new DailyLogDetailModel 
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
                new DailyLogDetailModel 
                { 
                    LogDetailID = 22, 
                    DutyStatusID = 1, 
                    StartTime = new DateTime(2016,9,8, 21,30,0),
                    StopTime = new DateTime(2016,9,8, 23,59,0),
                    LocationCity = "Aurora",
                    StateProvinceId = 6,
                    DutyStatusActivityID = 9,
                    LogID = 2
                },

            };

            return data.AsQueryable();
        }
    }
}
