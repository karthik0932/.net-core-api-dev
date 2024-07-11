using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tech_Arch_360.Models;

namespace Tech_Arch_360.Services
{
    public class MenuService
    {
        private readonly Tech_Arc_360Context _context;

        public MenuService(Tech_Arc_360Context context)
        {
            _context = context;
        }

        public async Task<List<Menu>> GetMenusByUserId(int userId)
        {
            // Fetch menus from the database
            var menus = await _context.MenuMasters
                .Where(m => m.CreatedBy == userId || m.ModifiedBy == userId)
                .ToListAsync(); // Fetch the data from the database first

            // Apply the nullable boolean check on the client side
            menus = menus
                .Where(m => m.IsActive.GetValueOrDefault())
                .ToList();

            // Organize the menus into a hierarchical structure
            var menuDict = menus.ToDictionary(m => m.MenuId);

            var mainMenus = new List<Menu>();

            foreach (var menu in menus)
            {
                if ((bool)menu.IsParent)
                {
                    mainMenus.Add(new Menu
                    {
                        Id = menu.MenuId,
                        Name = menu.MenuName,
                        Link = "/link/" + menu.MenuName.Replace(" ", "-"),
                        SubMenus = new List<SubMenu>()
                    });
                }
                else if (menu.ParentMenuId.HasValue)
                {
                    if (menuDict.TryGetValue(menu.ParentMenuId.Value, out var parentMenu))
                    {
                        var parent = mainMenus.FirstOrDefault(m => m.Id == parentMenu.MenuId);
                        parent?.SubMenus.Add(new SubMenu
                        {
                            Id = menu.MenuId,
                            Name = menu.MenuName,
                            Link = "/link/" + menu.MenuName.Replace(" ", "-")
                        });
                    }
                }
            }

            return mainMenus;
        }
    }
}
