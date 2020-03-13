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
            
            Nbox.Minimum = decimal.MinValue;
            Nbox.Maximum = decimal.MaxValue;
            Mbox.Minimum = decimal.MinValue;
            Mbox.Maximum = decimal.MaxValue;

            NmaxBox.Maximum = decimal.MaxValue;

            XoBox.Value = -1;
            XnBox.Value = +1;
            YoBox.Value = -1;
            YnBox.Value = +1;

            OmegaTextBox.Text = "1.0";
            AccuracyBox.Text  = "0.00001";
            NmaxBox.Value     = 100;
            Nbox.Value        = 3;
            Mbox.Value        = 3;

            ZeroApprocsimationCheckBox.Checked = true;
            Table.RowHeadersVisible            = false;
            Table.ColumnHeadersVisible         = false;
            TableMain.RowHeadersVisible        = false;
            TableMain.ColumnHeadersVisible     = false;
            TableExact.RowHeadersVisible       = false;
            TableExact.ColumnHeadersVisible    = false;
            TableDiffTest.RowHeadersVisible    = false;
            TableDiffTest.ColumnHeadersVisible = false;
            TableHalf.RowHeadersVisible        = false;
            TableHalf.ColumnHeadersVisible     = false;
            TableDiffMain.RowHeadersVisible    = false;
            TableDiffMain.ColumnHeadersVisible = false;

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

            if(ZeroApprocsimationCheckBox.Checked)
            {
                ZeroApprocsimation();
            }
            else if(XInterpolationCheckBox.Checked)
            {
                XInterpolation();
            }
            else
            {
                YInterpolation();
            }

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

            if (ZeroApprocsimationCheckBox.Checked)
            {
                ZeroApprocsimationMain();
            }
            else if (XInterpolationCheckBox.Checked)
            {
                XInterpolationMain();
            }
            else
            {
                YInterpolationMain();
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
            w = double.Parse(OmegaTextBox.Text);

            TopRelaxationMethod();

            double R = 0.0;

            for (uint j = 1u; j < M; ++j)
            {
                for (uint i = 1u; i < N; ++i)
                {
                    R += Math.Pow(a2 * v[i, j] + h2 * (v[i - 1, j] + v[i + 1, j]) + k2 * (v[i, j - 1] + v[i, j + 1]) + Function(i, j), 2.0);
                }
            }

            ResidualTextBox.Text = Math.Sqrt(R).ToString();

            double max_dif = 0.0;
            uint max_i = 0;
            uint max_j = 0;

            for (uint i = 0; i < N + 1; ++i)
            {
                for (uint j = 0; j < M + 1; ++j)
                {
                    double cur_dif = Math.Abs(FunctionExact(i, j) - v[i, j]);

                    if (cur_dif > max_dif)
                    {
                        max_dif = cur_dif;
                        max_i = i;
                        max_j = j;
                    }
                }
            }


            IterLabel.Text   = counter.ToString();
            AccMaxLabel.Text = accuracy.ToString();
            maxDif.Text      = max_dif.ToString();

            Table.Rows.Clear();
            Table.Columns.Clear();
            TableExact.Rows.Clear();
            TableExact.Columns.Clear();
            TableDiffTest.Rows.Clear();
            TableDiffTest.Columns.Clear();

            for (int i = 0; i < N + 1; ++i)
            {
                if (Table.Columns.Count <= i)
                {
                    Table.Columns.Add("", "");
                    TableExact.Columns.Add("", "");
                    TableDiffTest.Columns.Add("", "");
                }

                for (int j = 0; j < M + 1; ++j)
                {
                    if (Table.Rows.Count <= j)
                    {
                        Table.Rows.Add();
                        TableExact.Rows.Add();
                        TableDiffTest.Rows.Add();
                    }
                }
            }

            for (int i = 0; i < N + 1; ++i)
            {
                for (int j = 0; j < M + 1; ++j)
                {
                    double test_value  = v[i, j];
                    double exact_value = FunctionExact((uint)i, (uint)j);

                    Table.Rows[(int)M - j].Cells[i].Value         = Math.Round(test_value, 3);
                    TableExact.Rows[(int)M - j].Cells[i].Value    = Math.Round(exact_value, 3);
                    TableDiffTest.Rows[(int)M - j].Cells[i].Value = Math.Round(Math.Abs(test_value - exact_value), 3);
                }
            }
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
            w = double.Parse(OmegaTextBox.Text);

            TopRelaxationMethodMain();

            double R = 0.0;

            for (uint j = 1u; j < M; ++j)
            {
                for (uint i = 1u; i < N; ++i)
                {
                    R += Math.Pow(a2 * v[i, j] + h2 * (v[i - 1, j] + v[i + 1, j]) + k2 * (v[i, j - 1] + v[i, j + 1]) + FunctionMain(i, j), 2.0);
                }
            }

            ResidualMainTextBox.Text = Math.Sqrt(R).ToString();
            IterLabelMain.Text       = counter.ToString();
            AccMaxLabelMain.Text     = accuracy.ToString();

            double[,] vStep = (double[,])v.Clone();

            TableMain.Rows.Clear();
            TableMain.Columns.Clear();
            TableDiffMain.Rows.Clear();
            TableDiffMain.Columns.Clear();

            for (int i = 0; i < N + 1; ++i)
            {
                if (TableMain.Columns.Count <= i)
                {
                    TableMain.Columns.Add("", "");
                    TableDiffMain.Columns.Add("", "");
                }

                for (int j = 0; j < M + 1; ++j)
                {
                    if (TableMain.Rows.Count <= j)
                    {
                        TableMain.Rows.Add();
                        TableDiffMain.Rows.Add();
                    }
                }
            }

            for (int i = 0; i < N + 1; ++i)
            {
                for (int j = 0; j < M + 1; ++j)
                {
                    TableMain.Rows[(int)M - j].Cells[i].Value     = Math.Round(v[i, j], 3);
                    TableDiffMain.Rows[(int)M - j].Cells[i].Value = 0.0;
                }
            }

            // Using half step

            N *= 2;
            M *= 2;

            TopRelaxationMethodMain();

            R = 0.0;

            for (uint j = 1u; j < M; ++j)
            {
                for (uint i = 1u; i < N; ++i)
                {
                    R += Math.Pow(a2 * v[i, j] + h2 * (v[i - 1, j] + v[i + 1, j]) + k2 * (v[i, j - 1] + v[i, j + 1]) + FunctionMain(i, j), 2.0);
                }
            }

            ResidualHalfTextBox.Text = Math.Sqrt(R).ToString();

            TableHalf.Rows.Clear();
            TableHalf.Columns.Clear();

            for (int i = 0; i < N + 1; ++i)
            {
                if (TableHalf.Columns.Count <= i)
                {
                    TableHalf.Columns.Add("", "");
                }

                for (int j = 0; j < M + 1; ++j)
                {
                    if (TableHalf.Rows.Count <= j)
                    {
                        TableHalf.Rows.Add();
                    }
                }
            }

            for (int i = 0; i < N + 1; ++i)
            {
                for (int j = 0; j < M + 1; ++j)
                {
                    TableHalf.Rows[(int)M - j].Cells[i].Value = Math.Round(v[i, j], 3);
                }
            }

            IterLabelMainHalf.Text   = counter.ToString();
            AccMaxLabelMainHalf.Text = accuracy.ToString();

            double max_dif = 0.0;
            int max_i = 0;
            int max_j = 0;

            for (int iStep = 1, iHalf = 2; (iStep < N / 2) && (iHalf < N); ++iStep, iHalf += 2)
            {
                for (int jStep = 1, jHalf = 2; (jStep < M / 2) && (jHalf < M); ++jStep, jHalf += 2)
                {
                    double cur_dif = Math.Abs(vStep[iStep, jStep] - v[iHalf, jHalf]);

                    if (cur_dif > max_dif)
                    {
                        max_dif = cur_dif;
                        max_i = iHalf;
                        max_j = jHalf;
                    }

                    TableDiffMain.Rows[(int)M / 2 - jStep].Cells[iStep].Value = cur_dif;
                }
            }

            MaxDifLabel.Text = Math.Round(max_dif, 7).ToString();
            DotLabel.Text    = "Соответствует узлу x = " + Math.Round(Math.Abs(X((uint)max_i)), 3).ToString() + 
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
            return (4.0 *  Math.Pow(X(i), 2.0) * Math.Exp(-Math.Pow(X(i), 2.0) -
                           Math.Pow(Y(j), 2.0) + 1.0)) - 2.0 * Math.Exp(-Math.Pow(X(i), 2.0) - Math.Pow(Y(j), 2.0) + 1.0) +
                   ((4.0 * Math.Pow(Y(j), 2.0) * Math.Exp(-Math.Pow(X(i), 2.0) -
                           Math.Pow(Y(j), 2.0) + 1.0)) - 2.0 * Math.Exp(-Math.Pow(X(i), 2.0) - Math.Pow(Y(j), 2.0) + 1.0));
        }

        private double FunctionExact(uint i, uint j)
        {
            return Math.Exp(1.0 - Math.Pow(X(i), 2.0) - Math.Pow(Y(j), 2.0));
        }

        private double FunctionMain(uint i, uint j)
        {
            return -Math.Abs(Math.Pow(X(i), 2.0) - Math.Pow(Y(j), 2.0));
        }


        private void ZeroApprocsimation()
        {
            for(uint i = 0u; i < N + 1; ++i)
            {
                for(uint j = 0u; j < M + 1; ++j)
                {
                    v[i, j] = 0.0;
                }
            }

            TestTypeTextBox.Text = "Использовалось нулевое приближение";
        }


        private void ZeroApprocsimationMain()
        {
            for (uint i = 0u; i < N + 1; ++i)
            {
                for (uint j = 0u; j < M + 1; ++j)
                {
                    v[i, j] = 0.0;
                }
            }

            MainTypeTextBox.Text = "Использовалось нулевое приближение";
        }


        private void XInterpolation()
        {
            for (uint i = 1u; i < N; ++i)
            {
                for (uint j = 1u; j < M; ++j)
                {
                    v[i, j] = ((Xo + i * h) - Xo) / (Xn - Xo) * mu2(Yo + j * k) +
                              ((Xo + i * h) - Xn) / (Xo - Xn) * mu1(Yo + j * k);
                }
            }

            TestTypeTextBox.Text = "Использовалась интерполяция по x";            
        }


        private void XInterpolationMain()
        {
            for (uint i = 1u; i < N; ++i)
            {
                for (uint j = 1u; j < M; ++j)
                {
                    v[i, j] = ((Xo + i * h) - Xo) / (Xn - Xo) * mu2Main(Yo + j * k) +
                              ((Xo + i * h) - Xn) / (Xo - Xn) * mu1Main(Yo + j * k);
                }
            }

            MainTypeTextBox.Text = "Использовалась интерполяция по x";
        }


        private void YInterpolation()
        {
            for (uint i = 1u; i < N; ++i)
            {
                for (uint j = 1u; j < M; ++j)
                {
                    v[i, j] = ((Yo + j * k) - Yo) / (Yn - Yo) * mu4(Xo + i * h) +
                              ((Yo + j * k) - Yn) / (Yo - Yn) * mu3(Xo + i * h);
                }
            }

            TestTypeTextBox.Text = "Использовалась интерполяция по y";
        }


        private void YInterpolationMain()
        {
            for (uint i = 1u; i < N; ++i)
            {
                for (uint j = 1u; j < M; ++j)
                {
                    v[i, j] = ((Yo + j * k) - Yo) / (Yn - Yo) * mu4Main(Xo + i * h) +
                              ((Yo + j * k) - Yn) / (Yo - Yn) * mu3Main(Xo + i * h);
                }
            }

            MainTypeTextBox.Text = "Использовалась интерполяция по y";
        }


        private void ZeroApprocsimationCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;

            if(checkBox.Checked)
            {
                XInterpolationCheckBox.Checked = false;
                YInterpolationCheckBox.Checked = false;
            }
            else if(!XInterpolationCheckBox.Checked && !YInterpolationCheckBox.Checked)
            {
                ZeroApprocsimationCheckBox.Checked = true;
            }
        }

        private void XInterpolationCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;

            if (checkBox.Checked)
            {
                ZeroApprocsimationCheckBox.Checked = false;
                YInterpolationCheckBox.Checked     = false;
            }
            else if (!ZeroApprocsimationCheckBox.Checked && !YInterpolationCheckBox.Checked)
            {
                XInterpolationCheckBox.Checked = true;
            }
        }

        private void YInterpolationCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;

            if (checkBox.Checked)
            {
                ZeroApprocsimationCheckBox.Checked = false;
                XInterpolationCheckBox.Checked = false;
            }
            else if (!ZeroApprocsimationCheckBox.Checked && !XInterpolationCheckBox.Checked)
            {
                YInterpolationCheckBox.Checked = true;
            }
        }
    }
}
