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
using System.Windows.Threading;

namespace MyTetris
{
    public partial class MainWindow : Window
    {
        private static int rows = 22;
        private static int cols = 10;
        private int[,] Field = new int[cols, rows];
        private int[,] Piece = new int[cols, rows];
        private static readonly Random getrandom = new Random();
        private Tetramino tetramino;
        private Tetramino next;
        private int linescleared;
        private int lvl;
        private int score;
        DispatcherTimer autodown = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            autodown.Tick += new EventHandler(tickdown);
            update();
            drawpreview();
        }

        private void btnTestField_Click(object sender, RoutedEventArgs e)
        {
            Array.Clear(Field, 0, Field.Length);
            for (int i = 0; i < 50; i++)
            {
                Field[getrandom.Next(0, cols), getrandom.Next(0, rows)] = getrandom.Next(0, 8);
            }
            update();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            newgame();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (tetramino != null)
            {
                switch (e.Key)
                {
                    case Key.Left:
                        moveleft();
                        break;
                    case Key.Right:
                        moveright();
                        break;
                    case Key.Down:
                        movedown();
                        break;
                    case Key.Up:
                        if (tetramino.rotate)
                            rotate();
                        break;
                    default:
                        break;
                }
            }
        }

        private void btnObstacle_Click(object sender, RoutedEventArgs e)
        {
            Field[3, 10] = 1;
            Field[4, 10] = 1;
            Field[3, 9] = 1;
            Field[4, 9] = 1;
            Field[5, 9] = 1;
            update();
        }

        private void update()
        {
            if (tetramino != null)
                Piece = drawpiece(tetramino, Piece);
            drawfield();
            lblscrnum.Content = score.ToString();
            lbllvlnum.Content = lvl.ToString();
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

            Canvas stack = new Canvas();
            stack.Name = "Cstack";
            stack.Width = 310;
            stack.Height = 620;
            for (int i = 2; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Rectangle cell = new Rectangle();
                    if (tetramino != null && Piece[j, i] != 0)
                        cell.Fill = paint(Piece[j, i]);
                    else
                        cell.Fill = paint(Field[j, i]);
                    cell.StrokeThickness = 1;
                    cell.Stroke = Brushes.Gray;
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
                    brush = Brushes.Crimson;
                    break;
                case 2:
                    brush = Brushes.OrangeRed;
                    break;
                case 3:
                    brush = Brushes.Gold;
                    break;
                case 4:
                    brush = Brushes.Green;
                    break;
                case 5:
                    brush = Brushes.LightSkyBlue;
                    break;
                case 6:
                    brush = Brushes.Blue;
                    break;
                case 7:
                    brush = Brushes.DarkViolet;
                    break;
                default:
                    break;
            }
            return brush;
        }

        private void nextpiece()
        {
            if (over())
            {
                autodown.Stop();
                MessageBox.Show("Your Score: "+score, "Game over");
                tetramino = null;
                
            }
            else
            {
                tetramino = next;
                tetramino.position = new Point(5, 1);
                next = new Tetramino(getrandom.Next(1, 8));
                drawpreview();
                movedown();
            }
        }

        private int[,] drawpiece(Tetramino t, int[,] item)
        {
            Array.Clear(item, 0, item.Length);
            for (int i = 0; i < 4; i++)
            {
                item[(int)(t.position.X + t.shape[i].X), (int)(t.position.Y + t.shape[i].Y)] = t.colorcode;
            }
            return item;
        }

        private void moveright()
        {
            bool right = true;
            for (int i = 0; i < 4; i++)
            {
                if (tetramino.position.X + tetramino.shape[i].X + 1 >= cols || Field[(int)(tetramino.position.X + tetramino.shape[i].X + 1), (int)(tetramino.position.Y + tetramino.shape[i].Y)] != 0)
                    right = false;
            }
            if (right)
            {
                tetramino.position.X += 1;
                update();
            }
        }

        private void moveleft()
        {
            bool left = true;
            for (int i = 0; i < 4; i++)
            {
                if (tetramino.position.X + tetramino.shape[i].X - 1 < 0 || Field[(int)(tetramino.position.X + tetramino.shape[i].X - 1), (int)(tetramino.position.Y + tetramino.shape[i].Y)] != 0)
                    left = false;
            }
            if (left)
            {
                tetramino.position.X -= 1;
                update();
            }
        }

        private void movedown()
        {
            bool down = true;
            for (int i = 0; i < 4; i++)
            {
                if (tetramino.position.Y + tetramino.shape[i].Y + 1 >= rows || Field[(int)(tetramino.position.X + tetramino.shape[i].X), (int)(tetramino.position.Y + tetramino.shape[i].Y + 1)] != 0)
                {
                    down = false;
                }
            }
            if (down)
                tetramino.position.Y += 1;
            else
            {
                anchor();
                nextpiece();
            }
            autodown.Interval = new TimeSpan(0, 0, 1);
            update();
        }

        private void anchor()
        {
            for (int i = 0; i < 4; i++)
            {
                Field[(int)(tetramino.position.X + tetramino.shape[i].X), (int)(tetramino.position.Y + tetramino.shape[i].Y)] = tetramino.colorcode;
            }
            checklines();
        }

        private bool over()
        {
            bool over = false;
            for (int i = 0; i < cols; i++)
            {
                if (Field[i, 1] != 0)
                {
                    over = true;
                }
            }
            return over;
        }

        private void checklines()
        {
            int[] full = new int[4];
            for (int i = 0; i < 4; i++)
            {
                int line = (int)(tetramino.position.Y + tetramino.shape[i].Y);
                bool all = true;
                for (int j = 0; j < cols; j++)
                {
                    if (Field[j, line] == 0)
                    {
                        all = false;
                    }
                }
                int size = full.Count(x => x != 0);
                bool unic = true;
                for (int u = 0; u < size; u++)
                {
                    if (full[u] == line)
                        unic = false;
                }
                if (all && unic)
                    full[size] = line;
            }
            if (full.Count(x => x != 0) > 0)
                clearlines(full);
        }

        private async void clearlines(int[] full)
        {
            int count = full.Count(x => x != 0);
            
            for (int k = 0; k < 7; k++)
            {
                for (int i = 0; i < count; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        Field[j, full[i]] = getrandom.Next(1, 8);
                    }
                }
                update();
                await Task.Delay(50);
            }

            for (int l = 0; l < count; l++)
            {
                int line = full.Where(f => f > 0).Min();
                for (int i = line; i > 0; i--)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        Field[j, i] = Field[j, i - 1];
                    }
                }
                full[Array.IndexOf(full, line)] = 0;
            }
            scoring(count);
            checklvl(count);
            update();

            await Task.Delay(500);
            
        }

        private void newgame()
        {
            
            Array.Clear(Field, 0, Field.Length);
            Array.Clear(Piece, 0, Piece.Length);
            score = 0;
            lvl = 0;
            autodown.Interval = new TimeSpan(0, 0, 1);
            next = new Tetramino(getrandom.Next(1, 8));
            nextpiece();
            update();
            autodown.Start();
        }

        private void rotate()
        {
            Tetramino temp = new Tetramino(1);
            temp.position = tetramino.position;
            bool valid = true;
            bool mvright = true;
            bool mvleft = true;
            for (int i = 0; i < 4; i++)
            {
                temp.shape[i].X = tetramino.shape[i].Y * -1;
                temp.shape[i].Y = tetramino.shape[i].X;
                if ((temp.position.X + temp.shape[i].X) >= cols || (temp.position.X + temp.shape[i].X) < 0 || (temp.position.Y + temp.shape[i].Y) > rows || Field[(int)(temp.position.X + temp.shape[i].X), (int)(temp.position.Y + temp.shape[i].Y)] != 0)
                    valid = false;
                if ((temp.position.X + temp.shape[i].X + 1) >= cols || (temp.position.X + temp.shape[i].X + 1) < 0 || (temp.position.Y + temp.shape[i].Y) >= rows || Field[(int)(temp.position.X + temp.shape[i].X + 1), (int)(temp.position.Y + temp.shape[i].Y)] != 0)
                    mvright = false;
                if ((temp.position.X + temp.shape[i].X - 1) >= cols || (temp.position.X + temp.shape[i].X - 1) < 0 || (temp.position.Y + temp.shape[i].Y) >= rows || Field[(int)(temp.position.X + temp.shape[i].X - 1), (int)(temp.position.Y + temp.shape[i].Y)] != 0)
                    mvleft = false;
            }
            if (valid)
            {
                tetramino.shape = temp.shape;
                tetramino.position = temp.position;
            }
            else
            {
                if (mvright)
                {
                    temp.position.X += 1;
                    tetramino.shape = temp.shape;
                    tetramino.position = temp.position;
                }
                else
                {
                    if (mvleft)
                    {
                        temp.position.X -= 1;
                        tetramino.shape = temp.shape;
                        tetramino.position = temp.position;
                    }
                }
            }
            update();
        }

        private void drawpreview()
        {

            try
            {
                var element = GridPreview.Children
        .OfType<Canvas>()
        .FirstOrDefault(e => e.Name == "Cpreview");
                GridPreview.Children.Remove(element);
            }
            catch { }
            int[,] Preview = new int[4, 4];
            if (next != null)
                Preview = drawpiece(next, Preview);

            Canvas stack = new Canvas();
            stack.Name = "Cpreview";
            stack.Width = 128;
            stack.Height = 128;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Rectangle cell = new Rectangle();
                    cell.Fill = paint(Preview[j, i]);
                    cell.StrokeThickness = 1;
                    cell.Stroke = Brushes.Gray;
                    cell.Width = 32;
                    cell.Height = 32;
                    stack.Children.Add(cell);
                    Canvas.SetLeft(cell, 32 * j - j);
                    Canvas.SetTop(cell, 32 * i - i);
                }
            }
            GridPreview.Children.Add(stack);
        }

        private void scoring(int lines)
        {
            switch (lines)
            {
                case 1:
                    score += 40 * (lvl + 1);
                    break;
                case 2:
                    score += 100 * (lvl + 1);
                    break;
                case 3:
                    score += 300 * (lvl + 1);
                    break;
                case 4:
                    score += 1200 * (lvl + 1);
                    break;
                default:
                    break;
            }
        }
        private void checklvl(int lines)
        {
            linescleared += lines;
            lvl = (int)(Math.Truncate((decimal)linescleared / 10));
        }

        private void tickdown(object sender, EventArgs e)
        {
            movedown();
        }
    }
}
