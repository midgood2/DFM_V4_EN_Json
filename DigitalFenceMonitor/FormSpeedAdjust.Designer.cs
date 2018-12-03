namespace DigitalFenceMonitor
{
    partial class FormSpeedAdjust
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
            this.lab_speed = new System.Windows.Forms.Label();
            this.lab_speedAdj = new System.Windows.Forms.Label();
            this.btn_Up = new System.Windows.Forms.Button();
            this.btn_Down = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lab_speed
            // 
            this.lab_speed.AutoSize = true;
            this.lab_speed.Font = new System.Drawing.Font("Cambria", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lab_speed.ForeColor = System.Drawing.Color.Maroon;
            this.lab_speed.Location = new System.Drawing.Point(47, 37);
            this.lab_speed.Name = "lab_speed";
            this.lab_speed.Size = new System.Drawing.Size(142, 21);
            this.lab_speed.TabIndex = 1;
            this.lab_speed.Text = "Speed Now :";
            // 
            // lab_speedAdj
            // 
            this.lab_speedAdj.AutoSize = true;
            this.lab_speedAdj.Font = new System.Drawing.Font("Cambria", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lab_speedAdj.Location = new System.Drawing.Point(167, 30);
            this.lab_speedAdj.Name = "lab_speedAdj";
            this.lab_speedAdj.Size = new System.Drawing.Size(0, 29);
            this.lab_speedAdj.TabIndex = 2;
            // 
            // btn_Up
            // 
            this.btn_Up.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.btn_Up.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_Up.ForeColor = System.Drawing.Color.Black;
            this.btn_Up.Location = new System.Drawing.Point(75, 84);
            this.btn_Up.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Up.Name = "btn_Up";
            this.btn_Up.Padding = new System.Windows.Forms.Padding(1);
            this.btn_Up.Size = new System.Drawing.Size(85, 45);
            this.btn_Up.TabIndex = 6;
            this.btn_Up.Text = "UP";
            this.btn_Up.UseVisualStyleBackColor = false;
            this.btn_Up.Click += new System.EventHandler(this.btn_Up_Click);
            // 
            // btn_Down
            // 
            this.btn_Down.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.btn_Down.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_Down.ForeColor = System.Drawing.Color.Black;
            this.btn_Down.Location = new System.Drawing.Point(164, 84);
            this.btn_Down.Margin = new System.Windows.Forms.Padding(2);
            this.btn_Down.Name = "btn_Down";
            this.btn_Down.Padding = new System.Windows.Forms.Padding(1);
            this.btn_Down.Size = new System.Drawing.Size(85, 45);
            this.btn_Down.TabIndex = 7;
            this.btn_Down.Text = "DOWN";
            this.btn_Down.UseVisualStyleBackColor = false;
            this.btn_Down.Click += new System.EventHandler(this.btn_Down_Click);
            // 
            // FormSpeedAdjust
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 140);
            this.Controls.Add(this.btn_Down);
            this.Controls.Add(this.btn_Up);
            this.Controls.Add(this.lab_speedAdj);
            this.Controls.Add(this.lab_speed);
            this.Name = "FormSpeedAdjust";
            this.Text = "Communication speed adjustment";
            this.Load += new System.EventHandler(this.FormSpeedAdjust_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lab_speed;
        private System.Windows.Forms.Label lab_speedAdj;
        private System.Windows.Forms.Button btn_Up;
        private System.Windows.Forms.Button btn_Down;
    }
}