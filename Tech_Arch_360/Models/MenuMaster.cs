﻿using System;
using System.Collections.Generic;

namespace Tech_Arch_360.Models
{
    public partial class MenuMaster
    {

        public MenuMaster()
        {
            RoleMenuMasters = new HashSet<RoleMenuMaster>();
        }

        public int MenuId { get; set; }
        public string? MenuName { get; set; }
        public bool? IsParent { get; set; }
        public int? ParentMenuId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public bool? IsActive { get; set; }
        public string? Link { get; set; }

        public virtual ICollection<RoleMenuMaster> RoleMenuMasters { get; set; }
        
    }
}
