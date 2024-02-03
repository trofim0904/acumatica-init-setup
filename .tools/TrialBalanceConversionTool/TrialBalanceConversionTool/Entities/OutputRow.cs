namespace TrialBalanceConversionTool.Entities
{
    public class OutputRow
    {
        public string Branch { get; set; }

        public string Type { get; set; }

        public string Account { get; set; }

        public string SubAccount { get; set; }

        public string Description { get; set; }

        public double BeginningBalance { get; set; }

        public double DebitAmount { get; set; }

        public double CreditAmount { get; set; }

        public double EndingBalance { get; set; }

        public bool IsFirstLineRow { get; set; }

        public bool IsSecondLineRow { get; set; }

        public bool IsSingleLineRow { get; set; }

        public bool IsContainError { get; set; }
    }
}
