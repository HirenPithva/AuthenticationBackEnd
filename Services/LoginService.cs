using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using tokenAuth.Helper;
using tokenAuth.Model;

namespace tokenAuth.Services
{
    public class LoginService:ILoginService
    {

        public Supabase.Client client;
        public IConfiguration configuration;
        public LoginService(Supabase.Client _client, IConfiguration _configuration)
        {
            client = _client;
            configuration = _configuration;
            //var appSettingProvider = service.AddOptions().Configure<AppSettings>(configuration.GetSection("AppSettings")).BuildServiceProvider();
            //appSettings = appSettingProvider.GetRequiredService<AppSettings>();
            //var xyz = service.ConfigureOptions(appSettings);
        }

        public async Task<string> LoginUser(RequestedUser requestedUser)
        {
            try
            {
                if (requestedUser != null)
                {
                    var possibleUser = await client.From<users>().Where(x => x.email == requestedUser.email && x.password == requestedUser.password).Single();
                    var time = DateTime.UtcNow;
                    var timeDuration = TimeSpan.FromMinutes(10);
                    var expirationTime = time.Add(timeDuration);

                    var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AppSettings:Key"]));
                    var signingCredintial = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

                    var claims = new List<Claim> {
                    new Claim(JwtRegisteredClaimNames.Iss, configuration["AppSettings:Issuer"]),
                    new Claim(JwtRegisteredClaimNames.Aud, configuration["AppSettings:Audience"]),
                    new Claim(JwtRegisteredClaimNames.Sub, "User details"),
                    new Claim(JwtRegisteredClaimNames.Email, possibleUser.email),
                    new Claim(JwtRegisteredClaimNames.Jti, possibleUser.id.ToString())
                };

                    if (possibleUser != null)
                    {

                        JwtSecurityToken token = new JwtSecurityToken(
                            issuer: configuration["AppSettings:Issuer"],
                            audience: configuration["AppSettings:Audience"],
                            claims: claims,
                            expires: expirationTime,
                            signingCredentials: signingCredintial
                            );

                        return new JwtSecurityTokenHandler().WriteToken(token);


                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            
        }
    }
}
