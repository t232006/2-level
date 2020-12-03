using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Elements1.Properties;
using System.Windows.Forms;

namespace elements
{
    public class asteroid : SpaceElement, IImpact, IMoving
    {
        //protected Point realpos;
        //protected byte coef;
        protected double ang; //angle
        protected Size size;
        protected byte speed;
        protected byte zoomspeed;
        protected bool zoom;         //false - object moves in plate
        protected int CorX; 
        protected int CorY;
        byte zoomprobability = 90;    //probability object will approach or depart

        protected string filepicture;     //picture from resources
        Size limitsizeMin = new Size(4, 4);    //limit of departure size
        Size limitsizeMax = new Size(150, 150); //limit of approach size
        protected direction direction;            //enum zoom or less
        bool hit;
        Timer timer2 = new Timer();
        byte levellife=1;
        public byte Levelife { get => levellife; }
        public bool Hit { get=> hit; }

        public Rectangle rect => new Rectangle(pos, size);
        /*public void Collision(IImpact obj)
        {
            //if (obj is Bullet) 
            if (obj is asteroid)
            {

                if ((ang>0)&&(ang<Math.PI/2) || (ang>Math.PI)&&(ang<Math.PI*4/3)) ang = Math.PI - ang;else
                if ((ang > Math.PI / 2) && (ang < Math.PI)  || (ang > Math.PI * 4 / 3) && (ang < 2*Math.PI)) ang = -ang;
            }
        }*/
        public void asterFinish()
        {
            timer2.Tick += timer2_Tick;
            timer2.Interval = 300;
            timer2.Start();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            filepicture = $"expl{levellife}";
            var bm = Resources.ResourceManager.GetObject(filepicture);
            this.image = new Bitmap((Bitmap)bm, new Size(size.Width*levellife,size.Height*levellife));
            levellife++;
            if (levellife == 4) 
                timer2.Stop();  
        }

        public bool IsCollision(IImpact obj)
        {
            return obj.rect.IntersectsWith(this.rect);
        }
        public void SpeedCor(List<Keys> KeysPressed)
        {
            const int RS=15;
            if (KeysPressed.Contains(Keys.A)) CorX = RS;
            else
                if (KeysPressed.Contains(Keys.D)) CorX = -RS; 
                    else CorX = 0;

            if (KeysPressed.Contains(Keys.S)) CorY = -RS;
            else
                if (KeysPressed.Contains(Keys.W)) CorY = RS;
                    else CorY = 0;
        }


        public byte ZoomProbability
        {
            get => zoomprobability;
            set { if ((value > 100) || (value < 0)) zoomprobability = 0; else zoomprobability = value; }
        }
        public asteroid(Point pos, Size size, byte speed) : base(pos)
        {
            this.pos = pos;
            this.size = size;
            this.speed = speed;
            //coef = (byte)(speed / 5); //relation real speed and screen speed
            //realpos.X = pos.X*coef; realpos.Y = pos.Y * coef;
            //this.ang = ang;
            zoomspeed = 1;
            Random k = new Random(pos.X*pos.Y);
            ang = k.NextDouble() * 2 * Math.PI; //set angle on this stage
            int r = k.Next(1, 5);
            filepicture = $"asteroid0{r}";
            var bm = Resources.ResourceManager.GetObject(filepicture);
            this.image = new Bitmap((Bitmap)bm, size);
            r = k.Next(101);
            if (r <= zoomprobability)
            {
                zoom = true;
                if (r <= zoomprobability / 2) direction = direction.grow; else direction = direction.less;
            }
            else zoom = false;
        }
        public override Bitmap draw(out Point position)
        {
            position = pos;
            hit = false;   //trigger - if true it's nessesary to change girection
            if (zoom)
            {
                Size zs = new Size(zoomspeed, zoomspeed);
                if (direction == direction.grow)
                    size += zs;//= new Size(size+new Size(zoomspeed,zoomspeed));
                else
                    size -= zs; //= new Size(size.Width - 3, size.Height - 3);
                if (size.Equals(limitsizeMax))
                {
                    direction = direction.less;
                    hit = true;
                }
                else
                if (size.Equals(limitsizeMin)) direction = direction.grow;
                try
                {
                    image = new Bitmap((Bitmap)Resources.ResourceManager.GetObject(filepicture), size);
                } catch { levellife = 4; } finally { };
                
            }
            return image;
        }
        public virtual void Move(Size size)
        {
            int dx = (int)Math.Round(speed * Math.Cos(ang));
            int dy = (int)Math.Round(speed * Math.Sin(ang));
            /*realpos.X +=dx ;
            realpos.Y += dy;*/
            pos.X += dx+CorX;//realpos.X; // coef;
            pos.Y += dy+CorY;//realpos.Y; // coef;
            if ((pos.X < 0) || (pos.X > size.Width)) ang = Math.PI - ang;           
            if ((pos.Y < 0) || (pos.Y > size.Height)) ang = -ang;     
        }
    }
}
