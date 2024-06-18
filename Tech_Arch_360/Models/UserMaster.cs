using System;
using System.Collections.Generic;

namespace Tech_Arch_360.Models
{
    public partial class UserMaster
    {
        public UserMaster()
        {
            InventoryQuestionnaires = new HashSet<InventoryQuestionnaire>();
        }

        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public int? TenantId { get; set; }
        public int? RoleId { get; set; }
        public string? UserEmail { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? IsActive { get; set; }

        public virtual RoleMaster? Role { get; set; }
        public virtual TenantMaster? Tenant { get; set; }
        public virtual ICollection<InventoryQuestionnaire> InventoryQuestionnaires { get; set; }
    }
}
