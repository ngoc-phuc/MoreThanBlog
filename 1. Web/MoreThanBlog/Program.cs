using System;
using Abstraction.Service.UserService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Repository;

namespace MoreThanBlog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<Repository.DbContext>();
                context.Database.Migrate();
                var userService = scope.ServiceProvider.GetService<IUserService>();
                userService.InitAdminAccountAsync().Wait();
                context.Dispose();

                try
                {
                    host.Run();
                }
                catch (Exception ex)
                {

                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}