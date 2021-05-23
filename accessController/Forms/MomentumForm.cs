using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace accessController.Forms
{
    public partial class MomentumForm : Form
    {
        public MomentumForm()
        {
            InitializeComponent();
        }
        public DeviceController device { get; set; }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void DoorActionPnl_Paint(object sender, PaintEventArgs e)
        {

        }

        private void openDoor1_Click(object sender, EventArgs e)
        {
            device.OpenDoor(1);
        }
    }
}
