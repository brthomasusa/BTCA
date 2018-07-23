using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BTCA.Common.Core;
using BTCA.Common.Validations;

namespace BTCA.Common.Entities
{
    public class DailyLog : BaseEntity
    {
        [Key]
        public virtual int LogID { get; set; }

        [Required, Display(Name = "Log Date"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public virtual DateTime LogDate { get; set; }

        // [Range(int.MinValue, 0)]
        [Display(Name = "Beginning Mileage")]
        [MustNotBeGreaterThan(nameof(EndingMileage)), NotNegative]  // Custom validation
        public virtual int BeginningMileage { get; set; }

        [Display(Name = "Ending Mileage"), Range(int.MinValue, 0)]
        public virtual int EndingMileage { get; set; }

        [Required, MaxLength(25), Display(Name = "Truck Number")]
        public virtual string TruckNumber { get; set; }

        [MaxLength(25), Display(Name = "Trailer Number")]
        public virtual string TrailerNumber { get; set; }

        [Required]
        public virtual bool IsSigned { get; set; }

        [MaxLength(4000)]
        public virtual string Notes { get; set; }         

        [Required, Display(Name = "Driver ID")]  // AppUser.Id
        public virtual int DriverID { get; set; }

        [ForeignKey(nameof(DriverID))]

        public virtual AppUser Driver { get; set; } 

        [InverseProperty(nameof(DailyLogDetail.DailyLog))]      
        public virtual IList<DailyLogDetail> DailyLogDetails { get; set; } = new List<DailyLogDetail>();              
    }
}
