using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace elements
{
    public abstract class SpaceElement
    {
        protected Point pos;        //real coord

        public Bitmap image;
        public SpaceElement(Point pos) => this.pos = pos;
        
        public abstract Bitmap draw(out Point position);
        //public abstract Bitmap draw(out Point position, out bool hit);
    }
    public interface IImpact
    {
        bool IsCollision(IImpact obj);
        void Collision(IImpact obj);

        Rectangle rect { get; }

    }
}
