using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Xunit;
using BTCA.Common.BusinessObjects;
using BTCA.DataAccess.Core;
using BTCA.DataAccess.EF;
using BTCA.DomainLayer.Managers.Interface;
using BTCA.DomainLayer.Managers.Implementation;

namespace BTCA.Tests.IntegrationTests
{
    public class CompanyAddressManagerTests
    {
        [Fact]
        public void Test_CompanyAddressMgr_Create()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try {

                var options = new DbContextOptionsBuilder<HOSContext>()
                    .UseSqlite(connection)
                    .EnableSensitiveDataLogging()
                    .Options;

                using (var context = new HOSContext(options))
                {
                    context.Database.EnsureCreated();
                }

                using (var context = new HOSContext(options))
                {
                    HOSTestData.LoadCompanyTable(context);
                    HOSTestData.LoadStateProvinceCodeTable(context);                                     
                }

                using (var context = new HOSContext(options))
                {
                    ICompanyAddressManager addressMgr = new CompanyAddressManager(new Repository(context));

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

            } finally {
                connection.Close();
            } 
        }

        [Fact]
        public void Test_CompanyAddressMgr_Update()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try {

                var options = new DbContextOptionsBuilder<HOSContext>()
                    .UseSqlite(connection)
                    .EnableSensitiveDataLogging()
                    .Options;

                using (var context = new HOSContext(options))
                {
                    context.Database.EnsureCreated();
                }

                using (var context = new HOSContext(options))
                {
                    HOSTestData.LoadCompanyTable(context);
                    HOSTestData.LoadStateProvinceCodeTable(context);
                    HOSTestData.LoadCompanyAddresses(context);                                     
                }

                using (var context = new HOSContext(options))
                {
                    // context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                    ICompanyAddressManager companyAddressMgr = new CompanyAddressManager(new Repository(context));

                    var companyAddress = companyAddressMgr.GetCompanyAddress(5, 4);
                    Assert.NotNull(companyAddress);

                    DateTime currentTimeStamp = DateTime.Now;
                    companyAddress.UpdatedOn = currentTimeStamp;
                    companyAddressMgr.Update(companyAddress);
                    companyAddressMgr.SaveChanges();

                    companyAddress =companyAddressMgr.GetCompanyAddress(a => a.CompanyId == 5 && a.ID == 4);
                    Assert.NotNull(companyAddress);
                    Assert.Equal(currentTimeStamp, companyAddress.UpdatedOn);                    
                }

            } finally {
                connection.Close();
            }
        }   

        [Fact]
        public void Test_CompanyAddressMgr_Delete()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try {

                var options = new DbContextOptionsBuilder<HOSContext>()
                    .UseSqlite(connection)
                    .EnableSensitiveDataLogging()
                    .Options;

                using (var context = new HOSContext(options))
                {
                    context.Database.EnsureCreated();
                }

                using (var context = new HOSContext(options))
                {
                    HOSTestData.LoadCompanyTable(context);
                    HOSTestData.LoadStateProvinceCodeTable(context);
                    HOSTestData.LoadCompanyAddresses(context);                                     
                }

                using (var context = new HOSContext(options))
                {
                    ICompanyAddressManager addressMgr = new CompanyAddressManager(new Repository(context));

                    var address = addressMgr.GetCompanyAddress(a => a.CompanyId == 5 && a.AddressLine1 == "2150 Cabot Boulevard West");
                    Assert.NotNull(address);

                    addressMgr.Delete(address);
                    addressMgr.SaveChanges();

                    address = addressMgr.GetCompanyAddress(a => a.CompanyId == 5 && a.AddressLine1 == "2150 Cabot Boulevard West");
                    Assert.Null(address);                    
                }

            } finally {
                connection.Close();
            }
        } 

        [Fact]
        public void Test_CompanyAddressMgr_SelectAll()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try {

                var options = new DbContextOptionsBuilder<HOSContext>()
                    .UseSqlite(connection)
                    .EnableSensitiveDataLogging()
                    .Options;

                using (var context = new HOSContext(options))
                {
                    context.Database.EnsureCreated();
                }

                using (var context = new HOSContext(options))
                {
                    HOSTestData.LoadCompanyTable(context);
                    HOSTestData.LoadStateProvinceCodeTable(context);
                    HOSTestData.LoadCompanyAddresses(context);                                     
                }

                using (var context = new HOSContext(options))
                {
                    ICompanyAddressManager addressMgr = new CompanyAddressManager(new Repository(context));

                    var addresses = addressMgr.GetCompanyAddresses(2).ToList();                   
                    Assert.NotNull(addresses);
                    Assert.Equal(2, addresses.Count());
                }

            } finally {
                connection.Close();
            }
        }                                 
    }
}
