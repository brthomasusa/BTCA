using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BTCA.Common.Core;

namespace BTCA.Common.BusinessObjects
{
    public class DailyLogDetailModel : BaseEntity
    {
        [Required]
        public int LogDetailID { get; set; }

        [Required]
        public int LogID { get; set; }

        [Required]
        public int DutyStatusID { get; set; }

        [Display(Name = "Duty Status")]
        public string ShortName { get; set; }

        [Required, Display(Name = "Start")]
        public DateTime StartTime { get; set; }

        [Required, Display(Name = "Stop")]
        public DateTime StopTime { get; set; }

        [Display(Name = "Total Time")]
        public int ElapseTime { get; set; }

        [Required, Display(Name = "City")]
        public string LocationCity { get; set; }

        [Required]
        public int StateProvinceId { get; set; }

        [Display(Name = "State")]
        public string StateCode { get; set; }
        [Column(TypeName="decimal(10,7)")]
        public decimal Longitude { get; set; }

        [Column(TypeName="decimal(10,7)")]
        public decimal Latitude { get; set; }
        
        public int DutyStatusActivityID { get; set; }

        [Display(Name = "Activity")]
        public string Activity { get; set; }

        public string Notes { get; set; }        
    }
}
