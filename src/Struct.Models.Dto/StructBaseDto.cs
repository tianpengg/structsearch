using System;
using System.Collections.Generic;
using System.Text;

namespace Struct.Models.Dto
{
    /// <summary>
    ///  整个结构式搜索Dto的父类
    /// </summary>
    public class StructBaseDto
    {
        /// <summary>
        /// 结构式所在文件名，用于多客户分开存储
        /// </summary>
        public string TableName { get; set; }
    }
}
