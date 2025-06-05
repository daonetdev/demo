using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DeleteFoldersDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            Console.WriteLine("正在获取当前目录下的所有子目录...");
            string[] subDirectories = Directory.GetDirectories(currentDirectory);

            Console.WriteLine("准备删除以下目录：");
            foreach (string dir in subDirectories)
            {
                string dirName = Path.GetFileName(dir);
                if (dirName == ".vs" || dirName == "bin" || dirName == "obj")
                {
                    Console.WriteLine(dir);
                }
            }

            Console.Write("是否确认删除上述目录？(y/n): ");
            string input = Console.ReadLine();

            if (input.ToLower() == "y")
            {
                foreach (string dir in subDirectories)
                {
                    string dirName = Path.GetFileName(dir);
                    if (dirName == ".vs" || dirName == "bin" || dirName == "obj")
                    {
                        try
                        {
                            Directory.Delete(dir, true);
                            Console.WriteLine($"已删除目录: {dir}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"删除目录 {dir} 时出错: {ex.Message}");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("取消删除操作。");
            }

            Console.WriteLine("删除操作已完成。");
            Console.Write("是否删除本程式本身？(y/n): ");
            input = Console.ReadLine();

            if (input.ToLower() == "y")
            {
                try
                {
                    //string currentExePath = System.Reflection.Assembly.GetEntryAssembly().Location;
                    //File.Delete(currentExePath);
                    DeleteItself();
                    //Console.WriteLine("程式已删除。");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"删除程式时出错: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("保留程式。");
                Console.WriteLine("操作结束，按任意键退出...");
                Console.ReadKey();
            }
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern uint WinExec(string lpCmdLine, uint uCmdShow);
        /// <summary>
        /// 删除程序自身 方式2
        /// </summary>
        private static void DeleteItself()
        {
            string vBatFile = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\DeleteItself.bat";
            using (StreamWriter vStreamWriter = new StreamWriter(vBatFile, false, Encoding.Default))
            {
                vStreamWriter.Write(string.Format(
                    ":del\r\n" +
                    " del \"{0}\"\r\n" +
                    "if exist \"{0}\" goto del\r\n" +
                    "del %0\r\n", System.Reflection.Assembly.GetEntryAssembly().Location));
            }

            //************ 执行批处理
            WinExec(vBatFile, 0);
            
            //************ 结束退出
            //Application.Exit();
        }
    }
}