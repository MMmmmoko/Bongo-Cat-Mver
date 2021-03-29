using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Toolkit.Wpf.UI.Controls;
using System.Net;
using System.Threading;
using System.Windows.Threading;
using System.ComponentModel;
using System.Diagnostics;

namespace MyWpfApp
{
    /// <summary>
    /// PageUpdate.xaml 的交互逻辑
    /// </summary>
    /// 
    public partial class PageUpdate : Page
    {
        String verson1, verson2, verson3, verson4,downloadlink;

        private void click_download(object sender, RoutedEventArgs e)
        {
            Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = downloadlink;
            proc.Start();
        }

        static updateStatus updatestatus = new updateStatus();


        public PageUpdate()
        {
            InitializeComponent();
            //DataContext = CommonData.version;
            DataContext = updatestatus;
            GridForLoadingSuccess.DataContext = CommonData.version;
            GridForLoadingSuccess.Visibility = Visibility.Collapsed;
            GridForLoadingFailure.Visibility = Visibility.Collapsed;
            updatestatus.isloading = true;
            Thread getVersionThread = new Thread(new ThreadStart(ReadVerson));
            getVersionThread.Start();


        }

        public void ReadVerson()
        {
            try
            {
                HttpWebRequest oHttp_Web_Req = (HttpWebRequest)WebRequest.Create();
                oHttp_Web_Req.Timeout = 5000;
                HttpWebResponse httpWebResponse = (HttpWebResponse)oHttp_Web_Req.GetResponse();
                if (httpWebResponse.StatusCode != HttpStatusCode.RequestTimeout)
                {
                    Stream oStream = oHttp_Web_Req.GetResponse().GetResponseStream();
                    using (StreamReader respStreamReader = new StreamReader(oStream, Encoding.UTF8))
                    {
                        verson1 = respStreamReader.ReadLine();
                        verson2 = respStreamReader.ReadLine();
                        verson3 = respStreamReader.ReadLine();
                        verson4 = respStreamReader.ReadLine();
                        downloadlink= respStreamReader.ReadLine();
                    }
                    CommonData.version.is_lastest = version.is_lastestversion(verson1, verson2, verson3, verson4);
                    this.Dispatcher.BeginInvoke((Action)delegate ()
                    {
                        //this.textBox2.Text = CalcSum((Int64)inputNumber).ToString();  
                        updatestatus.loading_result = true;
                        updatestatus.isloading = false;
                        GridForLoadingSuccess.Visibility = Visibility.Visible;
                        serverframe.Refresh();
                        
                    });

                }
                


            }
            catch (WebException exc)
            {
                

                verson1 = version.version1;
                verson2 = version.version2;
                verson3 = version.version3;
                verson4 = version.version4;

                this.Dispatcher.BeginInvoke((Action)delegate ()
                {
                    //this.textBox2.Text = CalcSum((Int64)inputNumber).ToString();  
                    GridForLoadingFailure.Visibility = Visibility.Visible;
                });

            }
            this.Dispatcher.BeginInvoke((Action)delegate ()
            {
                //this.textBox2.Text = CalcSum((Int64)inputNumber).ToString(); 
                
                updatestatus.isloading = false;
                updatestatus.loading_result = false;
            });
        }


    }





    public class updateStatus : INotifyPropertyChanged
    {
        private bool _isloading = false;
        public bool isloading
        {
            get { return _isloading; }
            set
            {
                _isloading = value; OnPropertyChanged("isloading");
            }
        }
        private bool _loading_result = false;
        public bool loading_result
        {
            get { return _loading_result; }
            set
            {
                _loading_result = value; OnPropertyChanged("loading_result");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }


}

