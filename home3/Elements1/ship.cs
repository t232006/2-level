using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using elements;

namespace elements
{
    public class Ship
    {
        public delegate void Message();
        public static event Message MessageDie;
        sbyte energy = 100;
        public sbyte Energy { get => energy; }
        public static List<crack> cracks = new List<crack>();
        public void damage(sbyte amount, Point p)
        {
            //Point p;
            if (amount > 0)
            {
                crack cr = new crack(p);
                cracks.Add(cr);
                
            }
            else if (cracks.Count!=0) cracks.Remove(cracks.First());
            energy -= amount;
            if (energy <= 0) MessageDie.Invoke();
        }
        
        //public override void List<crack>.Add
    }
}
