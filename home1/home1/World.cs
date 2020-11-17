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
    static class World
    {
        private static BufferedGraphicsContext context;
        public static BufferedGraphics buffer;

        public static int Width { set; get; }
        public static int Height { set; get; }
        //static World() { }
        static asteroid[] asteroids;
        static SpaceElement[] stars;
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
            Timer timer1 = new Timer() { Interval = 100 };
            timer1.Tick += timer1_Tick;
            timer1.Start();
        }

        private static void LoadElements()
        {
            asteroids = new asteroid[20];
            stars = new SpaceElement[60];

            Random k = new Random();
            for (int i=0; i<asteroids.Length; i++)
            {
                int[] kk=new int[4];
                kk[0] = k.Next(Width); kk[1] = k.Next(Height);//position
                kk[2] = k.Next(1, 20); //size
                kk[3] = k.Next(1, 5); //speed 
                double ang = k.Next();
                asteroids[i] = new asteroid(new Point(kk[0], kk[1]), new Size(kk[2], kk[2]), ang, (byte)kk[3]);
            }
            for (int i=0; i<stars.Length; i++)
            {
                stars[i] = new SpaceElement(new Point(k.Next(Width), k.Next(Height)));
            }
        }

        public static void Draw()
        {
            buffer.Graphics.Clear(Color.Black);
            
            foreach (SpaceElement se in stars)
            {
                Point p = se.draw();
                buffer.Graphics.FillRectangle(new SolidBrush(Color.White), new Rectangle(p, new Size(1, 1)));
            }
            
            buffer.Graphics.DrawImage(new Bitmap(Resources.planet, new Size(200, 200)), 100, 100);
            
            foreach (asteroid se in asteroids)
            {
                Point P;
                buffer.Graphics.DrawImage(se.draw(out P), P);
            }
            buffer.Render();
            

        }
        static void Update()
        {
            foreach (asteroid se in asteroids)
                se.Move(new Size(Width, Height));
        }
        private static void timer1_Tick(object sender, EventArgs e)
        {
            Draw();
            Update();
        }
    }  
}
