using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Moq;
using BTCA.Common.Entities;
using BTCA.DomainLayer.Managers.Implementation;
using BTCA.DataAccess.Core;
using BTCA.DataAccess.Initializers;

namespace BTCA.Tests.UnitTests
{
    public class CompanyManagerTests
    {
        [Fact]
        [Trait("Category", "UnitTest.CompanyManager")]
        public void GetCompanies_using_Repo_Filter()
        {
            var data = GetCompanies();

            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(repo => repo.Filter<Company>(It.IsAny<Func<Company, bool>>()))
                                        .Returns(data)
                                        .Callback(
                                            (Expression<Func<Company, bool>> expression) => 
                                                {
                                                    data = data.Where(expression);
                                                }
                                        );

            var companyMgr = new CompanyManager(mockRepo.Object);
            var result = companyMgr.GetCompanies(c => c.CompanyCode.Contains("SWIFT")); 

            mockRepo.Verify(repo => repo.Filter<Company>(It.IsAny<Func<Company, bool>>()), Times.Once());

            var results = data.ToList().OrderBy(c => c.CompanyName);
            Assert.Equal(2, results.Count());
            Assert.Equal("SWIFT001", results.OrderBy(c => c.CompanyCode).First().CompanyCode);                                                  
        } 

        [Fact]
        [Trait("Category", "UnitTest.CompanyManager")]
        public void GetAll_using_Repo_All()
        {
            var data = GetCompanies();  
            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(repo => repo.All<Company>())
                                        .Returns(data)
                                        .Callback(
                                            () => data = data.OrderBy(c => c.CompanyName)
                                        );

            var companyMgr = new CompanyManager(mockRepo.Object);
            var results = companyMgr.GetAll().ToList().OrderBy(c => ((Company)c).CompanyName);

            mockRepo.Verify(repo => repo.All<Company>(), Times.Once());
            Assert.Equal(7, results.Count());
            Assert.Equal("Btechnical Consulting, Inc.", ((Company)results.First()).CompanyName); 
        }

        [Fact]
        [Trait("Category", "UnitTest.CompanyManager")]
        public void GetCompany_using_Repo_Find()
        {
            var data = GetCompanies();

            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(repo => repo.Find<Company>(It.IsAny<Func<Company, bool>>()))                                        
                                        .Returns((Expression<Func<Company, bool>> expression) => 
                                        {
                                            var query = data.Where(expression);
                                            var company = from item in query
                                                select new Company 
                                                {
                                                    ID = item.ID,
                                                    CompanyCode = item.CompanyCode,
                                                    CompanyName = item.CompanyName,
                                                    DOT_Number = item.DOT_Number,
                                                    MC_Number = item.MC_Number
                                                };
                                                return company.SingleOrDefault();
                                        } );

            var companyMgr = new CompanyManager(mockRepo.Object);
            var result = companyMgr.GetCompany(c => c.CompanyCode == "GWLS001"); 

            mockRepo.Verify(repo => repo.Find<Company>(It.IsAny<Func<Company, bool>>()), Times.Once());
            Assert.Equal("Greatwide Logistics Services", result.CompanyName);
        }

        [Fact]
        [Trait("Category", "UnitTest.CompanyManager")]
        public void CreateCompany_using_Repo_Create()
        {
            var methodCall = 0;
            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(repo => repo.Create<Company>(It.IsAny<Company>())).Callback(() => methodCall++ );                                       
            mockRepo.Setup(repo => repo.Save());
                                        
            var companyMgr = new CompanyManager(mockRepo.Object);
            companyMgr.Create(GetOneCompany());
            companyMgr.SaveChanges();

            mockRepo.Verify(repo => repo.Create(It.IsAny<Company>()), Times.Once());
            mockRepo.Verify(repo => repo.Save(), Times.Once());
        }

        [Fact]
        [Trait("Category", "UnitTest.CompanyManager")]
        public void UpdateCompany_using_Repo_Update()
        {
            var methodCall = 0;
            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(repo => repo.Update<Company>(It.IsAny<Company>())).Callback(() => methodCall++ );                                       
            mockRepo.Setup(repo => repo.Save());
                                        
            var companyMgr = new CompanyManager(mockRepo.Object);
            companyMgr.Update(GetOneCompany());
            companyMgr.SaveChanges();

            mockRepo.Verify(repo => repo.Update(It.IsAny<Company>()), Times.Once());
            mockRepo.Verify(repo => repo.Save(), Times.Once());
        }

        [Fact]
        [Trait("Category", "UnitTest.CompanyManager")]
        public void DeleteCompany_using_Repo_Delete()
        {
            var methodCall = 0;
            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(repo => repo.Delete<Company>(It.IsAny<Company>())).Callback(() => methodCall++ );                                       
            mockRepo.Setup(repo => repo.Save());
                                        
            var companyMgr = new CompanyManager(mockRepo.Object);
            companyMgr.Delete(GetOneCompany());
            companyMgr.SaveChanges();

            mockRepo.Verify(repo => repo.Delete(It.IsAny<Company>()), Times.Once());
            mockRepo.Verify(repo => repo.Save(), Times.Once());
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
    }
}
