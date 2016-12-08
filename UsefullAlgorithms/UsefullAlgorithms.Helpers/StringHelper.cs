using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsefullAlgorithms.Helpers
{
    public static class StringHelper
    {
        public static int LevenhsteinDistance(this string a, string b)
        {
            if (a.Length == 0)
                return b.Length;

            if (b.Length == 0)
                return a.Length;

            var array = new int[a.Length + 1, b.Length + 1];

            for (int i = 0; i <= a.Length; array[i, 0] = i++) ;
            for (int j = 0; j <= b.Length; array[0, j] = j++) ;

            for(int i = 1; i < a.Length; ++i)
            {
                for(int j = 1; j < b.Length; ++j)
                {
                    int cost = b[j - 1] == a[i - 1] ? 0 : 1;

                    array[i, j] = Math.Min(Math.Min(array[i - 1, j] + 1, array[i, j - 1] + 1), array[i - 1, j - 1] + cost);
                }
            }
            return array[a.Length, b.Length];
        }
    }
}
