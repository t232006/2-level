using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace home_21
{

    abstract class baseClass
    {
        public string name { get; set; }
        protected double salery;
        protected double rate;
        public double Rate { get => rate; } //setting is forbidden for public
        protected double count; //total sum of salery
        protected double Count { get => count; } //setting is forbidden for public
        public abstract void GiveSalery();
        public void SetRate(double rate) //use for public
        {
            this.rate = rate;
        }

        /*public baseClass(string name, double salery)
        {
            this.name = name; this.salery = salery;
        }*/

    }
    class employer:baseClass, IComparable<employer>
    {
        public int CompareTo([AllowNull] employer other)
        {
            return string.Compare(this.name, other.name);
        }
        public override void GiveSalery() //костыль-заглушка
        {
            
        }
    }
    class SaleryComparer : IComparer<employer>
    {
        public int Compare([AllowNull] employer x, [AllowNull] employer y)
        {
            return (int)x.Rate - (int)y.Rate;
        }
    }
    class staff : baseClass 
    {
        public override void GiveSalery()
        {
            salery = rate;
            count += salery;
        }
    }
    class overstaff : baseClass
    {
        public override void GiveSalery()
        {
            salery = 8 * 20.8 * rate;
            count += salery;
        }
    }
}
