using System;
using System.Windows.Forms;
using TrialBalanceConversionTool.Interfaces;
using TrialBalanceConversionTool.Services;

namespace TrialBalanceConversionTool
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            IImportDataReader dataReader = new ImportDataReader();
            IDataConverter dataConverter = new DataConverter();
            IExportService exportService = new ExportService();
            Application.Run(new ConversionToolMenu(dataReader, dataConverter, exportService));
        }
    }
}
