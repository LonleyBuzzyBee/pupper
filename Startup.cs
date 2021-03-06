﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Pupper.Models;


namespace Pupper
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContext<PupperContext>(opt =>
          opt.UseMySql(Configuration.GetConnectionString("DefaultConnection")));
      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);


      services.AddSwaggerGen(c =>
       {
         c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
       });

      services.AddCors(options =>
        {
          options.AddPolicy(name: MyAllowSpecificOrigins,
            builder =>
            {
              builder.WithOrigins("http://localhost:5000");
            });
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }
      app.UseSwagger();
      app.UseSwaggerUI(c =>
 {
   c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
   c.RoutePrefix = string.Empty;
 });
      // app.UseHttpsRedirection();
      app.UseCors(MyAllowSpecificOrigins);
      app.UseMvc();
    }
  }
}