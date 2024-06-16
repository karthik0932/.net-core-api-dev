using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tech_Arch_360.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Tech_Arch_360.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryQuestionnairesController : ControllerBase
    {
        private readonly Tech_Arc_360Context _context;
        private readonly ExcelService _excelService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public InventoryQuestionnairesController(Tech_Arc_360Context context, ExcelService excelService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _excelService = excelService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("upload")]
        [Authorize]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Please upload a valid Excel file.");

            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var username = _httpContextAccessor.HttpContext.User.Identity.Name;
                var questionnaires = _excelService.ReadExcelFile(filePath, username);

                _context.InventoryQuestionnaires.AddRange(questionnaires);
                await _context.SaveChangesAsync();

                return Ok("File uploaded and data saved successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error uploading file: {ex.Message}");
            }
        }

        [HttpPost("json")]
        [Authorize]
        public async Task<IActionResult> StoreJsonData([FromBody] List<InventoryQuestionnaireCreateModel> models)
        {
            if (models == null || models.Count == 0)
                return BadRequest("No JSON data provided or empty list.");

            try
            {
                var username = _httpContextAccessor.HttpContext.User.Identity.Name;

                List<InventoryQuestionnaire> questionnaires = new List<InventoryQuestionnaire>();

                foreach (var model in models)
                {
                    var questionnaire = new InventoryQuestionnaire
                    {
                        Question = model.Question,
                        Answer = model.Answer,
                        Instructions = model.Instructions,
                        JsonData = model.JsonData,
                        CreatedBy = username,
                        CreatedOn = DateTime.UtcNow
                    };

                    questionnaires.Add(questionnaire);
                }

                _context.InventoryQuestionnaires.AddRange(questionnaires);
                await _context.SaveChangesAsync();

                return Ok("JSON data saved successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error saving JSON data: {ex.Message}");
            }
        }

        [HttpGet("user-data/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetUserQuestionnaires(int userId)
        {
            try
            {
                var questionnaires = await _context.InventoryQuestionnaires
                    .Where(q => q.CreatedBy == userId.ToString()) // Assuming CreatedBy stores user ID as a string
                    .Select(q => new
                    {
                        q.Question,
                        q.Answer,
                        q.Instructions
                    })
                    .ToListAsync();

                if (questionnaires == null || !questionnaires.Any())
                    return NotFound("No data found for the given user ID.");

                return Ok(questionnaires);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error fetching user data: {ex.Message}");
            }
        }

        public class InventoryQuestionnaireCreateModel
        {
            public string Question { get; set; }
            public string Answer { get; set; }
            public string Instructions { get; set; }
            public string JsonData { get; set; }
        }
    }
}
