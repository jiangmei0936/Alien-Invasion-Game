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
    public class Tent : Alien
    {
        private int blood;
        public int hitedCount = 0;
        //构造时同时构造基类

        public override UserControl CreateAlien(int index)
        {
            return new ucTent();
        }
        public Tent(Form1 form, Point point, int index) : base(form, point, index)
        {
            blood = 4 - index;
            Location = point;
            size = new Size(100, 50);
        }


        /// <summary>
        /// 障碍物被击中 3 次后消失
        /// </summary>
        public void Hited()
        {
            hitedCount++;
            if (hitedCount >= 3)
            {
                this.IsAlive = false;
                this.alien.Visible = false;
            }
        }

        public string GetCurtBlood()
        {
            return blood.ToString();
        }
    }
}
