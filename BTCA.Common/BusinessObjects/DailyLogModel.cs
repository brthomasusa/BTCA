using System;
using System.ComponentModel.DataAnnotations;
using BTCA.Common.Core;

namespace BTCA.Common.BusinessObjects
{
    public class DailyLogModel : BaseEntity
    {
        [Required]
        public int ID { get; set; }

        [Required, Display(Name ="Date")]
        public DateTime LogDate { get; set; }        

        [Range(int.MinValue, 0), Display(Name = "Start Miles")]
        public int BeginningMileage { get; set; }

        [Range(int.MinValue, 0), Display(Name = "End Miles")]
        public int EndingMileage { get; set; }

        [Required, MaxLength(25), Display(Name = "Truck Number")]
        public string TruckNumber { get; set; }

        [MaxLength(25), Display(Name = "Trailer Number")]
        public string TrailerNumber { get; set; }

        [Required, Display(Name = "Signed?")]
        public bool IsSigned { get; set; }

        [Display(Name = "Violations?")]
        public bool HasViolations { get; set; }

        [MaxLength(4000)]
        public string Notes { get; set; }         

        [Required, Display(Name = "Driver ID")]  // AppUser.Id
        public int DriverID { get; set; } 

        [Display(Name = "Driver Code")]
        public string UserName { get; set; }

        [MaxLength(30), Display(Name= "First Name")]
        public virtual string FirstName { get; set; }

        [MaxLength(30), Display(Name = "Last Name")]
        public virtual string LastName { get; set; }

        [ MaxLength(1), Display(Name = "M.I.")]
        public virtual string MiddleInitial { get; set; }        
    }
}
