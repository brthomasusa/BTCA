using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using BTCA.Common.Core;

namespace BTCA.DomainLayer.Core
{
    public interface IActionManager
    {
        void Create(BaseEntity entity);
        void Update(BaseEntity entity);
        void Delete(BaseEntity entity);
        IEnumerable<BaseEntity> GetAll();
        void SaveChanges();         
    }
}
