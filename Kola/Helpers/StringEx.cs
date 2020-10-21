using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kola.Helpers
{
    static class StringEx
    {
        /// <summary>
        /// Measures differences of two strings using Levenshtein distance algorithm.
        /// </summary>
        public static int Distance(this string s, string t)
        {
            int[] v0 = new int[t.Length + 1];
            int[] v1 = new int[t.Length + 1];

            for(int i = 0; i < t.Length + 1; i++)
            {
                v0[i] = i;
            }

            for(int i = 0; i < s.Length; i++)
            {
                v1[0] = i + 1;
                for(int j = 0; j < t.Length; j++)
                {
                    int deletionCost = v0[j + 1] + 1;
                    int insertionCost = v1[j] + 1;
                    int substitutionCost;
                    if(s[i] == t[j])
                    {
                        substitutionCost = v0[j];
                    }
                    else
                    {
                        substitutionCost = v0[j] + 1;
                    }

                    v1[j + 1] = Math.Min(Math.Min(deletionCost, insertionCost), substitutionCost);

                    //swap v0 with v1
                    var tmp = v0;
                    v0 = v1;
                    v1 = tmp;
                }
            }
            return v0[t.Length];
        }
    }
}
