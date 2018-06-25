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
                                        ID = dailyLog.LogID,
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
                                        ID = dailyLog.LogID,
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

                return dailyLogModel.FirstOrDefault(); 

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

        public void Update(BaseEntity entity)
        {
            try {

                var model = (DailyLogModel)entity;
                var dailyLog = MapDailyLogModel2DailyLog(model);

                _repository.Update<DailyLog>(dailyLog);

            } catch (Exception ex) {
                _logger.Error(ex, "Update: Update failed for daily log with ID: {0}", ((DailyLogModel)entity).ID);
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

        public void SaveChanges()
        {
            try {

                _repository.Save();

            } catch (Exception ex) {
                _logger.Error(ex, "SaveChanges: Saving daily log to the database failed");
                throw ex;
            }            
        } 

        private DailyLog MapDailyLogModel2DailyLog(DailyLogModel source)
        {
            var destination = new DailyLog 
            {
                LogID = source.ID,
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
    }
}
