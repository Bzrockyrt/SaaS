using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using Microsoft.AspNetCore.Mvc;
using DocumentFormat.OpenXml.Spreadsheet;
using SaaS.DataAccess.Repository;
using SaaS.Domain.Work;
using SaaS.DataAccess.Repository.IRepository;
using SaaS.Domain.Identity;

namespace SaaS.Areas.Application.Controllers
{
    [Area("Application")]
    public class AdministrationController : Controller
    {
        private readonly IApplicationUnitOfWork applicationUnitOfWork;

        public AdministrationController(IApplicationUnitOfWork applicationUnitOfWork)
        {
            this.applicationUnitOfWork = applicationUnitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region APICALLS
        public IActionResult ExportToExcel()
        {
            string filePath = "C:\\Users\\pierr\\OneDrive\\PIPL Développement\\Application\\Développement\\Test.xlsx";

            /*using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(filePath, true))
            {
                var workbookPart = spreadsheetDocument.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                var sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild(new Sheets());

                var sheet = new Sheet()
                {
                    Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "NomDeLaFeuille"
                };
                sheets.Append(sheet);

                var worksheet = worksheetPart.Worksheet;
                var sheetData = worksheet.GetFirstChild<SheetData>();

                var headerRow = new Row();

                headerRow.Append(CreateCell("Employé"));
                headerRow.Append(CreateCell("Jour"));
                headerRow.Append(CreateCell("Début matin"));
                headerRow.Append(CreateCell("Fin matin"));
                headerRow.Append(CreateCell("Début après-midi"));
                headerRow.Append(CreateCell("Fin après-midi"));
                headerRow.Append(CreateCell("Chantier(s)"));

                sheetData.Append(headerRow);

                foreach (var item in workHours)
                {
                    var dataRow = new Row();

                    User user = this.applicationUnitOfWork.User.Get(u => u.Id == item.UserId);
                    List<WorkSite> worksites = this.applicationUnitOfWork.WorkHour_WorkSite.GetAll().Where(ww => ww.WorkHourId == item.Id).Select(ws => ws.WorkSite).ToList();
                    string allworksites = "";

                    foreach (var worksite in worksites)
                    {
                        allworksites = allworksites + worksite.Name;
                    }

                    dataRow.Append(CreateCell(user.Fullname));
                    dataRow.Append(CreateCell(item.WorkDay.ToString("dd/MM/yyyy")));
                    dataRow.Append(CreateCell(item.MorningStart.ToString()));
                    dataRow.Append(CreateCell(item.MorningEnd.ToString()));
                    dataRow.Append(CreateCell(item.EveningStart.ToString()));
                    dataRow.Append(CreateCell(item.EveningEnd.ToString()));

                    sheetData.Append(dataRow);
                }
            }*/
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = spreadsheetDocument.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>("rId1");
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild(new Sheets());

                Sheet sheet = new Sheet() { Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet1" };
                sheets.Append(sheet);

                Worksheet worksheet = worksheetPart.Worksheet;
                SheetData sheetData = worksheet.GetFirstChild<SheetData>();

                Cell cell = InsertCellInWorksheet("A", 1, sheetData);
                cell.CellValue = new CellValue("Hello, Excel!");
                cell.DataType = new EnumValue<CellValues>(CellValues.String);

                Cell cellB = InsertCellInWorksheet("B", 1, sheetData);
                cellB.CellValue = new CellValue("42");
                cellB.DataType = new EnumValue<CellValues>(CellValues.Number);
                
                worksheetPart.Worksheet.Save();
                return View();
            }
        }

        private static Cell InsertCellInWorksheet(string columnName, uint rowIndex, SheetData sheetData)
        {
            string cellReference = columnName + rowIndex;

            // Si la cellule existe déjà, retourne-la
            if (sheetData.Elements<Cell>().Where(c => c.CellReference.Value == cellReference).Count() != 0)
            {
                return sheetData.Elements<Cell>().Where(c => c.CellReference.Value == cellReference).First();
            }
            else
            {
                // Sinon, crée la cellule et ajoute-la à la feuille
                Cell cell = new Cell() { CellReference = cellReference };
                sheetData.Append(cell);
                return cell;
            }
        }
        #endregion
    }

    public class TOTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
