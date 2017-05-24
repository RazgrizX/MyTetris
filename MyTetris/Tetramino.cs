using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyTetris
{
    public class Tetramino
    {
        public Point position;
        public Point[] shape;
        public int colorcode;
        public bool rotate;
        public Tetramino(int s)
        {
            shape = new Point[4];
            position = new Point(4,1);
            switch (s)
            {
                case 1: //I
                    shape[0] = new Point(-1, 0);
                    shape[1] = new Point(0, 0);
                    shape[2] = new Point(1, 0);
                    shape[3] = new Point(2, 0);
                    colorcode = 5;
                    rotate = true;
                    break;
                case 2: //J
                    shape[0] = new Point(0, -1);
                    shape[1] = new Point(0, 0);
                    shape[2] = new Point(1, 0);
                    shape[3] = new Point(2, 0);
                    colorcode = 6;
                    rotate = true;
                    break;
                case 3: //L
                    shape[0] = new Point(0, 0);
                    shape[1] = new Point(1, 0);
                    shape[2] = new Point(2, 0);
                    shape[3] = new Point(2, -1);
                    colorcode = 2;
                    rotate = true;
                    break;
                case 4: //O
                    shape[0] = new Point(0, -1);
                    shape[1] = new Point(1, -1);
                    shape[2] = new Point(0, 0);
                    shape[3] = new Point(1, 0);
                    colorcode = 3;
                    rotate = false;
                    break;
                case 5: //S
                    shape[0] = new Point(0, 0);
                    shape[1] = new Point(1, 0);
                    shape[2] = new Point(1, -1);
                    shape[3] = new Point(2, -1);
                    colorcode = 4;
                    rotate = true;
                    break;
                case 6: //Z
                    shape[0] = new Point(0, -1);
                    shape[1] = new Point(1, -1);
                    shape[2] = new Point(1, 0);
                    shape[3] = new Point(2, 0);
                    colorcode = 1;
                    rotate = true;
                    break;
                case 7: //T
                    shape[0] = new Point(0, 0);
                    shape[1] = new Point(1, 0);
                    shape[2] = new Point(1, 1);
                    shape[3] = new Point(2, 0);
                    colorcode = 7;
                    rotate = true;
                    break;
                default:
                    break;
            }
        }
    }
}
