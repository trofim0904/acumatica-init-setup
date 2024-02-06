using Npoi.Mapper.Attributes;

namespace TrialBalanceConversionTool.Entities
{
    public class InputRow
    {
        [Column(0)]
        public string Number { get; set; }

        [Column(7)]
        public string Description { get; set; }

        [Column(11)]
        public double BeginningBalance { get; set; }
        
        [Column(13)]
        public double Debit { get; set; }

        [Column(16)]
        public double Credit { get; set; }

        [Column(20)]
        public double EndingBalance { get; set; }

        public string Account { get; set; }

        public string Branch { get; set; }

        public string AccountFirstNumber { get; set; }
    }
}
