using Npoi.Mapper.Attributes;

namespace TrialBalanceConversionTool.Entities
{
    public class BranchConversionRow
    {
        [Column(0)]
        public string InputBranch { get; set; }

        [Column(1)]
        public string OutputBranch { get; set; }

        [Column(2)]
        public string OutputDistrict { get; set; }

        [Column(3)]
        public string OutputRegion { get; set; }

        [Column(4)]
        public string OutputSubAccount { get; set; }
    }
}
