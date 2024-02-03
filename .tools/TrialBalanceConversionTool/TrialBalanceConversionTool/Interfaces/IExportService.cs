using TrialBalanceConversionTool.Entities;

namespace TrialBalanceConversionTool.Interfaces
{
    public interface IExportService
    {
        void CreateOutputFile(string localPath, OutputFileData outputFileData);
    }
}
