using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BTCA.Common.Core;

namespace BTCA.Common.Entities
{
    public class Address : BaseEntity
    {
        [Key]
        public virtual int ID { get; set; }
        
        [Required, MaxLength(50)]
        public virtual string AddressLine1 { get; set; }

        [MaxLength(50)]
        public virtual string AddressLine2 { get; set; }

        [Required, MaxLength(35)]
        public virtual string City { get; set; }

        [Required, Display(Name = "State")]
        public virtual int StateProvinceId { get; set; }

        [ForeignKey(nameof(StateProvinceId))]

        public virtual StateProvinceCode StateProvinceCode { get; set; }

        [Required, DataType(DataType.PostalCode), MaxLength(10), Display(Name = "Zip Code")]
        public virtual string Zipcode { get; set; }

        [Required, Display(Name = "Headquarters?")]
        public virtual bool IsHQ { get; set; }

        public virtual int CompanyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public virtual Company Company { get; set; }        
    }
}
