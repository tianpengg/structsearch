using Struct.Models.Dto;
using System;

namespace Struct.Module.IBusiness
{
    public interface IStructStore
    {
        /// <summary>
        /// 添加结构式
        /// </summary>
        /// <param name="model"></param>
        bool AddStruct(AddOrUpdateStructDto model);

        /// <summary>
        /// 修改结构式
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool UpdateStruct(AddOrUpdateStructDto model);
    }
}
