namespace TrialBalanceConversionTool
{
    partial class ConversionToolMenu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ImportButton = new System.Windows.Forms.Button();
            this.ConvertButton = new System.Windows.Forms.Button();
            this.openInputFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveOutputFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.ImportConversionRules = new System.Windows.Forms.Button();
            this.openConversionFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // ImportButton
            // 
            this.ImportButton.Enabled = false;
            this.ImportButton.Location = new System.Drawing.Point(99, 68);
            this.ImportButton.Name = "ImportButton";
            this.ImportButton.Size = new System.Drawing.Size(173, 23);
            this.ImportButton.TabIndex = 0;
            this.ImportButton.Text = "Import MAS500 File";
            this.ImportButton.UseVisualStyleBackColor = true;
            this.ImportButton.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // ConvertButton
            // 
            this.ConvertButton.Enabled = false;
            this.ConvertButton.Location = new System.Drawing.Point(99, 110);
            this.ConvertButton.Name = "ConvertButton";
            this.ConvertButton.Size = new System.Drawing.Size(173, 23);
            this.ConvertButton.TabIndex = 1;
            this.ConvertButton.Text = "Convert to Trial Balance Report";
            this.ConvertButton.UseVisualStyleBackColor = true;
            this.ConvertButton.Click += new System.EventHandler(this.ConvertButton_Click);
            // 
            // openInputFileDialog
            // 
            this.openInputFileDialog.FileName = "openInputFileDialog";
            // 
            // ImportConversionRules
            // 
            this.ImportConversionRules.Location = new System.Drawing.Point(99, 25);
            this.ImportConversionRules.Name = "ImportConversionRules";
            this.ImportConversionRules.Size = new System.Drawing.Size(173, 23);
            this.ImportConversionRules.TabIndex = 2;
            this.ImportConversionRules.Text = "Import Conversion File";
            this.ImportConversionRules.UseVisualStyleBackColor = true;
            this.ImportConversionRules.Click += new System.EventHandler(this.ImportConversionRules_Click);
            // 
            // openConversionFileDialog
            // 
            this.openConversionFileDialog.FileName = "openConversionFileDialog";
            // 
            // ConversionToolMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 166);
            this.Controls.Add(this.ImportConversionRules);
            this.Controls.Add(this.ConvertButton);
            this.Controls.Add(this.ImportButton);
            this.Name = "ConversionToolMenu";
            this.Text = "Trial Balance Conversion Tool";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ImportButton;
        private System.Windows.Forms.Button ConvertButton;
        private System.Windows.Forms.OpenFileDialog openInputFileDialog;
        private System.Windows.Forms.SaveFileDialog saveOutputFileDialog;
        private System.Windows.Forms.Button ImportConversionRules;
        private System.Windows.Forms.OpenFileDialog openConversionFileDialog;
    }
}

