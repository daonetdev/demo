using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace APITEST
{
    public static class Utils
    {
        public static string MD5Hash(string input)
        {
            using (var md5 = MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hash = md5.ComputeHash(inputBytes);

                var sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public static bool Compare<T>(string op, T value1, T value2)
            where T : IComparable<T>
        {
            switch (op)
            {
                case ">":
                    return value1.CompareTo(value2) > 0;
                case "<":
                    return value1.CompareTo(value2) < 0;
                case "=":
                default:
                    {
                        return value1.CompareTo(value2) == 0;
                    }
            }
        }

        /// <summary>
        /// Replace characters for Javscript string literals
        /// </summary>
        /// <param name="text">raw string</param>
        /// <returns>escaped string</returns>
        public static string EscapeStringForJS(string s)
        {
            //REF: http://www.javascriptkit.com/jsref/escapesequence.shtml
            //	FROM: http://blog.darkthread.net/blogs/darkthreadtw/archive/2009/09/04/5258.aspx
            return s.Replace(@"\", @"\\")
                    .Replace("\b", @"\b")
                    .Replace("\f", @"\f")
                    .Replace("\n", @"\n")
                    .Replace("\0", @"\0")
                    .Replace("\r", @"\r")
                    .Replace("\t", @"\t")
                    .Replace("\v", @"\v")
                    .Replace("'", @"\'")
                    .Replace(@"""", @"\""");
        }
        /// <summary>
        /// Gets the week of the month represented by this instance.
        /// Def : 以1號~7號為第一週，以此類推，不受星期影響
        /// </summary>
        /// <param name="val">the value which type is datetime</param>
        /// <returns></returns>
        public static int WeekOfMonth(this DateTime val)
        {
            return (val.Day - 1) / 7 + 1;
        }
        /// <summary>
        /// Gets the value from the key in dictionary.If the key is not exist,then return default value
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict">the dictionary</param>
        /// <param name="key">the key</param>
        /// <param name="dflVal">default value</param>
        /// <returns></returns>
        public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue dflVal = default(TValue))
        {
            var val = dflVal;

            if (dict == null) return val;

            if (dict.ContainsKey(key))
            {
                val = dict[key];
            }

            return val;
        }

        //public static MemoryStream ToZip(List<XLWorkbook> workbooks, int zipLevel = 3)
        //{
        //    var result = new MemoryStream();

        //    using (var zipOutputStream = new ZipOutputStream(result))
        //    {
        //        zipOutputStream.SetLevel(zipLevel);
        //        foreach (var workbook in workbooks)
        //        {
        //            using (var ms = new MemoryStream())
        //            {
        //                workbook.SaveAs(ms);
        //                ms.Seek(0, SeekOrigin.Begin);
        //                var zipEntry = new ZipEntry(workbook.Properties.Title + ".xlsx");
        //                zipOutputStream.PutNextEntry(zipEntry);
        //                StreamUtils.Copy(ms, zipOutputStream, new byte[65536]);

        //            }
        //            zipOutputStream.CloseEntry();
        //        }
        //        zipOutputStream.IsStreamOwner = false;
        //    }
        //    result.Seek(0, SeekOrigin.Begin);

        //    return result;
        //}
    }
}
