using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dirihle
{
    class TopRelaxationMethod : MethodBase
    {
        public double Ω { get; set; }

        public TopRelaxationMethod(
            double Xo, 
            double Xn, 
            double Yo, 
            double Yn, 
            uint N, 
            uint M,
            ApproximationType approximationType) : 
            base(Xo, Xn, Yo, Yn, N, M, approximationType) 
        {
            double lambdaMin = 
                2.0 * k * k / (h * h + k * k) * 
                Math.Sin(Math.PI * h / (2.0 * (Xn - Xo))) * 
                Math.Sin(Math.PI * h / (2.0 * (Xn - Xo))) + 
                2.0 * h * h / (h * h + k * k) * 
                Math.Sin(Math.PI * k / (2.0 * (Yn - Yo))) * 
                Math.Sin(Math.PI * k / (2.0 * (Yn - Yo)));

            Ω = 2.0 / (1.0 + Math.Sqrt(lambdaMin * (2.0 - lambdaMin)));
        }

        public override void Run(ref uint maxIter, ref double maxAccuracy)
        {
            double current  = 0.0;
            double accuracy = 0.0;
            double h2       = -Math.Pow(N / (Xn - Xo), 2);
            double k2       = -Math.Pow(M / (Yn - Yo), 2);
            double a2       = -2.0 * (h2 + k2);
            double a2Opp    = 1.0 / a2;
            uint counter    = 0u;

            double current_accuracy;
            double new_v;

            do
            {
                accuracy = 0.0;

                for (uint j = 1u; j < M; ++j)
                {
                    for (uint i = 1u; i < N; ++i)
                    {
                        current = data[i, j];

                        new_v   = 0.0;
                        new_v  += -Ω * (k2 * (data[i + 1u, j] + data[i - 1u, j]) +
                                        h2 * (data[i, j + 1u] + data[i, j - 1u]));
                        new_v  += (1.0 - Ω) * a2 * data[i, j] + Ω * -Function(i, j);
                        new_v  *= a2Opp;

                        current_accuracy = Math.Abs(current - new_v);

                        if (accuracy < current_accuracy)
                        {
                            accuracy = current_accuracy;
                        }

                        data[i, j] = new_v;
                    }
                }

            } while ((maxIter > ++counter) && (accuracy >= maxAccuracy));

            maxIter     = counter;
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

        public double CalculateR()
        {
            double R  = 0.0;
            double h2 = -Math.Pow(N / (Xn - Xo), 2);
            double k2 = -Math.Pow(M / (Yn - Yo), 2);
            double a2 = -2.0 * (h2 + k2);

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
                         Function(i, j), 2.0);
                }
            }

            return Math.Sqrt(R);
        }
    }
}
