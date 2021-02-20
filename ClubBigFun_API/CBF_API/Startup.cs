using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using CBF_API.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CBF_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IHostingEnvironment HostingEnvironment { get; private set; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddMvc();
            //services.AddTransient<IEmailService, EmailService>();

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            services.AddMvc(options =>
            {
                options.RespectBrowserAcceptHeader = true; // false by default
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "CBF API (Admin/Public)", Version = "v1" });

            });
            //var connection = @"data source=NOWGRAY-SERVER\MSSQL2012;initial catalog=ClubBigFun_DB;user id=sa;password=abc@123;multipleactiveresultsets=True;application name=EntityFramework";


            var connection = GlobalConfig.ConnectionString;// 

            //var connection = @"data source=sql1-p2stl.ezhostingserver.com;initial catalog=ClubBigFun_UAT;user id=ClubBigFun;password=Nowgray@2019;multipleactiveresultsets=True;application name=EntityFramework";

            services.AddDbContext<Models.CbfDbContext>
            (options => options.UseSqlServer(connection));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {

                x.SwaggerEndpoint("/swagger/v1/swagger.json", "CBF v1");
                x.RoutePrefix = string.Empty;
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseMvc();


        }
    }
    //public class ValidateModelAttribute : ActionFilterAttribute
    //{
    //    public override void OnActionExecuting(ActionExecutingContext context)
    //    {
    //        if (!context.ModelState.IsValid)
    //        {
    //            context.Result = new ValidationHelper.GetErrorListFromModelState(context.ModelState);
    //        }
    //    }
    //}
}
