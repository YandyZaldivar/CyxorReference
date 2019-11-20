using System;
using System.Collections.Generic;
using System.Text;

namespace Gravitonia
{
    class Game
    {
        public static int StartEnergy = 5000;
        public static int MidEnergy = 3000;
        public static int LowEnergy = 1000;

        public List<Player> Players { get; }

        public List<Square> Squares { get; }

        public Player CurrentPlayer { get; private set; }

        public Game()
        {

        }

        public void NextTurn() => CurrentPlayer = CurrentPlayer.Next;
    }
}
