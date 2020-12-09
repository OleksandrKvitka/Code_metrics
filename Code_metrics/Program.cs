using System;

namespace Code_metrics
{
    class Program
    {
        static void Main(string[] args)
        {
            App();
        }

        static void App()
        {
            var analyzer = new Analyzer();
            analyzer.Read(@"D:\Studing\University\!Магистратура\Хицко\Лабы\App_Code.zip");
            analyzer.CalculateAllMetrics();
            analyzer.Print();
        }    
    }
}
