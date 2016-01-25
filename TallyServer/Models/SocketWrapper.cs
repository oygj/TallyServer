using System;
using System.ComponentModel;
using System.Text;
using System.Windows.Media;
using Skt_05;

namespace TallyServer.Models
{
    
    public class SocketWrapper : INotifyPropertyChanged
    {

        private Brush disconnectedBrushColor = (Brush)new BrushConverter().ConvertFromString("Red");
        private Brush connectedBrushColor = (Brush)new BrushConverter().ConvertFromString("Green");

        private int _portNumber;
        private Brush _statusColor;
        private bool _isListening;
        private string _buttonText;
        private string _logMessages = string.Empty;
        private  Connection_Mgr serverSocket;


        public int PortNumber 
        {
            get { return _portNumber; }

            set
            {
                _portNumber = value;
                OnPropertyChanged("PortNumber");
                
            }
        }

        public Brush StatusColor
        {
            get { return _statusColor; }
            set
            {
                _statusColor = value;
                OnPropertyChanged("StatusColor");
            }
        }

        public bool IsListening 
        { 
            get {return _isListening;}
            set
            {
                _isListening = value;
                OnPropertyChanged("IsListening");
            }
        }

        public string ButtonText
        {
            get { return _buttonText; }
            set
            {
                _buttonText = value;
                OnPropertyChanged("ButtonText");
            }
        }

        public string LogMessages
        {
            get { return _logMessages; }
        }

        internal void AppendLogMessage(string message)
        {
            _logMessages += message + Environment.NewLine;
            OnPropertyChanged("LogMessages");
        }

        public void StartListening()
        {
            if (this.IsListening)
            {
                serverSocket.ShutDown();
                StatusColor = disconnectedBrushColor;

                this.ButtonText = "Start Listening...";
                this.IsListening = false;

                AppendLogMessage("Stopped listening...");
            }
            else
            {
                ConfigureSockets();
                serverSocket.StartListening();

                this.ButtonText = "Stop Listening";
                this.IsListening = true;

                AppendLogMessage("Listening for incoming connections...");
            }
        }

        //Constructor
        public SocketWrapper()
        {
            //Default values
            StatusColor = disconnectedBrushColor;
            ButtonText = "Start Listening...";
            PortNumber = 10570;

            ConfigureSockets();
        }

        private void ConfigureSockets()
        {
            serverSocket = new Connection_Mgr();
            serverSocket.Port = PortNumber;
            serverSocket.SktEncoding = Encoding.Default;

            serverSocket.ConnectionAccepted += serverSocket_ConnectionAccepted;
            serverSocket.Closed += serverSocket_Closed;
            serverSocket.MsgReceived += serverSocket_MsgReceived;

            IsListening = false;
        }

        private void serverSocket_MsgReceived(string sMsg, Skt_Wrapper Wrapper)
        {
            AppendLogMessage(sMsg);
        }

        private void serverSocket_Closed(Skt_Wrapper Wrapper)
        {
            if (serverSocket.Count == 0)
                StatusColor = disconnectedBrushColor;

            AppendLogMessage(string.Format("Disc {0}", Wrapper.RemoteInfo.ToString()));
        }

        private void serverSocket_ConnectionAccepted(Skt_Wrapper Wrapper)
        {
            if (serverSocket.Count > 0)
                StatusColor = connectedBrushColor;

            AppendLogMessage(string.Format("Con from {0}", Wrapper.RemoteInfo.ToString()));
        }


        //INotifyPropertyChanged implementation...
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }


    }
}
