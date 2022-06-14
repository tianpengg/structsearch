using System;
using System.Collections.Generic;
using System.Text;

namespace Struct.Models.Dto
{
    /// <summary>
    /// 子结构搜索
    /// </summary>
    public class StructSearchSmiDto : StructBaseSearchDto
    {
        /// <summary>
        /// 最小相似度  取值范围  0-1之间
        /// </summary>
        public float MinPer { get; set; }

        /// <summary>
        /// 最大相似度 取值范围  0-1之间
        /// </summary>
        public float MaxPer { get; set; }
    }
}
