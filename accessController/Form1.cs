using System;
using System.Windows.Forms;

namespace accessController
{
    public partial class Form1 : Form
    {
        private long controllerSN = 423130361;
        private string controllerIP = "192.168.0.123";

       

        public Form1()
        {
            long controllerSN = 423130361;
            string controllerIP = "192.168.0.123";
            DeviceController controller = new DeviceController{controllerIP=controllerIP, controllerSN=controllerSN};
            controller.Initialize();
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void momentumBtn_Click(object sender, EventArgs e)
        {
            DeviceController controller = new DeviceController { controllerIP = controllerIP, controllerSN = controllerSN };
            controller.Initialize();
            activePanel.Height = momentumBtn.Height;
            activePanel.Top = momentumBtn.Top;
            this.activePnl.Controls.Clear();
            Forms.MomentumForm momentum = new Forms.MomentumForm();
            momentum.device = controller;
            momentum.TopLevel = false;
            this.activePnl.Controls.Add(momentum);
            momentum.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            momentum.Dock = DockStyle.Fill;
            momentum.Show();
        }

        private void clientsBtn_Click(object sender, EventArgs e)
        {
            activePanel.Height = clientsBtn.Height;
            activePanel.Top = clientsBtn.Top;
            this.activePnl.Controls.Clear();
        }

        private void productsBtn_Click(object sender, EventArgs e)
        {
            activePanel.Height = productsBtn.Height;
            activePanel.Top = productsBtn.Top;
            this.activePnl.Controls.Clear();
        }

        private void activityBtn_Click(object sender, EventArgs e)
        {
            activePanel.Height = activityBtn.Height;
            activePanel.Top = activityBtn.Top;
            this.activePnl.Controls.Clear();
        }

        private void statisticsBtn_Click(object sender, EventArgs e)
        {
            activePanel.Height = statisticsBtn.Height;
            activePanel.Top = statisticsBtn.Top;
            this.activePnl.Controls.Clear();
        }

        private void devicesBtn_Click(object sender, EventArgs e)
        {
            this.activePanel.Height = devicesBtn.Height;
            this.activePanel.Top = devicesBtn.Top;
            this.activePnl.Controls.Clear();
            Forms.DeviceForm deviceForm = new Forms.DeviceForm();
            deviceForm.TopLevel = false;
            this.activePnl.Controls.Add(deviceForm);
            deviceForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            deviceForm.Dock = DockStyle.Fill;
            deviceForm.Show();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void activePnl_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}