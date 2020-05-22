using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using HotelixApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HotelixApi
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
			services.AddDbContext<HotelsContext>(opt => {
				opt.UseInMemoryDatabase("Hotels");
			});
			services.AddControllers().AddNewtonsoftJson(options => {
				options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
				options.SerializerSettings.Converters.Add(new StringEnumConverter());
			});
			services.AddOpenApiDocument(document => document.DocumentName = "v1");
			services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
			{
				builder
					.AllowAnyOrigin()
					.AllowAnyMethod()
					.AllowAnyHeader();
			}));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwaggerUi3();
			}
			app.UseCors("MyPolicy");
			app.UseOpenApi();
			app.UseHttpsRedirection();
			app.UseRouting();
			app.UseAuthorization();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
