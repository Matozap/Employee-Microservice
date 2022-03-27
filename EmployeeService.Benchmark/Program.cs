using System;
using System.Diagnostics.CodeAnalysis;
using BenchmarkDotNet.Running;

namespace EmployeeService.Benchmark
{
    [ExcludeFromCodeCoverage]
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<ApplicationBenchmark>();
            Console.ReadLine();
        }
    }
}
