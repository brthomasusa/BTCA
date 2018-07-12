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
    public class CompanyAddressControllerTests : BaseTestClass
    {
        private readonly ILogger<AdminController> _logger;
        private Mock<ICompanyAddressManager> _mockCompAddMgr; 

        public CompanyAddressControllerTests()
        {
            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            _logger = factory.CreateLogger<AdminController>();                        
            _mockCompAddMgr = new Mock<ICompanyAddressManager>();              
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpGet_CompanyAddress_GetAll()
        {
            var companyId = 2;

            _mockCompAddMgr.Setup(mgr => mgr.GetCompanyAddresses(It.Is<int>(i => i == companyId)))
                           .Returns(GetCompanyAddresses().Where(c => c.CompanyId == companyId).ToList());

            var controller = new CompanyAddressController(_mockCompAddMgr.Object, _logger);

            var result = controller.GetAll(companyId);
            Assert.IsType<OkObjectResult>(result);
            _mockCompAddMgr.Verify(m => m.GetCompanyAddresses(It.Is<int>(i => i == companyId)), Times.Once());

            var okResult = (OkObjectResult)result;
            Assert.NotNull(okResult);

            var companyAddresses = (List<CompanyAddress>)okResult.Value;            
            Assert.Equal(2, companyAddresses.Count());              
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpGet_GetCompanyAddress_GetById()
        {
            var addressId = GetOneCompanyAddress().ID;

            _mockCompAddMgr.Setup(mgr => mgr.GetCompanyAddress(It.Is<int>(i => i == addressId)))
                           .Returns(GetOneCompanyAddress());

            var controller = new CompanyAddressController(_mockCompAddMgr.Object, _logger);

            var result = controller.GetById(addressId);
            Assert.IsType<OkObjectResult>(result);
            _mockCompAddMgr.Verify(m => m.GetCompanyAddress(It.Is<int>(i => i == addressId)), Times.Once());

            var okResult = (OkObjectResult)result;
            Assert.NotNull(okResult);
            Assert.Equal(((CompanyAddress)okResult.Value).ID, addressId);              
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpGet_GetCompanyAddress_GetById_WithInvalidId()
        {
            var addressId = 220;

            _mockCompAddMgr.Setup(mgr => mgr.GetCompanyAddress(It.Is<int>(i => i == addressId)))
                           .Returns(GetCompanyAddresses().Where(ca => ca.ID == addressId).SingleOrDefault());

            var controller = new CompanyAddressController(_mockCompAddMgr.Object, _logger);

            var result = controller.GetById(addressId);
            Assert.IsType<NotFoundObjectResult>(result);
            _mockCompAddMgr.Verify(m => m.GetCompanyAddress(It.Is<int>(i => i == addressId)), Times.Once());           
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpPost_CreateCompanyAddress()
        {
            var company = new CompanyAddress() 
            { 
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

            _mockCompAddMgr.Setup(mgr => mgr.Create(It.IsAny<CompanyAddress>()));
            var controller = new CompanyAddressController(_mockCompAddMgr.Object, _logger); 

            var result = controller.Create(GetOneCompanyAddress());
            _mockCompAddMgr.Verify(m => m.Create(It.IsAny<CompanyAddress>()), Times.Once());
            _mockCompAddMgr.Verify(m => m.SaveChanges(), Times.Once());            
            Assert.IsType<CreatedAtRouteResult>(result);            
        }        

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpPost_CreateCompanyAddress_InvalidModelState()
        {
            var company = new CompanyAddress() 
            { 
                AddressLine1 = null,
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

            _mockCompAddMgr.Setup(mgr => mgr.Create(It.IsAny<CompanyAddress>()));
            var controller = new CompanyAddressController(_mockCompAddMgr.Object, _logger); 
            controller.ModelState.AddModelError("AddressLine1", "Required"); 

            var result = controller.Create(company);
            
            Assert.IsType<BadRequestObjectResult>(result); 
            _mockCompAddMgr.Verify(m => m.Create(It.IsAny<CompanyAddress>()), Times.Never());
            _mockCompAddMgr.Verify(m => m.SaveChanges(), Times.Never());                   
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpPost_CreateCompanyAddress_NullCompanyAddress()
        {
            _mockCompAddMgr.Setup(mgr => mgr.Create(It.IsAny<CompanyAddress>()));
            var controller = new CompanyAddressController(_mockCompAddMgr.Object, _logger); 

            var result = controller.Create(null);
            Assert.IsType<BadRequestObjectResult>(result);  
            _mockCompAddMgr.Verify(m => m.Create(It.IsAny<CompanyAddress>()), Times.Never());
            _mockCompAddMgr.Verify(m => m.SaveChanges(), Times.Never());                               
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpPut_UpdateCompanyAddress()
        {
            _mockCompAddMgr.Setup(mgr => mgr.Update(It.IsAny<CompanyAddress>()));
            _mockCompAddMgr.Setup(mgr => mgr.SaveChanges());

            var controller = new CompanyAddressController(_mockCompAddMgr.Object, _logger); 
            var result = controller.Update(GetOneCompanyAddress().ID, GetOneCompanyAddress());

            _mockCompAddMgr.Verify(m => m.Update(It.IsAny<CompanyAddress>()), Times.Once());
            _mockCompAddMgr.Verify(m => m.SaveChanges(), Times.Once()); 
            Assert.IsType<NoContentResult>(result);             
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpPut_UpdateCompanyAddress_WithNullCompanyAddress()
        {
            _mockCompAddMgr.Setup(mgr => mgr.Update(It.IsAny<CompanyAddress>()));
            _mockCompAddMgr.Setup(mgr => mgr.SaveChanges());

            var controller = new CompanyAddressController(_mockCompAddMgr.Object, _logger); 
            var result = controller.Update(GetOneCompanyAddress().ID, null);

            _mockCompAddMgr.Verify(m => m.Update(It.IsAny<CompanyAddress>()), Times.Never());
            _mockCompAddMgr.Verify(m => m.SaveChanges(), Times.Never()); 
            Assert.IsType<BadRequestObjectResult>(result); 
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpPut_UpdateCompanyAddress_WithInvalidAddressID()
        {
            _mockCompAddMgr.Setup(mgr => mgr.Update(It.IsAny<CompanyAddress>()));
            _mockCompAddMgr.Setup(mgr => mgr.SaveChanges());

            var controller = new CompanyAddressController(_mockCompAddMgr.Object, _logger); 
            var result = controller.Update(1000, GetOneCompanyAddress());

            _mockCompAddMgr.Verify(m => m.Update(It.IsAny<CompanyAddress>()), Times.Never());
            _mockCompAddMgr.Verify(m => m.SaveChanges(), Times.Never()); 
            Assert.IsType<BadRequestObjectResult>(result); 
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpPut_UpdateCompanyAddress_WithInvalidModelState()
        {
            _mockCompAddMgr.Setup(mgr => mgr.Update(It.IsAny<CompanyAddress>()));
            _mockCompAddMgr.Setup(mgr => mgr.SaveChanges());

            var badCompanyAddress = GetOneCompanyAddress();
            //badCompanyAddress.AddressLine1 = null;
            badCompanyAddress.CompanyId = 0;
            var controller = new CompanyAddressController(_mockCompAddMgr.Object, _logger);
            controller.ModelState.AddModelError("AddressLine1", "Required"); 

            var result = controller.Update(GetOneCompanyAddress().ID, badCompanyAddress);

            _mockCompAddMgr.Verify(m => m.Update(It.IsAny<CompanyAddress>()), Times.Never());
            _mockCompAddMgr.Verify(m => m.SaveChanges(), Times.Never()); 
            Assert.IsType<BadRequestObjectResult>(result);            
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpDelete_CompanyAddress()
        {
            var company = GetOneCompanyAddress();
            _mockCompAddMgr.Setup(mgr => mgr.GetCompanyAddress(It.Is<int>(i => i == GetOneCompanyAddress().ID)))
                           .Returns(GetOneCompanyAddress());            
            _mockCompAddMgr.Setup(mgr => mgr.Delete(It.IsAny<CompanyAddress>()));
            _mockCompAddMgr.Setup(mgr => mgr.SaveChanges());

            var controller = new CompanyAddressController(_mockCompAddMgr.Object, _logger);

            var result = controller.Delete(company.ID);

            _mockCompAddMgr.Verify(m => m.GetCompanyAddress(It.Is<int>(i => i == GetOneCompanyAddress().ID)), Times.Once());
            _mockCompAddMgr.Verify(m => m.Delete(It.IsAny<CompanyAddress>()), Times.Once());
            _mockCompAddMgr.Verify(m => m.SaveChanges(), Times.Once()); 
            Assert.IsType<NoContentResult>(result);       
        }

        [Fact]
        [Trait("Category", "UnitTest.WebApiControllers")]
        public void HttpDelete_CompanyAddress_InvalidAddressID()
        {
            _mockCompAddMgr.Setup(mgr => mgr.GetCompanyAddress(It.Is<int>(i => i == GetOneCompanyAddress().ID)))
                           .Returns(GetOneCompanyAddress());            
            _mockCompAddMgr.Setup(mgr => mgr.Delete(It.IsAny<CompanyAddress>()));
            _mockCompAddMgr.Setup(mgr => mgr.SaveChanges());

            var controller = new CompanyAddressController(_mockCompAddMgr.Object, _logger);

            var result = controller.Delete(1000);

            _mockCompAddMgr.Verify(m => m.GetCompanyAddress(It.Is<int>(i => i == GetOneCompanyAddress().ID)), Times.Never());
            _mockCompAddMgr.Verify(m => m.Delete(It.IsAny<CompanyAddress>()), Times.Never());
            _mockCompAddMgr.Verify(m => m.SaveChanges(), Times.Never()); 
            Assert.IsType<NotFoundObjectResult>(result);             
        }

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
