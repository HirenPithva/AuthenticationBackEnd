using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Postgrest;
using Supabase;
using Supabase.Functions;
using Supabase.Gotrue;
using Supabase.Interfaces;
using tokenAuth.Model;
using tokenAuth.Services;

namespace tokenAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public readonly Supabase.Client Client;
        public readonly ILoginService loginService;
        public UsersController(Supabase.Client client, ILoginService _loginService) {
            Client = client;
            loginService = _loginService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var userData = new users
            {
                id = 1,
                first_name = "hiren",
                last_name = "pithva",
                email = "hirenpithva105@gmail.com",
                password = "xyz@123",
                phone_code = 91,
                phone = "1234567890",
                inserted_at = DateTime.Now,
                updated_at = DateTime.Now
            };
            var response = await Client.From<users>().Insert(userData);
            return Ok(response);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(RequestedUser formData)
        {
            var response = await loginService.LoginUser(formData);
            if(response == "")
            {
                return BadRequest("invalid");
            }
            return Ok(response);
        }
    }
}
