using System.Collections.Generic;
using MakiseSharpServer.API.Results;
using Microsoft.AspNetCore.Mvc.Formatters;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MakiseSharpServer.API.Swagger.Filters
{
    /// <summary>
    /// Replaces MessageResult junk schema with the correct one (just one string message property)
    /// </summary>
    public class MessageResultSchemaOverrideDocumentFilter : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            var schema = new Schema { Type = "object" };

            var messageSchema = new Schema
            {
                Type = "string",
                ReadOnly = true
            };
            schema.Properties = new Dictionary<string, Schema> { { "message", messageSchema } };
            swaggerDoc.Definitions[nameof(MessageResult)] = schema;

            swaggerDoc.Definitions.Remove(nameof(IOutputFormatter)); //left after the old MessageResult schema
        }
    }
}
