using MarketApp.DataTransferObjects;
using MarketApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarketApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserDto request, double money)
        {
            try
            {
                await _userService.Register(request, money);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }


            return Ok(" new account registered");
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserDto request)
        {
            string jwt = string.Empty;
            try
            {
                jwt = await _userService.Login(request);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }


            return Ok(jwt);
        }


        [HttpPost("AdminLogin")]
        public IActionResult LoginToAdminAcount(UserDto request)
        {
            string jwt = String.Empty;
            try
            {
                jwt = _userService.LoginToAdminAccount(request);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(jwt);
        }

        [HttpPost("Buy"), Authorize]
        public async Task<IActionResult> BuyProduct(int productId, int amount)
        {
            try
            {
                await _userService.BuyProduct(productId, amount);
            }
            catch (ArgumentException ex)
            {
                BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                BadRequest(ex.Message);
            }


            return Ok("User bought a product");
        }
    }
}
