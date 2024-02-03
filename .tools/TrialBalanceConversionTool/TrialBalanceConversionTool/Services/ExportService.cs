using System.IO;
using System.Drawing;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using TrialBalanceConversionTool.Entities;
using TrialBalanceConversionTool.Interfaces;
using BorderStyle = NPOI.SS.UserModel.BorderStyle;

namespace TrialBalanceConversionTool.Services
{
    public class ExportService: IExportService
    {
        private static int rowIndex;

        public void CreateOutputFile(string localPath, OutputFileData outputFileData)
        {
            rowIndex = 0;
            XSSFWorkbook workbook = CreateWorkbook(outputFileData);
            CreateFile(localPath, workbook);
        }

        private static XSSFWorkbook CreateWorkbook(OutputFileData outputFileData)
        {
            var workbook = new XSSFWorkbook();
            UpdateWorkbook(workbook, outputFileData);
            return workbook;
        }

        private static void UpdateWorkbook(IWorkbook workbook, OutputFileData outputFileData)
        {
            ISheet sheet = workbook.CreateSheet(Constants.OutputFileSheetName);
            CreateHeader(sheet, workbook);
            AddOutputRows(sheet, workbook, outputFileData);
            AddDebitAndCreditTotals(sheet, workbook, outputFileData);
            AddAccountingGroupsTotals(workbook, sheet, outputFileData);
            AutoSizeColumns(sheet);
        }

        private static void AddAccountingGroupsTotals(IWorkbook workbook, ISheet sheet, OutputFileData outputFileData)
        {
            AddEquityGroupTotals(workbook, sheet, outputFileData);
            AddProfitGroupTotals(workbook, sheet, outputFileData);
            AddBalanceGroupTotals(workbook, sheet, outputFileData);
        }

        private static void AddBalanceGroupTotals(IWorkbook workbook, ISheet sheet, OutputFileData outputFileData)
        {
            AddTotal(sheet, workbook, outputFileData.Debits, Constants.TotalDescriptions.Debits);
            AddTotal(sheet, workbook, outputFileData.Credits, Constants.TotalDescriptions.Credits);
            AddTotal(sheet, workbook, outputFileData.BalanceTotal,
                Constants.TotalDescriptions.BalanceTotal);
        }

        private static void AddProfitGroupTotals(IWorkbook workbook, ISheet sheet, OutputFileData outputFileData)
        {
            AddTotal(sheet, workbook, outputFileData.IncomeBalanceTotal, Constants.TotalDescriptions.Income);
            AddTotal(sheet, workbook, outputFileData.ExpensesBalanceTotal,
                Constants.TotalDescriptions.Expenses);
            AddTotal(sheet, workbook, outputFileData.Profit, Constants.TotalDescriptions.Profit);
            AddBlankRow(sheet);
        }

        private static void AddEquityGroupTotals(IWorkbook workbook, ISheet sheet, OutputFileData outputFileData)
        {
            AddTotal(sheet, workbook, outputFileData.AssetsBalanceTotal, Constants.TotalDescriptions.Assets);
            AddTotal(sheet, workbook, outputFileData.LiabilitiesBalanceTotal,
                Constants.TotalDescriptions.Liabilities);
            AddTotal(sheet, workbook, outputFileData.Equity, Constants.TotalDescriptions.Equity);
            AddBlankRow(sheet);
        }

        private static void AutoSizeColumns(ISheet sheet)
        {
            for (var i = 0 ; i < Constants.OutputFileAllColumnsCount; i++)
            {
                sheet.AutoSizeColumn(i);
            }
        }

        private static void CreateHeader(ISheet sheet, IWorkbook workbook)
        {
            IRow row = sheet.CreateRow(rowIndex);
            ICellStyle cellStyle = GetCellStyle(workbook, new XSSFColor(Color.DarkBlue), new XSSFColor(Color.White),
                Constants.HeaderFontName, Constants.HeaderFontSize, isBold: true);
            ICellStyle altCellStyle = GetCellStyle(workbook, new XSSFColor(Color.DeepSkyBlue), new XSSFColor(Color.Black),
                Constants.HeaderFontName, Constants.HeaderFontSize, isBold: true);
            SetCellValueWithStyle(row.CreateCell(Constants.ColumnNumbers.BranchColumnNumber), cellStyle,
                Constants.ColumnHeaders.Branch);
            SetCellValueWithStyle(row.CreateCell(Constants.ColumnNumbers.TypeColumnNumber), cellStyle,
                Constants.ColumnHeaders.Type);
            SetCellValueWithStyle(row.CreateCell(Constants.ColumnNumbers.AccountColumnNumber), altCellStyle,
                Constants.ColumnHeaders.Account);
            SetCellValueWithStyle(row.CreateCell(Constants.ColumnNumbers.SubAccountColumnNumber), altCellStyle,
                Constants.ColumnHeaders.SubAccount);
            SetCellValueWithStyle(row.CreateCell(Constants.ColumnNumbers.DescriptionColumnNumber), altCellStyle,
                Constants.ColumnHeaders.Description);
            SetCellValueWithStyle(row.CreateCell(Constants.ColumnNumbers.BeginningBalanceColumnNumber), altCellStyle,
                Constants.ColumnHeaders.BeginningBalance);
            SetCellValueWithStyle(row.CreateCell(Constants.ColumnNumbers.DebitAmountColumnNumber), altCellStyle,
                Constants.ColumnHeaders.DebitAmount);
            SetCellValueWithStyle(row.CreateCell(Constants.ColumnNumbers.CreditAmountColumnNumber), altCellStyle,
                Constants.ColumnHeaders.CreditAmount);
            SetCellValueWithStyle(row.CreateCell(Constants.ColumnNumbers.EndingBalanceColumnNumber), altCellStyle,
                Constants.ColumnHeaders.EndingBalance);
            rowIndex++;
        }

        private static void SetCellValueWithStyle(ICell cell,  ICellStyle style, string value)
        {
            cell.CellStyle = style;
            cell.SetCellValue(value);
        }

        private static void SetCellValueWithStyle(ICell cell, ICellStyle style, double value)
        {
            cell.CellStyle = style;
            cell.SetCellValue(value);
        }

        private static void AddOutputRows(ISheet sheet, IWorkbook workbook, OutputFileData outputFileData)
        {
            foreach (OutputRow outputRow in outputFileData.OutputRows)
            {
                IRow row = sheet.CreateRow(rowIndex);
                if (outputRow.IsContainError)
                {
                    AddErrorMessageToRow(workbook, row, outputRow);
                }
                else
                {
                    row.CreateCell(Constants.ColumnNumbers.BranchColumnNumber).SetCellValue(outputRow.Branch);
                    row.CreateCell(Constants.ColumnNumbers.TypeColumnNumber).SetCellValue(outputRow.Type);
                    row.CreateCell(Constants.ColumnNumbers.AccountColumnNumber).SetCellValue(outputRow.Account);
                    row.CreateCell(Constants.ColumnNumbers.SubAccountColumnNumber).SetCellValue(outputRow.SubAccount);
                    row.CreateCell(Constants.ColumnNumbers.DescriptionColumnNumber).SetCellValue(outputRow.Description);
                    SetBeginningBalance(outputRow, row, workbook);
                    SetCellValueWithStyle(row.CreateCell(Constants.ColumnNumbers.DebitAmountColumnNumber),
                        GetCellStyle(workbook, GetDoubleValuesFormat(workbook)),
                        outputRow.DebitAmount);
                    SetCellValueWithStyle(row.CreateCell(Constants.ColumnNumbers.CreditAmountColumnNumber),
                        GetCellStyle(workbook, GetDoubleValuesFormat(workbook)),
                        outputRow.CreditAmount);
                    SetEndingBalance(outputRow, row, workbook);
                }
                rowIndex++;
            }
        }

        private static void AddErrorMessageToRow(IWorkbook workbook, IRow row, OutputRow outputRow)
        {
            ICellStyle errorStyle = GetCellStyle(workbook, new XSSFColor(Color.Red), new XSSFColor(Color.Black));
            SetCellValueWithStyle(row.CreateCell(0), errorStyle, outputRow.Branch);
            for (var i = 1; i<Constants.OutputFileAccountingColumnsCount; i++)
            {
                SetCellValueWithStyle(row.CreateCell(i), errorStyle, null);
            }
        }

        private static void SetBeginningBalance(OutputRow outputRow, IRow row, IWorkbook workbook)
        {
            if (outputRow.IsSecondLineRow)
            {
                row.CreateCell(Constants.ColumnNumbers.BeginningBalanceColumnNumber);
            }
            else
            {
                SetCellValueWithStyle(row.CreateCell(Constants.ColumnNumbers.BeginningBalanceColumnNumber),
                    GetCellStyle(workbook, GetDoubleValuesFormat(workbook)),
                    outputRow.BeginningBalance);
            }
        }

        private static void SetEndingBalance(OutputRow outputRow, IRow row, IWorkbook workbook)
        {
            if (outputRow.IsFirstLineRow)
            {
                row.CreateCell(Constants.ColumnNumbers.EndingBalanceColumnNumber);
            }
            else
            {
                SetCellValueWithStyle(row.CreateCell(Constants.ColumnNumbers.EndingBalanceColumnNumber),
                    GetCellStyle(workbook, GetDoubleValuesFormat(workbook)),
                    outputRow.EndingBalance);
            }
        }

        private static void AddDebitAndCreditTotals(ISheet sheet, IWorkbook workbook, OutputFileData outputFileData)
        {
            IRow row = sheet.CreateRow(rowIndex);
            ICellStyle cellStyle = GetCellStyle(workbook, borderTop: BorderStyle.Medium, borderBottom: BorderStyle.Double,
                borderLeft: null, borderRight: null, dataFormat: GetDoubleValuesFormat(workbook),
                fontHeight: Constants.TotalsFontSize, fontName: Constants.DefaultFontName, isBold: true);
            SetCellValueWithStyle(row.CreateCell(Constants.ColumnNumbers.DebitAmountColumnNumber), cellStyle,
                outputFileData.DebitAmountTotal);
            SetCellValueWithStyle(row.CreateCell(Constants.ColumnNumbers.CreditAmountColumnNumber), cellStyle,
                outputFileData.CreditAmountTotal);
            rowIndex++;
        }

        private static void AddTotal(ISheet sheet, IWorkbook workbook, double totalValue, string totalDescription)
        {
            IRow row = sheet.CreateRow(rowIndex);
            SetCellValueWithStyle(row.CreateCell(Constants.ColumnNumbers.CalculatedTotalsColumnNumber),
                GetCellStyle(workbook, GetDoubleValuesFormat(workbook)),
                totalValue);
            SetCellValueWithStyle(row.CreateCell(Constants.ColumnNumbers.TotalsDescriptionColumnNumber),
                GetCellStyle(workbook, Constants.DefaultFontName, Constants.TotalsFontSize, true),
                totalDescription);
            rowIndex++;
        }

        private static void AddBlankRow(ISheet sheet)
        {
            sheet.CreateRow(rowIndex);
            rowIndex++;
        }

        private static void CreateFile(string localPath, IWorkbook workbook)
        {
            using (var fileStream = new FileStream(localPath, FileMode.Create))
            {
                workbook.Write(fileStream);
            }
        }

        private static ICellStyle GetCellStyle(IWorkbook workbook, short dataFormat)
        {
            var cellStyle = (XSSFCellStyle)workbook.CreateCellStyle();
            cellStyle.SetDataFormat(dataFormat);
            return cellStyle;
        }

        private static ICellStyle GetCellStyle(
            IWorkbook workbook,
            XSSFColor backgroundColorIndex,
            XSSFColor fontColorIndex,
            string fontName = Constants.DefaultFontName,
            int fontHeight = Constants.DefaultFontSizeValue,
            bool isBold = false,
            BorderStyle? borderBottom = null,
            BorderStyle? borderTop = null,
            BorderStyle? borderLeft = null,
            BorderStyle? borderRight = null)
        {
            var cellStyle = (XSSFCellStyle)workbook.CreateCellStyle();
            var fontStyle = (XSSFFont)workbook.CreateFont();
            cellStyle.SetFillForegroundColor(backgroundColorIndex);
            cellStyle.FillPattern = FillPattern.SolidForeground;
            fontStyle.SetColor(fontColorIndex);
            UpdateFontStyle(fontName, fontHeight, isBold, fontStyle);
            cellStyle.SetFont(fontStyle);
            SetBorders(borderBottom, borderTop, borderLeft, borderRight, cellStyle);

            return cellStyle;
        }

        private static ICellStyle GetCellStyle(
            IWorkbook workbook,
            string fontName = Constants.DefaultFontName,
            int fontHeight = Constants.DefaultFontSizeValue,
            bool isBold = false)
        {
            var cellStyle = (XSSFCellStyle)workbook.CreateCellStyle();
            SetFontStyleToCell(workbook, fontName, fontHeight, isBold, cellStyle);
            return cellStyle;
        }

        private static ICellStyle GetCellStyle(
            IWorkbook workbook,
            BorderStyle? borderBottom,
            BorderStyle? borderTop,
            BorderStyle? borderLeft,
            BorderStyle? borderRight,
            short dataFormat,
            string fontName = Constants.DefaultFontName,
            int fontHeight = Constants.DefaultFontSizeValue,
            bool isBold = false)
        {
            var cellStyle = (XSSFCellStyle)workbook.CreateCellStyle();
            SetFontStyleToCell(workbook, fontName, fontHeight, isBold, cellStyle);
            cellStyle.SetDataFormat(dataFormat);
            SetBorders(borderBottom, borderTop, borderLeft, borderRight, cellStyle);
            return cellStyle;
        }

        private static void SetFontStyleToCell(IWorkbook workbook,
            string fontName, int fontHeight, bool isBold, XSSFCellStyle cellStyle)
        {
            var fontStyle = (XSSFFont)workbook.CreateFont();
            UpdateFontStyle(fontName, fontHeight, isBold, fontStyle);
            cellStyle.SetFont(fontStyle);
        }

        private static void SetBorders(
            BorderStyle? borderBottom,
            BorderStyle? borderTop,
            BorderStyle? borderLeft,
            BorderStyle? borderRight,
            ICellStyle cellStyle)
        {
            if (borderLeft != null)
            {
                cellStyle.BorderLeft = (BorderStyle)borderLeft;
            }

            if (borderRight != null)
            {
                cellStyle.BorderRight = (BorderStyle)borderRight;
            }

            if (borderBottom != null)
            {
                cellStyle.BorderBottom = (BorderStyle)borderBottom;
            }

            if (borderTop != null)
            {
                cellStyle.BorderTop = (BorderStyle)borderTop;
            }
        }

        private static void UpdateFontStyle(string fontName, int fontHeight, bool isBold, XSSFFont fontStyle)
        {
            fontStyle.IsBold = isBold;
            fontStyle.FontName = fontName;
            fontStyle.FontHeightInPoints = fontHeight;
        }

        private static short GetDoubleValuesFormat(IWorkbook workbook)
        {
            var format = (XSSFDataFormat)workbook.CreateDataFormat();
            return format.GetFormat("#,##0.00; (#,##0.00)");
        }
    }
}
