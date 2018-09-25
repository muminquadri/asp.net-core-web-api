using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

namespace CityInfo.Api
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
                //logic to set json output property name to same as defined in class name. Normally we get id in JSON 
                //when in class its defined as Id i-e camel case for all properties. So if we want the case to be same
                //we use this logic
                //.AddJsonOptions(o=>
                //{
                //    if (o.SerializerSettings.ContractResolver != null)
                //    {
                //        var castedResolver = o.SerializerSettings.ContractResolver as DefaultContractResolver;
                //        castedResolver.NamingStrategy = null;
                //    }
                //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStatusCodePages();

            app.UseMvc();
        }
    }
}
