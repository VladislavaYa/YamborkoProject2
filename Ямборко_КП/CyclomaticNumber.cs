using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ямборко_КП
{
    class CyclomaticNumber
    {
        ModelGraph graph;
        int[] temp_array;
        public CyclomaticNumber(ModelGraph graph)
        {
            this.graph = graph;
            
        }
        public void Characteristic()
        {
            graph.Counter_connectivity = 0;
            temp_array = new int[graph.Vertex];
            for (int i = 0; i < graph.Vertex; i++)
            {
                if (temp_array[i] != 0)
                {
                    continue;
                }
                temp_array[i] = ++graph.Counter_connectivity;
                for (int j = 0; j < graph.Vertex; j++)
                {
                    if (graph.Matrix_adjacency[i, j] == 1 && temp_array[j] == 0)
                    {
                        temp_array[j] = graph.Counter_connectivity;
                        find1(j);
                    }
                }
            }
        }
        private void find1(int k)
        {
            for (int i = 0; i < graph.Vertex; i++)
            {
                if (graph.Matrix_adjacency[k, i] == 1 && temp_array[i] == 0)
                {
                    temp_array[i] = graph.Counter_connectivity;
                    find1(i);
                }
            }
        }
    }
}
