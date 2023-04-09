using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using CloudEventData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Net.Http.Headers;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using System.Runtime.CompilerServices;
using Backend.Model;
using Backend.Interfaces;

namespace Backend.API;

[ApiController]
public class Servicing
{
    private readonly Settings _settings;
    private readonly IValidator _validator;
    public Servicing(IConfiguration config, IValidator validator)
    {
        _settings = config.Get<Settings>();
        _validator = validator;
    }

    [FunctionName("GetPatientData")]
    [OpenApiOperation(operationId: "GetPatientData", tags: new[] { "reads" })]
    [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
    [OpenApiParameter(name: "idOfPatient", In = ParameterLocation.Path, Required = true, Type = typeof(string), Description = "the id of the requested price")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/json", bodyType: typeof(JObject), Description = "OK response")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.NotFound, contentType: "text/plain", bodyType: typeof(string), Description = "NOT FOUND response")]
    public async Task<IActionResult> GetPatientData(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "prices/{id}")]
        HttpRequest req, string id)
    {
        if (!_validator.ValidatePatientDataRequest(req, id).Result)
            return new UnauthorizedResult();
        try
        {
            //Get data from database and return
            return new ObjectResult(null);
        }
        catch (Exception ex)
        {
            return new NotFoundObjectResult(ex.GetBaseException().Message);
        }
    }
}
