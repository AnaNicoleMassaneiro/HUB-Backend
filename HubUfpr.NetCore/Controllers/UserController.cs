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
        public ActionResult ValidateUser([FromBody] UserLogin request)
        {
            if (request.usuario == null || request.usuario == "" || request.senha == null || request.senha == "")
            {
                Response.StatusCode = 400;
                return Json(new { msg = "Por favor, informe a senha e email/GRR." });
            }

            var user = _userService.GetToken(request.usuario, request.senha);

            if (user == null)
            {
                Response.StatusCode = 401;
                return Json(new { msg = "Email/GRR e/ou senha incorretos." });
            }

            string token = TokenService.GenerateToken(user);

            _userService.UpdateLastLoginTime(user.Id);

            int idCliente = _userService.GetCustomerCode(user.Id);

            if (user.IsVendedor)
            {
                int idVendedor = _userService.GetSellerCode(user.Id);
                return Json(new { token, user, idVendedor, idCliente });
            }
            else {             
                return Json(new { token, user, idCliente });
            }
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

                if (userId != 0)
                {
                    if (request.isVendedor)
                    {
                        _userService.InsertVendedor(userId, 1, 0);
                    }
                    
                    _userService.InsertCliente(userId);
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

        [AllowAnonymous]
        [HttpPatch]
        [Route("atualizarLocalizacao/{userId}")]
        public ActionResult UpdateLocation([FromBody] UpdateUserLocation req, int userId)
        {
            try
            {
                Response.StatusCode = 400;

                if (req.Latitude == 0 || req.Longitude == 0)
                {
                    return Json(new { msg = "Você deve informar a latitude e longitude do usuário!" });
                }

                if (userId <= 0)
                {
                    return Json(new { msg = "Você deve informar um ID de usuário válido." });
                }

                if (_userService.UpdateUserLocation(userId, req.Latitude, req.Longitude) == 0)
                {
                    return Json(new { msg = "Usuário não encontrado." });
                }
                else
                {
                    Response.StatusCode = 200;
                    return Json(new { msg = "Usuário alterado com sucesso." });
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { msg = "Houve um problema ao atualizar o usuário: " + ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPatch]
        [Route("atualizarSenha/{userId}")]
        public ActionResult UpdatePassword([FromBody] UpdateUserPassword req, int userId)
        {
            try
            {
                Response.StatusCode = 400;

                if (req.NewPassword == null || req.NewPassword.Trim().Length == 0 ||
                        req.ConfirmNewPassword == null || req.ConfirmNewPassword.Trim().Length == 0)
                {
                    return Json(new { msg = "Você deve informar a nova senha e a confirmação da nova senha!" });
                }

                if (!req.ConfirmNewPassword.Equals(req.NewPassword))
                {
                    return Json(new { msg = "As senhas não coincidem!" });
                }

                if (req.NewPassword.Length < 6 || req.ConfirmNewPassword.Length < 6)
                {
                    return Json(new { msg = "As senhas devem conter ao menos 6 caracteres!" });
                }

                if (userId <= 0)
                {
                    return Json(new { msg = "Você deve informar um ID de usuário válido." });
                }

                if (_userService.UpdatePassword(userId, req.NewPassword) == 0)
                {
                    return Json(new { msg = "Usuário não encontrado." });
                }
                else
                {
                    Response.StatusCode = 200;
                    return Json(new { msg = "Senha atualizada com sucesso." });
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { msg = "Houve um problema ao alterar a senha: " + ex.Message });
            }
        }
            
        [AllowAnonymous]
        [HttpPatch]
        [Route("updateUser/{id}")]
        public ActionResult UpdateUser([FromBody] UpdateUser req, int id)
        {
            Response.StatusCode = 400;

            if ((req.Nome == null && req.Telefone == null) ||
                ((req.Nome != null && req.Nome.Trim() == "") && (req.Telefone != null && req.Telefone.Trim() == "")))
                return Json(new { msg = "Por favor, informe o nome e/ou o telefone!" });

            try
            {
                _userService.UpdateUser(req.Nome, req.Telefone, id);

                Response.StatusCode = 200;
                return Json(new { msg = "Usuário editado com sucesso." });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { msg = "Houve um problema ao editar o usuário: " + ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("buscarUserPorId/{id}")]
        public JsonResult SearchUserById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    Response.StatusCode = 400;
                    return Json(new { msg = "Você deve especificar o ID do user!" });                    
                }

                var user = _userService.GetUserById(id);

                if (user == null)
                {
                    Response.StatusCode = 404;
                    return Json(new { msg = "Nenhum usuário encontrado." });
                }

                return Json(new { user });

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erro ao buscar user: ", ex);
            }
        }
    }
}