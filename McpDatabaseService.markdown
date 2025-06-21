### McpDatabaseService 项目

#### McpDatabaseService.csproj
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <ProjectReference Include="..\DapperMultiDbLibrary\DapperMultiDbLibrary.csproj" />
    <PackageReference Include="ModelContextProtocol" Version="0.1.0-preview.1" />
    <PackageReference Include="Microsoft.Extensions.Hosting" Version="7.0.0" />
    <PackageReference Include="Microsoft.Extensions.Configuration" Version="7.0.0" />
    <PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="7.0.0" />
    <PackageReference Include="Microsoft.Data.SqlClient" Version="5.2.1" />
  </ItemGroup>
</Project>
```

#### Program.cs
```csharp
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ModelContextProtocol;
using DapperMultiDbLibrary;

// 创建主机
var builder = Host.CreateApplicationBuilder(args);

// 配置日志
builder.Logging.AddConsole(options =>
{
    options.LogToStandardErrorThreshold = LogLevel.Trace;
});

// 添加 MCP 服务器，使用 stdio 传输
builder.Services.AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly(typeof(DatabaseTools).Assembly);

// 添加数据库访问器（示例：SQL Server）
builder.Services.AddSingleton<IDatabaseAccessor>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("SqlServer");
    return new SqlServerClient(connectionString);
});

// 构建并运行主机
var host = builder.Build();
await host.RunAsync();
```

#### DatabaseTools.cs
```csharp
using DapperMultiDbLibrary;
using ModelContextProtocol;
using System.Collections.Generic;

[McpServerToolType]
public class DatabaseTools
{
    private readonly IDatabaseAccessor _dbAccessor;

    public DatabaseTools(IDatabaseAccessor dbAccessor)
    {
        _dbAccessor = dbAccessor;
    }

    [McpServerTool(Description = "获取所有用户")]
    public IEnumerable<User> GetUsers()
    {
        return _dbAccessor.Query<User>("SELECT * FROM Users");
    }

    [McpServerTool(Description = "根据 ID 获取用户")]
    public User GetUserById(int id)
    {
        return _dbAccessor.Query<User>("SELECT * FROM Users WHERE Id = @Id", new { Id = id }).FirstOrDefault();
    }
}
```

#### appsettings.json
```json
{
  "ConnectionStrings": {
    "SqlServer": "Server=(localdb)\\mssqllocaldb;Database=mydb;Trusted_Connection=True;"
  }
}
```

### DapperMultiDbLibrary 项目（参考）
以下是 `DapperMultiDbLibrary` 的关键代码，确保与 MCP 服务兼容。

#### IDatabaseAccessor.cs
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

#### DapperDatabaseAccessor.cs
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

#### DatabaseClient.cs
```csharp
using System;
using System.Data;
using System.Collections.Generic;
using Dapper;

// 抽象数据库客户端类
public abstract class DatabaseClient
{
    protected readonly string _connectionString;
    protected readonly Func<string, IDbConnection> _connectionFactory;

    protected DatabaseClient(string connectionString, Func<string, IDbConnection> connectionFactory)
    {
        _connectionString = connectionString;
        _connectionFactory = connectionFactory;
    }

    // 查询方法
    public IEnumerable<T> Query<T>(string sql, object param = null, CommandType? commandType = null)
    {
        using var connection = _connectionFactory(_connectionString);
        return connection.Query<T>(sql, param, commandType: commandType);
    }

    // 执行 SQL 方法
    public int Execute(string sql, object param = null, CommandType? commandType = null)
    {
        using var connection = _connectionFactory(_connectionString);
        return connection.Execute(sql, param, commandType: commandType);
    }
}
```

#### SqlServerClient.cs
```csharp
using Microsoft.Data.SqlClient;
using System.Data;

// SQL Server 数据库客户端
public class SqlServerClient : DatabaseClient
{
    public SqlServerClient(string connectionString) : base(connectionString, connStr => new SqlConnection(connStr)) { }
}
```

#### User.cs
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