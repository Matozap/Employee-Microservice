using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using BenchmarkDotNet.Engines;

namespace EmployeeService.Benchmark
{
    [ExcludeFromCodeCoverage]
    [MemoryDiagnoser]
    [TailCallDiagnoser]
    [ThreadingDiagnoser]
    [SimpleJob(runStrategy:RunStrategy.Throughput,launchCount:1,warmupCount:1,targetCount:20)]
    public class ApplicationBenchmark
    {
        [Params(true)]
        public bool local;
        
        [Benchmark]
        public string GetAllEmployeesX10()
        {
            return GetEmployees(local, 0, 10);
        }

        [Benchmark]
        public string GetAllEmployeesX100()
        {
            return GetEmployees(local, 0, 100);
        }

        [Benchmark]
        public string GetAllEmployeesX1000()
        {
            return GetEmployees(local, 0, 1000);
        }

        [Benchmark]
        public string GetAllEmployeesX10000()
        {
            return GetEmployees(local, 0, 10000);
        }
        
        
        private static string GetEmployees(bool local, int from, int to)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            ServicePointManager.DefaultConnectionLimit = 10000;
            using var client = new HttpClient();
            var url = local ? $"https://localhost:5001/api/employee/{from}/{to}" : $"https://lv0ny0ptl4.execute-api.us-east-2.amazonaws.com/Prod/api/employee/{from}/{to}";
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var requestUri = new Uri(url, UriKind.Absolute);
            var response = Task.Run(() => client.GetAsync(requestUri));
            response.Wait();
            var result = Task.Run(() => response.Result.Content.ReadAsStringAsync());
            result.Wait();
            return result.Result;
        } 
    }
}
