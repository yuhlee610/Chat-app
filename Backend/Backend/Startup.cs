using Backend.Configuration;
using Backend.Data;
using Backend.GraphQL.Groups;
using Backend.GraphQL.Messages;
using Backend.GraphQL.Users;
using Backend.IRepository;
using Backend.Repository;
using GraphQL.Server.Ui.Voyager;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins",
                    builder => 
                        builder.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader().AllowAnyMethod());
            });

            services.AddPooledDbContextFactory<ApplicationDbContext>(opt =>
                opt.UseSqlServer(_configuration.GetConnectionString("MyConnection")));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = "audience",
                    ValidIssuer = "issuer",
                    RequireSignedTokens = false,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secretsecretsecret"))
                };

                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
            });

            services
                .AddGraphQLServer()
                .AddAuthorization()
                .AddQueryType(d => d.Name("Query"))
                    .AddTypeExtension<UserQuery>()
                    .AddTypeExtension<GroupQuery>()
                    .AddTypeExtension<MessageQuery>()
                .AddMutationType(d => d.Name("Mutation"))
                    .AddTypeExtension<UserMutation>()
                    .AddTypeExtension<GroupMutation>()
                    .AddTypeExtension<MessageMutation>()
                .AddSubscriptionType(d => d.Name("Subscription"))
                    .AddTypeExtension<GroupSubscription>()
                .AddFiltering()
                .AddInMemorySubscriptions();

            services.AddAutoMapper(typeof(MapperInitializer));
            services.AddScoped<IIdentityRepository, IdentityRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowSpecificOrigins");

            app.UseRouting();

            app.UseWebSockets();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });

            // add Voyager - a design allow see relationship between schema
            app.UseGraphQLVoyager(new VoyagerOptions()
            {
                GraphQLEndPoint = "/graphql",
            }, "/graphql-voyager");
        }
    }
}
