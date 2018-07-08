using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Moq;
using BTCA.Common.Entities;
using BTCA.Common.BusinessObjects;
using BTCA.DomainLayer.Managers.Implementation;
using BTCA.DataAccess.Core;
using BTCA.DataAccess.EF;
using BTCA.DataAccess.Initializers;

namespace BTCA.Tests.UnitTests
{
    public class CompanyAddressManagerTests
    {
        [Fact]
        [Trait("Category", "UnitTest.CompanyAddressManager")]
        public void CompanyAddressManager_GetAll()
        { 
            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(repo => repo.AllQueryType<CompanyAddress>())
                                        .Returns(GetCompanyAddresses());

            var addressMgr = new CompanyAddressManager(mockRepo.Object);
            var results = addressMgr.GetAll();

            mockRepo.Verify(repo => repo.AllQueryType<CompanyAddress>(), Times.Once());
            Assert.Equal(6, results.Count());
            Assert.Equal("22 South 75th Street", ((CompanyAddress)results.First()).AddressLine1); 
        }

        [Fact]
        [Trait("Category", "UnitTest.CompanyAddressManager")]
        public void CompanyAddressManager_GetCompanyAddress_ByExpression()
        {
            var data = GetCompanyAddresses();

            var mockRepo = new Mock<IRepository>();

            /*
                This does not work, Moq can not mock extension methods. So, AllQueryType<CompanyAddress>().Where causes
                the following error: System.NotSupportedException : Invalid setup on an extension method
            */

            // mockRepo.Setup(repo => repo.AllQueryType<CompanyAddress>().Where(It.IsAny<Expression<Func<CompanyAddress, bool>>>()))                                        
            //                             .Callback(
            //                                 (Expression<Func<CompanyAddress, bool>> expression) => 
            //                                     {
            //                                         data = data.Where(expression);
            //                                     }
            //                             )
            //                             .Returns(data);

            // var companyAddressMgr = new CompanyAddressManager(mockRepo.Object);
            // var result = companyAddressMgr.GetCompanyAddress(ca => ((CompanyAddress)ca).AddressLine1 == "1346 Markum Ranch Rd"); 

            // mockRepo.Verify(repo => repo.AllQueryType<CompanyAddress>().Where(It.IsAny<Expression<Func<CompanyAddress, bool>>>()), Times.Once());
            // Assert.Equal("1346 Markum Ranch Rd", result.AddressLine1); 
        }

        [Fact]
        [Trait("Category", "UnitTest.CompanyAddressManager")]
        public void CompanyAddressManager_CreateCompanyAddress()
        {
            int calls = 0;
            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(repo => repo.Create<Address>(It.IsAny<Address>())).Callback(() => calls++);
            var addressMgr = new CompanyAddressManager(mockRepo.Object);
            addressMgr.Create(GetOneCompanyAddress());

            mockRepo.Verify(repo => repo.Create<Address>(It.IsAny<Address>()), Times.Once());
        }

        [Fact]
        [Trait("Category", "UnitTest.CompanyAddressManager")]
        public void CompanyAddressManager_UpdateCompanyAddress()
        {
            int calls = 0;
            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(repo => repo.Update<Address>(It.IsAny<Address>())).Callback(() => calls++);
            var addressMgr = new CompanyAddressManager(mockRepo.Object);
            addressMgr.Update(GetOneCompanyAddress());

            mockRepo.Verify(repo => repo.Update<Address>(It.IsAny<Address>()), Times.Once());
        }        

        [Fact]
        [Trait("Category", "UnitTest.CompanyAddressManager")]
        public void CompanyAddressManager_DeleteCompanyAddress()
        {
            int calls = 0;
            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(repo => repo.Delete<Address>(It.IsAny<Address>())).Callback(() => calls++);
            var addressMgr = new CompanyAddressManager(mockRepo.Object);
            addressMgr.Delete(GetOneCompanyAddress());

            mockRepo.Verify(repo => repo.Delete<Address>(It.IsAny<Address>()), Times.Once());
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
