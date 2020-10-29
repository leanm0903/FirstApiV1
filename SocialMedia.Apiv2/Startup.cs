using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Interfaces;
using SocialMedia.Core.Services;
using SocialMedia.Infrastucture.Data;
using SocialMedia.Infrastucture.Filters;
using SocialMedia.Infrastucture.Interfaces;
using SocialMedia.Infrastucture.Repositories;
using SocialMedia.Infrastucture.Service;

namespace SocialMedia.Apiv2
{

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(option =>
            option.Filters.Add<GlobalExceptionFilter>()//filtro de excepciones
            ).AddNewtonsoftJson(option =>
            {
                option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                option.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //services.AddTransient<IPostRepository, PostRepository>(); Se tenia un repositorio por cada clase y se remplazo por una generica.
            services.AddTransient<IPostService, PostService>();//trasient se genera una nueva instancia por cada ejecucion
            //services.AddTransient<IUserRepository, UserRepository>();
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));//cuando una clase estemos usando una interface la replazamos por BaseRepostory ç,  se usa scope en vez de transient por el tipo de vidad de la implementancion
            services.AddDbContext<SocialMediaContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SocialMedia")));
            //busca los profiles del automapper
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IUrlService>(provider =>
            {
                var accersor = provider.GetRequiredService<IHttpContextAccessor>();//obtien el httpClient que se genera e la ejecucio
                var request = accersor.HttpContext.Request;//se obitiene el request
                var absolutUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UrlService(absolutUri);


            });//singleton genera una sola instancia para todas las ejecuciones
            services.Configure<PaginationOption>(Configuration.GetSection("Pagination")); //esta seccion mapea la configuracion
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSwaggerGen(doc =>
            {
                doc.SwaggerDoc("v1", new OpenApiInfo { Title = "Social Media api", Version = "v1" });//agrega swagger
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                doc.IncludeXmlComments(xmlPath);

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(option => {
                option.SwaggerEndpoint("/swagger/v1/swagger.json", "Social MEdia API");//agrega swagger UI
                option.RoutePrefix=string.Empty;
                });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
