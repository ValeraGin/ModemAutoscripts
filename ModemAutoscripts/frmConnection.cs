using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ModemAutoscripts
{
    public partial class frmConnection : Form
    {
        public void SetData(string portName, int baudRate, int timeout)
        {
            cboPort.Text = portName;
            cboBaudRate.Text = baudRate.ToString();
            cboTimeout.Text = timeout.ToString();
        }

        public void GetData(out string portName, out int baudRate, out int timeout)
        {
            portName = cboPort.Text;
            baudRate = Convert.ToInt32(cboBaudRate.Text);
            timeout = Convert.ToInt32(cboTimeout.Text);
        }

        public frmConnection()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            cboPort.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());

            cboBaudRate.Items.Add("9600");
            cboBaudRate.Items.Add("19200");
            cboBaudRate.Items.Add("38400");
            cboBaudRate.Items.Add("57600");
            cboBaudRate.Items.Add("115200");

            cboTimeout.Items.Add("150");
            cboTimeout.Items.Add("300");
            cboTimeout.Items.Add("600");
            cboTimeout.Items.Add("900");
            cboTimeout.Items.Add("1200");
            cboTimeout.Items.Add("1500");
            cboTimeout.Items.Add("1800");
            cboTimeout.Items.Add("2000");
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
