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
        private uint counter;


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


        protected override void   InitMethod()
        {
            K = 1u;
        }


        protected override void   InitRun()
        {
            double radius    = Math.Abs(h2) + Math.Abs(k2) + Math.Abs(k2);
            double lambdaMin = (a2 - radius);
            double lambdaMax = (a2 + radius);
            tau              = new double[K];

            for (uint i = 0u; i < K; ++i)
            {
                tau[i] = Math.Pow((((lambdaMin + lambdaMax) / 2.0) +
                                   ((lambdaMax - lambdaMin) / 2.0) *
                                     Math.Cos((Math.PI / (2.0 * K)) * (1.0 + (2.0 * i)))), -1.0);
            }

            counter = 0u;
        }


        protected override void   InitIteration()
        {
            UpdateResidual();
        }


        protected override double GetNextValue(uint i, uint j)
        {
            return data[i, j] - tau[counter++ % tau.Length] * residual[i, j];
        }
    }
}
