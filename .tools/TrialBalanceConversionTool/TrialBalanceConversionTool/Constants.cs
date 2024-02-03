namespace TrialBalanceConversionTool
{
    public class Constants
    {
        public static readonly string OutputFileSheetName = "Conversion Output File";

        public static readonly string DefaultInputFileName = "MAS500 Export File";

        public static readonly string DefaultConversionFileName = "Trial Balance Conversion File";

        public static readonly string DefaultOutputFileName = "Vesta Energy Trial Balance";

        /// <summary>
        /// The output file only accounting columns count
        /// </summary>
        public static readonly int OutputFileAccountingColumnsCount = 9;

        /// <summary>
        /// The output file all columns count
        /// </summary>
        public static readonly int OutputFileAllColumnsCount = 10;

        internal const int DefaultFontSizeValue = 10;

        internal const int HeaderFontSize = 11;

        internal const int TotalsFontSize = 8;

        internal const int InputFileColumnsCount = 42;

        internal const int InputDataSheetIndex = 0;

        internal const int AccountSheetIndex = 0;

        internal const int BranchSheetIndex = 1;

        internal const int TypeSheetIndex = 2;

        internal const string DefaultFontName = "Arial";

        internal const string HeaderFontName = "Calibri";

        public static readonly string ExcelFileExtension = "xlsx";

        public static readonly string OldExcelFileExtension = "xls";

        public static readonly string FileDialogOldExcelWorkbookFilter = "Excel 97-2003 Workbook (*.xls)|*.xls";

        public static readonly string FileDialogExcelWorkbookFilter = "Excel Workbook files (*.xlsx)|*.xlsx";

        public class ErrorMessages
        {
            public static readonly string BranchIsNotFound = "Branch cannot be found";

            public static readonly string AccountIsNotFound = "Account cannot be found";

            public static readonly string TypeIsNotFound = "Type cannot be found";

            public static readonly string DistrictIsNotFound = "District cannot be found for Branch - ";

            public static readonly string RegionIsNotFound = "Region cannot be found for Branch - ";
        }

        public class TotalDescriptions
        {
            public static readonly string Assets =
                "Assets (SUM of Ending Balance for all Accounts with Type = Asset)";

            public static readonly string Liabilities =
                "Liabilities  (SUM of Ending Balance for all Accounts with Type = Liability)";

            public static readonly string Equity = "Assets - Liabilities";

            public static readonly string Income =
                "Income  (SUM of Ending Balance for all Accounts with Type = Income)";

            public static readonly string Expenses =
                "Expenses  (SUM of Ending Balance for all Accounts with Type = Expense)";

            public static readonly string Profit = "Income - Expenses";

            public static readonly string Debits = "Debits (Assets + Expenses)";

            public static readonly string Credits = "Credits (Liabilities + Income)";

            public static readonly string BalanceTotal = "Debits - Credits";
        }

        public class ColumnHeaders
        {
            public static readonly string Branch = "Branch";
            public static readonly string Type = "Type";
            public static readonly string Account = "Account";
            public static readonly string SubAccount = "Subaccount";
            public static readonly string Description = "Description";
            public static readonly string BeginningBalance = "Beginning Balance";
            public static readonly string DebitAmount = "Debit Amount";
            public static readonly string CreditAmount = "Credit Amount";
            public static readonly string EndingBalance = "Ending Balance";
        }

        public class ColumnNumbers
        {
            public static readonly int BranchColumnNumber = 0;
            public static readonly int TypeColumnNumber = 1;
            public static readonly int AccountColumnNumber = 2;
            public static readonly int SubAccountColumnNumber = 3;
            public static readonly int DescriptionColumnNumber = 4;
            public static readonly int BeginningBalanceColumnNumber = 5;
            public static readonly int DebitAmountColumnNumber = 6;
            public static readonly int CreditAmountColumnNumber = 7;
            public static readonly int EndingBalanceColumnNumber = 8;
            public static readonly int CalculatedTotalsColumnNumber = 8;
            public static readonly int TotalsDescriptionColumnNumber = 9;
        }
    }
}
