using CommanderGQL.Data;
using CommanderGQL.GraphQL;
using CommanderGQL.GraphQL.Commands;
using CommanderGQL.GraphQL.Platforms;
using GraphQL.Server.Ui.Voyager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CommanderGQL
{
    public class Startup
    {
        private readonly IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddPooledDbContextFactory<AppDbContext>(cfg => cfg.UseSqlServer(Configuration.GetConnectionString("SqlConnection")));

            services
            .AddGraphQLServer() // Adds GraphQL Server
            .AddQueryType<Query>() // Adds graphql Query object
            .AddMutationType<Mutation>() // Adds data mutation object
            .AddSubscriptionType<Subscription>() // adds subscribers to the service
            .AddType<PlatformType>()
            .AddType<CommandType>()
            .AddFiltering()
            .AddSorting()
            .AddInMemorySubscriptions(); // manage and track subscribers in memeory - NOTE: if in production, should add persistance layer instead of in memory.
            //.AddProjections(); // Uses UseProjections attribute ""Comment out because we're using resolvers""
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Adds subscription sockets to the request pipeline for notifications
            app.UseWebSockets();

            app.UseRouting();

            // Request Pipeline
            app.UseEndpoints(endpoints =>
            {
                // Map GraphQL to the pipeline
                endpoints.MapGraphQL();
            });

            app.UseGraphQLVoyager(new GraphQLVoyagerOptions()
            {
                GraphQLEndPoint = "/graphql",
                Path = "/graphql-voyager"
            });
        }
    }
}
