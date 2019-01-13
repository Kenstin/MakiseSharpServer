using System.Linq;
using System.Reflection;
using MakiseSharpServer.API.Results;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MakiseSharpServer.API.Swagger.Filters
{
    /// <summary>
    /// When an operation is decorated with <see cref="SwaggerResponseAttribute"/> and has a
    /// description, the description will be used as an example response for MessageResult json response
    /// </summary>
    public class MessageResultExampleOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var attributes = context.MethodInfo.GetCustomAttributes().OfType<SwaggerResponseAttribute>();
            foreach (var attribute in attributes)
            {
                if (attribute.StatusCode >= 400 && attribute.Description != null)
                {
                    dynamic message = new JObject();
                    message.message = attribute.Description;
                    var json = new JObject(new JProperty("application/json", message));
                    operation.Responses[attribute.StatusCode.ToString()].Examples = json;

                    operation.Responses[attribute.StatusCode.ToString()].Schema =
                        context.SchemaRegistry.GetOrRegister(typeof(MessageResult));
                }
            }
        }
    }
}
