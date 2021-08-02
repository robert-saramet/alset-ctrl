using System;
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

using System.IO.Ports;
using System.Threading;

namespace Alset_CTRL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string port;
        private int baud;

        static SerialPort serial;

        public delegate void SerialUpdateDelegate();

        public MainWindow()
        {
            InitializeComponent();
        }

        protected void PortChanged(object sender, SelectionChangedEventArgs e)
        {
            string portLong = this.PortComboBox.SelectedItem.ToString();
            port = portLong.Substring(portLong.LastIndexOf(' ') + 1);
            if (baud == 0) baud = 115200;
        }

        protected void BaudChanged(object sender, SelectionChangedEventArgs e)
        {
            string baudLong = this.BaudComboBox.SelectedItem.ToString();
            baudLong = baudLong.Substring(baudLong.LastIndexOf(' ') + 1);
            int.TryParse(baudLong, out baud);
        }

        protected void ConnectClicked(object sender, RoutedEventArgs e)
        {
            //var connection = new SharerConnection(port, baud);
            MessageBox.Show($"Port: {port}\nBaud: {baud}");
            serial = new SerialPort();
            serial.PortName = port;
            serial.BaudRate = baud;
            try
            {
                serial.Open();
                ConnectButton.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.SystemIdle, new SerialUpdateDelegate(this.UpdateSerial));
            }
            catch (System.IO.IOException)
            {
                MessageBox.Show("Invalid port selected");
            }
            
        }

        private void DisconnectClicked(object sender, RoutedEventArgs e)
        {

        }

        public void UpdateSerial()
        {
            string payload = serial.ReadExisting();
            MessageBox.Show(payload);
            ConnectButton.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.SystemIdle, new SerialUpdateDelegate(this.UpdateSerial));
        }
    }
}
