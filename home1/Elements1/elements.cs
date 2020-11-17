using Elements1.Properties;
using System;
using System.Drawing;
using System.IO;


namespace elements
{
    public class SpaceElement
    {
        protected Point pos;
        public SpaceElement(Point pos)
        {
            this.pos = pos;
        }
        public virtual Point draw()
        {
            return pos;
        }
    }
    
    
    public class asteroid: SpaceElement
    {
        
        protected double ang; //angle
        protected Size size;
        protected byte speed;
        protected Bitmap image;
        public asteroid(Point pos, Size size, double ang, byte speed):base(pos)
        {
            this.pos = pos;
            //this.dir = dir;
            this.size = size;
            this.speed = speed;
            this.ang = ang;
            
            DirectoryInfo dir = new DirectoryInfo(AppContext.BaseDirectory);
            Random rand = new Random();
            int r = rand.Next(1,4);
            string s= dir.Parent.Parent.Parent.FullName.ToString() + @"\Resources\asteroid0" + r.ToString()+".png";
            //Image pic = Image.FromFile(s) 
            this.image = new Bitmap(Image.FromFile(s), size);
        }
        public Bitmap draw(out Point position)
        {
            position = pos; 
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
    /*public class Star : SpaceElement
    {
        public Star(Point pos, Size size, double ang, byte speed) :base SpaceElement(pos, size, ang, speed)
        {
        }
    }*/
}
