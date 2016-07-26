using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Management;
using System.Net.NetworkInformation;
using System.Threading;
using System.IO;
using System.Text;

namespace AdvantageLauncher
{


    public partial class MainWindow : Form
    {
        private Thread thrStartup;
        private Thread thrUpdate;
        private Thread thrPing;
        private delegate void UpdateCheck();
        private delegate void PingCheck(string hostname);



        public MainWindow()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Thread thrStartup = new Thread(() => startUp());
            thrStartup.Start();
        }


        private bool checknetwork()
        {
            bool status = true;
            var path = AppDomain.CurrentDomain.BaseDirectory + "CheckNetwork.bat";
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = " /C call \"" + path + "\"";
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
            string isup = string.Empty;
            path = AppDomain.CurrentDomain.BaseDirectory + "isup.txt";
            using (StreamReader streamReader = new StreamReader(path, Encoding.UTF8))
            {
                isup = streamReader.ReadToEnd();
                streamReader.Close();
            }

            if (isup.Trim() == "up")
            {
                status = true;
            }
            else
            {

                status = false;
            }

            return status;
        }

        private void startUp()
        {
            bool isup = checknetwork();
            System.Threading.Timer timer = null;
            timer = new System.Threading.Timer((obj) =>
            {
                if (isup == true)
                {
                    getUpdate("no");
                    checkupdatedelayed(10000);
                }
                else
                {
                    MessageBox.Show("Cannot connect to Advantage server, please make sure you are on the VPN.");
                }
                timer.Dispose();
            },
             null, 3000, Timeout.Infinite);



        }



        private void getUpdate(string yesorno)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "Updater.bat";
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = " /C call \"" + path + "\" " + yesorno;
            process.StartInfo = startInfo;
            process.Start();
        }

        private bool updateCheck()
        {

            var path = AppDomain.CurrentDomain.BaseDirectory + "versioninfo.html";
            if (File.Exists(path))
            {
                string versioninfo = string.Empty;
                using (StreamReader streamReader = new StreamReader(path, Encoding.UTF8))
                {
                    versioninfo = streamReader.ReadToEnd();
                    streamReader.Close();
                }
                string[] ssize = versioninfo.Split(null);
                System.Version currentversion = new System.Version(ssize[1]);
                System.Version myversion = new System.Version(Application.ProductVersion);
                int test = currentversion.CompareTo(myversion);
                string result = Convert.ToString(test);
                if (test != 1)
                {
                    return true;
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Your software needs to be updated.\nWould you like to restart the application and update now?", "", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        getUpdate("yes");
                    }
                }

            }
            else
            {
                MessageBox.Show("Cannot locate update\n" + path);
            }


            return false;
        }



        private void checknetworkdelayed()
        {

            bool isup = checknetwork();
            System.Threading.Timer timer = null;
            timer = new System.Threading.Timer((obj) =>
            {

                if (isup == true)
                {
                    MessageBox.Show("Successfully connected to Advantage server");
                }
                else
                {
                    MessageBox.Show("Cannot connect to Advantage server, please make sure you are on the VPN.");
                }
                timer.Dispose();
            },
             null, 1000, Timeout.Infinite);
        }


        private void checkupdatedelayed(int seconds)
        {

            bool isup = checknetwork();
            System.Threading.Timer timer = null;
            timer = new System.Threading.Timer((obj) =>
            {
                if (isup == true)
                {
                    updateCheck();
                }
                else
                {
                    MessageBox.Show("No internet connection. Please connect to the internet");
                }
                timer.Dispose();
            },
             null, seconds, Timeout.Infinite);
        }


        private void networkCheck_Click(object sender, EventArgs e)
        {

            thrPing = new Thread(new ThreadStart(checknetworkdelayed));
            thrPing.Start();


        }
        private void updaterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread thrUpdate = new Thread(() => checkupdatedelayed(4000));
            thrUpdate.Start();

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
                Process.Start("cmd", "/C /b Taskkill /IM tsentry.exe /F").WaitForExit();
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


    }



}


