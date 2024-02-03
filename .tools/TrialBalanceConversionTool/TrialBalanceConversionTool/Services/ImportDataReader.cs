using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Npoi.Mapper;
using NPOI.SS.UserModel;
using TrialBalanceConversionTool.Entities;
using TrialBalanceConversionTool.Interfaces;

namespace TrialBalanceConversionTool.Services
{
    public class ImportDataReader : IImportDataReader
    {
        private static ConversionRules ConversionRules { get; set; }

        private static InputFileData InputFileData { get; set; }

        public ImportDataReader()
        {
            ConversionRules = new ConversionRules();
            InputFileData = new InputFileData();
        }

        public ConversionRules GetConversionRules(string filePath)
        {
            InitializeConversionRules(filePath);
            return ConversionRules;
        }

        public InputFileData GetInputFileData(string filePath)
        {
            InitializeInputFileData(filePath);
            return InputFileData;
        }

        private static void InitializeConversionRules(string filePath)
        {
            IWorkbook conversionWorkbook = OpenWorkbook(filePath);
            InitializeAccountConversionDictionary(conversionWorkbook);
            InitializeBranchConversionDictionary(conversionWorkbook);
            InitializeTypeConversionDictionary(conversionWorkbook);
        }

        private static void InitializeInputFileData(string filePath)
        {
            InputFileData.InputRows = GetInputRows(filePath, Constants.InputDataSheetIndex);
        }

        private static void UpdateInputRowsAccountData(IEnumerable<InputRow> inputRows)
        {
            inputRows.ToList().ForEach(row =>
            {
                row.AccountFirstNumber = row.Number[0].ToString();
                row.Account = row.Number.Split('-')[0];
                row.Branch = row.Number.Split('-')[1];
            });
        }

        private static IEnumerable<InputRow> GetInputRows(string localPath, int sheetIndex)
        {
            IWorkbook workbook = OpenWorkbook(localPath);
            UpdateWorkbook(workbook);
            var importer = new Mapper(workbook);
            IEnumerable<RowInfo<InputRow>> items = importer.Take<InputRow>(sheetIndex);
            List<InputRow> inputRows = items.Select(item => item.Value).ToList();
            UpdateInputRowsAccountData(inputRows);
            return inputRows;
        }

        private static void InitializeAccountConversionDictionary( IWorkbook workbook)
        {
            var mapper = new Mapper(workbook);
            IEnumerable<RowInfo<AccountConversionRow>> items =
                mapper.Take<AccountConversionRow>(Constants.AccountSheetIndex);
            ConversionRules.AccountConversionDictionary = items.Select(item => item.Value);
        }

        private static void InitializeBranchConversionDictionary(IWorkbook workbook)
        {
            var mapper = new Mapper(workbook);
            IEnumerable<RowInfo<BranchConversionRow>> items =
                mapper.Take<BranchConversionRow>(Constants.BranchSheetIndex);
            ConversionRules.BranchConversionDictionary = items.Select(item => item.Value);
        }

        private static void InitializeTypeConversionDictionary(IWorkbook workbook)
        {
            var mapper = new Mapper(workbook);
            IEnumerable<RowInfo<TypeConversionRow>> items = 
                mapper.Take<TypeConversionRow>(Constants.TypeSheetIndex);
            ConversionRules.TypeConversionDictionary = items.Select(item => item.Value);
        }

        private static void UpdateWorkbook(IWorkbook workbook)
        {
            ISheet sheet = workbook.GetSheetAt(0);
            int rowCount = sheet.PhysicalNumberOfRows;

            for (var i = 1; i < rowCount; i++)
            {
                IRow row = sheet.GetRow(i);
                if (IsNotContainsAccountNumber(row))
                {
                    sheet.RemoveRow(row);
                }
            }

            sheet.CreateRow(0);
            IRow newHeader = sheet.GetRow(0);
            for (var i = 0; i < Constants.InputFileColumnsCount; i++)
            {
                newHeader.CreateCell(i).SetCellValue(i);
            }
        }

        private static bool IsNotContainsAccountNumber(IRow row)
        {
            ICell cell = row.GetCell(0);
            var accountNumberFormat = new Regex(@"\d{4}-\d{2}");
            return cell == null || !accountNumberFormat.IsMatch(cell.StringCellValue);
        }

        private static IWorkbook OpenWorkbook(string localPath)
        {
            IWorkbook workbook;
            using (var fileStream = new FileStream(localPath, FileMode.Open, FileAccess.Read))
            {
                workbook = WorkbookFactory.Create(fileStream);
            }

            return workbook;
        }
    }
}
