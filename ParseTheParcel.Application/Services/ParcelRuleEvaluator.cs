using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ParseTheParcel.Application.DTOs;
using ParseTheParcel.Application.Interfaces;
using ParseTheParcel.Application.Models;
using ParseTheParcel.Domain.ValueObjects;

namespace ParseTheParcel.Application.Services
{
    public class ParcelRuleEvaluator(
        IOptions<List<ParcelRule>> options,
        ILogger<ParcelRuleEvaluator> logger) : IParcelRuleEvaluator
    {
        private readonly IEnumerable<ParcelRule> _rules = options.Value;
        private readonly ILogger<ParcelRuleEvaluator> _logger = logger;

        public (string? Type, double? Cost, string? Message) Evaluate(ParcelRequest request)
        {
            _logger.LogInformation("Evaluating parcel request: {@Request}", request);

            var dimensions = new Dimensions(request.Length, request.Breadth, request.Height);
            var failureMessages = new List<string>();

            foreach (var rule in GetOrderedRules())
            {
                if (!IsWeightWithinLimit(request.Weight, rule, out var weightError))
                {
                    LogAndCollect(rule.Type, weightError!, failureMessages); 
                    continue; // skipping dimension check if weight check fails
                }

                if (!IsDimensionWithinLimit(dimensions, rule, out var dimensionError))
                {
                    LogAndCollect(rule.Type, dimensionError!, failureMessages);
                    continue;
                }

                _logger.LogInformation("Matched rule: {RuleType}, cost: {Cost}", rule.Type, rule.Cost);
                return (
                    rule.Type,
                    rule.Cost,
                    $"Requested parcel accepted as '{rule.Type}' package. Shipping cost is ${rule.Cost:F2}."
                );
            }

            _logger.LogWarning("Parcel did not match any rule.");

            var message = failureMessages
                .FirstOrDefault(m => m.StartsWith("Weight"))
                ?? failureMessages.LastOrDefault()
                ?? "Requested parcel does not meet any supported rule conditions.";

            return (null, null, message);
        }

        private IEnumerable<ParcelRule> GetOrderedRules()
        {
            return _rules
                    .OrderBy(r => r.MaxWeight)
                    .ThenBy(r => r.MaxLength)
                    .ThenBy(r => r.MaxBreadth)
                    .ThenBy(r => r.MaxHeight);
        }

        private static bool IsWeightWithinLimit(double weight, ParcelRule rule, out string? error)
        {
            if (weight <= rule.MaxWeight)
            {
                error = null;
                return true;
            }

            error = $"Requested parcel weight {weight}kg exceeds '{rule.Type}' limit ({rule.MaxWeight}kg).";
            return false;
        }

        private static bool IsDimensionWithinLimit(Dimensions dims, ParcelRule rule, out string? error)
        {
            if (dims.Length <= rule.MaxLength &&
                dims.Breadth <= rule.MaxBreadth &&
                dims.Height <= rule.MaxHeight)
            {
                error = null;
                return true;
            }

            error = $"Requested parcel dimensions {dims} exceed '{rule.Type}' limit ({rule.MaxLength}x{rule.MaxBreadth}x{rule.MaxHeight}mm).";
            return false;
        }

        private void LogAndCollect(string ruleType, string error, List<string> collection)
        {
            _logger.LogDebug("Rule {RuleType} check failed: {Error}", ruleType, error);
            collection.Add(error);
        }
    }
}
