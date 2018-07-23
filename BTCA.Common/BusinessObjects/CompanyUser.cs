using System.ComponentModel.DataAnnotations;

namespace BTCA.Common.BusinessObjects
{
    public class CompanyUser
    {
        [Required]
        public int Id { get; set; }

        [Required, MaxLength(25), Display(Name = "User Name")]
        public virtual string UserName { get; set; }        

        [Required, Display(Name= "First Name")]
        public virtual string FirstName { get; set; }

        [Required, Display(Name = "Last Name")]
        public virtual string LastName { get; set; }

        [Display(Name = "M.I.")] 
        public virtual string MiddleInitial { get; set; }

        [Required, DataType(DataType.EmailAddress), MaxLength(50)]
        public virtual string Email { get; set; }

        [Display(Name = "Telephone")]
        [DataType(DataType.PhoneNumber), MaxLength(12)]
        public virtual string PhoneNumber { get; set; }

        [MaxLength(10), Display(Name = "Ext.")]
        public virtual string PhoneExtension { get; set; }

        [Required, MaxLength(50)]
        public virtual string Password { get; set; }       

        [Required]
        public int CompanyId { get; set; }

        [Display(Name = "Company")]
        public string CompanyName { get; set; }   
        
        [Display(Name ="Role")]
        public string RoleName { get; set; } 
    }
}
