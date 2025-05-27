using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.IO;

class JsonRpcRequest
{
    public string jsonrpc { get; set; } = "2.0";
    public string method { get; set; }
    public JsonObject @params { get; set; }
    public int id { get; set; }
}

class JsonRpcResponse
{
    public string jsonrpc { get; set; } = "2.0";
    public JsonElement? result { get; set; }
    public int id { get; set; }
}

class Program
{
    private static readonly string LogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error.log");
    private static readonly string AccessLogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "access.log");

    static async Task Main()
    {
        try
        {
            // 记录启动日志
            var startLog = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] 程序启动\n" +
                          $"工作目录: {AppDomain.CurrentDomain.BaseDirectory}\n" +
                          new string('-', 80) + "\n";
            File.AppendAllText(AccessLogPath, startLog);

            var clientTransport = new StdioClientTransport(new StdioClientTransportOptions
            {
                Name = "MyServer",
                Command = @"D:\GitSource\demo\ConsoleMCPServer\bin\Debug\net8.0\ConsoleMCPServer.exe",
                Arguments = Array.Empty<string>(),
            });

            var client = await McpClientFactory.CreateAsync(clientTransport);
            // 记录连接成功日志
            var connectLog = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] 服务器连接成功\n" +
                           new string('-', 80) + "\n";
            File.AppendAllText(AccessLogPath, connectLog);

            PromptForInput();
            while (Console.ReadLine() is string line && !"exit".Equals(line, StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    PromptForInput();
                    continue;
                }

                // 记录请求日志
                var requestLog = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] 收到请求: {line}\n";
                File.AppendAllText(AccessLogPath, requestLog);

                try
                {
                    var request = JsonSerializer.Deserialize<JsonRpcRequest>(line);

                    // 根据请求类型调用相应的服务器方法
                    if (request.method == "initialize")
                    {
                        var protocolVersion = "2024-11-05";//目前MCP协议的版本，暂时没有找到SDK哪里有标注版本号，只能手动写死
                        var serverResponse = new
                        {
                            experimental = new { },
                            logging = new { },
                            prompts = new { },
                            resources = new { },
                            tools = new { listChanged = true },
                            completions = new { }
                        };
                        var serverInfo = client.ServerInfo;

                        // 将服务器响应转换为JSON-RPC响应
                        var response = new JsonRpcResponse
                        {
                            id = request.id,
                            result = JsonSerializer.Deserialize<JsonElement>(
                                JsonSerializer.Serialize(new
                                {
                                    protocolVersion = protocolVersion,
                                    capabilities = serverResponse,
                                    serverInfo = serverInfo
                                })
                            )
                        };
                        // 记录成功响应日志
                        var successLog = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] 响应成功: {JsonSerializer.Serialize(response)}\n" +
                                       new string('-', 80) + "\n";
                        File.AppendAllText(AccessLogPath, successLog);

                        // 输出响应
                        Console.WriteLine(JsonSerializer.Serialize(response));
                    }
                    else if (request.method == "tools/list")
                    {
                        // 获取服务器工具列表
                        var tools = await client.ListToolsAsync();

                        // 将工具列表转换为标准格式
                        var formattedTools = tools.Select(tool => new
                        {
                            name = tool.Name,
                            description = tool.Description,
                            inputSchema = tool.JsonSchema
                        }).ToList();

                        // 将工具列表转换为JSON-RPC响应
                        var response = new JsonRpcResponse
                        {
                            id = request.id,
                            result = JsonSerializer.Deserialize<JsonElement>(
                                JsonSerializer.Serialize(new { tools = formattedTools })
                            )
                        };

                        // 记录成功响应日志
                        var successLog = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] 响应成功: {JsonSerializer.Serialize(response)}\n" +
                                       new string('-', 80) + "\n";
                        File.AppendAllText(AccessLogPath, successLog);

                        // 输出响应
                        Console.WriteLine(JsonSerializer.Serialize(response));
                    }
                    else if (request.method == "tools/call")
                    {
                        // 从请求中获取工具名称和参数
                        string toolName = request.@params["name"]?.ToString();
                        var arguments = request.@params["arguments"]?.Deserialize<Dictionary<string, object?>>();

                        // 调用服务器工具
                        var result = await client.CallToolAsync(
                            toolName,
                            arguments,
                            cancellationToken: CancellationToken.None
                        );

                        // 将工具调用结果转换为JSON-RPC响应
                        var response = new JsonRpcResponse
                        {
                            id = request.id,
                            result = JsonSerializer.Deserialize<JsonElement>(
                                JsonSerializer.Serialize(new { content = result.Content })
                            )
                        };

                        // 记录成功响应日志
                        var successLog = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] 响应成功: {JsonSerializer.Serialize(response)}\n" +
                                       new string('-', 80) + "\n";
                        File.AppendAllText(AccessLogPath, successLog);

                        // 输出响应
                        Console.WriteLine(JsonSerializer.Serialize(response));
                    }
                    else if (request.method == "notifications/initialized")
                    {
                        //预留后续处理
                    }
                    else
                    {
                        // 处理未知方法
                        var response = new JsonRpcResponse
                        {
                            id = request.id,
                            result = JsonSerializer.Deserialize<JsonElement>(
                                JsonSerializer.Serialize(new { code = -32601, message = "Method not found" })
                            )
                        };

                        // 记录错误日志
                        var errorLog = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Error: Method not found\n" +
                                     $"Request: {line}\n" +
                                     $"Response: {JsonSerializer.Serialize(response)}\n" +
                                     new string('-', 80) + "\n";
                        File.AppendAllText(LogPath, errorLog);

                        // 输出响应
                        Console.WriteLine(JsonSerializer.Serialize(response));
                    }
                }
                catch (Exception ex)
                {
                    // 处理异常
                    var response = new JsonRpcResponse
                    {
                        id = 0,
                        result = JsonSerializer.Deserialize<JsonElement>(
                            JsonSerializer.Serialize(new { code = -32603, message = ex.Message })
                        )
                    };

                    // 记录错误到日志文件
                    var errorLog = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Error: {ex.Message}\n" +
                                 $"Request: {line}\n" +
                                 $"Stack Trace: {ex.StackTrace}\n" +
                                 $"Response: {JsonSerializer.Serialize(response)}\n" +
                                 new string('-', 80) + "\n";

                    File.AppendAllText(LogPath, errorLog);

                    // 输出响应
                    Console.WriteLine(JsonSerializer.Serialize(response));
                }

                PromptForInput();
            }

            // 记录退出日志
            var exitLog = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] 程序正常退出\n" +
                         new string('-', 80) + "\n";
            File.AppendAllText(AccessLogPath, exitLog);
        }
        catch (Exception ex)
        {
            // 记录启动错误日志
            var startupErrorLog = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] 程序启动失败\n" +
                                $"Error: {ex.Message}\n" +
                                $"Stack Trace: {ex.StackTrace}\n" +
                                new string('-', 80) + "\n";
            File.AppendAllText(LogPath, startupErrorLog);
            throw;
        }
    }

    static void PromptForInput()
    {
        Console.WriteLine("请输入JSON-RPC请求（输入 'exit' 退出）：");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("> ");
        Console.ResetColor();
    }
}
