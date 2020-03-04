using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb44
{
    public class Key : Basetile, ITile
    {
        private int x;
        private int y;
        private bool isFound = false;

        public Key(int X, int Y)
        {
            x = X;
            y = Y;
        }

        public bool IsFound
        {
            get { return isFound; }
            set { isFound = value; }
        }
        public int X
        {
            get { return x; }
        }
        public int Y
        {
            get { return y; }
        }

        public string Tile { get { return "k"; } }

        public Enum TileType { get { return BoardTile.Key; } }

    }
}
