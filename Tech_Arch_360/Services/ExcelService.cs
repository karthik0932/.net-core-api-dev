using OfficeOpenXml;
using Tech_Arch_360.Models;
public class ExcelService
{
    public List<InventoryQuestionnaire> ReadExcelFile(string filePath, string username)
    {
        var questionnaires = new List<InventoryQuestionnaire>();

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Ensure this is set

        using (var package = new ExcelPackage(new FileInfo(filePath)))
        {
            var worksheet = package.Workbook.Worksheets[0];

            for (int row = 2; row <= worksheet.Dimension.Rows; row++)
            {
                if (int.TryParse(worksheet.Cells[row, 1].Text, out int tenantID))
                {
                    var questionnaire = new InventoryQuestionnaire
                    {
                        TenantId = tenantID,
                        Question = worksheet.Cells[row, 2].Text,
                        Instructions = worksheet.Cells[row, 3].Text,
                        CreatedBy = username, // Set the username passed from the controller
                        CreatedOn = DateTime.Now
                    };

                    questionnaires.Add(questionnaire);
                }
                else
                {
                    // Log or handle the case where TenantID is not a valid integer
                    Console.WriteLine($"Invalid TenantID at row {row}: {worksheet.Cells[row, 1].Text}");
                }
            }
        }

        return questionnaires;
    }
}
