using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;
using Moq;
using BTCA.Common.Entities;
using BTCA.Common.BusinessObjects;
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
        private Mock<ICompanyAddressManager> _mockCompAddMgr;

        public AdminControllerTests()
        {
            // _mockLogger = Mock.Of<ILogger<AdminController>>();
            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            _logger = factory.CreateLogger<AdminController>();                        
            _mockCompanyMgr = new Mock<ICompanyManager>();
            _mockCompAddMgr = new Mock<ICompanyAddressManager>();            
        }  

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void Index_Returns_ViewResult_GetAll()
        {
            var companyId = 0;
        
            _mockCompanyMgr.Setup(mgr => mgr.GetAll())
                           .Returns(GetCompanies().ToList());
            _mockCompAddMgr.Setup(mgr => mgr.GetCompanyAddresses(companyId))
                           .Returns(GetCompanyAddresses().Where(c => c.CompanyId == companyId).ToList());

            var controller = new AdminController(_mockCompanyMgr.Object, _mockCompAddMgr.Object, _logger);            
            
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
            var companyId = 0;
            _mockCompanyMgr.Setup(mgr => mgr.GetCompany(It.IsAny<Expression<Func<Company, bool>>>()))
                           .Returns((Expression<Func<Company, bool>> expression) => 
                            GetCompanies().Where(expression).SingleOrDefault()
                           );
            _mockCompAddMgr.Setup(mgr => mgr.GetCompanyAddresses(companyId))
                           .Returns(GetCompanyAddresses().Where(c => c.CompanyId == companyId).ToList());

            var controller = new AdminController(_mockCompanyMgr.Object, _mockCompAddMgr.Object, _logger); 

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
            var companyId = 0;
            _mockCompanyMgr.Setup(mgr => mgr.Create(It.IsAny<Company>()));
            _mockCompAddMgr.Setup(mgr => mgr.GetCompanyAddresses(companyId))
                           .Returns(GetCompanyAddresses().Where(c => c.CompanyId == companyId).ToList());

            var controller = new AdminController(_mockCompanyMgr.Object, _mockCompAddMgr.Object, _logger); 

            var result = controller.Create(GetOneCompany());

            _mockCompanyMgr.Verify(m => m.Create(It.IsAny<Company>()), Times.Once());
            _mockCompanyMgr.Verify(m => m.SaveChanges(), Times.Once());            
            Assert.IsType<CreatedAtRouteResult>(result);          
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpPost_Company_Create_WithNullCompany()
        {
            var companyId = 0;
            _mockCompanyMgr.Setup(mgr => mgr.Create(It.IsAny<Company>()));
            _mockCompAddMgr.Setup(mgr => mgr.GetCompanyAddresses(companyId))
                           .Returns(GetCompanyAddresses().Where(c => c.CompanyId == companyId).ToList());

            var controller = new AdminController(_mockCompanyMgr.Object, _mockCompAddMgr.Object, _logger); 

            var result = controller.Create(null);
            Assert.IsType<BadRequestObjectResult>(result);          
        }        

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpPost_Company_Create_WithInvalidModelState()
        {
            var companyId = 0;
            _mockCompanyMgr.Setup(mgr => mgr.Create(It.IsAny<Company>()));
            _mockCompAddMgr.Setup(mgr => mgr.GetCompanyAddresses(companyId))
                           .Returns(GetCompanyAddresses().Where(c => c.CompanyId == companyId).ToList());

            var controller = new AdminController(_mockCompanyMgr.Object, _mockCompAddMgr.Object, _logger);
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
        public void HttpPost_Company_Update()
        {
            var companyId = 1;
            _mockCompanyMgr.Setup(mgr => mgr.GetCompany(It.IsAny<Expression<Func<Company, bool>>>()))
                           .Returns((Expression<Func<Company, bool>> expression) => 
                            GetCompanies().Where(expression).SingleOrDefault()
                           );
            _mockCompanyMgr.Setup(mgr => mgr.Update(It.IsAny<Company>()));
            _mockCompanyMgr.Setup(mgr => mgr.SaveChanges());
            _mockCompAddMgr.Setup(mgr => mgr.GetCompanyAddresses(companyId))
                           .Returns(GetCompanyAddresses().Where(c => c.CompanyId == companyId).ToList());

            var controller = new AdminController(_mockCompanyMgr.Object, _mockCompAddMgr.Object, _logger); 
            var result = controller.Update(GetOneCompany().ID, GetOneCompany());

            _mockCompanyMgr.Verify(m => m.Update(It.IsAny<Company>()), Times.Once());
            _mockCompanyMgr.Verify(m => m.SaveChanges(), Times.Once()); 
            Assert.IsType<NoContentResult>(result);   // NotFoundObjectResult       
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpPost_Company_UpdateWithInvalidCompanyID()
        {
            var companyId = 1;
            _mockCompanyMgr.Setup(mgr => mgr.GetCompany(It.IsAny<Expression<Func<Company, bool>>>()))
                           .Returns((Expression<Func<Company, bool>> expression) => 
                            GetCompanies().Where(expression).SingleOrDefault()
                           );
            _mockCompanyMgr.Setup(mgr => mgr.Update(It.IsAny<Company>()));
            _mockCompanyMgr.Setup(mgr => mgr.SaveChanges());
            _mockCompAddMgr.Setup(mgr => mgr.GetCompanyAddresses(companyId))
                           .Returns(GetCompanyAddresses().Where(c => c.CompanyId == companyId).ToList());

            var controller = new AdminController(_mockCompanyMgr.Object, _mockCompAddMgr.Object, _logger);

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
            var companyId = 1;
            _mockCompanyMgr.Setup(mgr => mgr.GetCompany(It.IsAny<Expression<Func<Company, bool>>>()))
                           .Returns((Expression<Func<Company, bool>> expression) => 
                            GetCompanies().Where(expression).SingleOrDefault()
                           );
            _mockCompanyMgr.Setup(mgr => mgr.Update(It.IsAny<Company>()));
            _mockCompanyMgr.Setup(mgr => mgr.SaveChanges());
            _mockCompAddMgr.Setup(mgr => mgr.GetCompanyAddresses(companyId))
                           .Returns(GetCompanyAddresses().Where(c => c.CompanyId == companyId).ToList());

            var controller = new AdminController(_mockCompanyMgr.Object, _mockCompAddMgr.Object, _logger);
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
            var companyId = 1;
            _mockCompanyMgr.Setup(mgr => mgr.GetCompany(It.IsAny<Expression<Func<Company, bool>>>()))
                           .Returns((Expression<Func<Company, bool>> expression) => 
                            GetCompanies().Where(expression).SingleOrDefault()
                           );
            _mockCompanyMgr.Setup(mgr => mgr.Update(It.IsAny<Company>()));
            _mockCompanyMgr.Setup(mgr => mgr.SaveChanges());
            _mockCompAddMgr.Setup(mgr => mgr.GetCompanyAddresses(companyId))
                           .Returns(GetCompanyAddresses().Where(c => c.CompanyId == companyId).ToList());

            var controller = new AdminController(_mockCompanyMgr.Object, _mockCompAddMgr.Object, _logger);
            controller.ModelState.AddModelError("CompanyCode", "Required"); 

            var company = GetOneCompany();
            company.CompanyCode = null;

            var result = controller.Update(company.ID, null);

            Assert.IsType<BadRequestObjectResult>(result);   // NotFoundObjectResult       
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpPost_Company_Delete()
        {
            var companyId = 1;
            _mockCompanyMgr.Setup(mgr => mgr.GetCompany(It.IsAny<Expression<Func<Company, bool>>>()))
                           .Returns((Expression<Func<Company, bool>> expression) => 
                            GetCompanies().Where(expression).SingleOrDefault()
                           );
            _mockCompanyMgr.Setup(mgr => mgr.Delete(It.IsAny<Company>()));
            _mockCompanyMgr.Setup(mgr => mgr.SaveChanges());
            _mockCompAddMgr.Setup(mgr => mgr.GetCompanyAddresses(companyId))
                           .Returns(GetCompanyAddresses().Where(c => c.CompanyId == companyId).ToList());

            var controller = new AdminController(_mockCompanyMgr.Object, _mockCompAddMgr.Object, _logger);

            var company = GetOneCompany();

            var result = controller.Delete(company.ID);

            _mockCompanyMgr.Verify(m => m.Delete(It.IsAny<Company>()), Times.Once());
            _mockCompanyMgr.Verify(m => m.SaveChanges(), Times.Once()); 
            Assert.IsType<NoContentResult>(result);   // NotFoundObjectResult       
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpPost_CompanyAddress_GetAddressesForCompany()
        {
            var companyId = 2;

            _mockCompanyMgr.Setup(mgr => mgr.GetCompany(It.IsAny<Expression<Func<Company, bool>>>()))
                           .Returns((Expression<Func<Company, bool>> expression) => 
                            GetCompanies().Where(expression).SingleOrDefault()
                           );
            _mockCompAddMgr.Setup(mgr => mgr.GetCompanyAddresses(It.IsAny<int>()))
                           .Returns(GetCompanyAddresses().Where(c => c.CompanyId == companyId).ToList());

            var controller = new AdminController(_mockCompanyMgr.Object, _mockCompAddMgr.Object, _logger);

            var result = controller.GetAddressesForCompany(companyId);
            Assert.IsType<OkObjectResult>(result);
            _mockCompAddMgr.Verify(m => m.GetCompanyAddresses(It.IsAny<int>()), Times.Once());

            var okResult = (OkObjectResult)result;
            Assert.NotNull(okResult);

            var companyAddresses = (List<CompanyAddress>)okResult.Value;            
            Assert.Equal(2, companyAddresses.Count());              
        }

        // [Fact]
        // [Trait("Category", "UnitTest.WebApiControllers")]
        // public void HttpPost_CompanyAddress_GetCompanyAddress()
        // {
        //     var companyId = 2;

        //     _mockCompanyMgr.Setup(mgr => mgr.GetCompany(It.IsAny<Expression<Func<Company, bool>>>()))
        //                    .Returns((Expression<Func<Company, bool>> expression) => 
        //                     GetCompanies().Where(expression).SingleOrDefault()
        //                    );
        //     _mockCompAddMgr.Setup(mgr => mgr.GetCompanyAddress(It.IsAny<int>()))
        //                    .Returns(GetCompanyAddresses().Where(c => c.CompanyId == companyId).ToList());

        //     var controller = new AdminController(_mockCompanyMgr.Object, _mockCompAddMgr.Object, _logger);

        //     var result = controller.GetAddressesForCompany(companyId);
        //     Assert.IsType<OkObjectResult>(result);
        //     _mockCompAddMgr.Verify(m => m.GetCompanyAddresses(It.IsAny<int>()), Times.Once());

        //     var okResult = (OkObjectResult)result;
        //     Assert.NotNull(okResult);

        //     var companyAddresses = (List<CompanyAddress>)okResult.Value;            
        //     Assert.Equal(2, companyAddresses.Count());              
        // }

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

        private CompanyAddress GetOneCompanyAddress() 
            =>  new CompanyAddress() 
                { 
                    ID = 1,
                    AddressLine1 = "1346 Markum Ranch Rd",
                    AddressLine2 = "Ste 100",
                    City = "Fort Worth",
                    StateProvinceId = 45,
                    StateCode = "TX",
                    Zipcode = "76126",
                    CountryCode = "USA",
                    IsHQ = true,
                    CompanyId = 2,
                    CreatedBy = "admin", 
                    CreatedOn = DateTime.Now, 
                    UpdatedBy = "admin", 
                    UpdatedOn = DateTime.Now                    
                };

        private IQueryable<CompanyAddress> GetCompanyAddresses()
        {
            var data = new List<CompanyAddress>()
            {
                new CompanyAddress() 
                { 
                    ID = 1,
                    AddressLine1 = "1346 Markum Ranch Rd",
                    AddressLine2 = "Ste 100",
                    City = "Fort Worth",
                    StateProvinceId = 45,
                    StateCode = "TX",
                    Zipcode = "76126",
                    CountryCode = "USA",
                    IsHQ = true,
                    CompanyId = 2,
                    CreatedBy = "admin", 
                    CreatedOn = DateTime.Now, 
                    UpdatedBy = "admin", 
                    UpdatedOn = DateTime.Now                    
                },
                
                new CompanyAddress() 
                { 
                    ID = 2,
                    AddressLine1 = "6591 Brighton Blvd",
                    City = "Commerce City",
                    StateProvinceId = 6,
                    StateCode = "CO",
                    Zipcode = "80022",
                    CountryCode = "USA",
                    IsHQ = false,
                    CompanyId = 2,
                    CreatedBy = "admin", 
                    CreatedOn = DateTime.Now, 
                    UpdatedBy = "admin", 
                    UpdatedOn = DateTime.Now                                         
                },   
                new CompanyAddress() 
                { 
                    ID = 3,
                    AddressLine1 = "12404 Park Central D",
                    AddressLine2 = "Ste 300",
                    City = "Dallas",
                    StateProvinceId = 45,
                    StateCode = "TX",
                    Zipcode = "75251",
                    CountryCode = "USA",
                    IsHQ = true,
                    CompanyId = 7,
                    CreatedBy = "admin", 
                    CreatedOn = DateTime.Now, 
                    UpdatedBy = "admin", 
                    UpdatedOn = DateTime.Now                                         
                },
                new CompanyAddress() 
                { 
                    ID = 4,
                    AddressLine1 = "2150 Cabot Boulevard West",
                    City = "Langhorne",
                    StateProvinceId = 39,
                    StateCode = "PA",
                    Zipcode = "19047",
                    CountryCode = "USA",
                    IsHQ = true,
                    CompanyId = 5,
                    CreatedBy = "admin", 
                    CreatedOn = DateTime.Now, 
                    UpdatedBy = "admin", 
                    UpdatedOn = DateTime.Now                                         
                }, 
                new CompanyAddress() 
                { 
                    ID = 5,
                    AddressLine1 = "3250 N Longhorn Dr",
                    City = "Lancaster",
                    StateProvinceId = 45,
                    StateCode = "TX",
                    Zipcode = "75134",
                    CountryCode = "USA",
                    IsHQ = false,
                    CompanyId = 3,
                    CreatedBy = "admin", 
                    CreatedOn = DateTime.Now, 
                    UpdatedBy = "admin", 
                    UpdatedOn = DateTime.Now                                         
                },
                new CompanyAddress() 
                { 
                    ID = 6,
                    AddressLine1 = "22 South 75th Street",
                    City = "Phoenix",
                    StateProvinceId = 4,
                    StateCode = "AZ",
                    Zipcode = "85043",
                    CountryCode = "USA",
                    IsHQ = true,
                    CompanyId = 3,
                    CreatedBy = "admin", 
                    CreatedOn = DateTime.Now, 
                    UpdatedBy = "admin", 
                    UpdatedOn = DateTime.Now                                         
                }, 
                                                          
            };

            return data.AsQueryable();
        }                              
    }
}
