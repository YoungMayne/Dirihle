using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dirihle
{
    class TopRelaxationMethodMainTask : TopRelaxationMethod
    {
        public TopRelaxationMethodMainTask(
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

        protected override double mu1(double y) =>  1.0 - Math.Pow(y, 2.0);
        protected override double mu2(double y) => (1.0 - Math.Pow(y, 2.0)) * Math.Exp(y);
        protected override double mu3(double x) =>  1.0 - Math.Pow(x, 2.0);
        protected override double mu4(double x) =>  1.0 - Math.Pow(x, 2.0);

        protected override double Function(uint i, uint j)
        {
            return -Math.Abs(Math.Pow(X(i), 2.0) - Math.Pow(Y(j), 2.0));
        }

        public void CalculateMaxDifference(    double [,] difference,
                                           out double maxDif,
                                           out double maxX,
                                           out double maxY)
        {
            maxDif = 0.0;
            maxX   = 0.0;
            maxY   = 0.0;

            for(uint i = 1u; i < N; ++i)
            {
                for(uint j = 1u; j < M; ++j)
                {
                    if (difference[i, j] > maxDif)
                    {
                        maxDif = difference[i, j];
                        maxX   = X(i);
                        maxY   = Y(j);
                    }
                }
            }
        }

        public double[,] GetDifference(double[,] halfStep)
        {
            uint width  = M * 2;
            uint height = N * 2;
            double[,] difference = new double[N + 1u, M + 1u];

            for (int iStep = 1, iHalf = 2; (iStep < height / 2) && (iHalf < height); ++iStep, iHalf += 2)
            {
                for (int jStep = 1, jHalf = 2; (jStep < width / 2) && (jHalf < width); ++jStep, jHalf += 2)
                {
                    difference[iStep, jStep] = 
                        Math.Abs(data[iStep, jStep] - halfStep[iHalf, jHalf]);
                }
            }

            return difference;
        }
    }
}
