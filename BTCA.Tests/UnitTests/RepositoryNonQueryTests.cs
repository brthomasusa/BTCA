using System;
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
