using System;

using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Configurations;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;

namespace com.dwp.user.location.api.OpenApiConfiguration
{
    public class OpenApiConfiguration : DefaultOpenApiConfigurationOptions
    {
        public override OpenApiInfo Info { get; set; } = new OpenApiInfo()
        {
            Version = "1.0.0",
            Title = "DWP Location Api",
            Description = "Specification for Location Api",
            TermsOfService = new Uri("https://www.gov.uk/help/terms-conditions"),
            Contact = new OpenApiContact()
            {
                Name = "Department for Work and Pensions",
                Email = "supportDWP@gov.uk",
                Url = new Uri("https://www.gov.uk/government/organisations/department-for-work-pensions"),
            }

        };

        public override OpenApiVersionType OpenApiVersion { get; set; } = OpenApiVersionType.V3;
    }
}
