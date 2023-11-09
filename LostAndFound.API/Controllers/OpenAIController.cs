using LostAndFound.API.ResponseWrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenAI_API;
using OpenAI_API.Completions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LostAndFound.API.Controllers
{
    [Route("api/openai")]
    [ApiController]
    public class OpenAIController : ControllerBase
    {
        /// <summary>
        /// Use Open AI
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiOkResponse<>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiNotFoundResponse))]
        public async Task<IActionResult> UseOpenAI(string query)
        {
            string outPutResult = "";
            //Go get the key from appsetting and put here, will read directly later
            var openai = new OpenAIAPI("put-the-key-here");
            CompletionRequest completionRequest = new CompletionRequest();
            completionRequest.Prompt = query;
            completionRequest.Model = OpenAI_API.Models.Model.DavinciText;

            var completions = openai.Completions.CreateCompletionAsync(completionRequest);

            foreach (var completion in completions.Result.Completions)
            {
                outPutResult += completion.Text;
            }

            return ResponseFactory.Ok(outPutResult);
        }
    }
}
