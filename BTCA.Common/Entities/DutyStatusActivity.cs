using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BTCA.Common.Core;

namespace BTCA.Common.Entities
{
    public class DutyStatusActivity : BaseEntity
    {
        [Key]
        public virtual int DutyStatusActivityID { get; set; }

        [Required, MaxLength(18), Display(Name = "Purpose")]
        public virtual string Activity { get; set; }

        [Required, MaxLength(35), Display(Name = "Description")]
        public virtual string Description { get; set; }

        [InverseProperty(nameof(DailyLogDetail.DutyStatusActivity))]
        public virtual IList<DailyLogDetail> DailyLogDetails { get; set; } = new List<DailyLogDetail>();        
    }
}
