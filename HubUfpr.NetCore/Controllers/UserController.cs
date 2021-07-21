using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HubUfpr.API.Requests;
using HubUfpr.Service.Interface;

namespace HubUfpr.API.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : Controller
    {
        protected readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("")]
        public ActionResult ValidateUser([FromBody]UserLogin request)
        {
            
            var ret = _userService.GetToken(request.UserName, request.Password);

            if (ret == null)
                return StatusCode(401);

            return Json(ret);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("create")]
        public JsonResult InsertUser(string username, string password, string confirmpassword)
        {
            if (password == confirmpassword)
            {
                _userService.InsertUser(username, password);
            }
            return Json("Usuário criado com sucesso! :)");
        }
    }
}