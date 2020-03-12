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
        private double Xo = -1;
        private double Xn = 1;
        private double Yo = -1;
        private double Yn = 1;
        private double w = 1.0;
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

            NmaxBox.Maximum = decimal.MaxValue;

            XoBox.Value = -1;
            XnBox.Value = +1;
            YoBox.Value = -1;
            YnBox.Value = +1;

            AccuracyBox.Text = "0.00001";
            NmaxBox.Value = 100;
            Nbox.Value = 3;
            Mbox.Value = 3;

            Table.RowHeadersVisible        = false;
            Table.ColumnHeadersVisible     = false;
            TableMain.RowHeadersVisible    = false;
            TableMain.ColumnHeadersVisible = false;


            label10.Text = "При решении основной задачи \n" +
                           "        с половинным шагом";
        }


        private void TopRelaxationMethod()
        {
            v = new double[N + 1, M + 1];

            h = (Xn - Xo) / N;
            h2 = -Math.Pow(N / (Xn - Xo), 2);
            k = (Yn - Yo) / M;
            k2 = -Math.Pow(M / (Yn - Yo), 2);
            a2 = -2.0 * (h2 + k2);

            for (uint i = 0u; i < N + 1; ++i)
            {
                for (uint j = 0u; j < M + 1; ++j)
                {
                    v[i, j] = V(i, j);
                }
            }

            double current_accuracy;
            double new_v;
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

                        new_v  = 0.0;
                        new_v += -w * (h2 * (V(i + 1, j) + V(i - 1, j)));
                        new_v += -w * (k2 * (V(i, j + 1) + V(i, j - 1)));
                        new_v += (1.0 - w) * a2 * V(i, j) + w * -Function(i, j);
                        new_v /= a2;

                        current_accuracy = Math.Abs(current - new_v);

                        if (accuracy < current_accuracy)
                        {
                            accuracy = current_accuracy;
                        }

                        v[i, j] = new_v;
                    }
                }

                counter++;

            } while ((Nmax > counter) && (accuracy >= acc_max));
        }


        private void TopRelaxationMethodMain()
        {
            v = new double[N + 1, M + 1];

            h = (Xn - Xo) / N;
            h2 = -Math.Pow(N / (Xn - Xo), 2);
            k = (Yn - Yo) / M;
            k2 = -Math.Pow(M / (Yn - Yo), 2);
            a2 = -2.0 * (h2 + k2);

            for(uint i = 0u; i < N + 1; ++i)
            {
                for(uint j = 0u; j < M + 1; ++j)
                {
                    v[i, j] = VMain(i, j);
                }
            }

            double current_accuracy;
            double new_v;
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

                        new_v = 0.0;
                        new_v += -w * (h2 * (VMain(i + 1, j) + VMain(i - 1, j)));
                        new_v += -w * (k2 * (VMain(i, j + 1) + VMain(i, j - 1)));
                        new_v += (1.0 - w) * a2 * VMain(i, j) + w * -FunctionMain(i, j);
                        new_v /= a2;

                        current_accuracy = Math.Abs(current - new_v);

                        if (accuracy < current_accuracy)
                        {
                            accuracy = current_accuracy;
                        }

                        v[i, j] = new_v;
                    }
                }

                counter++;

            } while ((Nmax > counter) && (accuracy >= acc_max));
        }

        // Test task
        private void button1_Click(object sender, EventArgs e)
        {
            Xo = (double)XoBox.Value;
            Xn = (double)XnBox.Value;
            Yo = (double)YoBox.Value;
            Yn = (double)YnBox.Value;
            N  = (uint)Nbox.Value;
            M  = (uint)Mbox.Value;
            Nmax = (uint)NmaxBox.Value;
            acc_max = double.Parse(AccuracyBox.Text);

            TopRelaxationMethod();

            IterLabel.Text       = counter.ToString();
            //AccMaxLabelMain.Text = accuracy.ToString();

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
                    Table.Rows[(int)M - j].Cells[i].Value = Math.Round(v[i, j], 3);
                }
            }


            // TODO: Add accuracy calculation

        }


        // Main task
        private void button2_Click(object sender, EventArgs e)
        {
            Xo = (double)XoBox.Value;
            Xn = (double)XnBox.Value;
            Yo = (double)YoBox.Value;
            Yn = (double)YnBox.Value;
            N  = (uint)Nbox.Value;
            M  = (uint)Mbox.Value;
            Nmax = (uint)NmaxBox.Value;
            acc_max = double.Parse(AccuracyBox.Text);

            TopRelaxationMethodMain();

            IterLabelMain.Text   = counter.ToString();
            AccMaxLabelMain.Text = accuracy.ToString();

            double[,] vStep = (double[,])v.Clone();

            TableMain.Rows.Clear();
            TableMain.Columns.Clear();

            for (int i = 0; i < N + 1; ++i)
            {
                if (TableMain.Columns.Count <= i)
                {
                    TableMain.Columns.Add("", "");
                }

                for (int j = 0; j < M + 1; ++j)
                {
                    if (TableMain.Rows.Count <= j)
                    {
                        TableMain.Rows.Add();
                    }
                }
            }

            for (int i = 0; i < N + 1; ++i)
            {
                for (int j = 0; j < M + 1; ++j)
                {
                    TableMain.Rows[(int)M - j].Cells[i].Value = Math.Round(v[i, j], 3);
                }
            }

            // Using half step

            N *= 2;
            M *= 2;

            TopRelaxationMethodMain();

            IterLabelMainHalf.Text   = counter.ToString();
            AccMaxLabelMainHalf.Text = accuracy.ToString();

            double max_dif = 0.0;
            int max_i = 0;
            int max_j = 0;

            for (int iStep = 1, iHalf = 2; (iStep < N / 2) && (iHalf < N); ++iStep, iHalf += 2)
            {
                for (int jStep = 1, jHalf = 2; (jStep < N / 2) && (jHalf < N); ++jStep, jHalf += 2)
                {
                    double cur_dif = Math.Abs(vStep[iStep, jStep] - v[iHalf, jHalf]);

                    Console.WriteLine(vStep[iStep, jStep]);
                    Console.WriteLine(v[iHalf, jHalf]);
                    if (cur_dif > max_dif)
                    {
                        max_dif = cur_dif;
                        max_i = iHalf;
                        max_j = jHalf;
                    }
                }
            }

            MaxDifLabel.Text = Math.Round(max_dif, 7).ToString();
            DotLabel.Text    = "Соответсвует узлу x = " + Math.Round(Math.Abs(X((uint)max_i)), 3).ToString() + 
                                               "  y = " + Math.Round(Math.Abs(Y((uint)max_j)), 3).ToString();
        }


        private double mu1(double y)
        {
            return Math.Exp(1.0 - Math.Pow(Xo, 2) - Math.Pow(y, 2));
        }

        private double mu1Main(double y)
        {
            return 1.0 - Math.Pow(y, 2.0);
        }

        private double mu2(double y)
        {
            return Math.Exp(1.0 - Math.Pow(Xn, 2) - Math.Pow(y, 2));
        }

        private double mu2Main(double y)
        {
            return (1.0 - Math.Pow(y, 2.0)) * Math.Exp(y);
        }

        private double mu3(double x)
        {
            return Math.Exp(1.0 - Math.Pow(x, 2) - Math.Pow(Yo, 2));
        }

        private double mu3Main(double x)
        {
            return 1.0 - Math.Pow(x, 2.0);
        }

        private double mu4(double x)
        {
            return Math.Exp(1.0 - Math.Pow(x, 2) - Math.Pow(Yn, 2));
        }

        private double mu4Main(double x)
        {
            return 1.0 - Math.Pow(x, 2.0);
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

        private double VMain(uint i, uint j)
        {
            if (0u == i)
            {
                return mu1Main(Y(j));
            }
            if (N == i)
            {
                return mu2Main(Y(j));
            }
            if (0u == j)
            {
                return mu3Main(X(i));
            }
            if (M == j)
            {
                return mu4Main(X(i));
            }

            return v[i, j];
        }

        private double Function(uint i, uint j)
        {
            return (4.0 * Math.Pow(X(i), 2.0) * Math.Exp(-Math.Pow(X(i), 2.0) -
                          Math.Pow(Y(j), 2.0) + 1.0)) - 2.0 * Math.Exp(-Math.Pow(X(i), 2.0) - Math.Pow(Y(j), 2.0) + 1.0) +
                   ((4.0 * Math.Pow(Y(j), 2.0) * Math.Exp(-Math.Pow(X(i), 2.0) -
                          Math.Pow(Y(j), 2.0) + 1.0)) - 2.0 * Math.Exp(-Math.Pow(X(i), 2.0) - Math.Pow(Y(j), 2.0) + 1.0));
        }

        private double FunctionMain(uint i, uint j)
        {
            return -Math.Abs(Math.Pow(X(i), 2.0) - Math.Pow(Y(j), 2.0));
        }
    }
}
