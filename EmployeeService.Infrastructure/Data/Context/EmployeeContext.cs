using EmployeeService.Domain.EmployeeDemographics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using EmployeeService.Domain.Location;

namespace EmployeeService.Infrastructure.Data.Context
{
    public sealed class EmployeeContext : DbContext
    {
        public EmployeeContext()
        {
        }

        public EmployeeContext(DbContextOptions<EmployeeContext> options, ILogger<EmployeeContext> logger)
            : base(options)
        {
            SeedCosmosData();
        }

        private DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower() == "development")
            {
                optionsBuilder.EnableSensitiveDataLogging();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultContainer("Employee");
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.OwnsOne(e => e.HomeCity, sa =>
                {
                    sa.ToJsonProperty("HomeCity");
                    sa.OwnsOne(s => s.State, ssa =>
                    {
                        ssa.ToJsonProperty("State");
                        ssa.OwnsOne(d => d.Country, csa => { csa.ToJsonProperty("Country"); });
                    });
                });
                entity.OwnsOne(e => e.BirthCity, sa =>
                {
                    sa.ToJsonProperty("BirthCity");
                    sa.OwnsOne(s => s.State, ssa =>
                    {
                        ssa.ToJsonProperty("State");
                        ssa.OwnsOne(d => d.Country, csa => { csa.ToJsonProperty("Country"); });
                    });
                });

                entity.HasPartitionKey(e => e.Id);
                entity.Property(p => p.Id).ToJsonProperty("id");

                entity.HasIndex(e => new { e.CompanyId, e.ClientId })
                    .IsUnique();
            });
        }

        private void SeedCosmosData()
        {
            try
            {
                if (Employees.AsNoTracking().FirstOrDefault() == null)
                {
                    var random = new Random();
                    var count = 3;

                    for (var i = 1; i < count; i++)
                    {
                        var fixture = $"000{i}";
                        Employees.Add(new Employee
                        {
                            Id = i.ToString(),
                            ClientId = $"CTI{fixture}",
                            SelectedFName = $"EmployeeFName{i}",
                            SelectedSurname = $"EmployeeSurname{i}",
                            SocialInsNumber = $"SIN{fixture}",
                            DeleteFlag = "N",
                            DisabledInd = "N",
                            GenderInd = "M",
                            LastUpdtDate = DateTime.Now,
                            LastUpdtUserId = $"user{i}",
                            CompanyId = "NID",
                            HomeCity = GenerateCity(random.Next(1, 3)),
                            BirthCity = GenerateCity(random.Next(1, 3))
                        });
                    }

                    SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}  - {ex.StackTrace}");
            }
        }

        private City GenerateCity(int seed)
        {
            switch (seed)
            {
                default:
                    return new City
                    {
                        Id = Guid.NewGuid().ToString(), CityName = "Miami",
                        State = new State
                        {
                            Id = Guid.NewGuid().ToString(), StateCode = "FL", StateName = "Florida",
                            Country = new Country()
                            {
                                Id = Guid.NewGuid().ToString(), CountryCode = "US", CountryName = "USA"
                            }
                        }
                    };
                case 2:
                    return new City
                    {
                        Id = Guid.NewGuid().ToString(), CityName = "Boston",
                        State = new State
                        {
                            Id = Guid.NewGuid().ToString(), StateCode = "MS", StateName = "Massachusetts",
                            Country = new Country()
                            {
                                Id = Guid.NewGuid().ToString(), CountryCode = "US", CountryName = "USA"
                            }
                        }
                    };
                case 3:
                    return new City
                    {
                        Id = Guid.NewGuid().ToString(), CityName = "Toronto",
                        State = new State
                        {
                            Id = Guid.NewGuid().ToString(), StateCode = "ON", StateName = "Ontario",
                            Country = new Country()
                            {
                                Id = Guid.NewGuid().ToString(), CountryCode = "CA", CountryName = "Canada"
                            }
                        }
                    };
            }
        }
    }
}