using BenchmarkDotNet.Running;

namespace raccoonLog.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
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