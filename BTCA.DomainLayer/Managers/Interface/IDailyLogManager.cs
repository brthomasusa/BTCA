using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using BTCA.DomainLayer.Core;
using BTCA.Common.BusinessObjects;
using BTCA.Common.Core;

namespace BTCA.DomainLayer.Managers.Interface
{
    public interface IDailyLogManager : IActionManager
    {
        IEnumerable<DailyLogModel> GetDailyLogs(Expression<Func<DailyLogModel, bool>> expression);
        IEnumerable<DailyLogModel> GetDailyLogs(int driverId);
        DailyLogModel GetDailyLog(Expression<Func<DailyLogModel, bool>> expression);
        DailyLogModel GetDailyLog(DateTime logDate, int driverId);
        void CreateLogDetail(BaseEntity entity);
        void UpdateLogDetail(BaseEntity entity);
        void DeleteLogDetail(BaseEntity entity);  
        IEnumerable<DailyLogDetailModel> GetDailyLogDetails(Expression<Func<DailyLogDetailModel, bool>> expression);
        IEnumerable<DailyLogDetailModel> GetDailyLogDetails(int logID);
        DailyLogDetailModel GetDailyLogDetail(Expression<Func<DailyLogDetailModel, bool>> expression);                      
        DailyLogDetailModel GetDailyLogDetail(int logDetailID);
        DailyLogDetailModel GetLastPreTripInspection(Expression<Func<DailyLogDetailModel, bool>> expression);
    }
}
