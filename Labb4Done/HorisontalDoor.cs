using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb44
{
    public class HorisontalDoor : Basetile, ITile
    {
        public string Tile { get { return "═"; } }

        public Enum TileType { get { return BoardTile.Wall; } }
    }
}
