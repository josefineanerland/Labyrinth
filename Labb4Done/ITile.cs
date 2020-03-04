using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb44
{
    public interface ITile
    {
        Enum TileType { get; }
        string Tile { get; }
    }
}
