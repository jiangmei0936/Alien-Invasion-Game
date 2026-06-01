using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using AIG.Controls;

namespace AIG.Helper
{
    public class Bullet:Alien
    {
        private int speed = 30;

        public override UserControl CreateAlien(int index)
        {
            return new ucBullet();
        }
        public Bullet(Form1 form, Point point) : base(form, point, 0)
        {

            Location = point;
            size = new Size(10, 20);
        }

        public void GoUp()
        {
            Location = new Point(Location.X, Location.Y - speed);
            SetObjectLocation();
        }
    }
}
