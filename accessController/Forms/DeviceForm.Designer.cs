
namespace accessController.Forms
{
    partial class DeviceForm
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
            this.controllerSN = new System.Windows.Forms.TextBox();
            this.controllerIP = new System.Windows.Forms.TextBox();
            this.addController = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.controllerStatus = new System.Windows.Forms.Label();
            this.displayRecords = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.deviceName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.existingControllers = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // controllerSN
            // 
            this.controllerSN.Location = new System.Drawing.Point(30, 43);
            this.controllerSN.Name = "controllerSN";
            this.controllerSN.Size = new System.Drawing.Size(161, 23);
            this.controllerSN.TabIndex = 0;
            // 
            // controllerIP
            // 
            this.controllerIP.Location = new System.Drawing.Point(240, 43);
            this.controllerIP.Name = "controllerIP";
            this.controllerIP.Size = new System.Drawing.Size(161, 23);
            this.controllerIP.TabIndex = 1;
            this.controllerIP.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // addController
            // 
            this.addController.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(98)))), ((int)(((byte)(102)))));
            this.addController.FlatAppearance.BorderSize = 0;
            this.addController.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addController.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.addController.ForeColor = System.Drawing.Color.White;
            this.addController.Location = new System.Drawing.Point(450, 71);
            this.addController.Name = "addController";
            this.addController.Size = new System.Drawing.Size(139, 23);
            this.addController.TabIndex = 2;
            this.addController.Text = "Add Controller";
            this.addController.UseVisualStyleBackColor = false;
            this.addController.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(30, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Serial Number";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(240, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Ip address";
            // 
            // controllerStatus
            // 
            this.controllerStatus.AutoSize = true;
            this.controllerStatus.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.controllerStatus.ForeColor = System.Drawing.Color.Red;
            this.controllerStatus.Location = new System.Drawing.Point(250, 103);
            this.controllerStatus.Name = "controllerStatus";
            this.controllerStatus.Size = new System.Drawing.Size(116, 17);
            this.controllerStatus.TabIndex = 5;
            this.controllerStatus.Text = "Controller Status ";
            // 
            // displayRecords
            // 
            this.displayRecords.Location = new System.Drawing.Point(30, 208);
            this.displayRecords.Multiline = true;
            this.displayRecords.Name = "displayRecords";
            this.displayRecords.Size = new System.Drawing.Size(371, 121);
            this.displayRecords.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(450, 205);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(139, 27);
            this.button1.TabIndex = 7;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_2);
            // 
            // deviceName
            // 
            this.deviceName.Location = new System.Drawing.Point(30, 97);
            this.deviceName.Name = "deviceName";
            this.deviceName.Size = new System.Drawing.Size(161, 23);
            this.deviceName.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(30, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 17);
            this.label3.TabIndex = 10;
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(30, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 17);
            this.label4.TabIndex = 11;
            this.label4.Text = "Device Name";
            // 
            // existingControllers
            // 
            this.existingControllers.Location = new System.Drawing.Point(634, 43);
            this.existingControllers.Multiline = true;
            this.existingControllers.Name = "existingControllers";
            this.existingControllers.Size = new System.Drawing.Size(371, 286);
            this.existingControllers.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label5.Location = new System.Drawing.Point(753, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(129, 17);
            this.label5.TabIndex = 13;
            this.label5.Text = "Existing Controllers";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // DeviceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(71)))), ((int)(((byte)(120)))));
            this.ClientSize = new System.Drawing.Size(1045, 545);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.existingControllers);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.deviceName);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.displayRecords);
            this.Controls.Add(this.controllerStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.addController);
            this.Controls.Add(this.controllerIP);
            this.Controls.Add(this.controllerSN);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DeviceForm";
            this.Text = "DeviceForm";
            this.Load += new System.EventHandler(this.DeviceForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox controllerSN;
        private System.Windows.Forms.TextBox controllerIP;
        private System.Windows.Forms.Button addController;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label controllerStatus;
        private System.Windows.Forms.TextBox displayRecords;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox deviceName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox existingControllers;
        private System.Windows.Forms.Label label5;
    }
}