using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AIG.Helper;
using System.Media;
using AIG.Controls;


namespace AIG
{
    
    public partial class Form1 : Form
    {
        int direction = 1;
        IList<Alien1> listAlien = new List<Alien1>();
        IList<Tent> listTent = new List<Tent>();
        Ship ship;
        IList<Bullet> listBullet = new List<Bullet>();
        SoundPlayer Fire = new SoundPlayer(Properties.Resources.Fire);
        SoundPlayer Destroy = new SoundPlayer(Properties.Resources.Destroy);
        SoundPlayer Blocked = new SoundPlayer(Properties.Resources.Blocked);
        SoundPlayer Eat = new SoundPlayer(Properties.Resources.Eat);
        SoundPlayer Mali = new SoundPlayer(Properties.Resources.Mali);
        SoundPlayer Victory = new SoundPlayer(Properties.Resources.Victory);      

        private bool isBlinkingShipStarted = false;//提供状态监测，防止被多次调用
        public Form1()
        {
            InitializeComponent();
            CreateAlien(1);
            CreateAlien(2);
            CreateAlien(3);
            CreateShip();
            CreateTent(1);
            labelKilledAliens.Text = $"0/{aliensToBeatForWin}";
        }

        // 开始闪烁飞船
        private void StartBlinkingShip()
        {            
            ucShip s = (ucShip)ship.alien;//将 ship.alien 对象转换为 ucShip 类型，并将其赋值给变量 s
            s.BlinkTimer.Start();
            isBlinkingShipStarted = true;
        }


        /// <summary>
        /// 创建外星人
        /// </summary>
        private void CreateAlien(int index)
        {
            for (int i = 0; i < 5; i++)//循环5次，i确定X位置
            {
                Point point = new Point(i * 100, (index - 1) * 55);//坐标X,Y，间隔100,55
                Alien1 currentAlien = new Alien1(this, point, index);//赋值
                listAlien.Add(currentAlien);//添加到列表中
            }
        }


        /// <summary>
        /// 创建飞船
        /// </summary>
        private void CreateShip()
        {
            Point point = new Point(1000, 800); // 确保初始位置正确
            ship = new Ship(this, point);
            isBlinkingShipStarted = false; // 重置闪烁状态
            if (ship.alien is ucShip ucShip)
            {
                ucShip.BlinkTimer.Stop(); // 停止闪烁定时器
            }
        }


        /// <summary>
        /// 创建子弹
        /// </summary>
        private void CreateBullet()
        {
            Point point = new Point(ship.Location.X + ship.size.Width / 8, ship.Location.Y);
            Bullet bullet = new Bullet(this, point);
            listBullet.Add(bullet);
            
        }


        /// <summary>
        /// 创建南瓜
        /// </summary>
        private void CreateTent(int index)
        {
            for (int i = 0; i <= 4; i++)
            {
                //设置遮挡物的座标
                Point point = new Point(70 + 300 * i, 500);
                //将遮挡物实例化后放置到屏幕上
                this.listTent.Add(new Tent(this, point, index));
            }
        }


        /// <summary>
        /// 位置是否被占用
        /// </summary>
        /// <param name="newLocation"></param>
        /// <returns></returns>
        private bool IsPositionOccupied(Point newLocation)
        {
            foreach (Alien1 alien in listAlien)//遍历循环
            {
                if (alien.IsAlive)//当外星人活着就检测是否重叠
                {
                    Rectangle alienRect = new Rectangle(alien.Location, alien.size);//旧
                    Rectangle newAlienRect = new Rectangle(newLocation, alien.size);//新
                    if (alienRect.IntersectsWith(newAlienRect))//判断
                    {
                        return true; // 位置被占用
                    }
                }
            }
            return false; // 位置未被占用
        }
        /// <summary>
        /// 刷新外星人
        /// </summary>
        private void SpawnRandomAlien()//在屏幕右边刷新
        {
            Random rand = new Random();
            int numberOfAliensToSpawn = rand.Next(2, 6);
            const int maxAttempts = 5; // 设置最大尝试次数
            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                int x = rand.Next(this.Width * 21 / 32, this.Width * 29 / 32); // 假设Alien的宽度为100
                int y = rand.Next(0, this.Height / 5); // 假设Alien的高度为55
                Point newLocation = new Point(x, y);

                if (!IsPositionOccupied(newLocation))
                {
                    Alien1 newAlien = new Alien1(this, newLocation, (rand.Next(1, 4))); // 随机选择1, 2, 3级Alien
                    listAlien.Add(newAlien);
                    return; // 成功生成外星人后退出方法
                }
            }

        }
        private void SpawnRandomAlien1()//在屏幕左边刷新
        {
            Random rand = new Random();
            int numberOfAliensToSpawn = rand.Next(2, 6);
            const int maxAttempts = 5; // 设置最大尝试次数
            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                int x = rand.Next(this.Width * 3 / 32, this.Width * 9 / 32); // 假设Alien的宽度为100
                int y = rand.Next(0, this.Height / 5); // 假设Alien的高度为55
                Point newLocation = new Point(x, y);

                if (!IsPositionOccupied(newLocation))
                {
                    Alien1 newAlien = new Alien1(this, newLocation, (rand.Next(1, 4))); // 随机选择1, 2, 3级Alien
                    listAlien.Add(newAlien);
                    return; // 成功生成外星人后退出方法
                }
            }

        }

        /// <summary>
        /// 计数通关
        /// </summary>
        private int killedAliensCount = 0;
        private int aliensToBeatForWin = 5; // 需要击败的外星人数量以赢得游戏
        public void IncrementKilledAliensCount()
        {
            killedAliensCount++;//后置自增，X=X+1
            labelKilledAliens.Text = $"{killedAliensCount}/{aliensToBeatForWin}";
            this.Invalidate(); // 强制界面刷新
            if (listAlien != null)
            {
                if (killedAliensCount >= aliensToBeatForWin)
                {
                    Victory.Play();
                    GameWin(); 
                }
            }
        }


        /// <summary>
        /// 子弹碰外星人
        /// </summary>
        private void CheckFireAline()
        {
            
            foreach (Bullet bullet in listBullet)
            {
                if (!bullet.IsAlive)
                    continue;
                if (listAlien != null)
                {
                    foreach (Alien1 alien in listAlien)
                    {
                        if (!alien.IsAlive)
                            continue;
                        if (alien.CheckCollision(bullet))//子弹打中外星人
                        {
                            bullet.IsAlive = false;
                            alien.Hited();
                            Destroy.Play();
                            break;
                        }
                    }
                }
            }
      
        }

        /// <summary>
        /// 子弹碰南瓜
        /// </summary>
        private void CheckFireTent()
        {
            if (listAlien != null)
            {
                foreach (Bullet bullet in listBullet)
                {
                    if (!bullet.IsAlive)
                        continue;
                    foreach (Tent tent in listTent)
                    {
                        if (!tent.IsAlive)
                            continue;
                        if (tent.CheckCollision(bullet))
                        {
                            bullet.IsAlive = false;
                            tent.Hited();
                            Blocked.Play();
                            break;
                        }
                    }

                }
            }
        }

        /// <summary>
        /// 外星人碰南瓜
        /// </summary>
        private void AlienTent()
        {
            if (listAlien != null)
            {
                foreach (Alien alien in listAlien)
                {
                    if (!alien.IsAlive)
                        continue;
                    foreach (Tent tent in listTent)
                    {
                        if (!tent.IsAlive)
                            continue;
                        if (tent.CheckCollision(alien))//相遇
                        {
                            alien.IsAlive = false;
                            tent.IsAlive = false;
                            Eat.Play();
                            break;
                        }
                    }

                }
            }
        }

        /// <summary>
        /// 飞船消失
        /// </summary>
        private async void AlienInvasionAsync()
        {
            if (listAlien != null)
            {
                foreach (Alien alien in listAlien)//遍历所有外星人
                {
                    if (!alien.IsAlive)
                        continue;
                    if (alien.CheckCollision(ship))//如果当前外星人与战斗机发生碰撞
                    {
                        StartBlinkingShip();
                        Mali.Play();
                        await Task.Delay(1500);//延时触发
                        GameOver();//游戏结束
                        return;
                    }
                }
                if (listTent != null)
                {
                    foreach (Tent tent in listTent)//遍历所有南瓜
                    {
                        if (!tent.IsAlive)
                            continue;
                        if (tent.CheckCollision(ship))//如果当前外星人与战斗机发生碰撞
                        {

                            StartBlinkingShip();
                            Mali.Play();
                            await Task.Delay(1500);//延时
                            GameOver();//游戏结束
                            return;
                        }
                    }
                }
            }
        }


        private bool gameOver = false;
        /// <summary>
        /// 游戏结束，释放资源
        /// </summary>
        private void GameOver()
        {
            if (listAlien != null)
            {
                if (gameOver)
                {
                    return;
                }
                gameOver = true;
                timer1.Enabled = false; // 停止定时器
                this.ship = null; // 清空变量，释放内存
                this.Controls.Clear(); // 清除屏幕上所有元素
                DialogResult result = MessageBox.Show("游戏结束！是否再来一局？", "失败", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    // 重置游戏状态
                    InitializeGame();
                }
                else
                {
                    Application.Exit(); // 退出程序
                }
            }
        }

        /// <summary>
        /// 重开
        /// </summary>
        private void InitializeGame()
        {
            // 重置游戏结束标志
            gameOver = false;
            gamewin = false;

            // 重置计数器
            killedAliensCount = 0;
            labelKilledAliens.Text = "0";

            // 清空现有的游戏对象列表
            listAlien.Clear();
            listTent.Clear();
            listBullet.Clear();

            // 清除界面上的所有控件
            this.Controls.Clear();

            // 初始化网格
            //InitializeGrid();

            // 重新创建外星人、飞船和南瓜
            CreateAlien(1);
            CreateAlien(2);
            CreateAlien(3);
            CreateShip();
            CreateTent(1);

            // 重新添加控件到界面上
            if (ship != null && ship.alien != null)
            {
                this.Controls.Add(ship.alien as Control);
            }
            foreach (var alien in listAlien)
            {
                if (alien != null && alien.alien != null)
                {
                    this.Controls.Add(alien.alien as Control);
                }
            }
            foreach (var tent in listTent)
            {
                if (tent != null && tent.alien != null)
                {
                    this.Controls.Add(tent.alien as Control);
                }
            }

            // 重新启动定时器
            timer1.Enabled = true;

            // 重新加载界面
            this.Invalidate();
            this.Controls.Add(labelKilledAliens);
            labelKilledAliens.Text = $"0/{aliensToBeatForWin}";
            this.Controls.Add(label1);

            // 确保事件处理程序仍然绑定
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);

        }
        // 键盘事件处理程序
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (ship != null && !isBlinkingShipStarted)
            {
                switch (e.KeyCode)
                {
                    case Keys.C:
                        CreateBullet();
                        Fire.Play();
                        break;
                    case Keys.Up:
                    case Keys.W:
                        if (ship.Location.Y > 0)
                            ship.GoUp();
                        break;
                    case Keys.Down:
                    case Keys.S:
                        if (ship.Location.Y < this.Height - ship.size.Height - 40)
                            ship.GoDown();
                        break;
                    case Keys.Left:
                    case Keys.A:
                        if (ship.Location.X > 0)
                            ship.GoLeft();
                        break;
                    case Keys.Right:
                    case Keys.D:
                        if (ship.Location.X < this.Width - ship.size.Width - 5)
                            ship.GoRight();
                        break;
                }
            }
        }

        private bool gamewin= false;
        /// <summary>
        /// 游戏胜利，再来一局
        /// </summary>
        private void GameWin()
        {
            if (gamewin) // 检查游戏是否已经胜利
            {
                return;
            }
            gamewin = true;
            timer1.Enabled = false;
            //RefreshTimer.Enabled = false;
            this.listAlien = null;//清空变量，释放内存
            this.listBullet = null;//清空变量，释放内存
            this.listTent = null;//清空变量，释放内存
            this.ship = null;//清空变量，释放内存
            this.Controls.Clear();//清除屏幕上所有元素
            //MessageBox.Show("有手就行");
            DialogResult result = MessageBox.Show("这很简单！进入下一关", "成功", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
            {
                Application.Exit(); // 退出程序
            }
            return;
        }

        /// <summary>
        /// 调用刷新功能
        /// </summary>
        private bool alienReachedLeftBorder = false;
        private bool alienReachedLeftBorder1 = false;
        /// <summary>
        /// 改变运动反向
        /// </summary>
        private void CheckAlienReachBorder()
        {
            bool changed = false;//检测外星人什么时候改变方向
            foreach (Alien1 a in listAlien)
            {
                if (changed) break;//为true,跳出循环
                if (direction == 1)
                {
                    if (a.Location.X + a.size.Width < this.Width)
                        continue;
                    else
                    {
                        direction = 2;
                        changed = true;//有外星人到达边界，转向跳出循环
                        alienReachedLeftBorder = true; // 调用右边刷新外星人
                    }
                }
                else
                {
                    if (a.Location.X > 0)
                        continue;
                    else
                    {
                        direction = 1;
                        changed = true;
                        alienReachedLeftBorder1 = true; // 设置标志位为true
                    }
                }
            }
            if (!changed)
            {
                foreach (Alien1 a in listAlien)
                {
                    if (!a.IsAlive) continue;
                    if (direction == 1)
                        a.GoRight();
                    else
                        a.GoLeft();

                }
            }
            else
            {

                foreach (Alien1 a in listAlien)
                {
                    if (!a.IsAlive) continue;
                    a.GoDown();
                    if (direction == 1)
                        a.GoRight();
                    else
                        a.GoLeft();

                }
            }
            // 如果外星人到达边界，刷新外星人
            if (alienReachedLeftBorder)
            {
                for (int i = 0; i < 5; i++) // 假设每次刷新5个Alien
                {
                    SpawnRandomAlien();
                }
                alienReachedLeftBorder = false; // 重置标志
            }
            else if(alienReachedLeftBorder1)
            {
                for (int i = 0; i < 5; i++) // 假设每次刷新5个Alien
                {
                    SpawnRandomAlien1();
                }
                //alienReachedLeftBorder1 = false; // 重置标志
                alienReachedLeftBorder1 = false; // 重置标志
            }
        }

        /// <summary>
        /// 控制飞船方向
        /// </summary>
        /// <param name="KeyData"></param>
        /// <returns></returns>
        protected override bool ProcessDialogKey(Keys KeyData)
        {
            if (ship != null && !isBlinkingShipStarted)
            {
                if (KeyData == Keys.C)
                {
                    CreateBullet();
                    Fire.Play();
                }


                if (KeyData == Keys.Up || KeyData == Keys.W)
                {
                    if (ship.Location.Y > 0)
                        ship.GoUp();
                }
                if (KeyData == Keys.Down || KeyData == Keys.S)
                {
                    if (ship.Location.Y < this.Height - ship.size.Height - 40)
                        ship.GoDown();
                }
                if (KeyData == Keys.Left || KeyData == Keys.A)
                {
                    if (ship.Location.X > 0)
                        ship.GoLeft();
                }
                if (KeyData == Keys.Right || KeyData == Keys.D)
                {
                    if (ship.Location.X < this.Width - ship.size.Width - 5)
                        ship.GoRight();
                }
            }

            return base.ProcessDialogKey(KeyData);
        }


        private void timer1_Tick_1(object sender, EventArgs e)
        {
            if (gameOver || gamewin)
            {
                return; // 如果游戏已经结束或胜利，跳过操作
            }
            if (listAlien != null)
            {
                CheckAlienReachBorder();
                foreach (Bullet b in listBullet)
                    b.GoUp();

                CheckFireAline();
                CheckFireTent();
                AlienTent();
                AlienInvasionAsync();

            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            // 绑定定时器事件
            timer1.Tick += new EventHandler(timer1_Tick_1);
        }
    }
}
