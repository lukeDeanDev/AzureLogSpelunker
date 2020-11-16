using System;
using System.Net;
using System.Windows.Forms;
using AzureLogSpelunker.Forms;

namespace AzureLogSpelunker
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            catch (Exception e)
            {
                MainForm.MyMessageBox(e.ToString());
                throw;
            }
        }
    }
}
