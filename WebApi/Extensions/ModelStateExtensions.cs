using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApi.Models;

namespace WebApi.Extensions
{
    public static class ModelStateExtensions
    {
        public static ErroModel ToErroModel(this ModelStateDictionary modelState, HttpContext httpContext = null)
        {
            var erros = modelState
                .Where(ms => ms.Value.Errors.Any())
                .ToDictionary(
                    ms => ms.Key,
                    ms => ms.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            var ErroModel = new ErroModel
            {
                Status = StatusCodes.Status400BadRequest,
                Titulo = "Um ou mais erros de validação ocorreram.",
                Erros = erros,
                TraceId = httpContext?.TraceIdentifier,
                
            };

            return ErroModel;
        }
    }
}
