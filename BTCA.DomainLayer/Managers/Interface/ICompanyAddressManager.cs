using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using BTCA.DomainLayer.Core;
using BTCA.Common.Entities;
using BTCA.Common.BusinessObjects;

namespace BTCA.DomainLayer.Managers.Interface
{
    public interface ICompanyAddressManager : IActionManager
    {
        IEnumerable<CompanyAddress> GetCompanyAddresses(int companyId);
        CompanyAddress GetCompanyAddress(int addressId);
        CompanyAddress GetCompanyAddress(Expression<Func<CompanyAddress, bool>> expression);               
    }
}
