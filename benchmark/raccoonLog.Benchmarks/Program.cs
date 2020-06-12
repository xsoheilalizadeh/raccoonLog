using BenchmarkDotNet.Running;

namespace raccoonLog.Benchmarks
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // var benchmark = new MessageFactoryBenchmarks();
            //
            // benchmark.SetUp();
            //
            // benchmark.CreateRequestLog();
            //
            // Console.ReadKey();

            BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }
}