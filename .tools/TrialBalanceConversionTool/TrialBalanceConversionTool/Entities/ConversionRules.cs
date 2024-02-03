using System.Collections.Generic;

namespace TrialBalanceConversionTool.Entities
{
    public class ConversionRules
    {
        public IEnumerable<AccountConversionRow> AccountConversionDictionary { get; set; }

        public IEnumerable<BranchConversionRow> BranchConversionDictionary { get; set; }

        public IEnumerable<TypeConversionRow> TypeConversionDictionary { get; set; }
    }
}
