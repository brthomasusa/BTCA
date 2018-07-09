using System;
using System.Linq;
using Xunit;
using BTCA.Common.BusinessObjects;
using BTCA.DataAccess.Core;
using BTCA.DataAccess.EF;
using BTCA.DomainLayer.Managers.Interface;
using BTCA.DomainLayer.Managers.Implementation;

namespace BTCA.Tests.IntegrationTests
{
    public class CompanyAddressManagerFixture : IDisposable
    {
        private IRepository _repository;
        public CompanyAddressManagerFixture()
        {
            _repository = new Repository(new HOSContext());
            BTCA.Tests.SeedDatabase.CleanDatabase();
        }
        public void Dispose()
        {   
            BTCA.Tests.SeedDatabase.CleanDatabase();
            _repository.DBContext.Dispose();
        }
    }

    [CollectionDefinition("CompanyAddressManager collection", DisableParallelization = true)]
    public class CompanyAddressManagerTests : IClassFixture<CompanyAddressManagerFixture>
    {
        private IRepository _repository;
        private CompanyAddressManagerFixture _fixture;

        public CompanyAddressManagerTests(CompanyAddressManagerFixture fixture)
        {
            _repository = new Repository(new HOSContext());
            _fixture = fixture;           
        }
                
        [Fact]
        [Trait("Category", "IntegrationCompanyAddressMgr")]
        public void Test_CompanyAddressMgr_Create()
        {
            ICompanyAddressManager addressMgr = new CompanyAddressManager(_repository);

            var companyAddress = new CompanyAddress
            {
                AddressLine1 = "5333 Davidson Highway",
                AddressLine2 = "",
                City = "Concord",
                StateProvinceId = 28,
                Zipcode = "28027",
                CountryCode = "USA",
                IsHQ = true,
                CreatedBy = "admin",
                CreatedOn = DateTime.Now,
                UpdatedBy = "admin",
                UpdatedOn = DateTime.Now,
                CompanyId = 6
            };

            addressMgr.Create(companyAddress);
            addressMgr.SaveChanges();

            var test = addressMgr.GetCompanyAddress(a => a.CompanyId == 6 && a.AddressLine1 == "5333 Davidson Highway");
            Assert.Equal(companyAddress.CreatedOn, test.CreatedOn); 
        }

        [Fact]
        [Trait("Category", "IntegrationCompanyAddressMgr")]
        public void Test_CompanyAddressMgr_Update()
        {
            ICompanyAddressManager addressMgr = new CompanyAddressManager(_repository);
            var companyAddress = addressMgr.GetCompanyAddress(2);
            Assert.NotNull(companyAddress);

            DateTime currentTimeStamp = DateTime.Now;
            companyAddress.UpdatedOn = currentTimeStamp;
            addressMgr.Update(companyAddress);
            addressMgr.SaveChanges();

            companyAddress = addressMgr.GetCompanyAddress(2);
            Assert.NotNull(companyAddress);
            Assert.Equal(currentTimeStamp, companyAddress.UpdatedOn); 
        }        

        [Fact]
        [Trait("Category", "IntegrationCompanyAddressMgr")]
        public void Test_CompanyAddressMgr_Delete()
        {
            ICompanyAddressManager addressMgr = new CompanyAddressManager(_repository);
            var address = addressMgr.GetCompanyAddress(a => a.CompanyId == 5 && a.AddressLine1 == "2150 Cabot Boulevard West");
            Assert.NotNull(address);

            addressMgr.Delete(address);
            addressMgr.SaveChanges();

            address = addressMgr.GetCompanyAddress(a => a.CompanyId == 5 && a.AddressLine1 == "2150 Cabot Boulevard West");
            Assert.Null(address);                        
        }

        [Fact]
        [Trait("Category", "IntegrationCompanyAddressMgr")]
        public void Test_CompanyAddressMgr_GetAddresses_ByCompanyId()
        {
            ICompanyAddressManager addressMgr = new CompanyAddressManager(_repository);

            // In this app, getting all company addresses (or all of anything) is not usefull,
            // getting all for a company or a driver is useful.

            var addresses = addressMgr.GetCompanyAddresses(2).ToList();                   
            Assert.NotNull(addresses);
            Assert.Equal(2, addresses.Count());
        }

        [Fact]
        [Trait("Category", "IntegrationCompanyAddressMgr")]
        public void Test_CompanyAddressMgr_GetAddress_ByAddressId()
        {
            ICompanyAddressManager addressMgr = new CompanyAddressManager(_repository);

            var address = addressMgr.GetCompanyAddress(1);                   
            Assert.NotNull(address);
            Assert.Equal("1346 Markum Ranch Rd", address.AddressLine1);
        }

        [Fact]
        [Trait("Category", "IntegrationCompanyAddressMgr")]
        public void Test_CompanyAddressMgr_GetAddress_ByLinqExpression()                                                 
        {
            ICompanyAddressManager addressMgr = new CompanyAddressManager(_repository);

            var address = addressMgr.GetCompanyAddress(a => a.AddressLine1 == "1346 Markum Ranch Rd");                   
            Assert.NotNull(address);            
        }
    }
}
