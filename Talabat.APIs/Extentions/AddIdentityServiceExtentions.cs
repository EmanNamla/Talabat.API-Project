using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Talabat.Core.Entites.Identity;
using Talabat.Core.Services;
using Talabat.Repository.Identity;
using Talabat.Service;

namespace Talabat.APIs.Extentions
{
    public static class AddIdentityServiceExtentions
    {
        public static IServiceCollection AddIdentityService(this IServiceCollection Services,IConfiguration configuration)
        {
            Services.AddScoped<ITokenService, TokenService>();


           Services.AddIdentity<AppUser, IdentityRole>()
                 .AddEntityFrameworkStores<AppIdentityDbContext>();

            Services.AddAuthentication(options=>
            {
                options.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options=>
                options.TokenValidationParameters=new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JWT:VaildIssuer"],
                    ValidateAudience = true,
                   ValidAudience=configuration["JWT:VaildAudience"],
                   ValidateLifetime=true,
                   ValidateIssuerSigningKey=true,
                   IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
                }



                );
            return Services;
        }
    }
}
