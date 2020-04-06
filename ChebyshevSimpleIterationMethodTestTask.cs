using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dirihle
{
    class ChebyshevSimpleIterationMethodTestTask : ChebyshevSimpleIterationMethod
    {
        public ChebyshevSimpleIterationMethodTestTask(
            double Xo,
            double Xn,
            double Yo,
            double Yn,
            uint N,
            uint M,
            ApproximationType approximationType) :
            base(Xo, Xn, Yo, Yn, N, M, approximationType)
        {
        }


        protected override double mu1(double y) => Math.Exp(1.0 - Math.Pow(Xo, 2) - Math.Pow(y, 2));
        protected override double mu2(double y) => Math.Exp(1.0 - Math.Pow(Xn, 2) - Math.Pow(y, 2));
        protected override double mu3(double x) => Math.Exp(1.0 - Math.Pow(x, 2) - Math.Pow(Yo, 2));
        protected override double mu4(double x) => Math.Exp(1.0 - Math.Pow(x, 2) - Math.Pow(Yn, 2));

        protected override double Function(uint i, uint j)
        {
            double xx = X(i) * X(i);
            double yy = Y(j) * Y(j);
            return Math.Exp(-xx - yy + 1.0) * (4.0 * xx - 2.0 + 4.0 * yy - 2.0);
        }

        private double ExactFunction(uint i, uint j)
        {
            return Math.Exp(1.0 - Math.Pow(X(i), 2.0) - Math.Pow(Y(j), 2.0));
        }

        public void CalculateMaxDifference(out double maxDif,
                                           out double maxX,
                                           out double maxY)
        {
            uint maxI = 0u;
            uint maxJ = 0u;
            maxDif = 0.0;

            for (uint i = 0; i < N + 1; ++i)
            {
                for (uint j = 0; j < M + 1; ++j)
                {
                    double cur_dif = Math.Abs(ExactFunction(i, j) - data[i, j]);

                    if (cur_dif > maxDif)
                    {
                        maxDif = cur_dif;
                        maxI = i;
                        maxJ = j;
                    }
                }
            }

            maxX = X(maxI);
            maxY = Y(maxJ);
        }

        public double[,] GetExact()
        {
            double[,] exact = new double[N + 1u, M + 1u];

            for (uint i = 0u; i < N + 1u; ++i)
            {
                for (uint j = 0u; j < M + 1u; ++j)
                {
                    exact[i, j] = ExactFunction(i, j);
                }
            }

            return exact;
        }

        public double[,] GetDifference(double[,] exact)
        {
            double[,] difference = new double[N + 1u, M + 1u];

            for (uint i = 0u; i < N + 1u; ++i)
            {
                for (uint j = 0u; j < M + 1u; ++j)
                {
                    difference[i, j] = Math.Abs(data[i, j] - exact[i, j]);
                }
            }

            return difference;
        }
    }
}
