using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Elements1.Properties;

namespace elements
{
    public class asteroid : SpaceElement, IImpact
    {
        //protected Point realpos;
        //protected byte coef;
        protected double ang; //angle
        protected Size size;
        protected byte speed;
        protected bool zoom;         //false - object moves in plate
        byte zoomprobability = 0;    //probability object will approach or depart

        protected string filepicture;     //picture from resources
        Size limitsizeMin = new Size(4, 4);    //limit of departure size
        Size limitsizeMax = new Size(150, 150); //limit of approach size
        protected direction direction;            //enum zoom or less
        bool hit;
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

        public bool IsCollision(IImpact obj)
        {
            return obj.rect.IntersectsWith(this.rect);
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
                if (direction == direction.grow)
                    size = new Size(size.Width + 1, size.Height + 1);
                else
                    size = new Size(size.Width - 1, size.Height - 1);
                if (size.Equals(limitsizeMax))
                {
                    direction = direction.less;
                    hit = true;
                }
                else
                if (size.Equals(limitsizeMin)) direction = direction.grow;
                image = new Bitmap((Bitmap)Resources.ResourceManager.GetObject(filepicture), size);
            }
            return image;
        }
        public virtual void Move(Size size)
        {
            int dx = (int)Math.Round(speed * Math.Cos(ang));
            int dy = (int)Math.Round(speed * Math.Sin(ang));
            /*realpos.X +=dx ;
            realpos.Y += dy;*/
            pos.X += dx;//realpos.X; // coef;
            pos.Y += dy;//realpos.Y; // coef;
            if ((pos.X < 0) || (pos.X > size.Width)) ang = Math.PI - ang;           
            if ((pos.Y < 0) || (pos.Y > size.Height)) ang = -ang;     
        }

    }
}
