using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericalMethods
{
    class MinimalDiscrepancyMethod : RectangularMethodBase
    {
        private double t;


        public MinimalDiscrepancyMethod() : base()
        {

        }


        public MinimalDiscrepancyMethod(double Xo,
                                        double Xn,
                                        double Yo,
                                        double Yn,
                                        uint N,
                                        uint M,
                                        ApproximationType approximationType) :
                                   base(Xo, Xn, Yo, Yn, N, M, approximationType)
        {
            t = 0.0;
        }


        public override double GetSpecialParameter()
        {
            return t;
        }


        public override void SetSpecialParameter(double value)
        {
            t = value;
        }


        protected override double GetNextValue(uint i, uint j)
        {
            return data[i, j] - t * residual[i, j];
        }


        protected override void InitIteration()
        {
            t = 0.0;
            double denominator = 0.0;

            for (uint i = 1u; i < N; ++i)
            {
                for (uint j = 1u; j < M; ++j)
                {
                    residual[i, j] = a2 * data[i, j] +
                                     h2 * (data[i - 1u, j] + data[i + 1u, j]) +
                                     k2 * (data[i, j - 1u] + data[i, j + 1u]) +
                                     function[i, j];
                }
            }

            for (uint i = 1u; i < N; ++i)
            {
                for (uint j = 1u; j < M; ++j)
                {
                    double ar = a2 * residual[i, j] +
                                h2 * (residual[i - 1, j] + residual[i + 1, j]) +
                                k2 * (residual[i, j - 1] + residual[i, j + 1]);
                    t += ar * residual[i, j];
                    denominator += ar * ar;
                }
            }

            t /= denominator;
        }


        protected override void InitMethod()
        {
            residual = new double[N + 1u, M + 1u];
        }


        protected override void InitRun()
        {

        }
    }
}
