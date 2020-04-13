using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APK_Signer
{
    public partial class Main : Form
    {

        StreamWriter stdin = null;

        public Main()
        {
           
            InitializeComponent();
        }
        private void Main_Load(object sender, EventArgs e)
        {

            textBox1.Enabled = false;
            monoFlat_TextBox1.Enabled = false;
          
        }


        private void monoFlat_ThemeContainer1_DragEnter(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length != 0)
            {
                if(monoFlat_TextBox1.Text.Equals("")) {
                    textBox1.Text = "";
                    textBox1.ForeColor = Color.Red;

                    monoFlat_TextBox1.Text = "java -jar u.jar --apks " + files[0];
                    textBox1.Text += files[0] + Environment.NewLine;
                }else{
                    monoFlat_TextBox1.Text += " "+files[0];
                    textBox1.Text += files[0] + Environment.NewLine;
                    monoFlat_Button1.Text = "SIGNER APKS";
                } 
            }
        }


        private void monoFlat_Button1_Click(object sender, EventArgs e)
        {

            textBox1.ForeColor = Color.LawnGreen;
            textBox1.Text = "";
            StartCmdProcess();
            //stdin.WriteLine();
            stdin.Write(monoFlat_TextBox1.Text + Environment.NewLine);
            stdin.WriteLine();
            
        }
        private void StartCmdProcess()
        {
            ProcessStartInfo pStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = "start /WAIT",
                WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            Process cmdProcess = new Process
            {
                StartInfo = pStartInfo,
                EnableRaisingEvents = true,
            };

            cmdProcess.Start();
            cmdProcess.BeginErrorReadLine();
            cmdProcess.BeginOutputReadLine();
            stdin = cmdProcess.StandardInput;

            cmdProcess.OutputDataReceived += (s, evt) =>
            {
                if (evt.Data != null)
                {
                    BeginInvoke(new MethodInvoker(() =>
                    {
                        textBox1.AppendText(evt.Data + Environment.NewLine);
                        textBox1.ScrollToCaret();
                        monoFlat_TextBox1.Text = "";
                    }));
                }
            };

            cmdProcess.ErrorDataReceived += (s, evt) =>
            {
                if (evt.Data != null)
                {
                    BeginInvoke(new Action(() =>
                    {
                        //rtbStdErr.AppendText(evt.Data + Environment.NewLine);
                        //rtbStdErr.ScrollToCaret();
                    }));
                }
            };

            cmdProcess.Exited += (s, evt) =>
            {
                // cmdProcess?.Dispose();
            };
        }


        private void monoFlat_TextBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
        }



    }
}
