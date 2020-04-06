using CenterSpace.NMath.Core;
using CenterSpace.NMath.Matrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dirihle
{
    class SimpleIterationMethod : MethodBase
    {
        public double τ { get; set; }

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
            double h2 = -Math.Pow(N / (Xn - Xo), 2);
            double k2 = -Math.Pow(M / (Yn - Yo), 2);
            double a2 = -2.0 * (h2 + k2);
            double r = Math.Abs(h2) + Math.Abs(k2) + Math.Abs(k2);

            τ = 2.0 / ((a2 - r) + (a2 + r));
        }

        public override void Run(ref uint maxIter, ref double maxAccuracy)
        {
            double[,] residual = new double[N + 1u, M + 1u];
            double accuracy    = 0.0;
            double h2          = -Math.Pow(N / (Xn - Xo), 2);
            double k2          = -Math.Pow(M / (Yn - Yo), 2);
            double a2          = -2.0 * (h2 + k2);
            uint counter       = 0u;

            double current_accuracy;
            double current;
            double new_v;

            do
            {
                accuracy = 0.0;

                for (uint i = 1; i < N; i++)
                {
                    for (uint j = 1; j < M; j++)
                    {
                        residual[i, j] =
                            a2 * data[i, j] +
                            h2 * (data[i - 1, j] + data[i + 1, j]) +
                            k2 * (data[i, j - 1] + data[i, j + 1])
                            + function[i, j];
                    }
                }


                for (uint i = 1; i < N; i++)
                {
                    for (uint j = 1; j < M; j++)
                    {
                        current = data[i, j];

                        new_v = data[i, j] - τ * residual[i, j];

                        current_accuracy = Math.Abs(current - new_v);

                        if (accuracy < current_accuracy)
                        {
                            accuracy = current_accuracy;
                        }

                        data[i, j] = new_v;
                    }
                }

            } while ((maxIter > ++counter) && (accuracy >= maxAccuracy));

            maxIter = counter;
            maxAccuracy = accuracy;
        }

        protected override double Function(uint i, uint j)
        {
            throw new NotImplementedException();
        }

        protected override double mu1(double y)
        {
            throw new NotImplementedException();
        }

        protected override double mu2(double y)
        {
            throw new NotImplementedException();
        }

        protected override double mu3(double x)
        {
            throw new NotImplementedException();
        }

        protected override double mu4(double x)
        {
            throw new NotImplementedException();
        }
    }
}
