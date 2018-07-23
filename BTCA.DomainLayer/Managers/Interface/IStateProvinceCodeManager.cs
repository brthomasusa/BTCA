using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using BTCA.DomainLayer.Core;
using BTCA.Common.Entities;

namespace BTCA.DomainLayer.Managers.Interface
{
    public interface IStateProvinceCodeManager : IActionManager
    {
        IEnumerable<StateProvinceCode> GetStateProvinceCodes(Func<StateProvinceCode, bool> expression);
        StateProvinceCode GetStateProvinceCode(Func<StateProvinceCode, bool> expression);         
    }
}
