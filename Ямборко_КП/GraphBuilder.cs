using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ямборко_КП
{
    /// <summary>
    /// Класс для визуализации графа
    /// </summary>
    class GraphBuilder
    {
        /// <summary>
        /// Массив координат для построения вершин
        /// </summary>
        private PointF[] _pointsForVerticles;
        /// <summary>
        /// Смещение передвигаемой вершины относительно точки, которая попадает в одну из вершин, полученной щелчком по области для рисования
        /// </summary>
        private Point _delta;
        /// <summary>
        /// Область для рисования графа
        /// </summary>
        private PictureBox _region;
        /// <summary>
        /// Ссылка на объект класса Graphics - предоставление средств для рисования
        /// </summary>
        private Graphics _graphics;
        /// <summary>
        /// Ссылка на объект класса Bitmap - хранение полного изображения графа
        /// </summary>
        private Bitmap _bitmap;
        /// <summary>
        /// Ссылка на объект класса Bitmap - хранение неполного изображения графа(только той части, которая является неподвижной во время перемещения вершины)
        /// </summary>
        private Bitmap _backgroundBitmap;
        /// <summary>
        /// Карандаш для рисования ребер графа
        /// </summary>
        private Pen _grayPen;
        /// <summary>
        /// Карандаш для рисования подвижных ребер во время перемещения одной из вершин графа
        /// </summary>
        private Pen _orangeRedPen;
        /// <summary>
        /// Шрифт для рисования подписей номеров графа
        /// </summary>
        private Font _font;
        /// <summary>
        /// Цвет для ребер
        /// </summary>
        private Color _colorForEdge;
        /// <summary>
        /// Цвет для вершин
        /// </summary>
        private Color _colorForVerticle;
        /// <summary>
        /// Индекс передвигаемой вершины по области для рисования 
        /// </summary>
        private int _indexOfUsingVerticle;
        /// <summary>
        /// Хранение результата проверки вхождения точки, полученной кликом по области для рисования - true, если точка попадает в одну из вершин, false - если не попадает
        /// </summary>
        private bool _isClicked;
        /// <summary>
        /// Количество вершин
        /// </summary>
        private int _verticles;
        /// <summary>
        /// Количество ребер
        /// </summary>
        private int _edges;
        /// <summary>
        /// Матрица смежности
        /// </summary>
        private bool[][] _adjacencyMatrix;

        /// <summary>
        /// Конструктор с параметрами - установка области для рисования графа
        /// </summary>
        /// <param name="region">Ссылка на экземпляр класса PictureBox</param>
        public GraphBuilder(PictureBox region)
        {
            this._region = region;
            this._bitmap = new Bitmap(this._region.Width, this._region.Height);
            this._backgroundBitmap = new Bitmap(this._region.Width, this._region.Height);
            this._graphics = Graphics.FromImage(_bitmap);
            this._graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            this._region.MouseMove += pictureBox_MouseMove;
            this._region.MouseDown += pictureBox_MouseDown;
            this._region.MouseUp += pictureBox_MouseUp;
            this._colorForEdge = Color.OrangeRed;
            this._orangeRedPen = new Pen(_colorForEdge, 1f) { CustomEndCap = new AdjustableArrowCap(5, 5) };
            this._grayPen = new Pen(Color.Gray, 1f) { CustomEndCap = new AdjustableArrowCap(5, 5) };
            this._font = new Font("Tahoma", 9f);
        }

        /// <summary>
        /// Нарисовать граф
        /// </summary>
        /// <param name="AdjacencyMatrix">Матрица смежности</param>
        public void DrawGraph(bool[][] AdjacencyMatrix)
        {
            if (AdjacencyMatrix != null)
            {
                this._verticles = AdjacencyMatrix.Length;
                this._edges = FindNumberOfEdges(AdjacencyMatrix);
                this._adjacencyMatrix = AdjacencyMatrix;
                _graphics.Clear(_region.BackColor);
                CreatePointsForVerticles();
                DrawEdges();
                DrawVerticles();

                _region.Image = _bitmap;
            }
        }

        /// <summary>
        /// Найти координаты для построения вершин на области для рисования
        /// </summary>
        private void CreatePointsForVerticles()
        {
            _pointsForVerticles = new PointF[_verticles];
            double degree = 360.0 / _verticles;
            double d = 0;
            Point xy = new Point(_region.Width / 2 - 12, _region.Height / 2 - 15);
            Point x0y0 = new Point(_region.Width - 50, _region.Height / 2 - 15);
            Point rxry = new Point((x0y0.X - xy.X), (x0y0.Y - xy.Y));
            for (int i = 0; i < _verticles; i++, d += degree)
            {
                double cos = Math.Cos((Math.PI / 180) * d);
                double sin = Math.Sin((Math.PI / 180) * d);
                double tmps = (xy.X + rxry.X * cos - rxry.Y * sin);
                double tmp1 = (xy.Y + rxry.X * sin + rxry.Y * cos);
                Point x1y1 = new Point((int)tmps, (int)tmp1);
                _pointsForVerticles[i] = new Point(x1y1.X, x1y1.Y);
            }
        }


        /// <summary>
        /// Найти количество ребер в графе по матрице смежности - используется для рисования этого числа на области для рисования
        /// </summary>
        /// <param name="AdjacencyMatrix">Матрица смежности</param>
        /// <returns>Количество ребер</returns>
        private int FindNumberOfEdges(bool[][] AdjacencyMatrix)
        {
            int count = 0;
            for (int i = 0; i < AdjacencyMatrix.Length; i++)
            {
                for (int j = 0; j < AdjacencyMatrix.Length; j++)
                {
                    if (AdjacencyMatrix[i][j])
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// Нарисовать ребра
        /// </summary>
        private void DrawEdges()
        {
            if (_pointsForVerticles != null && _adjacencyMatrix != null)
            {
                for (int i = 0; i < _adjacencyMatrix.Length; i++)
                {
                    for (int j = 0; j < _adjacencyMatrix[i].Length; j++)
                    {
                        if (!(_isClicked && (_indexOfUsingVerticle == i || _indexOfUsingVerticle == j)))
                        {
                            if (i == j && _adjacencyMatrix[i][j])
                            {
                                _graphics.DrawEllipse(Pens.Gray, _pointsForVerticles[i].X - 3, _pointsForVerticles[i].Y - 18, 30, 30);
                            }
                            else if (_adjacencyMatrix[i][j])
                            {
                                double Xc = 0;
                                double Yc = 0;
                                GetPointsForEdge(ref Xc, ref Yc, _pointsForVerticles[j], _pointsForVerticles[i]);
                                _graphics.DrawLine(_grayPen, _pointsForVerticles[i].X + 12, _pointsForVerticles[i].Y + 12, (int)Xc + 12, (int)Yc + 12);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Нарисовать вершины
        /// </summary>
        private void DrawVerticles()
        {
            if (_pointsForVerticles != null && _adjacencyMatrix != null)
            {
                for (int i = 0; i < _adjacencyMatrix.Length; i++)
                {
                    _colorForVerticle = _isClicked && i == _indexOfUsingVerticle ? Color.Orange : Color.LightSeaGreen;
                    _graphics.FillEllipse(new SolidBrush(_colorForVerticle), _pointsForVerticles[i].X, _pointsForVerticles[i].Y, 25, 25);
                    if (i < 9)
                    {
                        _graphics.DrawString((i + 1).ToString(), _font, Brushes.GhostWhite, _pointsForVerticles[i].X + 8, _pointsForVerticles[i].Y + 5);
                    }
                    else
                    {
                        _graphics.DrawString((i + 1).ToString(), _font, Brushes.GhostWhite, _pointsForVerticles[i].X + 4, _pointsForVerticles[i].Y + 5);
                    }
                }
            }
        }

        /// <summary>
        /// Нарисовать подвижные ребра для перемещаемой вершины
        /// </summary>
        private void DrawEdgesForMovingVerticle()
        {
            for (int i = 0, index = _indexOfUsingVerticle; i < _adjacencyMatrix.Length; i++)
            {
                Pen p = new Pen(_colorForEdge);
                if (i == index && _adjacencyMatrix[i][index])
                {
                    _graphics.DrawEllipse(p, _pointsForVerticles[i].X - 3, _pointsForVerticles[i].Y - 18, 30, 30);
                }
                else if (_adjacencyMatrix[i][index] || _adjacencyMatrix[index][i])
                {
                    double Xc = 0;
                    double Yc = 0;
                    if (_adjacencyMatrix[i][index])
                    {
                        GetPointsForEdge(ref Xc, ref Yc, _pointsForVerticles[index], _pointsForVerticles[i]);
                        _graphics.DrawLine(_orangeRedPen, _pointsForVerticles[i].X + 12, _pointsForVerticles[i].Y + 12, (int)Xc + 12, (int)Yc + 12);
                    }
                    if (_adjacencyMatrix[index][i])
                    {
                        GetPointsForEdge(ref Xc, ref Yc, _pointsForVerticles[i], _pointsForVerticles[index]);
                        _graphics.DrawLine(_orangeRedPen, _pointsForVerticles[index].X + 12, _pointsForVerticles[index].Y + 12, (int)Xc + 12, (int)Yc + 12);
                    }
                }
            }
        }

        /// <summary>
        /// Получить координаты конца вектора для построения дуги
        /// </summary>
        /// <param name="Xc"></param>
        /// <param name="Yc"></param>
        /// <param name="firstPoint"></param>
        /// <param name="secondPoint"></param>
        private void GetPointsForEdge(ref double Xc, ref double Yc, PointF firstPoint, PointF secondPoint)
        {
            double Rab = Math.Sqrt((firstPoint.X - secondPoint.X) * (firstPoint.X - secondPoint.X) +
            (firstPoint.Y - secondPoint.Y) * (firstPoint.Y - secondPoint.Y));
            double k = Rab == 0 ? 0 : (Rab - 12) / Rab;
            Xc = secondPoint.X + (firstPoint.X - secondPoint.X) * k;
            Yc = secondPoint.Y + (firstPoint.Y - secondPoint.Y) * k;
        }

        /// <summary>
        /// Обработчик для проверки попадания полученной точки в результате клина по области для рисования на вхождение в одну из вершин графа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (_pointsForVerticles != null)
            {
                for (int i = 0; i < _pointsForVerticles.Length; i++)
                {
                    PointF ab = new PointF(25 / 2 + _pointsForVerticles[i].X, 25 / 2 + _pointsForVerticles[i].Y);
                    PointF xy = new PointF(e.X, e.Y);
                    int R = 25 / 2;

                    if (((ab.X - xy.X) * (ab.X - xy.X) + (ab.Y - xy.Y) * (ab.Y - xy.Y)) < R * R)
                    {
                        _isClicked = true;
                        _delta.X = (int)(e.X - _pointsForVerticles[i].X);
                        _delta.Y = (int)(e.Y - _pointsForVerticles[i].Y);
                        _indexOfUsingVerticle = i;
                        _graphics.Clear(_region.BackColor);
                        DrawEdges();
                        _backgroundBitmap = _bitmap.Clone(new Rectangle(0, 0, _region.Width, _region.Height), _bitmap.PixelFormat);
                    }
                }
            }
        }

        /// <summary>
        /// Обработчик для полной перерисовки графа после отпускания клавиши мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            _graphics.Clear(_region.BackColor);
            _isClicked = false;
            DrawEdges();
            DrawVerticles();
            _region.Image = _bitmap;
        }

        /// <summary>
        /// Обработчик для перерисовки подвижных ребер графа и перемещаемой вершины
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isClicked && _pointsForVerticles != null)
            {
                if ((e.X - _delta.X) < _region.Width - 25 && (e.X - _delta.X) >= 0)
                {
                    _pointsForVerticles[_indexOfUsingVerticle].X = e.X - _delta.X;
                }
                if ((e.Y - _delta.Y) < _region.Height - 25 && (e.Y - _delta.Y) >= 0)
                {
                    _pointsForVerticles[_indexOfUsingVerticle].Y = e.Y - _delta.Y;
                }
                _graphics.Clear(_region.BackColor);
                _graphics.DrawImage(_backgroundBitmap, 0, 0, _region.Width, _region.Height);
                DrawEdgesForMovingVerticle();
                DrawVerticles();
                _region.Image = _bitmap;
            }
        }
    }
}

