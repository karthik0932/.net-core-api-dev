using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tech_Arch_360.Models;
namespace Tech_Arch_360.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryQuestionnairesController : ControllerBase
    {
        private readonly Tech_Arc_360Context _context;
        private readonly ExcelService _excelService;
        private readonly IHttpContextAccessor _httpContextAccessor; // Inject IHttpContextAccessor

        public InventoryQuestionnairesController(Tech_Arc_360Context context, ExcelService excelService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _excelService = excelService;
            _httpContextAccessor = httpContextAccessor; // Assign to local variable
        }

        [HttpPost("upload")]
        [Authorize]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Please upload a valid Excel file.");

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Get authenticated user's name from HttpContext
            var username = _httpContextAccessor.HttpContext.User.Identity.Name;

            var questionnaires = _excelService.ReadExcelFile(filePath, username); // Pass username to service

            _context.InventoryQuestionnaires.AddRange(questionnaires);
            await _context.SaveChangesAsync();

            return Ok("File uploaded and data saved successfully.");
        }
    }
}
