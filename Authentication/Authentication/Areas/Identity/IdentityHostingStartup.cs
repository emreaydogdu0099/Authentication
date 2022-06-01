using System;
using Authentication.Areas.Identity.Data;
using Authentication.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Authentication.Areas.Identity.IdentityHostingStartup))]
namespace Authentication.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<AuthenticationDbContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("AuthenticationDbContextConnection")));

                services.AddDefaultIdentity<AuthenticationUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                })
                    .AddEntityFrameworkStores<AuthenticationDbContext>();
            });
        }
    }
}