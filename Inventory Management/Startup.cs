using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security;
using Microsoft.IdentityModel.Tokens;
using System.Text;

[assembly: OwinStartup(typeof(Inventory_Management.Startup))]

namespace Inventory_Management
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)  
        {  
            app.UseJwtBearerAuthentication(  
                new JwtBearerAuthenticationOptions  
                {  
                    AuthenticationMode = AuthenticationMode.Active,  
                    TokenValidationParameters = new TokenValidationParameters()  
                    {  
                        ValidateIssuer = true,  
                        ValidateAudience = true,  
                        ValidateIssuerSigningKey = true,  
                        ValidIssuer = "http://localhost:56183", //domain  
                        ValidAudience = "http://localhost:56183",  
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("d1arta2c-3g7c-47hc-8tp3-3405d5db2f8e"))  
                    }  
                });  
        } 
    }
}
