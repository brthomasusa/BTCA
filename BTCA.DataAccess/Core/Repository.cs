using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using NLog;
using BTCA.DataAccess.EF;

namespace BTCA.DataAccess.Core
{
    public class Repository : IRepository
    {
        private HOSContext _context;
        private Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public Repository(HOSContext ctx) => _context = ctx;

        public IQueryable<T> All<T>() where T : class
        {
            return _context.Set<T>().AsQueryable();
        }

        public virtual IEnumerable<T> Filter<T>(Expression<Func<T, bool>> predicate) where T : class
        {            
            return _context.Set<T>().Where<T>(predicate).AsEnumerable<T>();
        }

        public virtual T Find<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return _context.Set<T>().FirstOrDefault<T>(predicate);
        }

        public virtual void Create<T>(T TObject) where T : class
        {
            _context.Set<T>().Add(TObject);
        }

        public virtual void Delete<T>(T TObject) where T : class
        {
            _context.Set<T>().Remove(TObject);
        }

        public virtual void Update<T>(T TObject) where T : class
        {
            try
            {
               var entry = _context.Entry(TObject);
               _context.Set<T>().Attach(TObject);
               entry.State = EntityState.Modified;
            }
            catch (Exception ex) {
                _logger.Error(ex, ex.Message);
                throw ex;
            }
        }

        public virtual void Delete<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            var objects = Filter<T>(predicate);
            foreach (var obj in objects)
                _context.Set<T>().Remove(obj);
        }

        public bool Contains<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return _context.Set<T>().Count<T>(predicate) > 0;
        }

        public virtual void ExecuteProcedure(String procedureCommand, params SqlParameter[] sqlParams)
        {
            try {

                _context.Database.ExecuteSqlCommand(procedureCommand, sqlParams);

            } catch (Exception ex) {
                _logger.Error(ex, ex.Message);
                throw ex;
            }            
        } 

        public void Save()
        {
            try {

                _context.SaveChanges();
            
            } catch (Exception ex) {
                _logger.Error(ex, ex.Message);
                throw ex;
            }
                         
        } 

        public HOSContext DBContext
        {
            get { return _context; }
        } 
             
    }
}
