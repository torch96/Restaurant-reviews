using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Securty.Claims;
using Resturant.Repositories;

namespace Restaurant{
    public class Startup{
        public Startup(IConfiguration configuration){
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
       
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services){
            services.RegisterMongoDbRepositories();
            services.AddMvcCore().AddAuthorization().AddNewtonsoftJson();
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/build"; });
            services.Configure<JwtAuthentication>(Configuration.GetSection("JwtAuthentication"));
            services.AddSingleton<IPostConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env){
            if (env.IsDevelopment()){
                app.UseDeveloperExceptionPage();
            }else{
                app.UseExceptionHandler("/Error");
            }
            app.UseHsts();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>{
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
            app.UseSpa(spa =>{
                spa.Options.SourcePath = "ClientApp";
                if (env.IsDevelopment()){
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }


    }
    class ConfigureJwt : IPostConfigureOptions<JwtBearerOptions>
    {
        private readonly JwtAuthentication _jwtAuthentication;
        public ConfigureJwt(IOptions<JwtAuthentication> jwtAuthentication)
        {
            _jwtAuthentication = jwtAuthentication.Value;
        }

        public void Configure(string name, JwtBearerOptions options)
        {
            var jwtAuthentication = _jwtAuthentication.value;
            options.ClaimsIssuer = _jwtAuthentication.Issuer;
            options.IncludeErrorDetails = true;
            options.RequireHttpsMetadata = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtAuthentication.Issuer,
                ValidAudience = jwtAuthentication.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtAuthentication.SecretKey))
                NameClaimType = ClaimTypes.NameIdentifier
            };
        }

    }
}
