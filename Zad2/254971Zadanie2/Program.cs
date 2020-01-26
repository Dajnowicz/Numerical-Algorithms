using System;
using System.IO;
using System.Text;
namespace GaussLinear
{
    // Procesor i5-83500h
    // 20 GB Ram
    // GPU GTX 1050 4gb
    // Visual Studio 2019 Professional
    public enum GaussType { Basic, Part, Full };

    class Program
    {
        static void Main()
        {
            var header = new StringBuilder();
            header.AppendLine("_time;;;;;;;;;_difference");
            header.AppendLine("Float;;;Double;;;Fraction;;;Float;;;Double;;;Fraction;;");
            header.AppendLine("G;PG;FG;G;PG;FG;G;PG;FG;G;PG;FG;G;PG;FG;G;PG;FG");
            File.WriteAllText("result.csv", header.ToString());

            for (int index = 3; index < 611; index += 3)
            {
                var content = new StringBuilder();
                var etest = new EfficiencyTest(index);
                content.AppendLine(etest.Run().Result);
                File.AppendAllText("result.csv", content.ToString());
            }

            Console.ReadKey();
        }
    }
}
