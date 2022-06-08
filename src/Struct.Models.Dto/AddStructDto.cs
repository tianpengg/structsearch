using System;

namespace Struct.Models.Dto
{
    public class AddOrUpdateStructDto:StructBaseStoreDto
    {
        /// <summary>
        /// 入库的文本（mol、smiles、反应式）
        /// </summary>
        public string ChemContent { get; set; }
    }
}
