using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MakiseSharpServer.API.Swagger.Filters
{
    /// <summary>
    /// Sets all operations' response types to application/json
    /// </summary>
    public class JsonResponseOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            operation.Produces.Clear();
            operation.Produces.Add("application/json");
        }
    }
}
