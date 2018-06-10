using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using BTCA.DataAccess.EF;

namespace BTCA.DataAccess.Core
{
    public interface IRepository
    {
        HOSContext DBContext { get; }
        IQueryable<T> All<T>() where T : class;
        void Create<T>(T TObject) where T : class;
        void Delete<T>(T TObject) where T : class;
        void Delete<T>(Expression<Func<T, bool>> predicate) where T : class;
        void Update<T>(T TObject) where T : class;
        void ExecuteProcedure(string procedureCommand, params SqlParameter[] sqlParams);
        IEnumerable<T> Filter<T>(Expression<Func<T, bool>> predicate) where T : class;
        IEnumerable<T> Filter<T>(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50) where T : class;
        T Find<T>(Expression<Func<T, bool>> predicate) where T : class;
        T Single<T>(Expression<Func<T, bool>> expression) where T : class;
        bool Contains<T>(Expression<Func<T, bool>> predicate) where T : class; 
        void Save();       
    }
}
