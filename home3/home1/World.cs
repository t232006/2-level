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
    class World //: IMoving
    {
        private static BufferedGraphicsContext context;
        public static BufferedGraphics buffer;

        public static int Width { set; get; }
        public static int Height { set; get; }
        public static List<Keys> KeysPressed = new List<Keys>();
        static asteroid[] asteroids;
        static Aid[] aids;
        static byte asterAppProb = 30;
        static byte aidAppProb = 20;
        static Star[] stars;
        static Random k = new Random();

        static List<Bullet> queue = new List<Bullet>();
        private static Timer timer1;
        static Ship ship = new Ship();
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
            Ship.MessageDie += Finish;
            timer1 = new Timer() { Interval = 100 };
            timer1.Tick += timer1_Tick;
            timer1.Start();
        }

        private static void Finish()
        {
            timer1.Stop();
            buffer.Graphics.DrawString("The End!", new Font(FontFamily.GenericSansSerif, 60, FontStyle.Underline), Brushes.White, 200, 100);
            buffer.Render();
        }

        public static void Shot()
        {
            Bullet bullet = new Bullet(new Point(0, Height-70), new Size(45, 45), 35);
            queue.Add(bullet);
        }

        public static void SpeedCor(List<Keys> KeysPressed)
        {
            foreach (asteroid se in asteroids)
                if (se!=null) se.SpeedCor(KeysPressed);
            foreach (Bullet bullet in queue)
                bullet.SpeedCor(KeysPressed); 
        }
        /*static asteroid generator()
        {
            int[] kk = new int[4];
            kk[0] = k.Next(Width); kk[1] = k.Next(Height);//position
                //kk[0] = 380; kk[1] = 290;
            kk[2] = k.Next(20, 40); //size
            kk[3] = k.Next(5,10); //speed 
                
            return new asteroid(new Point(kk[0], kk[1]), new Size(kk[2], kk[2]), (byte) kk[3]);
        }*/
        static void loadAster(ref asteroid ast)
        {
            int[] kk = new int[4];
            kk[0] = k.Next(Width); kk[1] = k.Next(Height);//position
                                                            //kk[0] = 380; kk[1] = 290;
            kk[2] = k.Next(20, 40); //size
            kk[3] = k.Next(5, 10); //speed 

            ast = new asteroid(new Point(kk[0], kk[1]), new Size(kk[2], kk[2]), (byte)kk[3]);
        }
        
        static void loadAids(ref Aid aid)
        {   
            
            int[] kk = new int[4];
            kk[0] = k.Next(Width); kk[1] = k.Next(Height);//position
                                                            //kk[0] = 380; kk[1] = 290;
            kk[2] = k.Next(20, 40); //size
            kk[3] = k.Next(5, 10); //speed 

            aid = new Aid(new Point(kk[0], kk[1]), new Size(kk[2], kk[2]), (byte)kk[3]);
            
        }
        private static void LoadElements()
        { 
       //---------------------     
            asteroids = new asteroid[4];
            for (int i = 0; i < asteroids.Length; i++)
            {
                loadAster(ref asteroids[i]);
            }

                aids = new Aid[(asteroids.Length / 10)==0 ? 1: 1];
            for (int i = 0; i < aids.Length; i++)
            {
                loadAids(ref aids[i]);
            }


                stars = new Star[60];
            for (int i=0; i<stars.Length; i++)
            {
                stars[i] = new Star(new Point(k.Next(Width), k.Next(Height)));
            }
/*
            aids = new Aid[asteroids.Length/10];
            for (int i = 0; i < asteroids.Length; i++)
            {
                aids[i] = (Aid)generator();
            }*/
//------------------------------
            //Bullet _bullet=new Bullet()
        }
        public static void aim(Point pos)
        {
            buffer.Graphics.DrawLine(new Pen(Color.Red), pos.X - 5, pos.Y, pos.X + 5, pos.Y);
            buffer.Graphics.DrawLine(new Pen(Color.Red), pos.X, pos.Y - 5, pos.X, pos.Y + 5);
        }
        

public static void Draw()
        {
            buffer.Graphics.Clear(Color.Black);
            Point p;
            foreach (Star se in stars)    
                buffer.Graphics.DrawImage(se.draw(out p), p);
                      
            buffer.Graphics.DrawImage(new Bitmap(Resources.planet, new Size(200, 200)), 100, 100);
            #region Режет глаз 2  
//---------------------------------
            for (int i=0; i<asteroids.Length; i++)
            {
                if (asteroids[i] != null)
                {
                    buffer.Graphics.DrawImage(asteroids[i].draw(out p), p);
                    if (asteroids[i].Hit)
                        ship.damage(10, p);
                }
                else
                    if (k.Next(asterAppProb * 10) == 1) loadAster(ref asteroids[i]);
            }
            
            for (int i = 0; i < aids.Length; i++)
            {
                if (aids[i] != null)
                {
                    buffer.Graphics.DrawImage(aids[i].draw(out p), p);
                    if (aids[i].Hit)
                    {
                        ship.damage(-10, p);
                        aids[i].asterFinish();
                    }
                }
                else
                    if (k.Next(aidAppProb * 10) == 1) loadAids(ref aids[i]);
            }
//------------------------------------
            #endregion

            foreach (crack cr in Ship.cracks)            
                buffer.Graphics.DrawImage(cr.draw(out p), p);      

            foreach (Bullet bullet in queue)
                    buffer.Graphics.DrawImage(bullet.draw(out p), p);
            
            aim(new Point(400, 300));
            buffer.Graphics.DrawString("Energy:" + ship.Energy, SystemFonts.DefaultFont, Brushes.White, 0, 0);

            buffer.Render();  
        }
        static void Update()
        {
            bool target=false;//if (bullet != null)
            foreach (Bullet bullet in queue)
                if (bullet.Move()) target = true; //else target = false;
            #region режет глаз 3     
//-------------------------------
            foreach (asteroid ast in asteroids)
                {
                    if (ast != null)
                    {
                        ast.Move(new Size(Width, Height));
                    
                        if (target)
                        {
                        //buffer.Graphics.DrawRectangle(new Pen(Color.White), queue.First().rect);
                        buffer.Graphics.DrawImage(new Bitmap(Resources.bum), queue.First().rect);
                        buffer.Render();
                            if (ast.IsCollision(queue.First()))
                                ast.asterFinish(); //Hit!!      
                        } 
                    }        
                }
                if (target) queue.Remove(queue.First());
                for (int i = 0; i < asteroids.Length; i++)
                    if ((asteroids[i]!=null)&&(asteroids[i].Levelife == 4)) //Time is over
                        Array.Clear(asteroids, i, 1); //Delete from array   

            foreach (Aid aid in aids)
            {
                if (aid != null)
                {
                    aid.Move(new Size(Width, Height));

                    if (target)
                    {
                        //buffer.Graphics.DrawRectangle(new Pen(Color.White), queue.First().rect);
                        buffer.Graphics.DrawImage(new Bitmap(Resources.bum), queue.First().rect);
                        buffer.Render();
                        if (aid.IsCollision(queue.First()))
                            aid.asterFinish(); //Hit!!      
                    }
                }
            }
            if (target) queue.Remove(queue.First());
            for (int i = 0; i < aids.Length; i++)
                if ((aids[i] != null) && (aids[i].Levelife == 4)) //Time is over
                    Array.Clear(aids, i, 1); //Delete from array   
                                                  //--------------------------------------
            #endregion Режет глаз 3


        }
        private static void timer1_Tick(object sender, EventArgs e)
        {
            
            Draw();
            Update();
        }

        
    }  
}
