using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using BTCA.DomainLayer.Core;
using BTCA.Common.BusinessObjects;

namespace BTCA.DomainLayer.Managers.Interface
{
    public interface ICompanyAddressManager : IActionManager
    {
        IEnumerable<CompanyAddress> GetCompanyAddresses(int companyId);
        IEnumerable<CompanyAddress> GetCompanyAddresses(Func<CompanyAddress, bool> expression);
        CompanyAddress GetCompanyAddress(int addressId);             
        CompanyAddress GetCompanyAddress(Func<CompanyAddress, bool> expression);
    }
}
