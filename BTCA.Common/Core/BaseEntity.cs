using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTCA.Common.Core
{
    public abstract class BaseEntity
    {
        public virtual string CreatedBy { get; set; }
        
        public virtual DateTime CreatedOn { get; set; }
        
        public virtual string UpdatedBy { get; set; }
        
        public virtual DateTime UpdatedOn { get; set; }
        
        [NotMapped]
        public int State { get; set; }        

        public BaseEntity()
        {
            this.CreatedOn = DateTime.Now;
            this.UpdatedOn = DateTime.Now;           
        }



        public enum EntityState
        {
            New=1, 
            Update=2, 
            Delete =3, 
            Ignore=4
        }
    }
}