using Elements1.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;


namespace elements
{
    
    public class Star : SpaceElement
    {
        public Star(Point pos) : base(pos) { }
        public override Bitmap draw(out Point position)
        {
            position = pos;
            image = new Bitmap(2, 2);
            image.SetPixel(1, 1, Color.White);
            return image;
        }
    }
    public class Bullet : asteroid
    {
        public Bullet(Point pos, Size size, byte speed) :base(pos, size, speed)
        {
            filepicture = "laserRed011Rot";
            //image = new Bitmap((Bitmap)Resources.ResourceManager.GetObject(filepicture), size);
            zoom = true;
            direction = direction.less;
        }
        public new bool Move(Size size)    //ray moves to the screen center
        {
            int x1 = 0; int x2 = size.Width / 2;
            int y1 = size.Height-100; int y2 = size.Height / 2;
            
            pos.X += speed;
            pos.Y = (pos.X - x1) * (y2 - y1) / (x2 - x1) + y1;
            if (pos.X == x2) return true; else return false;//true - center is reached
        }

        
    }

    public enum direction {grow, less}
    public class crack : SpaceElement
    {
        protected Size size=new Size(160,160);
        public crack(Point pos) : base(pos)
        {
            image = new Bitmap(Resources.crack, size);   //from picture
        }

        public override Bitmap draw(out Point LeftTop)
        {
            LeftTop = new Point(pos.X,pos.Y);
            return image;

        }
    }
    
}
