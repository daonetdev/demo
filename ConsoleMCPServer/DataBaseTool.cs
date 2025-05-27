using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Text.Json;
using ModelContextProtocol.Server;
using System.IO;
using System.Linq;

namespace ConsoleMCPServer
{

    [McpServerToolType]
    public static class DatabaseTool
    {
        private static string GetMockData()
        {
            var dataSet = new System.Data.DataSet();
            var table = new System.Data.DataTable("UserTable");
            table.Columns.Add("ID", typeof(int));
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Email", typeof(string));
            table.Rows.Add(1, "张三", "zhangsan@example.com");
            table.Rows.Add(2, "李四", "lisi@example.com");
            dataSet.Tables.Add(table);
            var result = new
            {
                tableName = table.TableName,
                columns = table.Columns.Cast<System.Data.DataColumn>()
                    .Select(c => new { name = c.ColumnName, type = c.DataType.Name })
                    .ToList(),
                rows = table.Rows.Cast<System.Data.DataRow>()
                    .Select(r => table.Columns.Cast<System.Data.DataColumn>()
                        .ToDictionary(c => c.ColumnName, c => r[c]))
                    .ToList()
            };
            return JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        }

        [McpServerTool, Description("Query Oracle database and return results (only simple SELECT queries are supported)")]
        public static string QueryDatabase(
            [Description("The SQL statement to execute")] string sql)
        {
            try
            {
                var sb = new StringBuilder();
                sb.AppendLine($"Executing SQL: {sql}");

                // 调用模拟数据方法
                return GetMockData();
            }
            catch (Exception ex)
            {
                // 日志路径
                var logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error.log");
                var errorLog = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Error executing SQL: {sql}\n" +
                               $"Exception: {ex.Message}\n" +
                               $"StackTrace: {ex.StackTrace}\n" +
                               new string('-', 80) + "\n";
                File.AppendAllText(logPath, errorLog);

                // 返回错误信息
                return JsonSerializer.Serialize(new { error = ex.Message });
            }
        }
    }
}
