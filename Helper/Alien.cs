using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;

namespace AIG.Helper
{
    public abstract class Alien
    {
        public UserControl alien; //类成员变量
        //标识当前对象是否“活的”，false:死了，true:活的
        private bool isAlive;
        public int direction = 1;
        public bool IsAlive
        {
            get { return isAlive; }
            set
            {
                isAlive = value;
                //如果设置当前对象死亡则将当前对象设置为“不可见”
                if (isAlive == false)
                {
                    alien.Visible = false;
                }
            }
        }
        public Point Location { get; set; }//对象的座标
        public Size size { get; set; }//对象的大小（宽度和高度）
        protected void SetObjectLocation()
        {
            alien.Location = new Point(this.Location.X, this.Location.Y);
            
        }
        public Rectangle CreateRec()
        {
            return new Rectangle(Location, size);
        }

        public abstract UserControl CreateAlien(int index);


        public Alien(Form canvasHost, Point point, int index)
        {
            IsAlive = true;
            alien = CreateAlien(index);
            Location = point;
            SetObjectLocation();
            canvasHost.Controls.Add(alien);//将 BlueAlien 添加到屏幕中
        }
        public bool CheckCollision(Alien b)
        {
            Rectangle rectA = CreateRec();
            Rectangle rectB = b.CreateRec();
            rectA.Intersect(rectB);
            return (rectA != Rectangle.Empty);
        }

    }
}
