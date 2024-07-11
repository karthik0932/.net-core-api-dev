using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tech_Arch_360.Models;
using Tech_Arch_360.Services;

public class MenuController : ControllerBase
{
    private readonly MenuService _menuService;

    public MenuController(MenuService menuService)
    {
        _menuService = menuService;
    }

    [HttpGet("api/menus/{userId}")]
    public async Task<IActionResult> GetMenus(int userId)
    {
        var menus = await _menuService.GetMenusByUserId(userId);
        if (menus == null || !menus.Any())
        {
            return NotFound();
        }

        // Projecting necessary properties without including $id
        var result = new
        {
            menulist = menus.Select(m => new { menuid = m.Id, name = m.Name, link = m.Link }),
            submenu = menus.SelectMany(m => m.SubMenus.Select(sm => new { maninmenuid = m.Id, name = sm.Name, link = sm.Link }))
        };

        return Ok(result);
    }
}




public class Menu
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Link { get; set; }
    public List<SubMenu> SubMenus { get; set; }
}

public class SubMenu
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Link { get; set; }
}
