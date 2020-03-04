using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb44
{
    class Player : Basetile, ITile
    {
        private int x = 2;
        private int y = 2;
        private int steps = 0;
        private int keys = 0;
        private string name;
        private string tile = "@";
        private Enum tiletype = BoardTile.Player;
        public string Tile { get { return tile; } }
        public  Enum TileType { get { return tiletype; } }

        public int Steps
        {
            get { return steps; }
            set { steps = value; }
        }
        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public void AddPlayerKey()
        {
            keys++;
        }

        public void RemovePlayerKey()
        {
            keys--;
        }

        public int GetPlayerKeys()
        {
            return keys;
        }

        public void SetPlayerName(string Name)
        {
            name = Name;
        }
        public string GetPlayerName()
        {
            return name;
        }



    }
}
