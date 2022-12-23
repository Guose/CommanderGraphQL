using System.Linq;
using CommanderGQL.Data;
using CommanderGQL.Model;
using HotChocolate;
using HotChocolate.Data;

namespace CommanderGQL.GraphQL
{
    public class Query
    {
        [UseDbContext(typeof(AppDbContext))]
        [UseFiltering]
        [UseSorting]
        //[UseProjection] // project forward and pull back any child objects ""Comment out because we're using resolvers""
        public IQueryable<Platform> GetPlatform([ScopedService] AppDbContext context) => context.Platforms;

        [UseDbContext(typeof(AppDbContext))]
        [UseFiltering]
        [UseSorting]
        //[UseProjection]  ""Comment out because we're using resolvers""
        public IQueryable<Command> GetCommand([ScopedService] AppDbContext context) => context.Commands;
    }
}