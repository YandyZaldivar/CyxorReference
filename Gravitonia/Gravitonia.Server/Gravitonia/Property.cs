using System;
using System.Collections.Generic;
using System.Text;

namespace Gravitonia
{
    enum PropertyType
    {
        Planet,
        Station,
    }

    class Property
    {
        public char Symbol { get; }
        public string Name { get; }
        public PropertyType Type { get; }

        public Player Owner { get; internal set; }

        public int Mines { get; internal set; }
        public int ParallelMines { get; internal set; }
    }
}
