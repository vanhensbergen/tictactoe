using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace tictactoe.models
{
    public class Game: INotifyPropertyChanged
    {
        private const TicTacToe.Sign EMPTY = TicTacToe.Sign._;
        private const TicTacToe.Sign X = TicTacToe.Sign.X;
        private const TicTacToe.Sign O = TicTacToe.Sign.O;
        private const int GAME_SIZE = 9;
        private const TicTacToe.Status BUSY = TicTacToe.Status.Busy;
        private const TicTacToe.Status WIN = TicTacToe.Status.Win;
        private const TicTacToe.Status DRAW = TicTacToe.Status.Draw;
        //private const TicTacToe.Status LOSS = TicTacToe.Status.Loss;

        public TicTacToe.Sign ActiveSign { private set; get; }
        public TicTacToe.Sign PassiveSign { private set; get; }
        public List<Move> Moves { get; set; }
        public TicTacToe.Status GameStatus { private set; get; }
        public Action<TicTacToe.Status> OnGameEnded { get; set; }
        public Game()
        {
            InitializeGame();
            ActiveSign = TicTacToe.Sign.O;
            PassiveSign = TicTacToe.Sign.X;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
        private void InitializeGame()
        {
            Moves = new List<Move>(GAME_SIZE);
            for(int i = 0; i<GAME_SIZE; i++)
            {
                Moves.Add(new Move(i / 3, i % 3));
            }
            GameStatus = BUSY;
        }

        
        private bool Draw()
        {
            return !Win()&&(Moves.FindAll(x=>x.Sign==EMPTY).Count==0);
        }

        private bool Win(TicTacToe.Sign _Sign)
        {
            int _Index = 0;
            List<Move> Set;
            bool winner = false;
            do
            {
                Set = Moves.FindAll(x => x.Row == _Index && x.Sign == _Sign);
                if (Set.Count == 3)
                {
                    Set.ForEach(x => x.OneOfWinTriplet = true);
                    winner = true;
                }
                Set = Moves.FindAll(x => x.Column == _Index && x.Sign == _Sign);
                if (Set.Count == 3)
                {
                    Set.ForEach(x => x.OneOfWinTriplet = true);
                    winner = true;
                }
                _Index++;
            }
            while (_Index < 3);
            Set = Moves.FindAll(x => x.Row == x.Column && x.Sign == _Sign);
            if (Set.Count == 3)
            {
                Set.ForEach(x => x.OneOfWinTriplet = true);
                winner = true;
            }
            Set = Moves.FindAll(x => x.Row + x.Column == 2 && x.Sign == _Sign);
            if (Set.Count == 3)
            {
                Set.ForEach(x => x.OneOfWinTriplet = true);
                winner = true;
            }
            return winner;
        }

        private bool Win() => Win(X) || Win(O);

        public bool Accept(int row, int column)
        {
            if (Win())
            {
                return false;
            }
            if (Moves.Exists(x => x.Row == row && x.Column == column && x.Sign != EMPTY))
            {
                return false;
            }
            if (Draw())
            {
                return false;
            }
            return true;
        }

        private void Swap()
        {
            TicTacToe.Sign tmp;
            tmp = ActiveSign;
            ActiveSign = PassiveSign;
            PassiveSign = tmp;
        }

        public void Reset()
        {
            Moves.ForEach(x =>x.UnDo());
            Swap();
            GameStatus = BUSY;
            OnPropertyChanged("GameStatus");
        }

        public void DoMove(int row, int column)
        {
            int Index = Moves.FindAll(x => x.Sign != EMPTY).Count;
            Move theMove = Moves.Find(x => x.Row == row && x.Column == column);
            theMove.Sign = ActiveSign;
            theMove.Index = Index+1;
            if (Win())
            {
                GameStatus = WIN;
                OnPropertyChanged("GameStatus");
                OnGameEnded?.Invoke(GameStatus);
                return;
            }
            if (Draw())
            {
                GameStatus = DRAW;
                OnPropertyChanged("GameStatus");
                OnGameEnded?.Invoke(GameStatus);
                return;
            }
            Swap();
        }

        public bool Ended
        {
            get
            {
                return Win() || Draw();
            }
        }
        public bool HasLastMove()
        {
            Move LastMove = Moves.Find(x => x.Index == 1);
            return LastMove != null;
        }
        public void UndoLastMove()
        {
            int Index = Moves.FindAll(x => x.Sign != EMPTY).Count;
            Move LastMove = Moves.Find(x => x.Index == Index);
            if (LastMove == null)
            {
                return;
            }
            if (Win() || Draw())
            {
                GameStatus = BUSY;
                OnPropertyChanged("GameStatus");
                LastMove.UnDo();
                return;
            }
            LastMove.UnDo();
            Swap();  
        }
    }
}
