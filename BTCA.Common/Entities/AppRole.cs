using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BTCA.Common.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public AppRole() : base() { }

        public AppRole(string roleName) : base(roleName) { }               
    }
}
