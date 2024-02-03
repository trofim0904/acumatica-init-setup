using System;
using System.Windows.Forms;
using TrialBalanceConversionTool.Entities;
using TrialBalanceConversionTool.Interfaces;
using TrialBalanceConversionTool.Services;

namespace TrialBalanceConversionTool
{
    public partial class ConversionToolMenu : Form
    {
        private readonly IImportDataReader dataReader;
        private readonly IDataConverter dataConverter;
        private readonly IExportService exportService;

        private static InputFileData InputFileData { get; set; }
        private static OutputFileData OutputFileData { get; set; }
        private static ConversionRules ConversionRules { get; set; }

        public ConversionToolMenu(
            IImportDataReader dataReader,
            IDataConverter dataConverter,
            IExportService exportService)
        {
            this.dataReader = dataReader;
            this.dataConverter = dataConverter;
            this.exportService = exportService;
            InitializeComponent();
            SetFileDialogSettings();
            InitializeEntities();
        }

        private static void InitializeEntities()
        {
            InputFileData = new InputFileData();
            OutputFileData = new OutputFileData();
            ConversionRules = new ConversionRules();
        }

        private void SetFileDialogSettings()
        {
            saveOutputFileDialog.DefaultExt = Constants.ExcelFileExtension;
            openInputFileDialog.DefaultExt = Constants.OldExcelFileExtension;
            openConversionFileDialog.DefaultExt = Constants.ExcelFileExtension;
            openInputFileDialog.FileName = Constants.DefaultInputFileName;
            openConversionFileDialog.FileName = Constants.DefaultConversionFileName;
            saveOutputFileDialog.FileName = Constants.DefaultOutputFileName;
            openInputFileDialog.Filter = Constants.FileDialogOldExcelWorkbookFilter;
            openConversionFileDialog.Filter = Constants.FileDialogExcelWorkbookFilter;
            saveOutputFileDialog.Filter = Constants.FileDialogExcelWorkbookFilter;
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            if (openInputFileDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            InputFileData = dataReader.GetInputFileData(openInputFileDialog.FileName);
            ConvertButton.Enabled = true;
        }

        private void ConvertButton_Click(object sender, EventArgs e)
        {
            if (saveOutputFileDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            if (IsRequiredDataFilled())
            {
                OutputFileData outputFileData = dataConverter.GetOutputFileData(InputFileData, ConversionRules);
                exportService.CreateOutputFile(saveOutputFileDialog.FileName, outputFileData);
            }
        }

        private void ImportConversionRules_Click(object sender, EventArgs e)
        {
            if (openConversionFileDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            ConversionRules = dataReader.GetConversionRules(openConversionFileDialog.FileName);
            ImportButton.Enabled = true;
        }

        private bool IsRequiredDataFilled()
        {
            return InputFileData.InputRows != null &&
                   ConversionRules.AccountConversionDictionary != null &&
                   ConversionRules.BranchConversionDictionary != null &&
                   ConversionRules.TypeConversionDictionary != null;
        }
    }
}
