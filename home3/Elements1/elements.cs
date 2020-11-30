using Elements1.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

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
    public class Bullet : asteroid, IMoving
    {
        public Bullet(Point pos, Size size, byte speed) :base(pos, size, speed)
        {
            filepicture = "laserRed011Rot";
            //image = new Bitmap((Bitmap)Resources.ResourceManager.GetObject(filepicture), size);
            zoom = true;
            direction = direction.less;
            ang = -Math.PI / 6;
        }
        public bool Move()    //ray moves to the screen center
        {
            /*int x1 = 0; 
            int x2 = size.Width / 2;
            int y1 = size.Height-100; int y2 = size.Height / 2;
            
            pos.X += speed+CorX;
            pos.Y = (pos.X - x1) * (y2 - y1) / (x2 - x1) + y1+CorY;*/
            int dx = (int)Math.Round(speed * Math.Cos(ang));
            int dy = (int)Math.Round(speed * Math.Sin(ang));
            pos.X += dx + CorX;
            pos.Y += dy + CorY;
            if (this.size.Width<5) return true; else return false;//true - center is reached
        }
        //public void SpeedCor(List<Keys> KeysPressed):base(KeysPressed);



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
