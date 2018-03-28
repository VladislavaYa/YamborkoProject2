using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ямборко_КП
{
    static class Compare
    {
        static public string Comp(ModelGraph graph1, ModelGraph graph2)
        {
            graph1.Counter_slats();
            graph2.Counter_slats();
            int g1 = graph1.Slats - graph1.Vertex + graph1.Counter_connectivity;
            int g2 = graph2.Slats - graph2.Vertex + graph2.Counter_connectivity;
            string s_equal = "  Цикломатическое число  \n" + "Первый граф: " + 
                                g1 + "\nВторой граф: " +
                                g2 + "\n\nГрафы эквивалентны.";
            string s_noequal = "  Цикломатическое число  \n" + "Первый граф: " +
                                g1 + "\nВторой граф: " +
                                g2 + "\n\nГрафы не эквивалентны.";
            if (g1 == g2) { return s_equal; }
            else { return s_noequal; }
        }
    }
}
