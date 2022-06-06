using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BackGroundTest
{
    public partial class Form1 : Form
    {
        int elapseTotal = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            int elapsed = 0;
            elapseTotal = 0;
            int i = 1;
            e.Result = i;
            string filename = @"C:\Activa\EditionFiles\test space.dxf";
            //backgroundWorker1.ReportProgress(i++, filename);
            while (i < 8)
            {
                if (!backgroundWorker1.CancellationPending)
                {
                    backgroundWorker1.ReportProgress(i, filename);

                    //backgroundWorker1.ReportProgress(i++, filename);
                    //if (!File.Exists(objConfig.ActivePath + row["filename"].ToString() + ".pdf") && row["ref_n"].ToString() == "" && row["filename"].ToString() != "")
                    //    CreatePdf(objConfig.ActivePath, row["filename"].ToString(), row["ext"].ToString());
                    ////wait until pdf is created
                    while ((!File.Exists(filename)) && (elapsed < 5000))
                    {
                        Thread.Sleep(100);
                        elapsed += 100;
                        elapseTotal += 100;
                    }
                    if (File.Exists(filename)&& isFileLocked(filename))
                    {
                        elapsed = 0;
                        while (isFileLocked(filename) && (elapsed < 5000))
                        {
                            Thread.Sleep(1000);
                            elapsed += 1000;
                            elapseTotal += 1000;
                        }
                    }else if (File.Exists(filename) && !isFileLocked(filename))
                        i = 99;
                    ////if more than 5s has passed, cancel it
                    //if (elapsed >= 20000)
                    //{

                    //    backgroundWorker1.CancelAsync();
                    //}
                }
                i++;
                elapsed = 0;
                

                if (elapseTotal >= 20000)
                {

                    backgroundWorker1.CancelAsync();
                }


            }

            //if (elapsed >= 20000)
            //{

            //    backgroundWorker1.CancelAsync();
            //}

            e.Result = i;
            if (backgroundWorker1.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
            if (e.Cancelled == true)
            {
                MessageBox.Show("kantzelatu da " +elapseTotal );
            }
            else if (e.Error != null)
            {
                MessageBox.Show("akatsa da " + elapseTotal);
            }
            else
            {
                MessageBox.Show("bukatu da "+elapseTotal+" result "+ e.Result);
            }

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //MessageBox.Show("Progresoa da");
            label1.Text = elapseTotal+"";
        }


        private bool isFileLocked(string path)
        {
            //string path = @"c:\temp\MyTest.txt";
            FileInfo file = new FileInfo(path);

            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
                //file.MoveTo(@"C:\Activa\EditionFiles\DS\bukatu.txt");

            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;





            //try
            //{
            //    using (Stream stream = new FileStream(path, FileMode.Open))
            //    { // File/Stream manipulating code here } } catch { //check here why it failed and ask user to retry if the file is in use. }

            //        //Fuente: https://www.iteramos.com/pregunta/1769/-hay-alguna-forma-de-comprobar-si-un-archivo-esta-en-uso-

            //        return false;

            //}
            //}
            //catch
            //{ //check here why it failed and ask user to retry if the file is in use. }

            //    return true;

            //}

        }



    }
}
