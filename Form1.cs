using System;
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
        delegate void SetChangeButtonVisibilityCallBack(Button button, bool status);
        delegate void SetChangeTextBoxValueCallBack(TextBox textBox, string value);
        delegate void SetChangeLabelValueCallBack(Label label, string value);
        delegate void SetChangeTableValuesCallBack(DataGridView table, double[,] values);

        private double Xo;
        private double Xn;
        private double Yo;
        private double Yn;
        private double acc_max;
        private bool use_optimal_parameter;
        private ApproximationType approximationType;

        private uint Nmax;
        private uint N;
        private uint M;


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
            AccuracyBox.Text = "0.00001";
            NmaxBox.Value = 100;
            Nbox.Value = 3;
            Mbox.Value = 3;

            ZeroApprocsimationCheckBox.Checked = true;
            Table.RowHeadersVisible = false;
            TableMain.RowHeadersVisible = false;
            TableExact.RowHeadersVisible = false;
            TableDiffTest.RowHeadersVisible = false;
            TableHalf.RowHeadersVisible = false;
            TableDiffMain.RowHeadersVisible = false;
            OptimalOmegaCheckBox.Checked = true;

            label10.Text = "При решении основной задачи \n" +
                           "        с половинным шагом";

            MethodComboBox.Items.Add("Метод верхней релаксации");
            MethodComboBox.Items.Add("Метод простых итераций");
            MethodComboBox.Items.Add("Метод с параметрами Чебышева");
            MethodComboBox.Items.Add("Метод минимальных невязок");

            MethodComboBox.SelectedIndex = 0;
        }


        private void RunTestTask<Method>() where Method : RectangularMethodBase, new()
        {
            ChangeButtonVisibility(button1, false);

            double maxDif = 0.0;
            double maxAcc = acc_max;
            uint iterCount = Nmax;
            uint maxI = 0u;
            uint maxJ = 0u;

            Method testTask = new Method();

            testTask.Init(Xo, Xn, Yo, Yn, N, M, approximationType);
            testTask.SetFunctions(
                Functions.mu1Test,
                Functions.mu2Test,
                Functions.mu3Test,
                Functions.mu4Test,
                Functions.FunctionTest,
                Functions.ExactFunction);

            if (!use_optimal_parameter)
            {
                testTask.SetSpecialParameter(double.Parse(OmegaTextBox.Text));
            }

            testTask.Run(ref iterCount, ref maxAcc);
            FindMax(
                CalculateDifferenceTableForTestTask(
                    testTask.GetData(), testTask.GetExactTable(), testTask.GetN(), testTask.GetM()),
                out maxDif, out maxI, out maxJ);

            ChangeTextBoxValue(OmegaTextBox, testTask.GetSpecialParameter().ToString());
            ChangeLabelValue(ResidualTextBox, testTask.CalculateResidual().ToString());
            ChangeLabelValue(IterLabel, iterCount.ToString());
            ChangeLabelValue(AccMaxLabel, maxAcc.ToString());
            ChangeLabelValue(maxDifLabel, maxDif.ToString());
            ChangeLabelValue(DotLabelTest, "Соответствует узлу x = " + Math.Abs(Math.Round(testTask.X(maxI), 3)).ToString() +
                                                               "  y = " + Math.Abs(Math.Round(testTask.Y(maxJ), 3)).ToString());

            TableCreator.Init(N + 1u, M + 1u);

            ChangeTableValues(Table, testTask.GetData());
            ChangeTableValues(TableExact, testTask.GetExactTable());
            ChangeTableValues(TableDiffTest,
                CalculateDifferenceTableForTestTask(testTask.GetData(), testTask.GetExactTable(), N, M));

            ChangeButtonVisibility(button1, true);
        }


        private void RunTestTaskCustom<Method>() where Method : CustomMethodBase, new()
        {
            ChangeButtonVisibility(button3, false);

            double maxDif = 0.0;
            double maxAcc = acc_max;
            uint iterCount = Nmax;
            uint maxI = 0u;
            uint maxJ = 0u;

            Method testTask = new Method();

            testTask.Init(Xo, Xn, Yo, Yn, N, M, approximationType);
            testTask.SetFunctions(
                Functions.mu1Test,
                Functions.mu2Test,
                Functions.mu3Test,
                Functions.mu4Test,
                Functions.mu5Test,
                Functions.mu6Test,
                Functions.FunctionTest,
                Functions.ExactFunction);

            if (!use_optimal_parameter)
            {
                testTask.SetSpecialParameter(double.Parse(OmegaTextBox.Text));
            }

            testTask.Run(ref iterCount, ref maxAcc);
            FindMax(
                CalculateDifferenceTableForTestTask(
                    testTask.GetData(), testTask.GetExactTable(), testTask.GetN(), testTask.GetM()),
                out maxDif, out maxI, out maxJ);

            ChangeTextBoxValue(OmegaTextBox, testTask.GetSpecialParameter().ToString());
            ChangeLabelValue(ResidualTextBox, testTask.CalculateResidual().ToString());
            ChangeLabelValue(IterLabel, iterCount.ToString());
            ChangeLabelValue(AccMaxLabel, maxAcc.ToString());
            ChangeLabelValue(maxDifLabel, maxDif.ToString());
            ChangeLabelValue(DotLabelTest, "Соответствует узлу x = " + Math.Abs(Math.Round(testTask.X(maxI), 3)).ToString() +
                                                               "  y = " + Math.Abs(Math.Round(testTask.Y(maxJ), 3)).ToString());

            TableCreator.Init(N + 1u, M + 1u);

            ChangeTableValues(Table, testTask.GetData());
            ChangeTableValues(TableExact, testTask.GetExactTable());
            ChangeTableValues(TableDiffTest,
                CalculateDifferenceTableForTestTask(testTask.GetData(), testTask.GetExactTable(), N, M));

            ChangeButtonVisibility(button3, true);
        }


        private void RunMainTask<Method>() where Method : RectangularMethodBase, new()
        {
            ChangeButtonVisibility(button2, false);

            double maxDif = 0.0;
            double maxAccMain = 0.0;
            double maxAccHalf = 0.0;
            uint maxIterMain = 0u;
            uint maxIterHalf = 0u;
            uint maxI = 0u;
            uint maxJ = 0u;


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

            TableCreator.Init(N * 2 + 1u, M * 2 + 1u);
            ChangeTableValues(TableHalf, halfTask.GetData());

            TableCreator.Init(N + 1u, M + 1u);
            ChangeTableValues(TableMain, mainTask.GetData());
            ChangeTableValues(TableDiffMain,
                CalculateDifferenceTableForMainTask(mainTask.GetData(), halfTask.GetData(), N, M));

            ChangeButtonVisibility(button2, true);
        }


        private void FindMax(
            double[,] a, out double maxDif, out uint maxX, out uint maxY)
        {
            maxDif = 0.0;
            maxX = 0u;
            maxY = 0u;

            for (uint i = 1u; i < N; ++i)
            {
                for (uint j = 1u; j < M; ++j)
                {
                    if (a[i, j] > maxDif)
                    {
                        maxDif = a[i, j];
                        maxX = i;
                        maxY = j;
                    }
                }
            }
        }


        private double[,] CalculateDifferenceTableForTestTask(
            double[,] solutionTable, double[,] exactTable, uint _N, uint _M)
        {
            double[,] difference = new double[_N + 1u, _M + 1u];

            for (uint i = 0u; i < N + 1u; ++i)
            {
                for (uint j = 0u; j < M + 1u; ++j)
                {
                    difference[i, j] = Math.Abs(exactTable[i, j] - solutionTable[i, j]);
                }
            }

            return difference;
        }


        private double[,] CalculateDifferenceTableForMainTask(
            double[,] wholeStep, double[,] halfStep, uint _N, uint _M)
        {
            uint width = _M * 2u;
            uint height = _N * 2u;
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
            where Method : RectangularMethodBase, new()
        {
            maxAcc = acc_max;
            iterCount = Nmax;

            task = new Method();
            task.Init(Xo, Xn, Yo, Yn, _N, _M, approximationType);
            task.SetFunctions(
                Functions.mu1Main,
                Functions.mu2Main,
                Functions.mu3Main,
                Functions.mu4Main,
                Functions.FunctionMain);

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
                case 3:
                    testTaskThread = new Thread(new ThreadStart(RunTestTask<MinimalDiscrepancyMethod>));
                    testTaskThread.Start();
                    break;
            }

            // F*** C#
            GC.Collect();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (!ParseArguments())
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
                case 3:
                    mainTaskThread = new Thread(new ThreadStart(RunMainTask<MinimalDiscrepancyMethod>));
                    mainTaskThread.Start();
                    break;
            }

            // F*** C#
            GC.Collect();
        }


        private void button3_Click(object sender, EventArgs e)
        {
            if (!ParseArguments())
            {
                MessageBox.Show("Ошибка ввода!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Thread testTaskThread;

            switch (MethodComboBox.SelectedIndex)
            {
                case 3:
                    testTaskThread = new Thread(new ThreadStart(RunTestTaskCustom<MinimalDiscrepancyMethodCustom>));
                    testTaskThread.Start();
                    break;
                default:
                    MessageBox.Show("Метод не поддерживается!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }

            // F*** C#
            GC.Collect();
        }


        private bool ParseArguments()
        {
            Xo = (double)XoBox.Value;
            Xn = (double)XnBox.Value;
            Yo = (double)YoBox.Value;
            Yn = (double)YnBox.Value;
            N = (uint)Nbox.Value;
            M = (uint)Mbox.Value;
            Nmax = (uint)NmaxBox.Value;

            Functions.Set(Xo, Yo, Xn, Yn);

            return double.TryParse(AccuracyBox.Text, out acc_max);
        }


        private void ZeroApprocsimationCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;

            if (checkBox.Checked)
            {
                XInterpolationCheckBox.Checked = false;
                YInterpolationCheckBox.Checked = false;
                approximationType = ApproximationType.ZERO_APPROXIMATION;
            }
            else if (!XInterpolationCheckBox.Checked && !YInterpolationCheckBox.Checked)
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
                YInterpolationCheckBox.Checked = false;
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

            if (checkBox.Checked)
            {
                OmegaTextBox.Enabled = false;
                use_optimal_parameter = true;
            }
            else
            {
                OmegaTextBox.Enabled = true;
                use_optimal_parameter = false;
            }
        }


        private void ChangeButtonVisibility(Button button, bool status)
        {
            if (button.InvokeRequired)
            {
                this.Invoke(new SetChangeButtonVisibilityCallBack(ChangeButtonVisibility),
                    new object[] { button, status });
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
                TableCreator.Fill<double>(table, values);
            }
        }
    }
}
