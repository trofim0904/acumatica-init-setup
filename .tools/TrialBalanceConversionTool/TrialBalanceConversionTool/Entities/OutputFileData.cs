using System.Collections.Generic;

namespace TrialBalanceConversionTool.Entities
{
    public class OutputFileData
    {
        public IEnumerable<OutputRow> OutputRows { get; set; }

        public double DebitAmountTotal { get; set; }

        public double CreditAmountTotal { get; set; }
        
        /// <summary>
        /// Sum of Ending Balance for all Accounts with Type equal Asset
        /// </summary>
        public double AssetsBalanceTotal { get; set; }


        /// <summary>
        /// Sum of Ending Balance for all Accounts with Type equal Liability
        /// </summary>
        public double LiabilitiesBalanceTotal { get; set; }

        /// <summary>
        /// Subtraction Liabilities from Assets
        /// </summary>
        public double Equity { get; set; }

        /// <summary>
        /// Sum of Ending Balance for all Accounts with Type equal Income
        /// </summary>
        public double IncomeBalanceTotal { get; set; }

        /// <summary>
        /// Sum of Ending Balance for all Accounts with Type equal Expenses
        /// </summary>
        public double ExpensesBalanceTotal { get; set; }

        /// <summary>
        /// Subtraction Expenses from Income
        /// </summary>
        public double Profit { get; set; }

        /// <summary>
        /// Sum of Assets and Expenses
        /// </summary>
        public double Debits { get; set; }

        /// <summary>
        /// Sum of Liabilities and Income
        /// </summary>
        public double Credits { get; set; }

        /// <summary>
        /// Subtraction Credits from Debits
        /// </summary>
        public double BalanceTotal { get; set; }
    }
}
