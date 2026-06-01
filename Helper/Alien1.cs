using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using AIG.Controls;
using System.Media;

namespace AIG.Helper
{
    public class Alien1:Alien
    {
        private int blood;//血量
        public int speed = 15;
        private Form1 form;


        public override UserControl CreateAlien(int index)
        {
            if (index == 1)
                return new ucAlien1();
            else if (index == 2)
                return new ucAlien2();
            else
                return new ucAlien3();
        }

        public Alien1(Form1 form, Point point, int index) : base(form, point, index)
        {
            this.form = form;
            blood = 4 - index;
            Location = point;
            size = new Size(100, 50);

        }


        public void GoDown()
        {
            Location = new Point(Location.X, Location.Y + speed*2);
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
            {
                IsAlive = false;
                if (form != null)
                {
                    form.IncrementKilledAliensCount(); // 调用 Form1 中的计数器方法
                }
            }
        }

        public string GetCurtBlood()
        {
            return blood.ToString();
        }
    }
}
