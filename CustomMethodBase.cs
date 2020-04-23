using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericalMethods
{
    abstract class CustomMethodBase : MethodBase
    {
        protected uint P;
        protected uint Q;

        private Func<double, double> mu1;
        private Func<double, double> mu2;
        private Func<double, double> mu3;
        private Func<double, double> mu4;
        private Func<double, double> mu5;
        private Func<double, double> mu6;

        public CustomMethodBase() : base()
        {

        }


        public CustomMethodBase(double Xo,
                                double Xn,
                                double Yo,
                                double Yn,
                                uint N,
                                uint M,
                                ApproximationType approximationType) :
                                base(Xo, Xn, Yo, Yn, N, M, approximationType)
        {
            
        }


        public override void Run(ref uint maxIter, ref double maxAccuracy)
        {
            double current_accuracy;
            double nextValue;
            double currentValue;
            double accuracy;
            uint counter = 0u;

            P = M / 2u;
            Q = N / 2u;

            Approximate();


            for (uint i = 0u; i < N + 1; ++i)
            {
                for (uint j = 0u; j < M + 1; ++j)
                {
                    data[i, j] = V(i, j);
                    function[i, j] = Function(X(i), Y(j));
                }
            }

            InitRun();

            do
            {
                accuracy = 0.0;

                InitIteration();

                for (uint i = Q + 1u; i < N; ++i)
                {
                    for (uint j = 1u; j < P; ++j)
                    {
                        currentValue = data[i, j];
                        nextValue = GetNextValue(i, j);
                        current_accuracy = Math.Abs(currentValue - nextValue);

                        if (accuracy < current_accuracy)
                        {
                            accuracy = current_accuracy;
                        }

                        data[i, j] = nextValue;
                    }
                }

                for (uint i = 1u; i < N; ++i)
                {
                    for (uint j = P; j < M; ++j)
                    {
                        currentValue = data[i, j];
                        nextValue = GetNextValue(i, j);
                        current_accuracy = Math.Abs(currentValue - nextValue);

                        if (accuracy < current_accuracy)
                        {
                            accuracy = current_accuracy;
                        }

                        data[i, j] = nextValue;
                    }
                }

            } while ((maxIter > ++counter) && (accuracy >= maxAccuracy));

            maxIter = counter;
            maxAccuracy = accuracy;
        }


        protected override double V(uint i, uint j)
        {
            if ((0u == i) && (P <= j))
            {
                return mu1(Y(j));
            }
            if (N == i)
            {
                return mu2(Y(j));
            }
            if ((0u == j) && (Q <= i))
            {
                return mu3(X(i));
            }
            if (M == j)
            {
                return mu4(X(i));
            }
            if ((Q == i) && (P >= j))
            {
                return mu5(X(i));
            }
            if ((P == j) && (Q >= i))
            {
                return mu6(Y(j));
            }

            return data[i, j];
        }


        public void SetFunctions(
            Func<double, double> mu1, Func<double, double> mu2,
            Func<double, double> mu3, Func<double, double> mu4,
            Func<double, double> mu5, Func<double, double> mu6,
            Func<double, double, double> Function,
            Func<double, double, double> ExactFunction = null)
        {
            this.mu1 = mu1;
            this.mu2 = mu2;
            this.mu3 = mu3;
            this.mu4 = mu4;
            this.mu5 = mu5;
            this.mu6 = mu6;
            this.Function = Function;
            this.ExactFunction = ExactFunction;
        }


        private void Approximate()
        {
            switch (approximationType)
            {
                case ApproximationType.ZERO_APPROXIMATION:
                    ZeroApproximation();
                    break;

                case ApproximationType.X_INTERPOLATION:
                    XInterpolation();
                    break;

                case ApproximationType.Y_INTERPOLATION:
                    YInterpolation();
                    break;
            }
        }


        private void ZeroApproximation()
        {
            for (uint i = 0u; i < N + 1; ++i)
            {
                for (uint j = 0u; j < M + 1; ++j)
                {
                    data[i, j] = 0.0;
                }
            }
        }


        private void XInterpolation()
        {
            for (uint i = Q + 1u; i < N; ++i)
            {
                for (uint j = 1u; j < P; ++j)
                {
                    data[i, j] = ((Xo + i * h) - Xo) / (Xn - Xo) * mu2(Yo + j * k) +
                                 ((Xo + i * h) - Xn) / (Xo - Xn) * mu1(Yo + j * k);
                }
            }

            for (uint i = 1u; i < N; ++i)
            {
                for (uint j = P; j < M; ++j)
                {
                    data[i, j] = ((Xo + i * h) - Xo) / (Xn - Xo) * mu2(Yo + j * k) +
                                 ((Xo + i * h) - Xn) / (Xo - Xn) * mu1(Yo + j * k);
                }
            }
        }


        private void YInterpolation()
        {
            for (uint i = Q + 1u; i < N; ++i)
            {
                for (uint j = 1u; j < P; ++j)
                {
                    data[i, j] = ((Yo + j * k) - Yo) / (Yn - Yo) * mu4(Xo + i * h) +
                                 ((Yo + j * k) - Yn) / (Yo - Yn) * mu3(Xo + i * h);
                }
            }

            for (uint i = 1u; i < N; ++i)
            {
                for (uint j = P; j < M; ++j)
                {
                    data[i, j] = ((Yo + j * k) - Yo) / (Yn - Yo) * mu4(Xo + i * h) +
                                 ((Yo + j * k) - Yn) / (Yo - Yn) * mu3(Xo + i * h);
                }
            }
        }
    }
}
