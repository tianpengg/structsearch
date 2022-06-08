using Struct.Models.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Struct.Module.IBusiness
{
    public interface IStructSearch
    {
        /// <summary>
        /// 精确搜索
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        List<int> SearchExactStruct(StructSearchExactDto model);
    }
}
