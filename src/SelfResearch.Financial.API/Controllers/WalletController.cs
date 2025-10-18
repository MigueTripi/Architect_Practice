using FluentResults;
using FluentResults.Extensions.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using SelfResearch.Core.Infraestructure.ErrorHandling;
using SelfResearch.Financial.API.Feature.Wallet;

namespace SelfResearch.Financial.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly ILogger<WalletController> _logger;
        private readonly IRetrieveWalletService _retrieveWalletService;

        public WalletController(ILogger<WalletController> logger, IRetrieveWalletService retrieveWalletService)
        {
            _logger = logger;
            _retrieveWalletService = retrieveWalletService;
        }

        /// <summary>
        /// Retrieves a wallet by user identifier.
        /// </summary>
        /// <param name="userId">The user identifier</param>
        /// <returns>The wallet</returns>
        [HttpGet("get_list_by_user/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<WalletDto>>>  GetWalletsByUserId(int userId)
        {
            if (userId <= 0)
            {
                return Result.Fail(new ArgumentError(nameof(userId), "User ID must be a positive integer.")).ToActionResult();
            }

            var result = await _retrieveWalletService.GetWalletsByUserAsync(userId);
            return Ok(result);
        }
    }
}
