using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Moq;
using BTCA.Common.Entities;
using BTCA.Common.BusinessObjects;
using BTCA.DataAccess.Core;
using BTCA.DataAccess.EF;
using BTCA.DataAccess.Initializers;

namespace BTCA.Tests.UnitTests
{
    /*
        The following IRepository methods depend upon DbContext extension methods:
             IEnumerable<T> Filter<T>(Expression<Func<T, bool>> predicate) where T : class;
             IEnumerable<T> FilterQuery<T>(Expression<Func<T, bool>> predicate) where T : class;
             T Find<T>(Expression<Func<T, bool>> predicate) where T : class;
             T FindQuery<T>(Expression<Func<T, bool>> predicate) where T : class;
             bool Contains<T>(Expression<Func<T, bool>> predicate) where T : class; 
             void ExecuteProcedure(string procedureCommand, params SqlParameter[] sqlParams);

        The Moq framework does not support mocking c# extension methods, attempting to do so
        results in 'System.NotSupportedException : Invalid setup on an extension method'.
        There are no unit tests for these repository methods; testing is via integration tests.
    */

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
        [Trait("Category", "UnitTest.Repository")]
        public void GetAllDailyLogModels_As_DbQuery()
        {
            var mockSet = LoadDailyLogMockSet();
            var mockContext = new Mock<HOSContext>();

            mockContext.Setup(c => c.Query<DailyLogModel>()).Returns(mockSet.Object);   

            var repository = new Repository(mockContext.Object);

            // QueryTypes are a feature of EntityFrameworkCore 2.1
            var actual = repository.AllQueryType<DailyLogModel>();      

            Assert.Equal(4, actual.Count());
            Assert.Equal(new DateTime(2016,9,7), actual.First().LogDate);
        }     

        private Mock<DbQuery<DailyLogModel>> LoadDailyLogMockSet()
        {
            var mockSet = new Mock<DbQuery<DailyLogModel>>();
            mockSet.As<IQueryable<DailyLogModel>>().Setup(m => m.Provider).Returns(GetDailyLogModels().Provider);
            mockSet.As<IQueryable<DailyLogModel>>().Setup(m => m.Expression).Returns(GetDailyLogModels().Expression);
            mockSet.As<IQueryable<DailyLogModel>>().Setup(m => m.ElementType).Returns(GetDailyLogModels().ElementType);
            mockSet.As<IQueryable<DailyLogModel>>().Setup(m => m.GetEnumerator()).Returns(GetDailyLogModels().GetEnumerator());

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

        
        


        private DailyLogModel GetOneDailyLogModel() =>        
            new DailyLogModel 
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
            };            
        
        private IQueryable<DailyLogModel> GetDailyLogModels()
        {
            var data = new List<DailyLogModel> 
            {
                new DailyLogModel 
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
                new DailyLogModel 
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
                new DailyLogModel 
                {
                    LogID = 3,
                    LogDate = new DateTime(2016,9,9),
                    BeginningMileage = 800065,
                    EndingMileage = 900565,
                    TruckNumber = "3082",
                    TrailerNumber = "9159",
                    IsSigned = true,
                    DriverID = 4
                },
                new DailyLogModel 
                {
                    LogID = 4,
                    LogDate = new DateTime(2016,9,9),
                    BeginningMileage = 201255,
                    EndingMileage = 201601,
                    TruckNumber = "7895",
                    TrailerNumber = "99999",
                    IsSigned = true,
                    DriverID = 3
                },                                                                
            };
           
            return data.AsQueryable();            
        }                                  
    
    }
}
