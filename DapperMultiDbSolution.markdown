##### DapperMultiDbLibrary 项目

**DapperMultiDbLibrary.csproj**:
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <LangVersion>preview</LangVersion>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Dapper" Version="2.0.123" />
  </ItemGroup>
</Project>
```

**IDatabaseAccessor.cs**:
```csharp
using System.Data;
using System.Collections.Generic;

// 数据库访问器接口
public interface IDatabaseAccessor
{
    // 执行查询并返回结果集
    IEnumerable<T> Query<T>(string sql, object param = null, CommandType? commandType = null);
    // 执行 SQL 语句并返回受影响的行数
    int Execute(string sql, object param = null, CommandType? commandType = null);
}
```

**DapperDatabaseAccessor.cs**:
```csharp
using Dapper;
using System.Data;
using System.Collections.Generic;

// 使用 Dapper 实现的数据库访问器
public class DapperDatabaseAccessor : IDatabaseAccessor
{
    private readonly IDbConnection _connection;

    // 构造函数，接受 IDbConnection
    public DapperDatabaseAccessor(IDbConnection connection)
    {
        _connection = connection;
    }

    // 查询方法，返回结果集
    public IEnumerable<T> Query<T>(string sql, object param = null, CommandType? commandType = null)
    {
        return _connection.Query<T>(sql, param, commandType: commandType);
    }

    // 执行 SQL 方法，返回受影响的行数
    public int Execute(string sql, object param = null, CommandType? commandType = null)
    {
        return _connection.Execute(sql, param, commandType: commandType);
    }
}
```

**Models/User.cs**:
```csharp
// 用户模型
public class User
{
    // 用户 ID
    public int Id { get; set; }
    // 用户名称
    public string Name { get; set; }
}
```

##### DapperMultiDb.Examples 项目

**DapperMultiDb.Examples.csproj**:
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <LangVersion>preview</LangVersion>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="..\DapperMultiDbLibrary\DapperMultiDbLibrary.csproj" />
    <PackageReference Include="MySql.Data" Version="8.0.31" />
    <PackageReference Include="Oracle.ManagedDataAccess.Core" Version="2.20.4" />
    <PackageReference Include="Microsoft.Data.Sqlite" Version="5.0.10" />
    <PackageReference Include="Npgsql" Version="6.0.6" />
    <PackageReference Include="Microsoft.Extensions.Configuration" Version="7.0.0" />
    <PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="7.0.0" />
  </ItemGroup>
</Project>
```

**Program.cs**:
```csharp
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using Microsoft.Data.Sqlite;
using Npgsql;
using System.Data.SqlClient;

// 示例程序，展示不同数据库的访问
class Program
{
    static void Main(string[] args)
    {
        // 加载配置文件
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        Console.WriteLine("MySQL 示例：");
        MySqlExample(configuration);

        Console.WriteLine("\nOracle 示例：");
        OracleExample(configuration);

        Console.WriteLine("\nSQL Server 示例：");
        SqlServerExample(configuration);

        Console.WriteLine("\nSQLite 示例：");
        SqliteExample(configuration);

        Console.WriteLine("\nPostgreSQL 示例：");
        PostgreSqlExample(configuration);
    }

    // MySQL 示例
    static void MySqlExample(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MySql");
        using var connection = new MySqlConnection(connectionString);
        var dbAccessor = new DapperDatabaseAccessor(connection);
        // 查询用户表
        var users = dbAccessor.Query<User>("SELECT * FROM Users");
        foreach (var user in users)
        {
            Console.WriteLine($"Id: {user.Id}, Name: {user.Name}");
        }
    }

    // Oracle 示例，包括存储过程、CLOB 和 Sys_refcursor
    static void OracleExample(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Oracle");
        using var connection = new OracleConnection(connectionString);
        var dbAccessor = new DapperDatabaseAccessor(connection);

        // 调用存储过程，假设返回用户列表
        var usersFromProc = dbAccessor.Query<User>("pkg_MyPackage.GetUsers", commandType: CommandType.StoredProcedure);
        foreach (var user in usersFromProc)
        {
            Console.WriteLine($"Id: {user.Id}, Name: {user.Name}");
        }

        // 获取 CLOB 数据
        var clobData = dbAccessor.Query<string>("SELECT Description FROM MyTable WHERE Id = 1").FirstOrDefault();
        Console.WriteLine($"CLOB 数据: {clobData}");

        // 执行 SQL 查询
        var queryResult = dbAccessor.Query<User>("SELECT * FROM Users WHERE Id = 1");
        foreach (var user in queryResult)
        {
            Console.WriteLine($"Id: {user.Id}, Name: {user.Name}");
        }
    }

    // SQL Server 示例
    static void SqlServerExample(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SqlServer");
        using var connection = new SqlConnection(connectionString);
        var dbAccessor = new DapperDatabaseAccessor(connection);
        // 查询用户表
        var users = dbAccessor.Query<User>("SELECT * FROM Users");
        foreach (var user in users)
        {
            Console.WriteLine($"Id: {user.Id}, Name: {user.Name}");
        }
    }

    // SQLite 示例
    static void SqliteExample(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SQLite");
        using var connection = new SqliteConnection(connectionString);
        var dbAccessor = new DapperDatabaseAccessor(connection);
        // 查询用户表
        var users = dbAccessor.Query<User>("SELECT * FROM Users");
        foreach (var user in users)
        {
            Console.WriteLine($"Id: {user.Id}, Name: {user.Name}");
        }
    }

    // PostgreSQL 示例
    static void PostgreSqlExample(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PostgreSql");
        using var connection = new NpgsqlConnection(connectionString);
        var dbAccessor = new DapperDatabaseAccessor(connection);
        // 查询用户表，注意表名大小写
        var users = dbAccessor.Query<User>("SELECT * FROM users");
        foreach (var user in users)
        {
            Console.WriteLine($"Id: {user.Id}, Name: {user.Name}");
        }
    }
}
```

**appsettings.json**:
```json
{
  "ConnectionStrings": {
    "MySql": "Server=localhost;Database=mydb;User Id=myuser;Password=mypassword;",
    "Oracle": "Data Source=oracle_server;User Id=oracle_user;Password=oracle_password;",
    "SqlServer": "Server=(localdb)\\mssqllocaldb;Database=mydb;Trusted_Connection=True;",
    "SQLite": "Data Source=mydb.sqlite",
    "PostgreSql": "Host=localhost;Database=mydb;Username=postgres;Password=mypassword;"
  }
}
```