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
        IQueryable<T> AllQueryType<T>() where T : class;        
        IEnumerable<T> Filter<T>(Func<T, bool> expression) where T : class;        
        IEnumerable<T> FilterQuery<T>(Func<T, bool> expression) where T : class;
        T Find<T>(Func<T, bool> expression) where T : class;
        T FindQuery<T>(Func<T, bool> expression) where T : class;
        bool Contains<T>(Func<T, bool> expression) where T : class; 
        void Create<T>(T TObject) where T : class;
        void Delete<T>(T TObject) where T : class;
        void Delete<T>(Func<T, bool> expression) where T : class;
        void Update<T>(T TObject) where T : class;      
        void Save(); 
        void ExecuteProcedure(string procedureCommand, params SqlParameter[] sqlParams); 
     
    }
}
