using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmeticShampoo.Server.ViewModels
{
    public class Log : Notifier
    {
        private string _logView;
        public string logView
        {
            get
            {
                return _logView;
            }

            set
            {
                this._logView = value;
                OnPropertyChanged("logView");
            }
        }
    }
}
