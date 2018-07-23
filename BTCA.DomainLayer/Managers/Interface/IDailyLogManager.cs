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
        IEnumerable<DailyLogModel> GetDailyLogs(Func<DailyLogModel, bool> expression);
        IEnumerable<DailyLogModel> GetDailyLogsForDriver(int driverId);
        IEnumerable<DailyLogModel> GetDailyLogsForCompany(int companyId);
        
        DailyLogModel GetDailyLog(Func<DailyLogModel, bool> expression);
        DailyLogModel GetDailyLog(DateTime logDate, int driverId);
        DailyLogModel GetDailyLog(int LogId);
        void CreateLogDetail(BaseEntity entity);
        void UpdateLogDetail(BaseEntity entity);
        void DeleteLogDetail(BaseEntity entity);  
        IEnumerable<DailyLogDetailModel> GetDailyLogDetails(Func<DailyLogDetailModel, bool> expression);
        IEnumerable<DailyLogDetailModel> GetDailyLogDetails(int logID);
        DailyLogDetailModel GetDailyLogDetail(Func<DailyLogDetailModel, bool> expression);                      
        DailyLogDetailModel GetDailyLogDetail(int logDetailID);
        DailyLogDetailModel GetLastPreTripInspection(Func<DailyLogDetailModel, bool> expression);
    }
}
