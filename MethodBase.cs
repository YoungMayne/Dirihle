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

        protected Func<double, double, double> Function;
        protected Func<double, double, double> ExactFunction;

        protected ApproximationType approximationType;


        public MethodBase()
        {

        }


        public MethodBase(
            double Xo, double Xn, double Yo, double Yn,
            uint N, uint M, ApproximationType approximationType)
        {
            Init(Xo, Xn, Yo, Yn, N, M, approximationType);
        }


        public void Init(
            double Xo, double Xn, double Yo, double Yn,
            uint N, uint M, ApproximationType approximationType)
        {
            this.data = new double[N + 1u, M + 1u];
            this.function = new double[N + 1u, M + 1u];
            this.Xo = Xo;
            this.Yo = Yo;
            this.Xn = Xn;
            this.Yn = Yn;
            this.N = N;
            this.M = M;
            this.h = (this.Xn - this.Xo) / N;
            this.k = (this.Yn - this.Yo) / M;
            this.h2 = -Math.Pow(N / (Xn - Xo), 2);
            this.k2 = -Math.Pow(M / (Yn - Yo), 2);
            this.a2 = -2.0 * (h2 + k2);
            this.approximationType = approximationType;
            InitMethod();
        }


        public abstract void Run(ref uint maxIter, ref double maxAccuracy);


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


        public double X(uint i) => Xo + i * h;


        public double Y(uint j) => Yo + j * k;


        protected abstract double V(uint i, uint j);


        protected abstract void InitMethod();


        protected abstract void InitRun();


        protected abstract void InitIteration();


        protected abstract double GetNextValue(uint i, uint j);
    }
}
