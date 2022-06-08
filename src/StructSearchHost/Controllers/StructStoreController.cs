using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Struct.Models.Dto;
using Struct.Module.IBusiness;

namespace StructSearchHost.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StructStoreController : ControllerBase
    {
        private readonly IStructStore _structStore;

        public StructStoreController(IStructStore structStore)
        {
            _structStore = structStore;
        }

        /// <summary>
        /// 添加结构式
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public bool AddStruct(AddOrUpdateStructDto model) 
        {
            return _structStore.AddStruct(model);
        }
    }
}
