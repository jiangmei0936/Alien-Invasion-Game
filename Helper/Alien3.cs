using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using AIG.Controls;

namespace AIG.Helper
{
    
    public class Alien3 : Alien
    {
        private int blood;//血量
        private int speed = 15;
        
        public override UserControl CreateAlien(int index)
        {
            return new ucAlien3();
        }
        public Alien3(Form1 form, Point point, int index) : base(form, point, index)
        {
            blood = 4 - index;
            Location = point;
            size = new Size(100, 50);
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
        public void Hited()
        {
            if (blood > 0)
                blood--;
            else
                IsAlive = false;
        }

        public string GetCurtBlood()
        {
            return blood.ToString();
            
        }
    }
}
