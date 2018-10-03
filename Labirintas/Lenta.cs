using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labirintas
{
    class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position()
        {
            X = 0;
            Y = 0;
        }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    class Lenta
    {
        public int SizeX { get; set; }
        public int SizeY { get; set; }
        public Position CurPos { get; set; }

        public int MoveCount { get; set; }

        public int [,] Map { get; set; }

        public Lenta(int sizeX, int sizeY, Position startPos)
        {
            SizeX = sizeX;
            SizeY = sizeY;
            CurPos = startPos;

            Map = new int[SizeX, SizeY];

            MoveCount = 2;
        }
    }
}
