using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace home_21
{
    class Program
    {
        class employers
        {
            public employer[] empl;
            
            public void output()
            {
                foreach (employer e in empl)
                    Console.WriteLine($"{e.name} --> {e.Rate}");
            }

            public employers(List<employer> list)
            {
                //empl = new employer[list.Count];
                empl=list.ToArray();
            }
            public enum en{salery, name };
            public void sort(en en)
            {
                switch (en)
                {
                    case en.name:
                        Array.Sort(empl);
                        break;
                    case en.salery:
                        Array.Sort(empl, new SaleryComparer());
                        break;
                }
            }
        }  
        static List<employer> readfile()
        {
            List<employer> list1 = new List<employer>();
            StreamReader f = new StreamReader("employers.txt");
            while (!f.EndOfStream)
            {
                string s = f.ReadLine();
                string[] ss=s.Split(new char[] { ' ' });
                employer bc = new employer();
                bc.name = ss[0]; bc.SetRate(double.Parse(ss[1]));
                list1.Add(bc);
            } 
            f.Close();
            return list1;
        }
   
        static void Main(string[] args)
        {
            employers em = new employers(readfile());
            em.sort(employers.en.name);
            em.output();
            em.sort(employers.en.salery);
            Console.WriteLine();
            em.output();
            Console.ReadLine();
            
        }
    }
}
