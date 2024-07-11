using System;
using System.Collections.Generic;

namespace Tech_Arch_360.Models
{
    public partial class RoleMaster
    {
        public RoleMaster()
        {
            RoleMenuMasters = new HashSet<RoleMenuMaster>();
            UserMasters = new HashSet<UserMaster>();
        }

        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        public string? ActionIds { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<RoleMenuMaster> RoleMenuMasters { get; set; }
        public virtual ICollection<UserMaster> UserMasters { get; set; }
    }
}
