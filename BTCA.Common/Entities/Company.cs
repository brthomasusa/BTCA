using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BTCA.Common.Core;

namespace BTCA.Common.Entities
{
    public class Company : BaseEntity
    {
        [Key]
        public virtual int ID { get; set; }

        [Display(Name = "Code")]
        public virtual string CompanyCode { get; set; }

        [Display(Name = "Name")]
        public virtual string CompanyName { get; set; }

        [Display(Name = "DOT Number")]
        public virtual string DOT_Number { get; set; }

        [Display(Name = "MC Number")]
        public virtual string MC_Number { get; set; }

        [InverseProperty(nameof(AppUser.Company))]
        public virtual IList<AppUser> AppUsers { get; set; } = new List<AppUser>();  

        [InverseProperty(nameof(Address.Company))]      
        public virtual IList<Address> Addresses { get; set; } = new List<Address>();   

        [InverseProperty(nameof(LoadAssignment.Company))]      
        public virtual IList<LoadAssignment> LoadAssignments { get; set; } = new List<LoadAssignment>();             

        [InverseProperty(nameof(DailyLog.Company))]      
        public virtual IList<DailyLog> DailyLogs { get; set; } = new List<DailyLog>();         
    }
}
