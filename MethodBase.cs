using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dirihle
{
    enum ApproximationType
    {
        ZERO_APPROXIMATION,
        X_INTERPOLATION,
        Y_INTERPOLATION
    }

    abstract class MethodBase
    {
        protected uint N;
        protected uint M;

        protected double Xo;
        protected double Xn;
        protected double Yo;
        protected double Yn;
        protected double h;
        protected double k;

        protected double[,] data;


        public MethodBase(
            double Xo, 
            double Xn, 
            double Yo, 
            double Yn, 
            uint N, 
            uint M, 
            ApproximationType approximationType)
        {
            this.data = new double[N + 1, M + 1];
            this.Xo   = Xo;
            this.Yo   = Yo;
            this.Xn   = Xn;
            this.Yn   = Yn;
            this.N    = N;
            this.M    = M;
            this.h    = (this.Xn - this.Xo) / N;
            this.k    = (this.Yn - this.Yo) / M;

            Init(approximationType);
        }


        public    abstract void   Run           (ref uint   maxIter, 
                                                 ref double maxAccuracy);
        protected abstract double Function      (uint i, uint j);
        protected abstract double mu1(double y);
        protected abstract double mu2(double y);
        protected abstract double mu3(double x);
        protected abstract double mu4(double x);


        protected double X(uint i)     => Xo + i * h;
        protected double Y(uint j)     => Yo + j * k;
        public    double[,] GetData()  => data;
        public    uint      GetN()     => N;
        public    uint      GetM()     => M;


        protected double V(uint i, uint j)
        {
            if (0u == i)
            {
                return mu1(Y(j));
            }
            if (N == i)
            {
                return mu2(Y(j));
            }
            if (0u == j)
            {
                return mu3(X(i));
            }
            if (M == j)
            {
                return mu4(X(i));
            }

            return data[i, j];
        }

        private void Init(ApproximationType approximationType)
        {
            Approximate(approximationType);

            for (uint i = 0u; i < N + 1; ++i)
            {
                for (uint j = 0u; j < M + 1; ++j)
                {
                    data[i, j] = V(i, j);
                }
            }
        }

        private void Approximate(ApproximationType approximationType)
        {
            switch(approximationType)
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
            for (uint i = 1u; i < N; ++i)
            {
                for (uint j = 1u; j < M; ++j)
                {
                    data[i, j] = ((Xo + i * h) - Xo) / (Xn - Xo) * mu2(Yo + j * k) +
                                 ((Xo + i * h) - Xn) / (Xo - Xn) * mu1(Yo + j * k);
                }
            }
        }

        private void YInterpolation()
        {
            for (uint i = 1u; i < N; ++i)
            {
                for (uint j = 1u; j < M; ++j)
                {
                    data[i, j] = ((Yo + j * k) - Yo) / (Yn - Yo) * mu4(Xo + i * h) +
                              ((Yo + j * k) - Yn) / (Yo - Yn) * mu3(Xo + i * h);
                }
            }
        }
    }
}
