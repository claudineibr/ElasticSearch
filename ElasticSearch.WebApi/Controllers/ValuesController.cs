using ElasticSearch.Domain.IApplicationService;
using ElasticSearch.Domain.Utilities;
using ElasticSearch.Domain.ViewModel;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ElasticSearch.WebApi.Controllers
{
    public class ValuesController : Controller
    {
        private readonly ISearchApplicationService search;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        public ValuesController(ISearchApplicationService search)
        {
            this.search = search;
        }

        [HttpGet, Route("api/v1/Search/ReIndex")]
        [ApiExplorerSettings(GroupName = "v1")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ReIndex()
        {
            try
            {
                await search.ReIndex();
                return Ok();
            }
            catch (Exception ex)
            {
                logger.Error($"Product/Search::{ex.Message}::{ex.InnerException}::{ex.StackTrace}::{ex.Data}");
                return StatusCode((int)HttpStatusCode.ExpectationFailed, ExceptionErrors.Extract(ex));
            }
        }


        [HttpGet, Route("api/v1/Search/ReIndexMany")]
        [ApiExplorerSettings(GroupName = "v1")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ReIndexMany()
        {
            try
            {
                await search.ReIndexMany();
                return Ok();
            }
            catch (Exception ex)
            {
                logger.Error($"Product/Search::{ex.Message}::{ex.InnerException}::{ex.StackTrace}::{ex.Data}");
                return StatusCode((int)HttpStatusCode.ExpectationFailed, ExceptionErrors.Extract(ex));
            }
        }

        [HttpGet, Route("api/v1/Search/ReIndexBulkAsync")]
        [ApiExplorerSettings(GroupName = "v1")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ReIndexBulkAsync()
        {
            try
            {
                await search.ReIndexBulkAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                logger.Error($"Product/Search::{ex.Message}::{ex.InnerException}::{ex.StackTrace}::{ex.Data}");
                return StatusCode((int)HttpStatusCode.ExpectationFailed, ExceptionErrors.Extract(ex));
            }
        }

        [HttpGet, Route("api/v1/Search/ReIndexUpdate")]
        [ApiExplorerSettings(GroupName = "v1")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ReIndexUpdate()
        {
            try
            {
                await search.ReIndexUpdate();
                return Ok();
            }
            catch (Exception ex)
            {
                logger.Error($"Product/Search::{ex.Message}::{ex.InnerException}::{ex.StackTrace}::{ex.Data}");
                return StatusCode((int)HttpStatusCode.ExpectationFailed, ExceptionErrors.Extract(ex));
            }
        }

        [HttpGet, Route("api/v1/search/")]
        [ApiExplorerSettings(GroupName = "v1")]
        [ProducesResponseType(typeof(List<ProductViewModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Search([FromQuery]FilterProductViewModel filter)
        {
            try
            {
                return Ok(await search.Find(filter));
            }
            catch (Exception ex)
            {
                logger.Error($"Product/Search::{ex.Message}::{ex.InnerException}::{ex.StackTrace}::{ex.Data}");
                return StatusCode((int)HttpStatusCode.ExpectationFailed, ExceptionErrors.Extract(ex));
            }
        }


    }
}
