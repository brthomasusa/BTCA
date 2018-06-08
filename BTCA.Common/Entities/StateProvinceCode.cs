using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BTCA.Common.Core;

namespace BTCA.Common.Entities
{
    public class StateProvinceCode : BaseEntity
    {
        [Key]
        public virtual int ID { get; set; }

        [Required, MaxLength(2)]
        public virtual string StateCode { get; set; }

        [Required, MaxLength(50), Display(Name = "State/Province Name")]
        public virtual string StateName { get; set; }

        [Required, MaxLength(3), Display(Name = "Country")]
        public virtual string CountryCode { get; set; }  

        [InverseProperty(nameof(Address.StateProvinceCode))]
        public virtual List<Address> Addresses { get; set; } = new List<Address>();        
    }
}
