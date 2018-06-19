using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using BTCA.DomainLayer.Core;
using BTCA.Common.Entities;

namespace BTCA.DomainLayer.Managers.Interface
{
    public interface ICompanyManager : IActionManager
    {
        IEnumerable<Company> GetCompanies(Expression<Func<Company, bool>> expression);
        Company GetCompany(Expression<Func<Company, bool>> expression);         
    }
}
