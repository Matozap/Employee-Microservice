``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1198 (1909/November2018Update/19H2)
Intel Xeon CPU E5-2686 v4 2.30GHz, 1 CPU, 8 logical and 8 physical cores
.NET Core SDK=5.0.101
  [Host]     : .NET Core 5.0.1 (CoreCLR 5.0.120.57516, CoreFX 5.0.120.57516), X64 RyuJIT
  Job-CYVKDA : .NET Core 5.0.1 (CoreCLR 5.0.120.57516, CoreFX 5.0.120.57516), X64 RyuJIT

IterationCount=5  LaunchCount=1  RunStrategy=Throughput  
WarmupCount=1  

```
|                 Method |        Mean |       Error |    StdDev |
|----------------------- |------------:|------------:|----------:|
|     GetAllEmployeesX10 |    177.6 ms |     5.09 ms |   0.79 ms |
|    GetAllEmployeesX100 |    200.7 ms |     9.93 ms |   1.54 ms |
|   GetAllEmployeesX1000 |    501.1 ms |    89.48 ms |  23.24 ms |
|  GetAllEmployeesX10000 |  4,182.8 ms | 1,412.58 ms | 218.60 ms |
| GetAllEmployeesX100000 | 18,680.9 ms | 2,604.78 ms | 676.45 ms |
