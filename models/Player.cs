using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tictactoe.models
{
    public class Player:INotifyPropertyChanged
    {
        public string _Name;
        private readonly string DefaultName;
        public TicTacToe.Sign Sign { get; }
        private int _Wins;
        public int _Losses;
        public int _Draws;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
        public Player(TicTacToe.Sign sign, string defaultName)
        {
            Sign = sign;
            Name = defaultName;
            DefaultName = Name;
        }
        
        public string Name
        {
            get { return _Name; }
            set { 
                    _Name = value; 
                    if(string.IsNullOrWhiteSpace(_Name))
                    {
                        _Name = DefaultName;
                    }
                    OnPropertyChanged("Name");
            }
        }
        public int Wins
        {
            get{  return _Wins;  }
            set { _Wins = value; OnPropertyChanged("Wins"); }
        }

        public int Draws
        {
            get { return _Draws; }
            set { _Draws = value; OnPropertyChanged("Draws"); }
        }

        public int Losses
        {
            get { return _Losses; }
            set { _Losses = value; OnPropertyChanged("Losses"); }
        }
        public void ResetScore()
        {
            Wins = 0;
            Losses = 0;
            Draws = 0;
        }

    }
}
