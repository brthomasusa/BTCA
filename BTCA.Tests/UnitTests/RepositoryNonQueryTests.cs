using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Moq;
using BTCA.Common.Entities;
using BTCA.DataAccess.Core;
using BTCA.DataAccess.EF;

namespace BTCA.Tests.UnitTests
{
    public class RepositoryNonQueryTests
    {
        [Fact]
        [Trait("Category", "Repository")]
        public void CreateCompany_saves_a_company_via_context()
        {
            // Testing BTCA.DataAccess.Core.IRepository.Create()
            var mockSet = new Mock<DbSet<Company>>();
              
            var mockContext = new Mock<HOSContext>(); 
            mockContext.Setup(c => c.Set<Company>()).Returns(mockSet.Object); 
            mockContext.Setup(c => c.SaveChanges()).Returns(1); 

            var repository = new Repository(mockContext.Object);
            repository.Create<Company>(GetOneCompany());
            repository.Save();

            mockSet.Verify(m => m.Add(It.IsAny<Company>()), Times.Once()); 
            mockContext.Verify(m => m.SaveChanges(), Times.Once()); 
        }

        [Fact]
        [Trait("Category", "Repository")]
        public void UpdateCompany_update_a_company_via_context()  
        {
            // Testing BTCA.DataAccess.Core.IRepository.Update()
            int methodInvocation = 0;
            var mockSet = new Mock<DbSet<Company>>();
              
            var mockContext = new Mock<HOSContext>(); 
            mockContext.Setup(c => c.Set<Company>()).Returns(mockSet.Object);
            // Setup HOSContext.SetModified to do nothing ...
            mockContext.Setup(c => c.SetModified(It.IsAny<Company>())).Callback(() => methodInvocation++);
            mockContext.Setup(c => c.SaveChanges()).Returns(1); 

            var repository = new Repository(mockContext.Object);
            repository.Update<Company>(GetOneCompany());
            repository.Save();  

            mockSet.Verify(m => m.Attach(It.IsAny<Company>()), Times.Once());
            mockContext.Verify(m => m.SetModified(It.IsAny<Company>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());                       
        }   

        [Fact]
        [Trait("Category", "Repository")]
        public void DeleteCompany_delete_a_company_via_context() 
        {
            // Testing BTCA.DataAccess.Core.IRepository.Delete() (delete 1 entity obj)
            var mockSet = new Mock<DbSet<Company>>();
              
            var mockContext = new Mock<HOSContext>(); 
            mockContext.Setup(c => c.Set<Company>()).Returns(mockSet.Object); 
            mockContext.Setup(c => c.SaveChanges()).Returns(1); 

            var repository = new Repository(mockContext.Object);
            repository.Delete<Company>(GetOneCompany());
            repository.Save();

            mockSet.Verify(m => m.Remove(It.IsAny<Company>()), Times.Once()); 
            mockContext.Verify(m => m.SaveChanges(), Times.Once());             
        }          

        [Fact]
        [Trait("Category", "Repository")]
        public void DeleteCompany_delete_multiple_companies_via_context() 
        {
            var mockSet = new Mock<DbSet<Company>>();
            mockSet.As<IQueryable<Company>>().Setup(m => m.Provider).Returns(GetCompanies().Provider);
            mockSet.As<IQueryable<Company>>().Setup(m => m.Expression).Returns(GetCompanies().Expression);
            mockSet.As<IQueryable<Company>>().Setup(m => m.ElementType).Returns(GetCompanies().ElementType);
            mockSet.As<IQueryable<Company>>().Setup(m => m.GetEnumerator()).Returns(GetCompanies().GetEnumerator());

            var mockContext = new Mock<HOSContext>();
            mockContext.Setup(c => c.Set<Company>()).Returns(mockSet.Object);   

            var repository = new Repository(mockContext.Object);
            repository.Delete<Company>(c => c.ID > 0);
            repository.Save(); 

            mockSet.Verify(m => m.Remove(It.IsAny<Company>()), Times.Exactly(7)); 
            mockContext.Verify(m => m.SaveChanges(), Times.Once());                        
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
    }
}
