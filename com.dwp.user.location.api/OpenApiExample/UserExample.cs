

using com.dwp.user.location.Model;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Resolvers;
using Newtonsoft.Json.Serialization;

namespace com.dwp.user.location.api.OpenApiExample
{
    public class UserExample : OpenApiExample<User>
    {
        public override IOpenApiExample<User> Build(NamingStrategy namingStrategy = null)
        {
            this.Examples.Add(OpenApiExampleResolver.Resolve("user", new User
            {
                Id = 1,
                Email = "mb1@test.com",
                FirstName = "John",
                LastName = "Snow",
                Latitude = 51.6710832,
                Longitude = 0.8078532,
                IpAddress = "113.71.242.187"
            }, namingStrategy));

            return this;
        }
    }

}
