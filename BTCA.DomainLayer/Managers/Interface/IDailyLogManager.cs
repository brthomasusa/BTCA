using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using BTCA.DomainLayer.Core;
using BTCA.Common.Entities;
using BTCA.Common.BusinessObjects;

namespace BTCA.DomainLayer.Managers.Interface
{
    public interface IDailyLogManager : IActionManager
    {
        IEnumerable<DailyLogModel> GetDailyLogs(Expression<Func<DailyLog, bool>> expression);
        DailyLogModel GetDailyLog(Expression<Func<DailyLog, bool>> expression);        
    }
}
