namespace ModemAutoscripts
{
    partial class frmConnection
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
            this.button1 = new System.Windows.Forms.Button();
            this.lblTimeout = new System.Windows.Forms.Label();
            this.lblBaudRate = new System.Windows.Forms.Label();
            this.cboTimeout = new System.Windows.Forms.ComboBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.cboPort = new System.Windows.Forms.ComboBox();
            this.cboBaudRate = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(12, 103);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(216, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Подключение";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblTimeout
            // 
            this.lblTimeout.Location = new System.Drawing.Point(12, 66);
            this.lblTimeout.Name = "lblTimeout";
            this.lblTimeout.Size = new System.Drawing.Size(100, 23);
            this.lblTimeout.TabIndex = 10;
            this.lblTimeout.Text = "Таймаут (ms):";
            this.lblTimeout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBaudRate
            // 
            this.lblBaudRate.Location = new System.Drawing.Point(12, 42);
            this.lblBaudRate.Name = "lblBaudRate";
            this.lblBaudRate.Size = new System.Drawing.Size(100, 23);
            this.lblBaudRate.TabIndex = 8;
            this.lblBaudRate.Text = "Скорость порта";
            this.lblBaudRate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboTimeout
            // 
            this.cboTimeout.Location = new System.Drawing.Point(124, 66);
            this.cboTimeout.Name = "cboTimeout";
            this.cboTimeout.Size = new System.Drawing.Size(104, 21);
            this.cboTimeout.TabIndex = 11;
            // 
            // lblPort
            // 
            this.lblPort.Location = new System.Drawing.Point(12, 18);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(88, 23);
            this.lblPort.TabIndex = 6;
            this.lblPort.Text = "Имя порта";
            this.lblPort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboPort
            // 
            this.cboPort.Location = new System.Drawing.Point(124, 18);
            this.cboPort.Name = "cboPort";
            this.cboPort.Size = new System.Drawing.Size(104, 21);
            this.cboPort.TabIndex = 7;
            // 
            // cboBaudRate
            // 
            this.cboBaudRate.Location = new System.Drawing.Point(124, 42);
            this.cboBaudRate.Name = "cboBaudRate";
            this.cboBaudRate.Size = new System.Drawing.Size(104, 21);
            this.cboBaudRate.TabIndex = 9;
            // 
            // frmConnection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(243, 148);
            this.Controls.Add(this.lblTimeout);
            this.Controls.Add(this.lblBaudRate);
            this.Controls.Add(this.cboTimeout);
            this.Controls.Add(this.lblPort);
            this.Controls.Add(this.cboPort);
            this.Controls.Add(this.cboBaudRate);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmConnection";
            this.Text = "Подключение";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblTimeout;
        private System.Windows.Forms.Label lblBaudRate;
        private System.Windows.Forms.ComboBox cboTimeout;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.ComboBox cboPort;
        private System.Windows.Forms.ComboBox cboBaudRate;
    }
}