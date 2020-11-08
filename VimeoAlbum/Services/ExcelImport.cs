using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VimeoAlbum.Models;

namespace VimeoAlbum.Services
{
    public class ExcelImport
    {
        public static List<ExcelModel> procExcelVimeo(string fileName)
        {
            List<ExcelModel> liste = new List<ExcelModel>();
            try
            {

                IWorkbook workbook = null;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                if (fileName.IndexOf(".xlsx") > 0)
                    workbook = new XSSFWorkbook(fs);
                else if (fileName.IndexOf(".xls") > 0)
                    workbook = new HSSFWorkbook(fs);
                //First sheet
                ISheet sheet = workbook.GetSheetAt(0);
                if (sheet != null)
                {
                    int rowCount = sheet.LastRowNum; // This may not be valid row count.
                                                     // If first row is table head, i starts from 1
                    for (int i = 0; i <= rowCount; i++)
                    {
                        IRow curRow = sheet.GetRow(i);
                        //ICell cell = curRow.GetCell(0);
                        //if (cell == null)
                        //{
                        //    cell = curRow.CreateCell(0);
                        //    cell.SetCellType(CellType.String);
                        //    cell.SetCellValue("");
                        //}


                        // Works for consecutive data. Use continue otherwise 
                        if (curRow == null)
                        {
                            // Valid row count
                            rowCount = i - 1;
                            break;
                        }
                        // Get data from the 4th column (4th cell of each row)
                        //liste.Add(curRow.GetCell(satirNumber - 1).ToString().Trim());
                        ExcelModel model = new ExcelModel
                        {
                            VideoName = curRow.GetCell(0).ToString().Trim(),
                            //Test = curRow.GetCell(1).ToString().Trim(),
                            //QrCode = Convert.ToInt64(curRow.GetCell(2)?.ToString().Trim()),
                            //AlbumAdi = curRow.GetCell(3).ToString().Trim()
                        };
                        liste.Add(model);
                    }

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            return liste;
        }

        public static List<ExcelViewModel> procExcelOrbim(string fileName)
        {
            List<ExcelViewModel> liste = new List<ExcelViewModel>();
            try
            {

                IWorkbook workbook = null;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                if (fileName.IndexOf(".xlsx") > 0)
                    workbook = new XSSFWorkbook(fs);
                else if (fileName.IndexOf(".xls") > 0)
                    workbook = new HSSFWorkbook(fs);
                //First sheet
                ISheet sheet = workbook.GetSheetAt(0);
                if (sheet != null)
                {
                    int rowCount = sheet.LastRowNum; // This may not be valid row count.
                                                     // If first row is table head, i starts from 1
                    for (int i = 0; i <= rowCount; i++)
                    {
                        IRow curRow = sheet.GetRow(i);
                        //ICell cell = curRow.GetCell(0);
                        //if (cell == null)
                        //{
                        //    cell = curRow.CreateCell(0);
                        //    cell.SetCellType(CellType.String);
                        //    cell.SetCellValue("");
                        //}


                        // Works for consecutive data. Use continue otherwise 
                        if (curRow == null)
                        {
                            // Valid row count
                            rowCount = i - 1;
                            break;
                        }
                        // Get data from the 4th column (4th cell of each row)
                        //liste.Add(curRow.GetCell(satirNumber - 1).ToString().Trim());
                        if (curRow.GetCell(3)?.ToString().Trim()  != null)
                        {
                            ExcelViewModel model = new ExcelViewModel
                            {
                                Unite = curRow.GetCell(0)?.ToString().Trim(),
                                Test = curRow.GetCell(1).ToString().Trim(),
                                QrCode = Convert.ToInt64(curRow.GetCell(2)?.ToString().Trim()),
                                AlbumAdi = curRow.GetCell(3).ToString().Trim()
                            };
                            liste.Add(model);
                        }
                       
                    }

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            return liste;
        }



    }
}
