using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumericalMethods
{
    static class Functions
    {
        private static double Xo;
        private static double Yo;
        private static double Xn;
        private static double Yn;

        public static void Set(double Xo, double Yo, double Xn, double Yn)
        {
            Functions.Xo = Xo;
            Functions.Yo = Yo;
            Functions.Xn = Xn;
            Functions.Yn = Yn;
        }

        public static double mu1Test(double y) => Math.Exp(1.0 - Math.Pow(Xo, 2) - Math.Pow(y, 2));
        public static double mu2Test(double y) => Math.Exp(1.0 - Math.Pow(Xn, 2) - Math.Pow(y, 2));
        public static double mu3Test(double x) => Math.Exp(1.0 - Math.Pow(x, 2) - Math.Pow(Yo, 2));
        public static double mu4Test(double x) => Math.Exp(1.0 - Math.Pow(x, 2) - Math.Pow(Yn, 2));
        public static double FunctionTest(double x, double y) =>
            Math.Exp(-(x * x) - (y * y) + 1.0) * (4.0 * x * x - 2.0 + 4.0 * y * y - 2.0);
        public static double ExactFunction(double x, double y) =>
            Math.Exp(1.0 - (x * x) - (y * y));

        public static double mu1Main(double y) => 1.0 - Math.Pow(y, 2.0);
        public static double mu2Main(double y) => (1.0 - Math.Pow(y, 2.0)) * Math.Exp(y);
        public static double mu3Main(double x) => 1.0 - Math.Pow(x, 2.0);
        public static double mu4Main(double x) => 1.0 - Math.Pow(x, 2.0);
        public static double FunctionMain(double x, double y) => -Math.Abs(x * x - y * y);
    }
}
