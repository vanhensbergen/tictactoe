using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using tictactoe.commands;
using tictactoe.models;

namespace tictactoe.viewmodels
{
    public class MainViewModel:INotifyPropertyChanged
    {
        public Game Game { get; set; }
        public List<Player> Players { get; set; }

        public Player ActivePlayer { 
            get 
            {
                TicTacToe.Sign ActiveSign = Game.ActiveSign;
                return Players.Find(x => x.Sign == ActiveSign);
            } 
        }
        public Player PassivePlayer
        {
            get
            {
                TicTacToe.Sign PassiveSign = Game.PassiveSign;
                return Players.Find(x => x.Sign ==PassiveSign);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        public MainViewModel()
        {
            Game = new Game();
            Players = new List<Player>(2)
            {
                new Player(TicTacToe.Sign.O, "SpelerO"),
                new Player(TicTacToe.Sign.X, "SpelerX")
            };
            Game.OnGameEnded= OnGameEnded;

            MoveRequestedCommand = new RelayCommand(CanExecuteMove, ExecuteMove);
            ActionCommand = new RelayCommand(CanExecuteAction, ExecuteAction);
        }
        private void OnGameEnded(TicTacToe.Status status)
        {
            switch (status)
            {
                case TicTacToe.Status.Win:
                    ActivePlayer.Wins += 1;
                    PassivePlayer.Losses += 1;
                    break;
                case TicTacToe.Status.Draw:
                    ActivePlayer.Draws += 1;
                    PassivePlayer.Draws += 1;
                    break;
                default:
                    throw new NotImplementedException("unknown ending status of game");
            }
        }
        private void ExecuteAction(object obj)
        {
            string Action = obj.ToString();
            switch (Action)
            {
                case "undo":
                    Game.UndoLastMove();
                    break;
                case "new":
                    Game.Reset();
                    break;
                case "clear":
                    Players.ForEach(x => x.ResetScore());
                    break;
            }
            OnPropertyChanged("ActivePlayer");
            OnPropertyChanged("PassivePlayer");
        }

        private bool CanExecuteAction(object obj)
        {
            string Action = obj.ToString();
            switch (Action)
            {
                case "undo":
                    return !Game.Ended && Game.HasLastMove();
                case "new":
                    return Game.Ended;
                case "clear":
                    return true;
            }
            return false;
        }

        private void ExecuteMove(object obj)
        {
            Point p = (Point)obj;
            int Row = (int)p.X;
            int Column = (int)p.Y;
            Game.DoMove(Row, Column);
            OnPropertyChanged("ActivePlayer");
            OnPropertyChanged("PassivePlayer");


        }

        private bool CanExecuteMove(object obj)
        {
            Point p = (Point)obj;
            int Row = (int)p.X;
            int Column = (int)p.Y;
            return Game.Accept(Row, Column);
        }

        

        public ICommand MoveRequestedCommand { get; set; }

        public ICommand ActionCommand{ get;set; }
    }
}
