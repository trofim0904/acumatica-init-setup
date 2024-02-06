using TrialBalanceConversionTool.Entities;

namespace TrialBalanceConversionTool.Interfaces
{
    public interface IImportDataReader
    {
        ConversionRules GetConversionRules(string filePath);

        InputFileData GetInputFileData(string filePath);
    }
}
