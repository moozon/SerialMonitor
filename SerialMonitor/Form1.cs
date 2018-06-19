using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SerialMonitor
{

    public partial class Form1 : Form
    {
        int[] baudRate = { 9600, 115200 };
        string[] comPorts;
        //string buf;

        public Form1()
        {
            InitializeComponent();

            if (checkBox1.Checked == false) textBox1.Enabled = false;
            comPorts = System.IO.Ports.SerialPort.GetPortNames();
            Array.Sort(comPorts);
            comboBox1.DataSource = comPorts;
            comboBox2.DataSource = baudRate;
            buttonSend.Enabled = false;
            //buttonClose.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBoxOut.Clear();
        }

        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) textBox1.Enabled = true;
            else textBox1.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {           
            comPorts = System.IO.Ports.SerialPort.GetPortNames();
            Array.Sort(comPorts);
            comboBox1.DataSource = comPorts;
            if (comboBox1.Items.Count == 0) comboBox1.Text = "";
            else comboBox1.Text = comboBox1.Items[0].ToString();
            comboBox1.Update();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.Close();
                buttonConnect.Text = "Connect";
                this.Text = "SerialMonitor";
                buttonSend.Enabled = false;
                buttonRefresh.Enabled = true;
                textBoxOut.AppendText("Close Connect" + Environment.NewLine);
            }
            else
            {                
                if (comboBox1.Items.Count > 0) serialPort1.PortName = comboBox1.SelectedItem.ToString();
                else
                {
                    textBoxOut.AppendText("Выберите порт" + Environment.NewLine);
                    return;
                }

                try
                {
                    serialPort1.BaudRate = (int)comboBox2.SelectedItem;
                }
                catch (Exception)
                {
                    serialPort1.BaudRate = Convert.ToInt32(comboBox2.Text);
                }


                try
                {
                    serialPort1.Open();
                    buttonConnect.Text = "Disconnect";
                    this.Text += " " + comboBox1.SelectedItem.ToString();
                    textBoxOut.AppendText("Connect succesfully" + Environment.NewLine);
                    //buttonConnect.Enabled = false;
                    buttonRefresh.Enabled = false;
                    buttonSend.Enabled = true;
                    //buttonClose.Enabled = true;
                }
                catch (Exception)
                {
                    textBoxOut.AppendText("No Connect" + Environment.NewLine);
                }

            }
        }
        private void buttonClose_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            this.Text = "SerialMonitor";
            //buttonClose.Enabled = false;
            buttonConnect.Enabled = true;
            buttonSend.Enabled = false;
            textBoxOut.AppendText("Close Connect" + Environment.NewLine);            
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen) serialPort1.Write(textBoxSend.Text);
            else textBoxOut.AppendText("Соединение разорвано" + Environment.NewLine);
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
                textBoxOut.Invoke((MethodInvoker)(() => textBoxOut.AppendText(serialPort1.ReadExisting())));            
        }

        private void serialPort1_PinChanged(object sender, System.IO.Ports.SerialPinChangedEventArgs e)
        {
            textBoxOut.AppendText("Соединение разорвано" + Environment.NewLine);
        }        

        private void textBoxSend_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) buttonSend_Click(new object(), new EventArgs());
        }
    }
}
