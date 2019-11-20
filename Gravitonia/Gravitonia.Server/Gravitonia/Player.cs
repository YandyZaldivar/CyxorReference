using System;
using System.Collections.Generic;
using System.Text;

namespace Gravitonia
{
    class Player
    {
        public Player Left { get; }
        public Player Right { get; }

        public Player Next => Left;
        public Player Previous => Right;

        public Square Square { get; }

        public int Turn { get; }
        public int Energy { get; } = Game.StartEnergy;

        public int Dices => Energy > Game.MidEnergy ? 2 : Energy > Game.LowEnergy ? 3 : 4;

        public Player()
        {

        }
    }
}
