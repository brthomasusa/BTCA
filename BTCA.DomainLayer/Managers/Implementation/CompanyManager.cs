using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NLog;
using Microsoft.EntityFrameworkCore;
using BTCA.DomainLayer.Managers.Interface;
using BTCA.Common.Entities;
using BTCA.Common.Core;
using BTCA.DataAccess.Core;

namespace BTCA.DomainLayer.Managers.Implementation
{
    public class CompanyManager : ICompanyManager
    {
        private IRepository _repository;
        private Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public CompanyManager(IRepository repository) => _repository = repository;

        public IEnumerable<Company> GetCompanies(Expression<Func<Company, bool>> expression)
        {
            try {
                
                return _repository.DBContext.Companies.AsNoTracking().AsEnumerable();

            } catch (Exception ex) {
                _logger.Error(ex, "GetCompanies: Operation failed using expression {0}", expression);
                throw ex;                
            }
        }

        public IEnumerable<BaseEntity> GetAll()
        {
            try {

                return _repository.DBContext.Companies
                        .AsNoTracking()
                        .OrderBy(c => c.CompanyName);

            } catch (Exception ex) {
                _logger.Error(ex, "GetAll: Failed to retrieve companies.");
                throw ex;                 
            }
        }

        public Company GetCompany(Expression<Func<Company, bool>> expression)
        {            
            try {

                return _repository.Find<Company>(expression);

            } catch (Exception ex) {
                _logger.Error(ex, "GetCompanies: Operation failed using expression {0}", expression);
                throw ex;                
            }            
        }

        public void Create(BaseEntity entity)
        {
            try {

                Company company = (Company)entity;
                _logger.Info("Creating record for {0}", this.GetType());
                _repository.Create<Company>(company);
                _logger.Info("Record saved for {0}", this.GetType());

            } catch (Exception ex) {
                _logger.Error(ex, "Create: Create failed for company");
                throw ex;                
            }
        }

        public void Update(BaseEntity entity)
        {
            try {

                Company company = (Company)entity;
                _logger.Info("Updating record for {0}", this.GetType());
                _repository.Update<Company>(company);
                _logger.Info("Record saved for {0}", this.GetType());

            } catch (Exception ex) {
                _logger.Error(ex, "Update: Update failed for company with ID: {0}", ((Company)entity).ID);
                throw ex;                 
            }
        }

        public void Delete(BaseEntity entity)
        {
            try {

                Company company = (Company)entity;
                _logger.Info("Deleting record for {0}", this.GetType());
                _repository.Delete<Company>(company);
                _logger.Info("Record saved for {0}", this.GetType());

            } catch (Exception ex) {
                _logger.Error(ex, "Delete: Delete failed for Company with ID: {0}", ((Company)entity).ID);
                throw ex;                 
            }
        }

        public void SaveChanges()
        {
            try {

                _repository.Save();

            } catch (Exception ex) {
                _logger.Error(ex, "SaveChanges: Failed to save company.");
                throw ex;                
            }
        }        
    }
}
