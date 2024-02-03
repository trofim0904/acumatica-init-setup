using TrialBalanceConversionTool.Entities;

namespace TrialBalanceConversionTool.Interfaces
{
    public interface IDataConverter
    {
        OutputFileData GetOutputFileData(InputFileData inputFileData, ConversionRules conversionRules);
    }
}
