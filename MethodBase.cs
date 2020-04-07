using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericalMethods
{
    enum ApproximationType
    {
        ZERO_APPROXIMATION,
        X_INTERPOLATION,
        Y_INTERPOLATION
    }

    abstract class MethodBase
    {
        protected uint N;
        protected uint M;

        protected double Xo;
        protected double Xn;
        protected double Yo;
        protected double Yn;
        protected double h2;
        protected double k2;
        protected double a2;
        protected double h;
        protected double k;

        protected double[,] data;
        protected double[,] function;
        protected double[,] residual;


        private Func<double, double> mu1;
        private Func<double, double> mu2;
        private Func<double, double> mu3;
        private Func<double, double> mu4;
        private Func<double, double, double> Function;
        private Func<double, double, double> ExactFunction;


        public MethodBase()
        {

        }


        public MethodBase(double Xo,
                          double Xn,
                          double Yo,
                          double Yn,
                          uint N,
                          uint M,
                          ApproximationType approximationType)
        {
            Init(Xo, Xn, Yo, Yn, N, M, approximationType);
        }


        public void Init(double Xo,
                         double Xn,
                         double Yo,
                         double Yn,
                         uint N,
                         uint M,
                         ApproximationType approximationType)
        {
            this.data     = new double[N + 1u, M + 1u];
            this.function = new double[N + 1u, M + 1u];
            this.residual = new double[N + 1u, M + 1u];
            this.Xo       = Xo;
            this.Yo       = Yo;
            this.Xn       = Xn;
            this.Yn       = Yn;
            this.N        = N;
            this.M        = M;
            this.h        = (this.Xn - this.Xo) / N;
            this.k        = (this.Yn - this.Yo) / M;
            this.h2       = -Math.Pow(N / (Xn - Xo), 2);
            this.k2       = -Math.Pow(M / (Yn - Yo), 2);
            this.a2       = -2.0 * (h2 + k2);

            Approximate(approximationType);
            InitMethod();
        }


        public void Run(ref uint maxIter, ref double maxAccuracy)
        {
            double current_accuracy;
            double nextValue;
            double currentValue;
            double accuracy;
            uint counter = 0u;

            InitRun();

            do
            {
                accuracy = 0.0;

                InitIteration();

                for (uint j = 1u; j < M; ++j)
                {
                    for (uint i = 1u; i < N; ++i)
                    {
                        currentValue     = data[i, j];
                        nextValue        = GetNextValue(i, j);
                        current_accuracy = Math.Abs(currentValue - nextValue);

                        if (accuracy < current_accuracy)
                        {
                            accuracy = current_accuracy;
                        }

                        data[i, j] = nextValue;
                    }
                }

            } while ((maxIter > ++counter) && (accuracy >= maxAccuracy));

            maxIter = counter;
            maxAccuracy = accuracy;
        }


        public void SetFunctions(
            Func<double, double> mu1, Func<double, double> mu2, Func<double, double> mu3, 
            Func<double, double> mu4, Func<double, double, double> Function, Func<double, double, double> ExactFunction = null)
        {
            this.mu1 = mu1;
            this.mu2 = mu2;
            this.mu3 = mu3;
            this.mu4 = mu4;
            this.Function      = Function;
            this.ExactFunction = ExactFunction;

            for (uint i = 0u; i < N + 1; ++i)
            {
                for (uint j = 0u; j < M + 1; ++j)
                {
                    data[i, j] = V(i, j);
                }
            }

            for (uint i = 1u; i < N; ++i)
            {
                for (uint j = 1u; j < M; ++j)
                {
                    function[i, j] = Function(X(i), Y(j));
                }
            }
        }


        public double[,] GetData() => data;


        public double[,] GetExactTable()
        {
            double[,] exact = new double[N + 1u, M + 1u];

            for (uint i = 0u; i < N + 1u; ++i)
            {
                for (uint j = 0u; j < M + 1u; ++j)
                {
                    exact[i, j] = ExactFunction(X(i), Y(j));
                }
            }

            return exact;
        }


        public double CalculateResidual()
        {
            double R = 0.0;

            for (uint j = 1u; j < M; ++j)
            {
                for (uint i = 1u; i < N; ++i)
                {
                    R += Math.Pow(
                         a2 * data[i, j] +
                         h2 * (data[i - 1, j] +
                         data[i + 1, j]) +
                         k2 * (data[i, j - 1] +
                         data[i, j + 1]) +
                         Function(X(i), Y(j)), 2.0);
                }
            }

            return Math.Sqrt(R);
        }


        public uint GetN() => N;


        public uint GetM() => M;


        public abstract double GetSpecialParameter();


        public abstract void SetSpecialParameter(double value);


        protected double X(uint i) => Xo + i * h;


        protected double Y(uint j) => Yo + j * k;


        protected double V(uint i, uint j)
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

            return data[i, j];
        }


        protected void UpdateResidual()
        {
            for (uint i = 1; i < N; ++i)
            {
                for (uint j = 1; j < M; ++j)
                {
                    residual[i, j] = a2 * data[i, j] +
                                     h2 * (data[i - 1, j] + data[i + 1, j]) +
                                     k2 * (data[i, j - 1] + data[i, j + 1]) +
                                     function[i, j];
                }
            }
        }


        protected abstract void InitMethod();


        protected abstract void InitRun();


        protected abstract void InitIteration();


        protected abstract double GetNextValue(uint i, uint j);


        private void Approximate(ApproximationType approximationType)
        {
            switch (approximationType)
            {
                case ApproximationType.ZERO_APPROXIMATION:
                    ZeroApproximation();
                    break;

                case ApproximationType.X_INTERPOLATION:
                    XInterpolation();
                    break;

                case ApproximationType.Y_INTERPOLATION:
                    YInterpolation();
                    break;
            }
        }


        private void ZeroApproximation()
        {
            for (uint i = 0u; i < N + 1; ++i)
            {
                for (uint j = 0u; j < M + 1; ++j)
                {
                    data[i, j] = 0.0;
                }
            }
        }


        private void XInterpolation()
        {
            for (uint i = 1u; i < N; ++i)
            {
                for (uint j = 1u; j < M; ++j)
                {
                    data[i, j] = ((Xo + i * h) - Xo) / (Xn - Xo) * mu2(Yo + j * k) +
                                 ((Xo + i * h) - Xn) / (Xo - Xn) * mu1(Yo + j * k);
                }
            }
        }


        private void YInterpolation()
        {
            for (uint i = 1u; i < N; ++i)
            {
                for (uint j = 1u; j < M; ++j)
                {
                    data[i, j] = ((Yo + j * k) - Yo) / (Yn - Yo) * mu4(Xo + i * h) +
                              ((Yo + j * k) - Yn) / (Yo - Yn) * mu3(Xo + i * h);
                }
            }
        }
    }
}
