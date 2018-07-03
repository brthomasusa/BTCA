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

        public IEnumerable<DailyLogModel> GetDailyLogs(Expression<Func<DailyLogModel, bool>> expression)
        {
            try {

                var dailyLogs = _repository.DBContext.DailyLogModels.Where(expression).ToList();
                return dailyLogs.OrderBy(log => log.LogDate);

            } catch (Exception ex) {
                _logger.Error(ex, "GetDailyLogs: Retrieval of daily log failed");
                throw ex;                   
            }            
        }

        public IEnumerable<DailyLogModel> GetDailyLogs(int driverId)
        {
            try {

                var dailyLogs = _repository.DBContext.DailyLogModels
                                                    .FromSql($"SELECT * FROM dbo.DailyLogModelByDriverId ({driverId})")
                                                    .ToList();

                return dailyLogs.OrderBy(log => log.LogDate);

            } catch (Exception ex) {
                _logger.Error(ex, "GetDailyLogs: Retrieval of daily log failed");
                throw ex;                 
            }            
        }

        public DailyLogModel GetDailyLog(Expression<Func<DailyLogModel, bool>> expression)
        {
            try {

                var dailyLog = _repository.DBContext.DailyLogModels.Where(expression).SingleOrDefault();
                return dailyLog;

            } catch (Exception ex) {
                _logger.Error(ex, "GetDailyLogs: Retrieval of daily log failed");
                throw ex;                 
            }
        }

        public DailyLogModel GetDailyLog(DateTime logDate, int driverId)
        {
            try {

                var strDate = logDate.Date.ToString("d");
                var dailyLog = _repository.DBContext.DailyLogModels
                                                    .FromSql($"SELECT * FROM dbo.DailyLogModelByLogDateAndDrvId ({strDate}, {driverId})")
                                                    .SingleOrDefault(); 

                return dailyLog;

            } catch (Exception ex) {
                _logger.Error(ex, "GetDailyLogs: Retrieval of daily log failed");
                throw ex;                 
            }            
        }

        public IEnumerable<BaseEntity> GetAll()
        {
            try {

                var dailyLogs = _repository.DBContext.DailyLogModels.ToList();
                return dailyLogs.OrderBy(log => log.DriverID).ThenBy(log => log.LogDate);

            } catch (Exception ex) {
                _logger.Error(ex, "GetAll: Failed to retrieve daily logs.");
                throw ex;                 
            }
        }

        public IEnumerable<DailyLogDetailModel> GetDailyLogDetails(Expression<Func<DailyLogDetailModel, bool>> expression)
        {
            try {
                
                var dailyLogDetailModels = _repository.DBContext.DailyLogDetailModels.Where(expression).ToList();
                return dailyLogDetailModels.OrderBy(detail => detail.StartTime);

            } catch (Exception ex) {
                _logger.Error(ex, "Failed to retrieve daily log details with expression: {0}.", expression);
                throw ex; 
            }
        }

        public IEnumerable<DailyLogDetailModel> GetDailyLogDetails(int logID)
        {
            try {
                
                var dailyLogs = _repository.DBContext.DailyLogDetailModels
                                                    .FromSql($"SELECT * FROM dbo.DailyLogDetailModelByLogID ({logID})")
                                                    .ToList();

                return dailyLogs.OrderBy(detail => detail.StartTime);

            } catch (Exception ex) {
                _logger.Error(ex, "Failed to retrieve daily log details with LogID: {0}.", logID);
                throw ex; 
            }            
        }

        public DailyLogDetailModel GetDailyLogDetail(Expression<Func<DailyLogDetailModel, bool>> expression)
        {
            try {
                
                var dailyLogDetailModels = _repository.DBContext.DailyLogDetailModels.Where(expression).SingleOrDefault();
                return dailyLogDetailModels;

            } catch (Exception ex) {
                _logger.Error(ex, "Failed to retrieve daily log details with expression: {0}.", expression);
                throw ex; 
            }
        }

        public DailyLogDetailModel GetDailyLogDetail(int logDetailID)
        {
            try {
                
                var dailyLog = _repository.DBContext.DailyLogDetailModels
                                                    .FromSql($"SELECT * FROM dbo.DailyLogDetailModelByLogDetailID ({logDetailID})")
                                                    .SingleOrDefault();

                return dailyLog;

            } catch (Exception ex) {
                _logger.Error(ex, "Failed to retrieve daily log details with LogDetaiID: {0}.", logDetailID);
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
                _logger.Error(ex, "Delete: Delete failed for DailyLog with ID: {0}", ((DailyLogModel)entity).LogID);
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

        public DailyLogDetailModel GetLastPreTripInspection(Expression<Func<DailyLogDetailModel, bool>> expression)
        {
            // expression should be the driver id
            // var filterQuery = _repository.DBContext.DailyLogs
            //                 .Where(expression)
            //                 .AsQueryable()
            //                 .AsNoTracking();

            // // Join filtered (by driverID) DailyLog to DailyLogDetails
            // var logDetailModel = (from dailyLog in filterQuery.AsNoTracking()
            //                         join dailyLogDetail in _repository.DBContext.DailyLogDetails.AsNoTracking()
            //                             on dailyLog.LogID equals dailyLogDetail.LogID
            //                         orderby dailyLogDetail.StartTime

            //                         select new DailyLogDetailModel 
            //                         {
            //                             LogDetailID = dailyLogDetail.LogDetailID,
            //                             LogID = dailyLogDetail.LogID,
            //                             DutyStatusID = dailyLogDetail.DutyStatusID,
            //                             StartTime = dailyLogDetail.StartTime,
            //                             StopTime = dailyLogDetail.StopTime,
            //                             LocationCity = dailyLogDetail.LocationCity,
            //                             StateProvinceId = dailyLogDetail.StateProvinceId,
            //                             DutyStatusActivityID = dailyLogDetail.DutyStatusActivityID                                                  
            //                         });  

            // var shiftBeginQuery = from shiftBegin in logDetailModel 
            //                       where(shiftBegin.DutyStatusActivityID == 1)
            //                       orderby(shiftBegin.StartTime) 
            //                       select(shiftBegin);

            // return shiftBeginQuery.LastOrDefault();
            return new DailyLogDetailModel();
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
