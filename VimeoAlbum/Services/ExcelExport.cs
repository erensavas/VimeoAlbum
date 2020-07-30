using ICSharpCode.SharpZipLib.Zip;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VimeoAlbum.Models;

namespace VimeoAlbum.Services
{
    public class ExcelExport
    {
     
        public async static Task<bool> excelExportKaydet(string fileName,List<ExcelModel> model)
        {
            try
            {
              
                var memory = new MemoryStream();

                using (var fs = new FileStream(Path.Combine(fileName+"\\Kitap.xlsx"), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("Videolar");

                    IRow row = excelSheet.CreateRow(0);
                    //row.CreateCell(0).SetCellValue("ID");
                    //row.CreateCell(1).SetCellValue("Name");
                  
                    foreach (var item in model)
                    {
                        row = excelSheet.CreateRow(excelSheet.LastRowNum +1);
                        row.CreateCell(0).SetCellValue(item.VideoName);
                        excelSheet.SetColumnWidth(0, 2000);
                      
                    }

                 

                    workbook.Write(fs);

                    return true;

                }

                return false;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
           
        }
    }
}
