using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Xunit;
using BTCA.Common.Entities;
using BTCA.DataAccess.Core;
using BTCA.DataAccess.EF;

namespace BTCA.Tests.DataAccess
{
    public class CompanyTests
    {
        [Fact]
        public void Test_CompanyCreate()
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
                    IRepository repository = new Repository(context);
                    
                    var company = new Company()
                    {
                        ID = 1,
                        CompanyCode = "FCT001",
                        CompanyName = "First Choice Transport",
                        DOT_Number = "951560",
                        MC_Number = "MC-407377",
                        CreatedBy = "admin",
                        CreatedOn = DateTime.Now,
                        UpdatedBy = "admin",
                        UpdatedOn = DateTime.Now
                    };

                    repository.Create<Company>(company);
                    repository.Save();
                    var test = repository.Find<Company>(c => c.ID == company.ID);

                    Assert.Equal(company.ID, test.ID);
                }

            } finally {
                connection.Close();
            }            
        }

        [Fact]
        public void Test_CompanyUpdate()
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
                    IRepository repository = new Repository(context);
                    
                    var company = new Company()
                    {
                        ID = 1,
                        CompanyCode = "FCT001",
                        CompanyName = "First Choice Transport",
                        DOT_Number = "951560",
                        MC_Number = "MC-407377",
                        CreatedBy = "admin",
                        CreatedOn = DateTime.Now,
                        UpdatedBy = "admin",
                        UpdatedOn = DateTime.Now
                    };

                    repository.Create<Company>(company);
                    repository.Save();
                    var test = repository.Find<Company>(c => c.ID == company.ID);

                    Assert.Equal(company.ID, test.ID);
                    
                    var currentDateTime= DateTime.Now;
                    company.CompanyCode = "FCT002";
                    company.CompanyName = "First Choice Transportation, Inc";
                    company.UpdatedOn = currentDateTime;
                    repository.Update<Company>(company);
                    repository.Save();
                    test = repository.Find<Company>(c => c.ID == company.ID);

                    Assert.Equal("FCT002", test.CompanyCode);
                    Assert.Equal("First Choice Transportation, Inc", test.CompanyName);
                    Assert.Equal(currentDateTime, test.UpdatedOn);
                }

            } finally {
                connection.Close();
            }            
        }

        [Fact]
        public void Test_CompanyDeleteByObject()
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
                    IRepository repository = new Repository(context);
                    
                    var company = new Company()
                    {
                        ID = 1,
                        CompanyCode = "FCT001",
                        CompanyName = "First Choice Transport",
                        DOT_Number = "951560",
                        MC_Number = "MC-407377",
                        CreatedBy = "admin",
                        CreatedOn = DateTime.Now,
                        UpdatedBy = "admin",
                        UpdatedOn = DateTime.Now
                    };

                    repository.Create<Company>(company);
                    repository.Save();
                    var test = repository.Find<Company>(c => c.ID == company.ID);

                    Assert.Equal(company.ID, test.ID);
                    
                    repository.Delete<Company>(company);
                    repository.Save();
                    test = repository.Find<Company>(c => c.ID == company.ID);

                    Assert.Null(test);
                }

            } finally {
                connection.Close();
            }              
        } 

        [Fact]
        public void Test_CompanyDeleteByID()
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
                    IRepository repository = new Repository(context);
                    
                    var company = new Company()
                    {
                        ID = 1,
                        CompanyCode = "FCT001",
                        CompanyName = "First Choice Transport",
                        DOT_Number = "951560",
                        MC_Number = "MC-407377",
                        CreatedBy = "admin",
                        CreatedOn = DateTime.Now,
                        UpdatedBy = "admin",
                        UpdatedOn = DateTime.Now
                    };

                    repository.Create<Company>(company);
                    repository.Save();
                    var test = repository.Find<Company>(c => c.ID == company.ID);

                    Assert.Equal(company.ID, test.ID);
                    
                    repository.Delete<Company>(c => c.ID == company.ID);
                    repository.Save();
                    test = repository.Find<Company>(c => c.ID == company.ID);

                    Assert.Null(test);
                }

            } finally {
                connection.Close();
            }              
        }

        [Fact]
        public void Test_CompanyGetAll()
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
                    IRepository repository = new Repository(context);
                    var companies = repository.All<Company>();
                    
                    Assert.NotNull(companies);
                    Assert.Equal(7, companies.ToList().Count());
                }

            } finally {
                connection.Close();
            }
        }

        [Fact]
        public void Test_CompanyFilter()
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
                    IRepository repository = new Repository(context);
                    var companies = repository.Filter<Company>(c => c.CompanyName.Contains("Swift"));
                    
                    Assert.NotNull(companies);
                    Assert.Equal(2, companies.ToList().Count());
                }

            } finally {
                connection.Close();
            }            
        }

        [Fact]
        public void Test_CompanyContains()
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
                    IRepository repository = new Repository(context);
                    var result = repository.Contains<Company>(c => c.CompanyCode == "ADMIN001");
                    
                    Assert.True(result);

                    result = repository.Contains<Company>(c => c.CompanyCode == "123456");

                    Assert.False(result);
                }

            } finally {
                connection.Close();
            }            
        }

        [Fact]
        public void Test_CompanyFromSql()
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
                    IRepository repository = new Repository(context);

                    string sql = "SELECT * FROM Companies WHERE ID = 1";
                    var btechnical = repository.DBContext.Companies.FromSql(sql).FirstOrDefault();

                    Assert.NotNull(btechnical);
                    Assert.Equal("ADMIN001", btechnical.CompanyCode);
                }

            } finally {
                connection.Close();
            }             
        }               
    }
}
