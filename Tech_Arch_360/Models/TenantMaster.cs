using System;
using System.Collections.Generic;

namespace Tech_Arch_360.Models
{
    public partial class TenantMaster
    {
        public TenantMaster()
        {
            UserMasters = new HashSet<UserMaster>();
        }

        public int TenantId { get; set; }
        public string? TenantName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<UserMaster> UserMasters { get; set; }
    }
}
