using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BTCA.Common.Core;

namespace BTCA.Common.Entities
{
    public class DailyLogDetail : BaseEntity
    {
        [Key]
        public virtual int LogDetailID { get; set; }

        [Required, DataType(DataType.DateTime), Display(Name = "Begin")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public virtual DateTime StartTime { get; set; }

        [DataType(DataType.DateTime), Display(Name = "End")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public virtual DateTime StopTime { get; set; }        

        [Required, MaxLength(35), Display(Name = "Location")]
        public virtual string LocationCity { get; set; }

        public virtual decimal Longitude { get; set; }

        public virtual decimal Latitude { get; set; }

        public virtual string Notes { get; set; }

        [Required]
        public virtual int LogID { get; set; }

        [ForeignKey(nameof(LogID))]

        public virtual DailyLog DailyLog { get; set; } 

        [Required]
        public virtual int DutyStatusID { get; set; } 

        [ForeignKey(nameof(DutyStatusID))]

        public virtual DutyStatus DutyStatus { get; set; }

        public virtual int StateProvinceId { get; set; }

        [ForeignKey(nameof(StateProvinceId))]

        public virtual StateProvinceCode StateProvinceCode { get; set; }

        public virtual int DutyStatusActivityID { get; set; }

        [ForeignKey(nameof(DutyStatusActivityID))] 

        public virtual DutyStatusActivity DutyStatusActivity { get; set; }       
    }
}
