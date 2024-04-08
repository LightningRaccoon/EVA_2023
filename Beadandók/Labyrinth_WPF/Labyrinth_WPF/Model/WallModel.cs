using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labyrinth_WPF.Model
{
    public class Wall
    {
        public int X { get; }
        public int Y { get; }

        public Wall(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Wall()
        {
            X = 0;
            Y = 0;
        }
    }
}
