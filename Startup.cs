using System.Collections.Generic;
using System.Linq;
using ApiGateway.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace ApiGateway
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var jwtBearers = _configuration.GetSection("JwtBearers").Get<List<JwtBearer>>();

            services.AddOcelot();
            services.AddControllers();
            foreach (var bearer in jwtBearers)
                services.AddAuthentication().AddJwtBearer(bearer.AuthenticationProviderKey,
                    options => CopyValues(bearer, options));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseOcelot().Wait();
            app.UseAuthentication();

            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private static void CopyValues<TSource, TTarget>(TSource source, TTarget target)
        {
            var sourceProps = typeof(TSource).GetProperties().Where(info => info.CanRead).ToList();
            var destProps = typeof(TTarget).GetProperties().Where(info => info.CanWrite).ToList();

            foreach (var sourceProp in sourceProps)
            {
                if (destProps.All(info => info.Name != sourceProp.Name)) continue;

                var prop = destProps.First(info => info.Name == sourceProp.Name);
                if (prop.CanWrite)
                    prop.SetValue(target, sourceProp.GetValue(source, null), null);
            }
        }
    }
}