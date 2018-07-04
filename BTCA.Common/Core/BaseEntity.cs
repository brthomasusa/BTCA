using System;
using System.ComponentModel.DataAnnotations;
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

    }
}