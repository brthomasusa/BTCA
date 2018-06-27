using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using BTCA.DomainLayer.Core;
using BTCA.Common.Entities;
using BTCA.Common.BusinessObjects;
using BTCA.Common.Core;

namespace BTCA.DomainLayer.Managers.Interface
{
    public interface IDailyLogManager : IActionManager
    {
        IEnumerable<DailyLogModel> GetDailyLogs(Expression<Func<DailyLog, bool>> expression);
        DailyLogModel GetDailyLog(Expression<Func<DailyLog, bool>> expression);

        void CreateLogDetail(BaseEntity entity);
        void UpdateLogDetail(BaseEntity entity);
        void DeleteLogDetail(BaseEntity entity);  
        IEnumerable<DailyLogDetailModel> GetDailyLogDetails(Expression<Func<DailyLogDetail, bool>> expression);
        DailyLogDetailModel GetDailyLogDetail(Expression<Func<DailyLogDetail, bool>> expression);                      
        DailyLogDetailModel GetLastPreTripInspection(Expression<Func<DailyLog, bool>> expression);
    }
}
