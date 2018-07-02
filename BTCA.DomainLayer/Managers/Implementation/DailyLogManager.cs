using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NLog;
using Microsoft.EntityFrameworkCore;
using BTCA.DomainLayer.Managers.Interface;
using BTCA.Common.Entities;
using BTCA.Common.Core;
using BTCA.Common.BusinessObjects;
using BTCA.DataAccess.Core;

namespace BTCA.DomainLayer.Managers.Implementation
{
    public class DailyLogManager : IDailyLogManager
    {
        private IRepository _repository;
        private Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        public DailyLogManager(IRepository repository) => _repository = repository; 

        public IEnumerable<DailyLogModel> GetDailyLogs(Expression<Func<DailyLog, bool>> expression)
        {
            try {

                var logsQuery = _repository.Filter<DailyLog>(expression).AsQueryable().AsNoTracking();

                var dailyLogModels = from dailyLog in logsQuery
                                    join appUser in _repository.All<AppUser>()
                                    on dailyLog.DriverID equals appUser.Id
                                    select new DailyLogModel
                                    {
                                        LogID = dailyLog.LogID,
                                        LogDate = dailyLog.LogDate,
                                        BeginningMileage = dailyLog.BeginningMileage,
                                        EndingMileage = dailyLog.EndingMileage,
                                        TruckNumber= dailyLog.TruckNumber,
                                        TrailerNumber = dailyLog.TrailerNumber,
                                        IsSigned = dailyLog.IsSigned,
                                        Notes = dailyLog.Notes,
                                        UserName = appUser.UserName,
                                        FirstName = appUser.FirstName,
                                        LastName = appUser.LastName,
                                        MiddleInitial = appUser.MiddleInitial,
                                        CreatedBy = dailyLog.CreatedBy,
                                        CreatedOn = dailyLog.CreatedOn,
                                        UpdatedBy = dailyLog.UpdatedBy,
                                        UpdatedOn = dailyLog.UpdatedOn
                                    };

                return dailyLogModels
                            .AsEnumerable()
                            .OrderBy(dl => dl.LogDate);                

            } catch (Exception ex) {
                _logger.Error(ex, "GetDailyLogs: Retrieval of daily log failed");
                throw ex;                   
            }            
        }

        public DailyLogModel GetDailyLog(Expression<Func<DailyLog, bool>> expression)
        {
            try {

                var query = _repository.DBContext.DailyLogs
                            .Where(expression)
                            .AsNoTracking();

                var dailyLogModel = from dailyLog in query
                                    join appUser in _repository.DBContext.Users.AsNoTracking()
                                    on dailyLog.DriverID equals appUser.Id
                                    select new DailyLogModel
                                    {
                                        LogID = dailyLog.LogID,
                                        LogDate = dailyLog.LogDate,
                                        BeginningMileage = dailyLog.BeginningMileage,
                                        EndingMileage = dailyLog.EndingMileage,
                                        TruckNumber= dailyLog.TruckNumber,
                                        TrailerNumber = dailyLog.TrailerNumber,
                                        IsSigned = dailyLog.IsSigned,
                                        Notes = dailyLog.Notes,
                                        DriverID = dailyLog.DriverID,
                                        UserName = appUser.UserName,
                                        FirstName = appUser.FirstName,
                                        LastName = appUser.LastName,
                                        MiddleInitial = appUser.MiddleInitial,
                                        CreatedBy = dailyLog.CreatedBy,
                                        CreatedOn = dailyLog.CreatedOn,
                                        UpdatedBy = dailyLog.UpdatedBy,
                                        UpdatedOn = dailyLog.UpdatedOn
                                    };

                var buffer = dailyLogModel.FirstOrDefault();
                if (buffer != null) {
                    //LoadDailyLogDetails(buffer);
                }

                return buffer; 

            } catch (Exception ex) {
                _logger.Error(ex, "GetDailyLogs: Retrieval of daily log failed");
                throw ex;                 
            }
        }

        public IEnumerable<BaseEntity> GetAll()
        {
            try {

                return _repository.DBContext.DailyLogs
                        .AsNoTracking()
                        .AsQueryable()
                        .AsEnumerable()
                        .OrderBy(dl => dl.DriverID)
                        .ThenBy(dl => dl.LogDate);

            } catch (Exception ex) {
                _logger.Error(ex, "GetAll: Failed to retrieve daily logs.");
                throw ex;                 
            }
        }

        public IEnumerable<DailyLogDetailModel> GetDailyLogDetails(Expression<Func<DailyLogDetail, bool>> expression)
        {
            try {
                
                // Filter all DailyLogDetails by expression
                var filterQuery = _repository.DBContext.DailyLogDetails
                                .Where(expression)
                                .AsQueryable()
                                .AsNoTracking();

                // Join filtered DailyLogDetails to DutyStatus, StateProvinceCode, and DutyStatusActivity tables
                var logDetailModel = (from dailyDetailEntry in filterQuery
                                      join dutyStatus in _repository.DBContext.DutyStatuses.AsNoTracking() 
                                            on dailyDetailEntry.DutyStatusID equals dutyStatus.DutyStatusID
                                      join stateCodes in _repository.DBContext.StateProvinceCodes.AsNoTracking() 
                                            on dailyDetailEntry.StateProvinceId equals stateCodes.ID
                                      join dutyStatusActivity in _repository.DBContext.DutyStatusActivities.AsNoTracking() 
                                            on dailyDetailEntry.DutyStatusActivityID equals dutyStatusActivity.DutyStatusActivityID

                                      select new DailyLogDetailModel 
                                      {
                                            LogDetailID = dailyDetailEntry.LogDetailID,
                                            LogID = dailyDetailEntry.LogID,
                                            DutyStatusID = dailyDetailEntry.DutyStatusID,
                                            DutyStatusShortName = dutyStatus.ShortName,
                                            StartTime = dailyDetailEntry.StartTime,
                                            StopTime = dailyDetailEntry.StopTime,
                                            ElapseTime = dailyDetailEntry.StopTime == null 
                                                ? TimeSpan.Zero 
                                                : dailyDetailEntry.StopTime - dailyDetailEntry.StartTime,
                                            LocationCity = dailyDetailEntry.LocationCity,
                                            StateProvinceId = dailyDetailEntry.StateProvinceId,
                                            StateCode = stateCodes.StateCode,
                                            Longitude = dailyDetailEntry.Longitude,
                                            Latitude = dailyDetailEntry.Latitude,
                                            DutyStatusActivity = dutyStatusActivity.Activity,
                                            DutyStatusActivityID = dailyDetailEntry.DutyStatusActivityID,
                                            Notes = dailyDetailEntry.Notes,
                                            CreatedBy = dailyDetailEntry.CreatedBy,
                                            CreatedOn = dailyDetailEntry.CreatedOn,
                                            UpdatedBy = dailyDetailEntry.UpdatedBy,
                                            UpdatedOn = dailyDetailEntry.UpdatedOn                                                   
                                      });

                return logDetailModel.OrderBy(log => log.StartTime).AsEnumerable();

            } catch (Exception ex) {
                _logger.Error(ex, "Failed to retrieve daily log details with expression: {0}.", expression);
                throw ex; 
            }
        }

        public DailyLogDetailModel GetDailyLogDetail(Expression<Func<DailyLogDetail, bool>> expression)
        {
            try {
                
                // Filter all DailyLogDetails by expression
                var filterQuery = _repository.DBContext.DailyLogDetails
                                .Where(expression)
                                .AsQueryable()
                                .AsNoTracking();

                // Join filtered DailyLogDetails to DutyStatus, StateProvinceCode, and DutyStatusActivity tables
                var logDetailModel = (from dailyDetailEntry in filterQuery.AsNoTracking()
                                      join dutyStatus in _repository.DBContext.DutyStatuses.AsNoTracking() 
                                            on dailyDetailEntry.DutyStatusID equals dutyStatus.DutyStatusID
                                      join stateCodes in _repository.DBContext.StateProvinceCodes.AsNoTracking() 
                                            on dailyDetailEntry.StateProvinceId equals stateCodes.ID
                                      join dutyStatusActivity in _repository.DBContext.DutyStatusActivities.AsNoTracking() 
                                            on dailyDetailEntry.DutyStatusActivityID equals dutyStatusActivity.DutyStatusActivityID

                                      select new DailyLogDetailModel 
                                      {
                                            LogDetailID = dailyDetailEntry.LogDetailID,
                                            LogID = dailyDetailEntry.LogID,
                                            DutyStatusID = dailyDetailEntry.DutyStatusID,
                                            DutyStatusShortName = dutyStatus.ShortName,
                                            StartTime = dailyDetailEntry.StartTime,
                                            StopTime = dailyDetailEntry.StopTime,
                                            ElapseTime = dailyDetailEntry.StopTime == null 
                                                ? TimeSpan.Zero 
                                                : dailyDetailEntry.StopTime - dailyDetailEntry.StartTime,
                                            LocationCity = dailyDetailEntry.LocationCity,
                                            StateProvinceId = dailyDetailEntry.StateProvinceId,
                                            StateCode = stateCodes.StateCode,
                                            Longitude = dailyDetailEntry.Longitude,
                                            Latitude = dailyDetailEntry.Latitude,
                                            DutyStatusActivity = dutyStatusActivity.Activity,
                                            DutyStatusActivityID = dailyDetailEntry.DutyStatusActivityID,
                                            Notes = dailyDetailEntry.Notes,
                                            CreatedBy = dailyDetailEntry.CreatedBy,
                                            CreatedOn = dailyDetailEntry.CreatedOn,
                                            UpdatedBy = dailyDetailEntry.UpdatedBy,
                                            UpdatedOn = dailyDetailEntry.UpdatedOn                                                   
                                      });

                return logDetailModel.FirstOrDefault();

            } catch (Exception ex) {
                _logger.Error(ex, "Failed to retrieve daily log details with expression: {0}.", expression);
                throw ex; 
            }
        }

        public void Create(BaseEntity entity)
        {            
            try {

                var model = (DailyLogModel)entity;
                var dailyLog = MapDailyLogModel2DailyLog(model);

                _repository.Create<DailyLog>(dailyLog);

            } catch (Exception ex) {
                _logger.Error(ex, "Create: Create failed for daily log.");
                throw ex;                
            }
        }

        public void CreateLogDetail(BaseEntity entity)
        {
            try {

                var model = (DailyLogDetailModel)entity;
                var dailyLogDetail = MapDailyLogDetailModel2DailyLogDetail(model);

                _repository.Create<DailyLogDetail>(dailyLogDetail);

            } catch (Exception ex) {
                _logger.Error(ex, "CreateDetail: Create failed for daily log detail entry.");
                throw ex;                
            }            
        }

        public void Update(BaseEntity entity)
        {
            try {

                var model = (DailyLogModel)entity;
                var dailyLog = MapDailyLogModel2DailyLog(model);

                _repository.Update<DailyLog>(dailyLog);

            } catch (Exception ex) {
                _logger.Error(ex, "Update: Update failed for daily log with ID: {0}", ((DailyLogModel)entity).LogID);
                throw ex;
            }
        }

        public void UpdateLogDetail(BaseEntity entity)
        {
            try {

                var model = (DailyLogDetailModel)entity;
                var dailyLogDetail = MapDailyLogDetailModel2DailyLogDetail(model);

                _repository.Update<DailyLogDetail>(dailyLogDetail);

            } catch (Exception ex) {
                _logger.Error(ex, "UpdateDetail: Update failed for daily log detail entry.");
                throw ex;                
            } 
        }

        public void Delete(BaseEntity entity)
        {
            try {

                var model = (DailyLogModel)entity;
                var dailyLog = MapDailyLogModel2DailyLog(model);

                _repository.Delete<DailyLog>(dailyLog);

            } catch (Exception ex) {
                _logger.Error(ex, "Delete: Delete failed for company address with ID: {0}", ((CompanyAddress)entity).ID);
                throw ex;                
            }
        }

        public void DeleteLogDetail(BaseEntity entity)
        {
            try {

                var model = (DailyLogDetailModel)entity;
                var dailyLogDetail = MapDailyLogDetailModel2DailyLogDetail(model);

                _repository.Delete<DailyLogDetail>(dailyLogDetail);

            } catch (Exception ex) {
                _logger.Error(ex, "DeleteDetail: Delete failed for daily log detail entry.");
                throw ex;                
            }             
        }

        public void SaveChanges()
        {
            try {

                _repository.Save();

            } catch (Exception ex) {
                _logger.Error(ex, "SaveChanges: Saving daily log to the database failed");
                throw ex;
            }            
        } 

        public DailyLogDetailModel GetLastPreTripInspection(Expression<Func<DailyLog, bool>> expression)
        {
            // expression should be the driver id
            var filterQuery = _repository.DBContext.DailyLogs
                            .Where(expression)
                            .AsQueryable()
                            .AsNoTracking();

            // Join filtered (by driverID) DailyLog to DailyLogDetails
            var logDetailModel = (from dailyLog in filterQuery.AsNoTracking()
                                    join dailyLogDetail in _repository.DBContext.DailyLogDetails.AsNoTracking()
                                        on dailyLog.LogID equals dailyLogDetail.LogID
                                    orderby dailyLogDetail.StartTime

                                    select new DailyLogDetailModel 
                                    {
                                        LogDetailID = dailyLogDetail.LogDetailID,
                                        LogID = dailyLogDetail.LogID,
                                        DutyStatusID = dailyLogDetail.DutyStatusID,
                                        StartTime = dailyLogDetail.StartTime,
                                        StopTime = dailyLogDetail.StopTime,
                                        LocationCity = dailyLogDetail.LocationCity,
                                        StateProvinceId = dailyLogDetail.StateProvinceId,
                                        DutyStatusActivityID = dailyLogDetail.DutyStatusActivityID                                                  
                                    });  

            var shiftBeginQuery = from shiftBegin in logDetailModel 
                                  where(shiftBegin.DutyStatusActivityID == 1)
                                  orderby(shiftBegin.StartTime) 
                                  select(shiftBegin);

            return shiftBeginQuery.LastOrDefault();
        }

        
        private DailyLogModel LoadDailyLogDetails(DailyLogModel dailyLogModel)
        {
            try {

                // IEnumerable<DailyLogDetailModel>
                var dailyLogDetails = GetDailyLogDetails(log => log.LogID == dailyLogModel.LogID);

                if (dailyLogDetails != null)
                {
                    foreach (DailyLogDetailModel detail in dailyLogDetails)
                    {
                        //dailyLogModel.DailyLogDetails.Add(detail);
                    }
                }

                return dailyLogModel;

            } catch (Exception ex) {
                _logger.Error(ex, "GetDailyLogs: Retrieval of daily log failed");
                throw ex;                 
            }
        }

        private DailyLog MapDailyLogModel2DailyLog(DailyLogModel source)
        {
            var destination = new DailyLog 
            {
                LogID = source.LogID,
                LogDate = source.LogDate,
                BeginningMileage = source.BeginningMileage,
                EndingMileage = source.EndingMileage,
                TruckNumber = source.TruckNumber,
                TrailerNumber = source.TrailerNumber,
                Notes = source.Notes,
                DriverID = source.DriverID,
                CreatedBy = source.CreatedBy,
                CreatedOn = source.CreatedOn,
                UpdatedBy = source.UpdatedBy,
                UpdatedOn = source.UpdatedOn
            };

            return destination;
        }

        private DailyLogDetail MapDailyLogDetailModel2DailyLogDetail(DailyLogDetailModel source)                             
        {
            var destination = new DailyLogDetail 
            {
                LogDetailID = source.LogDetailID,
                LogID = source.LogID,
                DutyStatusID = source.DutyStatusID,
                StartTime = source.StartTime,
                StopTime = source.StopTime,
                LocationCity = source.LocationCity,
                StateProvinceId = source.StateProvinceId,
                Longitude = source.Longitude,
                Latitude = source.Latitude,
                DutyStatusActivityID = source.DutyStatusActivityID,
                Notes = source.Notes,
                CreatedBy = source.CreatedBy,
                CreatedOn = source.CreatedOn,
                UpdatedBy = source.UpdatedBy,
                UpdatedOn = source.UpdatedOn
            };  

            return destination;
        }
    }
}
