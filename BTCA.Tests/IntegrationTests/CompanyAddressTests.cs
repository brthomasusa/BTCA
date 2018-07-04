using System;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using BTCA.DataAccess.Core;
using BTCA.DataAccess.EF;
using BTCA.DomainLayer.Managers.Interface;
using BTCA.DomainLayer.Managers.Implementation;

namespace BTCA.Tests.IntegrationTests
{
    public class CompanyAddressTests
    {
        [Fact]
        public void Test_CompanyAddressGetFromView()
        {
            try {

                using (var ctx = new HOSContext())
                {
                    /*
                        Test using repository directly
                     */
                    IRepository repository = new Repository(ctx);

                    // DbQuery<CompanyAddress> is connected to the view 'CompanyAddress'
                    var queryByCompanyId = repository.DBContext.CompanyAddresses.Where(ca => ca.CompanyId == 2).ToList();
                    Assert.NotEmpty(queryByCompanyId);
                    Assert.Equal(2, queryByCompanyId.Count());

                    // Query the same View as above, but use raw sql rather LINQ to filter it
                    // Should be faster as the select (filtering) happens on the RDBMS
                    string sql = "SELECT * FROM dbo.CompanyAddress WHERE CompanyId = 2";  
                    var queryByRawSql2View = repository.DBContext.CompanyAddresses.FromSql(sql).ToList();
                    Assert.NotEmpty(queryByRawSql2View);
                    Assert.Equal(2, queryByRawSql2View.Count());                    

                    // Query the same View as above without any filtering, so all records
                    // are return, for this app, that's not useful.
                    var addresses = repository.DBContext.CompanyAddresses.ToList();
                    Assert.NotEmpty(addresses);
                    Assert.Equal(6, addresses.Count());                                                             
                }

            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        [Fact]
        public void Test_CompanyAddressGetFromTableValuedFunc()
        {
            try {

                using (var ctx = new HOSContext())
                {
                    // Access through Repository
                    IRepository repository = new Repository(ctx);

                    var addresses = repository.DBContext.CompanyAddresses
                                                        .FromSql(@"SELECT * FROM dbo.CompanyAddressByCompanyId (2)")
                                                        .ToList();                                       

                    Assert.NotEmpty(addresses);
                    Assert.Equal(2, addresses.Count()); 

                    // Access through CompanyAddressManager
                    ICompanyAddressManager addressMgr = new CompanyAddressManager(repository);
                    var companyAddresses = addressMgr.GetCompanyAddressesRawSql(2);
                    Assert.NotEmpty(companyAddresses);
                    Assert.Equal(2, companyAddresses.Count());

                    var companyAddress = addressMgr.GetCompanyAddressRawSql(1);
                    Assert.NotNull(companyAddress);
                    Assert.Equal("1346 Markum Ranch Rd", companyAddress.AddressLine1);                                                        
                }

            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }        
    }
}
