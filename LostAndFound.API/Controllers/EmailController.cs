using LostAndFound.API.ResponseWrapper;
using LostAndFound.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace LostAndFound.API.Controllers
{
    [ApiController]
    [Route("api/test/email")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailSendingService _emailSendingService;

        public EmailController(IEmailSendingService emailSendingService)
        {
            _emailSendingService = emailSendingService;
        }

        ///<summary>
        /// Test email sending for register
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost("send-email-register")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> SendMailRegister([Required] string email)
        {
            _emailSendingService.SendMailToRegister(email);

            return ResponseFactory.NoContent();
        }

        ///<summary>
        /// Reset password email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="newPass"></param>
        /// <returns></returns>
        [HttpPost("send-email-reset-pasword")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> SendMailResetPassword([Required] string email, [Required] string newPass)
        {
            _emailSendingService.SendMailResetPassword(email, newPass);

            return ResponseFactory.NoContent();
        }

        ///<summary>
        /// Test email sending for ban
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost("send-email-user-ban")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> SendMailBan([Required] string email)
        {
            _emailSendingService.SendMailWhenUserBan(email);

            return ResponseFactory.NoContent();
        }

        ///<summary>
        /// Test email sending for ban post
        /// </summary>
        /// <param name="email"></param>
        /// <param name="postName"></param>
        /// <returns></returns>
        [HttpPost("send-email-post-ban")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> SendMailBanPost([Required] string email, [Required]string postName)
        {
            _emailSendingService.SendMailWhenPostBan(email, postName);

            return ResponseFactory.NoContent();
        }

        ///<summary>
        /// Test email sending for ban item
        /// </summary>
        /// <param name="email"></param>
        /// <param name="itemName"></param>
        /// <returns></returns>
        [HttpPost("send-email-item-ban")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> SendMailBanItem([Required] string email, [Required] string itemName)
        {
            _emailSendingService.SendMailWhenItemBan(email, itemName);

            return ResponseFactory.NoContent();
        }

        ///<summary>
        /// Test email giveaway winner
        /// </summary>
        /// <param name="email"></param>
        /// <param name="itemName"></param>
        /// <returns></returns>
        [HttpPost("send-email-giveaway-winner")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> SendMailGiveawayWinner([Required] string email, [Required] string itemName)
        {
            _emailSendingService.SendMailGiveawayWinner(email, itemName);

            return ResponseFactory.NoContent();
        }

        ///<summary>
        /// Test email giveaway reroll
        /// </summary>
        /// <param name="email"></param>
        /// <param name="itemName"></param>
        /// <returns></returns>
        [HttpPost("send-email-giveaway-reroll")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiUnauthorizedResponse))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> SendMailGiveawayReroll([Required] string email, [Required] string itemName)
        {
            _emailSendingService.SendMailGiveawayReroll(email, itemName);

            return ResponseFactory.NoContent();
        }
    }
}
