namespace DigitalFenceMonitor
{
    partial class FormHoyo
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
            this.textBox_content = new System.Windows.Forms.RichTextBox();
            this.richTextBox0 = new System.Windows.Forms.RichTextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // textBox_content
            // 
            this.textBox_content.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.textBox_content.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox_content.EnableAutoDragDrop = true;
            this.textBox_content.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_content.Location = new System.Drawing.Point(34, 25);
            this.textBox_content.Name = "textBox_content";
            this.textBox_content.ReadOnly = true;
            this.textBox_content.Size = new System.Drawing.Size(650, 403);
            this.textBox_content.TabIndex = 0;
            this.textBox_content.Text = "";
            // 
            // richTextBox0
            // 
            this.richTextBox0.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.richTextBox0.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox0.EnableAutoDragDrop = true;
            this.richTextBox0.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richTextBox0.Location = new System.Drawing.Point(31, 443);
            this.richTextBox0.Name = "richTextBox0";
            this.richTextBox0.ReadOnly = true;
            this.richTextBox0.Size = new System.Drawing.Size(650, 25);
            this.richTextBox0.TabIndex = 1;
            this.richTextBox0.Text = "";
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.EnableAutoDragDrop = true;
            this.richTextBox1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richTextBox1.Location = new System.Drawing.Point(31, 482);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(650, 25);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // FormHoyo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(713, 536);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.richTextBox0);
            this.Controls.Add(this.textBox_content);
            this.Name = "FormHoyo";
            this.Text = "About Hoyo";
            this.Load += new System.EventHandler(this.FormHoyo_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox textBox_content;
        private System.Windows.Forms.RichTextBox richTextBox0;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}