using System;
using System.Collections.Generic;

namespace Tech_Arch_360.Models
{
    public partial class RoleMenuMaster
    {
        public int RoleMenuId { get; set; }
        public int? RoleId { get; set; }
        public int? MenuId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public bool? IsActive { get; set; }
    }
}
