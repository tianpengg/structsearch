using com.epam.indigo;
using Struct.Models.Help;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Struct.Common.Utils
{
    public class IndigoUtils
    {
        /// <summary>
        /// 创建获取获取数据库
        /// </summary>
        /// <param name="indigo"></param>
        /// <param name="tableName"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static Bingo GetWriteableBingoInstance(Indigo indigo, string tableName, string dbType)
        {
            // 忽略报错
            indigo.setOption("ignore-stereochemistry-errors", true);
            string sdbPath = Path.Combine(AppConfig.SdbPath, tableName);

            if (!Directory.Exists(sdbPath))
            {
                Directory.CreateDirectory(sdbPath);
            }

            string dbPath = string.Empty;
            if (dbType == DBType.Molecule)
            {
                dbPath = Path.Combine(sdbPath, AppConfig.Moleculedb);
            }
            else
            {
                dbPath = Path.Combine(sdbPath, AppConfig.Reactiondb);
            }

            Bingo bingo = null;
            if (Directory.Exists(dbPath))
            {
                try
                {
                    bingo = Bingo.loadDatabaseFile(indigo, dbPath);
                }
                catch (Exception ex)
                {
                    //NLogHelper.Default.Error($"获取文件db：{ex.Message}");
                    ////LogHelper.Info(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace);
                }
            }
            else
            {
                bingo = Bingo.createDatabaseFile(indigo, dbPath, dbType);
            }
            return bingo;
        }

        /// <summary>
        /// 获取数据库
        /// </summary>
        /// <param name="indigo"></param>
        /// <param name="tableName"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static Bingo GetReadOnlyBingoInstance(Indigo indigo, string tableName, string dbType)
        {
            indigo.setOption("ignore-stereochemistry-errors", true);
            string sdbPath = Path.Combine(AppConfig.SdbPath, tableName);

            if (!Directory.Exists(sdbPath))
            {
                Directory.CreateDirectory(sdbPath);
            }

            string dbPath = string.Empty;
            if (dbType == DBType.Molecule)
            {
                dbPath = Path.Combine(sdbPath, AppConfig.Moleculedb);
            }
            else
            {
                dbPath = Path.Combine(sdbPath, AppConfig.Reactiondb);
            }

            Bingo bingo = null;
            if (Directory.Exists(dbPath))
            {
                try
                {
                    bingo = Bingo.loadDatabaseFile(indigo, dbPath, "read_only:true");
                }
                catch (Exception ex)
                {
                    //NLogHelper.Default.Error($"获取DB：{ex.Message}");
                    //LogHelper.Info(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace);
                }
            }
            return bingo;
        }        
    }
}
