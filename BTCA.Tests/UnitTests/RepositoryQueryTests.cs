using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Moq;
using BTCA.Common.Entities;
using BTCA.DataAccess.Core;
using BTCA.DataAccess.EF;

namespace BTCA.Tests.UnitTests
{
    public class RepositoryQueryTests
    {
        [Fact]
        [Trait("Category", "Repository")]
        public void GetAllCompanies()
        {
            var mockSet = LoadCompanyMockSet();
            var mockContext = new Mock<HOSContext>();

            mockContext.Setup(c => c.Set<Company>()).Returns(mockSet.Object);   

            var repository = new Repository(mockContext.Object);
            var actual = repository.All<Company>();      

            Assert.Equal(7, actual.Count());
            Assert.Equal("Btechnical Consulting", actual.First().CompanyName);
        }

        [Fact]
        [Trait("Category", "Repository")]
        public void GetListOfCompanies_UsingRepoFilterFunc()
        {
            var mockSet = LoadCompanyMockSet();
            var mockContext = new Mock<HOSContext>();
            mockContext.Setup(c => c.Set<Company>()).Returns(mockSet.Object);   

            var repository = new Repository(mockContext.Object);
            var actual = repository.Filter<Company>(c => c.CompanyCode.Contains("SWIFT"));      

            Assert.Equal(2, actual.Count());
            Assert.Equal("SWIFT001", actual.OrderBy(c => c.CompanyCode).First().CompanyCode);
        }

        [Fact]
        [Trait("Category", "Repository")]
        public void GetOneCompany_UsingRepoFindFunc()
        {
            var mockSet = LoadCompanyMockSet();
            var mockContext = new Mock<HOSContext>();
            mockContext.Setup(c => c.Set<Company>()).Returns(mockSet.Object);   

            var repository = new Repository(mockContext.Object);
            var actual = repository.Find<Company>(c => c.CompanyCode == "ADMIN001");      

            Assert.NotNull(actual);
            Assert.Equal("Btechnical Consulting", actual.CompanyName);            
        }
        
        [Fact]
        [Trait("Category", "Repository")]
        public void GetAllDailyLogs_As_DbQuery()
        {
            var mockSet = LoadDailyLogMockSet();
            var mockContext = new Mock<HOSContext>();

            mockContext.Setup(c => c.Query<DailyLog>()).Returns(mockSet.Object);   

            var repository = new Repository(mockContext.Object);

            // QueryTypes are a feature of EntityFrameworkCore 2.1
            var actual = repository.AllQueryType<DailyLog>();      

            Assert.Equal(3, actual.Count());
            Assert.Equal(new DateTime(2016,9,7), actual.First().LogDate);
        }

        private Mock<DbQuery<DailyLog>> LoadDailyLogMockSet()
        {
            var mockSet = new Mock<DbQuery<DailyLog>>();
            mockSet.As<IQueryable<DailyLog>>().Setup(m => m.Provider).Returns(GetDailyLogs().Provider);
            mockSet.As<IQueryable<DailyLog>>().Setup(m => m.Expression).Returns(GetDailyLogs().Expression);
            mockSet.As<IQueryable<DailyLog>>().Setup(m => m.ElementType).Returns(GetDailyLogs().ElementType);
            mockSet.As<IQueryable<DailyLog>>().Setup(m => m.GetEnumerator()).Returns(GetDailyLogs().GetEnumerator());

            return mockSet; 
        }
        private Mock<DbSet<Company>> LoadCompanyMockSet()
        {
            var mockSet = new Mock<DbSet<Company>>();
            mockSet.As<IQueryable<Company>>().Setup(m => m.Provider).Returns(GetCompanies().Provider);
            mockSet.As<IQueryable<Company>>().Setup(m => m.Expression).Returns(GetCompanies().Expression);
            mockSet.As<IQueryable<Company>>().Setup(m => m.ElementType).Returns(GetCompanies().ElementType);
            mockSet.As<IQueryable<Company>>().Setup(m => m.GetEnumerator()).Returns(GetCompanies().GetEnumerator());

            return mockSet;            
        }
        private IQueryable<Company> GetCompanies()
        {
            var data = new List<Company> 
            {
                new Company {ID = 1, CompanyCode = "ADMIN001", CompanyName = "Btechnical Consulting", DOT_Number = "000000", MC_Number = "MC-000000", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new Company {ID = 2, CompanyCode = "FCT001", CompanyName = "First Choice Transport", DOT_Number = "123456", MC_Number = "MC-123456", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new Company {ID = 3, CompanyCode = "SWIFT001", CompanyName = "Swift Transportation", DOT_Number = "937712", MC_Number = "MC-987665", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new Company {ID = 4, CompanyCode = "SWIFT004", CompanyName = "Swift Trans LLC", DOT_Number = "712025", MC_Number = "MC-987665", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new Company {ID = 5, CompanyCode = "GWTM001", CompanyName = "GreatWide Truckload Management", DOT_Number = "430147", MC_Number = "MC-014987", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},
                new Company {ID = 6, CompanyCode = "CARD001", CompanyName = "Cardinal Logistics", DOT_Number = "703028", MC_Number = "MC-654321", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now},                                
                new Company {ID = 7, CompanyCode = "GWLS001", CompanyName = "Greatwide Logistics Services", DOT_Number = "380085", MC_Number = "MC-665871", CreatedBy = "admin", CreatedOn = DateTime.Now, UpdatedBy = "admin", UpdatedOn = DateTime.Now}
            };

            return data.AsQueryable().OrderBy(co => co.CompanyName);
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

        private IQueryable<DailyLog> GetDailyLogs()
        {
            var data = new List<DailyLog> 
            {
                new DailyLog 
                {
                    LogID = 1,
                    LogDate = new DateTime(2016,9,7),
                    BeginningMileage = 899201,
                    EndingMileage = 899423,
                    TruckNumber = "3082",
                    TrailerNumber = "9225",
                    IsSigned = true,
                    Notes = "Dropped trailer  9225 at Whirlpool and picked up loaded trailer 9159",
                    DriverID = 4
                },
                new DailyLog 
                {
                    LogID = 2,
                    LogDate = new DateTime(2016,9,8),
                    BeginningMileage = 899423,
                    EndingMileage = 900065,
                    TruckNumber = "3082",
                    TrailerNumber = "9159",
                    IsSigned = true,
                    DriverID = 4
                },
                new DailyLog 
                {
                    LogID = 3,
                    LogDate = new DateTime(2016,9,9),
                    BeginningMileage = 900065,
                    TruckNumber = "3082",
                    TrailerNumber = "9159",
                    IsSigned = false,
                    DriverID = 4
                }                                                  
            };

            return data.AsQueryable().OrderBy(dl => dl.LogDate);
        }                           
    
    }
}
