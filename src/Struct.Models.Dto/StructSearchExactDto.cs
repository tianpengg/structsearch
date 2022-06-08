using System;
using System.Collections.Generic;
using System.Text;

namespace Struct.Models.Dto
{
    public class StructSearchExactDto:StructBaseSearchDto
    {
        /// <summary>
        /// 是否开启手性
        /// </summary>
        public bool UseChirality { get; set; }
    }
}
