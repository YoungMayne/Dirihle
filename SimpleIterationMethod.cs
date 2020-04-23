using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericalMethods
{
    class SimpleIterationMethod : RectangularMethodBase
    {
        private double tau;


        public SimpleIterationMethod() : base()
        {

        }


        public SimpleIterationMethod(
            double Xo,
            double Xn,
            double Yo,
            double Yn,
            uint N,
            uint M,
            ApproximationType approximationType)
            : base(Xo, Xn, Yo, Yn, N, M, approximationType)
        {
        }


        public override double GetSpecialParameter()
        {
            return tau;
        }


        public override void SetSpecialParameter(double value)
        {
            tau = value;
        }


        protected override void InitMethod()
        {
            double lambdaMin = 4.0 / (h * h) * Math.Sin(Math.PI / (2u * N)) * Math.Sin(Math.PI / (2u * N)) +
                               4.0 / (k * k) * Math.Sin(Math.PI / (2u * M)) * Math.Sin(Math.PI / (2u * M));

            double lambdaMax = 4.0 / (h * h) * Math.Sin(Math.PI * (N - 1u) / (2.0 * N)) * Math.Sin(Math.PI * (N - 1u) / (2u * N)) +
                               4.0 / (k * k) * Math.Sin(Math.PI * (M - 1u) / (2.0 * M)) * Math.Sin(Math.PI * (M - 1u) / (2u * M));

            tau = 2.0 / (lambdaMin + lambdaMax);
        }


        protected override void InitRun()
        {
            residual = new double[N + 1u, M + 1u];
        }


        protected override void InitIteration()
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


        protected override double GetNextValue(uint i, uint j)
        {
            return data[i, j] - tau * residual[i, j];
        }
    }
}
