using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericalMethods
{
    class ChebyshevSimpleIterationMethod : MethodBase
    {
        private uint K;
        private double[] tau;
        private long counter;


        public ChebyshevSimpleIterationMethod() : base()
        {

        }


        public ChebyshevSimpleIterationMethod(
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
            return (double)K;
        }


        public override void SetSpecialParameter(double value)
        {
            K = (uint)value;
        }


        protected override void InitMethod()
        {
            K = 1u;
        }


        protected override void InitRun()
        {
            double radiusMax = Math.Abs(h2) + Math.Abs(k2) + Math.Abs(k2);
            double lambdaMin = (a2 - radiusMax);
            double lambdaMax = (a2 + radiusMax);
            tau              = new double[K];

            for (uint i = 0u; i < K; ++i)
            {
                tau[i] = Math.Pow((((lambdaMin + lambdaMax) / 2.0) +
                                   ((lambdaMax - lambdaMin) / 2.0) *
                                     Math.Cos((Math.PI / (2.0 * K)) * (2.0 * i - 1.0))), -1.0);
            }

            counter  = -1u;
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

            counter = ++counter % tau.Length;
        }


        protected override double GetNextValue(uint i, uint j)
        {
            return data[i, j] - tau[counter] * residual[i, j];
        }
    }
}
