using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AIG.Controls
{
    public partial class ucShip : UserControl
    {
        public ucShip()
        {
            InitializeComponent();
            InitializeBlinkTimer();
        }


        public void InitializeBlinkTimer()
        {
            BlinkTimer = new Timer();
            BlinkTimer.Interval = 120; // 设置时间间隔
            BlinkTimer.Tick += BlinkTimer_Tick;
        }



        public void StartBlinking()
        {
            BlinkTimer.Start();
        }

        public void StopBlinking()
        {
            BlinkTimer.Stop();
        }


        public PictureBox PublicPictureBox => pictureBox1;

        public void ChangeBlinkInterval(int interval)
        {
            BlinkTimer.Interval = interval;
        }

        private void BlinkTimer_Tick(object sender, EventArgs e)
        {
            pictureBox1.Visible = !pictureBox1.Visible;
        }
    }
}
