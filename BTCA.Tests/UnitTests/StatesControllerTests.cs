using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Moq;
using BTCA.Common.Entities;
using BTCA.DomainLayer.Managers.Interface;
using BTCA.WebApi.Controllers;

namespace BTCA.Tests.UnitTests
{
    public class StatesControllerTests : BaseTestClass
    {
        private readonly ILogger<StatesController> _logger;
        private Mock<IStateProvinceCodeManager> _mockStateCodeMgr;
        public StatesControllerTests()
        {
            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            _logger = factory.CreateLogger<StatesController>();
            _mockStateCodeMgr = new Mock<IStateProvinceCodeManager>();
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void Index_Returns_ViewResult_GetAll()
        {
            _mockStateCodeMgr.Setup(mgr => mgr.GetAll()).Returns(GetTestStateProvinceCodes().ToList());
            var controller = new StatesController(_mockStateCodeMgr.Object, _logger);            
            
            IActionResult result = controller.Get();
            Assert.NotNull(result);
            var okResult = (OkObjectResult)result;

            var stateCodes = (List<StateProvinceCode>)okResult.Value;
            Assert.NotNull(okResult);
            Assert.Equal(4, stateCodes.Count());
        }        

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void Index_Returns_ViewResult_StateCode_GetById()
        {
            _mockStateCodeMgr.Setup(mgr => mgr.GetStateProvinceCode(It.IsAny<Expression<Func<StateProvinceCode, bool>>>()))
                             .Returns( () =>
                                  GetTestStateProvinceCodes().Where(code => code.ID == 1)
                                                             .SingleOrDefault()
                             );

            var controller = new StatesController(_mockStateCodeMgr.Object, _logger); 

            var result = controller.GetById(1);
            Assert.IsType<OkObjectResult>(result);           
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void Index_Returns_ViewResult_StateCode_GetById_NotFound()       
        {
            _mockStateCodeMgr.Setup(mgr => mgr.GetStateProvinceCode(code => code.ID == -1)).Returns(GetTestStateProvinceCode(-1));
            var controller = new StatesController(_mockStateCodeMgr.Object, _logger); 

            var result = controller.GetById(1);
            Assert.IsType<NotFoundObjectResult>(result);            
        }

        private IQueryable<StateProvinceCode> GetTestStateProvinceCodes()
        {
            var stateCodes = new List<StateProvinceCode>
            {
                new StateProvinceCode {ID = 1,StateCode = "AK", StateName = "Alaska",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 2,StateCode = "AL", StateName = "Alabama",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 3,StateCode = "AR", StateName = "Arkansas",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 4,StateCode = "AZ", StateName = "Arizona",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now}
            };

            return stateCodes.AsQueryable();
        }

        private StateProvinceCode GetTestStateProvinceCode(int id) 
        {
            var stateCodes = new List<StateProvinceCode>
            {
                new StateProvinceCode {ID = 1,StateCode = "AK", StateName = "Alaska",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 2,StateCode = "AL", StateName = "Alabama",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 3,StateCode = "AR", StateName = "Arkansas",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 4,StateCode = "AZ", StateName = "Arizona",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now}
            };

            return stateCodes.Where(code => code.ID == id).SingleOrDefault();            
        } 

    }
}
