using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BTCA.Common.Core;

namespace BTCA.Common.Entities
{
    public class LoadAssignment : BaseEntity
    {
        [Key]
        public virtual int LoadID { get; set; }

        [Required, MaxLength(20), Display(Name = "Load Number")]
        public virtual string LoadNumber { get; set; }

        [Required, Display(Name = "Dispatch Date"), DataType(DataType.Date)]
        public virtual DateTime DispatchDate { get; set; }

        [Display(Name = "Empty Begin")]
        public virtual int EmptyBeginMiles { get; set; }

        [Display(Name = "Empty End")]
        public virtual int EmptyEndMiles { get; set; }

        [Display(Name = "Loaded Begin")]
        public virtual int LoadedBeginMiles { get; set; }

        [Display(Name = "Loaded End")]
        public virtual int LoadedEndMiles { get; set; }

        [Required, Display(Name = "FSC")]
        public virtual double FuelSurchargeRate { get; set; }

        [Required, Display(Name = "Empty Rate")]
        public virtual double EmptyMilesRate { get; set; }

        [Required, Display(Name = "Loaded Rate")]
        public virtual double LoadedMilesRate { get; set; }

        [MaxLength(30), Display(Name = "Bill of Laden")]
        public virtual string BillOfLaden { get; set; }

        [DataType(DataType.Date), Display(Name = "Settlement Date")]
        public virtual DateTime SettlementDate { get; set; }

        [MaxLength(4000)]
        public virtual string Notes { get; set; }

        [Required, Display(Name = "AppUser Id")]
        public virtual int Id { get; set; }

        [ForeignKey(nameof(Id))]

        public virtual AppUser Driver { get; set; }    
                   
    }
}
