using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Commands.Users.UpdateUsers;

namespace Restaurants.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IMediator _mediator;

        public IdentityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpPatch("me")]
        public async Task<IActionResult> UpdateUserDetails(UpdateUserDetailsCommand command) 
        {
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
