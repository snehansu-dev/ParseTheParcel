using ParseTheParcel.Application.DTOs;
using ParseTheParcel.Application.Interfaces;
using ParseTheParcel.Application.Validators;
using Microsoft.Extensions.Logging;

namespace ParseTheParcel.Application.Services
{
    public class ParcelService(IParcelRuleEvaluator evaluator, ILogger<ParcelService> logger) : IParcelService
    {
        private readonly IParcelRuleEvaluator _evaluator = evaluator;
        private readonly ILogger<ParcelService> _logger = logger;

        public (string? Type, double? Cost, string? Message) GetParcelTypeAndCost(ParcelRequest request)
        {
            _logger.LogInformation("Evaluating parcel: {@Request}", request);

            var validationError = ParcelRequestValidator.Validate(request);
            if (validationError != null)
            {
                _logger.LogWarning("Validation failed: {Error}", validationError);
                return (null, null, validationError);
            }

            var result = _evaluator.Evaluate(request);

            if (result.Type != null)
                _logger.LogInformation("Parcel matched rule: {Type}, Cost: {Cost}", result.Type, result.Cost);
            else
                _logger.LogWarning("Parcel evaluation failed: {Message}", result.Message);

            return result;
        }
    }
}
