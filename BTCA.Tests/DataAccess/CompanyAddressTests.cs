using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Xunit;
using BTCA.Common.Entities;
using BTCA.DataAccess.Core;
using BTCA.DataAccess.EF;

namespace BTCA.Tests.DataAccess
{
    public class CompanyAddressTests
    {
        [Fact]
        public void Test_CompanyAddressCreate()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try {

                var options = new DbContextOptionsBuilder<HOSContext>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new HOSContext(options))
                {
                    context.Database.EnsureCreated();
                }

                using (var context = new HOSContext(options))
                {
                    HOSTestData.LoadCompanyTable(context); 
                    HOSTestData.LoadStateProvinceCodeTable(context);
                    HOSTestData.CreateViews(context);                                    
                }

                using (var context = new HOSContext(options))
                {
                    IRepository repository = new Repository(context);
                    var company = repository.Find<Company>(c => c.CompanyCode == "FCT001");

                    Assert.NotNull(company); 
                    
                    company.Addresses.Add(
                        new Address() 
                        { 
                            AddressLine1 = "1346 Markum Ranch Rd",
                            AddressLine2 = "Ste 100",
                            City = "Fort Worth",
                            StateProvinceId = 45,
                            Zipcode = "76126",
                            IsHQ = true,
                            CompanyId = company.ID,
                            CreatedBy = "sysadmin"                    
                        } 
                    );

                    repository.Save();

                    Assert.Equal(1, company.Addresses.Count);                                      
                }

            } finally {
                connection.Close();
            }
        } 

        [Fact]
        public void Test_CompanyAddressUpdate()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try {

                var options = new DbContextOptionsBuilder<HOSContext>()
                    .UseSqlite(connection)
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
                    HOSTestData.CreateViews(context);                                     
                }

                using (var context = new HOSContext(options))
                {
                    
                    IRepository repository = new Repository(context);
                    var company = repository.Find<Company>(c => c.CompanyCode == "FCT001");
                    Assert.NotNull(company);

                    var address = repository.Find<Address>(a => 
                                a.CompanyId == company.ID && a.AddressLine1 == "1346 Markum Ranch Rd");

                    Assert.NotNull(address);
                                
                    address.AddressLine1 = "1346 Markum Ranch Road";
                    address.AddressLine2 = "Bldg One";
                    address.UpdatedBy = "sysadmin";
                    address.UpdatedOn = DateTime.Now;

                    repository.Update<Address>(address);
                    repository.Save();

                    var test = repository.Find<Address>(a => a.ID == address.ID);
                    Assert.NotNull(test);
                    Assert.Equal(address.AddressLine1, test.AddressLine1);
                    Assert.Equal(address.AddressLine2, test.AddressLine2);
                    Assert.Equal(address.UpdatedOn, test.UpdatedOn);
                                                                                                   
                }

            } finally {
                connection.Close();
            }
        }

        [Fact]
        public void Test_CompanyAddressDelete()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try {

                var options = new DbContextOptionsBuilder<HOSContext>()
                    .UseSqlite(connection)
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
                    HOSTestData.CreateViews(context);                                     
                }

                using (var context = new HOSContext(options))
                {
                    IRepository repository = new Repository(context);
                    var company = repository.Find<Company>(c => c.CompanyCode == "FCT001");
                    Assert.NotNull(company);

                    var address = repository.Find<Address>(a => 
                                a.CompanyId == company.ID && a.AddressLine1 == "1346 Markum Ranch Rd");

                    Assert.NotNull(address);

                    repository.Delete<Address>(a => a.ID == address.ID);
                    repository.Save();

                    var test = repository.Find<Address>(a => a.ID == address.ID);
                    Assert.Null(test);                                                       
                }

            } finally {
                connection.Close();
            }
        } 

        [Fact]
        public void Test_CompanyAddressesAll()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try {

                var options = new DbContextOptionsBuilder<HOSContext>()
                    .UseSqlite(connection)
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
                    HOSTestData.CreateViews(context);                                      
                }

                using (var context = new HOSContext(options))
                {
                    IRepository repository = new Repository(context);
                    var addresses = repository.All<Address>();
                    Assert.NotNull(addresses);
                    Assert.Equal(6, addresses.Count());                                                       
                }

            } finally {
                connection.Close();
            }
        }

        [Fact]
        public void Test_CompanyAddressesUsingView()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try {

                var options = new DbContextOptionsBuilder<HOSContext>()
                    .UseSqlite(connection)
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
                    HOSTestData.CreateViews(context);                                       
                }

                using (var context = new HOSContext(options))
                {
                    IRepository repository = new Repository(context);
                    var queryByCompanyId = repository.DBContext.CompanyAddresses.Where(ca => ca.CompanyId == 2).ToList();
                    Assert.NotEmpty(queryByCompanyId);
                    Assert.Equal(2, queryByCompanyId.Count());

                    var addresses = repository.DBContext.CompanyAddresses.ToList();
                    Assert.NotEmpty(addresses);
                    Assert.Equal(6, addresses.Count());                                                       
                }

            } finally {
                connection.Close();
            }
        }                                       
    }
}
