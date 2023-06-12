using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_rpg.Dtos.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    { 
        private readonly IAuthRepository _autRepo;
        public AuthController(IAuthRepository autRepo)
        {
            _autRepo = autRepo;
            
        }
        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto request)
        {
            var response = await _autRepo.Register(
                new User{UserName=request.UserName}, request.Password
            );
            if(!response.Success){
                return BadRequest(response);
            }
            return Ok(response);
        }
          [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<int>>> Login(UserLoginDto request)
        {
            var response = await _autRepo.Login(request.UserName, request.Password);
            if(!response.Success){
                return BadRequest(response);
            }
            return Ok(response);
        }
        
    }
}