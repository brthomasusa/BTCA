using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NLog;
using Microsoft.EntityFrameworkCore;
using BTCA.DomainLayer.Managers.Interface;
using BTCA.Common.Entities;
using BTCA.Common.BusinessObjects;
using BTCA.DataAccess.Core;

namespace BTCA.DomainLayer.Managers.Implementation
{
    public class CompanyUserManager : ICompanyUserManager
    {
        private Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private IRepository _repository;

        public CompanyUserManager(IRepository repo)
        {
            // All Business objects that are composed from multiple entity objects must have this!!
            repo.DBContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;            
            _repository = repo;
        }

        public IEnumerable<CompanyUser> GetAll()
        {
            try {

                var query = _repository.All<AppUser>();

                var companyUsers = from appUser in query
                                        join company in _repository.All<Company>()
                                        on appUser.CompanyID equals company.ID
                                        select new CompanyUser
                                        {
                                            Id = appUser.Id,
                                            UserName = appUser.UserName,
                                            FirstName = appUser.FirstName,
                                            LastName = appUser.LastName,
                                            MiddleInitial = appUser.MiddleInitial,
                                            Email = appUser.Email,
                                            PhoneNumber = appUser.PhoneNumber,
                                            PhoneExtension = appUser.PhoneExtension,
                                            Password = appUser.PasswordHash,
                                            CompanyId = appUser.CompanyID,
                                            CompanyName = company.CompanyName
                                        };
            
                return companyUsers.OrderBy(user => user.CompanyName)
                                        .ThenBy(user => user.LastName)
                                        .ThenBy(user => user.FirstName);                

            } catch (Exception ex) when(Log(ex, "GetAll: Retrieval of all company users failed"))
            {
                throw ex;                
            }
        }

        public IEnumerable<CompanyUser> GetCompanyUsers(int companyId)
        {
            try {

                var query = _repository.Filter<AppUser>(u => u.CompanyID == companyId).AsQueryable();
                                    
                var companyUsers = from appUser in query
                                        join company in _repository.All<Company>()
                                        on appUser.CompanyID equals company.ID
                                        select new CompanyUser
                                        {
                                            Id = appUser.Id,
                                            UserName = appUser.UserName,
                                            FirstName = appUser.FirstName,
                                            LastName = appUser.LastName,
                                            MiddleInitial = appUser.MiddleInitial,
                                            Email = appUser.Email,
                                            PhoneNumber = appUser.PhoneNumber,
                                            PhoneExtension = appUser.PhoneExtension,
                                            Password = appUser.PasswordHash,
                                            CompanyId = appUser.CompanyID,
                                            CompanyName = company.CompanyName
                                        };
            
                return companyUsers.OrderBy(user => user.CompanyName)
                                        .ThenBy(user => user.LastName)
                                        .ThenBy(user => user.FirstName);

            } catch (Exception ex) when(Log(ex, $"GetCompanyUsers: Retrieval of users with company Id {companyId} failed"))
            {
                throw ex;
            }
        }
        public CompanyUser GetCompanyUser(int companyId, int userId)
        {
            try {

                var query = _repository.Filter<AppUser>(u => u.CompanyID == companyId)
                                    .Where(a => a.Id == userId).AsQueryable();

                var companyUser = from appUser in query
                                        join company in _repository.All<Company>()
                                        on appUser.CompanyID equals company.ID
                                        select new CompanyUser
                                        {
                                            Id = appUser.Id,
                                            UserName = appUser.UserName,
                                            FirstName = appUser.FirstName,
                                            LastName = appUser.LastName,
                                            MiddleInitial = appUser.MiddleInitial,
                                            Email = appUser.Email,
                                            PhoneNumber = appUser.PhoneNumber,
                                            PhoneExtension = appUser.PhoneExtension,
                                            Password = appUser.PasswordHash,
                                            CompanyId = appUser.CompanyID,
                                            CompanyName = company.CompanyName
                                        };

                return companyUser.SingleOrDefault();

            } catch (Exception ex) when(Log(ex, $"GetCompanyUser: Retrieval of user with company Id: {companyId} and user Id {userId} failed"))
            {
                throw ex;                
            }
        }

        public CompanyUser GetCompanyUser(Func<AppUser, bool> expression)
        {            
            try {

                var query = _repository.Filter<AppUser>(expression).AsQueryable();

                var companyUser = from appUser in query
                                        join company in _repository.All<Company>()
                                        on appUser.CompanyID equals company.ID
                                        select new CompanyUser
                                        {
                                            Id = appUser.Id,
                                            UserName = appUser.UserName,
                                            FirstName = appUser.FirstName,
                                            LastName = appUser.LastName,
                                            MiddleInitial = appUser.MiddleInitial,
                                            Email = appUser.Email,
                                            PhoneNumber = appUser.PhoneNumber,
                                            PhoneExtension = appUser.PhoneExtension,
                                            Password = appUser.PasswordHash,
                                            CompanyId = appUser.CompanyID,
                                            CompanyName = company.CompanyName
                                        };

                return companyUser.SingleOrDefault();

            } catch (Exception ex) when(Log(ex, "GetCompanyUser: Retrieval of user failed"))
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
