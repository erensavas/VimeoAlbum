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
                var fileAdi = new DirectoryInfo(fileName).Name;

                var memory = new MemoryStream();

                using (var fs = new FileStream(Path.Combine(fileName+"\\aaa-" + fileAdi+".xlsx"), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("Videolar");

                    //IRow row = excelSheet.CreateRow(0);
                    //row.CreateCell(0).SetCellValue(model[0].VideoName);
                    //row.CreateCell(0).SetCellValue("ID");
                    //row.CreateCell(1).SetCellValue("Name");
                    IRow row;

                    //foreach (var item in model)
                    //{
                    //    row = excelSheet.CreateRow(excelSheet.LastRowNum+1);
                    //    row.CreateCell(0).SetCellValue(item.VideoName);
                    //    excelSheet.SetColumnWidth(0, 2000);
                      
                    //}

                    for (int i = 0; i < model.Count; i++)
                    {
                        // row = excelSheet.CreateRow(excelSheet.LastRowNum + 1);
                        row = excelSheet.CreateRow(i);
                        row.CreateCell(0).SetCellValue(model[i].VideoName);
                      
                
                    }

                    excelSheet.SetColumnWidth(0, 15000);
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

        public async static Task<bool> excelExportKaydet(string fileName, List<AlbumOlusturModel> model)
        {
            try
            {
                //var fileAdi = new DirectoryInfo(fileName).Name;

                var memory = new MemoryStream();

                using (var fs = new FileStream(Path.Combine(fileName +  ".xlsx"), FileMode.Create, FileAccess.Write))
                {
                    IWorkbook workbook = new XSSFWorkbook();
                    ISheet excelSheet = workbook.CreateSheet("AlbumId");

                    //IRow row = excelSheet.CreateRow(0);
                    //row.CreateCell(0).SetCellValue(model[0].VideoName);
                    //row.CreateCell(0).SetCellValue("ID");
                    //row.CreateCell(1).SetCellValue("Name");
                    IRow row;

                    //foreach (var item in model)
                    //{
                    //    row = excelSheet.CreateRow(excelSheet.LastRowNum+1);
                    //    row.CreateCell(0).SetCellValue(item.VideoName);
                    //    excelSheet.SetColumnWidth(0, 2000);

                    //}

                    for (int i = 0; i < model.Count; i++)
                    {
                        // row = excelSheet.CreateRow(excelSheet.LastRowNum + 1);
                        row = excelSheet.CreateRow(i);
                        row.CreateCell(0).SetCellValue(model[i].id);
                        row.CreateCell(1).SetCellValue(model[i].Name);


                    }

                    excelSheet.SetColumnWidth(0, 3500);
                    excelSheet.SetColumnWidth(1, 10000);
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
