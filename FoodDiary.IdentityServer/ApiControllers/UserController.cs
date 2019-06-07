using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDiary.IdentityServer.ApiControllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        // TODO: Rate limiting and recaptcha or equivalent

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUserRequest createUserRequest)
        {
            var result = await _userManager.CreateAsync(new IdentityUser
            {
                UserName = createUserRequest.UserName,
                Email = createUserRequest.Email
            }, createUserRequest.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.ToDictionary(k => k.Code, v => v.Description));
            }

            return Ok();
        }
    }

    public class CreateUserRequest
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
