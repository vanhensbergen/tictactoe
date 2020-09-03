using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace tictactoe.models
{
    public class Move:INotifyPropertyChanged
    {
        public TicTacToe.Sign Sign
        {
            get
            {
                return _Sign;
            }
            set
            {
                _Sign = value;
                OnPropertyChanged("Sign");
            }
        }
            
        public int Row { get; }
        public int Column { get; }
        private TicTacToe.Sign _Sign;
        private bool _OneOfWinTriplet;
        public int? Index { get; set; }

        public bool OneOfWinTriplet
        {
            get { return _OneOfWinTriplet; }
            set
            {
                _OneOfWinTriplet = value;
                OnPropertyChanged("OneOfWinTriplet");

            }
        }

        public Move(int _Row, int _Column)
        {
            Row = _Row;
            Column = _Column;
            Sign = TicTacToe.Sign._;
            OneOfWinTriplet = false;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        public void UnDo()
        {
            Sign = TicTacToe.Sign._;
            OneOfWinTriplet = false;
            Index = null;
        }
    }
    
}
