using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Backend_AYNI.Models;
using Microsoft.EntityFrameworkCore;
using Backend_AYNI.ResponseModels;
using Backend_AYNI.Services;

namespace Backend_AYNI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserModelController : Controller
    {
        private DatabaseContext context;
        private readonly Microsoft.AspNetCore.Identity.UserManager<UserModel> userManager;
        private readonly IIdentityService _identityService;

        public UserModelController(DatabaseContext context, Microsoft.AspNetCore.Identity.UserManager<UserModel> userManager, IIdentityService identityService)
        {
            this.context = context;
            this.userManager = userManager;
            this._identityService = identityService;
        }




        [HttpPost(template: ApiRoutes.User.Register)]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = ModelState.Values.SelectMany(x => x.Errors.Select(e => e.ErrorMessage))
                });
            }

            var authResponse = await _identityService.RegisterAsync(request.userName, request.userMail, request.password);


            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                }); ;
            }

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token
            });
        }

        [HttpPost(template: ApiRoutes.User.Login)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var authResponse = await _identityService.LoginAsync(request.userMail, request.password);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                }); ;
            }

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token
            });
        }





        [HttpGet]
        public async Task<ActionResult<ICollection<UserModel>>> Get()
        {
            var users = await context.Users.ToListAsync();
            if (!users.Any())
                return NotFound();
            return users;
        }

        [HttpGet("id")]
        public async Task<ActionResult<ICollection<UserModel>>> GetUserModel(string id)
        {
            var user = await context.Users.FirstOrDefaultAsync();
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpDelete("id")]
        public async Task<ActionResult> Delete(string id)
        {
            var existe = await Existe(id);

            if (!existe)
                return NotFound();

            var user = await context.Users.FindAsync(id);
            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return NoContent();
        }

        private async Task<bool> Existe(string id)
        {
            return await context.Users.AnyAsync(p => p.Id == id);
        }
    }
}
