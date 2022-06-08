using Struct.Models.Help;
using System;

namespace Struct.Common.Utils
{
    public class DBTypeUtils
    {
        /// <summary>
        /// 根据内容获取该内容是反应式还是普通的分子式
        /// </summary>
        /// <param name="chemContent"></param>
        /// <returns></returns>
        public static string GetDBType(string chemContent)
        {
            /*
             * c1ccccc1>>C1CCCCC1.C1CCCCC1  --反应式
             * c1ccccc1                     --分子式
             */
            
            var chemStr = chemContent.Split(">>");

            // 防止调用服务出现  c1ccccc1>>的情况，这里多加几个判断
            if (chemStr.Length > 1 && !string.IsNullOrEmpty(chemStr[0]) && !string.IsNullOrEmpty(chemStr[1]))
            {
                return DBType.Reaction;
            }
            else
            {
                return DBType.Molecule;
            }
            
        }
    }
}
