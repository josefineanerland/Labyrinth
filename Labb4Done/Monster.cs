using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb44
{
    public class Monster : Basetile, ITile
    {
        private int x;
        private int y;
        public Monster(int X, int Y)
        {
            x = X;
            y = Y;
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }
        public string Tile { get { return "m"; } }

        public Enum TileType { get { return BoardTile.Monster; } }

    }
}
