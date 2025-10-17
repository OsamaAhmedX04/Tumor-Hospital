using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TumorHospital.WebAPI.Extensions
{
    public static class ModelStateExtensions
    {
        public static Dictionary<string, string[]> ToErrorResponse(this ModelStateDictionary modelState)
            => modelState
                .Where(m => m.Value.Errors.Any())
                .ToDictionary(
                    m => m.Key,
                    m => m.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );
    }
}
