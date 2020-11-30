using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using elements;
using home1.Properties;

namespace home1
{
    class World :IMoving
    {
        private static BufferedGraphicsContext context;
        public static BufferedGraphics buffer;

        public static int Width { set; get; }
        public static int Height { set; get; }
        public static List<Keys> KeysPressed = new List<Keys>();
        static asteroid[] asteroids;
        static Star[] stars;
        static Bullet bullet;
        static List<crack> cracks=new List<crack>();
        private static Timer timer1;
        static World() { }
        
        public static void Init(Form space)
        {
            Graphics g;
            context = BufferedGraphicsManager.Current;
            g = space.CreateGraphics();
            Width = space.Width;
            Height = space.Height;
            buffer = context.Allocate(g, new Rectangle(0, 0, Width, Height));

            LoadElements();
            timer1 = new Timer() { Interval = 100 };
            timer1.Tick += timer1_Tick;
            timer1.Start();
            //form.
        }
        public static void Shot()
        {
            bullet = new Bullet(new Point(0, Height-70), new Size(45, 45), 35);
            //bullet.draw();
        }

        public static void SpeedCor(List<Keys> KeysPressed)
        {
            foreach (asteroid se in asteroids)
                se.SpeedCor(KeysPressed);
            if (bullet != null) bullet.SpeedCor(KeysPressed);
        }

        private static void LoadElements()
        {
            asteroids = new asteroid[10];
            stars = new Star[60];

            Random k = new Random();
            for (int i=0; i<asteroids.Length; i++)
            {
                int[] kk=new int[4];
                kk[0] = k.Next(Width); kk[1] = k.Next(Height);//position
                //kk[0] = Width / 2; kk[1] = Height / 2;
                kk[2] = k.Next(5, 20); //size
                kk[3] = k.Next(5,10); //speed 
                
                asteroids[i] = new asteroid(new Point(kk[0], kk[1]), new Size(kk[2], kk[2]), (byte)kk[3]);
            }
            for (int i=0; i<stars.Length; i++)
            {
                stars[i] = new Star(new Point(k.Next(Width), k.Next(Height)));
            }
            //Bullet _bullet=new Bullet()
        }

        public static void Draw()
        {
            buffer.Graphics.Clear(Color.Black);
            Point p;
            foreach (Star se in stars)    
                buffer.Graphics.DrawImage(se.draw(out p), p);
                      
            buffer.Graphics.DrawImage(new Bitmap(Resources.planet, new Size(200, 200)), 100, 100);
            
            foreach (asteroid se in asteroids)
            {               
                //bool hit;
                if (se != null)
                {
                    buffer.Graphics.DrawImage(se.draw(out p), p);
                    if (se.Hit)
                    {
                        crack cr = new crack(p);
                        cracks.Add(cr);
                    }
                }    
            }

            foreach (crack cr in cracks)            
                buffer.Graphics.DrawImage(cr.draw(out p), p);      

            if (bullet != null) 
                buffer.Graphics.DrawImage(bullet.draw(out p), p);

            buffer.Render();  
        }
        static void Update()
        {
            bool target=false;
            if (bullet != null)
                if (bullet.Move()) target = true; else target = false;
            for (int i=0; i<asteroids.Length; i++)
            {
                if (asteroids[i] != null)
                {
                    asteroids[i].Move(new Size(Width, Height));
                    /*for (int j=1; j<asteroids.Length;j++)
                    {
                        if ((i != j) && (asteroids[i].IsCollision(asteroids[j])))
                        { asteroids[i].Collision(asteroids[j]);
                            buffer.Graphics.DrawRectangle(new Pen(Color.White), asteroids[i].rect);
                            //timer1.Stop();
                            buffer.Render();
                        }
                            

                    }*/
                        
                    if (target)
                    {
                        if (asteroids[i].IsCollision(bullet)) 
                            Array.Clear(asteroids, i, 1); //Hit!! to delete from array   
                    } 
                }        
            } 
            if (target) bullet = null;
                
        }
        private static void timer1_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }

        public void SpeedCor(List<char> KeysPressed)
        {
            throw new NotImplementedException();
        }
    }  
}
