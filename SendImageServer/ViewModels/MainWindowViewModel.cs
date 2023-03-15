using SendImageServer.Commands;
using SendImageServer.Helpers;
using SendImageServer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Instrumentation;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SendImageServer.ViewModels
{
    public class MainWindowViewModel:BaseViewModel
    {
        public Socket socket = null;

        public DispatcherTimer timer { get; set; }
        public RelayCommand StartCommand { get; set; }

        private ObservableCollection<Item> images;

        public ObservableCollection<Item> Images
        {
            get { return images; }
            set { images = value;OnPropertyChanged(); }
        }

        public MainWindow window { get; set; }


        public MainWindowViewModel(MainWindow mw)
        {
            window= mw;
            timer= new DispatcherTimer();
            timer.Interval=new TimeSpan(0,0,1);
            timer.Tick += CheckImages;
            Images= new ObservableCollection<Item>();
            StartCommand = new RelayCommand((o) =>
            {
                var ipAdress = IPAddress.Parse(IpHelper.GetLocalIpAddress());
                var port = 80;
                socket=new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
                var endpoint=new IPEndPoint(ipAdress, port);
                socket.Bind(endpoint);
                socket.Listen(10);
                MessageBox.Show("Connected");
                Task.Run(() =>
                {
                    timer.Start();
                });
            });

        }

        private void CheckImages(object sender, EventArgs e)
        {
            if (socket!=null)
            {
                Task.Run(() =>
                {
                    try
                    {
                        var client = socket.Accept();
                        Task.Run(() =>
                        {
                            var lenght = 0;
                            var bytes=new byte[1024];
                            lenght= client.Receive(bytes);
                            var image = ImageHelper.SaveAndGetImagePath(bytes);
                            window.Dispatcher.Invoke(() =>
                            {
                                Images.Add(new Item
                                {
                                    ImagePath=image
                                });

                                
                            });

                        });
                    }
                    catch (Exception)
                    {

                        
                    }
                });
            }
        }
    }
}
