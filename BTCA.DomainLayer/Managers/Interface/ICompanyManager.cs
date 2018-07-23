using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using BTCA.DomainLayer.Core;
using BTCA.Common.Entities;

namespace BTCA.DomainLayer.Managers.Interface
{
    public interface ICompanyManager : IActionManager
    {
        IEnumerable<Company> GetCompanies(Func<Company, bool> expression);
        Company GetCompany(Func<Company, bool> expression);         
    }
}
