using System.Collections.Generic;

namespace Common.Comperator
{
    public class NumericComparer<T> : IComparer<T>
    {
        public int Compare(T x, T y)
        {
            if ((x is string) && (y is string))
            {
                return CompaireNatural.Compare(x.ToString(), y.ToString());
            }
            return -1;
        }
    }
}