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

namespace Dirihle
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
        private bool   use_optimal_omega;
        private ApproximationType approximationType;
        private TableCreator      tableCreator;

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

            tableCreator = new TableCreator();
        }


        // Test task
        private void RunTestTask()
        {
            ChangeButtonVisibility(button1, false);

            double maxDif    = 0.0;
            double maxX      = 0.0;
            double maxY      = 0.0;
            double maxAcc    = acc_max;
            uint   iterCount = Nmax;

            TopRelaxationMethodTestTask testTask = new
                TopRelaxationMethodTestTask(Xo, Xn, Yo, Yn, N, M, approximationType);

            if (!use_optimal_omega)
            {
                testTask.Ω = double.Parse(OmegaTextBox.Text);
            }

            testTask.Run(ref iterCount, ref maxAcc);
            testTask.CalculateMaxDifference(out maxDif, out maxX, out maxY);

            ChangeTextBoxValue(OmegaTextBox,    testTask.Ω.ToString());
            ChangeLabelValue  (ResidualTextBox, testTask.CalculateR().ToString());
            ChangeLabelValue  (IterLabel,       iterCount.ToString());
            ChangeLabelValue  (AccMaxLabel,     maxAcc.ToString());
            ChangeLabelValue  (maxDifLabel,     maxDif.ToString());
            ChangeLabelValue  (DotLabelTest,    "Соответствует узлу x = " + Math.Round(Math.Abs(maxX), 3).ToString() +
                                                                 "  y = " + Math.Round(Math.Abs(maxY), 3).ToString());

            tableCreator.Init(N + 1u, M + 1u);

            ChangeTableValues(Table,         testTask.GetData());
            ChangeTableValues(TableExact,    testTask.GetExact());
            ChangeTableValues(TableDiffTest, testTask.GetDifference(testTask.GetExact()));

            ChangeButtonVisibility(button1, true);
        }


        private void RunTask(
            out TopRelaxationMethodMainTask task, 
            out double maxAcc, 
            out uint iterCount,
            uint _N,
            uint _M)
        {
            maxAcc    = acc_max;
            iterCount = Nmax;

            task = new
                TopRelaxationMethodMainTask(Xo, Xn, Yo, Yn, _N, _M, approximationType);

            if (!use_optimal_omega)
            {
                task.Ω = double.Parse(OmegaTextBox.Text);
            }

            task.Run(ref iterCount, ref maxAcc);
        }


        // For main task
        private void RunMainTask()
        {
            ChangeButtonVisibility(button2, false);

            double maxDif      = 0.0;
            double maxX        = 0.0;
            double maxY        = 0.0;
            double maxAccMain  = 0.0;
            double maxAccHalf  = 0.0;
            uint   maxIterMain = 0u;
            uint   maxIterHalf = 0u;

            TopRelaxationMethodMainTask mainTask = null;
            TopRelaxationMethodMainTask halfTask = null;

            Thread stepTaskThread = new Thread(() => 
                RunTask(out mainTask, out maxAccMain, out maxIterMain, N, M));

            Thread halfTaskThread = new Thread(() => 
                RunTask(out halfTask, out maxAccHalf, out maxIterHalf, N * 2u, M * 2u));

            stepTaskThread.Start();
            halfTaskThread.Start();
            stepTaskThread.Join();
            halfTaskThread.Join();


            mainTask.CalculateMaxDifference(mainTask.GetDifference(halfTask.GetData()), out maxDif, out maxX, out maxY);

            ChangeTextBoxValue(OmegaTextBox,        mainTask.Ω.ToString());
            ChangeLabelValue  (MaxDifLabelMain,     Math.Round(maxDif, 7).ToString());
            ChangeLabelValue  (ResidualMainTextBox, mainTask.CalculateR().ToString());
            ChangeLabelValue  (IterLabelMain,       maxIterMain.ToString());
            ChangeLabelValue  (AccMaxLabelMain,     maxAccMain.ToString());
            ChangeLabelValue  (ResidualHalfTextBox, halfTask.CalculateR().ToString());
            ChangeLabelValue  (IterLabelMainHalf,   maxIterHalf.ToString());
            ChangeLabelValue  (AccMaxLabelMainHalf, maxAccHalf.ToString());
            ChangeLabelValue  (DotLabel,            "Соответствует узлу x = " + 
                                                    Math.Round(Math.Abs(maxX), 3).ToString() + "  y = " + 
                                                    Math.Round(Math.Abs(maxY), 3).ToString());

            tableCreator.Init(N * 2 + 1u, M * 2 + 1u);
            ChangeTableValues(TableHalf, halfTask.GetData());

            tableCreator.Init(N + 1u, M + 1u);
            ChangeTableValues(TableMain,     mainTask.GetData());
            ChangeTableValues(TableDiffMain, mainTask.GetDifference(halfTask.GetData()));

            ChangeButtonVisibility(button2, true);
        }


        // Test task
        private void button1_Click(object sender, EventArgs e)
        {
            if (!ParseArguments())
            {
                MessageBox.Show("Ошибка ввода!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Thread testTaskThread = new Thread(new ThreadStart(RunTestTask));
            testTaskThread.Start();
        }


        // Main task
        private void button2_Click(object sender, EventArgs e)
        {
            if(!ParseArguments())
            {
                MessageBox.Show("Ошибка ввода!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Thread mainTaskThread = new Thread(new ThreadStart(RunMainTask));
            mainTaskThread.Start();
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
                use_optimal_omega    = true;
            }
            else
            {
                OmegaTextBox.Enabled = true;
                use_optimal_omega    = false;
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
