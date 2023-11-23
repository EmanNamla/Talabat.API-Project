using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.DTOS;
using Talabat.APIs.Error;
using Talabat.APIs.Extentions;
using Talabat.Core.Entites.Identity;
using Talabat.Core.Services;
using Talabat.Repository.Identity;

namespace Talabat.APIs.Controllers
{
    public class AccountsController : APIBaseController
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public AccountsController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,ITokenService tokenService,IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if(CkeckEmailExist(model.Email).Result.Value)
            {
                return BadRequest(new APIResponse(404, "The Email Is Exists"));
            }
            var User = new AppUser
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split('@')[0]
            };
            var result = await userManager.CreateAsync(User, model.Password);
            if (!result.Succeeded) { return BadRequest(new APIResponse(400)); }
            var ReturnedUser = new UserDto
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                Token = await tokenService.CreateTokenAsync(User, userManager)
            };
            return Ok(ReturnedUser);
        }
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var User= await userManager.FindByEmailAsync(model.Email);
            if(User== null) { return Unauthorized(new APIResponse(401)); }
            var Result= await signInManager.CheckPasswordSignInAsync(User, model.Password,false);
            if(!Result.Succeeded) return Unauthorized(new APIResponse(401));
            return Ok(new UserDto { DisplayName=User.DisplayName,Email=User.Email,Token= await tokenService.CreateTokenAsync(User, userManager) } );
        }
        [Authorize]
        [HttpGet("CurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email=User.FindFirstValue(ClaimTypes.Email);
            var user =await userManager.FindByEmailAsync(email);
            return Ok(new UserDto { DisplayName=user.DisplayName, Email=user.Email,Token=await tokenService.CreateTokenAsync(user,userManager) });
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddresDto>> GetCurrentUserAddress()
        {
            var user =await userManager.FindUserWithAddressAsync(User);
            var mappeduser=mapper.Map<Address,AddresDto>(user.Address);
            return Ok(mappeduser);
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddresDto>> UpdateCurrentUserAddress(AddresDto UpdateAddres)
        {
            var user = await userManager.FindUserWithAddressAsync(User);
            if(user == null) { return Unauthorized(new APIResponse(401)); }
            var Address = mapper.Map<AddresDto, Address>(UpdateAddres);
            Address.Id=user.Address.Id;
            user.Address= Address;
            var result=await userManager.UpdateAsync(user);
            if(!result.Succeeded) { return BadRequest(new APIResponse(400)); }
            return Ok(UpdateAddres);
        }

        [HttpGet]
        public async Task<ActionResult<bool>>CkeckEmailExist(string email)
        {
            var User =await userManager.FindByEmailAsync(email);
            if (User is null)
                return false;
            else return true;
        }
    }
}
