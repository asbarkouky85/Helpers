using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;

namespace Generator.Customization
{

    public class DesignTimeServices : IDesignTimeServices
    {
        public void ConfigureDesignTimeServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IPluralizer, Pluralizer>();
            serviceCollection.AddSingleton<ICSharpEntityTypeGenerator, EntityGenerator>();
            serviceCollection.AddSingleton<ICSharpDbContextGenerator, DBContextGenerator>();
        }
    }


}
