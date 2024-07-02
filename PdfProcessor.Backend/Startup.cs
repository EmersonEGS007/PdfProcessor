using PdfProcessor.Backend.src.Application.Interfaces;
using PdfProcessor.Backend.src.Application.Services;
using PdfProcessor.Backend.src.Infrastructure.Data;
using PdfProcessor.Backend.src.Infrastructure.ML;
using Microsoft.EntityFrameworkCore;


namespace PdfProcessor.Backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {


            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<IPdfService, PdfService>();
            services.AddSingleton(new PdfModelTrainer(Configuration.GetConnectionString("DefaultConnection")));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHttpsRedirection();
            }

            app.UseSwagger();
            app.UseSwaggerUI();


            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
