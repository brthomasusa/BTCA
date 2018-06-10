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

        [Required, Display(Name = "Dispatch Date")]
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

        [Required, Display(Name = "Pickups")]
        public virtual int NumberOfPickups { get; set; }

        [Required, Display(Name = "Stops")]
        public virtual int NumberOfStops { get; set; }

        public virtual int CompanyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public virtual Company Company { get; set; }          
    }
}
