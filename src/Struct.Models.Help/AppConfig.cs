using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Struct.Models.Help
{
    public class AppConfig
    {
        /// <summary>
        /// 结构式库路径
        /// </summary>
        public static string SdbPath = Path.Combine(AppContext.BaseDirectory, "sdb");

        /// <summary>
        /// 分子式存储位置
        /// </summary>
        public static string Moleculedb = "moleculedb";

        /// <summary>
        /// 反应是存储位置
        /// </summary>
        public static string Reactiondb = "reactiondb";
    }
}
