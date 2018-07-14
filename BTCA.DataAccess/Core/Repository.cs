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

        public virtual IQueryable<T> All<T>() where T : class
        {
            return _context.Set<T>().AsQueryable();
        }

        public virtual IQueryable<T> AllQueryType<T>() where T : class
        {
            return _context.Query<T>().AsQueryable();
        }

        // This method can not be unit tested; that would require that
        // _context.Set<T>().Where<T>() be mocked. Doing so, results in the following
        // error: System.NotSupportedException : Invalid setup on an extension method: ...
        // Moq can not mock extension methods!!
        // Testing for this is done solely through integration testing.
        public virtual IEnumerable<T> Filter<T>(Func<T, bool> predicate) where T : class
        {            
            return _context.Set<T>().Where<T>(predicate).AsEnumerable<T>();
        }

        // This method can not be unit tested; that would require that
        // _context.Query<T>().Where<T>() be mocked. Doing so, results in the following
        // error: System.NotSupportedException : Invalid setup on an extension method: ...
        // Moq can not mock extension methods!!
        // Testing for this is done solely through integration testing.
        public virtual IEnumerable<T> FilterQuery<T>(Func<T, bool> predicate) where T : class
        {            
            return _context.Query<T>().Where<T>(predicate).AsEnumerable<T>();
        }

        // This method can not be unit tested; that would require that
        // _context.Set<T>().FirstOrDefault<T>() be mocked. Doing so, results in the following
        // error: System.NotSupportedException : Invalid setup on an extension method: ...
        // Moq can not mock extension methods!!
        // Testing for this is done solely through integration testing.
        public virtual T Find<T>(Func<T, bool> predicate) where T : class
        {
            return _context.Set<T>().SingleOrDefault<T>(predicate);
        }

        // This method can not be unit tested; that would require that
        // _context.Query<T>().FirstOrDefault<T>() be mocked. Doing so, results in the following
        // error: System.NotSupportedException : Invalid setup on an extension method: ...
        // Moq can not mock extension methods!!
        // Testing for this is done solely through integration testing.
        public virtual T FindQuery<T>(Func<T, bool> predicate) where T : class
        {
            return _context.Query<T>().FirstOrDefault<T>(predicate);
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
                _context.Set<T>().Attach(TObject);
                _context.SetModified(TObject);
            }
            catch (Exception ex) when(Log(ex, ex.Message))
            {
                throw ex;
            }
        }

        public virtual void Delete<T>(Func<T, bool> predicate) where T : class
        {
            var objects = Filter<T>(predicate);
            foreach (var obj in objects)
                _context.Set<T>().Remove(obj);
        }

        public virtual bool Contains<T>(Func<T, bool> predicate) where T : class
        {
            return _context.Set<T>().Count<T>(predicate) > 0;
        }

        public virtual void ExecuteProcedure(String procedureCommand, params SqlParameter[] sqlParams)
        {
            try {

                _context.Database.ExecuteSqlCommand(procedureCommand, sqlParams);

            } catch (Exception ex) when(Log(ex, ex.Message))
            {
                throw ex;
            }            
        } 

        public virtual void Save()
        {
            try {

                _context.SaveChanges();
            
            } catch (Exception ex)  when(Log(ex, ex.Message))
            {
                throw ex;
            }
                         
        } 

        public virtual HOSContext DBContext
        {
            get { return _context; }
        } 

        private bool Log(Exception e, string msg)
        {
            _logger.Error(e, msg);
            return true;
        }
    }
}
