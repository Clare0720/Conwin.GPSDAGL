using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Conwin.GPSDAGL.Services.Services
{
    public class NopiExcel
    {
        HSSFWorkbook hssfworkbook;
        ISheet currentSheet;
        public void ChangeSheet(int index)
        {
            try
            {
                currentSheet = hssfworkbook.GetSheetAt(index);
                hssfworkbook.SetActiveSheet(index);
            }
            catch
            {
                currentSheet = hssfworkbook.GetSheetAt(0);
                hssfworkbook.SetActiveSheet(0);
            }
        }
        public string CurrentSheetName
        {
            get { return currentSheet.SheetName; }
            set { hssfworkbook.SetSheetName(hssfworkbook.ActiveSheetIndex, value); }
        }
        /// <summary>
        /// 新建立一个EXCEL操作
        /// </summary>
        public NopiExcel()
        {
            hssfworkbook = new HSSFWorkbook();
            currentSheet = hssfworkbook.CreateSheet("Sheet1");
            hssfworkbook.CreateSheet("Sheet2");
            hssfworkbook.CreateSheet("Sheet3");
        }
        /// <summary>
        /// 新建立一个EXCEL,无任何操作
        /// </summary>
        public NopiExcel(int type, string SheetName = "Sheet1")
        {
            hssfworkbook = new HSSFWorkbook();
            if (type == 1)
            {
                currentSheet = hssfworkbook.CreateSheet(SheetName);
            }
        }
        //创建新的Excel表格，并且光标移到到新表格中
        public void SetCurrentCreateNewSheet(string SheetName)
        {
            currentSheet = hssfworkbook.CreateSheet(SheetName);
        }
        public void SetCurrentSheet(string SheetName)
        {
            currentSheet = hssfworkbook.GetSheet(SheetName);
        }
        public void SetSheetName(string SheetName)
        {
            hssfworkbook.SetSheetName(hssfworkbook.GetSheetIndex(currentSheet), SheetName);
        }
        /// <summary>
        /// 读取模板
        /// </summary>
        /// <param name="path"></param>
        public NopiExcel(string path)
        {
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
            hssfworkbook = new HSSFWorkbook(file);
            currentSheet = hssfworkbook.GetSheetAt(0);
            //DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            //dsi.Company = "";
            //hssfworkbook.DocumentSummaryInformation = dsi;
            //SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            //si.Subject = "";
            //hssfworkbook.SummaryInformation = si;
        }
        public void Save(string path)
        {
            FileStream file = new FileStream(path, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
        }
        public byte[] SaveToBinary()
        {
            byte[] temp;
            using (MemoryStream stream = new MemoryStream())
            {
                hssfworkbook.Write(stream);
                temp = new byte[stream.Length];
                temp = stream.GetBuffer();
            }
            return temp;
        }
        public void SetVerticalCenter(int startRow, int startColumn, int rowCount, int columnCount)
        {
            ICellStyle cellStyle = SetStyle(startRow, startColumn, rowCount, columnCount);
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
        }
        public void SetCellFormulaString(int Rows, int ColumnS, string FormulaString)
        {
            //CreateCell(Rows, ColumnS);
            HSSFSheet sheet1 = (HSSFSheet)hssfworkbook.GetSheetAt(0);
            HSSFRow row1 = (HSSFRow)sheet1.CreateRow(0);
            HSSFCell cell = (HSSFCell)row1.CreateCell(ColumnS);
            cell.SetCellFormula(FormulaString);
            //currentSheet.GetRow(Rows).GetCell(ColumnS).SetCellFormula(FormulaString);
            //currentSheet.ForceFormulaRecalculation = true;
        }
        public void SetHorizontalCenter(int startRow, int startColumn, int rowCount, int columnCount)
        {
            ICellStyle cellStyle = SetStyle(startRow, startColumn, rowCount, columnCount);
            cellStyle.Alignment = HorizontalAlignment.Center;
        }
        public void SetHorizontalLeft(int startRow, int startColumn, int rowCount, int columnCount)
        {
            ICellStyle cellStyle = SetStyle(startRow, startColumn, rowCount, columnCount);
            cellStyle.Alignment = HorizontalAlignment.Left;
        }
        public void SetFontFamliy(int startRow, int startColumn, int rowCount, int columnCount, short fontSize, string FontName, string ColorType, short boldweight)
        {
            ICellStyle cellStyle = hssfworkbook.CreateCellStyle();
            IFont font = hssfworkbook.CreateFont();
            if (fontSize != 0)
            {
                font.FontHeight = fontSize;
            }
            if (FontName != "")
            {
                font.FontName = FontName;
            }
            if (ColorType != "")
            {
                switch (ColorType)
                {
                    case "1":
                        font.Color = HSSFColor.Red.Index;
                        break;
                    case "2":
                        font.Color = HSSFColor.Green.Index;
                        break;
                    case "3":
                        font.Color = HSSFColor.Blue.Index;
                        break;
                    default:
                        font.Color = HSSFColor.Yellow.Index;
                        break;
                }
            }
            if (boldweight != 0)
            {
                font.Boldweight = boldweight;
            }
            cellStyle.SetFont(font);
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    currentSheet.GetRow(startRow + i).GetCell(startColumn + j).CellStyle = cellStyle;
                }
            }
        }
        public void SetFontSize(int startRow, int startColumn, int rowCount, int columnCount, short fontSize)
        {
            ICellStyle cellStyle = SetStyle(startRow, startColumn, rowCount, columnCount);
            IFont font = hssfworkbook.CreateFont();
            font.FontHeight = fontSize;
            cellStyle.SetFont(font);
            //for (int i = 0; i < rowCount; i++)
            //{
            //    for (int j = 0; j < columnCount; j++)
            //    {
            //        currentSheet.GetRow(startRow + i).GetCell(startColumn + j).CellStyle = cellStyle;
            //    }
            //}
        }
        public void SetFontName(int startRow, int startColumn, int rowCount, int columnCount, string FontName)
        {
            ICellStyle cellStyle = SetStyle(startRow, startColumn, rowCount, columnCount);
            IFont font = hssfworkbook.CreateFont();
            font.FontName = FontName;
            cellStyle.SetFont(font);
        }
        public void SetCellLock(int startRow, int startColumn, int rowCount, int columnCount, bool Locked)
        {
            ICellStyle cellStyle = SetStyle(startRow, startColumn, rowCount, columnCount);
            cellStyle.IsLocked = Locked;
        }
        public void SetFontColor(int startRow, int startColumn, int rowCount, int columnCount, string ColorType)
        {
            ICellStyle cellStyle = SetStyle(startRow, startColumn, rowCount, columnCount);
            IFont font = hssfworkbook.CreateFont();
            switch (ColorType)
            {
                case "1":
                    font.Color = HSSFColor.Red.Index;
                    break;
                case "2":
                    font.Color = HSSFColor.Green.Index;
                    break;
                case "3":
                    font.Color = HSSFColor.Blue.Index;
                    break;
                default:
                    font.Color = HSSFColor.Yellow.Index;
                    break;
            }
            cellStyle.SetFont(font);
            //for (int i = 0; i < rowCount; i++)
            //{
            //    for (int j = 0; j < columnCount; j++)
            //    {
            //        currentSheet.GetRow(startRow + i).GetCell(startColumn + j).CellStyle = cellStyle;
            //    }
            //}
        }
        public void SetFontWeight(int startRow, int startColumn, int rowCount, int columnCount, short boldweight)
        {
            ICellStyle cellStyle = SetStyle(startRow, startColumn, rowCount, columnCount);
            IFont font = hssfworkbook.CreateFont();
            font.Boldweight = boldweight;
            cellStyle.SetFont(font);
            //for (int i = 0; i < rowCount; i++)
            //{
            //    for (int j = 0; j < columnCount; j++)
            //    {
            //        currentSheet.GetRow(startRow + i).GetCell(startColumn + j).CellStyle = cellStyle;
            //    }
            //}
        }
        public void SetCellFormat(int startRow, int startColumn, int rowCount, int columnCount)
        {
            IFont font = hssfworkbook.CreateFont();
            ICellStyle cellStyle = SetStyle(startRow, startColumn, rowCount, columnCount);
            cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("0.00");
            cellStyle.SetFont(font);
        }

        public void SetCellTextFormat(int startRow, int startColumn, int rowCount, int columnCount)
        {
            IFont font = hssfworkbook.CreateFont();
            ICellStyle cellStyle = SetStyle(startRow, startColumn, rowCount, columnCount);
            cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");
            cellStyle.SetFont(font);
        }

        public void WrapText(int startRow, int startColumn, int rowCount, int columnCount)
        {
            ICellStyle cellStyle = SetStyle(startRow, startColumn, rowCount, columnCount);
            cellStyle.WrapText = true;
        }
        public void SetFont(int startRow, int startColumn, int rowCount, int columnCount, short fontSize, bool IsBold)
        {
            ICellStyle cellStyle = SetStyle(startRow, startColumn, rowCount, columnCount);
            IFont font = hssfworkbook.CreateFont();
            font.FontHeight = fontSize * 20;
            if (IsBold == true)
            {
                font.Boldweight = (short)FontBoldWeight.Bold;
            }

            cellStyle.SetFont(font);
        }
        public void SetCellRangeAddress(int rowstart, int rowend, int colstart, int colend)
        {
            CellRangeAddress cellRangeAddress = new CellRangeAddress(rowstart, rowend, colstart, colend);
            currentSheet.AddMergedRegion(cellRangeAddress);

        }
        public void DrawThinBorder(int startRow, int startColumn, int rowCount, int columnCount)
        {
            ICellStyle cellStyle = SetStyle(startRow, startColumn, rowCount, columnCount);
            cellStyle.BorderBottom = cellStyle.BorderLeft = cellStyle.BorderRight = cellStyle.BorderTop = BorderStyle.Thin;
        }
        public void DrawFatBorderRight(int startRow, int startColumn, int rowCount, int columnCount)
        {
            ICellStyle cellStyle = SetStyle(startRow, startColumn, rowCount, columnCount);
            cellStyle.BorderRight = BorderStyle.Medium;
        }
        private ICellStyle SetStyle(int startRow, int startColumn, int rowCount, int columnCount)
        {
            ICellStyle cellStyle = hssfworkbook.CreateCellStyle();
            CreateCell(startRow, startColumn);
            cellStyle.CloneStyleFrom(currentSheet.GetRow(startRow).GetCell(startColumn).CellStyle);
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    CreateCell(startRow + i, startColumn + j);
                    currentSheet.GetRow(startRow + i).GetCell(startColumn + j).CellStyle = cellStyle;
                }
            }
            return cellStyle;
        }
        public void SetBackgroundColor(int startRow, int startColumn, int rowCount, int columnCount, string ColorType)
        {
            ICellStyle cellStyle = hssfworkbook.CreateCellStyle();
            //cellStyle.FillBackgroundColor = HSSFColor.RED.index;
            //cellStyle.FillForegroundColor = HSSFColor.WHITE.index;
            cellStyle.FillPattern = FillPattern.SolidForeground;
            switch (ColorType)
            {
                case "1":
                    cellStyle.FillForegroundColor = HSSFColor.Red.Index;
                    break;
                case "2":
                    cellStyle.FillForegroundColor = HSSFColor.LightYellow.Index;
                    break;
                case "3":
                    cellStyle.FillForegroundColor = HSSFColor.LightGreen.Index;
                    break;
                case "4":
                    cellStyle.FillForegroundColor = HSSFColor.LightBlue.Index;
                    break;
                default:
                    cellStyle.FillForegroundColor = HSSFColor.White.Index;
                    break;
            }
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    currentSheet.GetRow(startRow + i).GetCell(startColumn + j).CellStyle = cellStyle;
                }
            }
        }
        public void Merge(int startRow, int startColumn, int endRow, int endColumn)
        {
            currentSheet.AddMergedRegion(new CellRangeAddress(startRow, endRow, startColumn, endColumn));
        }
        public void SetColumnWidth(int startColumn, int columnCount, int Width)
        {
            for (int i = 0; i < columnCount; i++)
            {
                currentSheet.SetColumnWidth(startColumn + i, Width);
                SetAutoColumnForCh(startColumn + i);
            }
        }
        /// <summary>
        /// 设置列自适应(中文)
        /// </summary>
        /// <param name="columnNum">索引从0开始</param>
        public void SetAutoColumnForCh(int columnNum)
        {
            int columnWidth = currentSheet.GetColumnWidth(columnNum) / 256;
            for (int rowNum = 1; rowNum <= currentSheet.LastRowNum; rowNum++)
            {
                IRow currentRow = currentSheet.GetRow(rowNum);
                if (currentRow != null && currentRow.GetCell(columnNum) != null)
                {
                    ICell currentCell = currentRow.GetCell(columnNum);
                    int length = Encoding.Default.GetBytes(currentCell.ToString()).Length;
                    if (!currentCell.IsMergedCell && columnWidth < length)
                    {
                        columnWidth = length;
                    }
                }
            }
            currentSheet.SetColumnWidth(columnNum, columnWidth * 256);
        }
        public void SetCellHeight(int startRow, int rowCount, short Height)
        {
            for (int i = 0; i < rowCount; i++)
            {
                currentSheet.GetRow(startRow + i).Height = Height;
            }
        }
        /// <summary>
        /// 创建未存在的单元格
        /// </summary>
        /// <param name="startRow"></param>
        /// <param name="startColumn"></param>
        private void CreateCell(int startRow, int startColumn)
        {
            if (currentSheet.GetRow(startRow) == null)
            {
                currentSheet.CreateRow(startRow);
            }
            if (currentSheet.GetRow(startRow).GetCell(startColumn) == null)
            {
                currentSheet.GetRow(startRow).CreateCell(startColumn);
            }
        }
        public void WriteCell(int startRow, int startColumn, double value)
        {
            CreateCell(startRow, startColumn);
            currentSheet.GetRow(startRow).GetCell(startColumn).SetCellValue(value);
        }
        public void WriteCell(int startRow, int startColumn, bool value)
        {
            CreateCell(startRow, startColumn);
            currentSheet.GetRow(startRow).GetCell(startColumn).SetCellValue(value);
        }
        public void WriteCell(int startRow, int startColumn, DateTime value)
        {
            CreateCell(startRow, startColumn);
            currentSheet.GetRow(startRow).GetCell(startColumn).SetCellValue(value);
        }
        public void WriteCell(int rowNum, int cellNum, string value)
        {
            if (value == null || value.ToLower() == "null")
            {
                value = "";
            }
            CreateCell(rowNum, cellNum);
            currentSheet.GetRow(rowNum).GetCell(cellNum).SetCellValue(value);
        }
        public void WriteTitleRow(List<ExcelEntityMap> mappingList)
        {
            List<string> list = new List<string>();
            foreach (ExcelEntityMap map in mappingList)
                list.Add(map.DestinaName);
            this.InsertRow(0, 0, list.ToArray());
        }
        public void WriteCellFormula(int startRow, int startColumn, string FormulaString)
        {
            CreateCell(startRow, startColumn);
            currentSheet.GetRow(startRow).GetCell(startColumn).SetCellType(CellType.Formula);
            currentSheet.GetRow(startRow).GetCell(startColumn).CellFormula = FormulaString;
        }
        public void SetCellFormulaRecalculation()
        {
            currentSheet.ForceFormulaRecalculation = true;
        }
        /// <summary>
        /// 移动ROW行
        /// </summary>
        /// <param name="startRow"></param>
        /// <param name="endRow"></param>
        /// <param name="row"></param>
        public void ShiftRows(int startRow, int endRow, int row)
        {
            //currentSheet.ShiftRows(startRow, endRow, row, true, true);
            //currentSheet.ShiftRows(startRow, endRow, row, false, false);
            currentSheet.ShiftRows(startRow, endRow, row);
        }
        public void ShiftAllRows(int startRow, int row)
        {
            currentSheet.ShiftRows(startRow, currentSheet.LastRowNum, row);
        }
        /// <summary>
        /// 插入1行
        /// </summary>
        /// <param name="startRow"></param>
        /// <param name="startColumn"></param>
        /// <param name="cellValue"></param>
        public void InsertRow(int startRow, int startColumn, string[] cellValue)
        {
            currentSheet.ShiftRows(startRow, currentSheet.LastRowNum, 1);
            for (int i = 0; i < cellValue.Length; i++)
            {
                CreateCell(startRow, startColumn + i);
                currentSheet.GetRow(startRow).GetCell(startColumn + i).SetCellValue(cellValue[i]);
            }
        }
        public void Insertrow(int startRow, int startColumn, string[] cellValue)
        {
            currentSheet.ShiftRows(startRow, currentSheet.LastRowNum, 0);
            for (int i = 0; i < cellValue.Length; i++)
            {
                CreateCell(startRow, startColumn + i);
                currentSheet.GetRow(startRow).GetCell(startColumn + i).SetCellValue(cellValue[i]);
            }
        }
        public void CreateRow(int startRow, int startColumn, int rowCount, int columnCount)
        {
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    currentSheet.CreateRow(startRow + i).CreateCell(startColumn + j);
                }
            }
        }
        public void CopyRow(int startRow, int startColumn, string[] cellValue)
        {
            currentSheet.ShiftRows(startRow, currentSheet.LastRowNum, 1);
            IRow IRow = currentSheet.GetRow(startRow + 1);
            for (int i = 0; i < cellValue.Length; i++)
            {
                CreateCell(startRow, startColumn + i);
                currentSheet.GetRow(startRow).GetCell(startColumn + i).CellStyle = IRow.GetCell(startColumn + i).CellStyle;
            }
        }
        private string GetCellValue(ICell cell)
        {
            if (cell == null)
                return string.Empty;
            switch (cell.CellType)
            {
                case CellType.Blank:
                    return string.Empty;
                case CellType.Boolean:
                    return cell.BooleanCellValue.ToString();
                case CellType.Error:
                    return cell.ErrorCellValue.ToString();
                case CellType.Numeric:
                case CellType.Unknown:
                default:
                    return cell.ToString();//This is a trick to get the correct value of the cell. NumericCellValue will return a numeric value no matter the cell value is a date or a number
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Formula:
                    try
                    {
                        HSSFFormulaEvaluator e = new HSSFFormulaEvaluator(cell.Sheet.Workbook);
                        e.EvaluateInCell(cell);
                        return cell.ToString();
                    }
                    catch
                    {
                        return cell.NumericCellValue.ToString();
                    }
            }
        }
        private DataTable RenderFromExcel(ISheet sheet, int headerRowIndex)
        {
            DataTable table = new DataTable();
            IRow headerRow = sheet.GetRow(headerRowIndex);
            int cellCount = headerRow.LastCellNum;//LastCellNum = PhysicalNumberOfCells
            int rowCount = sheet.LastRowNum;//LastRowNum = PhysicalNumberOfRows - 1
                                            //handling header.
            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
            {
                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                table.Columns.Add(column);
            }
            for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = table.NewRow();
                if (row != null)
                {
                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                            dataRow[j] = GetCellValue(row.GetCell(j));
                    }
                }
                table.Rows.Add(dataRow);
            }
            return table;
        }
        /// <summary>
        /// Excel文档流转换成DataTable
        /// </summary>
        /// <param name="excelFileStream">Excel文档流</param>
        /// <param name="sheetIndex">表索引号，如第一个表为0</param>
        /// <param name="headerRowIndex">标题行索引号，如第一行为0</param>
        /// <returns></returns>
        public DataTable RenderFromExcel(Stream excelFileStream, int sheetIndex, int headerRowIndex)
        {
            DataTable table = null;
            using (excelFileStream)
            {
                IWorkbook workbook = new HSSFWorkbook(excelFileStream);
                ISheet sheet = workbook.GetSheetAt(sheetIndex);
                table = RenderFromExcel(sheet, headerRowIndex);
            }
            return table;
        }
        /// <summary>
        /// Excel文档流是否有数据
        /// </summary>
        /// <param name="excelFileStream">Excel文档流</param>
        /// <returns></returns>
        public bool HasData(Stream excelFileStream)
        {
            return HasData(excelFileStream, 0);
        }
        public bool HasData(Stream excelFileStream, int sheetIndex)
        {
            using (excelFileStream)
            {
                IWorkbook workbook = new HSSFWorkbook(excelFileStream);
                if (workbook.NumberOfSheets > 0)
                {
                    if (sheetIndex < workbook.NumberOfSheets)
                    {
                        ISheet sheet = workbook.GetSheetAt(sheetIndex);
                        return sheet.PhysicalNumberOfRows > 0;
                    }
                }
            }
            return false;
        }
        public byte[] GetByte(string url)
        {
            byte[] fileBytes = null;
            Stream stream = null;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            stream = response.GetResponseStream();
            response.Close();
            fileBytes = new byte[stream.Length];
            stream.Read(fileBytes, 0, fileBytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            return fileBytes;
        }
        /// <summary>
        /// 获取列数
        /// </summary>
        /// <returns></returns>
        public int getLastCellNum(int startRow)
        {
            return currentSheet.GetRow(startRow).LastCellNum;
        }
        /// <summary>
        /// 获取行数
        /// </summary>
        /// <returns></returns>
        public int getLastRowNum()
        {
            return currentSheet.LastRowNum + 1;
        }
    }
}




