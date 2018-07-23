using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NLog;
using BTCA.DomainLayer.Managers.Interface;
using BTCA.Common.Entities;
using BTCA.Common.Core;
using BTCA.DataAccess.Core;

namespace BTCA.DomainLayer.Managers.Implementation
{
    public class StateProvinceCodeManager : IStateProvinceCodeManager
    {
        private IRepository _repository;
        private Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public StateProvinceCodeManager(IRepository repo) => _repository = repo;

        public IEnumerable<BaseEntity> GetAll()
        {
            try {

                return _repository.All<StateProvinceCode>();

            } catch (Exception ex) when(Log(ex, "GetAll: Failed to retrieve StateProvinceCodes"))
            {
                throw ex;                
            }
                  
        }

        public IEnumerable<StateProvinceCode> GetStateProvinceCodes(Func<StateProvinceCode, bool> expression)
        {
            try {

                return _repository.Filter<StateProvinceCode>(expression);

            } catch (Exception ex) when(Log(ex, $"GetStateProvinceCodes: Operation failed using expression {expression}"))
            {
                throw ex; 
            }            
        }

        public StateProvinceCode GetStateProvinceCode(Func<StateProvinceCode, bool> expression)
        {            
            try {

                return _repository.Find<StateProvinceCode>(expression);

            } catch (Exception ex) when(Log(ex, $"GetStateProvinceCode: Operation failed using expression {expression}"))
            {
                throw ex; 
            }            
        }

        public void Create(BaseEntity entity)
        {
            try {

            var stateCode = (StateProvinceCode)entity;
                _logger.Info("Creating record for {0}", this.GetType());
                
                _repository.Create<StateProvinceCode>(stateCode);
                
                _logger.Info("Record saved for {0}", this.GetType());

            } catch (Exception ex) when(Log(ex, "Create: Create failed for StateProvinceCode"))
            {
                throw ex;
            }
        }

        public void Update(BaseEntity entity)
        {
            try {

                var stateCode = (StateProvinceCode)entity;
                _logger.Info("Updating record for {0}", this.GetType());
                _repository.Update<StateProvinceCode>(stateCode);
                _logger.Info("Record saved for {0}", this.GetType());

            } catch (Exception ex) when(Log(ex, $"Update: Update failed for StateProvinceCode with ID: {((StateProvinceCode)entity).ID}"))
            {
                throw ex;                
            }
        }

        public void Delete(BaseEntity entity)
        {
            try {

                var stateCode = (StateProvinceCode)entity;
                _logger.Info("Deleting record for {0}", this.GetType());
                _repository.Delete<StateProvinceCode>(stateCode);
                _logger.Info("Record deleted for {0}", this.GetType());

            } catch (Exception ex) when(Log(ex, $"Delete: Delete failed for StateProvinceCode with ID: {((StateProvinceCode)entity).ID}"))
            {
                throw ex;                  
            }
        }

        public void SaveChanges()
        {
            try {

                _repository.Save();

            } catch (Exception ex) when(Log(ex, "SaveChanges: Failed to save entity to the database."))
            {
                throw ex; 
            }            
        }

        private bool Log(Exception e, string msg)
        {
            _logger.Error(e, msg);
            return true;
        }                 
    }
}
