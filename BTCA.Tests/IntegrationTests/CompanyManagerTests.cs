using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Xunit;
using BTCA.Common.Entities;
using BTCA.DataAccess.Core;
using BTCA.DataAccess.EF;
using BTCA.DomainLayer.Managers.Interface;
using BTCA.DomainLayer.Managers.Implementation;

namespace BTCA.Tests.DomainManagers
{
    public class CompanyManagerTests
    {
        [Fact]
        public void Test_CompanyMgr_CompanyInsert()
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
                }                

                using (var context = new HOSContext(options))
                {
                    ICompanyManager companyMgr = new CompanyManager(new Repository(context));
                    
                    var company = new Company()
                    {
                        ID = 8,
                        CompanyCode = "TEST001",
                        CompanyName = "Testing Transportation, Inc",
                        DOT_Number = "951560",
                        MC_Number = "MC-407377",
                        CreatedBy = "admin",
                        CreatedOn = new DateTime(2016,10,10,8,30,0),
                        UpdatedBy = "admin",
                        UpdatedOn = new DateTime(2016,10,10,8,30,0)
                    };

                    companyMgr.Create(company);
                    companyMgr.SaveChanges();

                    var test = companyMgr.GetCompany(c => c.CompanyCode == "TEST001");
                    Assert.NotNull(test);
                    Assert.Equal(company.CompanyCode, test.CompanyCode);
                }

            } finally {
                connection.Close();
            }            
        }

        [Fact]
        public void Test_CompanyMgr_CompanyUpdate()
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
                } 

                using (var context = new HOSContext(options))
                {
                    ICompanyManager companyMgr = new CompanyManager(new Repository(context));
                    
                    var company = companyMgr.GetCompany(co => co.CompanyCode == "GWLS001");
                    Assert.NotNull(company);
                    
                    var updateTime = DateTime.Now;
                    company.UpdatedOn = updateTime;
                    companyMgr.Update(company);
                    companyMgr.SaveChanges();

                    company = companyMgr.GetCompany(co => co.CompanyCode == "GWLS001");
                    Assert.NotNull(company);
                    Assert.Equal(updateTime, company.UpdatedOn);

                }

            } finally {
                connection.Close();
            }              
        }

        [Fact]
        public void Test_CompanyMgr_CompanyDelete()
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
                    ICompanyManager companyMgr = new CompanyManager(new Repository(context));
                    
                    var company = new Company()
                    {
                        CompanyCode = "FCT001",
                        CompanyName = "First Choice Transport",
                        DOT_Number = "951560",
                        MC_Number = "MC-407377",
                        CreatedBy = "admin",
                        CreatedOn = DateTime.Now,
                        UpdatedBy = "admin",
                        UpdatedOn = DateTime.Now
                    };

                    companyMgr.Create(company);
                    companyMgr.SaveChanges();

                    var test = companyMgr.GetCompany(c => c.CompanyCode == "FCT001");
                    Assert.NotNull(test);

                    companyMgr.Delete(company);
                    companyMgr.SaveChanges();

                    test = companyMgr.GetCompany(c => c.CompanyCode == "FCT001");
                    Assert.Null(test);                    
                }

            } finally {
                connection.Close();
            }             
        }

        [Fact]
        public void Test_CompanyMgr_GetAll()
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
                }

                using (var context = new HOSContext(options))
                {
                    ICompanyManager companyMgr = new CompanyManager(new Repository(context));
                    var companies = companyMgr.GetAll().ToList();
                    Assert.NotNull(companies);
                    Assert.Equal(7, companies.Count());  
                }

            } finally {
                connection.Close();
            }             
        }        

        [Fact]
        public void Test_CompanyMgr_GetCompanies()
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
                }

                using (var context = new HOSContext(options))
                {
                    ICompanyManager companyMgr = new CompanyManager(new Repository(context));
                    
                    var companies = companyMgr.GetCompanies(c => 
                                    c.CompanyCode.ToUpper()
                                    .Contains("SWIFT".ToUpper()))
                                    .ToList();

                    Assert.NotNull(companies);
                    Assert.Equal(7, companies.Count());  
                }

            } finally {
                connection.Close();
            }             
        }                                
    }
}
