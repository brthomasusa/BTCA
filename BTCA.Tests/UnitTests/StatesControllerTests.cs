using System;
using System.Collections.Generic;
using System.Linq;
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
        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void Index_Returns_ViewResult_ListOfStateCodes()
        {
            var mockStateCodeMgr = new Mock<IStateProvinceCodeManager>();
            mockStateCodeMgr.Setup(mgr => mgr.GetAll()).Returns(GetTestStateProvinceCodes());

            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            var logger = factory.CreateLogger<StatesController>();

            var controller = new StatesController(mockStateCodeMgr.Object, logger);

            var result = controller.Get();

            var objectResult = Assert.IsType<OkObjectResult>(result);

            List<StateProvinceCode> stateCodes = objectResult.Value as List<StateProvinceCode>;

            Assert.Equal(4, stateCodes.Count());
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void Index_Returns_ViewResult_1_StateCode()
        {
            var mockStateCodeMgr = new Mock<IStateProvinceCodeManager>();
            mockStateCodeMgr.Setup(mgr => mgr.GetStateProvinceCode(code => code.ID == 1)).Returns(GetTestStateProvinceCode(1));

            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            var logger = factory.CreateLogger<StatesController>();

            var controller = new StatesController(mockStateCodeMgr.Object, logger); 

            var result = controller.GetById(1);           
        }

        private IEnumerable<StateProvinceCode> GetTestStateProvinceCodes()
        {
            var stateCodes = new List<StateProvinceCode>
            {
                new StateProvinceCode {ID = 1,StateCode = "AK", StateName = "Alaska",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 2,StateCode = "AL", StateName = "Alabama",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 3,StateCode = "AR", StateName = "Arkansas",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new StateProvinceCode {ID = 4,StateCode = "AZ", StateName = "Arizona",CountryCode = "USA", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now}
            };

            return stateCodes;
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
