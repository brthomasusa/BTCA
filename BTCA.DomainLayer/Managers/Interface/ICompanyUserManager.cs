using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using BTCA.Common.Entities;
using BTCA.Common.BusinessObjects;

namespace BTCA.DomainLayer.Managers.Interface
{
    public interface ICompanyUserManager
    {
        IEnumerable<CompanyUser> GetAll();
        IEnumerable<CompanyUser> GetCompanyUsers(int companyId);
        CompanyUser GetCompanyUser(int companyId, int userId);
        CompanyUser GetCompanyUser(Func<AppUser, bool> expression);         
    }
}
