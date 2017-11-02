namespace LSATextSummarizationAccord
{
    partial class Form1
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
            this.button_LSA = new System.Windows.Forms.Button();
            this.textBox_LSA = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_Retain = new System.Windows.Forms.TextBox();
            this.checkBox_Limit60s = new System.Windows.Forms.CheckBox();
            this.button_LoadData = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_LSA
            // 
            this.button_LSA.Location = new System.Drawing.Point(12, 47);
            this.button_LSA.Name = "button_LSA";
            this.button_LSA.Size = new System.Drawing.Size(92, 23);
            this.button_LSA.TabIndex = 0;
            this.button_LSA.Text = "Tóm tắt ";
            this.button_LSA.UseVisualStyleBackColor = true;
            this.button_LSA.Click += new System.EventHandler(this.button_LSA_Click);
            // 
            // textBox_LSA
            // 
            this.textBox_LSA.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox_LSA.Location = new System.Drawing.Point(12, 76);
            this.textBox_LSA.Multiline = true;
            this.textBox_LSA.Name = "textBox_LSA";
            this.textBox_LSA.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_LSA.Size = new System.Drawing.Size(753, 578);
            this.textBox_LSA.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(316, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Số câu muốn giữ lại";
            // 
            // textBox_Retain
            // 
            this.textBox_Retain.Location = new System.Drawing.Point(422, 9);
            this.textBox_Retain.Name = "textBox_Retain";
            this.textBox_Retain.Size = new System.Drawing.Size(100, 20);
            this.textBox_Retain.TabIndex = 3;
            // 
            // checkBox_Limit60s
            // 
            this.checkBox_Limit60s.AutoSize = true;
            this.checkBox_Limit60s.Location = new System.Drawing.Point(114, 11);
            this.checkBox_Limit60s.Name = "checkBox_Limit60s";
            this.checkBox_Limit60s.Size = new System.Drawing.Size(159, 17);
            this.checkBox_Limit60s.TabIndex = 4;
            this.checkBox_Limit60s.Text = "VnTagger giới hạn chạy 60s";
            this.checkBox_Limit60s.UseVisualStyleBackColor = true;
            // 
            // button_LoadData
            // 
            this.button_LoadData.Location = new System.Drawing.Point(12, 7);
            this.button_LoadData.Name = "button_LoadData";
            this.button_LoadData.Size = new System.Drawing.Size(75, 23);
            this.button_LoadData.TabIndex = 7;
            this.button_LoadData.Text = "Load Data";
            this.button_LoadData.UseVisualStyleBackColor = true;
            this.button_LoadData.Click += new System.EventHandler(this.button_LoadData_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 666);
            this.Controls.Add(this.button_LoadData);
            this.Controls.Add(this.checkBox_Limit60s);
            this.Controls.Add(this.textBox_Retain);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_LSA);
            this.Controls.Add(this.button_LSA);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Text Summarization";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_LSA;
        private System.Windows.Forms.TextBox textBox_LSA;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_Retain;
        private System.Windows.Forms.CheckBox checkBox_Limit60s;
        private System.Windows.Forms.Button button_LoadData;
    }
}

