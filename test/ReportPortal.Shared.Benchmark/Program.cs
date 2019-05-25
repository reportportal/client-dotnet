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
        }
    }
}
