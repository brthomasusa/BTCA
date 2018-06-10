using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BTCA.Common.Core;

namespace BTCA.Common.Entities
{
    public class DutyStatus : BaseEntity
    {
        [Key]
        public virtual int DutyStatusID { get; set; }

        [Required, MaxLength(8), Display(Name = "Short Name")]
        public virtual string ShortName { get; set; }
        
        [Required, MaxLength(25), Display(Name = "Long Name")]
        public virtual string LongName { get; set; }

    }
}