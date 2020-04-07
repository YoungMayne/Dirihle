using CenterSpace.NMath.Core;
using CenterSpace.NMath.Matrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericalMethods
{
    class SimpleIterationMethod : MethodBase
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


        public override void   SetSpecialParameter(double value)
        {
            tau = value;
        }


        protected override void   InitMethod()
        {
            double radius = Math.Abs(h2) + Math.Abs(k2) + Math.Abs(k2);

            tau = 2.0 / ((a2 - radius) + (a2 + radius));
        }


        protected override void   InitRun()
        {

        }


        protected override void   InitIteration()
        {
            UpdateResidual();
        }


        protected override double GetNextValue(uint i, uint j)
        {
            return data[i, j] - tau * residual[i, j];
        }
    }
}
