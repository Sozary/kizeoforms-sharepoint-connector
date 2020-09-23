using System;
using System.ComponentModel;
using System.Net;

namespace KizeoAndSharepoint_wizard
{
    class Start
    {

        Loading loading;
        public Start()
        {
            WebClient webClient = new WebClient();
            try
            {
                if (!webClient.DownloadString("https://raw.githubusercontent.com/Sozary/kizeoforms-sharepoint-connector/master/version.txt").Contains(GetType().Assembly.GetName().Version.ToString()))
                {
                    var msgbox = System.Windows.Forms.MessageBox.Show("A new version is available, do you want to download it?", "KizeoForms SharePoint Connector", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question);
                    if (msgbox == System.Windows.Forms.DialogResult.Yes)
                    {
                        Uri uri = new Uri("https://github.com/Sozary/kizeoforms-sharepoint-connector/blob/master/wizard.zip?raw=true");
                        loading = new Loading();
                        loading.Show();
                        webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadComplete);
                        webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressed);
                        webClient.DownloadFileAsync(uri, @"wizard.zip");
                    }
                    else if(msgbox == System.Windows.Forms.DialogResult.No)
                    {
                        Launch(false);
                    }
                }
            }
            catch
            {
                Launch(false);
            }
        }

        private void Launch(bool instanced = true)
        {
        
            if (instanced)
            {
                loading.Dispatcher.Invoke(new Action(() =>
                {
                    Step1 step1 = new Step1();
                    step1.Show();
                }));
            }
            else
            {
                Step1 step1 = new Step1();
                step1.Show();
            }

        }

        private void DownloadComplete(object sender, AsyncCompletedEventArgs e)
        {
            loading.Dispatcher.Invoke(new Action(() =>
            {
                loading.Hide();
            }));
            Launch();
        }

        private void DownloadProgressed(object sender, DownloadProgressChangedEventArgs e)
        {
            loading.Dispatcher.Invoke(new Action(() =>
            {
                loading.dlStatus.Value = e.ProgressPercentage;
            }));
        }
    }
}
