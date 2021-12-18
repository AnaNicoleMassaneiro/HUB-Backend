using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using HubUfpr.Model;
using Microsoft.IdentityModel.Tokens;

namespace HubUfpr.Service.Class
{
    public static class TokenService
    {
        public static string GenerateToken(User user, int idCliente, int idVendedor)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AppSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("UserId", user.Id.ToString()),
                    new Claim("CustomerId", idCliente.ToString()),
                    new Claim("UserGRR", user.GRR),
                    new Claim("Email", user.Email),
                    new Claim("IsVendedor", user.IsVendedor.ToString()),
                    new Claim("SellerId", idVendedor.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(43800),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static bool IsTokenValid(string token)
        {
            const string emailRegex = @"^[\w!#$%&'*+\-\/=?\^_`{|}~]+(\.[\w!#$%&'*+\-\/=?\^_`{|}~]+)*@ufpr\.br$";
            const string grrRegex = @"^[0-9]{8}$";
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AppSettings.Secret));

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = key,
                }, out SecurityToken validatedToken);

                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
                var claims = jwtToken.Claims.GetEnumerator();

                while (claims.MoveNext())
                {
                    switch (claims.Current.Type)
                    {
                        case "UserId":
                            if (int.Parse(claims.Current.Value) <= 0)
                            {
                                return false;
                            }
                            break;
                        case "UserGRR":
                            if (!Regex.IsMatch(claims.Current.Value, grrRegex))
                            {
                                return false;
                            }
                            break;
                        case "Email":
                            if (!Regex.IsMatch(claims.Current.Value, emailRegex))
                            {
                                return false;
                            }
                            break;
                        default: break;
                    }
                }

                return true;
            }
            catch
            { 
                return false;
            }
        }

        public static bool IsTokenValidMatchUserId(string token, int id)
        {
            const string emailRegex = @"^[\w!#$%&'*+\-\/=?\^_`{|}~]+(\.[\w!#$%&'*+\-\/=?\^_`{|}~]+)*@ufpr\.br$";
            const string grrRegex = @"^[0-9]{8}$";
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AppSettings.Secret));

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = key,
                }, out SecurityToken validatedToken);

                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
                var claims = jwtToken.Claims.GetEnumerator();

                while (claims.MoveNext())
                {
                    switch (claims.Current.Type)
                    {
                        case "UserId":
                            if (int.Parse(claims.Current.Value) <= 0)
                            {
                                return false;
                            }

                            if (int.Parse(claims.Current.Value) != id)
                            {
                                return false;
                            }
                            break;
                        case "UserGRR":
                            if (!Regex.IsMatch(claims.Current.Value, grrRegex))
                            {
                                return false;
                            }
                            break;
                        case "Email":
                            if (!Regex.IsMatch(claims.Current.Value, emailRegex))
                            {
                                return false;
                            }
                            break;
                        default: break;
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsTokenValidMatchCustomerId(string token, int id)
        {
            const string emailRegex = @"^[\w!#$%&'*+\-\/=?\^_`{|}~]+(\.[\w!#$%&'*+\-\/=?\^_`{|}~]+)*@ufpr\.br$";
            const string grrRegex = @"^[0-9]{8}$";
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AppSettings.Secret));

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = key,
                }, out SecurityToken validatedToken);

                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
                var claims = jwtToken.Claims.GetEnumerator();

                while (claims.MoveNext())
                {
                    switch (claims.Current.Type)
                    {
                        case "UserId":
                            if (int.Parse(claims.Current.Value) <= 0)
                            {
                                return false;
                            }
                            break;
                        case "UserGRR":
                            if (!Regex.IsMatch(claims.Current.Value, grrRegex))
                            {
                                return false;
                            }
                            break;
                        case "Email":
                            if (!Regex.IsMatch(claims.Current.Value, emailRegex))
                            {
                                return false;
                            }
                            break;
                        case "CustomerId":
                            if (int.Parse(claims.Current.Value) != id)
                            {
                                return false;
                            }
                            break;
                        default: break;
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsTokenValidMatchSellerId(string token, int id)
        {
            const string emailRegex = @"^[\w!#$%&'*+\-\/=?\^_`{|}~]+(\.[\w!#$%&'*+\-\/=?\^_`{|}~]+)*@ufpr\.br$";
            const string grrRegex = @"^[0-9]{8}$";
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AppSettings.Secret));

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = key,
                }, out SecurityToken validatedToken);

                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
                var claims = jwtToken.Claims.GetEnumerator();

                while (claims.MoveNext())
                {
                    switch (claims.Current.Type)
                    {
                        case "UserId":
                            if (int.Parse(claims.Current.Value) <= 0)
                            {
                                return false;
                            }
                            break;
                        case "UserGRR":
                            if (!Regex.IsMatch(claims.Current.Value, grrRegex))
                            {
                                return false;
                            }
                            break;
                        case "Email":
                            if (!Regex.IsMatch(claims.Current.Value, emailRegex))
                            {
                                return false;
                            }
                            break;
                        case "IsVendedor":
                            if (claims.Current.Value != "True")
                            {
                                return false;
                            }
                            break;
                        case "SellerId":
                            if (int.Parse(claims.Current.Value) != id)
                            {
                                return false;
                            }

                            break;
                        default: break;
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}