using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericalMethods
{
    class TopRelaxationMethod : MethodBase
    {
        public double  omega;
        private double a2Opp;
        private double omegak2;
        private double omegah2;
        private double omegaa2;


        public TopRelaxationMethod() : base()
        {

        }


        public TopRelaxationMethod(double Xo, 
                                   double Xn, 
                                   double Yo, 
                                   double Yn, 
                                   uint N, 
                                   uint M,
                                   ApproximationType approximationType) : 
                                   base(Xo, Xn, Yo, Yn, N, M, approximationType) 
        {

        }


        public override double GetSpecialParameter() => omega;


        public override void   SetSpecialParameter(double value) => omega = value;


        protected override void   InitMethod()
        {
            double lambdaMin =
                (2.0 * k * k / (h * h + (k * k)) *
                Math.Sin(Math.PI * h / (2.0 * (Xn - Xo))) *
                Math.Sin(Math.PI * h / (2.0 * (Xn - Xo)))) +
                (2.0 * h * h / ((h * h) + (k * k)) *
                Math.Sin(Math.PI * k / (2.0 * (Yn - Yo))) *
                Math.Sin(Math.PI * k / (2.0 * (Yn - Yo))));

            omega = 2.0 / (1.0 + Math.Sqrt(lambdaMin * (2.0 - lambdaMin)));
        }


        protected override void   InitRun()
        {
            a2Opp = 1.0 / a2;
            omegak2 = omega * k2;
            omegah2 = omega * h2;
            omegaa2 = (1.0 - omega) * a2;
        }


        protected override void   InitIteration()
        {
            
        }


        protected override double GetNextValue(uint i, uint j)
        {
            return a2Opp * ((-omegak2 * (data[i + 1u, j] + data[i - 1u, j])) +
                            (-omegah2 * (data[i, j + 1u] + data[i, j - 1u])) +
                            (omegaa2 * data[i, j] + omega * -function[i, j]));
        }
    }
}
