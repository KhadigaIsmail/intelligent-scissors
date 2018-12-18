using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntelligentScissors
{
    public class Pair<T, U> where U: IComparable
    {
        public Pair()
        {
        }

        public Pair(T first, U second)
        {
            this.First = first;
            this.Second = second;
        }

        public T First { get; set; }
        public U Second { get; set; }

        public int CompareTo(Pair<T, U> obj)
        {
            return Second.CompareTo(obj.Second);
        }
    };
}
