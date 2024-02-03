using Npoi.Mapper.Attributes;

namespace TrialBalanceConversionTool.Entities
{
    public class TypeConversionRow
    {
        [Column(0)]
        public string AccountFirstNumber { get; set; }

        [Column(1)]
        public string OutputType { get; set; }
    }
}
