using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SportsStore.Models;

namespace SportsStore
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration["Data:SportStoreProducts:ConnectionString"])
            );
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddMvc();

            services.AddTransient<IProductRepository, EFProductRepository>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    ProjectPath = Path.Combine(Directory.GetCurrentDirectory(), "ClientApp"),
                    HotModuleReplacement = true
                });
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute("pagination", "product/page{page}", new
                {
                    Controller = "Product",
                    action = "List"
                });
                routes.MapRoute("default", "{controller=Product}/{action=List}/{id?}");
            });
            SeedData.EnsurePopulated(app);
        }
    }
}