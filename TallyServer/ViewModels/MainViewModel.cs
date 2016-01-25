using System.Windows.Input;
using TallyServer.CommandBehavior;

namespace TallyServer.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private Models.SocketWrapper _dataModel;
        private ICommand buttonCommand;

        public ICommand ButtonCommand 
        { 
            get {return buttonCommand;}
            set
            {
                buttonCommand = value;

            }
        }

        public Models.SocketWrapper DataModel
        {
            get { return _dataModel; }
            set
            {
                _dataModel = value;
                OnPropertyChanged("DataModel");
            }
        }

        //Constructor
       public MainViewModel()
        {
            DataModel = new Models.SocketWrapper();

            buttonCommand = new SimpleCommand
            {
                ExecuteDelegate = x => startListening(),
                CanExecuteDelegate = y => true
            };

        }

       private void startListening()
       {
           _dataModel.StartListening();
       }


    }
}
