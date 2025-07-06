using Microsoft.Extensions.Options;
using ParseTheParcel.Application.Interfaces;
using ParseTheParcel.Application.Models;


namespace ParseTheParcel.Infrastructure.Providers
{
    public class ConfigParcelRuleProvider(IOptions<List<ParcelRule>> options) : IParcelRuleProvider
    {
        private readonly List<ParcelRule> _rules = options.Value;

        public IReadOnlyList<ParcelRule> GetRules() => _rules; 
    }
}
