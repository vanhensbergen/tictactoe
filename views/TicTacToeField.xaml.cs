using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using tictactoe.models;

namespace tictactoe.views
{
    /// <summary>
    /// Interaction logic for TicTacToeField.xaml
    /// </summary>
    public partial class TicTacToeField : UserControl
    {

        private readonly Dictionary<TicTacToe.Sign, Shape> Shapes;
        private Point _Location = new Point();
        private Brush _SignalColor;

        public TicTacToeField()
        {
            Shapes = new Dictionary<TicTacToe.Sign, Shape>();
            InitializeComponent();
            FillShapes();
            canvas.Children.Add(Shapes[TicTacToe.Sign._]);
            _SignalColor = Brushes.Red;
            //handler for sizechange occurrence of the control,
            SizeChanged += AdjustCircle;

        }

        private void FillShapes()
        {
            Polygon myPolygon = new Polygon
            {
                Stroke = Brushes.Black,
                Fill = Brushes.LightSeaGreen,
                StrokeThickness = 2,
                Stretch = Stretch.Fill,
                Points = new PointCollection
                {
                    new Point(0,0),
                    new Point(1, 0),
                    new Point(5, 4),
                    new Point(9, 0),
                    new Point(10, 0),
                    new Point(10,1),
                    new Point(6, 5),
                    new Point(10,9),
                    new Point(10, 10),
                    new Point(9, 10),
                    new Point(5, 6),
                    new Point(1, 10),
                    new Point(0, 10),
                    new Point(0,9),
                    new Point(4, 5),
                    new Point(0,1)
                }
            };
            Ellipse myEllipse = new Ellipse
            {
                Stroke = Brushes.Black,
                StrokeThickness=5,
                Stretch = Stretch.Fill
            };

            Rectangle rect = new Rectangle
            {
                Stroke = Brushes.Black,
                StrokeThickness = 2,
                Fill = Brushes.Transparent,
                Stretch = Stretch.Fill
            };

            Shapes.Add(TicTacToe.Sign._, rect);
            Shapes.Add(TicTacToe.Sign.X, myPolygon);
            Shapes.Add(TicTacToe.Sign.O, myEllipse);
        }


        private void AdjustCircle(object sender, SizeChangedEventArgs e)
        {
            TicTacToeField control = sender as TicTacToeField;
            double width = e.NewSize.Width;
            double height = e.NewSize.Height;
            double squareSize = Math.Min(width, height);
            control.Width = squareSize;
            control.Height = squareSize;
            control.Shapes.TryGetValue(TicTacToe.Sign.O, out Shape Circle);
            Circle.Width = squareSize - 6 < 0 ? squareSize : squareSize - 6;
            Circle.Height = Circle.Width;
            Circle.StrokeThickness = 0.15 * squareSize;
        }
       

        //all having to do with a dependencyproperty Sign 

        public TicTacToe.Sign Sign
        {
            get { return (TicTacToe.Sign)GetValue(SignProperty); }
            set { SetValue(SignProperty, value); }
        }

        public static readonly DependencyProperty SignProperty =
            DependencyProperty.Register("Sign", typeof(TicTacToe.Sign), typeof(TicTacToeField), new UIPropertyMetadata(TicTacToe.Sign._, new PropertyChangedCallback(OnSignChanged)));

        private static void OnSignChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TicTacToeField control = d as TicTacToeField;
            control.OnSignChanged(e);
        }

        private void OnSignChanged(DependencyPropertyChangedEventArgs e)
        {
            TicTacToe.Sign sign = (TicTacToe.Sign)e.NewValue;
            /*
            canvas.Children.Clear();
            canvas.Children.Add(Shapes[TicTacToe.Sign._]);
            if(sign==TicTacToe.Sign._)
            { return;  }
            canvas.Children.Add(Shapes[sign]);
            */

            if (canvas.Children.Count == 2)
            {
                canvas.Children.RemoveAt(1);
            }
            if (sign == TicTacToe.Sign._)
            {
                return;
            }
            canvas.Children.Add(Shapes[sign]);

            
            
        }

        /*
         * ALL HAVING TO DO WITH THE DEPENDENCYPROPERTY STROKE
         */
        public Brush Stroke
        {
            get
            {
                return (Brush)GetValue(StrokeProperty);
            }
            set
            {
                SetValue(StrokeProperty, value);


            }
        }

        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register("Stroke", typeof(Brush), typeof(TicTacToeField), new UIPropertyMetadata(Brushes.Black, new PropertyChangedCallback(OnStrokeChanged)));


        private static void OnStrokeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TicTacToeField control = d as TicTacToeField;
            control.OnStrokeChanged(e);
        }

        private void OnStrokeChanged(DependencyPropertyChangedEventArgs e)
        {
            Brush brush = (Brush)e.NewValue;
            foreach(KeyValuePair<TicTacToe.Sign, Shape> item in Shapes)
            {
                item.Value.Stroke = brush;
            }
        }
        
        public bool Signal
        {   get{   return (bool)GetValue(SignalProperty);  }
            set {  SetValue(SignalProperty, value); }
        }

        public static readonly DependencyProperty SignalProperty =
            DependencyProperty.Register("Signal", typeof(bool), typeof(TicTacToeField), new UIPropertyMetadata(false, new PropertyChangedCallback(OnSignalChanged)));

        private static void OnSignalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TicTacToeField control = d as TicTacToeField;
            control.OnSignalChanged(e);
        }

        private void OnSignalChanged(DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == true)
            {
                Shapes[TicTacToe.Sign._].Fill = _SignalColor;
            }
            else
            {
                Shapes[TicTacToe.Sign._].Fill = Brushes.Transparent;
            }
        }

        public Brush SignalColor
        {
            get { return (Brush)GetValue(SignalColorProperty); }
            set { SetValue(SignalColorProperty, value); }
        }
        public static readonly DependencyProperty SignalColorProperty =
            DependencyProperty.Register("SignalColor", typeof(Brush), typeof(TicTacToeField), new UIPropertyMetadata(Brushes.Red, new PropertyChangedCallback(OnSignalColorChanged)));

        private static void OnSignalColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TicTacToeField control = d as TicTacToeField;
            control.OnSignalColorChanged(e);
        }

        private void OnSignalColorChanged(DependencyPropertyChangedEventArgs e)
        {
            _SignalColor = (Brush)e.NewValue;
        }

        /*
         * ALL HAVING TO DO WITH A DEPENDENCYPROPERTY ROW
         */
        public int Row
        {   
            get{    return (int)GetValue(RowProperty);}
            set{  SetValue(RowProperty, value);  }
        }

        public static readonly DependencyProperty RowProperty = 
            DependencyProperty.Register("Row", typeof(int), typeof(TicTacToeField), new UIPropertyMetadata( new PropertyChangedCallback(OnRowChanged)));

        private static void OnRowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TicTacToeField control = d as TicTacToeField;
            control.OnRowChanged(e);
        }

        private void OnRowChanged(DependencyPropertyChangedEventArgs e)
        {
            _Location.X = (int)e.NewValue;
            SetValue(LocationPropertyKey, _Location);
        }

        public int Column
        {
            get{  return (int)GetValue(ColumnProperty);}
            set{ SetValue(ColumnProperty, value); }
        }


        public static readonly DependencyProperty ColumnProperty = 
            DependencyProperty.Register("Column", typeof(int), typeof(TicTacToeField), new UIPropertyMetadata(new PropertyChangedCallback(OnColumnChanged)));

        private static void OnColumnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TicTacToeField control = d as TicTacToeField;
            control.OnColumnChanged(e);
        }

        private void OnColumnChanged(DependencyPropertyChangedEventArgs e)
        {
            _Location.Y = (int)e.NewValue;
            SetValue(LocationPropertyKey, _Location);
        }

        internal static readonly DependencyPropertyKey LocationPropertyKey =
            DependencyProperty.RegisterReadOnly("Location", typeof(Point), typeof(TicTacToeField), new PropertyMetadata(null));
        public static readonly DependencyProperty LocationProperty = LocationPropertyKey.DependencyProperty;
        
        public Point Location
        {
            get{  return (Point)GetValue(LocationProperty);}   
        }
//dependencyproperty for commands
        public static readonly DependencyProperty CommandProperty =
           DependencyProperty.Register("Command", typeof(ICommand), typeof(TicTacToeField));

        public ICommand Command
        {
            get{  return (ICommand)GetValue(CommandProperty);}
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandParameterProperty =
           DependencyProperty.Register("CommandParameter", typeof(object), typeof(TicTacToeField));

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }
    }
}
