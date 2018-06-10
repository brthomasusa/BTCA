using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BTCA.Common.Core;

namespace BTCA.Common.Entities
{
    public class DutyStatusChangeLocation : BaseEntity
    {
        [Key]
        public virtual int StatusChangeLocationID { get; set ;}

        [Required, MaxLength(35)]
        public virtual string City { get; set; }

        [Required, Display(Name = "State")]
        public virtual int StateProvinceId { get; set; }
        [ForeignKey(nameof(StateProvinceId))]

        public virtual StateProvinceCode StateProvinceCode { get; set; }
        public virtual decimal Latitude { get; set; }
        public virtual decimal Longitude { get; set; }
    }
}
