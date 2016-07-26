using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Net.NetworkInformation;
using System.Net;

namespace AdvantageLauncher
{


    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            checknetworkdelayed();
        }


        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;
            // Starts TabletInputService
            startServ();

        }


        private void showButton_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox1.BackColor = Color.LimeGreen;
                checkBox1.Text = "Start Timekeeper";
                Process.Start("cmd", "/C Taskkill /IM tsentry.exe /F").WaitForExit();
                startServ();

            }
            else
            {
                checkBox1.BackColor = Color.Red;
                checkBox1.Text = "Stop Timekeeper";
                stopServ();
                Process.Start(@"C:\Program Files (x86)\Deltek\Advantage\9.1\tsentry.exe");

            }

        }
        private void showButton2_Click(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                checkBox2.BackColor = Color.LimeGreen;
                checkBox2.Text = "Start Expensekeeper";
                Process.Start("cmd", "/C Taskkill /IM expense.exe /F").WaitForExit();
                startServ();

            }
            else
            {
                checkBox2.BackColor = Color.Red;
                checkBox2.Text = "Stop Expensekeeper";
                stopServ();
                Process.Start(@"C:\Program Files (x86)\Deltek\Advantage\9.1\expense.exe");

            }

        }
        private void showButton3_Click(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                checkBox3.BackColor = Color.LimeGreen;
                checkBox3.Text = "Start Advantage";
                Process.Start("cmd", "/C Taskkill /IM DeltekAdvantage.exe /F").WaitForExit();
                startServ();

            }
            else
            {
                checkBox3.BackColor = Color.Red;
                checkBox3.Text = "Stop Advantage";
                stopServ();
                Process.Start(@"C:\Program Files (x86)\Deltek\Advantage\9.1\DeltekAdvantage.exe");

            }

        }

        private void showButton4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("iexplore", "https://sp.thermaltech.com/Office%20Resources/Advantage%20Errors.aspx");
        }

        private void showButton5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please send details of the error and a screenshot to redmine.thermaltech@gmail.com");
            System.Diagnostics.Process.Start("mailto:redmine.thermaltech@gmail.com");
        }

        private void startServ()
        {
            var serviceName = "TabletInputService";
            string objPath = string.Format("Win32_Service.Name='{0}'", serviceName);
            using (var service = new ManagementObject(new ManagementPath(objPath)))
            {
                service.InvokeMethod("ChangeStartMode", new object[] { "Automatic" });
            }
            Process.Start("net", "start TabletInputService").WaitForExit();
            using (var service = new ManagementObject(new ManagementPath(objPath)))
            {
                service.InvokeMethod("ChangeStartMode", new object[] { "Automatic" });
            }

        }
        private void stopServ()
        {
            Process.Start("net", "stop TabletInputService").WaitForExit();
            Process.Start("sc", "config TabletInputService start= disabled").WaitForExit();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            AboutBox1 a = new AboutBox1();
            a.Show();
        }

        private bool checknetwork()
        {
            bool status = true;
            double pingtime;
                status = PingHost("advan.thermaltech.com");
                if (status == false)
                {
                    pingtime = PingTimeAverage("192.168.0.26", 4);
                if (pingtime == 0)
                {
                    MessageBox.Show("Cannot connect to Advantage server, please make sure you are on the VPN "+pingtime);
                }
                }        
            return status;
        }


        public bool PingHost(string nameOrAddress)
        {
            bool pingable = false;
            Ping pinger = new Ping();
            try
            {
                PingReply reply = pinger.Send(nameOrAddress);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
            }
            return pingable;

        }

        public double PingTimeAverage(string host, int echoNum)
        {
            long totalTime = 0;
            int timeout = 120;
            Ping pingSender = new Ping();
            try
            {
                for (int i = 0; i < echoNum; i++)
                {
                    PingReply reply = pingSender.Send(host, timeout);
                    if (reply.Status == IPStatus.Success)
                    {
                        totalTime += reply.RoundtripTime;
                    }
                }
            }
            catch (PingException)
            {
                return 0;
            }

            return totalTime / echoNum;
        }

        public void checknetworkdelayed()
        {

            System.Threading.Timer timer = null;
            timer = new System.Threading.Timer((obj) =>
            {
                updateCheck();
                checknetwork();
                timer.Dispose();
            },
              null, 500, System.Threading.Timeout.Infinite);


        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            bool networkstatus = checknetwork();
            if (networkstatus == true)
            {
                MessageBox.Show("Successfully connected to Advantage server");
            }

        }
        private void updaterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool needUpdate = updateCheck();
            if (needUpdate == true)
            {
                MessageBox.Show("Your version is up to date.");
            }

        }

        private bool updateCheck()
        {
            try
            {
                String versioninfo;
                WebClient web = new WebClient();
                System.IO.Stream stream = web.OpenRead("https://raw.githubusercontent.com/gigglesbw4/AdvantageLauncher/master/versioninfo.html");
                using (System.IO.StreamReader reader = new System.IO.StreamReader(stream))
                {
                    versioninfo = reader.ReadToEnd();
                    reader.Close();
                }
                string[] ssize = versioninfo.Split(null);
                System.Version currentversion = new System.Version(ssize[1]);
                System.Version myversion = new System.Version(Application.ProductVersion);
                int test = currentversion.CompareTo(myversion);
                string result = Convert.ToString(test);
                if (test == 1)
                {
                    DialogResult dialogResult = MessageBox.Show("Your software needs to be updated.\nWould you like to restart the application and update now?", "", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        var path = AppDomain.CurrentDomain.BaseDirectory + "Updater.bat";
                        Process.Start(path);
                    }
                    return false;

                }
                else
                {
                    return true;
                }
            }
            catch (WebException)
            {
                MessageBox.Show("No internet connection. Please connect to the internet");
                return false;
            }
        }

     
    }



}


