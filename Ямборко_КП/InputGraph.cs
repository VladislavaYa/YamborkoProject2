using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ямборко_КП
{
    class InputGraph
    {
        ModelGraph graph;
        string string_G;
        string string_P;

        public InputGraph(ModelGraph graph) {

            this.graph = graph;
        }

        public void File(string s) { 
            using (StreamReader streamReader = new StreamReader(s, System.Text.Encoding.Default)) {
                string_G = streamReader.ReadLine();
                string_P = streamReader.ReadLine();
            }
            if(String.IsNullOrEmpty(string_G) && String.IsNullOrEmpty(string_P)) { throw new FileException("Файл пустой."); }
            string_G = System.Text.RegularExpressions.Regex.Replace(string_G, @"\s+", " ");
            string_G = string_G.Trim();
            if (String.IsNullOrEmpty(string_G) && String.IsNullOrEmpty(string_P)) { throw new FileException("Файл пустой."); }
            if (String.IsNullOrEmpty(string_G) || String.IsNullOrEmpty(string_P)) { throw new FileException("В файле недостаточно данных."); }
            string_P = System.Text.RegularExpressions.Regex.Replace(string_P, @"\s+", " ");
            string_P = string_P.Trim();
        }

        public void TextboxRead(string S_G, string S_P) {
            S_G = S_G.Trim();
            S_G = System.Text.RegularExpressions.Regex.Replace(S_G, @"\s+", " ");
            S_P = S_P.Trim();
            S_P = System.Text.RegularExpressions.Regex.Replace(S_P, @"\s+", " ");
            if (String.IsNullOrEmpty(S_P) && String.IsNullOrEmpty(S_G)) { throw new TBException("Вы не ввели граф."); }
            if (String.IsNullOrEmpty(S_G)) { throw new TBException("Вы не ввели строку G."); }
            if (String.IsNullOrEmpty(S_P)) { throw new TBException("Вы не ввели строку P."); }
            if (Check_on_number(S_G)) { throw new TBException("Строка G содержит не числовые символы."); }
            if (Check_on_number(S_P)) { throw new TBException("Строка P содержит не числовые символы."); }
            this.string_G = S_G;
            this.string_P = S_P;
            Count_get();
        }

        private void Count_get() {    
            int[] Array_G = Array.ConvertAll(Regex.Split(string_G, @"\s+"), int.Parse);
            int[] Array_P = Array.ConvertAll(Regex.Split(string_P, @"\s+"), int.Parse);

            foreach (int k in Array_G)
            {
                graph.Slats++;
            }
            foreach (int k in Array_P)
            {
                graph.Vertex++;
            }
            graph.Matrix_adjacency = new int[graph.Vertex, graph.Vertex];
            MFO(Array_G, Array_P);
            graph.test_matrix();
        }

        private bool Check_on_number(string inp)
        {
            foreach (char c in inp)
            {
                if (c == ' ')
                {
                    continue;
                }
                if (!Char.IsNumber(c))
                {
                    return true;
                }
            }
            return false;
        }

        private void MFO(int[] arr_G, int[] arr_P)
        {
            for (int i = 0; i < (arr_P.Length - 1); i++)
            {
                if (arr_P[i] > arr_P[i+1])
                {
                    throw new TBException("Строка P записана не правильно.");
                }
            }
            for (int i = 0, j = 0; i < arr_G.Length; i++)
            {
                if (arr_P.Length < arr_G[i])
                {
                    throw new TBException("В строке G есть связь с несуществующей вершиной.");
                }
                if (arr_P[j] == 0)
                {
                    j++;
                    i--;
                    continue;
                }
                else if (arr_P[j] >= (i + 1))
                {
                    
                    graph.Matrix_adjacency[j, arr_G[i] - 1] = 1;
                }
                else
                {
                    j++;
                    if (arr_P[j] >= (i + 1))
                    {

                        graph.Matrix_adjacency[j, arr_G[i] - 1] = 1;
                    }
                    else
                    {
                        i--;
                    }
                }
            }
        }

        public void Fill_textBox(ref TextBox TB_G, ref TextBox TB_P)
        {
            TB_G.Text = string_G;
            TB_P.Text = string_P;
        }
    }
}
