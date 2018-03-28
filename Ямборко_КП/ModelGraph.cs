using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ямборко_КП
{
    class ModelGraph
    {
        private int[,] matrix_adjacency;
        private int vertex;
        private int slats;
        private int counter_connectivity;

        public int[,] Matrix_adjacency
        {
            get
            {
                return matrix_adjacency;
            }
            set
            {
                matrix_adjacency = value;
            }
        }

        public int Vertex
        {
            get
            {
                return vertex;
            }
            set
            {
                vertex = value;
            }
        }

        public int Slats
        {
            get
            {
                return slats;
            }
            set
            {
                slats = value;
            }
        }

        public int this[int i, int j]
        {
            get
            {
                return matrix_adjacency[i, j];
            }
            set
            {
                matrix_adjacency[i, j] = value;
            }
        }

        public int Counter_connectivity
        {
            get
            {
                return counter_connectivity;
            }
            set
            {
                counter_connectivity = value;
            }
        }

        public ModelGraph()
        {
            Vertex = 0;
            Slats = 0;
            Counter_connectivity = 0;
        }

        public void set_all_zero()
        {
            Vertex = 0;
            Slats = 0;
            Counter_connectivity = 0;
        }

        public void Counter_slats()
        {
            Slats = 0;
            for (int i = 0; i < Vertex; i++)
            {
                for (int j = 0; j < Vertex; j++)
                {
                    if (matrix_adjacency[i, j] == 1)
                    {
                        Slats++;
                    }
                }
            }
            Slats /= 2;
        }

        public bool[][] bool_matrix_for_pictureBox()
        { 
            bool[][] matrix_bool = new bool[Vertex][];
            for (int i = 0; i < Vertex; i++)
            {
                matrix_bool[i] = new bool[Vertex];
            }

            for (int i = 0; i < Vertex; i++)
            {
                for (int j = 0; j < Vertex; j++)
                {
                    matrix_bool[i][j] = Convert.ToBoolean(Matrix_adjacency[i, j]);
                }
            }
            return matrix_bool;
        }

        public void test_matrix()
        {
            if (Vertex > 20)
            {
                throw new MatrException("Вершин в графе больше 20!");
            }
            else if (Slats > 50)
            {
                throw new MatrException("Ребер в графе больше 50!");
            }

            for (int i = 0; i < Vertex; i++)
            {
                for (int j = 0; j < Vertex; j++)
                {
                    if (Matrix_adjacency[i, j] == 1 && Matrix_adjacency[j, i] != 1)
                    {
                        throw new MatrException("Введенный граф не является неориентированным!");
                    }
                }
            }
        }

       

        
    }
}
