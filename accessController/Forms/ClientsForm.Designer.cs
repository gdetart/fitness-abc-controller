
namespace accessController.Forms
{
    partial class ClientsForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.showAllClientsBtn = new System.Windows.Forms.Button();
            this.AddEditClientsBtn = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.clientPanel = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.AddEditClientsBtn);
            this.panel1.Controls.Add(this.showAllClientsBtn);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 46);
            this.panel1.TabIndex = 0;
            // 
            // showAllClientsBtn
            // 
            this.showAllClientsBtn.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.showAllClientsBtn.FlatAppearance.BorderSize = 0;
            this.showAllClientsBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.showAllClientsBtn.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.showAllClientsBtn.ForeColor = System.Drawing.Color.White;
            this.showAllClientsBtn.Location = new System.Drawing.Point(0, 0);
            this.showAllClientsBtn.Name = "showAllClientsBtn";
            this.showAllClientsBtn.Size = new System.Drawing.Size(398, 46);
            this.showAllClientsBtn.TabIndex = 0;
            this.showAllClientsBtn.Text = "Clients";
            this.showAllClientsBtn.UseVisualStyleBackColor = true;
            // 
            // AddEditClientsBtn
            // 
            this.AddEditClientsBtn.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.AddEditClientsBtn.FlatAppearance.BorderSize = 0;
            this.AddEditClientsBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddEditClientsBtn.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.AddEditClientsBtn.ForeColor = System.Drawing.Color.White;
            this.AddEditClientsBtn.Location = new System.Drawing.Point(395, 0);
            this.AddEditClientsBtn.Name = "AddEditClientsBtn";
            this.AddEditClientsBtn.Size = new System.Drawing.Size(405, 46);
            this.AddEditClientsBtn.TabIndex = 1;
            this.AddEditClientsBtn.Text = "Add/Edit";
            this.AddEditClientsBtn.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Location = new System.Drawing.Point(0, 41);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(398, 5);
            this.panel2.TabIndex = 1;
            // 
            // clientPanel
            // 
            this.clientPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clientPanel.Location = new System.Drawing.Point(0, 46);
            this.clientPanel.Name = "clientPanel";
            this.clientPanel.Size = new System.Drawing.Size(800, 404);
            this.clientPanel.TabIndex = 1;
            // 
            // ClientsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(71)))), ((int)(((byte)(120)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.ControlBox = false;
            this.Controls.Add(this.clientPanel);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ClientsForm";
            this.Text = "ClientsForm";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button AddEditClientsBtn;
        private System.Windows.Forms.Button showAllClientsBtn;
        private System.Windows.Forms.Panel clientPanel;
    }
}