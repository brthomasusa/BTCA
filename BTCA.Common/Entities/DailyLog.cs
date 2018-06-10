using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BTCA.Common.Core;

namespace BTCA.Common.Entities
{
    public class DailyLog : BaseEntity
    {
        [Key]
        public virtual int LogID { get; set; }


        [MaxLength(4000)]
        public virtual string Notes { get; set; }

        public virtual int CompanyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public virtual Company Company { get; set; }         
    }
}
