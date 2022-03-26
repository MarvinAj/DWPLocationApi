using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using com.dwp.user.location.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace com.dwp.user.location.api
{
    public class DWPLocationServiceApi
    {

        [FunctionName("UsersWithin50MilesOfLondon")]
        [OpenApiOperation(operationId: "GetUsersWithin50MilesOfLondon", tags: new[] { "DWP Location Service" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<User>), Description = "Success")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "Could not find any users")]
        public static async Task<IActionResult> GetUsersWithin50MilesOfLondon(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "city/London/users/within50milesRadius")] HttpRequest req,
            ILogger logger)
        {
            return await req.RunMethod(async loc => await loc.GetUsersAsync(50, "london"), logger);
        }

        [FunctionName("UsersWithinCityRadius")]
        [OpenApiOperation(operationId: "UsersWithinCityRadius", tags: new[] { "DWP Location Service" })]
        [OpenApiParameter(name: "city", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "Name of city ")]
        [OpenApiParameter(name: "radius", In = ParameterLocation.Query, Required = true, Type = typeof(double), Description = "Radius in miles")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<User>), Description = "Success")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "Could not find any users within given radius")]
        public static async Task<IActionResult> GetUsersWithinCityRadius(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "city/{city}/users")]
            HttpRequest req,
            string city,
            ILogger logger)
        {
            return await req.RunMethod(async loc => await loc.GetUsersAsync(req.GetRadius(), city), logger);
        }

        [FunctionName("GetCity")]
        [OpenApiOperation(operationId: "GetCity", tags: new[] { "DWP Location Service" })]
        [OpenApiParameter(name: "city", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "Name of city ")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(City), Description = "Success")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "Could not find city")]
        public static async Task<IActionResult> GetCity(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "cities/{city}")]
            HttpRequest req,
            string city,
            ILogger logger)
        {
            return await req.RunMethod(async loc => await loc.GetCityAsync(city), logger);
        }

        [FunctionName("GetCities")]
        [OpenApiOperation(operationId: "GetCities", tags: new[] { "DWP Location Service" })]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(City), Description = "Success")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.NotFound, Description = "Could not find any city")]
        public static async Task<IActionResult> GetCities(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "cities")]
            HttpRequest req,
            ILogger logger)
        {
            return await req.RunMethod(async loc => await loc.GetCitiesAsync(), logger);
        }

    }
}

