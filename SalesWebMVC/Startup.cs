﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Data;
using SalesWebMVC.Services;

namespace SalesWebMVC
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<SalesWebMVCContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("SalesWebMVCContext")));

            //Registrando Serviços no sistema de injeção de dependências.
            services.AddScoped<SeedingService>(); //Somente esta linha registra o serviço SeedingService no sistema de injeção de dependência da aplicação.
            services.AddScoped<SellerService>();
            services.AddScoped<DepartmentService>();
            services.AddScoped<SalesRecordService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, SeedingService seedingService) //O método Seed é chamado dentro deste método.
        {
            //Configuracao para que o App tenha como Location padrão o Locail do Brasil.
            var l_ptBR = new CultureInfo("pt-BR");
            var l_localizationOptions = new RequestLocalizationOptions()
            {
                DefaultRequestCulture = new RequestCulture(l_ptBR),
                SupportedCultures = new List<CultureInfo>() { l_ptBR },
                SupportedUICultures = new List<CultureInfo>() { l_ptBR }
            };

            app.UseRequestLocalization(l_localizationOptions);
            //FIM da configuracao de Location.
            
            
            //Se adicionarmos um parametro no método Configure e essa classe estiver registrada no sistema de injeção de dependência da aplicação (que é o nosso caso)
            //automaticamente será resolvida uma instância deste objeto.
            
            if (env.IsDevelopment()) //Se estamos no perfil de desenvolvimento:
            {
                app.UseDeveloperExceptionPage();
                seedingService.Seed(); //Irá popular a base da dados, caso nao esteja populada.
            }
            else //Se estamos no perfil de produção (App publicado):
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
