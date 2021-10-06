using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HubUfpr.API.Requests;
using HubUfpr.Service.Interface;
using System.Text.RegularExpressions;
using System;
using HubUfpr.Service.Class;

namespace HubUfpr.API.Controllers
{
    [Produces("application/json")]
    [Route("api/User")]
    public class UserController : Controller
    {
        protected readonly IUserService _userService;

        private const string emailRegex = @"^[\w!#$%&'*+\-\/=?\^_`{|}~]+(\.[\w!#$%&'*+\-\/=?\^_`{|}~]+)*@ufpr\.br$";
        private const string grrRegex = @"^[0-9]{8}$";

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("authenticate")]
        public ActionResult ValidateUser([FromBody]UserLogin request)
        {
            if (request.usuario == null || request.usuario == "" || request.senha == null || request.senha == "")
            {
                Response.StatusCode = 400;
                return Json(new { msg = "Por favor, informe a senha e email/GRR." });
            }

            var user = _userService.GetToken(request.usuario, request.senha);

            if (user == null) {
                Response.StatusCode = 401;
                return Json(new { msg = "Email/GRR e/ou senha incorretos." });
            }

            string token = TokenService.GenerateToken(user);

            _userService.UpdateLastLoginTime(user.Id);

            Response.StatusCode = 200;
            return Json(new { token = token, user = user });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("create")]
        public ActionResult InsertUser([FromBody] UserCad request)
        {
            Response.StatusCode = 400;

            if (request.senha == null || request.grr == null || request.email == null || request.confirmacaoSenha == null || request.nome == null ||
                request.senha == "" || request.grr == "" || request.email == "" || request.confirmacaoSenha == "" || request.nome == "")
                return Json(new { msg = "Por favor, informe o nome, email, GRR, senha e confirmação da senha!" });

            if (request.senha != request.confirmacaoSenha)
                return Json(new { msg = "Senhas não coincidem!" });

            if (!Regex.IsMatch(request.email, emailRegex))
                return Json(new { msg = "O email informado não é válido. Você deve usar um endereço de email válido que pertença ao domínio \"@ufpr.br\"." });

            if (_userService.IsEmailInUse(request.email))
                return Json(new { msg = "Este email já está em uso!" });

            if (!Regex.IsMatch(request.grr, grrRegex))
                return Json(new { msg = "O GRR informado é inválido. Seu GRR deve ser composto por 8 dígitos numéricos." });

            if (_userService.IsGRRInUse(request.grr))
                return Json(new { msg = "Este GRR já está em uso!" });

            try
            {
                var userId = _userService.InsertUser(request.nome, request.senha, request.email, request.grr, request.isVendedor);

                if (userId != null)
                {
                    if (request.isVendedor)
                    {
                        _userService.InsertVendedor(userId, 1, 0);
                    }
                    else
                    {
                        _userService.InsertCliente(userId);
                    }
                }

                Response.StatusCode = 200;
                return Json(new { msg = "Usuário criado com sucesso." });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { msg = "Houve um problema ao criar o usuário. " + ex.Message });
            }
        }
    }
}