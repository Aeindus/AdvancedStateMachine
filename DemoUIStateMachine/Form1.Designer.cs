namespace DemoUIStateMachine {
    partial class Form1 {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            cmbInput1 = new ComboBox();
            cmbInput2 = new ComboBox();
            btnSearch = new Button();
            cmbResult = new ComboBox();
            SuspendLayout();
            // 
            // cmbInput1
            // 
            cmbInput1.FormattingEnabled = true;
            cmbInput1.Location = new Point(58, 89);
            cmbInput1.Name = "cmbInput1";
            cmbInput1.Size = new Size(195, 23);
            cmbInput1.TabIndex = 0;
            cmbInput1.SelectedIndexChanged += cmbInput1_SelectedIndexChanged;
            // 
            // cmbInput2
            // 
            cmbInput2.FormattingEnabled = true;
            cmbInput2.Location = new Point(58, 135);
            cmbInput2.Name = "cmbInput2";
            cmbInput2.Size = new Size(195, 23);
            cmbInput2.TabIndex = 1;
            // 
            // btnSearch
            // 
            btnSearch.Location = new Point(292, 135);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(75, 23);
            btnSearch.TabIndex = 2;
            btnSearch.Text = "Search";
            btnSearch.UseVisualStyleBackColor = true;
            // 
            // cmbResult
            // 
            cmbResult.FormattingEnabled = true;
            cmbResult.Location = new Point(58, 218);
            cmbResult.Name = "cmbResult";
            cmbResult.Size = new Size(195, 23);
            cmbResult.TabIndex = 3;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(cmbResult);
            Controls.Add(btnSearch);
            Controls.Add(cmbInput2);
            Controls.Add(cmbInput1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion

        private ComboBox cmbInput1;
        private ComboBox cmbInput2;
        private Button btnSearch;
        private ComboBox cmbResult;
    }
}