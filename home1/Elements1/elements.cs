using Elements1.Properties;
using System;
using System.Drawing;
using System.IO;


namespace elements
{
    public class SpaceElement //general class
    {
        protected Point pos;
        protected Bitmap image;
        public SpaceElement(Point pos)
        {
            this.pos = pos;
        }
        public virtual Bitmap draw(out Point position)
        {
            position = pos;
            image = new Bitmap(2, 2);
            image.SetPixel(1, 1, Color.White);
            return image;
        }
    }

    enum direction {grow, less}
    public class crack : SpaceElement
    {
        protected Size size=new Size(160,160);
        public crack(Point pos) : base(pos)
        {

        }

        public override Bitmap draw(out Point LeftTop)
        {
            LeftTop = new Point(pos.X,pos.Y);
            image = new Bitmap(Resources.crack,size);   //from picture
            return image;

        }
    }
    #region asteroid
    public class asteroid : SpaceElement
    {

        protected double ang; //angle
        protected Size size;
        protected byte speed;
        protected bool zoom;         //false - object moves in plate
        byte zoomprobability=25;    //probability object will approach or depart
        
        string filepicture;     //picture from resources
        Size limitsizeMin=new Size(4,4);    //limit of departure size
        Size limitsizeMax=new Size(150,150); //limit of approach size
        direction direction;            //enum zoom or less

        public byte ZoomProbability 
            { get => zoomprobability;
            set { if ((value > 100) || (value < 0)) zoomprobability = 0; else zoomprobability = value; }
             }
        public asteroid(Point pos, Size size, double ang, byte speed):base(pos)
        {
            this.pos = pos;
            //this.dir = dir;
            this.size = size;
            this.speed = speed;
            this.ang = ang;
            //this.zoomprobability = zp;
            
            DirectoryInfo dir = new DirectoryInfo(AppContext.BaseDirectory);
            Random rand = new Random();
            int r = rand.Next(1,4);
            filepicture= dir.Parent.Parent.Parent.FullName.ToString() + @"\Resources\asteroid0" + r.ToString()+".png";
            //Image pic = Image.FromFile(s) 
            this.image = new Bitmap(Image.FromFile(filepicture), size);
            r = rand.Next(100);
            if (r <= zoomprobability)
            {
                zoom = true;
                if (r <= zoomprobability / 2) direction = direction.grow; else direction = direction.less;
            }else zoom = false;
            
        }
        public Bitmap draw(out Point position, out bool hit)
        {
            position = pos;
            hit = false;   //trigger - if true it's nessesary to change girection
            if (zoom)
            {
                if (direction==direction.grow)
                size = new Size(size.Width + 1, size.Height + 1); else
                size = new Size(size.Width - 1, size.Height - 1);
                    if (size.Equals(limitsizeMax))
                    {
                        direction = direction.less;
                        hit = true;
                    }
                    else
                    if (size.Equals(limitsizeMin)) direction = direction.grow;
                image = new Bitmap(Image.FromFile(filepicture),size);
            }
            return image;
        }
        public void Move(Size size)
        {
            pos.X += (int)Math.Round(speed*Math.Cos(ang));
            pos.Y += (int)Math.Round(speed*Math.Sin(ang));
            if ((pos.X < 0) || (pos.X > size.Width)) ang = Math.PI-ang;
            if ((pos.Y < 0) || (pos.Y > size.Height)) ang = -ang;
        }

    }
    #endregion
}
