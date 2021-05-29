using Inventory_Management.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web.Http;

namespace Inventory_Management.Controllers
{
    public class ValuesController : ApiController
    {
        ////Also check Startup.cs for the configuration and set-up of JWT token
        [HttpPost]
        public object GenerateToken([FromBody]Authentication auth)
        {  
            var issuer = "http://localhost:56183";

            //Auth Key is passed as a request body parameter and will be a constant value, also referred in Startup.cs
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(auth.AuthKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Create a List of Claims   
            var permClaims = new List<Claim>();
            permClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            permClaims.Add(new Claim("valid", "1"));
            permClaims.Add(new Claim("userid", "1"));
            permClaims.Add(new Claim("name", "admin"));

            //Create Token object by providing required parameters    
            var token = new JwtSecurityToken(issuer, //Issuer    
                            issuer,  //Audience    
                            permClaims,
                            expires: DateTime.Now.AddHours(2), //the token will be valid for 2 hours
                            signingCredentials: credentials);
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return new { data = jwtToken };
        }
    }
}
