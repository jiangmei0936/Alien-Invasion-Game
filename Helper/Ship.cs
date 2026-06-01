using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;
using AIG.Controls;

namespace AIG.Helper
{
    public class Ship:Alien
    {
        
        private int speed = 10;

        public override UserControl CreateAlien(int index)
        {
            return new ucShip();
        }

        
        public Ship(Form1 form, Point point) : base(form, point,0)
        {
          
            Location = point;
            size = new Size(120, 80);
        }

        public void GoUp()
        {
            Location = new Point(Location.X, Location.Y - speed);
            SetObjectLocation();
        }

        public void GoDown()
        {
            Location = new Point(Location.X, Location.Y + speed);
            SetObjectLocation();
        }

        public void GoLeft()
        {
            Location = new Point(Location.X - speed, Location.Y);
            SetObjectLocation();
        }

        public void GoRight()
        {
            Location = new Point(Location.X + speed, Location.Y);
            SetObjectLocation();
        }
    }
}
