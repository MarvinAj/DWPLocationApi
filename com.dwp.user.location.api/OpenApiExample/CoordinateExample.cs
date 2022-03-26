
using com.dwp.user.location.Model;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json.Serialization;

namespace com.dwp.user.location.api.OpenApiExample
{
    public class CoordinateExample : OpenApiExample<Coordinate>
    {
        public override IOpenApiExample<Coordinate> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(OpenApiExampleResolver.Resolve("cover", new Coordinate(52.205276, 0.119167), namingStrategy));

            return this;
        }
    }
}
