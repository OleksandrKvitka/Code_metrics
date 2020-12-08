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
            analyzer.Read();

        }    
    }
}
