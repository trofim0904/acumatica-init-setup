using Npoi.Mapper.Attributes;

namespace TrialBalanceConversionTool.Entities
{
    public class AccountConversionRow
    {
        [Column(0)]
        public string InputAccount { get; set; }

        [Column(1)]
        public string InputDescription { get; set; }

        [Column(2)]
        public string OutputAccount { get; set; }

        [Column(3)]
        public string OutputDescription { get; set; }
    }
}
