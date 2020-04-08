﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NumericalMethods
{
    public partial class Form1 : Form
    {
        delegate void SetChangeButtonVisibilityCallBack(Button       button,  bool      status);
        delegate void SetChangeTextBoxValueCallBack    (TextBox      textBox, string    value);
        delegate void SetChangeLabelValueCallBack      (Label        label,   string    value);
        delegate void SetChangeTableValuesCallBack     (DataGridView table,   double[,] values);

        private double Xo;
        private double Xn;
        private double Yo;
        private double Yn;
        private double acc_max;
        private bool   use_optimal_parameter;
        private ApproximationType approximationType;
        private TableCreator      tableCreator;

        private uint Nmax;
        private uint N;
        private uint M;

        private double mu1Test(double y) => Math.Exp(1.0 - Math.Pow(Xo, 2) - Math.Pow(y, 2));
        private double mu2Test(double y) => Math.Exp(1.0 - Math.Pow(Xn, 2) - Math.Pow(y, 2));
        private double mu3Test(double x) => Math.Exp(1.0 - Math.Pow(x, 2)  - Math.Pow(Yo, 2));
        private double mu4Test(double x) => Math.Exp(1.0 - Math.Pow(x, 2)  - Math.Pow(Yn, 2));
        private double FunctionTest(double x, double y)  =>
            Math.Exp(-(x * x) - (y * y) + 1.0) * (4.0 * x * x - 2.0 + 4.0 * y * y - 2.0);
        private double ExactFunction(double x, double y) => 
            Math.Exp(1.0 - (x * x) - (y * y));

        private double mu1Main(double y) =>  1.0 - Math.Pow(y, 2.0);
        private double mu2Main(double y) => (1.0 - Math.Pow(y, 2.0)) * Math.Exp(y);
        private double mu3Main(double x) =>  1.0 - Math.Pow(x, 2.0);
        private double mu4Main(double x) =>  1.0 - Math.Pow(x, 2.0);
        private double FunctionMain(double x, double y) => -Math.Abs(x * x - y * y);


        public Form1()
        {
            InitializeComponent();

            XoBox.Minimum = decimal.MinValue;
            XoBox.Maximum = decimal.MaxValue;
            XnBox.Minimum = decimal.MinValue;
            XnBox.Maximum = decimal.MaxValue;

            YoBox.Minimum = decimal.MinValue;
            YoBox.Maximum = decimal.MaxValue;
            YnBox.Minimum = decimal.MinValue;
            YnBox.Maximum = decimal.MaxValue;
            
            Nbox.Minimum  = decimal.MinValue;
            Nbox.Maximum  = decimal.MaxValue;
            Mbox.Minimum  = decimal.MinValue;
            Mbox.Maximum  = decimal.MaxValue;

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
            TableMain.RowHeadersVisible        = false;
            TableExact.RowHeadersVisible       = false;
            TableDiffTest.RowHeadersVisible    = false;
            TableHalf.RowHeadersVisible        = false;
            TableDiffMain.RowHeadersVisible    = false;
            OptimalOmegaCheckBox.Checked       = true;

            label10.Text = "При решении основной задачи \n" +
                           "        с половинным шагом";

            MethodComboBox.Items.Add("Метод верхней релаксации");
            MethodComboBox.Items.Add("Метод простых итераций");
            MethodComboBox.Items.Add("Метод с параметрами Чебышева");

            MethodComboBox.SelectedIndex = 0;

            tableCreator = new TableCreator();
        }


        private void RunTestTask<Method>() where Method : MethodBase, new()
        {
            ChangeButtonVisibility(button1, false);

            double maxDif  = 0.0;
            double maxAcc  = acc_max;
            uint iterCount = Nmax;
            uint maxI      = 0u;
            uint maxJ      = 0u;

            Method testTask = new Method();

            testTask.Init(Xo, Xn, Yo, Yn, N, M, approximationType);
            testTask.SetFunctions(mu1Test, mu2Test, mu3Test, mu4Test, FunctionTest, ExactFunction);

            if (!use_optimal_parameter)
            {
                testTask.SetSpecialParameter(double.Parse(OmegaTextBox.Text));
            }

            testTask.Run(ref iterCount, ref maxAcc);
            FindMax(
                CalculateDifferenceTableForTestTask(
                    testTask.GetData(), testTask.GetExactTable(), testTask.GetN(), testTask.GetM()), 
                out maxDif, out maxI, out maxJ);

            ChangeTextBoxValue(OmegaTextBox,  testTask.GetSpecialParameter().ToString());
            ChangeLabelValue(ResidualTextBox, testTask.CalculateResidual().ToString());
            ChangeLabelValue(IterLabel,       iterCount.ToString());
            ChangeLabelValue(AccMaxLabel,     maxAcc.ToString());
            ChangeLabelValue(maxDifLabel,     maxDif.ToString());
            ChangeLabelValue(DotLabelTest,    "Соответствует узлу x = " + Math.Abs(Math.Round(testTask.X(maxI), 3)).ToString() +
                                                               "  y = " + Math.Abs(Math.Round(testTask.Y(maxJ), 3)).ToString());

            tableCreator.Init(N + 1u, M + 1u);

            ChangeTableValues(Table,         testTask.GetData());
            ChangeTableValues(TableExact,    testTask.GetExactTable());
            ChangeTableValues(TableDiffTest, 
                CalculateDifferenceTableForTestTask(testTask.GetData(), testTask.GetExactTable(), N, M));

            ChangeButtonVisibility(button1, true);
        }


        private void RunMainTask<Method>() where Method : MethodBase, new()
        {
            ChangeButtonVisibility(button2, false);

            double maxDif     = 0.0;
            double maxAccMain = 0.0;
            double maxAccHalf = 0.0;
            uint maxIterMain  = 0u;
            uint maxIterHalf  = 0u;
            uint maxI         = 0u;
            uint maxJ         = 0u;


            Method mainTask = null;
            Method halfTask = null;

            Thread stepTaskThread = new Thread(() =>
                RunMethod<Method>(out mainTask, out maxAccMain, out maxIterMain, N, M));

            Thread halfTaskThread = new Thread(() =>
                RunMethod<Method>(out halfTask, out maxAccHalf, out maxIterHalf, N * 2u, M * 2u));

            stepTaskThread.Start();
            halfTaskThread.Start();
            stepTaskThread.Join();
            halfTaskThread.Join();


            FindMax(CalculateDifferenceTableForMainTask(mainTask.GetData(), halfTask.GetData(), N, M), out maxDif, out maxI, out maxJ);

            ChangeTextBoxValue(OmegaTextBox, mainTask.GetSpecialParameter().ToString());
            ChangeLabelValue(MaxDifLabelMain, maxDif.ToString());
            ChangeLabelValue(ResidualMainTextBox, mainTask.CalculateResidual().ToString());
            ChangeLabelValue(IterLabelMain, maxIterMain.ToString());
            ChangeLabelValue(AccMaxLabelMain, maxAccMain.ToString());
            ChangeLabelValue(ResidualHalfTextBox, halfTask.CalculateResidual().ToString());
            ChangeLabelValue(IterLabelMainHalf, maxIterHalf.ToString());
            ChangeLabelValue(AccMaxLabelMainHalf, maxAccHalf.ToString());
            ChangeLabelValue(DotLabel, "Соответствует узлу x = " + Math.Abs(Math.Round(mainTask.X(maxI), 3)).ToString() + 
                                                        "  y = " + Math.Abs(Math.Round(mainTask.Y(maxJ), 3)).ToString());

            tableCreator.Init(N * 2 + 1u, M * 2 + 1u);
            ChangeTableValues(TableHalf, halfTask.GetData());

            tableCreator.Init(N + 1u, M + 1u);
            ChangeTableValues(TableMain, mainTask.GetData());
            ChangeTableValues(TableDiffMain, 
                CalculateDifferenceTableForMainTask(mainTask.GetData(), halfTask.GetData(), N, M));

            ChangeButtonVisibility(button2, true);
        }


        private void FindMax(
            double[,] a, out double maxDif, out uint maxX, out uint maxY)
        {
            maxDif = 0.0;
            maxX   = 0u;
            maxY   = 0u;

            for (uint i = 1u; i < N; ++i)
            {
                for (uint j = 1u; j < M; ++j)
                {
                    if (a[i, j] > maxDif)
                    {
                        maxDif = a[i, j];
                        maxX   = i;
                        maxY   = j;
                    }
                }
            }
        }


        private double[,] CalculateDifferenceTableForTestTask(
            double[,] solutionTable, double[,] exactTable, uint _N, uint _M)
        {
            double[,] difference = new double[_N + 1u, _M + 1u];

            for (uint i = 0; i < N + 1; ++i)
            {
                for (uint j = 0; j < M + 1; ++j)
                {
                    difference[i, j] = Math.Abs(exactTable[i, j] - solutionTable[i, j]);
                }
            }

            return difference;
        }


        private double[,] CalculateDifferenceTableForMainTask(
            double[,] wholeStep, double[,] halfStep, uint _N, uint _M)
        {
            uint width  = _M * 2;
            uint height = _N * 2;
            double[,] difference = new double[N + 1u, M + 1u];

            for (int iStep = 1, iHalf = 2; (iStep < height / 2) && (iHalf < height); ++iStep, iHalf += 2)
            {
                for (int jStep = 1, jHalf = 2; (jStep < width / 2) && (jHalf < width); ++jStep, jHalf += 2)
                {
                    difference[iStep, jStep] =
                        Math.Abs(wholeStep[iStep, jStep] - halfStep[iHalf, jHalf]);
                }
            }

            return difference;
        }


        private void RunMethod<Method>(
            out Method task, out double maxAcc, out uint iterCount, uint _N, uint _M) 
            where Method : MethodBase, new()
        {
            maxAcc    = acc_max;
            iterCount = Nmax;

            task = new Method();
            task.Init(Xo, Xn, Yo, Yn, _N, _M, approximationType);
            task.SetFunctions(mu1Main, mu2Main, mu3Main, mu4Main, FunctionMain);

            if (!use_optimal_parameter)
            {
                task.SetSpecialParameter(double.Parse(OmegaTextBox.Text));
            }

            task.Run(ref iterCount, ref maxAcc);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (!ParseArguments())
            {
                MessageBox.Show("Ошибка ввода!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Thread testTaskThread;

            switch (MethodComboBox.SelectedIndex)
            {
                case 0:
                    testTaskThread = new Thread(new ThreadStart(RunTestTask<TopRelaxationMethod>));
                    testTaskThread.Start();
                    break;

                case 1:
                    testTaskThread = new Thread(new ThreadStart(RunTestTask<SimpleIterationMethod>));
                    testTaskThread.Start();
                    break;
                case 2:
                    testTaskThread = new Thread(new ThreadStart(RunTestTask<ChebyshevSimpleIterationMethod>));
                    testTaskThread.Start();
                    break;
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if(!ParseArguments())
            {
                MessageBox.Show("Ошибка ввода!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Thread mainTaskThread;

            switch (MethodComboBox.SelectedIndex)
            {
                case 0:
                    mainTaskThread = new Thread(new ThreadStart(RunMainTask<TopRelaxationMethod>));
                    mainTaskThread.Start();
                    break;

                case 1:
                    mainTaskThread = new Thread(new ThreadStart(RunMainTask<SimpleIterationMethod>));
                    mainTaskThread.Start();
                    break;
                case 2:
                    mainTaskThread = new Thread(new ThreadStart(RunMainTask<ChebyshevSimpleIterationMethod>));
                    mainTaskThread.Start();
                    break;
            }
        }


        private bool ParseArguments()
        {
            Xo   = (double)XoBox.Value;
            Xn   = (double)XnBox.Value;
            Yo   = (double)YoBox.Value;
            Yn   = (double)YnBox.Value;
            N    = (uint)Nbox.Value;
            M    = (uint)Mbox.Value;
            Nmax = (uint)NmaxBox.Value;

            return double.TryParse(AccuracyBox.Text, out acc_max);
        }


        private void ZeroApprocsimationCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;

            if(checkBox.Checked)
            {
                XInterpolationCheckBox.Checked = false;
                YInterpolationCheckBox.Checked = false;
                approximationType = ApproximationType.ZERO_APPROXIMATION;
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
                approximationType = ApproximationType.X_INTERPOLATION;
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
                approximationType = ApproximationType.Y_INTERPOLATION;
            }
            else if (!ZeroApprocsimationCheckBox.Checked && !XInterpolationCheckBox.Checked)
            {
                YInterpolationCheckBox.Checked = true;
            }
        }


        private void OptimalOmegaCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;

            if(checkBox.Checked)
            {
                OmegaTextBox.Enabled = false;
                use_optimal_parameter    = true;
            }
            else
            {
                OmegaTextBox.Enabled = true;
                use_optimal_parameter    = false;
            }
        }


        private void ChangeButtonVisibility(Button button, bool status)
        {
            if (button.InvokeRequired)
            {
                this.Invoke(new SetChangeButtonVisibilityCallBack(ChangeButtonVisibility), 
                    new object[] { button, status } );
            }
            else
            {
                button.Visible = status;
            }
        }


        private void ChangeTextBoxValue(TextBox textBox, string value)
        {
            if (textBox.InvokeRequired)
            {
                this.Invoke(new SetChangeTextBoxValueCallBack(ChangeTextBoxValue),
                    new object[] { textBox, value });
            }
            else
            {
                textBox.Text = value;
            }
        }


        private void ChangeLabelValue(Label label, string value)
        {
            if (label.InvokeRequired)
            {
                this.Invoke(new SetChangeLabelValueCallBack(ChangeLabelValue),
                    new object[] { label, value });
            }
            else
            {
                label.Text = value;
            }
        }


        private void ChangeTableValues(DataGridView table, double[,] values)
        {
            if (table.InvokeRequired)
            {
                this.Invoke(new SetChangeTableValuesCallBack(ChangeTableValues),
                    new object[] { table, values });
            }
            else
            {
                tableCreator.Fill<double>(table, values);
            }
        }
    }
}
