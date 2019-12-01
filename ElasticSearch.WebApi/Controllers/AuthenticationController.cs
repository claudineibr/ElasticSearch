using Microsoft.AspNetCore.Mvc;
using NascorpLib;
using NLog;
using ElasticSearch.Domain;
using ElasticSearch.Domain.IApplicationService;
using ElasticSearch.Domain.Utilities;
using ElasticSearch.Domain.ViewModel;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ElasticSearch.WebApi.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ILoginApplication loginApplication;
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public AuthenticationController(ILoginApplication loginApplication)
        {
            this.loginApplication = loginApplication;
        }

        [HttpPost, Route("api/v1/authentication")]
        [ApiExplorerSettings(GroupName = "v1")]
        [ProducesResponseType(typeof(LoginViewModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.ExpectationFailed)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Authentication([FromBody]AuthenticationViewModel entity)
        {
            BaseResponse<LoginViewModel> response = null;
            try
            {
                if (entity is null)
                    return StatusCode((int)HttpStatusCode.BadRequest, ExceptionErrors.Extract(new Exception(Constants.DATANOTFOUND), Domain.Utilities.ErrorStatusCode.NullData));

                if (!ModelState.IsValid)
                    return StatusCode((int)HttpStatusCode.BadRequest, ModelErrors.Extract(ModelState));

                response = new BaseResponse<LoginViewModel>
                {
                    Data = await loginApplication.Authentication(entity),
                    Sucess = true
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                if (response is null) response = new BaseResponse<LoginViewModel>();
                response.Sucess = false;
                response.Errors = ExceptionErrors.Extract(ex);
                logger.Error($"Authentication::{ex.Message}::{ex.InnerException}::{ex.StackTrace}::{ex.Data}");
                return StatusCode((int)HttpStatusCode.BadRequest, response);
            }
        }
    }
}