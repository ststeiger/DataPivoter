
namespace DataPivoter
{


    public static class GeometryExtensions
    {


        public static void Test()
        {
            // double h = System.Math.Sin(90 * System.Math.PI / 180);
            // double a = System.Math.Asin(h)/System.Math.PI * 180;
            // double b = ToDegrees(System.Math.Asin(h));

            double h = 2.2;
            double t = 6.3;
            double a = System.Math.Asin(h / t).ToDegrees();
            double h2 = t * System.Math.Sin(a.ToRadians());
            System.Console.WriteLine(h2);
        }

        public static double ToRadians(this double val)
        {
            return (System.Math.PI / 180.0) * val;
        }

        public static double ToDegrees(this double val)
        {
            return (180.0 / System.Math.PI) * val;
        }


    }


}
