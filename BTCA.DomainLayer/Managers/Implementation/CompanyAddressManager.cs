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
    public class CompanyAddressManager : ICompanyAddressManager
    {
        private IRepository _repository;
        private Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public CompanyAddressManager(IRepository repo)
        {
            _repository = repo;
        }

        public void Create(BaseEntity entity)
        {            
            try {

                var companyAddress = (CompanyAddress)entity;
                var address = MapFromCompanyAddress(companyAddress);

                _logger.Info("Creating record for {0}", this.GetType());

                _repository.Create<Address>(address);

                _logger.Info("Record saved for {0}", this.GetType());

            } catch (Exception ex) {
                _logger.Error(ex, "Create: Create failed for company address.");
                throw ex;                
            }
        }

        public void Update(BaseEntity entity)
        {
            try {

                var companyAddress = (CompanyAddress)entity;
                var address = MapFromCompanyAddress(companyAddress);

                _logger.Info("Updating record for {0}", this.GetType());

                _repository.Update<Address>(address);

                _logger.Info("Record saved for {0}", this.GetType());

            } catch (Exception ex) {
                _logger.Error(ex, "Update: Update failed for company address with ID: {0}", ((CompanyAddress)entity).ID);
                throw ex;
            }
        }

        public void Delete(BaseEntity entity)
        {
            try {

                var companyAddress = (CompanyAddress)entity;
                var address = MapFromCompanyAddress(companyAddress);

                _logger.Info("Deleting record for {0}", this.GetType());

                _repository.Delete<Address>(address);

                _logger.Info("Record saved for {0}", this.GetType());

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
                _logger.Error(ex, "SaveChanges: Saving entity to the database failed");
                throw ex;
            }            
        } 

        public IEnumerable<BaseEntity> GetAll()
        {
            try {
                
                // Return readonly results by turning off ChangeTracking
                return _repository.DBContext.Addresses.AsNoTracking().AsEnumerable();

            } catch (Exception ex) {
                _logger.Error(ex, "GetAll: Retrieval of all addresses failed");
                throw ex;
            }            
        }

        public IEnumerable<CompanyAddress> GetCompanyAddresses(int companyId)
        {
            try {

                // Multiple ways of doing the same thing!!
                // var AddressesQuery = from company_addresses in _repository.DBContext.Companies
                //                                              .AsNoTracking()
                //                                              .AsQueryable()
                //                         where company_addresses.ID == companyId
                //                         select company_addresses;

                var query = _repository.DBContext.Addresses
                        .Where(c => c.CompanyId == companyId)
                        .AsNoTracking()
                        .AsQueryable();
                                    
                var companyAddresses = from address in query
                                        join stateCodes 
                                        in _repository.DBContext.StateProvinceCodes.AsNoTracking()
                                        on address.StateProvinceId equals stateCodes.ID
                                        select new CompanyAddress
                                        {
                                            ID = address.ID,
                                            AddressLine1 = address.AddressLine1,
                                            AddressLine2 = address.AddressLine2,
                                            City = address.City,
                                            StateProvinceId = stateCodes.ID,
                                            StateCode = stateCodes.StateCode,
                                            Zipcode = address.Zipcode,
                                            CountryCode = stateCodes.CountryCode,
                                            CompanyId = address.CompanyId,
                                            IsHQ = address.IsHQ,
                                            CreatedBy = address.CreatedBy,
                                            CreatedOn = address.CreatedOn,
                                            UpdatedBy = address.UpdatedBy,
                                            UpdatedOn = address.UpdatedOn
                                        };

                return companyAddresses.OrderBy(address => address.StateCode)
                                        .ThenBy(address => address.City)
                                        .ThenBy(address => address.AddressLine1);

            } catch (Exception ex) {
                _logger.Error(ex, "GetCompanyAddresses: Retrieval of addresses with company Id {CompanyId} failed", companyId);
                throw ex;
            }
        } 

        public CompanyAddress GetCompanyAddress(int companyId, int addressId)
        {
            try {

                // Have to use DBContext directly in order to turn off ChangeTracking
                var query = _repository.DBContext.Addresses
                        .Where(a => a.CompanyId == companyId && a.ID == addressId)
                        .AsNoTracking()
                        .AsQueryable();
                
                        
                var companyAddress = from address in query
                                        join stateCodes 
                                        in _repository.DBContext.StateProvinceCodes.AsNoTracking()
                                        on address.StateProvinceId equals stateCodes.ID
                                        select new CompanyAddress
                                        {
                                            ID = addressId,
                                            AddressLine1 = address.AddressLine1,
                                            AddressLine2 = address.AddressLine2,
                                            City = address.City,
                                            StateProvinceId = address.StateProvinceId,
                                            StateCode = stateCodes.StateCode,
                                            Zipcode = address.Zipcode,
                                            CountryCode = stateCodes.CountryCode,
                                            IsHQ = address.IsHQ,
                                            CompanyId = companyId,
                                            CreatedBy = address.CreatedBy,
                                            CreatedOn = address.CreatedOn,
                                            UpdatedBy = address.UpdatedBy,
                                            UpdatedOn = address.UpdatedOn
                                        };

                return companyAddress.SingleOrDefault();

            } catch (Exception ex) {
                _logger.Error(ex, "GetCompanyAddress: Retrieval of address with company Id {CompanyId} failed", companyId);
                throw ex;                
            }
        }

        public CompanyAddress GetCompanyAddress(Expression<Func<Address, bool>> expression)
        {
            try {

                // Have to use DBContext directly in order to turn off ChangeTracking
                var query = _repository.DBContext.Addresses
                        .Where(expression)
                        .AsNoTracking()
                        .AsQueryable();

                var companyAddress = from address in query
                                        join stateCodes 
                                        in _repository.DBContext.StateProvinceCodes.AsNoTracking()
                                        on address.StateProvinceId equals stateCodes.ID
                                        select new CompanyAddress
                                        {
                                            ID = address.ID,
                                            AddressLine1 = address.AddressLine1,
                                            AddressLine2 = address.AddressLine2,
                                            City = address.City,
                                            StateProvinceId = stateCodes.ID,
                                            StateCode = stateCodes.StateCode,
                                            Zipcode = address.Zipcode,
                                            CountryCode = stateCodes.CountryCode,
                                            IsHQ = address.IsHQ,
                                            CompanyId = address.CompanyId,
                                            CreatedBy = address.CreatedBy,
                                            CreatedOn = address.CreatedOn,
                                            UpdatedBy = address.UpdatedBy,
                                            UpdatedOn = address.UpdatedOn
                                        };

                return companyAddress.SingleOrDefault();

            } catch (Exception ex) {
                _logger.Error(ex, "GetCompanyAddress: Retrieval of address with expression {0}.", expression);
                throw ex;                 
            }
        }  

        private Address MapFromCompanyAddress(CompanyAddress source)
        {
            var address = new Address
            {     
                ID = source.ID,           
                AddressLine1 = source.AddressLine1,
                AddressLine2 = source.AddressLine2,
                City = source.City,
                StateProvinceId = source.StateProvinceId,
                Zipcode = source.Zipcode,
                IsHQ = source.IsHQ,
                CompanyId = source.CompanyId,
                CreatedBy = source.CreatedBy,
                CreatedOn = source.CreatedOn,
                UpdatedBy = source.UpdatedBy,
                UpdatedOn = source.UpdatedOn
            };

            return address;
        }              
    }
}
