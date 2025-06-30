using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AvaliacaoFrontEnd.Models;
using Microsoft.IdentityModel.Tokens;

namespace AvaliacaoFrontEnd.Services
{
    public class AutenticacaoService
    {
        #region Jwt 

        public JWTRetornoVM JwtEncode(string usuario, string issuer, string audience, string secret, int validade = 1,
            string refreshToken = "", List<string>? roles = null, string tokenType = "Bearer")
        {
            if (validade <= 0)
            {
                validade = 1;
            }

            var dataCriacao = DateTime.UtcNow;
            var dataCriacaoOffset = new DateTimeOffset(dataCriacao);

            var dataValidade = dataCriacao.AddHours(validade);
            var authClaims = new List<Claim>
            {
                new Claim("user", usuario),
                new Claim("jti", Guid.NewGuid().ToString()),
                new Claim("iat", dataCriacaoOffset.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            var jwtSecurityToken = this.GetToken(authClaims, issuer, audience, secret, dataCriacao, dataValidade, roles, tokenType);
            return new JWTRetornoVM
            {
                TokenAcesso = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                TokenTipo = tokenType,
                TokenValidade = (validade * 60) * 60,
                RefreshToken = refreshToken
            };
        }

        #endregion

        #region Privates

        private JwtSecurityToken GetToken(List<Claim> authClaims, string issuer, string audience, string secret, DateTime dataCriacao, DateTime validade,
            List<string> roles = null, string tokenType = "Bearer")
        {
            var claimsIdentity = new ClaimsIdentity(authClaims, tokenType);
            if (roles != null)
            {
                claimsIdentity.AddClaims(roles.Select(role => new Claim("roles", role)));
            }

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                expires: validade,
                notBefore: dataCriacao.AddMinutes(-1),
                claims: claimsIdentity.Claims,
                signingCredentials: new SigningCredentials(GetSecurityKey(secret), SecurityAlgorithms.HmacSha256)
            );
            return token;
        }

        private SymmetricSecurityKey GetSecurityKey(string secret)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        }

        #endregion
    }
}
