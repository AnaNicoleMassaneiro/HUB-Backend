﻿using Microsoft.AspNetCore.Authorization;
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
            if (request.usuario == null || request.senha == "")
            {
                return Json("Por favor, informe a senha e nome de usuário");
            }
            else
            {
                var ret = _userService.GetToken(request.usuario, request.senha);

                if (ret == null)
                    return StatusCode(401);

                return Json(ret);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("create")]
        public JsonResult InsertUser([FromBody] UserCad request)
        {
            if (request.senha == null || request.usuario == null || request.confirmacaoSenha == null)
            {
                return Json("Por favor, informe a senha, nome de usuário e confirmação da senha");
            }
            else
            {
                {
                    _userService.InsertUser(request.usuario, request.senha);
                }

                if (request.senha == request.confirmacaoSenha)
                {
                    _userService.InsertUser(request.usuario, request.senha);
                }
                return Json("Usuário criado com sucesso! :)");
            }
           
        }
    }
}