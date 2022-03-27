# Introduction 
Employee Microservice PoC 

# Prerequisites:
- Update Visual studio 2019 to version 16.8.2 or later - This version will allow you to use .Net 5
- Download .Net5 SDK: https://dotnet.microsoft.com/download/dotnet/5.0
- Add the local Nuget Package source as it is needed for now while we put that in the DEV/Release Nuget repo.

# Learning Material 
- Mediatr and CQRS: https://www.youtube.com/watch?v=YzOBrVlthMk
- Onion Architecture: https://www.codewithmukesh.com/blog/onion-architecture-in-aspnet-core/ 
- Dapper: https://dapper-tutorial.net/
- Dependency injection in .Net 5: https://auth0.com/blog/dependency-injection-in-dotnet-core/
- Serilog: https://www.codewithmukesh.com/blog/serilog-in-aspnet-core-3-1/
- Setting up JWT: https://www.youtube.com/watch?v=M6AkbBaDGJE 
- Unit tests using Moq: https://www.youtube.com/watch?v=9ZvDBSQa_so 
- ActiveMQ Message send/receive: http://activemq.apache.org/components/nms/documentation 
- NSubstitute: https://nsubstitute.github.io/
- AutoFixture: https://github.com/AutoFixture/AutoFixture
- FluentAssertions: https://fluentassertions.com/

Additional/optional material:
- Versioning APIs and Handlers: https://www.youtube.com/watch?v=WFEE5yVJwGU
- BenchmarkDotNet: https://www.youtube.com/watch?v=EWmufbVF2A4 

# Features
It is a fully functional service (only for Employee table for now). It has the following features:

- Uses latest .Net version .Net5 (former .net core)
- Based on the Onion Architecture structure
- Uses ActiveMQ as service bus and [NID.ActiveMQ.Core](https://dev.azure.com/LifeAchievements/Employee/_git/NID.ActiveMQ.Core?version=GBmaster) middleware
- Uses SQL Server Cache (replacing Azure redis Cache for on-premise version)
- Uses Dapper
- Uses Serilog
- Uses Swagger
- Uses Automapper for mapping DTOs
- Uses BenchmarkDotNet for performance testing
- Integration Test for event subscribing for ActiveMQ
- Unit Tests using MS Test with NSubstitute, AutoFixture and fluentAssertions

# Missing Features

- Cache levels (short, mid, long, infinite)
- JWT Authentication
