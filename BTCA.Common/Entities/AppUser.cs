using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BTCA.Common.Entities
{
    public class AppUser : IdentityUser<int>
    {
        [Required, Display(Name= "First Name")]
        public virtual string FirstName { get; set; }

        [Required, Display(Name = "Last Name")]
        public virtual string LastName { get; set; }

        [Display(Name = "M.I.")]
        public virtual string MiddleInitial { get; set; }

        [Required]
        public virtual int CompanyID { get; set; }  
        
        [ForeignKey(nameof(CompanyID))]
        public virtual Company Company { get; set; }

        public virtual string PhoneExtension { get; set; }     
    }
}
