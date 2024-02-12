using BenchmarkDotNet.Running;
using ReportPortal.Shared.Benchmark.Reporter;
using System;

namespace ReportPortal.Shared.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<ReporterBenchmark>();

            // or local
            //var l = new ReporterBenchmark();
            //l.SuitesCount = 100000;

            //Console.WriteLine("Continue?");
            //Console.ReadLine();

            //l.LaunchReporter();

            //Console.WriteLine("Continue?");
            //Console.ReadLine();
        }
    }
}
