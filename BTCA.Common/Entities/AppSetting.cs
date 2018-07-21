using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BTCA.Common.Core;

namespace BTCA.Common.Entities
{
    public class AppSetting : BaseEntity
    {
        [Key]
        public virtual int AppSettingID { get; set; }
        [Required, MaxLength(30), Display(Name = "Key")]
        public virtual string SettingsKey { get; set; }
        [Required, MaxLength(30), Display(Name = "Value")]
        public virtual string SettingsValue { get; set; }
        [MaxLength(50)]
        public virtual string Description { get; set; }
    }
}
