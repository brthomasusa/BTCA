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

        public virtual void Create(BaseEntity entity)
        {            
            try {

                var companyAddress = (CompanyAddress)entity;
                var address = MapFromCompanyAddress(companyAddress);

                _repository.Create<Address>(address);

            } catch (Exception ex) {
                _logger.Error(ex, "Create: Create failed for company address.");
                throw ex;                
            }
        }

        public virtual void Update(BaseEntity entity)
        {
            try {

                var companyAddress = (CompanyAddress)entity;
                var address = MapFromCompanyAddress(companyAddress);

                _repository.Update<Address>(address);

            } catch (Exception ex) {
                _logger.Error(ex, "Update: Update failed for company address with ID: {0}", ((CompanyAddress)entity).ID);
                throw ex;
            }
        }

        public virtual void Delete(BaseEntity entity)
        {
            try {

                var companyAddress = (CompanyAddress)entity;
                var address = MapFromCompanyAddress(companyAddress);

                _repository.Delete<Address>(address);

            } catch (Exception ex) {
                _logger.Error(ex, "Delete: Delete failed for company address with ID: {0}", ((CompanyAddress)entity).ID);
                throw ex;                
            }
        }

        public virtual void SaveChanges()
        {
            try {

                _repository.Save();

            } catch (Exception ex) {
                _logger.Error(ex, "SaveChanges: Saving company address to the database failed");
                throw ex;
            }            
        } 

        public virtual IEnumerable<BaseEntity> GetAll()
        {
            try {
                    return _repository.AllQueryType<CompanyAddress>().AsEnumerable()
                                                                     .OrderBy(ca => ca.StateCode)
                                                                     .ThenBy(ca => ca.City)
                                                                     .ThenBy(ca => ca.AddressLine1);

            } catch (Exception ex) {
                _logger.Error(ex, "GetAll: Retrieval of all addresses failed");
                throw ex;
            }            
        }

        public virtual IEnumerable<CompanyAddress> GetCompanyAddresses(int companyId)
        {
            try {

                // dbo.CompanyAddressByCompanyId is a table-valued function
                return _repository.DBContext.CompanyAddresses
                                            .FromSql($"SELECT * FROM dbo.CompanyAddressByCompanyId ({companyId})")
                                            .ToList()
                                            .OrderBy(address => address.StateCode)
                                            .ThenBy(address => address.City)
                                            .ThenBy(address => address.AddressLine1);

            } catch (Exception ex) {
                _logger.Error(ex, "GetCompanyAddresses: Retrieval of addresses with company Id {0} failed", companyId);
                throw ex;
            }
        } 

        public virtual CompanyAddress GetCompanyAddress(int addressId)
        {
            try {

                // dbo.CompanyAddressByCompanyId is a table-valued function
                var companyAddress = _repository.DBContext.CompanyAddresses
                                                    .FromSql($"SELECT * FROM dbo.CompanyAddressByAddressId ({addressId})");

                return companyAddress.SingleOrDefault();

            } catch (Exception ex) {
                _logger.Error(ex, "GetCompanyAddress: Retrieval of address with address Id {0} failed", addressId);
                throw ex;                
            }
        }

        public virtual CompanyAddress GetCompanyAddress(Expression<Func<CompanyAddress, bool>> expression)
        {
            try {

                return _repository.AllQueryType<CompanyAddress>().Where(expression).SingleOrDefault();

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
