using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ямборко_КП
{
    static class DeleteLoops
    {
        public static void delete(int[,] matrix, int n)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j && matrix[i, j] == 1)
                    {
                        matrix[i, j] = 0;
                    }
                }
            }
        }
    }
}
