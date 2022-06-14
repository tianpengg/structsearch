using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Struct.Models.Dto;
using Struct.Module.IBusiness;
using System.Collections.Generic;
using System.Linq;

namespace StructSearchHost.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StructSearchController : ControllerBase
    {
        private readonly IStructSearch _structSearch;

        public StructSearchController(IStructSearch structSearch)
        {
            _structSearch = structSearch;
        }

        /// <summary>
        /// 精确搜索
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public List<int> SearchExactStruct(StructSearchExactDto model)
        {
            return _structSearch.SearchExactStruct(model);
        }

        /// <summary>
        /// 子结构搜索
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public List<int> SearchSubStruct(StructSearchSubDto model)
        {
            return _structSearch.SearchSubStruct(model);
        }

        /// <summary>
        /// 相似度搜索
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public List<int> SearchSmiStruct(StructSearchSmiDto model)
        {
            return _structSearch.SearchSmiStruct(model);
        }
    }
}
