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
using BTCA.DataAccess.Initializers;

namespace BTCA.Tests.UnitTests
{
    public class AdminControllerTests : BaseTestClass
    {
        private readonly ILogger<AdminController> _logger;
        // private readonly ILogger<AdminController> _mockLogger;
        private Mock<ICompanyManager> _mockCompanyMgr; 

        public AdminControllerTests()
        {
            // _mockLogger = Mock.Of<ILogger<AdminController>>();
            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            _logger = factory.CreateLogger<AdminController>();                        
            _mockCompanyMgr = new Mock<ICompanyManager>();          
        }  

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void Index_Returns_ViewResult_GetAll()
        {

            _mockCompanyMgr.Setup(mgr => mgr.GetAll())
                           .Returns(GetCompanies().ToList());

            var controller = new AdminController(_mockCompanyMgr.Object, _logger);            
            
            IActionResult result = controller.Get();
            Assert.NotNull(result);
            var okResult = (OkObjectResult)result;

            var companies = (List<Company>)okResult.Value;
            Assert.NotNull(okResult);
            Assert.Equal(7, companies.Count());
        }  

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void Index_Returns_ViewResult_Company_GetById()
        {
            _mockCompanyMgr.Setup(mgr => mgr.GetCompany(It.IsAny<Func<Company, bool>>()))
                           .Returns((Expression<Func<Company, bool>> expression) => 
                            GetCompanies().Where(expression).SingleOrDefault()
                           );

            var controller = new AdminController(_mockCompanyMgr.Object, _logger); 

            var result = controller.GetById(1);
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result); 
            
            var okResult = (OkObjectResult)result;

            var company = (Company)okResult.Value;
            Assert.NotNull(company);
            Assert.Equal(1, company.ID);                     
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpPost_Company_Create()
        {
            _mockCompanyMgr.Setup(mgr => mgr.Create(It.IsAny<Company>()));

            var controller = new AdminController(_mockCompanyMgr.Object, _logger); 

            var result = controller.Create(GetOneCompany());

            _mockCompanyMgr.Verify(m => m.Create(It.IsAny<Company>()), Times.Once());
            _mockCompanyMgr.Verify(m => m.SaveChanges(), Times.Once());            
            Assert.IsType<CreatedAtRouteResult>(result);          
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpPost_Company_Create_WithNullCompany()
        {
            _mockCompanyMgr.Setup(mgr => mgr.Create(It.IsAny<Company>()));

            var controller = new AdminController(_mockCompanyMgr.Object, _logger); 

            var result = controller.Create(null);
            Assert.IsType<BadRequestObjectResult>(result);          
        }        

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpPost_Company_Create_WithInvalidModelState()
        {
            _mockCompanyMgr.Setup(mgr => mgr.Create(It.IsAny<Company>()));

            var controller = new AdminController(_mockCompanyMgr.Object, _logger);
            controller.ModelState.AddModelError("CompanyCode", "Required"); 
            controller.ModelState.AddModelError("CompanyName", "Required");

            var company = new Company {
                CompanyCode = null,
                CompanyName = null
            };
            var result = controller.Create(company);
            Assert.IsType<BadRequestObjectResult>(result);          
        } 

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpPut_Company_Update()
        {
            _mockCompanyMgr.Setup(mgr => mgr.GetCompany(It.IsAny<Func<Company, bool>>()))
                           .Returns((Expression<Func<Company, bool>> expression) => 
                            GetCompanies().Where(expression).SingleOrDefault()
                           );
            _mockCompanyMgr.Setup(mgr => mgr.Update(It.IsAny<Company>()));
            _mockCompanyMgr.Setup(mgr => mgr.SaveChanges());

            var controller = new AdminController(_mockCompanyMgr.Object, _logger); 
            var result = controller.Update(GetOneCompany().ID, GetOneCompany());

            _mockCompanyMgr.Verify(m => m.Update(It.IsAny<Company>()), Times.Once());
            _mockCompanyMgr.Verify(m => m.SaveChanges(), Times.Once()); 
            Assert.IsType<NoContentResult>(result);   // NotFoundObjectResult       
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpPut_Company_UpdateWithInvalidCompanyID()
        {
            _mockCompanyMgr.Setup(mgr => mgr.GetCompany(It.IsAny<Func<Company, bool>>()))
                           .Returns((Expression<Func<Company, bool>> expression) => 
                            GetCompanies().Where(expression).SingleOrDefault()
                           );
            _mockCompanyMgr.Setup(mgr => mgr.Update(It.IsAny<Company>()));
            _mockCompanyMgr.Setup(mgr => mgr.SaveChanges());

            var controller = new AdminController(_mockCompanyMgr.Object, _logger);

            var company = new Company {
                ID = 22,
                CompanyCode = null,
                CompanyName = null
            };

            var result = controller.Update(GetOneCompany().ID, company);

            Assert.IsType<BadRequestObjectResult>(result);   // NotFoundObjectResult       
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpPost_Company_UpdateWithInvalidModelState()
        {
            _mockCompanyMgr.Setup(mgr => mgr.GetCompany(It.IsAny<Func<Company, bool>>()))
                           .Returns((Expression<Func<Company, bool>> expression) => 
                            GetCompanies().Where(expression).SingleOrDefault()
                           );
            _mockCompanyMgr.Setup(mgr => mgr.Update(It.IsAny<Company>()));
            _mockCompanyMgr.Setup(mgr => mgr.SaveChanges());

            var controller = new AdminController(_mockCompanyMgr.Object, _logger);
            controller.ModelState.AddModelError("CompanyCode", "Required"); 

            var company = GetOneCompany();
            company.CompanyCode = null;

            var result = controller.Update(company.ID, company);

            Assert.IsType<BadRequestObjectResult>(result);   // NotFoundObjectResult       
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpPost_Company_UpdateWithNullCompany()
        {
            _mockCompanyMgr.Setup(mgr => mgr.GetCompany(It.IsAny<Func<Company, bool>>()))
                           .Returns((Expression<Func<Company, bool>> expression) => 
                            GetCompanies().Where(expression).SingleOrDefault()
                           );
            _mockCompanyMgr.Setup(mgr => mgr.Update(It.IsAny<Company>()));
            _mockCompanyMgr.Setup(mgr => mgr.SaveChanges());

            var controller = new AdminController(_mockCompanyMgr.Object, _logger);
            controller.ModelState.AddModelError("CompanyCode", "Required"); 

            var company = GetOneCompany();
            company.CompanyCode = null;

            var result = controller.Update(company.ID, null);

            Assert.IsType<BadRequestObjectResult>(result);       
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpDelete_Company_Delete()
        {
            _mockCompanyMgr.Setup(mgr => mgr.GetCompany(It.IsAny<Func<Company, bool>>()))
                           .Returns((Expression<Func<Company, bool>> expression) => 
                            GetCompanies().Where(expression).SingleOrDefault()
                           );
            _mockCompanyMgr.Setup(mgr => mgr.Delete(It.IsAny<Company>()));
            _mockCompanyMgr.Setup(mgr => mgr.SaveChanges());

            var controller = new AdminController(_mockCompanyMgr.Object, _logger);

            var company = GetOneCompany();

            var result = controller.Delete(company.ID);

            _mockCompanyMgr.Verify(m => m.Delete(It.IsAny<Company>()), Times.Once());
            _mockCompanyMgr.Verify(m => m.SaveChanges(), Times.Once()); 
            Assert.IsType<NoContentResult>(result);   // NotFoundObjectResult       
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpDelete_Company_Delete_InvalidCompanyID()
        {
            _mockCompanyMgr.Setup(mgr => mgr.GetCompany(It.IsAny<Func<Company, bool>>()))
                           .Returns((Expression<Func<Company, bool>> expression) => 
                            GetCompanies().Where(expression).SingleOrDefault()
                           );
            _mockCompanyMgr.Setup(mgr => mgr.Delete(It.IsAny<Company>()));
            _mockCompanyMgr.Setup(mgr => mgr.SaveChanges());

            var controller = new AdminController(_mockCompanyMgr.Object, _logger);

            var company = GetOneCompany();

            var result = controller.Delete(1111);

            _mockCompanyMgr.Verify(m => m.Delete(It.IsAny<Company>()), Times.Never());
            _mockCompanyMgr.Verify(m => m.SaveChanges(), Times.Never()); 
            Assert.IsType<NotFoundObjectResult>(result);   // NotFoundObjectResult       
        }

        private IQueryable<Company> GetCompanies()
        {
            return BTCASampleData.LoadCompanyTable().AsQueryable().OrderBy(co => co.CompanyName);
        }

        private Company GetOneCompany() =>
            new Company 
            {
                ID = 1, 
                CompanyCode = "TEST002", 
                CompanyName = "MOQ Testing", 
                DOT_Number = "000000", 
                MC_Number = "MC-000000", 
                CreatedBy = "admin", 
                CreatedOn = DateTime.Now, 
                UpdatedBy = "admin", 
                UpdatedOn = DateTime.Now
            };                               
    }
}
