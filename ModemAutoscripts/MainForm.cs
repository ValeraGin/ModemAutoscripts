using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


using GsmComm.PduConverter;
using GsmComm.PduConverter.SmartMessaging;
using GsmComm.GsmCommunication;
using GsmComm.Interfaces;


namespace ModemAutoscripts
{

    public partial class MainForm : Form
    {

        private GsmCommMain comm;

        private List<SmsDeliverPdu> listDo = new List<SmsDeliverPdu>();

        public MainForm()
        {
            InitializeComponent();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void comm_PhoneConnected(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = String.Format("Подключено к {0}", comm.PortName);
        }

        private void comm_PhoneDisconnected(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = String.Format("НЕТУ ПОДКЛЮЧЕНИЯ !!!");
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
        }


        private void Output(string action, string text)
        {
            Output(action, " ", text);
        }

        private void Output(string action, string number, string text)
        {
            var ar = new string[4] { action, number, text, DateTime.Now.ToLongTimeString() };
            listView1.Items.Add(new ListViewItem(ar));
            listView1.EnsureVisible(listView1.Items.Count - 1);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Prompt user for connection settings
            string portName = "COM12";
            int baudRate = GsmCommMain.DefaultBaudRate;
            int timeout = GsmCommMain.DefaultTimeout;

            frmConnection dlg = new frmConnection();
            dlg.StartPosition = FormStartPosition.CenterScreen;
            dlg.SetData(portName, baudRate, timeout);
            if (dlg.ShowDialog(this) == DialogResult.OK)
                dlg.GetData(out portName, out baudRate, out timeout);
            else
            {
                Close();
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            comm = new GsmCommMain(portName, baudRate, timeout);
            Cursor.Current = Cursors.Default;
            comm.PhoneConnected += new EventHandler(comm_PhoneConnected);
            comm.PhoneDisconnected += new EventHandler(comm_PhoneDisconnected);
            comm.MessageReceived += new MessageReceivedEventHandler(comm_MessageReceived);

            bool retry;
            do
            {
                retry = false;
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    comm.Open();
                    Cursor.Current = Cursors.Default;
                    Output("Соединение", portName);

                    try
                    {
                        comm.DeleteMessages(DeleteScope.All, PhoneStorageType.Sim);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(this, "Не вставлена сим-карта. Вставьте сим-карту и запустите программу снова.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Close();
                        return;
                    }

                }
                catch (Exception)
                {
                    Cursor.Current = Cursors.Default;

                    if (MessageBox.Show(this, "Не получаеться открыть порт", "Ошибка",
                        MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning) == DialogResult.Retry)
                        retry = true;
                    else
                    {
                        Close();
                        return;
                    }
                }
            }
            while (retry);
            comm.EnableMessageNotifications();
        }



        void MessageReceived(DecodedShortMessage msg)
        {
            var d = (SmsDeliverPdu)msg.Data;
            Output("SMS IN", d.OriginatingAddress, d.UserDataText);
        }

        private void comm_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            var obj = e.IndicationObject;
            if (obj is MemoryLocation)
            {
                MemoryLocation loc = obj as MemoryLocation;
                try
                {
                    var msg = comm.ReadMessage(loc.Index, loc.Storage);
                    comm.DeleteMessage(msg.Index, PhoneStorageType.Sim);
                    var d = (SmsDeliverPdu)msg.Data;
                    if (d.OriginatingAddress != null)
                    {
                        this.Invoke((Action)(() =>
                        {
                            lock (listDo)
                            {

                                listDo.Add((SmsDeliverPdu)msg.Data);
                            };
                        }));
                        this.Invoke((Action)(() => { MessageReceived(msg); }));
                    }
                }
                catch (Exception ex)
                {
                    this.Invoke(new Action(() => ShowException(ex)));
                }



            }
        }



        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (comm != null)
            {
                comm.PhoneConnected -= new EventHandler(comm_PhoneConnected);
                comm.PhoneDisconnected -= new EventHandler(comm_PhoneDisconnected);
                // Close connection to phone
                if (comm != null && comm.IsOpen())
                    comm.Close();
                comm = null;
            }
        }


        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        void SendMsg(string phone, string text)
        {
            try
            {
                comm.SendMessage(new SmsSubmitPdu(text, phone));
                this.Invoke(new Action(() => Output("SMS OUT", phone, text)));
            }
            catch (Exception ex)
            {
                this.Invoke(new Action(() => ShowException(ex)));
            }
        }

        private void ShowException(Exception ex)
        {
            Output("Ошибка", ex.Message);
        }


        private void ProcessStart_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(DoWork));
            //DoWork(null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                IdentificationInfo info = comm.IdentifyDevice();
                Output("Manufacturer: ", info.Manufacturer);
                Output("Model: ", info.Model);
                Output("Revision: ", info.Revision);
                Output("Serial number: ", info.SerialNumber);
            }
            catch (Exception ex)
            {
                ShowException(ex);
            }

            Cursor.Current = Cursors.Default;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SendMsg("111", "11");
        }


        private object waitSMS(Func<SmsDeliverPdu, bool> cond, Func<SmsDeliverPdu, object> act)
        {
            do
            {
                Thread.Sleep(500);
                lock (listDo)
                {
                    if (listDo.Count > 0)
                    {
                        var l = listDo;
                        listDo = new List<SmsDeliverPdu>();
                        foreach (var d in l)
                        {
                            if (cond(d))
                            {
                                return act(d);
                            };
                        }
                    }
                }
            } while (true);
        }


        private int get_balance()
        {
            this.Invoke(new Action(() => { ProcessText.Text = "Проверка баланса"; }));
            try
            {
                SendMsg("111", "11");
                return (int)waitSMS(
                (d) => { return d?.OriginatingAddress == "111"; },
                (d) => { return Convert.ToInt32(System.Text.RegularExpressions.Regex.Match(d.UserDataText, "[\\+\\-]?\\d+\\.?\\d+").Value); }
                );
            }
            catch (Exception ex)
            {
                this.Invoke(new Action(() => { ShowException(ex); }));
                return 0;
            }
        }

        private void cashOut(string number, int step)
        {
            this.Invoke(new Action(() => { ProcessText.Text = "отправка смс на " + number; }));
            this.Invoke(new Action(() => { progressBar1.Value += step; }));
            try
            {
                SendMsg(number, textBox1.Text);
            }
            catch (Exception ex)
            {
                this.Invoke(new Action(() => { ShowException(ex); }));
            };


            this.Invoke(new Action(() => { ProcessText.Text = "ждем смс от 6996 для ответа"; }));
            this.Invoke(new Action(() => { progressBar1.Value += step; }));
            waitSMS(
            (d) => { return d?.OriginatingAddress == "6996"; },
            (d) =>
            {
                try
                {
                    this.Invoke(new Action(() => { ProcessText.Text = "отправляем подтверждение на 6996"; }));
                    this.Invoke(new Action(() => { progressBar1.Value += step; }));
                    SendMsg("6996", "1");
                }
                catch (Exception ex)
                {
                    this.Invoke(new Action(() => { ShowException(ex); }));
                };
                ;
                return true;
            }
            );

            this.Invoke(new Action(() => { ProcessText.Text = "ждем смс от 6-ти значного номера или о том что услага оплачена"; }));
            this.Invoke(new Action(() => { progressBar1.Value += step; }));
            string addr6or0 = (string)waitSMS(
            (d) => { return (d?.OriginatingAddress.Length == 6 | d.UserDataText.StartsWith("Услуга оплачена")); },
            (d) =>
            {
                if (d.OriginatingAddress.Length == 6)
                {
                    this.Invoke(new Action(() => { ProcessText.Text = "отправка смс на " + d.OriginatingAddress; }));
                    this.Invoke(new Action(() => { progressBar1.Value += step; }));
                    try
                    {
                        SendMsg(d.OriginatingAddress, "1");
                    }
                    catch (Exception ex)
                    {
                        this.Invoke(new Action(() => { ShowException(ex); }));
                    };
                    ;
                    return d.OriginatingAddress;
                }
                else
                {
                    this.Invoke(new Action(() => { ProcessText.Text = "получено подтверждение оплаты"; }));
                    this.Invoke(new Action(() => { progressBar1.Value += step; }));
                    return "0";
                }
            }

            );
        }

        private void DoWork(object state)
        {
            var b = get_balance();
            this.Invoke(new Action(() => { progressBar1.Maximum = b; }));
            this.Invoke(new Action(() => { progressBar1.Value = 0; }));
            while (b >= 35)
            {
                if (b >= 300) { cashOut("8916", 300 / 5); } else
                if (b >= 100) { cashOut("2090", 100 / 5); } else
                { cashOut("7375", 35 / 5); };
                b = get_balance();
                this.Invoke(new Action(() => { progressBar1.Value = progressBar1.Maximum - (progressBar1.Maximum - b); }));
            }
            this.Invoke(new Action(() => { progressBar1.Value = progressBar1.Maximum; }));
            this.Invoke(new Action(() => { ProcessText.Text = "Снятие денег окончено. Баланс:" + b.ToString() + "р. Было переведо в ArcheAge " + (progressBar1.Maximum - b).ToString() + "р."; }));
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            comm.DeleteMessages(DeleteScope.All, PhoneStorageType.Sim);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SendMsg("6996", "1");
        }
    }
}
