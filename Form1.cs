using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dirihle
{
    public partial class Form1 : Form
    {
        private double h;
        private double h2;
        private double k;
        private double k2;
        private double a2;
        private double Xn = 1;
        private double Xo = -1;
        private double Yn = 1;
        private double Yo = -1;
        private double acc_max = 0.0000001;
        private double accuracy = 0.0;

        private double[,] v;

        uint counter = 0u;
        private uint Nmax;
        private uint N;
        private uint M;

        public Form1()
        {
            InitializeComponent();

            N = 1;
            M = 1;

            XoBox.Minimum = decimal.MinValue;
            XoBox.Maximum = decimal.MaxValue;
            XnBox.Minimum = decimal.MinValue;
            XnBox.Maximum = decimal.MaxValue;

            YoBox.Minimum = decimal.MinValue;
            YoBox.Maximum = decimal.MaxValue;
            YnBox.Minimum = decimal.MinValue;
            YnBox.Maximum = decimal.MaxValue;

            XoBox.Value = -1;
            XnBox.Value = +1;
            YoBox.Value = -1;
            YnBox.Value = +1;

            AccuracyBox.Text = "0.00001";
            NmaxBox.Value = 100;
            Nbox.Value = 3;
            Mbox.Value = 3;

            Table.RowHeadersVisible = false;
            Table.ColumnHeadersVisible = false;
        }


        private void ZeidelMethod()
        {
            v = new double[N + 1, M + 1];

            h = (Xn - Xo) / N;
            h2 = -Math.Pow(N / (Xn - Xo), 2);
            k = (Yn - Yo) / M;
            k2 = -Math.Pow(M / (Yn - Yo), 2);
            a2 = -2.0 * (h2 + k2);

            for (uint i = 0u; i < N + 1; ++i)
            {
                v[i, 0] = mu3(X(i));

                for (uint j = 1u; j < M + 1; ++j)
                {
                    v[i, j] = 0.0;
                }

                v[i, M] = mu4(X(i));
            }

            for (uint j = 0u; j < M + 1; ++j)
            {
                v[0, j] = mu1(Y(j));
                v[N, j] = mu2(Y(j));
            }

            double current_accuracy;
            double current = 0.0;
            accuracy = 0.0;
            counter = 0u;

            do
            {
                accuracy = 0.0;

                for (uint j = 1u; j < M; ++j)
                {
                    for (uint i = 1u; i < N; ++i)
                    {
                        current = v[i, j];

                        v[i, j] = 0.0;
                        v[i, j] += -k2 * (V(i, j + 1) + V(i, j - 1));
                        v[i, j] += -h2 * (V(i + 1, j) + V(i - 1, j));
                        v[i, j] += Function(i, j);
                        v[i, j] /= a2;

                        current_accuracy = Math.Abs(current - v[i, j]);

                        if (accuracy < current_accuracy)
                        {
                            accuracy = current_accuracy;
                        }
                    }
                }

                counter++;

            } while ((Nmax > counter) && (accuracy >= acc_max));
        }


        private void button1_Click(object sender, EventArgs e)
        {
            N = (uint)Nbox.Value;
            M = (uint)Mbox.Value;
            Nmax = (uint)NmaxBox.Value;
            acc_max = double.Parse(AccuracyBox.Text);

            ZeidelMethod();

            IterLabel.Text = counter.ToString();
            AccMaxLabel.Text = accuracy.ToString();

            double[,] vStep = new double[N + 1, M + 1];

            Table.Rows.Clear();
            Table.Columns.Clear();

            for (int i = 0; i < N + 1; ++i)
            {
                if (Table.Columns.Count <= i)
                {
                    Table.Columns.Add("", "");
                }

                for (int j = 0; j < M + 1; ++j)
                {
                    if (Table.Rows.Count <= j)
                    {
                        Table.Rows.Add();
                    }

                    vStep[i, j] = v[i, j];
                }
            }

            for (int i = 0; i < N + 1; ++i)
            {
                for (int j = 0; j < M + 1; ++j)
                {
                    Table.Rows[j].Cells[i].Value = v[i, j];
                }
            }

            N *= 2;
            M *= 2;

            ZeidelMethod();

            double max_dif = 0;
            int max_i = 0;
            int max_j = 0;

            for (int i = 0; i < (N - 1) / 2 + 1; ++i)
            {
                for(int j = 0; j < (M - 1) / 2 + 1; ++j)
                {
                    double cur_dif = Math.Abs(vStep[i, j] - v[i * 2, j * 2]);
                    if(cur_dif > max_dif)
                    {
                        max_dif = cur_dif;
                        max_i = i;
                        max_j = j;
                    }
                }
            }

            MaxDifLabel.Text = max_dif.ToString();
            DotLabel.Text = "В точке i = " + max_i.ToString() + ", j = " + max_j.ToString();
        }


        private double mu1(double y)
        {
            //return 1.0 - y * y;
            return Math.Exp(1.0 - Xo * Xo - y * y);
        }

        private double mu2(double y)
        {
            //return (1.0 - y * y) * Math.Exp(y);
            return Math.Exp(1.0 - Xn * Xn - y * y);
        }

        private double mu3(double x)
        {
            //return 1.0 - x * x;
            return Math.Exp(1.0 - x * x - Yo * Yo);
        }

        private double mu4(double x)
        {
            //return 1.0 - x * x;
            return Math.Exp(1.0 - x * x - Yn * Yn);
        }

        private double X(uint i)
        {
            return Xo + i * h;
        }

        private double Y(uint j)
        {
            return Yo + j * k;
        }

        private double V(uint i, uint j)
        {
            if (0u == i)
            {
                return mu1(Y(j));
            }
            if (N == i)
            {
                return mu2(Y(j));
            }
            if (0u == j)
            {
                return mu3(X(i));
            }
            if (M == j)
            {
                return mu4(X(i));
            }

            return v[i, j];
        }

        private double Function(uint i, uint j)
        {
            return 14.0 / 9.0 * Math.Exp(7.0 / 9.0);
            //return (-2.0 * X(i) * Math.Exp(1.0 - X(i) * X(i) - Y(j) * Y(j))) + (-2.0 * Y(j) * Math.Exp(1.0 - X(i) * X(i) - Y(j) * Y(j)));
            //return Math.Abs(X(i) * X(i) - Y(j) * Y(j));
        }
    }
}
