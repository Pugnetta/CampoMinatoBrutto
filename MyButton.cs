using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampoMinatoWinForms
{
    internal class MyButton : Button
    {
        static public int BtnSize { get; private set; } = 25;
        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsSelected { get; set; }
        public bool IsBomb { get; set; }
        public bool IsVisited { get; set; }
        public bool IsFlagged { get; set; }

        public MyButton()
        {
            Height = Width = BtnSize;
            BackColor = Color.GhostWhite;

        }
    }
}
