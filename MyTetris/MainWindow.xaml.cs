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

namespace MyTetris
{
    public partial class MainWindow : Window
    {
        private static int rows = 22;
        private static int cols = 10;
        private int[,] Field = new int[cols, rows];
        private int[,] Piece = new int[cols, rows];
        private Tetramino tetramino;

        public MainWindow()
        {
            InitializeComponent();
            drawfield();
        }

        private void drawfield()
        {
            try
            {
                var element = GridField.Children
        .OfType<Canvas>()
        .FirstOrDefault(e => e.Name == "Cstack");
                GridField.Children.Remove(element);
            }
            catch { }

            if (tetramino!=null)
                drawpiece();

            Canvas stack = new Canvas();
            stack.Name = "Cstack";
            stack.Width = 310;
            stack.Height = 620;
            for (int i = 2; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Rectangle cell = new Rectangle();
                    if (tetramino!=null && Piece[j, i] != 0)
                        cell.Fill = paint(Piece[j, i]);
                    else
                        cell.Fill = paint(Field[j, i]);
                    cell.StrokeThickness = 1;
                    cell.Stroke = Brushes.Black;
                    cell.Width = 32;
                    cell.Height = 32;
                    stack.Children.Add(cell);

                    Canvas.SetLeft(cell, 32 * j - j);
                    Canvas.SetTop(cell, 32 * (i - 2) - i);
                }
            }
            GridField.Children.Add(stack);
        }

        private Brush paint(int c)
        {
            Brush brush = Brushes.Transparent;
            switch (c)
            {
                case 0:
                    brush = Brushes.Transparent;
                    break;
                case 1:
                    brush = Brushes.Red;
                    break;
                case 2:
                    brush = Brushes.Orange;
                    break;
                case 3:
                    brush = Brushes.Yellow;
                    break;
                case 4:
                    brush = Brushes.Green;
                    break;
                case 5:
                    brush = Brushes.Cyan;
                    break;
                case 6:
                    brush = Brushes.Blue;
                    break;
                case 7:
                    brush = Brushes.Violet;
                    break;
                default:
                    break;
            }
            return brush;
        }

        private void nextpiece()
        { 
            Random n = new Random();
            tetramino = new Tetramino(n.Next(1, 8));
        }

        private void drawpiece()
        {
            Array.Clear(Piece, 0, Piece.Length);
            for (int i = 0; i < 4; i++)
            {
                Piece[(int)(tetramino.position.X + tetramino.shape[i].X), (int)(tetramino.position.Y + tetramino.shape[i].Y)] = tetramino.colorcode;
            }
        }

        private void btnTestField_Click(object sender, RoutedEventArgs e)
        {
            Array.Clear(Field, 0, Field.Length);
            Random n = new Random();
            for (int i = 0; i < 50; i++)
            {
                Field[n.Next(0, cols), n.Next(0, rows)] = n.Next(0, 8);
            }
            drawfield();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            nextpiece();
            drawfield();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            bool left = true;
            bool right = true;
            bool down = true;
            for (int i = 0; i < 4; i++)
            {
                if (tetramino.position.X + tetramino.shape[i].X -1 < 0)
                    left = false;
                if (tetramino.position.X + tetramino.shape[i].X +1 >= cols)
                    right = false;
                if (tetramino.position.Y + tetramino.shape[i].Y +1 >= rows)
                    down = false;
            }
            switch (e.Key)
            {
                case Key.Left:
                    if (left)
                        tetramino.position.X -= 1;
                    break;
                case Key.Right:
                    if (right)
                        tetramino.position.X += 1;
                    break;
                case Key.Down:
                    if (down)
                        tetramino.position.Y += 1;
                    break;
                default:
                    break;
            }
            drawfield();
        }
    }
}
