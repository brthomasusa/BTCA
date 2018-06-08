using System;
using System.ComponentModel.DataAnnotations;
using BTCA.Common.Core;

namespace BTCA.Common.BusinessObjects
{
    public class CompanyAddress : BaseEntity
    {
        [Required]
        public int ID { get; set; }

        [Required, MaxLength(50), Display(Name = "Address1")]
        public string AddressLine1 { get; set; }

        [MaxLength(50), Display(Name = "Address2")]
        public string AddressLine2 { get; set; }

        [Required, MaxLength(35)]
        public string City { get; set; }

        [MaxLength(2), MinLength(2), Display(Name = "State")]

        public string StateCode { get; set; }

        [Required, Display(Name = "State")]
        public int StateProvinceId { get; set; }

        [Required, MaxLength(9), DataType(DataType.PostalCode)]
        public string Zipcode { get; set; }

        [Required, Display(Name = "Country")]
        public string CountryCode { get; set; }

        [Required, Display(Name = "HQ?")]
        public bool IsHQ { get; set; }   

        [Required]
        public int CompanyId { get; set; }             
    }
}
