using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RopeDetection.CommonData.ViewModels.UserViewModel;
using RopeDetection.Entities.Configuration;
using RopeDetection.Services.Interfaces;
using RopeDetection.Services.UserService;

namespace RopeDetection.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _userService;
        private readonly ILogger<AuthController> _logger;
        private readonly AppSettings _appSettings;

        public AuthController(ILogger<AuthController> logger, IAuthService userService, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _appSettings = appSettings.Value;
        }

        //[Route("Test")]
        //[HttpGet]
        //public IActionResult Test(int x, int y)
        //{
        //    MathCalc math = new MathCalc();
        //    int[] array = { 0, 0 };
        //    var result = math.calc((MWArray)x, (MWArray)y);
        //    return Ok(result.ToString());
        //}

        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPassword"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("LoginUser")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUser(string userName, string userPassword)
        {
            try
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userName),
                    new Claim("CanEditContent","true")
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    //AllowRefresh = <bool>,
                    // Refreshing the authentication session should be allowed.

                    //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.

                    //IsPersistent = true,
                    // Whether the authentication session is persisted across 
                    // multiple requests. When used with cookies, controls
                    // whether the cookie's lifetime is absolute (matching the
                    // lifetime of the authentication ticket) or session-based.

                    //IssuedUtc = <DateTimeOffset>,
                    // The time at which the authentication ticket was issued.

                    //RedirectUri = <string>
                    // The full path or absolute URI to be used as an http 
                    // redirect response value.
                };

                var userData = await _userService.LoginUser(userName, userPassword);
                if (userData.Result == CommonData.DefaultEnums.Result.OK)
                {
                    await HttpContext.SignInAsync(
                  CookieAuthenticationDefaults.AuthenticationScheme,
                  new ClaimsPrincipal(claimsIdentity),
                  authProperties);
                   
                }
                return Ok(userData);
            }
            catch (Exception exp)
            {
                return BadRequest(new UserShortModel() { Error = exp, Result = CommonData.DefaultEnums.Result.Error });
            }
        }

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RegisterUser")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser(UserModel model)
        {
            try
            {
                var userData = await _userService.Register(model);
                return Ok(userData);
            }
            catch (Exception exp)
            {
                return BadRequest(new UserShortModel() { Error = exp, Result = CommonData.DefaultEnums.Result.Error });
            }
        }

        /// <summary>
        /// Выход из приложения
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("LogoutUser")]
        [Authorize]
        public async Task<IActionResult> LogoutUser()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                //var result = SignOut();
                return Ok(new { message = "Успешно выполнен выход" });
            }
            catch
            {
                return BadRequest(new { message = "Ошибка. попробуйте снова." });
            }
        }

        //public async Task<object> GetUsers(string search, string start, string length, DataTableModel.DtOrder[] order, DataTableModel.DtColumn[] columns)
        //{
        //    return await _userService.GetUsers(search, start, length, order, columns);
        //}

        [HttpDelete("{id}")]
        [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUser(Guid Id)
        {
            try
            {
                await _userService.DeleteUser(Id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [Route("GetUser")]
        public async Task<IActionResult> GetUser(Guid Id)
        {
            try
            {
                var response =  await _userService.GetUser(Id);
                if (response.Result == CommonData.DefaultEnums.Result.OK)
                {
                    return Ok(response);
                }
                else
                {
                    throw new Exception("Ошибка получения пользователя");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new UserShortModel() { Error = ex, Result = CommonData.DefaultEnums.Result.Error });
            }
        }

        [HttpPost]
        [Route("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UserModel userModel)
        {
            try
            {
                await _userService.UpdateUser(userModel);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
