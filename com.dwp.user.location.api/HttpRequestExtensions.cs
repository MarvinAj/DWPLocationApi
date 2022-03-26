using com.dwp.user.location.Exceptions;
using com.dwp.user.location.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;

namespace com.dwp.user.location.api
{
    public static class HttpRequestExtensions
    {
        public static double GetRadius(this HttpRequest request)
        {
            if (request.Query.TryGetValue("radius", out var value))
            {
                if(double.TryParse(value, out var radius))
                {
                    return radius;
                }
                else
                {
                    throw new ArgumentException("Incorrect query parameter 'radius'");
                }
            }

            throw new ArgumentException("Missing query parameter 'radius'");
        }

        public static async Task<T> GetValidPostObject<T>(this HttpRequest req)
        {
            T content = default;

            if (!new[] { "post", "patch", "put", "delete" }.Any(
                m => req.Method.Equals(m, StringComparison.InvariantCultureIgnoreCase)))
            {
                return content;
            }

            if (req.Body != null)
            {
                string body = await new StreamReader(req.Body).ReadToEndAsync();

                try
                {
                    content = JsonSerializer.Deserialize<T>(body);
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(ex.Message);
                }

                var results = new List<ValidationResult>();
                var validationContext = new ValidationContext(content);
                if (!Validator.TryValidateObject(content, validationContext, results))
                {
                    throw new ArgumentException(string.Join("\n", results.Select(x => x.ErrorMessage)));
                }
            }

            return content;
        }

        public static async Task<IActionResult> RunMethod<S, T>(
        this HttpRequest req,
        Func<ILocationService, S, Task<T>> method,
        ILogger logger,
        [CallerMemberName] string methodName = null)
        {
            try
            {
                S content = await req.GetValidPostObject<S>();

                ILocationService locationService = req.HttpContext.RequestServices
                    .GetRequiredService<ILocationService>();

                T result = await method.Invoke(locationService, content);
                if (result != null)
                {
                    return new OkObjectResult(result);
                }

                return new OkResult();
            }
            catch(UsersNotFoundException ulex)
            {
                logger.LogError(ulex,$"{methodName} throw an exception");
                return new NotFoundObjectResult(ulex.Message);
            }
            catch (CityNotFoundException cex)
            {
                logger.LogError(cex, $"{methodName} throw an exception");
                return new NotFoundObjectResult(cex.Message);
            }
            catch (ArgumentException aex)
            {
                logger.LogError(aex, $"{methodName} throw an exception.");
                return new BadRequestObjectResult(aex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{methodName} throw an exception.");               
                return new ObjectResult(ex.Message)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                };
            }
        }

        public static async Task<IActionResult> RunMethod<T>(
            this HttpRequest req,
            Func<ILocationService, Task<T>> method,
            ILogger logger,
            [CallerMemberName] string methodName = null)
        {
            return await req.RunMethod<T, object>(
                async (loc, _) => await method.Invoke(loc),
                logger,
                methodName);
        }


    }
}
