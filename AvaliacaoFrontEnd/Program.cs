using System.Globalization;
using System.Text;
using AvaliacaoFrontEnd.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace AvaliacaoFrontEnd
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var configuration = builder.Configuration;
            builder.Configuration
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            const string LANGUAGE_CODE = "pt-BR";

            // Add services to the container.

            var services = builder.Services;
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
            });
            services.Configure<BrotliCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Fastest);
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.SmallestSize);
            services.AddCors();

            services.AddControllers().AddJsonOptions(options => { });
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            //builder.Services.AddOpenApi();

            services.AddDbContext<CacheContext>(opt =>
            {
                opt.UseInMemoryDatabase("DbTestCache");
            });

            #region JWT

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        RoleClaimType = "roles",

                        ValidAudience = configuration["JWT:ValidAudience"],
                        ValidateAudience = true,

                        ValidIssuer = configuration["JWT:ValidIssuer"],
                        ValidateIssuer = true,

                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!.Trim())),
                        ValidateIssuerSigningKey = true
                    };
                });

            #endregion

            #region SwaggerGen

            services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(x => x.FullName);

                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Version = "v1",
                    Title = "Api Teste Data Cempro",
                    Description = "Esta Api foi desenvolvida pela Data Cempro para prover integração com testes",
                    TermsOfService = new Uri("https://www.datacempro.com.br/PoliticaPrivacidade"),
                    //Contact
                    //License
                });

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    { JwtBearerDefaults.AuthenticationScheme, new string []{ } }
                };
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    Description = $"Copie '{JwtBearerDefaults.AuthenticationScheme} ' + token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = JwtBearerDefaults.AuthenticationScheme
                            }
                        }, Array.Empty<string>()
                    }
                });
            })
                .AddApiVersioning()
                .AddApiExplorer(options =>
                {
                    // Add the versioned API explorer, which also adds IApiVersionDescriptionProvider service
                    // note: the specified format code will format the version as "'v'major[.minor][-status]"
                    options.GroupNameFormat = "'v'VVV";

                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                });

            #endregion

            var app = builder.Build();
            var supportedCultures = new[] { new CultureInfo(LANGUAGE_CODE) };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(culture: LANGUAGE_CODE, uiCulture: LANGUAGE_CODE),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
            }

            app.UseResponseCompression();
            app.UseRouting();
            app.UseCors(x => x
                .SetIsOriginAllowed(origin => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            #region JWT
            app.UseAuthentication();
            app.UseAuthorization();
            #endregion

            app.MapControllers();

            #region Run Swagger

            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = string.Empty;

                if (app.Environment.IsDevelopment())
                {
                    options.SwaggerEndpoint($"/swagger/v1/swagger.json", "v1");
                }
            });


            #endregion

            app.Run();
        }
    }
}
