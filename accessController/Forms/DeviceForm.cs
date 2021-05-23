using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace accessController.Forms
{
    public partial class DeviceForm : Form
    {
        public DeviceForm()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        private long SN;
        private string IP;
        private string name;
        private DeviceController device;

        private void button1_Click(object sender, EventArgs e)
        {
            Database.Devicedb devicedb = new Database.Devicedb();
            SN = long.Parse(this.controllerSN.Text);
            IP = this.controllerIP.Text;
            name = this.deviceName.Text;
            devicedb.Devices.Add(new Database.Device() { name = name, IP = IP, SN = SN });
            device = new DeviceController { controllerIP=IP,controllerSN=SN};
            if (this.controllerIP.TextLength <1)
            {
                this.controllerStatus.Text = "You must Give an ip";
                this.controllerStatus.ForeColor = System.Drawing.Color.YellowGreen;
            }
            else if(this.controllerSN.TextLength<1){
                this.controllerStatus.Text = "You must Give a Serial Number";
                this.controllerStatus.ForeColor = System.Drawing.Color.YellowGreen;
            }
            else if (this.deviceName.TextLength < 1)
            {
                this.controllerStatus.Text = "You must Give a Name";
                this.controllerStatus.ForeColor = System.Drawing.Color.YellowGreen;
            }
            else
            {

                
                devicedb.SaveChanges();
                string response = device.Initialize();
                
                this.controllerStatus.Text = response;
                this.controllerStatus.ForeColor = System.Drawing.Color.GreenYellow;
                

            }
        }

        private void DeviceForm_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {



        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            
            List<string> records = device.extraxtRecord();
            if(records.Count<0)
            {
                this.displayRecords.Text = "0 records";
                return;
            }
            this.displayRecords.Text = Convert.ToString(records.Count);
            for (int i = 0; i < records.Count; i++)
            {
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
