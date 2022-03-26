using com.dwp.user.location.Model;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json.Serialization;


namespace com.dwp.user.location.api.OpenApiExample
{
    public class CityExample : OpenApiExample<City>
    {
        public override IOpenApiExample<City> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(OpenApiExampleResolver.Resolve("user", new City 
            {
                Name = "Manchester",
                Coordinate = new Coordinate(53.483959, -2.244644)
            }, namingStrategy));

            return this;
        }
    }
}
