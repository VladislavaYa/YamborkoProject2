using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ямборко_КП
{
    public partial class Form1 : Form
    {
        ModelGraph graph_1;
        ModelGraph graph_2;
        InputGraph input_graph_1;
        InputGraph input_graph_2;
        CyclomaticNumber find_number_graph1;
        CyclomaticNumber find_number_graph2;
        GraphBuilder build_graph1;
        GraphBuilder build_graph2;

        public Form1()
        {
            InitializeComponent();
            graph_1 = new ModelGraph();
            graph_2 = new ModelGraph();
            input_graph_1 = new InputGraph(graph_1);
            input_graph_2 = new InputGraph(graph_2);
            find_number_graph1 = new CyclomaticNumber(graph_1);
            find_number_graph2 = new CyclomaticNumber(graph_2);
            build_graph1 = new GraphBuilder(pictureBox1);
            build_graph2 = new GraphBuilder(pictureBox2);
            button2.Enabled = false;
            button3.Enabled = false;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox1.Text) && String.IsNullOrEmpty(textBox2.Text) && String.IsNullOrEmpty(textBox3.Text) && String.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show("Вы не ввели графы.", "Error", MessageBoxButtons.OK);
            }
            else
            {
                try
                {
                    graph_1.set_all_zero();
                    graph_2.set_all_zero();
                    input_graph_1.TextboxRead(textBox1.Text, textBox2.Text);
                    input_graph_2.TextboxRead(textBox3.Text, textBox4.Text);
                    dataGridView1.RowCount = dataGridView1.ColumnCount = graph_1.Vertex + 1;
                    dataGridView2.RowCount = dataGridView2.ColumnCount = graph_2.Vertex + 1;
                    dataGridView1.RowCount = dataGridView1.ColumnCount = graph_1.Vertex;
                    dataGridView2.RowCount = dataGridView2.ColumnCount = graph_2.Vertex;
                    build_graph1.DrawGraph(graph_1.bool_matrix_for_pictureBox());
                    build_graph2.DrawGraph(graph_2.bool_matrix_for_pictureBox());
                    button2.Enabled = true;
                }
                catch (TBException er)
                {
                    MessageBox.Show(er.Message, "Error", MessageBoxButtons.OK);
                }
                catch (MatrException er)
                {
                    MessageBox.Show(er.Message, "Error", MessageBoxButtons.OK);
                }
                catch (Exception er)
                {
                    MessageBox.Show(er.Message, "Error", MessageBoxButtons.OK);
                }
            }
        }

        private void dataGridView1_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            e.Value = graph_1.Matrix_adjacency[e.RowIndex, e.ColumnIndex];
        }

        private void dataGridView2_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            e.Value = graph_2.Matrix_adjacency[e.RowIndex, e.ColumnIndex];
        }

        private void button4_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            if (radioButton2.Checked == true)
            {
                graph_1.set_all_zero();
                openFileDialog1.FileName = "Graph_1";
                openFileDialog1.Filter = "Text Files(*.txt) | *.txt";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        input_graph_1.File(openFileDialog1.FileName);
                        input_graph_1.Fill_textBox(ref textBox1, ref textBox2);
                    }
                    catch (FileException error)
                    {
                        MessageBox.Show(error.Message, "Ошибка", MessageBoxButtons.OK);
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show(error.Message, "Ошибка", MessageBoxButtons.OK);
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
                find_number_graph1.Characteristic();
                find_number_graph2.Characteristic();
                MessageBox.Show(Compare.Comp(graph_1, graph_2), "Result", MessageBoxButtons.OK);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            pictureBox2.Image = null;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
            if (radioButton4.Checked == true)
            {
                graph_2.set_all_zero();
                openFileDialog1.FileName = "Graph_2";
                openFileDialog1.Filter = "Text Files(*.txt) | *.txt";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        input_graph_2.File(openFileDialog1.FileName);
                        input_graph_2.Fill_textBox(ref textBox3, ref textBox4);
                        
                    }
                    catch (FileException error)
                    {
                        MessageBox.Show(error.Message, "Ошибка", MessageBoxButtons.OK);
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show(error.Message, "Ошибка", MessageBoxButtons.OK);
                    }
                } 
            }
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            button3.Enabled = true;
            if (pictureBox1.Image != null && pictureBox2.Image != null)
            {
                DeleteLoops.delete(graph_1.Matrix_adjacency, graph_1.Vertex);
                DeleteLoops.delete(graph_2.Matrix_adjacency, graph_2.Vertex);
                dataGridView1.RowCount = dataGridView1.ColumnCount = graph_1.Vertex + 1;
                dataGridView2.RowCount = dataGridView2.ColumnCount = graph_2.Vertex + 1;
                dataGridView1.RowCount = dataGridView1.ColumnCount = graph_1.Vertex;
                dataGridView2.RowCount = dataGridView2.ColumnCount = graph_2.Vertex;
                build_graph1.DrawGraph(graph_1.bool_matrix_for_pictureBox());
                build_graph2.DrawGraph(graph_2.bool_matrix_for_pictureBox());
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox1.Enabled = true;
            textBox2.Enabled = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox1.Enabled = false;
            textBox2.Enabled = false;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            textBox3.Clear();
            textBox4.Clear();
            textBox3.Enabled = true;
            textBox4.Enabled = true;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            textBox3.Clear();
            textBox4.Clear();
            textBox3.Enabled = false;
            textBox4.Enabled = false;
        }
    }
}
