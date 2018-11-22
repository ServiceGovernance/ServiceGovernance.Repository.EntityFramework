# ServiceGovernance.Repository.EntityFramework

[![Build status](https://ci.appveyor.com/api/projects/status/4tghcdgi4dp2u5t5/branch/master?svg=true)](https://ci.appveyor.com/project/twenzel/servicegovernance-repository-entityframework/branch/master)
[![NuGet Version](http://img.shields.io/nuget/v/ServiceGovernance.Repository.EntityFramework.svg?style=flat)](https://www.nuget.org/packages/ServiceGovernance.Repository.EntityFramework/)
[![License](https://img.shields.io/badge/license-Apache-blue.svg)](LICENSE)

Persistance library for [ServiceRepository](https://github.com/ServiceGovernance/ServiceGovernance.Repository) using EntityFramework.

## Usage

```CSharp
public void ConfigureServices(IServiceCollection services)
{
    var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

    services.AddServiceRepository()
        // this adds the persistence to EF
        .AddRepositoryStore(options =>
        {
            options.ConfigureDbContext = builder =>
                builder.UseSqlServer(Configuration.GetConnectionString("default"),
                    sql => sql.MigrationsAssembly(migrationsAssembly));
        });
}
```

