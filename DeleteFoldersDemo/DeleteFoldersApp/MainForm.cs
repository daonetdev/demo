using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeleteVsDevFoldersApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // 获取当前程序所在目录
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // 收集所有符合条件的目录（递归查找）
            var directoriesToDelete = new System.Collections.Generic.List<string>();
            FindDirectories(currentDirectory, directoriesToDelete);

            // 显示要删除的目录
            string message = "以下目录将被删除：\n";
            foreach (string dir in directoriesToDelete)
            {
                message += dir + "\n";
            }

            // 弹出确认对话框
            DialogResult result = MessageBox.Show(message, "确认删除", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                foreach (string dir in directoriesToDelete)
                {
                    try
                    {
                        Directory.Delete(dir, true);
                        Console.WriteLine($"已删除目录: {dir}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("删除目录失败：" + ex.Message);
                    }
                }
            }

            // 删除完成后提示是否删除自身
            result = MessageBox.Show("是否删除本程式本身？", "确认操作", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                try
                {
                    //string currentExePath = System.Reflection.Assembly.GetEntryAssembly().Location;
                    //File.Delete(currentExePath);
                    DeleteItself();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("删除本程式失败：" + ex.Message);
                }
            }

            // 关闭窗体
            this.Close();
        }

        // 递归查找所有符合条件的子目录
        private void FindDirectories(string path, System.Collections.Generic.List<string> directories)
        {
            try
            {
                // 获取当前目录下的所有子目录
                foreach (string dir in Directory.GetDirectories(path))
                {
                    string dirName = Path.GetFileName(dir);
                    if (dirName == ".vs" || dirName == "bin" || dirName == "obj")
                    {
                        directories.Add(dir);
                    }

                    // 递归查找子目录下的目录
                    FindDirectories(dir, directories);
                }
            }
            catch (Exception)
            {
                // 忽略错误，如权限不足等
            }
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern uint WinExec(string lpCmdLine, uint uCmdShow);
        /// <summary>
        /// 删除程序自身 方式2
        /// </summary>
        private static void DeleteItself()
        {
            string vBatFile = Path.GetDirectoryName(Application.ExecutablePath) + "\\DeleteItself.bat";
            using (StreamWriter vStreamWriter = new StreamWriter(vBatFile, false, Encoding.Default))
            {
                vStreamWriter.Write(string.Format(
                    ":del\r\n" +
                    " del \"{0}\"\r\n" +
                    "if exist \"{0}\" goto del\r\n" +
                    "del %0\r\n", Application.ExecutablePath));
            }

            //************ 执行批处理
            WinExec(vBatFile, 0);
            //************ 结束退出
            Application.Exit();
        }
    }
}
