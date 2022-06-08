using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Struct.Common.Utils
{
    public class ChemUtils
    {
        /// <summary>
        /// 判断是不是mol
        /// </summary>
        /// <param name="chemContent"></param>
        /// <returns></returns>
        public static bool IsMolContent(string chemContent)
        {
            return chemContent.Contains("V3000") || chemContent.Contains("V2000");
        }

        /// <summary>
        /// 过滤H
        /// </summary>
        /// <param name="molContent"></param>
        /// <returns></returns>
        public static string FilterHCount(string molContent)
        {
            List<string> arr = molContent.Split('\n').ToList();
            for (int i = 0; i < arr.Count; i++)
            {
                arr[i] = Regex.Split(arr[i], "HCOUNT=")[0];
            }
            return string.Join("\n", arr);
        }

        /// <summary>
        /// 将结构式的符号替换
        /// </summary>
        /// <param name="chemContent"></param>
        /// <returns></returns>
        public static string ReplaceChemContent(string chemContent)
        {
            return chemContent.Replace(">>", ".");
        }
    }
}
