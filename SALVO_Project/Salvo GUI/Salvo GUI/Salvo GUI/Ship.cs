using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Salvo_GUI
{
    class Ship
    {

        private int size = 0;
        private int numeberOfHits = 0;
        private bool isPlaced = false;
        private Color color;

        private Dictionary<string, bool> positions = new Dictionary<string, bool>();

        public Ship(int size, Color color)
        {

        }

        public Ship()
        {

        }

    }
}
